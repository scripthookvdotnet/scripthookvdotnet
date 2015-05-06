#include "UIRectangle.hpp"
#include "Native.hpp"
#include "Viewport.hpp"

namespace GTA
{
	UIRectangle::UIRectangle() : mEnabled(true), mColor(System::Drawing::Color::Transparent), mLocation(0, 0), mSize(Viewport::WIDTH, Viewport::HEIGHT)
	{
	}
	UIRectangle::UIRectangle(System::Drawing::Point location, System::Drawing::Size size) : mEnabled(true), mColor(System::Drawing::Color::Transparent), mLocation(location), mSize(size)
	{
	}
	UIRectangle::UIRectangle(System::Drawing::Point location, System::Drawing::Size size, System::Drawing::Color color) : mEnabled(true), mColor(color), mLocation(location), mSize(size)
	{
	}

	bool UIRectangle::Enabled::get()
	{
		return this->mEnabled;
	}
	void UIRectangle::Enabled::set(bool enabled)
	{
		this->mEnabled = enabled;
	}
	System::Drawing::Color UIRectangle::Color::get()
	{
		return this->mColor;
	}
	void UIRectangle::Color::set(System::Drawing::Color color)
	{
		this->mColor = color;
	}
	System::Drawing::Point UIRectangle::Location::get()
	{
		return this->mLocation;
	}
	void UIRectangle::Location::set(System::Drawing::Point loc)
	{
		this->mLocation = loc;
	}
	System::Drawing::Size UIRectangle::Size::get()
	{
		return this->mSize;
	}
	void UIRectangle::Size::set(System::Drawing::Size size)
	{
		this->mSize = size;
	}

	void UIRectangle::Draw()
	{
		this->Draw(0, 0);
	}
	void UIRectangle::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
		{
			return;
		}

		const float w = static_cast<float>(this->mSize.Width) / Viewport::WIDTH;
		const float h = static_cast<float>(this->mSize.Height) / Viewport::HEIGHT;
		const float x = ((static_cast<float>(this->mLocation.X) + xMod) / Viewport::WIDTH) + w * 0.5f;
		const float y = ((static_cast<float>(this->mLocation.Y) + yMod) / Viewport::HEIGHT) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, this->mColor.R, this->mColor.G, this->mColor.B, this->mColor.A);
	}
}