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

#include "NativeCaller.h"
#include "ScriptDomain.hpp"

ref struct ScriptGlobals
{
	static GTA::ScriptDomain ^Domain = nullptr;
};

void ScriptMain()
{
	while (true)
	{
		ScriptGlobals::Domain = GTA::ScriptDomain::Load(System::IO::Path::Combine(System::IO::Path::GetDirectoryName(System::Reflection::Assembly::GetExecutingAssembly()->Location), "scripts"));

		if (System::Object::ReferenceEquals(ScriptGlobals::Domain, nullptr))
		{
			return;
		}

		ScriptGlobals::Domain->Start();

		while (true)
		{
			if (GetAsyncKeyState(VK_INSERT) & 0x8000)
			{
				GTA::ScriptDomain::Unload(ScriptGlobals::Domain);

				ScriptGlobals::Domain = nullptr;
				break;
			}

			ScriptGlobals::Domain->DoTick();

			scriptWait(0);
		}
	}
}
void ScriptKeyboardMessage(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow)
{
	if (key >= 255 || ScriptGlobals::Domain == nullptr)
	{
		return;
	}

	ScriptGlobals::Domain->DoKeyboardMessage(static_cast<System::Windows::Forms::Keys>(key), isUpNow == FALSE, (GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0, (GetAsyncKeyState(VK_MENU) & 0x8000) != 0, isWithAlt != FALSE);
}

#pragma unmanaged

#include <Windows.h>

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
			scriptUnregister(&ScriptMain);
			keyboardHandlerUnregister(&ScriptKeyboardMessage);
			break;
	}

	return TRUE;
}