#include "Menu.hpp"

namespace GTA
{
	Menu::Menu(System::String ^headerCaption, array<MenuItem ^> ^items)
	{
		//Put the items in the item stack
		//The menu itself will be initialized when it gets added to the viewport
		for each (MenuItem ^item in items)
		{
			mItems->Add(item);
			item->Parent = this;
		}

		//Set defaults for the properties
		HeaderColor = System::Drawing::Color::FromArgb(200, 255, 20, 147);
		HeaderTextColor = System::Drawing::Color::White;
		HeaderFont = 1;
		HeaderTextScale = 0.5f;
		HeaderCentered = true;
		FooterColor = System::Drawing::Color::FromArgb(200, 255, 182, 193);
		FooterTextColor = System::Drawing::Color::Black;
		FooterFont = 0;
		FooterTextScale = 0.4f;
		FooterCentered = false;
		SelectedItemColor = System::Drawing::Color::FromArgb(200, 255, 105, 180);
		UnselectedItemColor = System::Drawing::Color::FromArgb(200, 176, 196, 222);
		SelectedTextColor = System::Drawing::Color::Black;
		UnselectedTextColor = System::Drawing::Color::DarkSlateGray;
		ItemFont = 0;
		ItemTextScale = 0.4f;
		ItemTextCentered = true;

		Caption = headerCaption;
		Position = System::Drawing::Point(40, 60);

		Width = 200;
		HeaderHeight = 30;
		FooterHeight = 60;
		ItemHeight = 30;
		HasFooter = true;
	}

	void Menu::Draw()
	{
		if (mHeaderRect == nullptr || mHeaderText == nullptr || (HasFooter && (mFooterRect == nullptr || mFooterText == nullptr))) return;
		if (HasFooter)
		{
			mFooterRect->Draw();
			mFooterText->Draw();
		}
		mHeaderRect->Draw();
		mHeaderText->Draw();
		for each (MenuItem ^item in mItems)
		{
			item->Draw();
		}
	}

	void Menu::Initialize()
	{
		int currentY = HeaderHeight + Position.Y;
		System::Drawing::Size itemSize = System::Drawing::Size(Width, ItemHeight);
		for each (MenuItem ^item in mItems)
		{
			item->SetOriginAndSize(System::Drawing::Point(Position.X, currentY), itemSize);
			currentY += ItemHeight;
		}
		mSelectedIndex = 0;
		mFooterDescription = mItems[mSelectedIndex]->Description;
		mItems[mSelectedIndex]->Select();

		int itemsHeight = mItems->Count * ItemHeight;
		mHeaderRect = gcnew UIRectangle(Position,
			System::Drawing::Size(Width, HeaderHeight), HeaderColor);
		if (HasFooter) mFooterRect = gcnew UIRectangle(System::Drawing::Point(Position.X, Position.Y + HeaderHeight + itemsHeight),
			System::Drawing::Size(Width, FooterHeight), FooterColor);

		mHeaderText = gcnew UIText(Caption,
			HeaderCentered ? System::Drawing::Point(Position.X + Width / 2, Position.Y) : Position,
			HeaderTextScale,
			HeaderTextColor,
			HeaderFont,
			HeaderCentered);

		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Position.X + Width / 2, Position.Y + HeaderHeight + itemsHeight) : System::Drawing::Point(Position.X, Position.Y + HeaderHeight + itemsHeight),
			FooterTextScale,
			FooterTextColor,
			FooterFont,
			FooterCentered);
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
		mFooterDescription = mItems[mSelectedIndex]->Description;
		mItems[mSelectedIndex]->Select();

		//Update footer
		int itemsHeight = mItems->Count * ItemHeight;
		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Position.X + Width / 2, Position.Y + HeaderHeight + itemsHeight) : System::Drawing::Point(Position.X, Position.Y + HeaderHeight + itemsHeight),
			FooterTextScale,
			FooterTextColor,
			FooterFont,
			FooterCentered);
	}

	void Menu::OnChangeItem(bool right)
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Change(right);
	}
}