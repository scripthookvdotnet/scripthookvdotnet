#pragma once

#include "Control.hpp"

namespace GTA
{
	public ref class Controls sealed abstract
	{
	public:
		static bool IsDisabledControlPressed(int index, Control control);
		static bool IsDisabledControlJustPressed(int index, Control control);
		static bool IsDisabledControlJustReleased(int index, Control control);

		static bool IsControlEnabled(int index, Control control);
		static bool IsControlPressed(int index, Control control);
		static bool IsControlJustPressed(int index, Control control);
		static bool IsControlJustReleased(int index, Control control);

		static bool IsLookInverted();

		static void EnableControlThisFrame(int index, Control control);
		static void DisableControlThisFrame(int index, Control control);

		static void DisableAllControlsThisFrame(int index);
		static void EnableAllControlsThisFrame(int index);

		static float GetDisabledControlNormal(int index, Control control);
		static float GetControlNormal(int index, Control control);
		static int GetControlValue(int index, Control control);

		static void SetControlNormal(int index, Control control, float value);
	};
}