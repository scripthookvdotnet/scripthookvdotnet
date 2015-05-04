#include "Native.hpp"
#include "UIRectangle.h"

namespace GTA
{
	UIRectangle::UIRectangle()
	{
		this->Color = System::Drawing::Color::Transparent;
		this->size = gcnew System::Drawing::Point(UI::WIDTH, UI::HEIGHT);
		this->loc = gcnew System::Drawing::Point(0, 0);
		this->enabled = true;
	}

	UIRectangle::UIRectangle(System::Drawing::Point loc, System::Drawing::Point size)
	{
		this->Color = System::Drawing::Color::Transparent;
		this->size = size;
		this->loc = loc;
		this->enabled = true;
	}
	UIRectangle::UIRectangle(System::Drawing::Point loc, System::Drawing::Point size, System::Drawing::Color color)
	{
		this->Color = color;
		this->size = size;
		this->loc = loc;
		this->enabled = true;
	}

	void UIRectangle::Draw()
	{
		this->Draw(0, 0);
	}

	void UIRectangle::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
			return;
		float width = ((float)size->X / UI::WIDTH);
		float height = ((float)size->Y / UI::HEIGHT);
		float locX = (((float)loc->X + xMod) / UI::WIDTH) + (((float)this->size->X / 2) / UI::WIDTH);
		float locY = (((float)loc->Y + yMod) / UI::HEIGHT) + (((float)this->size->Y / 2) / UI::HEIGHT);

		Native::Function::Call(Native::Hash::DRAW_RECT, locX, locY, width, height, this->Color.R, this->Color.G, this->Color.B, this->Color.A);
	}

	System::Drawing::Color UIRectangle::Color::get()
	{
		return this->color;
	}

	void UIRectangle::Color::set(System::Drawing::Color color)
	{
		this->color = color;
	}

	bool UIRectangle::Enabled::get()
	{
		return this->enabled;
	}

	void UIRectangle::Enabled::set(bool enabled)
	{
		this->enabled = enabled;
	}

	System::Drawing::Point ^UIRectangle::Size::get()
	{
		return this->size;
	}

	void UIRectangle::Size::set(System::Drawing::Point ^size)
	{
		this->size = size;
	}

	System::Drawing::Point ^UIRectangle::Loc::get()
	{
		return this->loc;
	}

	void UIRectangle::Loc::set(System::Drawing::Point ^loc)
	{
		this->loc = loc;
	}
}