#pragma once

#include "UIElement.hpp"

namespace GTA
{
	public ref class UIText : public UIElement
	{
	public:
		UIText(System::String ^caption, System::Drawing::Point position, float scale);
		UIText(System::String ^caption, System::Drawing::Point position, float scale, System::Drawing::Color color);
		UIText(System::String ^caption, System::Drawing::Point position, float scale, System::Drawing::Color color, Font font, bool centered);

		virtual property bool Enabled;
		virtual property System::Drawing::Point Position;
		virtual property System::Drawing::Color Color;
		property System::String ^Caption;
		property Font Font;
		property float Scale;
		property bool Centered;

		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
	};
}