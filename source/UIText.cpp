#include "UIText.hpp"
#include "Native.hpp"

namespace GTA
{
	UIText::UIText(System::String ^text, System::Drawing::Point location, float size, System::Drawing::Color color, int font, bool centered) : mEnabled(true), mColor(color), mLocation(location), mText(text), mFont(font), mSize(size), mCentered(centered)
	{
	}

	bool UIText::Enabled::get()
	{
		return this->mEnabled;
	}
	void UIText::Enabled::set(bool enabled)
	{
		this->mEnabled = enabled;
	}
	System::Drawing::Color UIText::Color::get()
	{
		return this->mColor;
	}
	void UIText::Color::set(System::Drawing::Color color)
	{
		this->mColor = color;
	}
	System::Drawing::Point UIText::Location::get()
	{
		return this->mLocation;
	}
	void UIText::Location::set(System::Drawing::Point loc)
	{
		this->mLocation = loc;
	}
	System::String ^UIText::Text::get()
	{
		return this->mText;
	}
	void UIText::Text::set(System::String ^text)
	{
		this->mText = text;
	}
	int UIText::Font::get()
	{
		return this->mFont;
	}
	void UIText::Font::set(int font)
	{
		this->mFont = font;
	}
	float UIText::Size::get()
	{
		return this->mSize;
	}
	void UIText::Size::set(float size)
	{
		this->mSize = size;
	}
	bool UIText::Centered::get()
	{
		return this->mCentered;
	}
	void UIText::Centered::set(bool center)
	{
		this->mCentered = center;
	}

	void UIText::Draw()
	{
		this->Draw(0, 0);
	}
	void UIText::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
		{
			return;
		}

		const float x = (static_cast<float>(this->mLocation.X) + xMod) / UI::WIDTH;
		const float y = (static_cast<float>(this->mLocation.Y) + yMod) / UI::HEIGHT;

		Native::Function::Call(Native::Hash::SET_TEXT_FONT, this->mFont);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, this->mSize, this->mSize);
		Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, this->mColor.R, this->mColor.G, this->mColor.B, this->mColor.A);
		Native::Function::Call(Native::Hash::SET_TEXT_CENTRE, this->mCentered ? 1 : 0);
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, this->mText);
		Native::Function::Call(Native::Hash::_DRAW_TEXT, x, y);
	}
}