#pragma once

#include "UI.hpp"
#include "UIElement.hpp"

namespace GTA
{
	public ref class UIRectangle : UIElement
	{
	public:
		UIRectangle();
		UIRectangle(System::Drawing::Point loc, System::Drawing::Point size);
		UIRectangle(System::Drawing::Point loc, System::Drawing::Point size, System::Drawing::Color color);

		virtual property System::Drawing::Color Color
		{
			void set(System::Drawing::Color value);
			System::Drawing::Color get();
		}
		virtual property bool Enabled
		{
			void set(bool value);
			bool get();
		}
		virtual property System::Drawing::Point ^Loc
		{
			void set(System::Drawing::Point ^value);
			System::Drawing::Point ^get();
		}
		property System::Drawing::Point ^Size
		{
			void set(System::Drawing::Point ^value);
			System::Drawing::Point ^get();
		}

		virtual void Draw();
		virtual void Draw(int xMod, int yMod);

	private:
		System::Drawing::Point ^loc;
		System::Drawing::Point ^size;
		System::Drawing::Color color;
		bool enabled;
	};
}