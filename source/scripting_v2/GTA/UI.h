/**
 * Copyright (C) 2015 crosire & contributors
 * License: https://github.com/crosire/scripthookvdotnet#license
 */

#pragma once

#include "HudComponent.h"
#include "Notification.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;

namespace GTA
{
	public ref class UI sealed abstract
	{
	public:
		// These two definitions have a 'modopt(System.Runtime.Compilerservices.IsConst)', which only C++/CLI can generate
		// Therefore need to compile this whole class with C++/CLI and cannot use C# here
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		static Notification ^Notify(String ^message);
		static Notification ^Notify(String ^message, bool blinking);

		static void ShowSubtitle(String ^message);
		static void ShowSubtitle(String ^message, int duration);

		static void ShowHelpMessage(String ^message);
		static void ShowHelpMessage(String ^message, bool sound);
		static void ShowHelpMessage(String ^message, int duration);
		static void ShowHelpMessage(String ^message, int duration, bool sound);

		static bool IsHudComponentActive(HudComponent component);
		static void ShowHudComponentThisFrame(HudComponent component);
		static void HideHudComponentThisFrame(HudComponent component);

		//static Point WorldToScreen(Math::Vector3 position);

		static void DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size);
		static void DrawTexture(String ^filename, int index, int level, int time, Point pos, Size size, float rotation, Color color);
		static void DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color);
		static void DrawTexture(String ^filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color, float aspectRatio);

	internal:
		static Dictionary<String ^, int> ^_textures = gcnew Dictionary<String ^, int>();
	};
}
