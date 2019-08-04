/**
 * Copyright (C) 2015 crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

#pragma managed

// Import C# code base
#using "ScriptHookVDotNet.netmodule"

using namespace SHVDN;
using namespace System;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

[assembly:AssemblyTitle("Community Script Hook V .NET")];
[assembly:AssemblyDescription("An ASI plugin for Grand Theft Auto V, which allows running scripts written in any .NET language in-game.")];
[assembly:AssemblyCompany("crosire & contributors")];
[assembly:AssemblyProduct("ScriptHookVDotNet")];
[assembly:AssemblyCopyright("Copyright © 2015 crosire")];
[assembly:AssemblyVersion(SHVDN_VERSION)];
[assembly:AssemblyFileVersion(SHVDN_VERSION)];

static void ScriptHookVDotnet_ManagedInit()
{
	// Unload any previous instances
	for each (ScriptDomain ^domain in ScriptDomain::Instances)
		ScriptDomain::Unload(domain);
	ScriptDomain::Instances->Clear();

	// Clear log from previous ScriptHookVDotNet runs
	Log::Clear();

	// Create a separate script domain for each API version
	Log::Message(Log::Level::Info, "Loading ScriptHookVDotNet API versions ...");

	for each (String ^apiPath in IO::Directory::EnumerateFiles(".", "ScriptHookVDotNet*.dll", IO::SearchOption::TopDirectoryOnly))
	{
		ScriptDomain ^domain = ScriptDomain::Load(apiPath, "scripts");
		if (domain == nullptr)
			continue;

		// Start scripts in the newly created domain
		domain->Start();

		ScriptDomain::Instances->Add(domain);
	}
}

static void ScriptHookVDotnet_ManagedTick(unsigned int index)
{
	SHVDN::Console ^console = SHVDN::Console::Instance;
	console->DoTick();

	for each (ScriptDomain ^domain in ScriptDomain::Instances)
		domain->DoTick();
}

static void ScriptHookVDotnet_ManagedKeyboardMessage(unsigned long keycode, bool keydown, bool ctrl, bool shift, bool alt)
{
	// Filter out invalid key codes
	if (keycode < 0 || keycode >= 256)
		return;

	// Convert message into a key event
	WinForms::Keys keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	SHVDN::Console ^console = SHVDN::Console::Instance;
	if (keydown) // Send key down events to console
		console->DoKeyDown(keys);

	// Do not send keyboard events to other running scripts when console is open
	if (!console->IsOpen)
		for each (ScriptDomain ^domain in ScriptDomain::Instances)
			domain->DoKeyboardMessage(keys, keydown);
}

#pragma unmanaged

#include <Main.h>
#include <Windows.h>

bool sGameReloaded = false;
PVOID sGameFiber = nullptr;
PVOID sScriptFiber = nullptr;

static void ScriptMain()
{
	sGameReloaded = true;

	// ScriptHookV already turned the current thread into a fiber, so we can safely retrieve it.
	sGameFiber = GetCurrentFiber();

	// Check if our CLR fiber already exists. It should be created only once for the entire lifetime of the game process.
	if (sScriptFiber == nullptr)
	{
		const LPFIBER_START_ROUTINE FiberMain = [](LPVOID lpFiberParameter) {
			// Main script execution loop
			while (true)
			{
				ScriptHookVDotnet_ManagedInit();

				sGameReloaded = false;

				// If the game is reloaded, ScriptHookV will call the script main function again.
				// This will set the global 'sGameReloaded' variable to 'true' and on the next fiber switch to our CLR fiber, run into this condition, therefore exiting the inner loop and re-initialize.
				while (!sGameReloaded)
				{
					ScriptHookVDotnet_ManagedTick(0);

					// Switch back to main script fiber used by ScriptHookV.
					// Code continues from here the next time the loop below switches back to our CLR fiber.
					SwitchToFiber(sGameFiber);
				}
			}
		};

		// Create our own fiber for the common language runtime, aka CLR, once.
		// This is done because ScriptHookV switches its internal fibers sometimes, which would corrupt the CLR stack.
		sScriptFiber = CreateFiber(0, FiberMain, nullptr);
	}

	while (true)
	{
		// Yield execution and give it back to ScriptHookV.
		scriptWait(0);

		// Switch to our CLR fiber and wait for it to switch back.
		SwitchToFiber(sScriptFiber);
	}
}

//template <unsigned int index>
//static void ScriptThread()
//{
//	const LPFIBER_START_ROUTINE FiberMain = [](LPVOID lpFiberParameter) {
//		while (true)
//		{
//			ScriptHookVDotnet_ManagedTick(index);
//		}
//	};
//
//	PVOID fiber = CreateFiber(0, FiberMain, nullptr);
//
//	while (true)
//	{
//		scriptWait(0);
//
//		SwitchToFiber(fiber);
//	}
//}

static void ScriptCleanup()
{
	DeleteFiber(sScriptFiber);
}

static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	ScriptHookVDotnet_ManagedKeyboardMessage(
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
		// Avoid unnecessary DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications
		DisableThreadLibraryCalls(hModule);
		// Register ScriptHookVDotNet native script
		scriptRegister(hModule, ScriptMain);
		// Register a few additional threads for faster script execution
		//scriptRegisterAdditionalThread(hModule, ScriptThread<1>);
		//scriptRegisterAdditionalThread(hModule, ScriptThread<2>);
		//scriptRegisterAdditionalThread(hModule, ScriptThread<3>);
		//scriptRegisterAdditionalThread(hModule, ScriptThread<4>);
		// Register handler for keyboard messages
		keyboardHandlerRegister(ScriptKeyboardMessage);
		break;
	case DLL_PROCESS_DETACH:
		ScriptCleanup();
		// Unregister ScriptHookVDotNet native script
		scriptUnregister(hModule);
		// Unregister handler for keyboard messages
		keyboardHandlerUnregister(ScriptKeyboardMessage);
		break;
	}

	return TRUE;
}
