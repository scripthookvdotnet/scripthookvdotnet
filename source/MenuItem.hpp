//Modeled after Nacorpio's GUI library

#pragma once

#include "UIRectangle.hpp"
#include "UIText.hpp"
#include "Menu.hpp"

namespace GTA
{
	ref class MenuBase;

	public interface class IMenuItem
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

	public ref class MenuButton : IMenuItem
	{
	public:
		MenuButton(System::String ^caption, System::String ^description);
		MenuButton(System::String ^caption);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption {
			System::String ^get(){
				return mCaption;
			}
			void set(System::String ^value){
				mCaption = value;
				UpdateText();
			}
		}
		virtual property System::String ^Description;

	private:
		System::String ^mCaption;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuToggle : IMenuItem
	{
	public:
		MenuToggle(System::String ^caption, System::String ^description);
		MenuToggle(System::String ^caption, System::String ^description, bool value);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;
		virtual property bool Value {
			bool get(){
				return mToggleSelection;
			}
			void set(bool value){
				mToggleSelection = value;
				UpdateText();
			}
		}

	private:
		bool mToggleSelection;

		void UpdateText();

		void ChangeSelection();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuNumericScroller : IMenuItem
	{
	public:
		MenuNumericScroller(System::String ^caption, System::String ^description, double min, double max, double inc);
		MenuNumericScroller(System::String ^caption, System::String ^description, double min, double max, double inc, int timesIncremented);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;

		property double Min;
		property double Max;
		property double Increment;
		property int DecimalFigures;

		property int TimesIncremented {
			int get(){
				return mTimesIncrement;
			}
			void set(int value){
				mTimesIncrement = value;
				UpdateText();
			}
		}

		property double Value {
			double get(){
				return (double)TimesIncremented*Increment;
			}
		}

	private:
		int mTimesIncrement;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuEnumScroller : IMenuItem
	{
	public:
		MenuEnumScroller(System::String ^caption, System::String ^description, array<System::String ^> ^entries);
		MenuEnumScroller(System::String ^caption, System::String ^description, array<System::String ^> ^entries, int value);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Select();
		virtual void Deselect();
		virtual void Activate();
		virtual void Change(bool right);
		virtual void SetOriginAndSize(System::Drawing::Point topLeftOrigin, System::Drawing::Size size);

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption;
		virtual property System::String ^Description;
		virtual property System::String^ Value{
			System::String^ get(){
				return mEntries[Index];
			}
		}
		virtual property int Index {
			int get(){
				return mSelectedIndex;
			}
			void set(int value){
				mSelectedIndex = value;
				UpdateText();
			}
		}

	private:
		int mSelectedIndex;
		array<System::String ^> ^mEntries;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};

	public ref class MenuLabel : IMenuItem
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

		virtual property MenuBase ^Parent;
		virtual property System::String ^Caption {
			System::String ^get(){
				return mCaption;
			}
			void set(System::String ^value){
				mCaption = value;
				UpdateText();
			}
		}
		virtual property System::String ^Description;

		property bool UnderlinedBelow;
		property bool UnderlinedAbove;
		property System::Drawing::Color UnderlineColor;
		property int UnderlineHeight;

	private:
		UIRectangle ^mButton = nullptr, ^mUnderlineBelow = nullptr, ^mUnderlineAbove = nullptr;
		UIText ^mText = nullptr;

		System::String ^mCaption;

		void UpdateText();

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
}