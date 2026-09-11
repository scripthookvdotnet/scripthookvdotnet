/**
 * Copyright (C) 2015 crosire & kagikn & contributors
 * License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
 */

#pragma managed(push, off)

#include <Windows.h>
#include <atomic>
#include <mutex>

// The hosted .NET Framework CLR can end up with invalid per-culture NLS sort handles (seen on GTA V
// b1.0.3889.0 under Windows 11 25H2), so culture-sensitive String.ToLower/ToUpper fail with E_INVALIDARG
// (via TextInfo.InternalChangeCaseString -> LCMapStringEx). That breaks AppDomain.CreateDomain (its remoting
// Identity type initializer lower-cases a string) and prevents SHVDN from starting. The same call without
// the sort handle and the LCMAP_LINGUISTIC_CASING flag succeeds, so we hook LCMapStringEx and retry failed
// case-mappings that way. The CLR resolves the function dynamically, so we patch the function itself rather
// than an import table entry. See https://github.com/scripthookvdotnet/scripthookvdotnet/issues/1805
typedef int (WINAPI *LCMapStringExFunc)(LPCWSTR, DWORD, LPCWSTR, int, LPWSTR, int, LPNLSVERSIONINFO, LPVOID, LPARAM);
static LCMapStringExFunc sRealLCMapStringEx = nullptr;
static std::atomic_bool sLCMapStringExHookInstalled(false);

static int WINAPI HookedLCMapStringEx(LPCWSTR locale, DWORD flags, LPCWSTR src, int cchSrc,
    LPWSTR dst, int cchDst, LPNLSVERSIONINFO ver, LPVOID reserved, LPARAM sortHandle)
{
    int result = sRealLCMapStringEx(locale, flags, src, cchSrc, dst, cchDst, ver, reserved, sortHandle);
    if (result != 0 || (flags & (LCMAP_LOWERCASE | LCMAP_UPPERCASE)) == 0)
        return result; // succeeded, or not a case-mapping call we need to repair

    // Retry with the known-good parameters: no linguistic casing, no version info, no sort handle.
    LPCWSTR retryLocale = (locale != nullptr && locale[0] != L'\0') ? locale : LOCALE_NAME_INVARIANT;
    DWORD retryFlags = flags & ~static_cast<DWORD>(LCMAP_LINGUISTIC_CASING);
    result = sRealLCMapStringEx(retryLocale, retryFlags, src, cchSrc, dst, cchDst, nullptr, nullptr, 0);
    if (result != 0)
        return result;
    return sRealLCMapStringEx(LOCALE_NAME_INVARIANT, retryFlags, src, cchSrc, dst, cchDst, nullptr, nullptr, 0);
}

static void InstallLCMapStringExHook()
{
    if (sLCMapStringExHookInstalled.exchange(true))
        return;

    HMODULE kernelBase = GetModuleHandleW(L"kernelbase.dll");
    if (kernelBase == nullptr)
        return;
    unsigned char* target = reinterpret_cast<unsigned char*>(GetProcAddress(kernelBase, "LCMapStringEx"));
    if (target == nullptr)
        return;

    // Only hook if the prologue matches the known 15-byte, position-independent sequence, so relocating it
    // into the trampoline is safe. Otherwise leave the function untouched (the workaround simply no-ops).
    //   48 89 5C 24 08  mov [rsp+8],rbx
    //   48 89 74 24 10  mov [rsp+10h],rsi
    //   57              push rdi
    //   48 83 EC 50     sub rsp,50h
    static const unsigned char expectedPrologue[15] = {
        0x48, 0x89, 0x5C, 0x24, 0x08, 0x48, 0x89, 0x74, 0x24, 0x10, 0x57, 0x48, 0x83, 0xEC, 0x50 };
    for (int i = 0; i < 15; i++)
        if (target[i] != expectedPrologue[i])
            return;

    const int relocSize = 15;
    unsigned char* trampoline = reinterpret_cast<unsigned char*>(
        VirtualAlloc(nullptr, 64, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE));
    if (trampoline == nullptr)
        return;

    // trampoline = [original 15 bytes] + [absolute jump back to target + 15]
    for (int i = 0; i < relocSize; i++)
        trampoline[i] = target[i];
    trampoline[relocSize + 0] = 0xFF; trampoline[relocSize + 1] = 0x25; // jmp qword ptr [rip+0]
    trampoline[relocSize + 2] = 0x00; trampoline[relocSize + 3] = 0x00;
    trampoline[relocSize + 4] = 0x00; trampoline[relocSize + 5] = 0x00;
    *reinterpret_cast<unsigned long long*>(trampoline + relocSize + 6) =
        reinterpret_cast<unsigned long long>(target + relocSize);
    FlushInstructionCache(GetCurrentProcess(), trampoline, 64);
    sRealLCMapStringEx = reinterpret_cast<LCMapStringExFunc>(trampoline);

    // Patch the target with an absolute jump to our hook (14 bytes, padded to 15 with a nop). This runs
    // during early managed init, before SHVDN starts any script threads, so the brief window in which the
    // prologue is half-written is not worth suspending threads over.
    DWORD oldProtect = 0;
    if (!VirtualProtect(target, relocSize, PAGE_EXECUTE_READWRITE, &oldProtect))
        return;
    unsigned char patch[15];
    patch[0] = 0xFF; patch[1] = 0x25; patch[2] = 0x00; patch[3] = 0x00; patch[4] = 0x00; patch[5] = 0x00;
    *reinterpret_cast<unsigned long long*>(patch + 6) =
        reinterpret_cast<unsigned long long>(reinterpret_cast<void*>(&HookedLCMapStringEx));
    patch[14] = 0x90;
    for (int i = 0; i < relocSize; i++)
        target[i] = patch[i];
    VirtualProtect(target, relocSize, oldProtect, &oldProtect);
    FlushInstructionCache(GetCurrentProcess(), target, relocSize);
}

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

