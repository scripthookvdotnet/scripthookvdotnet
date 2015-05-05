#pragma once

namespace GTA
{
	public ref class Menu
	{
	public:
		/** Draws the menu */
		virtual void Draw();

		/** Called when the menu is first added to the viewport */
		virtual void OnOpen();

		/** Called when the user hits the back button */
		virtual void OnClose();

		/** Called when the user hits the activate button */
		virtual void OnActivate();

		/** Called when the user changes what element is selected (i.e. up and down) */
		virtual void OnChangeSelection(bool down);

		/** Called when the user changes the current element (i.e. left and right) */
		virtual void OnChangeElement(bool right);
	};
}