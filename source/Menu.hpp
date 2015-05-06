#pragma once

#include "UIRectangle.hpp"
#include "UIText.hpp"
#include "MenuItem.hpp"

namespace GTA
{
	public ref class Menu
	{
	public:
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin, int width, int headerHeight, int footerHeight, int itemHeight, int itemPadding, System::Drawing::Color headerColor, System::Drawing::Color footerColor, bool hasFooter);
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin, int width, int headerHeight, int footerHeight, int itemHeight, int itemPadding);
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin);
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items);

	public:
		/** Draws the menu */
		virtual void Draw();

		/** Called when the menu is first added to the viewport */
		virtual void OnOpen();

		/** Called when the user hits the back button */
		virtual void OnClose();

		/** Called when the user hits the activate button */
		virtual void OnActivate();

		/** Called when the user changes what element is selected (i.e. up and down) */
		virtual void OnChangeSelection(bool down);

		/** Called when the user changes the current element (i.e. left and right) */
		virtual void OnChangeItem(bool right);

	private:
		System::Drawing::Color mHeaderColor, mFooterColor, mSelectedItemColor, mUnselectedItemColor, mSelectedTextColor, mUnselectedTextColor;
		System::String ^mHeaderCaption;
		System::Drawing::Point mOrigin;
		int mWidth, mHeaderHeight, mFooterHeight, mItemHeight, mItemPadding;
		bool mHeaderCentered, mItemsCentered;
		bool mHasFooter;

		UIRectangle ^mHeaderRect = nullptr, ^mFooterRect = nullptr;
		UIText ^mHeaderText = nullptr, ^mFooterText = nullptr;

		System::Collections::Generic::List<MenuItem ^> ^mItems = gcnew System::Collections::Generic::List<MenuItem ^>();
		int mSelectedIndex = -1;
		System::String ^mFooterDescription = "footer description";
	};
}