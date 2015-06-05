#pragma once

namespace GTA
{
	ref class MenuBase;

	/**
	 * Static class that handles the active UIs and menus
	 */
	public ref class Viewport sealed
	{
	public:
		Viewport();

		/** Add a menu to the stack of active menus and set it as focused */
		void AddMenu(MenuBase ^newMenu);

		/** Remove a menu from the stack of active menus */
		void RemoveMenu(MenuBase ^menu);

		/** Remove the active menu from the stack, this will focus the next highest menu */
		void PopMenu();

		/** Closes all menus */
		void CloseAllMenus();

		/** Draw all the active UIs */
		void Draw();

		/** Handles when the activate button is pressed (e.g. numpad-5) */
		void HandleActivate();

		/** Handles when the back button is pressed (e.g. numpad-0) */
		void HandleBack();

		/** Handles when the user presses the up or down button (e.g. numpad-2 and 8) */
		void HandleChangeSelection(bool down);

		/** Handles when the user presses the left or right button (e.g. numpad-4 and 6) */
		void HandleChangeItem(bool right);

		/** The top left position of the current menu */
		property System::Drawing::Point MenuPosition;

		/** The offset each menu in the stack has from the one above it */
		property System::Drawing::Point MenuOffset;

		/** Have more than one menu on the screen on the time and transition them */
		property bool MenuTransitions;

		property int ActiveMenus
		{
			int get()
			{
				return mMenuStack->Count;
			}
		}

	private:
		//easeOutBack function
		float EaseOut(float time, float duration, float value0, float deltaValue);

	private:
		//This is a list (or stack) of the active menus, the highest index is the one that's currently in focus
		//The reason this is a List and not a Stack is because we need to be able to access and draw the unfocused windows too
		System::Collections::Generic::List<MenuBase ^> ^mMenuStack = gcnew System::Collections::Generic::List<MenuBase ^>();

		//The current time input for the ease function for the menu offset
		//1f means that the offset is full;
		float mEaseTime = 1.0f;
		//Are we currently easing the menu offsets?
		bool mIsEasing = false;
		//Are we easing in (false) or out (true)
		bool mEaseDirection = false;
		//Current ease offset
		System::Drawing::Point mEaseOffset = MenuOffset;
	};
}