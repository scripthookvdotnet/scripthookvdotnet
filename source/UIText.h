#pragma once

#include "Vector3.hpp"
#include "Vector2.hpp"
#include "Ui.h"
#include "UiElement.h"

namespace GTA
{
	public ref class UIText : UIElement
	{
	public:
		UIText(System::String ^text, System::Drawing::Point loc, float size, System::Drawing::Color color, int font, bool center);

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
		property System::String ^Text
		{
			void set(System::String ^value);
			System::String ^get();
		}
		property int Font
		{
			void set(int value);
			int get();
		}
		property float Size
		{
			void set(float value);
			float get();
		}
		property bool Center
		{
			void set(bool value);
			bool get();
		}

		virtual void Draw();
		virtual void Draw(int xMod, int yMod);

	private:
		System::String ^text;
		int font;
		float size;
		bool center;

		System::Drawing::Point ^loc;
		System::Drawing::Color color;
		bool enabled;
	};
}