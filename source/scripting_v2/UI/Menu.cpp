#include "Menu.hpp"
#include "Game.hpp"
#include "Native.hpp"
#include "Vector2.hpp"

namespace GTA
{
	Menu::Menu(System::String ^headerCaption, array<IMenuItem ^> ^items) : Menu(headerCaption, items, 10)
	{
	}
	Menu::Menu(System::String ^headerCaption, array<IMenuItem ^> ^items, int MaxItemsToDraw)
	{
		//Put the items in the item stack
		//The menu itself will be initialized when it gets added to the viewport
		for each (IMenuItem ^item in items)
		{
			mItems->Add(item);
			item->Parent = this;
		}

		MaxDrawLimit = MaxItemsToDraw;
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
		for (int i = 0; i < mItemDrawCount; i++)
		{
			Items[i + scrollOffset]->Draw(offset);
		}
		DrawScrollArrows(scrollOffset > 0, scrollOffset < mMaxScrollOffset, offset);
	}
	void Menu::DrawScrollArrows(bool up, bool down, System::Drawing::Size offset)
	{
		if (!up && !down)
			return;
		if (Native::Function::Call<bool>(Native::Hash::HAS_STREAMED_TEXTURE_DICT_LOADED, "CommonMenu"))
		{
			Math::Vector2 Resolution = Native::Function::Call<Math::Vector2>(Native::Hash::GET_TEXTURE_RESOLUTION, "CommonMenu", "arrowright");
			if (up)
			{
				float xscale = Resolution.X / (float)UI::WIDTH;
				float yscale = Resolution.Y / (float)UI::HEIGHT;
				float xpos = ((float)(Width + offset.Width)) / (float)UI::WIDTH - xscale * 0.5f;
				float ypos = ((float)(HeaderHeight + offset.Height + ItemHeight / 2)) / (float)UI::HEIGHT;
				Native::Function::Call(Native::Hash::DRAW_SPRITE, "CommonMenu", "arrowright", xpos, ypos, xscale, yscale, -90.0f, 255, 255, 255, 255);
			}
			if (down)
			{
				float xscale = Resolution.X / (float)UI::WIDTH;
				float yscale = Resolution.Y / (float)UI::HEIGHT;
				float xpos = ((float)(Width + offset.Width)) / (float)UI::WIDTH - xscale * 0.5f;
				float ypos = ((float)(HeaderHeight + offset.Height + ItemHeight * mItemDrawCount - ItemHeight / 2)) / (float)UI::HEIGHT;
				Native::Function::Call(Native::Hash::DRAW_SPRITE, "CommonMenu", "arrowright", xpos, ypos, xscale, yscale, 90.0f, 255, 255, 255, 255);
			}
		}
		else
		{
			Native::Function::Call(Native::Hash::REQUEST_STREAMED_TEXTURE_DICT, "CommonMenu", 0);
		}
	}
	void Menu::Initialize()
	{
		int currentY = HeaderHeight;
		System::Drawing::Size itemSize = System::Drawing::Size(Width, ItemHeight);
		for (int i = 0; i < mItemDrawCount; i++)
		{
			Items[i + scrollOffset]->SetOriginAndSize(System::Drawing::Point(0, currentY), itemSize);
			currentY += ItemHeight;
		}
		mSelectedIndex = 0;
		mFooterDescription = mItems[mSelectedIndex]->Description;
		mItems[mSelectedIndex]->Select();

		int itemsHeight = mItemDrawCount * ItemHeight;
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
		if (down)
		{
			if (newIndex - scrollOffset > mItemDrawCount - mStartScrollOffset - 1)
			{
				scrollOffset++;
			}
		}
		else
		{
			if (newIndex - scrollOffset < mStartScrollOffset)
			{
				scrollOffset--;
			}
		}
		OnChangeSelection(newIndex);
	}
	void Menu::OnChangeSelection(int newIndex) {
		if (newIndex < scrollOffset)
			scrollOffset = newIndex - mStartScrollOffset - 1;
		else if (newIndex >scrollOffset + mItemDrawCount)
			scrollOffset = newIndex + mStartScrollOffset + 1 - mItemDrawCount;
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
	void Menu::UpdateItemPositions()
	{
		int currentY = HeaderHeight;
		System::Drawing::Size itemSize = System::Drawing::Size(Width, ItemHeight);
		for (int i = 0; i < mItemDrawCount; i++)
		{
			Items[i + scrollOffset]->SetOriginAndSize(System::Drawing::Point(0, currentY), itemSize);
			currentY += ItemHeight;
		}
	}
	void Menu::OnChangeItem(bool right)
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Change(right);
	}
	void Menu::OnChangeDrawLimit()
	{
		if (scrollOffset > mMaxScrollOffset)
		{
			mScrollOffset = mMaxScrollOffset;
		}
		if (SelectedIndex < scrollOffset)
			scrollOffset = SelectedIndex - mStartScrollOffset - 1;
		else if (SelectedIndex >scrollOffset + mItemDrawCount)
			scrollOffset = SelectedIndex + mStartScrollOffset + 1 - mItemDrawCount;
		UpdateItemPositions();
		if (SelectedIndex >= 0 && SelectedIndex < mItems->Count)
			mItems[SelectedIndex]->Select();
	}

