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
	static WinForms::Keys ReloadKey = WinForms::Keys::None;
};

bool ManagedInit()
{
	if (!Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		GTA::ScriptDomain::Unload(ScriptHook::Domain);
	}

	auto location = Assembly::GetExecutingAssembly()->Location;
	auto settings = GTA::ScriptSettings::Load(IO::Path::ChangeExtension(location, ".ini"));

	ScriptHook::Domain = GTA::ScriptDomain::Load(IO::Path::Combine(IO::Path::GetDirectoryName(location), settings->GetValue(String::Empty, "ScriptsLocation", "scripts")));
	ScriptHook::ReloadKey = settings->GetValue<WinForms::Keys>(String::Empty, "ReloadKey", WinForms::Keys::Insert);

	if (Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		return false;
	}

	ScriptHook::Domain->Start();

	return true;
}
bool ManagedTick()
{
	if (ScriptHook::Domain->IsKeyPressed(ScriptHook::ReloadKey))
	{
		return false;
	}

	ScriptHook::Domain->DoTick();

	return true;
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
PVOID sMainFib = nullptr, sScriptFib = nullptr;

void ScriptMain()
{
	// Set up fibers
	sGameReloaded = true;
	sMainFib = GetCurrentFiber();

	if (sScriptFib == nullptr)
	{
		const auto callback = [](LPVOID)
		{
			while (ManagedInit())
			{
				sGameReloaded = false;

				// Run main loop
				while (!sGameReloaded && ManagedTick())
				{
					// Switch back to main script fiber used by Script Hook
					SwitchToFiber(sMainFib);
				}
			}
		};

		// Create our own fiber for the common language runtime once
		sScriptFib = CreateFiber(0, callback, nullptr);
	}

	while (true)
	{
		// Yield execution
		scriptWait(0);

		// Switch to our own fiber and wait for it to switch back
		SwitchToFiber(sScriptFib);
	}
}
void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
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
