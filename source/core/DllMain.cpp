/**
 * Copyright (C) 2015 crosire & kagikn & contributors
 * License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
 */

#pragma managed(push, off)

#include <Windows.h>
#include <atomic>
#include <mutex>

LPVOID sTlsContextAddrOfGameMainThread = nullptr;
DWORD sGameMainThreadId = 0;
std::mutex sGameMainThreadVarsMutex;
std::atomic_bool sGameMainThreadVarsInitialized(false);

std::atomic_bool sScriptDomainRequestedToReload(false);

static void SetTlsContext(LPVOID context)
{
    __writegsqword(0x58, reinterpret_cast<DWORD64>(context));
}
static LPVOID GetTlsContext()
{
    return reinterpret_cast<LPVOID>(__readgsqword(0x58));
}

static LPVOID GetTlsContextAddrOfGameMainThread()
{
    {
        // lock would take too short time for shared lock to be worth it, use exclusive lock
        std::lock_guard<std::mutex> lock(sGameMainThreadVarsMutex);
        return sTlsContextAddrOfGameMainThread;
    }
}
static DWORD GetGameMainThreadId()
{
    {
        std::lock_guard<std::mutex> lock(sGameMainThreadVarsMutex);
        return sGameMainThreadId;
    }
}

#pragma managed(pop)

// Import C# code base
#using "ScriptHookVDotNet.netmodule"

static void InitializeScriptRuntime()
{
    SHVDN::ScriptHookVDotNet::Init(
        System::IntPtr(reinterpret_cast<void*>(&GetTlsContext)),
        System::IntPtr(reinterpret_cast<void*>(&SetTlsContext)),
        System::IntPtr(GetTlsContextAddrOfGameMainThread()),
        GetGameMainThreadId());
}

static bool DidScriptRuntimeRequestReload()
{
    const bool managedReloadRequested = SHVDN::SharedVariables::DidRequestReloadResetFlag;
    return sScriptDomainRequestedToReload.exchange(false, std::memory_order_acq_rel) || managedReloadRequested;
}

static void TickScriptRuntime()
{
    SHVDN::ScriptHookVDotNet::Tick();
}

static void DispatchScriptKeyboardMessage(DWORD key, bool keydown, bool ctrl, bool shift, bool alt)
{
    SHVDN::ScriptHookVDotNet::KeyboardMessage(key, keydown, ctrl, shift, alt);
}
// This is needed to match `_tls_index` of .NET CLR dlls, which are loaded by the OS loader (if we understand
// correctly), in between our .NET threads and the game's main logic thread. If they mismatch, we would have to change
// those in our .NET threads to that in for the game's main logic thread to avoid the game crashing every time we do
// something that needs game's thread local variables, which is more tedious.
static int EmptyClrMethodForEagerClrDllLoading()
{
    return 1;
}

#pragma unmanaged

#include <Main.h>

std::atomic<HANDLE> hClrThread;
std::atomic<HANDLE> hClrWaitEvent{ nullptr };
std::atomic<HANDLE> hClrContinueEvent{ nullptr };

std::atomic_bool sClrEventsInitialized(false);
// synchronization with event objects would cause a deadlock or timeout (for executing longer than 2 seconds)
// in DllMain, so use a bool variable to tell procedures that the asi wants to get freed
// use `atomic_bool` to avoid unnecessary optimization
std::atomic_bool sClrThreadRequestedToExit(false);

int sTempValForEagerClrDllLoading = 0;

// A procedure that is supposed to be run in a dedicated thread for so a cached stack limit in .NET runtime won't panic for
// (false) stack overflow that can be caused by running managed code in a custom fiber (with a custom fiber data in other words)
static DWORD ClrThreadProc(LPVOID lparam)
{
    // Eagerly load CLR DLLs, so the game won't crash when we try to swap TLS address by making `_tls_index` of .NET
    // CLR dlls match in between our .NET threads and the game's main logic thread.
    sTempValForEagerClrDllLoading = EmptyClrMethodForEagerClrDllLoading();

    while (!sClrEventsInitialized.load(std::memory_order_acquire)) {
        // We want to give up our time slice of this thread but don't want to busy wait at all if not the all events
        // are initialized, so sleep at least 1 tick per test
        Sleep(1);
    }

    // DllMain gets called before ScriptHookV starts script fibers, so wait until getting signaled by ScriptMain
    WaitForSingleObject(hClrContinueEvent.load(std::memory_order_relaxed), INFINITE);

    while (!sClrThreadRequestedToExit.load(std::memory_order_relaxed)) {
        InitializeScriptRuntime();

        // ScriptHookV's script fibers will be executed in the main thread, so we assume this code won't have trouble
        // exiting the loop after letting the main fiber execute
        while (!DidScriptRuntimeRequestReload() && !sClrThreadRequestedToExit.load(std::memory_order_relaxed)) {
            TickScriptRuntime();

            SetEvent(hClrWaitEvent.load(std::memory_order_relaxed));
            WaitForSingleObject(hClrContinueEvent.load(std::memory_order_relaxed), INFINITE);
        }
    }

    return 0;
}

// solely for detection for recreation of the game session
PVOID sOldGameFiber = nullptr;

