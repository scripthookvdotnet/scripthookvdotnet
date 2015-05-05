#pragma once

#include "UIRectangle.hpp"
#include "UIText.hpp"

namespace GTA
{
	public interface class MenuItem
	{
	public:
		/** Called when the MenuItem should be drawn */
		void Draw();

		/** Called when the user selects this item */
		void Select();
		
		/** Called when the user deselects this item */
		void Deselect();

		/** Called when the user activates this item (e.g. numpad-5) */
		void Activate();

		/** Called when the user changes this item (e.g. numpad-4 and 6) */
		void Change(bool right);

		/** Called by the Menu to set this item's origin */
		void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		/** Called by the menu to get the footer text */
		System::String ^GetDescription();
	};

	public ref class MenuButton : MenuItem
	{
	public:
		MenuButton(System::String ^caption, System::Action ^activationAction);
		MenuButton(System::String ^caption, System::Action ^activationAction, System::Drawing::Color unselectedColor, System::Drawing::Color selectedColor, System::Drawing::Color textColor);

	public:
		virtual void Draw();

		virtual void Select();
		
		virtual void Deselect();
		
		virtual void Activate();

		virtual void Change(bool right);

		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		virtual System::String ^GetDescription();

	private:
		System::Drawing::Color mUnselectedColor, mSelectedColor, mTextColor;
		System::String ^mCaption;
		System::Action ^mActivationAction;

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
}