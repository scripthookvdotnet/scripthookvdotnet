#pragma once

#include "UIElement.hpp"

namespace GTA
{
	public ref class UIRectangle : public UIElement
	{
	public:
		UIRectangle();
		UIRectangle(System::Drawing::Point position, System::Drawing::Size size);
		UIRectangle(System::Drawing::Point position, System::Drawing::Size size, System::Drawing::Color color);

		virtual property bool Enabled;
		virtual property System::Drawing::Point Position;
		property System::Drawing::Size Size;
		virtual property System::Drawing::Color Color;

		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
	};
}