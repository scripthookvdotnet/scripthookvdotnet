#pragma once

#include "UIElement.hpp"

namespace GTA
{
	interface class IMenuItem;
	ref class Viewport;

	public ref class MenuItemIndexArgs : System::EventArgs
	{
	private:
		int mIndex;

	public:
		MenuItemIndexArgs(int index)
		{
			this->mIndex = index;
		}

		property int Index
		{
			int get() { return this->mIndex; }
		}
	};
	public ref class MenuItemDoubleValueArgs : System::EventArgs
	{
	private:
		double mValue;

	public:
		MenuItemDoubleValueArgs(double value)
		{
			this->mValue = value;
		}

		property double Index
		{
			double get() { return this->mValue; }
		}
	};
	public ref class SelectedIndexChangedArgs :System::EventArgs
	{
	private:
		int mSelectedIndex;

	public:
		SelectedIndexChangedArgs(int selectedIndex)
		{
			this->mSelectedIndex = selectedIndex;
		}

		property int SelectedIndex
		{
			int get() { return this->mSelectedIndex; }
		}
	};

	public ref class MenuBase
	{
	public:
		/** Draws the menu */
		virtual void Draw() { }

		//Drawing with an offset implies that this menu is not the active one
		/** Draws the menu with an offset */
		virtual void Draw(System::Drawing::Size offset) { }

		/** Called when the menu is first added to the Viewport */
		virtual void Initialize() { }

		/** Called when the menu gains or regains focus */
		virtual void OnOpen() { }

		/** Called when the user hits the back button or unfocuses from this menu */
		virtual void OnClose() { }

		/** Called when the user hits the activate button */
		virtual void OnActivate() { }

		/** Called when the user changes what element is selected (i.e. up and down) */
		virtual void OnChangeSelection(bool down) { }

		/** Called when the user changes the current element (i.e. left and right) */
		virtual void OnChangeItem(bool right) { }

	public:
		property Viewport ^Parent;
		property System::Drawing::Color HeaderColor;
		property System::Drawing::Color HeaderTextColor;
		property Font HeaderFont;
		property float HeaderTextScale;
		property bool HeaderCentered;
		property System::Drawing::Color FooterColor;
		property System::Drawing::Color FooterTextColor;
		property Font FooterFont;
		property float FooterTextScale;
		property bool FooterCentered;
		property System::Drawing::Color SelectedItemColor;
		property System::Drawing::Color UnselectedItemColor;
		property System::Drawing::Color SelectedTextColor;
		property System::Drawing::Color UnselectedTextColor;
		property Font ItemFont;
		property float ItemTextScale;
		property bool ItemTextCentered;
		property System::Drawing::Point Position;
		property System::Drawing::Point TextOffset;
		property System::String ^Caption;
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
	public ref class Menu : MenuBase
	{
	public:
		Menu(System::String ^headerCaption, array<IMenuItem ^> ^items);
		Menu(System::String ^headerCaption, array<IMenuItem ^> ^items, int MaxItemsToDraw);

	public:
		virtual void Draw() override;
		virtual void Draw(System::Drawing::Size offset) override;
		virtual void Initialize() override;
		virtual void OnOpen() override;
		virtual void OnClose() override;
		virtual void OnActivate() override;
		virtual void OnChangeSelection(bool down) override;
		virtual void OnChangeSelection(int newIndex);
		virtual void OnChangeItem(bool right) override;

	public:
		property int Width;
		property int HeaderHeight;
		property int FooterHeight;
		property int ItemHeight;
		property bool HasFooter;

		property System::Collections::Generic::List<IMenuItem ^> ^Items
		{
			System::Collections::Generic::List<IMenuItem ^> ^get() { return mItems; }
			void set(System::Collections::Generic::List<IMenuItem ^> ^items) {
				mItems = items;
			}
		}
		property int SelectedIndex
		{
			int get() { return mSelectedIndex; }
			void set(int newIndex)
			{
				OnChangeSelection(newIndex);
			}
		}
		property int MaxDrawLimit
		{
			int get() { return mMaxDrawLimit; }
			void set(int limit)
			{
				if (limit < 6 || limit > 20)
					throw gcnew System::ArgumentOutOfRangeException("MaxDrawLimit", "MaxDrawLimit must be between 6 and 20");
				mMaxDrawLimit = limit;
				StartScrollOffset = StartScrollOffset;//make sure value still falls in correct range
				OnChangeDrawLimit();
			}
		}
		property int StartScrollOffset
		{
			int get() { return mStartScrollOffset; }
			void set(int offset)
			{
				if (offset < 0)
					mStartScrollOffset = 0;
				else if (offset > MaxDrawLimit / 2 - 1)
					mStartScrollOffset = MaxDrawLimit / 2 - 1;
				else
					mStartScrollOffset = offset;
			}
		}

	public:
		event System::EventHandler<SelectedIndexChangedArgs^> ^SelectedIndexChanged;

	private:
		void OnChangeDrawLimit();
		void UpdateItemPositions();
		void DrawScrollArrows(bool up, bool down, System::Drawing::Size offset);
		property int mMaxScrollOffset
		{
			int get()
			{
				return mItems->Count < mItemDrawCount ? 0 : mItems->Count - mItemDrawCount;
			}
		}
		property int scrollOffset
		{
			int get()
			{
				return mScrollOffset;
			}
			void set(int value)
			{
				if (value > mMaxScrollOffset)
					mScrollOffset = mMaxScrollOffset;
				else if (value < 0)
					mScrollOffset = 0;
				else
					mScrollOffset = value;
				UpdateItemPositions();
			}
		}
		property int mItemDrawCount
		{
			int get()
			{
				return mItems->Count < mMaxDrawLimit ? mItems->Count : mMaxDrawLimit;
			}
		}
		UIRectangle ^mHeaderRect = nullptr, ^mFooterRect = nullptr;
		UIText ^mHeaderText = nullptr, ^mFooterText = nullptr;

		System::Collections::Generic::List<IMenuItem ^> ^mItems = gcnew System::Collections::Generic::List<IMenuItem ^>();
		int mSelectedIndex = -1;
		int mScrollOffset = 0;
		int mMaxDrawLimit = 10;
		int mStartScrollOffset = 2;


		System::String ^mFooterDescription = "footer description";
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
	public ref class MessageBox : MenuBase
	{
	public:
		MessageBox(System::String ^headerCaption);

	public:
		virtual void Draw() override;
		virtual void Draw(System::Drawing::Size offset) override;
		virtual void Initialize() override;
		virtual void OnOpen() override;
		virtual void OnClose() override;
		virtual void OnActivate() override;
		virtual void OnChangeSelection(bool down) override;
		virtual void OnChangeItem(bool right) override;

	public:
		event System::EventHandler<System::EventArgs ^> ^Yes;
		event System::EventHandler<System::EventArgs ^> ^No;

	public:
		property int Width;
		property int Height;
		property int ButtonHeight;

		/** Use Ok and Cancel instead of Yes and No */
		property bool OkCancel;

	private:
		UIRectangle ^mBodyRect = nullptr, ^mYesRect = nullptr, ^mNoRect = nullptr;
		UIText ^mText, ^mYesText, ^mNoText;
		bool mSelection = true;
	};

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
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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
			System::String ^get() {
				return mCaption;
			}
			void set(System::String ^value) {
				mCaption = value;
				UpdateText();
			}
		}
		virtual property System::String ^Description;

	public:
		event System::EventHandler<System::EventArgs ^> ^Activated;

	private:
		System::String ^mCaption;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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
			bool get() {
				return mToggleSelection;
			}
			void set(bool value) {
				mToggleSelection = value;
				UpdateText();
			}
		}

