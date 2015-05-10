#pragma once

#include "UIRectangle.hpp"
#include "UIText.hpp"
#include "Menu.hpp"

namespace GTA
{
	ref class MenuBase;

	public interface class MenuItem
	{
	public:
		/** Called when the MenuItem should be drawn */
		void Draw();

		/** Called when the MenuItem should be drawn with an offset */
		void Draw(System::Drawing::Point offset);

		/** Called when the user selects this item */
		void Select();
		
		/** Called when the user deselects this item */
		void Deselect();

		/** Called when the user activates this item (e.g. numpad-5) */
		void Activate();

		/** Called when the user changes this item (e.g. numpad-4 and 6) */
		void Change(bool right);

		/** Called by the Menu to set this item's origin */
		void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		/** Set by the parent so that the MenuItem can access its properties */
		property MenuBase ^Parent;

		property System::String ^Caption;

		property System::String ^Description;
	};

	public ref class MenuButton : MenuItem
	{
	public:
		MenuButton(System::String ^caption, System::String ^description, System::Action ^activationAction);
		MenuButton(System::String ^caption, System::Action ^activationAction);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Point offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);
		virtual System::String ^GetDescription();
		
		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

	private:
		System::Action ^mActivationAction;

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
}