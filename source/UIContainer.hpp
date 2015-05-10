#pragma once

#include "UIElement.hpp"
#include "UIRectangle.hpp"

namespace GTA
{
	public ref class UIContainer : public UIRectangle
	{
	public:
		UIContainer();
		UIContainer(System::Drawing::Point position, System::Drawing::Size size);
		UIContainer(System::Drawing::Point position, System::Drawing::Size size, System::Drawing::Color color);

		property System::Collections::Generic::List<UIElement ^> ^Items;

		virtual void Draw() override;
		virtual void Draw(System::Drawing::Size offset) override;
	};
}