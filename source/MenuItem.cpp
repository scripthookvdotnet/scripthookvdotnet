//Modeled after Nacorpio's GUI library

#include "MenuItem.hpp"

namespace GTA
{
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

	void MenuButton::UpdateText(){
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
	}

	void MenuToggle::UpdateText(){
		if (mToggleSelection){
			mText->Caption = Caption + " <ON>";
		}
		else{
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

	void MenuLabel::UpdateText(){
		mText->Caption = Caption;
	}
}