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
}