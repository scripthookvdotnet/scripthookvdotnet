#pragma once

#include "UIElement.h"
#include "UIRectangle.h"

namespace GTA
{
	public ref class UIContainer : UIElement, UIRectangle
	{

	public:
		UIContainer();
		UIContainer(System::Drawing::Point loc, System::Drawing::Point size);
		UIContainer(System::Drawing::Point loc, System::Drawing::Point size, System::Drawing::Color color);

		void Add(UIElement ^elem);
		virtual void Draw() override;
		virtual void Draw(int xMod, int yMod) override;
			
	private:
		System::Collections::Generic::List<UIElement ^> ^children;
	};
}