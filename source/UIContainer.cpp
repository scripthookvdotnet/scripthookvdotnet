#include "UIContainer.hpp"

namespace GTA
{
	UIContainer::UIContainer() : UIRectangle()
	{
		this->items = gcnew System::Collections::Generic::List<UIElement ^>();
	}

	UIContainer::UIContainer(System::Drawing::Point loc, System::Drawing::Point scale) : UIRectangle(loc, scale)
	{
		this->items = gcnew System::Collections::Generic::List<UIElement ^>();
	}
	UIContainer::UIContainer(System::Drawing::Point loc, System::Drawing::Point scale, System::Drawing::Color color) : UIRectangle(loc, scale, color)
	{
		this->items = gcnew System::Collections::Generic::List<UIElement ^>();
	}

	System::Collections::Generic::List<UIElement ^> ^UIContainer::Items::get()
	{
		return this->items;
	}
	void UIContainer::Items::set(System::Collections::Generic::List<UIElement ^> ^items)
	{
		this->items = items;
	}

	void UIContainer::Draw()
	{
		this->Draw(0, 0);
	}
	void UIContainer::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
			return;
		UIRectangle::Draw(xMod, yMod);
		for each (UIElement ^elem in this->items)
			elem->Draw(xMod + UIRectangle::Loc->X, yMod + UIRectangle::Loc->Y);
	}
}