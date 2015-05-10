/**
 * Copyright (C) 2015 Crosire
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

#include "Main.h"
#include "ScriptDomain.hpp"

ref struct ScriptHook
{
	static GTA::ScriptDomain ^Domain = nullptr;
};

bool ManagedInit()
{
	if (!System::Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		GTA::ScriptDomain::Unload(ScriptHook::Domain);
	}

	ScriptHook::Domain = GTA::ScriptDomain::Load(System::IO::Path::Combine(System::IO::Path::GetDirectoryName(System::Reflection::Assembly::GetExecutingAssembly()->Location), "scripts"));

	if (!System::Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		ScriptHook::Domain->Start();

		return true;
	}
	else
	{
		return false;
	}
}
bool ManagedTick()
{
	if (GetAsyncKeyState(VK_INSERT) & 0x8000)
	{
		return false;
	}

	ScriptHook::Domain->DoTick();

	return true;
}
void ManagedKeyboardMessage(int key, bool status, bool statusCtrl, bool statusShift, bool statusAlt)
{
	if (System::Object::ReferenceEquals(ScriptHook::Domain, nullptr))
	{
		return;
	}

	ScriptHook::Domain->DoKeyboardMessage(static_cast<System::Windows::Forms::Keys>(key), status, statusCtrl, statusShift, statusAlt);
}

#pragma unmanaged

#include <Windows.h>

void ScriptMainLoop()
{
	while (ManagedInit())
	{
		while (ManagedTick())
		{
			scriptWait(0);
		}
	}
}
void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	ManagedKeyboardMessage(static_cast<int>(key), isUpNow == FALSE, (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0, (GetAsyncKeyState(VK_MENU) & 0x8000) != 0, isWithAlt != FALSE);
}

BOOL WINAPI DllMain(HMODULE hModule, DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
			DisableThreadLibraryCalls(hModule);
			scriptRegister(hModule, &ScriptMainLoop);
			keyboardHandlerRegister(&ScriptKeyboardMessage);
			break;
		case DLL_PROCESS_DETACH:
			scriptUnregister(&ScriptMainLoop);
			keyboardHandlerUnregister(&ScriptKeyboardMessage);
			break;
	}

	return TRUE;
}