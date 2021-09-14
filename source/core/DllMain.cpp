/**
 * Copyright (C) 2015 crosire & contributors
 * License: https://github.com/crosire/scripthookvdotnet#license
 */

bool sGameReloaded = false;

#pragma managed

// Import C# code base
#using "ScriptHookVDotNet.netmodule"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

[assembly:AssemblyTitle("Community Script Hook V .NET")];
[assembly:AssemblyDescription("An ASI plugin for Grand Theft Auto V, which allows running scripts written in any .NET language in-game.")];
[assembly:AssemblyCompany("crosire & contributors")];
[assembly:AssemblyProduct("ScriptHookVDotNet")];
[assembly:AssemblyCopyright("Copyright © 2015 crosire")];
[assembly:AssemblyVersion(SHVDN_VERSION)];
[assembly:AssemblyFileVersion(SHVDN_VERSION)];
// Sign with a strong name to distinguish from older versions and cause .NET framework runtime to bind the correct assemblies
// There is no version check performed for assemblies without strong names (https://docs.microsoft.com/en-us/dotnet/framework/deployment/how-the-runtime-locates-assemblies)
[assembly:AssemblyKeyFileAttribute("PublicKeyToken.snk")];

public ref class ScriptHookVDotNet
{
public:
	[SHVDN::ConsoleCommand("Print the default help")]
	static void Help()
	{
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
		console->PrintHelpText(command);
	}

	[SHVDN::ConsoleCommand("Clear the console history and pages")]
	static void Clear()
	{
		console->Clear();
	}

	[SHVDN::ConsoleCommand("Reload all scripts from the scripts directory")]
	static void Reload()
	{
		console->PrintInfo("~y~Reloading ...");

		// Force a reload on next tick
		sGameReloaded = true;
	}

	[SHVDN::ConsoleCommand("Load scripts from a file")]
	static void Start(String ^filename)
	{
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
	[SHVDN::ConsoleCommand("Abort all scripts from a file")]
	static void Abort(String ^filename)
	{
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
		domain->Abort();

		console->PrintInfo("Stopped all running scripts. Use \"Start(filename)\" to start them again.");
	}

	[SHVDN::ConsoleCommand("List all loaded scripts")]
	static void ListScripts()
	{
		console->PrintInfo("~c~--- Loaded Scripts ---");
		for each (auto script in domain->RunningScripts)
			console->PrintInfo(IO::Path::GetFileName(script->Filename) + " ~h~" + script->Name + (script->IsRunning ? (script->IsPaused ? " ~o~[paused]" : " ~g~[running]") : " ~r~[aborted]"));
	}

internal:
	static SHVDN::Console ^console = nullptr;
	static SHVDN::ScriptDomain ^domain = SHVDN::ScriptDomain::CurrentDomain;
	static WinForms::Keys reloadKey = WinForms::Keys::None;
	static WinForms::Keys consoleKey = WinForms::Keys::F4;


	static void SetConsole()
	{
		console = (SHVDN::Console ^)AppDomain::CurrentDomain->GetData("Console");
	}
};

static void ScriptHookVDotnet_ManagedInit()
{
	SHVDN::Console^% console = ScriptHookVDotNet::console;
	SHVDN::ScriptDomain^% domain = ScriptHookVDotNet::domain;
	List<String^>^ stashedConsoleCommandHistory = gcnew List<String^>();

	// Unload previous domain (this unloads all script assemblies too)
	if (domain != nullptr)
	{
		// Stash the command history if console is loaded 
		if (console != nullptr)
		{
			stashedConsoleCommandHistory = console->CommandHistory;
		}

		SHVDN::ScriptDomain::Unload(domain);
	}


	// Clear log from previous runs
	SHVDN::Log::Clear();

	// Load configuration
	String ^scriptPath = "scripts";

	try
	{
		array<String ^> ^config = IO::File::ReadAllLines(IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));

		for each (String ^line in config)
		{
			// Perform some very basic key/value parsing
			line = line->Trim();
			if (line->StartsWith("//"))
				continue;
			array<String ^> ^data = line->Split('=');
			if (data->Length != 2)
				continue;

			     if (data[0] == "ReloadKey")
				Enum::TryParse(data[1], true, ScriptHookVDotNet::reloadKey);
			else if (data[0] == "ConsoleKey")
				Enum::TryParse(data[1], true, ScriptHookVDotNet::consoleKey);
			else if (data[0] == "ScriptsLocation")
				scriptPath = data[1];
		}
	}
	catch (Exception ^ex)
	{
		SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to load config: ", ex->ToString());
	}

	// Create a separate script domain
	domain = SHVDN::ScriptDomain::Load(".", scriptPath);
	if (domain == nullptr)
		return;

	try
	{
		// Instantiate console inside script domain, so that it can access the scripting API
		console = (SHVDN::Console ^)domain->AppDomain->CreateInstanceFromAndUnwrap(
			SHVDN::Console::typeid->Assembly->Location, SHVDN::Console::typeid->FullName);

		// Restore the console command history (set a empty history for the first time)
		console->CommandHistory = stashedConsoleCommandHistory;

		// Print welcome message
		console->PrintInfo("~c~--- Community Script Hook V .NET " SHVDN_VERSION " ---");
		console->PrintInfo("~c~--- Type \"Help()\" to print an overview of available commands ---");

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
	if (keycode <= 0 || keycode >= 256)
		return;

	// Convert message into a key event
	auto keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	SHVDN::Console ^console = ScriptHookVDotNet::console;
	if (console != nullptr)
	{
		if (keydown && keys == ScriptHookVDotNet::reloadKey)
		{
			// Force a reload
			ScriptHookVDotNet::Reload();
			return;
		}
		if (keydown && keys == ScriptHookVDotNet::consoleKey)
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

	SHVDN::ScriptDomain ^scriptdomain = ScriptHookVDotNet::domain;
	if (scriptdomain != nullptr)
	{
		// Send key events to all scripts
		scriptdomain->DoKeyEvent(keys, keydown);
	}
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
