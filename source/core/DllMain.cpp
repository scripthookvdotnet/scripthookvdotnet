// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "hosting.h"
#include "main.h"
#include "exports.h"
bool GameReloaded;

void SetTls(LPVOID tls)
{
	__writegsqword(0x58, (DWORD64)tls);
}
LPVOID GetTls()
{
	return (LPVOID)__readgsqword(0x58);
}

void Startup() {
	// Find asi path
	assert(GetModuleFileName(AsiModule, AsiFilePath, sizeof(AsiFilePath) / sizeof(TCHAR)) != ERROR_INSUFFICIENT_BUFFER);

	// Set up well-known properties
	SetEnvironmentVariable(L"SHVDN_ASI_PATH", AsiFilePath);
	SetPtr(KEY_ASI_GETTLSFUNC, GetTls);
	SetPtr(KEY_ASI_SETTLSFUNC, SetTls);
	SetPtr(KEY_ASI_HMODULE, AsiModule);
	SetPtr(KEY_ASI_PTRRELOADED, &GameReloaded);
	CLR_Startup();
}
void OnKeyboard(DWORD key, WORD repeats, BYTE scanCode, BOOL isExtended, BOOL isWithAlt, BOOL wasDownBefore, BOOL isUpNow) {
	CLR_DoKeyboard(
		key,
		!isUpNow,
		(GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0,
		(GetAsyncKeyState(VK_SHIFT) & 0x8000) != 0,
		isWithAlt != FALSE);
}
void ScriptMain() {
	while (true)
	{
		GameReloaded = false;
		CLR_DoInit();

		while (!GameReloaded)
		{
			CLR_DoTick();
			scriptWait(0);
		}
	}
}
BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		AsiModule = hModule;
		DisableThreadLibraryCalls(hModule);
		scriptRegister(hModule, ScriptMain);
		keyboardHandlerRegister(OnKeyboard);
		Startup();
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		// This does not allow proper cleanup of ScriptDomain, but we'll unload leftover domains during next init
		CLR_Shutdown();
		keyboardHandlerUnregister(OnKeyboard);
		scriptUnregister(AsiModule);
		break;
	}
	return TRUE;
}