static void ScriptMain()
{
    // We need these info to swap the TLS context of the main thread when necessary to access a lot of game stuff
    // such as a rage::SysMemAllocator instance
    if (!sGameMainThreadVarsInitialized.load(std::memory_order_acquire))
    {
        std::lock_guard<std::mutex> lock(sGameMainThreadVarsMutex);
        sTlsContextAddrOfGameMainThread = GetTlsContext();
        sGameMainThreadId = GetCurrentThreadId();
        sGameMainThreadVarsInitialized.store(true, std::memory_order_release);
    }

    // ScriptHookV already turned the current thread into a fiber, so can safely retrieve it
    const PVOID initialGameFiber = GetCurrentFiber();
    if (sOldGameFiber != nullptr)
    {
        // The game session is reloaded, so tell our script domain to reload from this fiber
        sScriptDomainRequestedToReload.store(true, std::memory_order_release);
    }
    sOldGameFiber = initialGameFiber;

    bool gameReloaded = false;
    while (!sClrThreadRequestedToExit.load(std::memory_order_acquire) && !gameReloaded)
    {
        // Break from the loop if ScriptHookV reloads scripts when the game creates a new session (because the old fiber is being disposed)
        // Script fibers gets executed at least once after a scriptWait call even after the new game session is reloaded,
        // so we should leave here without interfering anything in this fiber if the game is reloaded
        // ScriptHookV creates a new fiber only right after a "Started thread" message is written to the log
        const PVOID currentFiber = GetCurrentFiber();
        if (currentFiber != initialGameFiber)
        {
            gameReloaded = true;
            break;
        }

        SetEvent(hClrContinueEvent.load(std::memory_order_relaxed));
        // This call blocks the main thread so GtaThread instances (which ysc or external scripts internally rely on)
        // won't be executed except for one for the SHVDN runtime as long as it is executing
        WaitForSingleObject(hClrWaitEvent.load(std::memory_order_relaxed), INFINITE);
        scriptWait(0);
    }
}

// Keyboard event runs in a thread different from where tick event runs in Script Hook V, so we may want to synchronize
// data between them!
static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
    // Too dangerous to access the key events of our domains before the TLS address of the main thread is initialized,
    // as they may want info about the main thread TLS.
    if (!sGameMainThreadVarsInitialized.load(std::memory_order_acquire))
    {
        return;
    }

    DispatchScriptKeyboardMessage(
        key,
        !isUpNow,
        (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0,
        (GetAsyncKeyState(VK_SHIFT  ) & 0x8000) != 0,
        isWithAlt != FALSE);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpvReserved)
{
    switch (fdwReason)
    {
    case DLL_PROCESS_ATTACH:
        hClrContinueEvent.store(CreateEvent(NULL, false, false, NULL), std::memory_order_relaxed);
        hClrWaitEvent.store(CreateEvent(NULL, false, false, NULL), std::memory_order_relaxed);
        sClrEventsInitialized.store(true, std::memory_order_release);

        // Avoid unnecessary DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications
        DisableThreadLibraryCalls(hModule);
        // Register ScriptHookVDotNet native script
        scriptRegister(hModule, ScriptMain);
        // Register handler for keyboard messages
        keyboardHandlerRegister(ScriptKeyboardMessage);

        // Create a separate thread to run CLR so in .NET runtime won't panic for (false) stack overflow by a cached stack
        // limit in .NET runtime
        // ScriptHookV runs script fibers in the main thread, but our managed code needs to run in without a fiber data
        // to avoid (false) stack overflow exceptions
        // The issue that explains usage of fibers causes random .NET runtime crashes when the VEH of .NET runtime is called for
        // .NET exceptions or regular C++ exceptions: https://github.com/scripthookvdotnet/scripthookvdotnet/issues/976
        // The place that may cause the exception: https://github.com/dotnet/coreclr/blob/ef1e2ab328087c61a6878c1e84f4fc5d710aebce/src/vm/excep.cpp#L7763
        // The code for stack space detection: https://github.com/dotnet/coreclr/blob/ef1e2ab328087c61a6878c1e84f4fc5d710aebce/src/vm/threads.cpp#L7912
        hClrThread.store(CreateThread(NULL, NULL, ClrThreadProc, NULL, NULL, NULL), std::memory_order_release);
        break;
    case DLL_PROCESS_DETACH:
        sClrThreadRequestedToExit.store(true, std::memory_order_relaxed);

        // Gracefully let the CLR thread procedure and the script fiber exit (CloseHandle does not set events)
        // Probably these calls do not wake up ClrThreadProc however
        SetEvent(hClrContinueEvent.load(std::memory_order_relaxed));
        SetEvent(hClrWaitEvent.load(std::memory_order_relaxed));

        // Cleanup events and thread handles so the exe can exit
        CloseHandle(hClrContinueEvent.load(std::memory_order_relaxed));
        CloseHandle(hClrWaitEvent.load(std::memory_order_relaxed));
        CloseHandle(hClrThread.load(std::memory_order_relaxed));
        // Unregister ScriptHookVDotNet native script
        scriptUnregister(hModule);
        // Unregister handler for keyboard messages
        keyboardHandlerUnregister(ScriptKeyboardMessage);
        break;
    }

    return TRUE;
}