static void RequestScriptDomainToReload()
{
    sScriptDomainRequestedToReload.store(true, std::memory_order_release);
}

#pragma managed(pop)

// Import C# code base
#include <msclr\lock.h>
#using "ScriptHookVDotNet.netmodule"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

[FlagsAttribute]
private enum class ModifierKeyState
{
    None = 0,
    Shift = 0x10000,
    Control = 0x20000,
    Alt = 0x40000,
};

public ref class ScriptHookVDotNet // This is not a static class, so that console scripts can inherit from it for ConsoleInput class
{
public:
    [SHVDN::ConsoleCommand("Print the default help")]
    static void Help()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Help");
            return;
        }

        console->PrintInfo("~c~--- Help ---");
        console->PrintInfo("The console accepts ~h~C# expressions~h~ as input and has full access to the scripting API. To print the result of an expression, simply add \"return\" in front of it.");
        console->PrintInfo("You can use \"P\" as a shortcut for the player character and \"V\" for the current vehicle (without the quotes).");
        console->PrintInfo("Example: \"return P.IsInVehicle()\" will print a boolean value indicating whether the player is currently sitting in a vehicle to the console.");
        console->PrintInfo("~c~--- Commands ---");
        console->PrintHelpText();
    }
    [SHVDN::ConsoleCommand("Print the help for a specific command")]
    static void Help(String ^command)
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Help");
            return;
        }

        console->PrintHelpText(command);
    }

    [SHVDN::ConsoleCommand("Clear the console history and pages")]
    static void Clear()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Clear");
            return;
        }

        console->Clear();
    }

    [SHVDN::ConsoleCommand("Reload all scripts from the scripts directory")]
    static void Reload()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Reload");
            return;
        }

        console->PrintInfo("~y~Reloading ...");

        // Force a reload on next tick
        RequestScriptDomainToReload();
    }

    [SHVDN::ConsoleCommand("Load scripts from a file")]
    static void Start(String ^filename)
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Start");
            return;
        }

        if (!IO::Path::IsPathRooted(filename))
            filename = IO::Path::Combine(domain->ScriptPath, filename);
        if (!IO::Path::HasExtension(filename))
            filename += ".dll";

        String ^ext = IO::Path::GetExtension(filename)->ToLower();
        if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
            console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
            return;
        }

        domain->StartScripts(filename);
    }
    [SHVDN::ConsoleCommand("Load all scripts in the scripts folder")]
    static void StartAllScripts()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("StartAllScripts");
            return;
        }

        console->PrintInfo("~y~Loading all scripts ...");
        domain->Start();
    }
    [SHVDN::ConsoleCommand("Abort all scripts from a file")]
    static void Abort(String ^filename)
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("Abort");
            return;
        }

        if (!IO::Path::IsPathRooted(filename))
            filename = IO::Path::Combine(domain->ScriptPath, filename);
        if (!IO::Path::HasExtension(filename))
            filename += ".dll";

        String ^ext = IO::Path::GetExtension(filename)->ToLower();
        if (!IO::File::Exists(filename) || (ext != ".cs" && ext != ".vb" && ext != ".dll")) {
            console->PrintError(IO::Path::GetFileName(filename) + " is not a script file!");
            return;
        }

        domain->AbortScripts(filename);
    }
    [SHVDN::ConsoleCommand("Abort all scripts currently running")]
    static void AbortAll()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("AbortAll");
            return;
        }

        domain->Abort();

        console->PrintInfo("Stopped all running scripts. Use \"Start(filename)\" to start them again.");
    }

    [SHVDN::ConsoleCommand("List all loaded scripts")]
    static void ListScripts()
    {
        SHVDN::Console^ console = GetConsole();
        if (console == nullptr)
        {
            WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand("ListScripts");
            return;
        }

        console->PrintInfo("~c~--- Loaded Scripts ---");
        for each (auto script in domain->RunningScripts)
            console->PrintInfo(IO::Path::GetFileName(script->Filename) + " ~h~" + script->Name + (script->IsRunning ? (script->IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
    }

internal:
    static SHVDN::Console^ console = nullptr;
    static SHVDN::ScriptDomain ^domain = SHVDN::ScriptDomain::CurrentDomain;
    static array<WinForms::Keys>^ reloadKeyBinding = { WinForms::Keys::None };
    static array<WinForms::Keys>^ consoleKeyBinding = { WinForms::Keys::F4 };
    static unsigned int scriptTimeoutThreshold = 5000;
    static bool AutoLoadScripts = true;

    // We use this domain to prevent from the keyboard thread reading stale values, and to protect against race
    // condition during reload. Do note that static variables are not shared between `AppDomain`s.
    static Object^ variablesLockForMainDomain = gcnew Object();
    static Object^ variablesLockForScriptDomain = gcnew Object();
    static array<bool>^ _keyboardStatePool = gcnew array<bool>(256);
    static ModifierKeyState _modKeyState = ModifierKeyState::None;

    value struct LogMessageInfo
    {
        LogMessageInfo(SHVDN::Log::Level level, String^ message)
        {
            this->level = level;
            this->message = message;
        }

        SHVDN::Log::Level level;
        String^ message;
    };

    // Although static variables are not shared between `AppDomain`s, we need to use a lock because the keyboard
    // message thread may want to request the script domain to instant reload.
    static SHVDN::Console^ GetConsole()
    {
        msclr::lock l(ScriptHookVDotNet::variablesLockForScriptDomain);
        return console;
    }
    static void SetConsole()
    {
        msclr::lock l(ScriptHookVDotNet::variablesLockForScriptDomain);
        console = (SHVDN::Console^)AppDomain::CurrentDomain->GetData("Console");
    }

    static void WriteErrorMessageForConsoleNotLoadedWhenExecutingCommand(String^ commandName)
    {
        SHVDN::Log::WriteToFile(SHVDN::Log::Level::Error,
            String::Format(
                "Could not execute the console command \"{0}\". The console is not loaded. " +
                "You could report this error to the ScriptHookVDotNet's GitHub repository as the error can be " +
                "responsible to ScriptHookVDotNet's implementation.",
                commandName
            )
        );
    }

    static void SendPendingMessagesToConsole(SHVDN::Console^ console, List<LogMessageInfo>^ pendingMessageInfo)
    {
        if (console == nullptr)
        {
            return;
        }

        for each (LogMessageInfo messageInfo in pendingMessageInfo)
        {
            // Don't use Log.WriteToConsole here, we want to make sure that log strings will print via
            // the passed console instance
            switch (messageInfo.level)
            {
            case SHVDN::Log::Level::Error:
                console->PrintError(messageInfo.message);
                break;
            case SHVDN::Log::Level::Warning:
                console->PrintWarning(messageInfo.message);
                break;
            }
        }
    }

    static void UpdatePrimaryKeyboardStateCache(unsigned char keyCode, bool down)
    {
        _keyboardStatePool[keyCode] = down;
    }
    static void UpdateKeyboardModifierStateCache(bool shift, bool ctrl, bool alt)
    {
        ModifierKeyState newState = (shift ? ModifierKeyState::Shift : ModifierKeyState::None);
        newState = (ctrl ? newState | ModifierKeyState::Control : newState);
        newState = (alt ? newState | ModifierKeyState::Alt : newState);

        _modKeyState = newState;
    }
};

ref class InvalidKeysFoundError sealed
{
internal:
    value class PositionAndUnparsedStrInfo
    {
    private:
        int _position;
        String^ _unparsedStr;

    public:
        PositionAndUnparsedStrInfo(int position, String^ unparsedStr)
        {
            _position = position;
            _unparsedStr = unparsedStr;
        }

        property int Position
        {
            int get() {
                return _position;
            }
        }
        property String^ UnparsedString
        {
            String^ get()
            {
                return _unparsedStr;
            }
        }
    };

    array<InvalidKeysFoundError::PositionAndUnparsedStrInfo>^ _unparsedStrInfo;

    InvalidKeysFoundError(array<InvalidKeysFoundError::PositionAndUnparsedStrInfo>^ input)
    {
        _unparsedStrInfo = gcnew array<InvalidKeysFoundError::PositionAndUnparsedStrInfo>(input->Length);
        input->CopyTo(_unparsedStrInfo, 0);
    }

    array<InvalidKeysFoundError::PositionAndUnparsedStrInfo>^ GetUnparsedStrSequence()
    {
        auto newArray = gcnew array<InvalidKeysFoundError::PositionAndUnparsedStrInfo>(_unparsedStrInfo->Length);
        _unparsedStrInfo->CopyTo(newArray, 0);
        return newArray;
    }
};

ref class InvalidKeysFoundErrorBuilder sealed
{
private:
    List<InvalidKeysFoundError::PositionAndUnparsedStrInfo>^ _invalidKeys;

public:
    InvalidKeysFoundErrorBuilder()
    {
        _invalidKeys = gcnew List<InvalidKeysFoundError::PositionAndUnparsedStrInfo>();
    }

    void Add(int position, String^ unparsedStr)
    {
        auto posAndKeyInfo = InvalidKeysFoundError::PositionAndUnparsedStrInfo(position, unparsedStr);
        _invalidKeys->Add(posAndKeyInfo);
    }

    InvalidKeysFoundError^ Build()
    {
        return gcnew InvalidKeysFoundError(_invalidKeys->ToArray());
    }
};

// This is for internal use, so use a System.Array for faster iteration
static ValueTuple<array<WinForms::Keys>^, InvalidKeysFoundError^> ParseKeyBinding(String^ input)
{
    InvalidKeysFoundErrorBuilder^ errorBuilder = nullptr;

    array<String^>^ inputSubstrings = input->Split('+');
    List<WinForms::Keys>^ resultKeyBinding = gcnew List<WinForms::Keys>(inputSubstrings->Length);

    for (int i = 0; i < inputSubstrings->Length; i++)
    {
        String^ keyStr = inputSubstrings[i]->Trim();

        WinForms::Keys parsedKey;
        if (Enum::TryParse(keyStr, true, parsedKey))
        {
            resultKeyBinding->Add(parsedKey);
        }
        else
        {
            if (errorBuilder == nullptr)
            {
                errorBuilder = gcnew InvalidKeysFoundErrorBuilder();
            }

            errorBuilder->Add(i, keyStr);
        }
    }

    InvalidKeysFoundError^ error = (errorBuilder != nullptr ? errorBuilder->Build() : nullptr);
    return ValueTuple<array<WinForms::Keys>^, InvalidKeysFoundError^>(resultKeyBinding->ToArray(), error);
}

static void AppendPendingMessageForConsole(List<ScriptHookVDotNet::LogMessageInfo>^ messageInfoBuffer,
    SHVDN::Log::Level level, String^ input)
{
    messageInfoBuffer->Add(ScriptHookVDotNet::LogMessageInfo(level, input));
}

static String^ BuildKeyCombinationString(array<WinForms::Keys>^ keyBinding)
{
    if (keyBinding == nullptr)
    {
        throw gcnew ArgumentNullException("keyBinding");
    }
    if (keyBinding->Length == 0)
    {
        return String::Empty;
    }

    Text::StringBuilder^ strBuilder = gcnew Text::StringBuilder();
    bool hasAlreadyFoundFirstElement = false;

    for each (WinForms::Keys key in keyBinding)
    {
        if (hasAlreadyFoundFirstElement)
        {
            strBuilder->Append(" + ");
        }
        else
        {
            hasAlreadyFoundFirstElement = true;
        }

        strBuilder->Append((WinForms::Keys::typeid)->GetEnumName(static_cast<int>(key)));
    }

    return strBuilder->ToString();
}

static void LogKeyBindingParseError(String^ rawInput, InvalidKeysFoundError^ invalidKeysFoundError,
    List<ScriptHookVDotNet::LogMessageInfo>^ messageInfoBuffer, array<WinForms::Keys>^ defaultKeyBinding)
{
    AppendPendingMessageForConsole(messageInfoBuffer,
        SHVDN::Log::Level::Error,
        String::Format(
            "InvalidKeysFoundError: Failed to parse a key binding from the string \"{0}\". See {1} for details.",
            rawInput,
            SHVDN::Log::FileName
        )
    );

    Text::StringBuilder^ errorLineBuilder = gcnew Text::StringBuilder();
    bool hasAlreadyFoundFirstUnparsedString = false;
    for each (auto unparsedInfo in invalidKeysFoundError->GetUnparsedStrSequence())
    {
        if (hasAlreadyFoundFirstUnparsedString)
        {
            errorLineBuilder->Append(Environment::NewLine);
        }
        else
        {
            hasAlreadyFoundFirstUnparsedString = true;
        }
        errorLineBuilder->Append(
            String::Format("    Found an invalid key name \"{0}\" at index {1}", unparsedInfo.UnparsedString, unparsedInfo.Position)
        );
    }

    SHVDN::Log::WriteToFile(SHVDN::Log::Level::Error,
        String::Format(
            "InvalidKeysFoundError: Failed to parse a key binding from the string \"{0}\":{1}{2}{3}{4}{5}{6}",
            rawInput,
            Environment::NewLine,
            Environment::NewLine,
            errorLineBuilder->ToString(),
            Environment::NewLine,
            Environment::NewLine,
            "    Fallbacked to \"" + BuildKeyCombinationString(defaultKeyBinding) + "\""
        )
    );
}

static void ScriptHookVDotNet_ManagedInit()
{
    // Repair the broken NLS state (see the LCMapStringEx hook above) before any culture-sensitive
    // operation - String.ToLower, AppDomain.CreateDomain or DateTime.Now - is performed.
    InstallLCMapStringExHook();
    SHVDN::ScriptDomain::RepairBrokenNlsState();

    SHVDN::ScriptDomain^ domain = nullptr;
    SHVDN::Console^ console = nullptr;
    List<String^>^ stashedConsoleCommandHistory = gcnew List<String^>();
    List<ScriptHookVDotNet::LogMessageInfo>^ pendingLogMessageInfo = gcnew List<ScriptHookVDotNet::LogMessageInfo>();

    // Unload previous domain (this unloads all script assemblies too)
    {
        msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);

        domain = ScriptHookVDotNet::domain;
        console = ScriptHookVDotNet::console;

        if (domain != nullptr)
        {
            console = (SHVDN::Console^)domain->AppDomain->GetData("Console");
            // Stash the command history if console is loaded
            if (console != nullptr)
            {
                stashedConsoleCommandHistory = console->CommandHistory;
                ScriptHookVDotNet::console = nullptr;
            }

            SHVDN::ScriptDomain::Unload(domain);
            ScriptHookVDotNet::domain = nullptr;
        }
    }

    // Clear log from previous runs
    SHVDN::Log::Clear();

    // Load configuration
    String^ scriptPath = "scripts";

    try
    {
        array<String^>^ config = IO::File::ReadAllLines(IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));

        for each (String ^ line in config)
        {
            // Perform some very basic key/value parsing
            line = line->Trim();
            if (line->StartsWith(";") || line->StartsWith("#") || line->StartsWith("//"))
                continue;
            array<String^>^ data = line->Split('=');
            if (data->Length != 2)
                continue;

            // May fail to parse without trimming whitespaces
            String^ keyStr = data[0]->Trim();
            String^ valueStr = data[1]->Trim();

            ValueTuple<array<WinForms::Keys>^, InvalidKeysFoundError^> keyCombinationResult;

            if (String::Equals(keyStr, "ReloadKeyBinding", StringComparison::OrdinalIgnoreCase)) {
                keyCombinationResult = ParseKeyBinding(valueStr);
                InvalidKeysFoundError^ invalidKeysError = keyCombinationResult.Item2;
                if (invalidKeysError != nullptr)
                {
                    LogKeyBindingParseError(valueStr, invalidKeysError, pendingLogMessageInfo, ScriptHookVDotNet::reloadKeyBinding);
                }
                else
                {
                    // this lock is needed to prevent from the keyboard thread reading a stale value
                    msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);
                    ScriptHookVDotNet::reloadKeyBinding = keyCombinationResult.Item1;
                }
            }
            else if (String::Equals(keyStr, "ConsoleKeyBinding", StringComparison::OrdinalIgnoreCase))
            {
                keyCombinationResult = ParseKeyBinding(valueStr);
                InvalidKeysFoundError^ invalidKeysError = keyCombinationResult.Item2;
                if (invalidKeysError != nullptr)
                {
                    LogKeyBindingParseError(valueStr, invalidKeysError, pendingLogMessageInfo, ScriptHookVDotNet::consoleKeyBinding);
                }
                else
                {
                    msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);
                    ScriptHookVDotNet::consoleKeyBinding = keyCombinationResult.Item1;
                }
            }
            else if (String::Equals(keyStr, "ScriptTimeoutThreshold", StringComparison::OrdinalIgnoreCase))
            {
                unsigned int outVal;
                if (UInt32::TryParse(valueStr, outVal))
                {
                    ScriptHookVDotNet::scriptTimeoutThreshold = outVal;
                }
            }
            else if (String::Equals(keyStr, "ScriptsLocation", StringComparison::OrdinalIgnoreCase))
                scriptPath = valueStr->Trim('"');
            else if (String::Equals(keyStr, "AutoLoadScripts", StringComparison::OrdinalIgnoreCase))
            {
                bool outVal;
                if (Boolean::TryParse(valueStr, outVal))
                {
                    ScriptHookVDotNet::AutoLoadScripts = outVal;
                }
            }
        }
    }
    catch (Exception^ ex)
    {
        SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to load config: ", ex->ToString());
    }

    // Create a separate script domain
    domain = SHVDN::ScriptDomain::Load(".", scriptPath);
    if (domain == nullptr)
        return;

    {
        msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);
        ScriptHookVDotNet::domain = domain;
    }

    domain->ScriptTimeoutThreshold = ScriptHookVDotNet::scriptTimeoutThreshold;

    // Set functions for Thread Local Storage (TLS), so scripts can do tasks that need variables in the TLS of the main thread in their script thread
    domain->InitTlsStuffForTlsContextSwitch(static_cast<IntPtr>(GetTlsContext), static_cast<IntPtr>(SetTlsContext),
        static_cast<IntPtr>(GetTlsContextAddrOfGameMainThread()), GetGameMainThreadId());

    try
    {
        // Initialize and scan memory at a predictable point
        domain->InitNativeNemoryMembers();
    }
    catch (Exception^ ex)
    {
        SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to initialize native memory members: ", ex->ToString());
    }

    try
    {
        // Instantiate console inside script domain, so that it can access the scripting API
        console = (SHVDN::Console^)domain->AppDomain->CreateInstanceFromAndUnwrap(
            SHVDN::Console::typeid->Assembly->Location, SHVDN::Console::typeid->FullName);

        // Restore the console command history (set a empty history for the first time)
        console->CommandHistory = stashedConsoleCommandHistory;

        // Print welcome message
        console->PrintInfo("~c~--- Community Script Hook V .NET " SHVDN_VERSION " ---");
        console->PrintInfo("~c~--- Type \"Help()\" to print an overview of available commands ---");

        ScriptHookVDotNet::SendPendingMessagesToConsole(console, pendingLogMessageInfo);

        // Update console pointer in script domain
        domain->AppDomain->SetData("Console", console);
        domain->AppDomain->DoCallBack(gcnew CrossAppDomainDelegate(&ScriptHookVDotNet::SetConsole));

        // Add default console commands
        console->RegisterCommands(ScriptHookVDotNet::typeid);
    }
    catch (Exception^ ex)
    {
        SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to create console: ", ex->ToString());
    }
    {
        msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);
        ScriptHookVDotNet::console = console;
    }

    if (ScriptHookVDotNet::AutoLoadScripts)
    {
        // Start scripts in the newly created domain
        domain->Start();
    }
    else
    {
        SHVDN::Log::Message(SHVDN::Log::Level::Debug, "AutoLoadScripts is set to false, skipping auto loading scripts. " +
            "You can start scripts with specific console commands such as `StartAllScripts()`.");
    }
}

