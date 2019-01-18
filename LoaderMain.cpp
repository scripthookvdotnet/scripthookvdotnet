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

using namespace System;
using namespace System::Reflection;
using namespace System::Linq::Expressions;
namespace WinForms = System::Windows::Forms;

#pragma region Implicit PInvoke Definitions
[Runtime::InteropServices::DllImport("kernel32", CharSet = Runtime::InteropServices::CharSet::Unicode, SetLastError = false)]
extern int DeleteFile(String ^name);
#pragma endregion

delegate void KeyboardMethodDelegate(WinForms::Keys key, bool status, bool statusCtrl, bool statusShift, bool statusAlt);

ref struct LoaderData
{
	static Collections::Generic::List<Func<bool> ^> InitMethods;
	static Collections::Generic::List<Action ^> TickMethods;
	static Collections::Generic::List<KeyboardMethodDelegate ^> KeyboardMethods;
};

void ManagedInit()
{
	LoaderData::InitMethods.Clear();
	LoaderData::TickMethods.Clear();
	LoaderData::KeyboardMethods.Clear();

	String ^directory = IO::Path::GetDirectoryName(Assembly::GetExecutingAssembly()->Location);

	for each (String ^filename in IO::Directory::EnumerateFiles(directory, "ScriptHookVDotNet*.dll"))
	{
		Assembly ^assembly;

		try
		{
			String ^path = IO::Path::Combine(directory, filename);

			// Unblock file if it was downloaded from a network location
			DeleteFile(path + ":Zone.Identifier");

			assembly = Assembly::LoadFrom(path);
		}
		catch (Exception ^ex)
		{
			WinForms::MessageBox::Show(String::Concat("FATAL: Unable to load '", filename, "' due to the following exception:\n\n", ex->ToString()), "Community Script Hook V .NET");
			continue;
		}

		System::Type ^main = assembly->GetType("ScriptHookVDotNet");

		if (main && main->IsAbstract)
		{
			auto initMethod = safe_cast<Func<bool> ^>(main->GetMethod("Init", BindingFlags::Public | BindingFlags::Static)->CreateDelegate(Func<bool>::typeid));
			LoaderData::InitMethods.Add(initMethod);
			auto tickMethod = safe_cast<Action ^>(main->GetMethod("Tick", BindingFlags::Public | BindingFlags::Static)->CreateDelegate(Action::typeid));
			LoaderData::TickMethods.Add(tickMethod);
			auto keyboardMessageMethod = safe_cast<KeyboardMethodDelegate ^>(main->GetMethod("KeyboardMessage", BindingFlags::Public | BindingFlags::Static)->CreateDelegate(KeyboardMethodDelegate::typeid));
			LoaderData::KeyboardMethods.Add(keyboardMessageMethod);
		}
	}

	for each (Func<bool> ^Init in LoaderData::InitMethods)
	{
		Init();
	}
}
void ManagedTick()
{
	for each (Action ^Tick in LoaderData::TickMethods)
	{
		Tick();
	}
}
void ManagedKeyboardMessage(unsigned long key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
{
	for each (KeyboardMethodDelegate ^KeyboardMethod in LoaderData::KeyboardMethods)
	{
		KeyboardMethod(safe_cast<WinForms::Keys>(key), status, statusCtrl, statusShift, statusAlt);
	}
}

#pragma unmanaged

#include <Main.h>
#include <Windows.h>

bool sGameReloaded = false;
PVOID sMainFib = nullptr;
PVOID sScriptFib = nullptr;

static void ScriptMain()
{
	sGameReloaded = true;

	// ScriptHookV already turned the current thread into a fiber, so we can safely retrieve it.
	sMainFib = GetCurrentFiber();

	// Check if our CLR fiber already exists. It should be created only once for the entire lifetime of the game process.
	if (sScriptFib == nullptr)
	{
		const LPFIBER_START_ROUTINE FiberMain = [](LPVOID lpFiberParameter) {
			while (true)
			{
				ManagedInit();

				sGameReloaded = false;

				// If the game is reloaded, ScriptHookV will call the script main function again.
				// This will set the global 'sGameReloaded' variable to 'true' and on the next fiber switch to our CLR fiber, run into this condition, therefore exiting the inner loop and re-initialize.
				while (!sGameReloaded)
				{
					ManagedTick();

					// Switch back to main script fiber used by ScriptHookV.
					// Code continues from here the next time the loop below switches back to our CLR fiber.
					SwitchToFiber(sMainFib);
				}
			}
		};

		// Create our own fiber for the common language runtime, aka CLR, once.
		// This is done because ScriptHookV switches its internal fibers sometimes, which would corrupt the CLR stack.
		sScriptFib = CreateFiber(0, FiberMain, nullptr);
	}

	while (true)
	{
		// Yield execution and give it back to ScriptHookV.
		scriptWait(0);

		// Switch to our CLR fiber and wait for it to switch back.
		SwitchToFiber(sScriptFib);
	}
}
static void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	ManagedKeyboardMessage(key, isUpNow == FALSE, (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0, (GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0, isWithAlt != FALSE);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
			DisableThreadLibraryCalls(hModule);
			scriptRegister(hModule, &ScriptMain);
			keyboardHandlerRegister(&ScriptKeyboardMessage);
			break;
		case DLL_PROCESS_DETACH:
			DeleteFiber(sScriptFib);
			scriptUnregister(hModule);
			keyboardHandlerUnregister(&ScriptKeyboardMessage);
			break;
	}

	return TRUE;
}
