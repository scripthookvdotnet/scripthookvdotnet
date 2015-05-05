#include "Viewport.hpp"

namespace GTA
{
	void Viewport::AddMenu(Menu ^newMenu)
	{
		//Add it to the top of the stack
		//This should automatically put this menu in to focus, as it is the highest index in the list
		mMenuStack->Add(newMenu);
		newMenu->OnOpen();
	}

	void Viewport::PopMenu()
	{
		if (mMenuStack->Count <= 0) return;

		//Removes the highest menu
		//This should automatically give control back to the menu behind this one
		Menu ^current = mMenuStack[mMenuStack->Count - 1];
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
		for each (Menu ^menu in mMenuStack)
		{
			menu->Draw();
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

	void Viewport::HandleChangeElement(bool right)
	{
		if (mMenuStack->Count <= 0) return;
		mMenuStack[mMenuStack->Count - 1]->OnChangeElement(right);
	}
}