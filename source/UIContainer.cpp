#include "UI.h"
#include "UIContainer.h"

namespace GTA
{
	UIContainer::UIContainer() : UIRectangle()
	{
		this->children = gcnew System::Collections::Generic::List<UIElement ^>();
	}

	UIContainer::UIContainer(System::Drawing::Point loc, System::Drawing::Point scale) : UIRectangle(loc, scale)
	{
		this->children = gcnew System::Collections::Generic::List<UIElement ^>();
	}
	UIContainer::UIContainer(System::Drawing::Point loc, System::Drawing::Point scale, System::Drawing::Color color) : UIRectangle(loc, scale, color)
	{
		this->children = gcnew System::Collections::Generic::List<UIElement ^>();
	}

	void UIContainer::Add(UIElement ^elem)
	{
		this->children->Add(elem);
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
		for each (UIElement ^elem in this->children)
			elem->Draw(xMod + UIRectangle::Loc->X, yMod + UIRectangle::Loc->Y);
	}
}