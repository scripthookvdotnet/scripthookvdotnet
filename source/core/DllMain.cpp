/**
 * Copyright (C) 2015 crosire & kagikn & contributors
 * License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
 */

#pragma managed(push, off)

#include <Windows.h>
#include <atomic>

std::atomic<LPVOID> sTlsContextAddrOfGameMainThread(nullptr);
std::atomic<DWORD> sGameMainThreadId(0);

// Use atomic to guarantee its visibility
// Script domain should be reloaded as soon as getting requested to reload, so this should be used with std::memory_order_seq_cst
// when storing a value so a xchg instruction will be issued for memory barrier
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
	return sTlsContextAddrOfGameMainThread.load();
}
static DWORD GetGameMainThreadId()
{
	return sGameMainThreadId.load();
}

static void RequestScriptDomainToReload()
{
	sScriptDomainRequestedToReload.store(true);
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
		RequestScriptDomainToReload();
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
	static array<WinForms::Keys>^ reloadKeyBinding = { WinForms::Keys::None };
	static array<WinForms::Keys>^ consoleKeyBinding = { WinForms::Keys::F4 };
	static unsigned int scriptTimeoutThreshold = 5000;
	static bool shouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker = true;
	static Object^ unloadLock = gcnew Object();
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

	static void SetConsole()
	{
		console = (SHVDN::Console ^)AppDomain::CurrentDomain->GetData("Console");
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

// This is for internel use, so use a System.Array for faster iteration
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
	SHVDN::Console^% console = ScriptHookVDotNet::console;
	SHVDN::ScriptDomain^% domain = ScriptHookVDotNet::domain;
	List<String^>^ stashedConsoleCommandHistory = gcnew List<String^>();
	List<ScriptHookVDotNet::LogMessageInfo>^ pendingLogMessageInfo = gcnew List<ScriptHookVDotNet::LogMessageInfo>();

	// Unload previous domain (this unloads all script assemblies too)
	{
		msclr::lock l(ScriptHookVDotNet::unloadLock);

		if (domain != nullptr)
		{
			// Stash the command history if console is loaded
			if (console != nullptr)
			{
				stashedConsoleCommandHistory = console->CommandHistory;
				console = nullptr;
			}

			SHVDN::ScriptDomain::Unload(domain);
			domain = nullptr;
		}

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
			else if (String::Equals(keyStr, "WarnOfDeprecatedScriptsWithTicker", StringComparison::OrdinalIgnoreCase))
			{
				bool outVal;
				if (Boolean::TryParse(valueStr, outVal))
				{
					ScriptHookVDotNet::shouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker = outVal;
				}
			}
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

	// Set functions for Thread Local Storage (TLS), so scripts can do tasks that need variables in the TLS of the main thread in their script thread
	domain->InitTlsFunctionPointers(static_cast<IntPtr>(GetTlsContext), static_cast<IntPtr>(SetTlsContext));
	domain->SetTlsContextOfGameMainThread(static_cast<IntPtr>(GetTlsContextAddrOfGameMainThread()));
	domain->SetGameMainThreadId(GetGameMainThreadId());

	domain->ScriptTimeoutThreshold = ScriptHookVDotNet::scriptTimeoutThreshold;
	domain->ShouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker = ScriptHookVDotNet::shouldWarnOfScriptsBuiltAgainstDeprecatedApiWithTicker;

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
		console = (SHVDN::Console ^)domain->AppDomain->CreateInstanceFromAndUnwrap(
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
	catch (Exception ^ex)
	{
		SHVDN::Log::Message(SHVDN::Log::Level::Error, "Failed to create console: ", ex->ToString());
	}

	// Start scripts in the newly created domain
	domain->Start();
}

static void ScriptHookVDotNet_ManagedTick()
{
	SHVDN::Console ^console = ScriptHookVDotNet::console;
	if (console != nullptr)
		console->DoTick();

	SHVDN::ScriptDomain ^scriptDomain = ScriptHookVDotNet::domain;
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

	ScriptHookVDotNet::UpdatePrimaryKeyboardStateCache(static_cast<unsigned char>(keycode), keydown);
	ScriptHookVDotNet::UpdateKeyboardModifierStateCache(shift, ctrl, alt);

	// Convert message into a key event
	auto keys = safe_cast<WinForms::Keys>(keycode);
	if (ctrl)  keys = keys | WinForms::Keys::Control;
	if (shift) keys = keys | WinForms::Keys::Shift;
	if (alt)   keys = keys | WinForms::Keys::Alt;

	// Protect against race condition during reload
	msclr::lock l(ScriptHookVDotNet::unloadLock);
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

	SHVDN::ScriptDomain ^scriptDomain = ScriptHookVDotNet::domain;
	if (scriptDomain != nullptr)
	{
		// Send key events to all scripts
		scriptDomain->DoKeyEvent(keys, keydown);
	}
}

#pragma unmanaged

#include <Main.h>

// solely for detection for recreation of the game session
PVOID sOldGameFiber = nullptr;

HANDLE hClrThread;
HANDLE hClrWaitEvent;
HANDLE hClrContinueEvent;

// proper synchronization with event objects would cause a deadlock or timeout (for executing longer than 2 seconds)
// in DllMain, so use a bool variable to tell procedures that the asi wants to get freed
// use atomic_bool to avoid unnecessary optimization, with std::memory_order_seq_cst just in case for a xchg instruction
std::atomic_bool sClrThreadRequestedToExit(false);

// A procedure that is supposed to be run in a dedicated thread for so a cached stack limit in .NET runtime won't panic for
// (false) stack overflow that can be caused by running managed code in a custom fiber (with a custom fiber data in other words)
static DWORD ClrThreadProc(LPVOID lparam)
{
	// DllMain gets called before ScriptHookV starts script fibers, so wait until getting signaled by ScriptMain
	WaitForSingleObject(hClrContinueEvent, INFINITE);

	while (!sClrThreadRequestedToExit) {
		ScriptHookVDotNet_ManagedInit();
		sScriptDomainRequestedToReload.store(false);

		// ScriptHookV's script fibers will be executed in the main thread, so we assume this code won't have trouble
		// exiting the loop after letting the main fiber execute
		while (!sScriptDomainRequestedToReload.load() && !sClrThreadRequestedToExit.load()) {
			ScriptHookVDotNet_ManagedTick();
			SetEvent(hClrWaitEvent);
			WaitForSingleObject(hClrContinueEvent, INFINITE);
		}
	}

	return 0;
}

static void ScriptMain()
{
	// We need these info to swap the TLS context of the main thread when necessary to access a lot of game stuff
	// such as a rage::SysMemAllocator instance
	sTlsContextAddrOfGameMainThread.store(GetTlsContext());
	sGameMainThreadId.store(GetCurrentThreadId());

	// ScriptHookV already turned the current thread into a fiber, so can safely retrieve it
	const PVOID initialGameFiber = GetCurrentFiber();
	if (sOldGameFiber != nullptr)
	{
		// The game session is reloaded, so tell our script domain to reload from this fiber
		sScriptDomainRequestedToReload.store(true);
	}
	sOldGameFiber = initialGameFiber;

	bool gameReloaded = false;
	while (!sClrThreadRequestedToExit.load() && !gameReloaded)
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

		SetEvent(hClrContinueEvent);
		// This call blocks the main thread so GtaThread instances (which ysc or external scripts internally rely on)
		// won't be executed except for one for the SHVDN runtime as long as it is executing
		WaitForSingleObject(hClrWaitEvent, INFINITE);
		scriptWait(0);
	}
}

static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
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
		// Avoid unnecessary DLL_THREAD_ATTACH and DLL_THREAD_DETACH notifications
		DisableThreadLibraryCalls(hModule);
		// Register ScriptHookVDotNet native script
		scriptRegister(hModule, ScriptMain);
		// Register handler for keyboard messages
		keyboardHandlerRegister(ScriptKeyboardMessage);

		hClrContinueEvent = CreateEvent(NULL, false, false, NULL);
		hClrWaitEvent = CreateEvent(NULL, false, false, NULL);

		// Create a seperate thread to run CLR so in .NET runtime won't panic for (false) stack overflow by a cached stack
		// limit in .NET runtime
		// ScriptHookV runs script fibers in the main thread, but our managed code needs to run in without a fiber data
		// to avoid (false) stack overflow exceptions
		// The issue that explains usage of fibers causes random .NET runtime crashes when the VEH of .NET runtime is called for
		// .NET exceptions or regular C++ exceptions: https://github.com/scripthookvdotnet/scripthookvdotnet/issues/976
		// The place that may cause the exception: https://github.com/dotnet/coreclr/blob/ef1e2ab328087c61a6878c1e84f4fc5d710aebce/src/vm/excep.cpp#L7763
		// The code for stack space detection: https://github.com/dotnet/coreclr/blob/ef1e2ab328087c61a6878c1e84f4fc5d710aebce/src/vm/threads.cpp#L7912
		hClrThread = CreateThread(NULL, NULL, ClrThreadProc, NULL, NULL, NULL);
		break;
	case DLL_PROCESS_DETACH:
		sClrThreadRequestedToExit.store(true);

		// Gracefully let the CLR thread procedure and the script fiber exit (CloseHandle does not set events)
		// Probably these calls do not wake up ClrThreadProc however
		SetEvent(hClrContinueEvent);
		SetEvent(hClrWaitEvent);

		// Cleanup events and thread handles so the exe can exit
		CloseHandle(hClrContinueEvent);
		CloseHandle(hClrWaitEvent);
		CloseHandle(hClrThread);
		// Unregister ScriptHookVDotNet native script
		scriptUnregister(hModule);
		// Unregister handler for keyboard messages
		keyboardHandlerUnregister(ScriptKeyboardMessage);
		break;
	}

	return TRUE;
}
