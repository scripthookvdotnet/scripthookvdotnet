//Modeled after Nacorpio's GUI library

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
		void Draw(System::Drawing::Size offset);

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
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		void UpdateText();

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

	public ref class MenuToggle : MenuItem
	{
	public:
		MenuToggle(System::String ^caption, System::String ^description, System::Action ^activationAction, System::Action ^deactivationAction);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		void UpdateText();

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

	private:
		System::Action ^mActivationAction;
		System::Action ^mDeactivationAction;

		bool mToggleSelection = false;

		void ChangeSelection();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuNumericScroller : MenuItem
	{
	public:
		MenuNumericScroller(System::String ^caption, System::String ^description, System::Action<double> ^changeAction, System::Action<double> ^activateAction, double min, double max, double inc);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		void UpdateText();

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

		property double Min;
		property double Max;
		property double Increment;
		property int DecimalFigures;

	private:
		System::Action<double> ^mChangeAction;
		System::Action<double> ^mActivateAction;

		property int TimesIncrement;

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuEnumScroller : MenuItem
	{
	public:
		MenuEnumScroller(System::String ^caption, System::String ^description, System::Action<int> ^changeAction, System::Action<int> ^activateAction, array<System::String ^> ^entries);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		void UpdateText();

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

	private:
		System::Action<int> ^mChangeAction;
		System::Action<int> ^mActivateAction;

		int mSelectedIndex;
		array<System::String ^> ^mEntries;

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuLabel : MenuItem
	{
	public:
		MenuLabel(System::String ^caption, bool underlined);
		MenuLabel(System::String ^caption);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		void UpdateText();

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

		property bool UnderlinedBelow;
		property bool UnderlinedAbove;
		property System::Drawing::Color UnderlineColor;
		property int UnderlineHeight;

	private:
		UIRectangle ^mButton = nullptr, ^mUnderlineBelow = nullptr, ^mUnderlineAbove = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
}