#include "Menu.hpp"
#include "Native.hpp"
#include "Vector2.hpp"

namespace GTA
{
	Menu::Menu(System::String ^headerCaption, array<IMenuItem ^> ^items)
	{
		//Put the items in the item stack
		//The menu itself will be initialized when it gets added to the viewport
		for each (IMenuItem ^item in items)
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
		for (int i = 0; i<mItemDrawCount; i++)
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
		for (int i = 0; i<mItemDrawCount; i++)
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
			if (newIndex - scrollOffset > mItemDrawCount - 3)
			{
				scrollOffset++;
			}
		}
		else
		{
			if (newIndex - scrollOffset < 2)
			{
				scrollOffset--;
			}
		}
		OnChangeSelection(newIndex);
	}

	void Menu::OnChangeSelection(int newIndex){
		if (newIndex < scrollOffset)
			scrollOffset = newIndex - 3;
		else if (newIndex >scrollOffset + mItemDrawCount)
			scrollOffset = newIndex + 3 - mItemDrawCount;
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
		for (int i = 0; i<mItemDrawCount; i++)
		{
			Items[i+scrollOffset]->SetOriginAndSize(System::Drawing::Point(0, currentY), itemSize);
			currentY += ItemHeight;
		}
	}

	void Menu::OnChangeItem(bool right)
	{
		if (mSelectedIndex < 0 || mSelectedIndex >= mItems->Count) return;
		mItems[mSelectedIndex]->Change(right);
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
}