	MessageBox::MessageBox(System::String ^caption)
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
		if (mSelection)
		{
			this->Yes(this, System::EventArgs::Empty);
		}
		else
		{
			this->No(this, System::EventArgs::Empty);
		}
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

	MenuButton::MenuButton(System::String ^caption, System::String ^description)
	{
		mCaption = caption;
		Description = description;
	}
	MenuButton::MenuButton(System::String ^caption)
		: MenuButton(caption, "")
	{
	}

	void MenuButton::Draw()
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw();
		mText->Draw();
	}
	void MenuButton::Draw(System::Drawing::Size offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset);
		mText->Draw(offset);
	}
	void MenuButton::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->SelectedItemColor;
		mText->Color = Parent->SelectedTextColor;
	}
	void MenuButton::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->UnselectedItemColor;
		mText->Color = Parent->UnselectedTextColor;
	}
	void MenuButton::Activate()
	{
		this->Activated(this, System::EventArgs::Empty);
	}
	void MenuButton::Change(bool right)
	{
		//Nothing to do here
		return;
	}
	void MenuButton::SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size)
	{
		this->mOrigin = topLeftOrigin;
		this->mSize = size;
		mButton = gcnew UIRectangle(mOrigin, mSize, Parent->UnselectedItemColor);
		mText = gcnew UIText(Caption,
			Parent->ItemTextCentered ? System::Drawing::Point(mOrigin.X + mSize.Width / 2 + Parent->TextOffset.X, mOrigin.Y + Parent->TextOffset.Y) : System::Drawing::Point(mOrigin.X + Parent->TextOffset.X, mOrigin.Y + Parent->TextOffset.Y),
			Parent->ItemTextScale,
			Parent->UnselectedTextColor,
			Parent->ItemFont,
			Parent->ItemTextCentered);
	}
	void MenuButton::UpdateText() {
		mText->Caption = Caption;
	}

	MenuToggle::MenuToggle(System::String ^caption, System::String ^description)
	{
		this->Caption = caption;
		this->Description = description;
		mToggleSelection = false;
	}
	MenuToggle::MenuToggle(System::String ^caption, System::String ^description, bool value)
	{
		this->Caption = caption;
		this->Description = description;
		mToggleSelection = value;
	}

	void MenuToggle::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void MenuToggle::Draw(System::Drawing::Size offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset);
		mText->Draw(offset);
	}
	void MenuToggle::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->SelectedItemColor;
		mText->Color = Parent->SelectedTextColor;
	}
	void MenuToggle::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->UnselectedItemColor;
		mText->Color = Parent->UnselectedTextColor;
	}
	void MenuToggle::Activate()
	{
		ChangeSelection();
	}
	void MenuToggle::Change(bool _r)
	{
		ChangeSelection();
	}
	void MenuToggle::SetOriginAndSize(System::Drawing::Point origin, System::Drawing::Size size)
	{
		mButton = gcnew UIRectangle(origin, size, Parent->UnselectedItemColor);
		mText = gcnew UIText("", Parent->ItemTextCentered ? System::Drawing::Point(origin.X + size.Width / 2 + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y) : System::Drawing::Point(origin.X + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y), Parent->ItemTextScale, Parent->UnselectedTextColor, Parent->ItemFont, Parent->ItemTextCentered);
		UpdateText();
	}
	void MenuToggle::ChangeSelection()
	{
		Value = !mToggleSelection;
		this->Changed(this, System::EventArgs::Empty);
	}
	void MenuToggle::UpdateText() {
		if (mToggleSelection) {
			mText->Caption = Caption + " <ON>";
		}
		else {
			mText->Caption = Caption + " <OFF>";
		}
	}

	MenuNumericScroller::MenuNumericScroller(System::String ^caption, System::String ^description, double min, double max, double inc)
	{
		this->Caption = caption;
		this->Description = description;
		Min = min;
		Max = max;
		Increment = inc;
		DecimalFigures = -1;
		mTimesIncrement = 0;
	}
	MenuNumericScroller::MenuNumericScroller(System::String ^caption, System::String ^description, double min, double max, double inc, int timesIncremented)
	{
		this->Caption = caption;
		this->Description = description;
		Min = min;
		Max = max;
		Increment = inc;
		DecimalFigures = -1;
		mTimesIncrement = timesIncremented;
	}

	void MenuNumericScroller::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void MenuNumericScroller::Draw(System::Drawing::Size offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset);
		mText->Draw(offset);
	}
	void MenuNumericScroller::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->SelectedItemColor;
		mText->Color = Parent->SelectedTextColor;
	}
	void MenuNumericScroller::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->UnselectedItemColor;
		mText->Color = Parent->UnselectedTextColor;
	}
	void MenuNumericScroller::Activate()
	{
		this->Activated(this, gcnew MenuItemDoubleValueArgs(this->Value));
	}
	void MenuNumericScroller::Change(bool right)
	{
		if (right)
		{
			if (TimesIncremented + 1 > (int)((Max - Min) / Increment))
			{
				TimesIncremented = 0;
			}
			else
			{
				TimesIncremented++;
			}
		}
		else
		{
			if (TimesIncremented - 1 < 0)
			{
				TimesIncremented = (int)((Max - Min) / Increment);
			}
			else
			{
				TimesIncremented--;
			}
		}
		this->Changed(this, gcnew MenuItemDoubleValueArgs(this->Value));
	}
	void MenuNumericScroller::SetOriginAndSize(System::Drawing::Point origin, System::Drawing::Size size)
	{
		mButton = gcnew UIRectangle(origin, size, Parent->UnselectedItemColor);
		mText = gcnew UIText("", Parent->ItemTextCentered ? System::Drawing::Point(origin.X + size.Width / 2 + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y) : System::Drawing::Point(origin.X + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y), Parent->ItemTextScale, Parent->UnselectedTextColor, Parent->ItemFont, Parent->ItemTextCentered);
		UpdateText();
	}
	void MenuNumericScroller::UpdateText()
	{
		double Number = Min + Increment * (double)TimesIncremented;
		System::String ^NumberString = "";
		if (DecimalFigures == -1) NumberString = Number.ToString();
		else if (DecimalFigures == 0) NumberString = ((int)Number).ToString();
		else NumberString = Number.ToString("F" + DecimalFigures);
		mText->Caption = Caption + " <" + NumberString + ">";
	}

	MenuEnumScroller::MenuEnumScroller(System::String ^caption, System::String ^description, array<System::String ^> ^entries)
	{
		this->Caption = caption;
		this->Description = description;
		mEntries = entries;
		mSelectedIndex = 0;
	}
	MenuEnumScroller::MenuEnumScroller(System::String ^caption, System::String ^description, array<System::String ^> ^entries, int value)
	{
		this->Caption = caption;
		this->Description = description;
		mEntries = entries;
		mSelectedIndex = value;
	}

	void MenuEnumScroller::Draw()
	{
		Draw(System::Drawing::Size());
	}
	void MenuEnumScroller::Draw(System::Drawing::Size offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset);
		mText->Draw(offset);
	}
	void MenuEnumScroller::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->SelectedItemColor;
		mText->Color = Parent->SelectedTextColor;
	}
	void MenuEnumScroller::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->UnselectedItemColor;
		mText->Color = Parent->UnselectedTextColor;
	}
	void MenuEnumScroller::Activate()
	{
		this->Activated(this, gcnew MenuItemIndexArgs(this->Index));
	}
	void MenuEnumScroller::Change(bool right)
	{
		if (right)
		{
			if (Index + 1 > mEntries->Length - 1)
			{
				Index = 0;
			}
			else
			{
				Index++;
			}
		}
		else
		{
			if (Index - 1 < 0)
			{
				Index = mEntries->Length - 1;
			}
			else
			{
				Index--;
			}
		}
		this->Changed(this, gcnew MenuItemIndexArgs(this->Index));
	}
	void MenuEnumScroller::SetOriginAndSize(System::Drawing::Point origin, System::Drawing::Size size)
	{
		mButton = gcnew UIRectangle(origin, size, Parent->UnselectedItemColor);
		mText = gcnew UIText("", Parent->ItemTextCentered ? System::Drawing::Point(origin.X + size.Width / 2 + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y) : System::Drawing::Point(origin.X + Parent->TextOffset.X, origin.Y + Parent->TextOffset.Y), Parent->ItemTextScale, Parent->UnselectedTextColor, Parent->ItemFont, Parent->ItemTextCentered);
		UpdateText();
	}
	void MenuEnumScroller::UpdateText()
	{
		mText->Caption = Caption + " <" + mEntries[mSelectedIndex] + ">";
	}

	MenuLabel::MenuLabel(System::String ^caption, bool underlined)
	{
		mCaption = caption;
		Description = "";
		UnderlinedBelow = underlined;
		UnderlinedAbove = false;
		UnderlineColor = System::Drawing::Color::Black;
		UnderlineHeight = 2;
	}
	MenuLabel::MenuLabel(System::String ^caption)
		: MenuLabel(caption, false)
	{
	}

	void MenuLabel::Draw()
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw();
		mText->Draw();
		if (UnderlinedAbove && mUnderlineAbove != nullptr) mUnderlineAbove->Draw();
		if (UnderlinedBelow && mUnderlineBelow != nullptr) mUnderlineBelow->Draw();
	}
	void MenuLabel::Draw(System::Drawing::Size offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset);
		mText->Draw(offset);
		if (UnderlinedAbove && mUnderlineAbove != nullptr) mUnderlineAbove->Draw(offset);
		if (UnderlinedBelow && mUnderlineBelow != nullptr) mUnderlineBelow->Draw(offset);
	}
	void MenuLabel::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->SelectedItemColor;
		mText->Color = Parent->SelectedTextColor;
	}
	void MenuLabel::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = Parent->UnselectedItemColor;
		mText->Color = Parent->UnselectedTextColor;
	}
	void MenuLabel::Activate()
	{
		return;
	}
	void MenuLabel::Change(bool right)
	{
		return;
	}
	void MenuLabel::SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size)
	{
		this->mOrigin = topLeftOrigin;
		this->mSize = size;
		mButton = gcnew UIRectangle(mOrigin, mSize, Parent->UnselectedItemColor);
		mText = gcnew UIText(Caption,
			Parent->ItemTextCentered ? System::Drawing::Point(mOrigin.X + mSize.Width / 2 + Parent->TextOffset.X, mOrigin.Y + Parent->TextOffset.Y) : System::Drawing::Point(mOrigin.X + Parent->TextOffset.X, mOrigin.Y + Parent->TextOffset.Y),
			Parent->ItemTextScale,
			Parent->UnselectedTextColor,
			Parent->ItemFont,
			Parent->ItemTextCentered);
		if (UnderlinedBelow)
		{
			mUnderlineBelow = gcnew UIRectangle(System::Drawing::Point(mOrigin.X, mOrigin.Y + mSize.Height - UnderlineHeight), System::Drawing::Size(mSize.Width, 2), UnderlineColor);
		}
		if (UnderlinedAbove)
		{
			mUnderlineAbove = gcnew UIRectangle(System::Drawing::Point(mOrigin.X, mOrigin.Y), System::Drawing::Size(mSize.Width, UnderlineHeight), UnderlineColor);
		}
	}
	void MenuLabel::UpdateText() {
		mText->Caption = Caption;
	}

	Viewport::Viewport()
	{
		//Set property defaults
		MenuPosition = System::Drawing::Point(100, 50);
		MenuOffset = System::Drawing::Point(250, 0);
	}

	void Viewport::AddMenu(MenuBase ^newMenu)
	{
		//Reset the ease time, so the back menus will nicely ease to the side
		mEaseTime = 0.0f;
		mEaseDirection = true;
		mIsEasing = true;

		//Add it to the top of the stack
		//This should automatically put this menu in to focus, as it is the highest index in the list
		mMenuStack->Add(newMenu);
		newMenu->Parent = this;
		newMenu->Position = MenuPosition;
		newMenu->Initialize();
		newMenu->OnOpen();
	}
	void Viewport::RemoveMenu(MenuBase ^menu) {
		//Reset the ease time
		mEaseTime = 0.0f;
		mEaseDirection = true;
		mIsEasing = true;

		menu->OnClose();
		mMenuStack->Remove(menu);
	}
	void Viewport::PopMenu()
	{
		if (mMenuStack->Count <= 0) return;

		//Reset the ease time
		mEaseTime = 0.0f;
		mEaseDirection = false;
		mIsEasing = true;

		//Removes the highest menu
		//This should automatically give control back to the menu behind this one
		mMenuStack[mMenuStack->Count - 1]->OnClose();
		mMenuStack->RemoveAt(mMenuStack->Count - 1);
	}
	void Viewport::CloseAllMenus()
	{
		while (mMenuStack->Count != 0)
		{
			PopMenu();
		}
	}
	void Viewport::Draw()
	{
		//Menus should be drawn from lowest index to highest index
		//This way the unfocused menus will be placed behind the current focused menu
		int menuCount = mMenuStack->Count;

		if (!MenuTransitions) {
			if (mMenuStack->Count != 0)
			{
				System::Drawing::Size offset = System::Drawing::Size(MenuPosition.X, MenuPosition.Y);
				mMenuStack[mMenuStack->Count - 1]->Draw(offset);
			}
			return;
		}

		//Calculate the easing
		if (mIsEasing)
		{
			if (mEaseTime < 1.0f) mEaseTime += Game::LastFrameTime;
			if (mEaseTime > 1.0f)
			{
				mEaseTime = 1.0f;
				mIsEasing = false;
			}
			float varOffsetX = EaseOut(mEaseTime, 1.0f, 0.0f, (float)MenuOffset.X);
			float varOffsetY = EaseOut(mEaseTime, 1.0f, 0.0f, (float)MenuOffset.Y);
			mEaseOffset = System::Drawing::Point((int)varOffsetX, (int)varOffsetY);
		}
		else
		{
			mEaseTime = 1.0f;
			mEaseDirection = false;
			mEaseOffset = MenuOffset;
		}

		//The last index should be drawn without offset
		//And the second-last with the offset generated from the easing function
		//All the indices before that should be drawn a full offset from eachother
		//This means that we can subtract
		int i = 0;
		for each (MenuBase ^menu in mMenuStack)
		{
			if (mIsEasing)
			{
				if (mEaseDirection)
				{
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 2) + MenuPosition.X);
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 2) + MenuPosition.Y);

					System::Drawing::Size offset = System::Drawing::Size((int)(baseOffsetX + mEaseOffset.X), (int)(baseOffsetY + mEaseOffset.Y));
					menu->Draw(offset);
				}
				else
				{
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i) + MenuPosition.X);
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i) + MenuPosition.Y);

					System::Drawing::Size offset = System::Drawing::Size((int)(baseOffsetX - mEaseOffset.X), (int)(baseOffsetY - mEaseOffset.Y));
					menu->Draw(offset);
				}
			}
			else
			{
				if (i == menuCount)
				{
					menu->Draw();
				}
				else
				{
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 1) + MenuPosition.X);
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 1) + MenuPosition.Y);

					System::Drawing::Size offset = System::Drawing::Size((int)(baseOffsetX), (int)(baseOffsetY));
					menu->Draw(offset);
				}
			}
			i++;
		}
	}
	void Viewport::HandleActivate()
	{
		if (mMenuStack->Count <= 0) return;
		mMenuStack[mMenuStack->Count - 1]->OnActivate();
	}
	void Viewport::HandleBack()
	{
		PopMenu();
	}
	void Viewport::HandleChangeSelection(bool down)
	{
		if (mMenuStack->Count <= 0) return;
		mMenuStack[mMenuStack->Count - 1]->OnChangeSelection(down);
	}
	void Viewport::HandleChangeItem(bool right)
	{
		if (mMenuStack->Count <= 0) return;
		mMenuStack[mMenuStack->Count - 1]->OnChangeItem(right);
	}
	float Viewport::EaseOut(float t, float d, float v0, float dv)
	{
		float s = 1.70158f;
		return dv * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + v0;
	}
}