#pragma once

#include "UIElement.hpp"
#include "UIRectangle.hpp"

namespace GTA
{
	public ref class UIContainer : UIElement, UIRectangle
	{

	public:
		UIContainer();
		UIContainer(System::Drawing::Point location, System::Drawing::Size size);
		UIContainer(System::Drawing::Point location, System::Drawing::Size size, System::Drawing::Color color);

		property System::Collections::Generic::List<UIElement ^> ^Items
		{
			System::Collections::Generic::List<UIElement ^> ^get();
			void set(System::Collections::Generic::List<UIElement ^> ^value);
		}

		virtual void Draw() override;
		virtual void Draw(int xMod, int yMod) override;
			
	private:
		System::Collections::Generic::List<UIElement ^> ^mItems;
	};
}