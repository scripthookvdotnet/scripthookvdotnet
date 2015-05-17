#include "UIContainer.hpp"

namespace GTA
{
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