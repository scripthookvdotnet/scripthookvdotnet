#include "NativeCaller.h"
#include "ScriptDomain.hpp"

void ScriptMain()
{
	if (!GTA::ScriptDomain::Reload() && System::Object::ReferenceEquals(GTA::ScriptDomain::CurrentDomain, nullptr))
	{
		return;
	}

	GTA::ScriptDomain::CurrentDomain->StartScripts();

	while (true)
	{
		if (GetAsyncKeyState(VK_INSERT) & 0x8000)
		{
			GTA::ScriptDomain::Reload();
			GTA::ScriptDomain::CurrentDomain->StartScripts();
		}

		GTA::ScriptDomain::CurrentDomain->Run();

		scriptWait(0);
	}
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
			break;
		case DLL_PROCESS_DETACH:
			break;
	}

	return TRUE;
}