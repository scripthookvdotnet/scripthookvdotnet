#include "UIElement.hpp"
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

	UIRectangle::UIRectangle()
	{
		this->Enabled = true;
		this->Position = System::Drawing::Point();
		this->Size = System::Drawing::Size(UI::WIDTH, UI::HEIGHT);

		this->Color = System::Drawing::Color::Transparent;
	}
	UIRectangle::UIRectangle(System::Drawing::Point position, System::Drawing::Size size)
	{
		this->Enabled = true;
		this->Position = position;
		this->Size = size;
		this->Color = System::Drawing::Color::Transparent;
	}
	UIRectangle::UIRectangle(System::Drawing::Point position, System::Drawing::Size size, System::Drawing::Color color)
	{
		this->Enabled = true;
		this->Position = position;
		this->Size = size;
		this->Color = color;
	}

	void UIRectangle::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void UIRectangle::Draw(System::Drawing::Size offset)
	{
		if (!this->Enabled)
		{
			return;
		}

		const float w = static_cast<float>(this->Size.Width) / UI::WIDTH;
		const float h = static_cast<float>(this->Size.Height) / UI::HEIGHT;
		const float x = ((static_cast<float>(this->Position.X) + offset.Width) / UI::WIDTH) + w * 0.5f;
		const float y = ((static_cast<float>(this->Position.Y) + offset.Height) / UI::HEIGHT) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, this->Color.R, this->Color.G, this->Color.B, this->Color.A);
	}

	UIContainer::UIContainer() : UIRectangle(), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(System::Drawing::Point position, System::Drawing::Size size) : UIRectangle(position, size), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(System::Drawing::Point position, System::Drawing::Size size, System::Drawing::Color color) : UIRectangle(position, size, color), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}

	System::Collections::Generic::List<UIElement ^> ^UIContainer::Items::get()
	{
		return this->mItems;
	}
	void UIContainer::Items::set(System::Collections::Generic::List<UIElement ^> ^value)
	{
		this->mItems = value;
	}

	void UIContainer::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void UIContainer::Draw(System::Drawing::Size offset)
	{
		if (!this->Enabled)
		{
			return;
		}

		UIRectangle::Draw(offset);

		for each (UIElement ^item in this->Items)
		{
			item->Draw(static_cast<System::Drawing::Size>(UIRectangle::Position + offset));
		}
	}
}