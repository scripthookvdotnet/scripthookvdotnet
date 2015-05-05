#include "MenuItem.hpp"

namespace GTA
{
	MenuButton::MenuButton(System::String ^caption, System::Action ^activationAction, System::Drawing::Color unselected, System::Drawing::Color selected, System::Drawing::Color text)
		: mUnselectedColor(unselected), mSelectedColor(selected), mTextColor(text), mCaption(caption), mActivationAction(activationAction)
	{
	}

	void MenuButton::Draw()
	{
		if (mButton == nullptr || mText == nullptr) return;
		mButton->Draw();
		mText->Draw();
	}

	void MenuButton::Select()
	{
		if (mButton == nullptr) return;
		mButton->Color = mSelectedColor;
	}
		
	void MenuButton::Deselect()
	{
		if (mButton == nullptr) return;
		mButton->Color = mUnselectedColor;
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
		mButton = gcnew UIRectangle(mOrigin, mSize, mUnselectedColor);
		//TODO: Text scale and font!
		mText = gcnew UIText(mCaption, mOrigin, 0.4f, mTextColor, 0, false);
	}

	System::String ^MenuButton::GetDescription()
	{
		return "";
	}
}