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

#include "ScriptDomain.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"
#include "Matrix.hpp"
#include "Quaternion.hpp"
#include "Vector2.hpp"
#include "Vector3.hpp"
#include "Settings.hpp"

using namespace System;
using namespace System::Reflection;
namespace WinForms = System::Windows::Forms;

ref struct ScriptHook
{
	static GTA::ScriptDomain ^Domain = nullptr;
};

bool ManagedInit()
{
	if (!Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		GTA::ScriptDomain::Unload(ScriptHook::Domain);
	}

	auto settings = GTA::ScriptSettings::Load(IO::Path::ChangeExtension(Assembly::GetExecutingAssembly()->Location, ".ini"));
	ScriptHook::Domain = GTA::ScriptDomain::Load(settings->GetValue<String ^>(String::Empty, "ScriptsLocation", "scripts"));

	if (!Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		ScriptHook::Domain->Start();

		return true;
	}

	return false;
}
void ManagedTick()
{
	ScriptHook::Domain->DoTick();
}
void ManagedKeyboardMessage(int key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
{
	if (Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		return;
	}

	ScriptHook::Domain->DoKeyboardMessage(static_cast<WinForms::Keys>(key), status, statusCtrl, statusShift, statusAlt);
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
			while (ManagedInit())
			{
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
	ManagedKeyboardMessage(static_cast<int>(key), isUpNow == FALSE, (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0, (GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0, isWithAlt != FALSE);
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
