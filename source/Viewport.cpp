#include "Viewport.hpp"
#include "Game.hpp"
#include "Menu.hpp"

namespace GTA
{
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
		newMenu->Position = MenuPosition;
		newMenu->Initialize();
		newMenu->OnOpen();
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
		MenuBase ^current = mMenuStack[mMenuStack->Count - 1];
		current->OnClose();
		mMenuStack->Remove(current);
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
				mMenuStack[mMenuStack->Count - 1]->Draw();
			return;
		}

		//Calculate the easing
		if (mIsEasing)
		{
			if (mEaseTime < 1.0f) mEaseTime += Game::LastFrameTime;
			if (mEaseTime > 1.0f) {
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
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 2));
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 2));

					System::Drawing::Point offset = System::Drawing::Point((int)(baseOffsetX + mEaseOffset.X), (int)(baseOffsetY + mEaseOffset.Y));
					menu->Draw(offset);
				}
				else
				{
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i));
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i));

					System::Drawing::Point offset = System::Drawing::Point((int)(baseOffsetX - mEaseOffset.X), (int)(baseOffsetY - mEaseOffset.Y));
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
					float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 1));
					float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 1));

					System::Drawing::Point offset = System::Drawing::Point((int)(baseOffsetX), (int)(baseOffsetY));
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