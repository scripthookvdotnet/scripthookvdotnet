#pragma once

#include "UI.h"

namespace GTA
{
	public interface class UIElement
	{
		void Draw();
		void Draw(int xMod, int yMod);

		property System::Drawing::Color Color
		{
			void set(System::Drawing::Color value);
			System::Drawing::Color get();
		}
		property bool Enabled
		{
			void set(bool value);
			bool get();
		}
		
		property System::Drawing::Point ^Loc
		{
			void set(System::Drawing::Point ^value);
			System::Drawing::Point ^get();
		}
	};
}