static void ScriptHookVDotNet_ManagedTick()
{
    msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);

    SHVDN::Console^ console = ScriptHookVDotNet::console;
    if (console != nullptr)
        console->DoTick();

    SHVDN::ScriptDomain^ scriptDomain = ScriptHookVDotNet::domain;
    if (scriptDomain != nullptr)
        scriptDomain->DoTick();
}

static bool AreAllKeysPressed(array<WinForms::Keys>^ keys)
{
    for each (WinForms::Keys key in keys)
    {
        switch (key)
        {
        case WinForms::Keys::Shift:
            if ((ScriptHookVDotNet::_modKeyState & static_cast<ModifierKeyState>(WinForms::Keys::Shift))
                == ModifierKeyState::None)
            {
                return false;
            }
            break;
        case WinForms::Keys::Control:
            if ((ScriptHookVDotNet::_modKeyState & static_cast<ModifierKeyState>(WinForms::Keys::Control))
                == ModifierKeyState::None)
            {
                return false;
            }
            break;
        case WinForms::Keys::Alt:
            if ((ScriptHookVDotNet::_modKeyState & static_cast<ModifierKeyState>(WinForms::Keys::Alt))
                == ModifierKeyState::None)
            {
                return false;
            }
            break;
        default:
            if (!ScriptHookVDotNet::_keyboardStatePool[(int)key])
            {
                return false;
            }
        }
    }

    return true;
}

