#include "pch.h"
#include "main.h"
#pragma once
using namespace std;
typedef void(WINAPI* VoidFunc)();
typedef void(WINAPI* KeyboardFunc)(DWORD keycode, bool keydown, bool ctrl, bool shift, bool alt);

inline VoidFunc CLR_DoInit;
inline VoidFunc CLR_DoUnload;
inline VoidFunc CLR_DoTick;
inline KeyboardFunc CLR_DoKeyboard;
void CLR_Startup();
void CLR_Shutdown();

