#include "NativeCaller.h"

void ScriptMain()
{

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