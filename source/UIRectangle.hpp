#pragma once

#include "UIElement.hpp"

namespace GTA
{
	public ref class UIRectangle : UIElement
	{
	public:
		UIRectangle();
		UIRectangle(System::Drawing::Point location, System::Drawing::Size size);
		UIRectangle(System::Drawing::Point location, System::Drawing::Size size, System::Drawing::Color color);

		virtual property bool Enabled
		{
			void set(bool value);
			bool get();
		}
		virtual property System::Drawing::Color Color
		{
			void set(System::Drawing::Color value);
			System::Drawing::Color get();
		}
		virtual property System::Drawing::Point Location
		{
			void set(System::Drawing::Point value);
			System::Drawing::Point get();
		}
		property System::Drawing::Size Size
		{
			void set(System::Drawing::Size value);
			System::Drawing::Size get();
		}

		virtual void Draw();
		virtual void Draw(int xMod, int yMod);

	private:
		bool mEnabled;
		System::Drawing::Color mColor;
		System::Drawing::Point mLocation;
		System::Drawing::Size mSize;
	};
}