static void ScriptHookVDotNet_ManagedKeyboardMessage(unsigned long keycode, bool keydown, bool ctrl, bool shift, bool alt)
{
    // Filter out invalid key codes
    if (keycode <= 0 || keycode >= 256)
        return;

    // Protect against race condition during reload.
    // Also prevent from the keyboard thread reading stale values such as key bindings.
    msclr::lock l(ScriptHookVDotNet::variablesLockForMainDomain);

    SHVDN::ScriptDomain^ scriptDomain = ScriptHookVDotNet::domain;
    // If the TLS stuff is not initialized, the console or some script can call a native function without swapping
    // TLS address, leading the whole process to crash
    if (scriptDomain == nullptr || !scriptDomain->IsTlsStuffInitialized())
    {
        return;
    }

    ScriptHookVDotNet::UpdatePrimaryKeyboardStateCache(static_cast<unsigned char>(keycode), keydown);
    ScriptHookVDotNet::UpdateKeyboardModifierStateCache(shift, ctrl, alt);

    // Convert message into a key event
    auto keys = safe_cast<WinForms::Keys>(keycode);
    if (ctrl)  keys = keys | WinForms::Keys::Control;
    if (shift) keys = keys | WinForms::Keys::Shift;
    if (alt)   keys = keys | WinForms::Keys::Alt;

    SHVDN::Console^ console = ScriptHookVDotNet::console;
    if (console != nullptr)
    {
        if (keydown && AreAllKeysPressed(ScriptHookVDotNet::reloadKeyBinding))
        {
            // Force a reload
            ScriptHookVDotNet::Reload();
            return;
        }
        if (keydown && AreAllKeysPressed(ScriptHookVDotNet::consoleKeyBinding))
        {
            // Toggle open state
            console->IsOpen = !console->IsOpen;
            return;
        }

        // Send key events to console
        console->DoKeyEvent(keys, keydown);

        // Do not send keyboard events to other running scripts when console is open
        if (console->IsOpen)
            return;
    }

    // Send key events to all scripts
    scriptDomain->DoKeyEvent(keys, keydown);
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
        ScriptHookVDotNet_ManagedInit();
        sScriptDomainRequestedToReload.store(false, std::memory_order_release);

        // ScriptHookV's script fibers will be executed in the main thread, so we assume this code won't have trouble
        // exiting the loop after letting the main fiber execute
        while (!sScriptDomainRequestedToReload.load(std::memory_order_relaxed)
            && !sClrThreadRequestedToExit.load(std::memory_order_relaxed)) {
            ScriptHookVDotNet_ManagedTick();
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

    ScriptHookVDotNet_ManagedKeyboardMessage(
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