	public:
		event System::EventHandler<System::EventArgs ^> ^Changed;

	private:
		bool mToggleSelection;

		void UpdateText();

		void ChangeSelection();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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
			int get() {
				return mTimesIncrement;
			}
			void set(int value) {
				mTimesIncrement = value;
				UpdateText();
			}
		}

		property double Value {
			double get() {
				return (double)TimesIncremented*Increment;
			}
		}

	public:
		event System::EventHandler<MenuItemDoubleValueArgs ^> ^Activated;
		event System::EventHandler<MenuItemDoubleValueArgs ^> ^Changed;

	private:
		int mTimesIncrement;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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
		virtual property System::String^ Value {
			System::String^ get() {
				return mEntries[Index];
			}
		}
		virtual property int Index {
			int get() {
				return mSelectedIndex;
			}
			void set(int value) {
				mSelectedIndex = value;
				UpdateText();
			}
		}

	public:
		event System::EventHandler<MenuItemIndexArgs ^> ^Activated;
		event System::EventHandler<MenuItemIndexArgs ^> ^Changed;

	private:
		int mSelectedIndex;
		array<System::String ^> ^mEntries;

		void UpdateText();

		UIRectangle ^mButton = nullptr;
		UIText ^mText = nullptr;

		System::Drawing::Point mOrigin = System::Drawing::Point();
		System::Drawing::Size mSize = System::Drawing::Size(100, 100);
	};
	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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
			System::String ^get() {
				return mCaption;
			}
			void set(System::String ^value) {
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

	[System::ObsoleteAttribute("The built-in menu implementation is obsolete and will be removed soon. Please consider using external alternatives instead.")]
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