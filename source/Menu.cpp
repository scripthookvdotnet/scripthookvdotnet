#include "Menu.hpp"

namespace GTA
{
	Menu::Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin, int width, int headerHeight, int footerHeight, int itemHeight, int itemPadding, System::Drawing::Color bgColor, System::Drawing::Color headerColor, System::Drawing::Color footerColor)
		: mBackgroundColor(bgColor), mHeaderColor(headerColor), mFooterColor(footerColor), mHeaderCaption(headerCaption), mOrigin(origin), mWidth(width), mHeaderHeight(headerHeight), mFooterHeight(footerHeight), mItemHeight(itemHeight), mItemPadding(itemPadding)
	{
		int currentY = mHeaderHeight + mOrigin.Y;
		System::Drawing::Size itemSize = System::Drawing::Size(mWidth - mItemPadding * 2, mItemHeight - mItemPadding * 2);
		for each (MenuItem ^item in items)
		{
			mItems->Add(item);
			item->SetOriginAndSize(System::Drawing::Point(origin.X + mItemPadding, currentY + mItemPadding), itemSize);
			currentY += mItemHeight;
		}
		mSelectedIndex = 0;
		mItems[mSelectedIndex]->Select();

		int itemsHeight = mItems->Count * mItemHeight;
		mBackgroundRect = gcnew UIRectangle(origin, System::Drawing::Size(mWidth, mHeaderHeight + mFooterHeight + itemsHeight), mBackgroundColor);
		mHeaderRect = gcnew UIRectangle(origin, System::Drawing::Size(mWidth, mHeaderHeight), mHeaderColor);
		mFooterRect = gcnew UIRectangle(System::Drawing::Point(origin.X, origin.Y + mHeaderHeight + itemsHeight), System::Drawing::Size(mWidth, mFooterHeight), mFooterColor);

		//TODO: Custom text scale, color and font
		mHeaderText = gcnew UIText(mHeaderCaption, mOrigin, 0.5f, System::Drawing::Color::White, 1, false);
		mFooterText = gcnew UIText(mFooterDescription, System::Drawing::Point(origin.X, origin.Y + mHeaderHeight + itemsHeight), 0.3f, System::Drawing::Color::White, 0, false);
	}

	Menu::Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin, int width, int headerHeight, int footerHeight, int itemHeight, int itemPadding)
		: Menu(headerCaption, items, origin, width, headerHeight, footerHeight, itemHeight, itemPadding, System::Drawing::Color::Black, System::Drawing::Color::CornflowerBlue, System::Drawing::Color::DimGray)
	{}

	Menu::Menu(System::String ^headerCaption, array<MenuItem ^> ^items, System::Drawing::Point origin)
		: Menu(headerCaption, items, origin, 200, 50, 60, 40, 3)
	{}

	Menu::Menu(System::String ^headerCaption, array<MenuItem ^> ^items)
		: Menu(headerCaption, items, System::Drawing::Point(35, 60))
	{}

	void Menu::Draw()
	{
		if (mBackgroundRect == nullptr || mHeaderRect == nullptr || mFooterRect == nullptr || mHeaderText == nullptr || mFooterText == nullptr) return;
		mBackgroundRect->Draw();
		mFooterRect->Draw();
		mFooterText->Draw();
		mHeaderRect->Draw();
		mHeaderText->Draw();
		for each (MenuItem ^item in mItems)
		{
			item->Draw();
		}
	}

	void Menu::OnOpen()
	{
	}

	void Menu::OnClose()
	{
	}

	void Menu::OnActivate()
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Activate();
	}

	void Menu::OnChangeSelection(bool down)
	{
		int newIndex = down ? mSelectedIndex + 1 : mSelectedIndex - 1;
		if (newIndex < 0) newIndex = 0;
		if (newIndex >= mItems->Count) newIndex = mItems->Count - 1;
		mItems[mSelectedIndex]->Deselect();
		mSelectedIndex = newIndex;
		mItems[mSelectedIndex]->Select();
	}

	void Menu::OnChangeItem(bool right)
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Change(right);
	}
}