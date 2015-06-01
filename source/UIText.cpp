#include "UIText.hpp"
#include "Native.hpp"

namespace GTA
{
	UIText::UIText(System::String ^caption, System::Drawing::Point position, float scale)
	{
		this->Enabled = true;
		this->Caption = caption;
		this->Position = position;
		this->Scale = scale;
		this->Color = System::Drawing::Color::WhiteSmoke;
		this->Font = GTA::Font::ChaletLondon;
		this->Centered = false;
	}
	UIText::UIText(System::String ^caption, System::Drawing::Point position, float scale, System::Drawing::Color color)
	{
		this->Enabled = true;
		this->Caption = caption;
		this->Position = position;
		this->Scale = scale;
		this->Color = color;
		this->Font = GTA::Font::ChaletLondon;
		this->Centered = false;
	}
	UIText::UIText(System::String ^caption, System::Drawing::Point position, float scale, System::Drawing::Color color, GTA::Font font, bool centered)
	{
		this->Enabled = true;
		this->Caption = caption;
		this->Position = position;
		this->Scale = scale;
		this->Color = color;
		this->Font = font;
		this->Centered = centered;
	}

	void UIText::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void UIText::Draw(System::Drawing::Size offset)
	{
		if (!this->Enabled)
		{
			return;
		}

		const float x = (static_cast<float>(this->Position.X) + offset.Width) / UI::WIDTH;
		const float y = (static_cast<float>(this->Position.Y) + offset.Height) / UI::HEIGHT;

		Native::Function::Call(Native::Hash::SET_TEXT_FONT, (int)this->Font);
		Native::Function::Call(Native::Hash::SET_TEXT_SCALE, this->Scale, this->Scale);
		Native::Function::Call(Native::Hash::SET_TEXT_COLOUR, this->Color.R, this->Color.G, this->Color.B, this->Color.A);
		Native::Function::Call(Native::Hash::SET_TEXT_CENTRE, this->Centered ? 1 : 0);
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, this->Caption);
		Native::Function::Call(Native::Hash::_DRAW_TEXT, x, y);
	}
}