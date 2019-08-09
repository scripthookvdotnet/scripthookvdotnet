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

bool sGameReloaded = false;

#pragma managed

// Import C# code base
#using "ScriptHookVDotNet.netmodule"

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

public ref class ScriptHookVDotNet
{
public:
	[SHVDN::ConsoleCommand("Print the default help")]
	static void Help()
	{
		console->PrintInfo("--- Help ---");
		console->PrintHelpText();
	}
	[SHVDN::ConsoleCommand("Print the help for a specific command")]
	static void Help(String ^command)
	{
		console->PrintHelpText(command);
	}

	[SHVDN::ConsoleCommand("Clear the console history and pages")]
	static void Clear()
	{
		console->Clear();
	}

	[SHVDN::ConsoleCommand("Load scripts from a file")]
	static void Load(String ^filename)
	{
		if (!IO::Path::IsPathRooted(filename))
		{
			String ^basedirectory = domain->ScriptPath;

			if (!IO::File::Exists(IO::Path::Combine(basedirectory, filename)))
			{
				array<String ^> ^files = IO::Directory::GetFiles(basedirectory, filename, IO::SearchOption::AllDirectories);

				if (files->Length != 1)
				{
					console->PrintError("The file " + filename + " was not found in " + basedirectory);
					return;
				}
				else
				{
					console->PrintWarning("The file " + filename + " was not found in " + basedirectory + ", loading from " + IO::Path::GetDirectoryName(files[0]->Substring(basedirectory->Length + 1)) + " instead");
				}

				filename = files[0]->Substring(basedirectory->Length + 1);
			}
			else
			{
				filename = IO::Path::Combine(basedirectory, filename);
			}
		}

		if (IO::Path::HasExtension(filename))
		{
			String ^extension = IO::Path::GetExtension(filename)->ToLower();
			if (extension != ".cs" && extension != ".vb" && extension != ".dll")
			{
				console->PrintError("The file '" + filename + "' was not recognized as a script file");
				return;
			}
		}
		else
		{
			filename += ".dll";
		}

		domain->StartScripts(filename);
	}
	[SHVDN::ConsoleCommand("Reload all scripts from the scripts directory")]
	static void Reload()
	{
		console->PrintInfo("Reloading ...");

		// Force a reload on next tick
		sGameReloaded = true;
	}

	[SHVDN::ConsoleCommand("Abort all scripts from a file")]
	static void Abort(String ^filename)
	{
		domain->AbortScripts(IO::Path::Combine(domain->ScriptPath, filename));
	}
	[SHVDN::ConsoleCommand("Abort all scripts currently running")]
	static void AbortAll()
	{
		domain->Abort();
	}

	[SHVDN::ConsoleCommand("List all loaded scripts")]
	static void ListScripts()
	{
		for each (auto script in domain->RunningScripts)
			console->PrintInfo(IO::Path::GetFileName(script->Filename) + ": " + script->Name + (script->IsRunning ? " ~g~[running]" : " ~r~[aborted]"));
	}

internal:
	static SHVDN::Console ^console = nullptr;
	static SHVDN::ScriptDomain ^domain = SHVDN::ScriptDomain::CurrentDomain;

	static void SetConsole()
	{
		console = (SHVDN::Console ^)AppDomain::CurrentDomain->GetData("Console");
	}
};

static void ScriptHookVDotnet_ManagedInit()
{
	AppDomain::CurrentDomain->UnhandledException += gcnew UnhandledExceptionEventHandler(&SHVDN::ScriptDomain::HandleUnhandledException);

	SHVDN::Console ^%console = ScriptHookVDotNet::console;
	SHVDN::ScriptDomain ^%domain = ScriptHookVDotNet::domain;

	// Unload previous domain (this unloads all script assemblies too)
	if (domain != nullptr)
		SHVDN::ScriptDomain::Unload(domain);

	// Clear log from previous runs
	SHVDN::Log::Clear();

	// Create a separate script domain
	domain = SHVDN::ScriptDomain::Load(".", "scripts");
	if (domain == nullptr)
		return;

	// Initialize console
	try
	{
		console = (SHVDN::Console ^)domain->AppDomain->CreateInstanceFromAndUnwrap(
			SHVDN::Console::typeid->Assembly->Location, SHVDN::Console::typeid->FullName);

		// Print welcome message
		console->PrintInfo("--- Community Script Hook V .NET " SHVDN_VERSION " ---");
		console->PrintInfo("--- Type \"Help()\" to print an overview of available commands ---");

		// Update console pointer in script domain
		domain->AppDomain->SetData("Console", console);
		domain->AppDomain->DoCallBack(gcnew CrossAppDomainDelegate(&ScriptHookVDotNet::SetConsole));

		// Add default console commands
		console->RegisterCommands(ScriptHookVDotNet::typeid);
	}
	catch (Exception ^ex)
	{
		SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to create console: ", ex->ToString());
	}

	// Start scripts in the newly created domain
	domain->Start();
}

static void ScriptHookVDotnet_ManagedTick()
{
	SHVDN::Console ^console = ScriptHookVDotNet::console;
	if (console != nullptr)
		console->DoTick();

	SHVDN::ScriptDomain ^scriptdomain = ScriptHookVDotNet::domain;
	if (scriptdomain != nullptr)
		scriptdomain->DoTick();
}

static void ScriptHookVDotnet_ManagedKeyboardMessage(unsigned long keycode, bool keydown, bool ctrl, bool shift, bool alt)
{
	// Filter out invalid key codes
	if (keycode < 0 || keycode >= 256)
		return;

	// Convert message into a key event
	auto keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	SHVDN::Console ^console = ScriptHookVDotNet::console;
	if (console != nullptr) {
		// Send key events to console
		console->DoKeyEvent(keys, keydown);

		// Do not send keyboard events to other running scripts when console is open
		if (console->IsOpen)
			return;
	}

	SHVDN::ScriptDomain ^scriptdomain = ScriptHookVDotNet::domain;
	if (scriptdomain != nullptr)
		// Send key events to all scripts
		scriptdomain->DoKeyEvent(keys, keydown);
}

#pragma unmanaged

#include <Main.h>
#include <Windows.h>

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
				sGameReloaded = false;

				ScriptHookVDotnet_ManagedInit();

				// If the game is reloaded, ScriptHookV will call the script main function again.
				// This will set the global 'sGameReloaded' variable to 'true' and on the next fiber switch to our CLR fiber, run into this condition, therefore exiting the inner loop and re-initialize.
				while (!sGameReloaded)
				{
					ScriptHookVDotnet_ManagedTick();

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
