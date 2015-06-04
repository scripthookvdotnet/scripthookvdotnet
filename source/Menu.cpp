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
		HeaderFont = Font::HouseScript;
		HeaderTextScale = 0.5f;
		HeaderCentered = true;
		FooterColor = System::Drawing::Color::FromArgb(200, 255, 182, 193);
		FooterTextColor = System::Drawing::Color::Black;
		FooterFont = Font::ChaletLondon;
		FooterTextScale = 0.4f;
		FooterCentered = false;
		SelectedItemColor = System::Drawing::Color::FromArgb(200, 255, 105, 180);
		UnselectedItemColor = System::Drawing::Color::FromArgb(200, 176, 196, 222);
		SelectedTextColor = System::Drawing::Color::Black;
		UnselectedTextColor = System::Drawing::Color::DarkSlateGray;
		ItemFont = Font::ChaletLondon;
		ItemTextScale = 0.4f;
		ItemTextCentered = true;
		Caption = headerCaption;

		Width = 200;
		HeaderHeight = 30;
		FooterHeight = 60;
		ItemHeight = 30;
		HasFooter = true;
	}

	void Menu::Draw()
	{
		Draw(System::Drawing::Size());
	}

	void Menu::Draw(System::Drawing::Size offset)
	{
		if (mHeaderRect == nullptr || mHeaderText == nullptr || (HasFooter && (mFooterRect == nullptr || mFooterText == nullptr))) return;
		if (HasFooter)
		{
			mFooterRect->Draw(offset);
			mFooterText->Draw(offset);
		}
		mHeaderRect->Draw(offset);
		mHeaderText->Draw(offset);
		for each (MenuItem ^item in mItems)
		{
			item->Draw(offset);
		}
	}

	void Menu::Initialize()
	{
		int currentY = HeaderHeight;
		System::Drawing::Size itemSize = System::Drawing::Size(Width, ItemHeight);
		for each (MenuItem ^item in mItems)
		{
			item->SetOriginAndSize(System::Drawing::Point(0, currentY), itemSize);
			currentY += ItemHeight;
		}
		mSelectedIndex = 0;
		mFooterDescription = mItems[mSelectedIndex]->Description;
		mItems[mSelectedIndex]->Select();

		int itemsHeight = mItems->Count * ItemHeight;
		mHeaderRect = gcnew UIRectangle(System::Drawing::Point(),
			System::Drawing::Size(Width, HeaderHeight), HeaderColor);
		if (HasFooter) mFooterRect = gcnew UIRectangle(System::Drawing::Point(0, HeaderHeight + itemsHeight),
			System::Drawing::Size(Width, FooterHeight), FooterColor);

		mHeaderText = gcnew UIText(Caption,
			HeaderCentered ? System::Drawing::Point(Width / 2, 0) : System::Drawing::Point(),
			HeaderTextScale,
			HeaderTextColor,
			HeaderFont,
			HeaderCentered);

		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Width / 2, HeaderHeight + itemsHeight) : System::Drawing::Point(0, HeaderHeight + itemsHeight),
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
		if (newIndex >= mItems->Count) newIndex = 0;
		if (newIndex < 0) newIndex = mItems->Count - 1;
		OnChangeSelection(newIndex);
	}

	void Menu::OnChangeSelection(int newIndex){
		mItems[mSelectedIndex]->Deselect();
		mSelectedIndex = newIndex;
		mFooterDescription = mItems[mSelectedIndex]->Description;
		mItems[mSelectedIndex]->Select();

		//Update footer
		int itemsHeight = mItems->Count * ItemHeight;
		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Width / 2, HeaderHeight + itemsHeight) : System::Drawing::Point(0, HeaderHeight + itemsHeight),
			FooterTextScale,
			FooterTextColor,
			FooterFont,
			FooterCentered);
		this->SelectedIndexChanged(this, gcnew SelectedIndexChangedArgs(this->mSelectedIndex));
	}

	void Menu::OnChangeItem(bool right)
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Change(right);
	}

	ListMenu::ListMenu(System::String ^headerCaption, System::Action<ListMenu ^> ^activationAction)
	{
		mItems = gcnew System::Collections::Generic::List<System::Tuple<System::String ^, System::String ^> ^>();
		mUIEntries = gcnew System::Collections::Generic::List<System::Tuple<UIRectangle ^, UIText ^> ^>();
		//Set defaults for the properties
		HeaderColor = System::Drawing::Color::FromArgb(200, 255, 20, 147);
		HeaderTextColor = System::Drawing::Color::White;
		HeaderFont = Font::HouseScript;
		HeaderTextScale = 0.5f;
		HeaderCentered = true;
		FooterColor = System::Drawing::Color::FromArgb(200, 255, 182, 193);
		FooterTextColor = System::Drawing::Color::Black;
		FooterFont = Font::ChaletLondon;
		FooterTextScale = 0.4f;
		FooterCentered = false;
		SelectedItemColor = System::Drawing::Color::FromArgb(200, 255, 105, 180);
		UnselectedItemColor = System::Drawing::Color::FromArgb(200, 176, 196, 222);
		SelectedTextColor = System::Drawing::Color::Black;
		UnselectedTextColor = System::Drawing::Color::DarkSlateGray;
		ItemFont = Font::ChaletLondon;
		ItemTextScale = 0.4f;
		ItemTextCentered = true;
		Caption = headerCaption;
		mActivateAction = activationAction;

		Width = 200;
		HeaderHeight = 30;
		FooterHeight = 60;
		ItemHeight = 30;
		HasFooter = true;
	}

	void Nil(ListMenu ^ _menu) {}

	ListMenu::ListMenu(System::String ^headerCaption)
		: ListMenu(headerCaption, gcnew System::Action<ListMenu ^>(Nil))
	{
	}

	void ListMenu::UpdateEntries()
	{
		mUIEntries->Clear();
		if (mSelectedIndex < 0) mSelectedIndex = 0;
		else if (mSelectedIndex > mItems->Count - 1) mSelectedIndex = mItems->Count - 1;
		int i = 0;
		for each (System::Tuple<System::String ^, System::String ^> ^item in mItems)
		{
			System::String ^Caption = item->Item1, ^Description = item->Item2;
			bool Selected = i == mSelectedIndex;
			System::Drawing::Point Origin = System::Drawing::Point(0, HeaderHeight + ItemHeight * i);
			System::Drawing::Size Bounds = System::Drawing::Size(Width, ItemHeight);
			System::Drawing::Color BoxColor = Selected ? SelectedItemColor : UnselectedItemColor;
			UIRectangle ^Box = gcnew UIRectangle(Origin, Bounds, BoxColor);
			System::Drawing::Color TextColor = Selected ? SelectedTextColor : UnselectedTextColor;
			System::Drawing::Point TextOrigin = ItemTextCentered ? System::Drawing::Point(Width / 2, HeaderHeight + ItemHeight * i) : Origin;
			UIText ^Text = gcnew UIText(Caption, TextOrigin, ItemTextScale, TextColor, ItemFont, ItemTextCentered);
			System::Tuple<UIRectangle ^, UIText ^> ^EntryTuple = System::Tuple::Create<UIRectangle ^, UIText ^>(Box, Text);
			mUIEntries->Add(EntryTuple);
			i++;
		}
		if (HasFooter && mFooterRect != nullptr && mFooterText != nullptr)
		{
			mFooterRect->Position = System::Drawing::Point(0, HeaderHeight + i * ItemHeight);
			mFooterText->Position = FooterCentered ? System::Drawing::Point(Width / 2, HeaderHeight + i * ItemHeight)
				: System::Drawing::Point(0, HeaderHeight + i * ItemHeight);
		}
	}

	void ListMenu::UpdateHighlight()
	{
		if (mSelectedIndex < 0) mSelectedIndex = 0;
		else if (mSelectedIndex > mItems->Count - 1) mSelectedIndex = mItems->Count - 1;
		int i = 0;
		for each (System::Tuple<UIRectangle ^, UIText ^> ^EntryTuple in mUIEntries)
		{
			bool Selected = i == mSelectedIndex;
			System::Drawing::Color BoxColor = Selected ? SelectedItemColor : UnselectedItemColor;
			System::Drawing::Color TextColor = Selected ? SelectedTextColor : UnselectedTextColor;
			EntryTuple->Item1->Color = BoxColor;
			EntryTuple->Item2->Color = TextColor;
			i++;
		}
		this->SelectedIndexChanged(this, gcnew SelectedIndexChangedArgs(this->mSelectedIndex));
	}

	void ListMenu::Draw()
	{
		Draw(System::Drawing::Size());
	}

	void ListMenu::Draw(System::Drawing::Size offset)
	{
		if (mHeaderRect == nullptr || mHeaderText == nullptr) return;
		mHeaderRect->Draw(offset);
		mHeaderText->Draw(offset);
		if (HasFooter && mFooterRect != nullptr && mFooterText != nullptr)
		{
			mFooterRect->Draw(offset);
			mFooterText->Draw(offset);
		}
		for each (System::Tuple<UIRectangle ^, UIText ^> ^EntryTuple in mUIEntries)
		{
			EntryTuple->Item1->Draw(offset);
			EntryTuple->Item2->Draw(offset);
		}
	}

	void ListMenu::Initialize()
	{
		mSelectedIndex = 0;
		mFooterDescription = mItems[mSelectedIndex]->Item2;

		UpdateEntries();

		int itemsHeight = mItems->Count * ItemHeight;
		mHeaderRect = gcnew UIRectangle(System::Drawing::Point(),
			System::Drawing::Size(Width, HeaderHeight), HeaderColor);
		if (HasFooter) mFooterRect = gcnew UIRectangle(System::Drawing::Point(0, HeaderHeight + itemsHeight),
			System::Drawing::Size(Width, FooterHeight), FooterColor);

		mHeaderText = gcnew UIText(Caption,
			HeaderCentered ? System::Drawing::Point(Width / 2, 0) : System::Drawing::Point(),
			HeaderTextScale,
			HeaderTextColor,
			HeaderFont,
			HeaderCentered);

		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Width / 2, HeaderHeight + itemsHeight) : System::Drawing::Point(0, HeaderHeight + itemsHeight),
			FooterTextScale,
			FooterTextColor,
			FooterFont,
			FooterCentered);
	}

	void ListMenu::OnOpen()
	{
	}

	void ListMenu::OnClose()
	{
	}

	void ListMenu::OnActivate()
	{
		if (mActivateAction != nullptr) mActivateAction(this);
	}

	void ListMenu::OnChangeSelection(bool down)
	{
		int newIndex = down ? mSelectedIndex + 1 : mSelectedIndex - 1;
		mSelectedIndex = newIndex;
		UpdateHighlight();
		mFooterDescription = mItems[mSelectedIndex]->Item2;

		//Update footer
		int itemsHeight = mItems->Count * ItemHeight;
		if (HasFooter) mFooterText = gcnew UIText(mFooterDescription,
			FooterCentered ? System::Drawing::Point(Width / 2, HeaderHeight + itemsHeight) : System::Drawing::Point(0, HeaderHeight + itemsHeight),
			FooterTextScale,
			FooterTextColor,
			FooterFont,
			FooterCentered);
	}

	void ListMenu::OnChangeItem(bool right)
	{
	}

	void ListMenu::Add(System::String ^Caption)
	{
		Add(Caption, "");
	}

	void ListMenu::Add(System::String ^Caption, System::String ^Description)
	{
		mItems->Add(System::Tuple::Create<System::String ^, System::String ^>(Caption, Description));
		UpdateEntries();
	}

	void ListMenu::Remove(int Index)
	{
		mItems->RemoveAt(Index);
		UpdateEntries();
	}

	void ListMenu::Remove(System::String ^Caption)
	{
		int Index = -1, I = 0;
		for each (System::Tuple<System::String ^, System::String ^> ^ Tup in mItems)
		{
			if (Tup->Item1 == Caption)
			{
				Index = I;
				break;
			}
			I++;
		}
		UpdateEntries();
	}

	MessageBox::MessageBox(System::String ^caption, System::Action ^yes, System::Action ^no)
		: mYesAction(yes), mNoAction(no)
	{
		HeaderColor = System::Drawing::Color::FromArgb(200, 255, 20, 147);
		HeaderTextColor = System::Drawing::Color::White;
		HeaderFont = Font::HouseScript;
		HeaderTextScale = 0.5f;
		HeaderCentered = true;
		SelectedItemColor = System::Drawing::Color::FromArgb(200, 255, 105, 180);
		UnselectedItemColor = System::Drawing::Color::FromArgb(200, 176, 196, 222);
		SelectedTextColor = System::Drawing::Color::Black;
		UnselectedTextColor = System::Drawing::Color::DarkSlateGray;
		ItemFont = Font::ChaletLondon;
		ItemTextScale = 0.4f;
		ItemTextCentered = true;
		Caption = caption;

		Width = 200;
		Height = 50;
		ButtonHeight = 30;
		OkCancel = false;
	}

	void MessageBox::Draw()
	{
		Draw(System::Drawing::Size());
	}

	void MessageBox::Draw(System::Drawing::Size offset)
	{
		mBodyRect->Draw(offset);
		mText->Draw(offset);
		mYesRect->Draw(offset);
		mNoRect->Draw(offset);
		mYesText->Draw(offset);
		mNoText->Draw(offset);
	}

	void MessageBox::Initialize()
	{
		mBodyRect = gcnew UIRectangle(System::Drawing::Point(), System::Drawing::Size(Width, Height), HeaderColor);
		mText = gcnew UIText(Caption, HeaderCentered ? System::Drawing::Point(Width / 2, 0) : System::Drawing::Point(), HeaderTextScale, HeaderTextColor, HeaderFont, HeaderCentered);
		mYesRect = gcnew UIRectangle(System::Drawing::Point(0, Height), System::Drawing::Size(Width / 2, ButtonHeight), UnselectedItemColor);
		mNoRect = gcnew UIRectangle(System::Drawing::Point(Width / 2, Height), System::Drawing::Size(Width / 2, ButtonHeight), UnselectedItemColor);
		mYesText = gcnew UIText(OkCancel ? "OK" : "Yes", System::Drawing::Point(Width / 4, Height), ItemTextScale, UnselectedTextColor, ItemFont, ItemTextCentered);
		mNoText = gcnew UIText(OkCancel ? "Cancel" : "No", System::Drawing::Point(Width / 4 * 3, Height), ItemTextScale, UnselectedTextColor, ItemFont, ItemTextCentered);
		OnChangeItem(false);
	}

	void MessageBox::OnOpen()
	{
	}

	void MessageBox::OnClose()
	{
	}

	void MessageBox::OnActivate()
	{
		if (mSelection) mYesAction->Invoke();
		else mNoAction->Invoke();
		Parent->PopMenu();
	}

	void MessageBox::OnChangeSelection(bool _A)
	{
	}

	void MessageBox::OnChangeItem(bool _A)
	{
		mSelection = !mSelection;
		if (mSelection)
		{
			mYesRect->Color = SelectedItemColor;
			mNoRect->Color = UnselectedItemColor;
			mYesText->Color = SelectedTextColor;
			mNoText->Color = UnselectedTextColor;
		}
		else
		{
			mNoRect->Color = SelectedItemColor;
			mYesRect->Color = UnselectedItemColor;
			mNoText->Color = SelectedTextColor;
			mYesText->Color = UnselectedTextColor;
		}
	}
}