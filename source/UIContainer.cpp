#include "UIContainer.hpp"

namespace GTA
{
	UIContainer::UIContainer() : UIRectangle()
	{
	}
	UIContainer::UIContainer(System::Drawing::Point position, System::Drawing::Size size) : UIRectangle(position, size)
	{
	}
	UIContainer::UIContainer(System::Drawing::Point position, System::Drawing::Size size, System::Drawing::Color color) : UIRectangle(position, size, color)
	{
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