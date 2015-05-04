#include "Native.hpp"
#include "UI.h"
#include "UIText.h"

namespace GTA
{
	UIText::UIText(System::String ^text, System::Drawing::Point loc, float size, System::Drawing::Color color, int font, bool center)
	{
		this->text = text;
		this->loc = loc;
		this->size = size;
		this->color = color;
		this->font = font;
		this->center = center;
		this->enabled = true;
	}

	System::Drawing::Color UIText::Color::get()
	{
		return this->color;
	}
	void UIText::Color::set(System::Drawing::Color color)
	{
		this->color = color;
	}
	bool UIText::Enabled::get()
	{
		return this->enabled;
	}
	void UIText::Enabled::set(bool enabled)
	{
		this->enabled = enabled;
	}
	System::Drawing::Point ^UIText::Loc::get()
	{
		return this->loc;
	}
	void UIText::Loc::set(System::Drawing::Point ^loc)
	{
		this->loc = loc;
	}
	System::String ^UIText::Text::get()
	{
		return this->text;
	}
	void UIText::Text::set(System::String ^text)
	{
		this->text = text;
	}
	int UIText::Font::get()
	{
		return this->font;
	}
	void UIText::Font::set(int font)
	{
		this->font = font;
	}
	float UIText::Size::get()
	{
		return this->size;
	}
	void UIText::Size::set(float size)
	{
		this->size = size;
	}
	bool UIText::Center::get()
	{
		return this->center;
	}
	void UIText::Center::set(bool center)
	{
		this->center = center;
	}

	void UIText::Draw()
	{
		this->Draw(0, 0);
	}
	void UIText::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
			return;
		float locX = (((float)loc->X + xMod) / UI::WIDTH);
		float locY = (((float)loc->Y + yMod) / UI::HEIGHT);

		Native::Function::Call(Native::Hash::SET_TEXT_FONT, font);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, size, size);
		Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
		Native::Function::Call(Native::Hash::SET_TEXT_CENTRE, center ? 1 : 0);
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, text);
		Native::Function::Call(Native::Hash::_DRAW_TEXT, locX, locY);
	}
}