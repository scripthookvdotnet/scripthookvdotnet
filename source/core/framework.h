#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>
#include <iostream>
#include <string>
#include <assert.h>

static inline std::wstring GetExeDir() {
	TCHAR szExePath[MAX_PATH];
	GetModuleFileName(NULL, szExePath, MAX_PATH);
	std::wstring exePath(szExePath);
	return exePath.substr(0, exePath.rfind('\\'));
}

