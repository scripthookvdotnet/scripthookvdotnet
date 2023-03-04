#include "pch.h"
#pragma once

#define DllExport extern "C" __declspec( dllexport ) inline
#include <unordered_map>
#include <mutex>
#define LOCK(mtx) std::scoped_lock l(mtx)
using namespace std;

const LPCSTR KEY_CLR_INITFUNC = "SHVDN.CLR.InitFuncAddr";
const LPCSTR KEY_CLR_TICKFUNC = "SHVDN.CLR.TickFuncAddr";
const LPCSTR KEY_CLR_KBHFUNC = "SHVDN.CLR.KeyboardHandlerFuncAddr";
const LPCSTR KEY_ASI_GETTLSFUNC = "SHVDN.ASI.GetTlsFuncAddr";
const LPCSTR KEY_ASI_SETTLSFUNC = "SHVDN.ASI.SetTlsFuncAddr";
const LPCSTR KEY_ASI_HMODULE = "SHVDN.ASI.ModuleHandle";
const LPCSTR KEY_ASI_PTRRELOADED = "SHVDN.ASI.PtrGameReloaded";


inline unordered_map<string, LPVOID> PtrMap;
inline mutex PtrMapMutex;
inline HMODULE AsiModule;
inline TCHAR AsiFilePath[MAX_PATH];
DllExport LPVOID GetPtr(LPCSTR key) {
	LOCK(PtrMapMutex);
	return PtrMap[key];
}
DllExport void SetPtr(LPCSTR key, LPVOID value) {
	LOCK(PtrMapMutex);
	PtrMap[key] = value;
}
