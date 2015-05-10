#pragma once

#include "UI.hpp"

namespace GTA
{
	public interface class UIElement
	{
		void Draw();
		void Draw(System::Drawing::Size offset);

		property bool Enabled
		{
			bool get();
			void set(bool value);
		}
		property System::Drawing::Point Position
		{
			System::Drawing::Point get();
			void set(System::Drawing::Point value);
		}
		property System::Drawing::Color Color
		{
			System::Drawing::Color get();
			void set(System::Drawing::Color value);
		}
	};
}