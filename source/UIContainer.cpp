#include "UIContainer.hpp"

namespace GTA
{
	UIContainer::UIContainer() : UIRectangle(), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(System::Drawing::Point location, System::Drawing::Size size) : UIRectangle(location, size), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}
	UIContainer::UIContainer(System::Drawing::Point location, System::Drawing::Size size, System::Drawing::Color color) : UIRectangle(location, size, color), mItems(gcnew System::Collections::Generic::List<UIElement ^>())
	{
	}

	System::Collections::Generic::List<UIElement ^> ^UIContainer::Items::get()
	{
		return this->mItems;
	}
	void UIContainer::Items::set(System::Collections::Generic::List<UIElement ^> ^items)
	{
		this->mItems = items;
	}

	void UIContainer::Draw()
	{
		this->Draw(0, 0);
	}
	void UIContainer::Draw(int xMod, int yMod)
	{
		if (!this->Enabled)
		{
			return;
		}

		UIRectangle::Draw(xMod, yMod);

		for each (UIElement ^item in this->mItems)
		{
			item->Draw(xMod + UIRectangle::Location.X, yMod + UIRectangle::Location.Y);
		}
	}
}