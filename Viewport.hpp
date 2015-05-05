#pragma once

#include "Menu.hpp"

namespace GTA
{
	/**
	 * Static class that handles the active UIs and menus
	 */
	public ref class Viewport abstract sealed
	{
	public:
		/** Add a menu to the stack of active menus and set it as focused */
		static void AddMenu(Menu ^newMenu);

		/** Remove the active menu from the stack, this will focus the next highest menu */
		static void PopMenu();

		/** Closes all menus */
		static void CloseAllMenus();

		/** Draw all the active UIs */
		static void Draw();

		/** Handles when the activate button is pressed (e.g. numpad-5) */
		static void HandleActivate();

		/** Handles when the back button is pressed (e.g. numpad-0) */
		static void HandleBack();

		/** Handles when the user presses the up or down button (e.g. numpad-2 and 8) */
		static void HandleChangeSelection(bool down);

		/** Handles when the user presses the left or right button (e.g. numpad-4 and 6 */
		static void HandleChangeItem(bool right);

	private:
		//This is a list (or stack) of the active menus, the highest index is the one that's currently in focus
		//The reason this is a List and not a Stack is because we need to be able to access and draw the unfocused windows too
		static System::Collections::Generic::List<Menu ^> ^mMenuStack = gcnew System::Collections::Generic::List<Menu ^>();
	};
}