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

		property System::Collections::Generic::List<UIElement ^> ^Items
		{
			System::Collections::Generic::List<UIElement ^> ^get();
			void set(System::Collections::Generic::List<UIElement ^> ^value);
		}

		virtual void Draw() override;
		virtual void Draw(int xMod, int yMod) override;
			
	private:
		System::Collections::Generic::List<UIElement ^> ^items;
	};
}