#include "MenuItem.hpp"

namespace GTA
{
	MenuButton::MenuButton(System::String ^caption, System::String ^description, System::Action ^activationAction)
	{
		Caption = caption;
		Description = description;
		mActivationAction = activationAction;
	}

	MenuButton::MenuButton(System::String ^caption, System::Action ^activationAction)
		: MenuButton(caption, "", activationAction)
	{
	}

	void MenuButton::Draw()
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw();
		mText->Draw();
	}

	void MenuButton::Draw(System::Drawing::Point offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset.X, offset.Y);
		mText->Draw(offset.X, offset.Y);
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
		mActivationAction->Invoke();
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
			Parent->ItemTextCentered ? System::Drawing::Point(mOrigin.X + mSize.Width / 2, mOrigin.Y) : mOrigin,
			Parent->ItemTextScale,
			Parent->UnselectedTextColor,
			Parent->ItemFont,
			Parent->ItemTextCentered);
	}

	MenuToggle::MenuToggle(System::String ^caption, System::String ^description, System::Action ^activationAction, System::Action ^deactivationAction) 
	{
		this->Caption = caption;
		this->Description = description;
		this->mActivationAction = activationAction;
		this->mDeactivationAction = deactivationAction;
	}

	void MenuToggle::Draw()
	{
		Draw(System::Drawing::Point());
	}

	void MenuToggle::Draw(System::Drawing::Point offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset.X, offset.Y);
		mText->Draw(offset.X, offset.Y);
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
		mText = gcnew UIText(Caption + " <OFF>", Parent->ItemTextCentered ? System::Drawing::Point(origin.X + size.Width / 2, origin.Y) : origin, Parent->ItemTextScale, Parent->UnselectedTextColor, Parent->ItemFont, Parent->ItemTextCentered);
	}

	void MenuToggle::ChangeSelection()
	{
		mToggleSelection = !mToggleSelection;
		if (mToggleSelection)
		{
			mText->Text = Caption + " <ON>";
			mActivationAction->Invoke();
		}
		else
		{
			mText->Text = Caption + " <OFF>";
			mDeactivationAction->Invoke();
		}
	}
	
	MenuNumericScroller::MenuNumericScroller(System::String ^caption, System::String ^description, System::Action<double> ^changeAction, System::Action<double> ^activateAction, double min, double max, double inc)
	{
		this->Caption = caption;
		this->Description = description;
		this->mChangeAction = changeAction;
		this->mActivateAction = activateAction;
		Min = min;
		Max = max;
		Increment = inc;
		DecimalFigures = -1;
	}

	void MenuNumericScroller::Draw()
	{
		Draw(System::Drawing::Point());
	}

	void MenuNumericScroller::Draw(System::Drawing::Point offset)
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw(offset.X, offset.Y);
		mText->Draw(offset.X, offset.Y);
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
		mActivateAction(Min + Increment * (double)TimesIncrement);
	}

	void MenuNumericScroller::Change(bool right)
	{
		if (right) TimesIncrement++;
		else TimesIncrement--;
		if (TimesIncrement < 0) TimesIncrement = 0;
		if (TimesIncrement > (int)((Max - Min) / Increment)) TimesIncrement = (int)((Max - Min) / Increment);
		UpdateText();
	}

	void MenuNumericScroller::SetOriginAndSize(System::Drawing::Point origin, System::Drawing::Size size)
	{
		mButton = gcnew UIRectangle(origin, size, Parent->UnselectedItemColor);
		mText = gcnew UIText("", Parent->ItemTextCentered ? System::Drawing::Point(origin.X + size.Width / 2, origin.Y) : origin, Parent->ItemTextScale, Parent->UnselectedTextColor, Parent->ItemFont, Parent->ItemTextCentered);
		UpdateText();
	}

	void MenuNumericScroller::UpdateText()
	{
		double Number = Min + Increment * (double)TimesIncrement;
		System::String ^NumberString = "";
		if (DecimalFigures == -1) NumberString = Number.ToString();
		else if (DecimalFigures == 0) NumberString = ((int)Number).ToString();
		else NumberString = Number.ToString("F" + DecimalFigures);
		mText->Text = Caption + " <" + NumberString + ">";
	}
}