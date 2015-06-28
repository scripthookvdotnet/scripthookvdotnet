#include "UIRectangle.hpp"
#include "Native.hpp"

namespace GTA
{
	UIRectangle::UIRectangle()
	{
		this->Enabled = true;
		this->Position = System::Drawing::Point();
		this->Size = System::Drawing::Size(UI::Width, UI::Height);

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

		const float w = static_cast<float>(this->Size.Width) / UI::Width;
		const float h = static_cast<float>(this->Size.Height) / UI::Height;
		const float x = ((static_cast<float>(this->Position.X) + offset.Width) / UI::Width) + w * 0.5f;
		const float y = ((static_cast<float>(this->Position.Y) + offset.Height) / UI::Height) + h * 0.5f;

		Native::Function::Call(Native::Hash::DRAW_RECT, x, y, w, h, this->Color.R, this->Color.G, this->Color.B, this->Color.A);
	}
}