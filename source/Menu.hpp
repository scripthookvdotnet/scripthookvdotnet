#pragma once

#include "Viewport.hpp"
#include "UIRectangle.hpp"
#include "UIText.hpp"
#include "MenuItem.hpp"

namespace GTA
{
	interface class MenuItem;
	ref class Viewport;

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
			int get(){ return this->mSelectedIndex; }
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

	public ref class Menu : MenuBase
	{
	public:
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items);

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

		property System::Collections::Generic::List<MenuItem ^> ^Items
		{
			System::Collections::Generic::List<MenuItem ^> ^get() { return mItems; }
			void set(System::Collections::Generic::List<MenuItem ^> ^items){
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

	public:
		event System::EventHandler<SelectedIndexChangedArgs^> ^SelectedIndexChanged;

	private:
		UIRectangle ^mHeaderRect = nullptr, ^mFooterRect = nullptr;
		UIText ^mHeaderText = nullptr, ^mFooterText = nullptr;

		System::Collections::Generic::List<MenuItem ^> ^mItems = gcnew System::Collections::Generic::List<MenuItem ^>();
		int mSelectedIndex = -1;

		System::String ^mFooterDescription = "footer description";
	};

	public ref class ListMenu : MenuBase
	{
	public:
		ListMenu(System::String ^headerCaption, System::Action<ListMenu ^> ^activationAction);
		ListMenu(System::String ^headerCaption);

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
		void Add(System::String ^Caption, System::String ^Description);
		void Add(System::String ^Caption);
		void Remove(int Index);
		void Remove(System::String ^Caption);

		property System::Tuple<System::String ^, System::String ^> ^default[int]
		{
			System::Tuple<System::String ^, System::String ^> ^get(int index) { return mItems[index]; }
			void set(int index, System::Tuple<System::String ^, System::String ^> ^item)
			{
				mItems[index] = item;
				this->UpdateEntries();
			}
		}
		property int SelectedIndex
		{
			int get() { return mSelectedIndex; }
			void set(int NewIndex)
			{
				mSelectedIndex = NewIndex;
				UpdateHighlight();
			}
		}

	private:
		void UpdateEntries();
		void UpdateHighlight();

	public:
		property int Width;
		property int HeaderHeight;
		property int FooterHeight;
		property int ItemHeight;
		property bool HasFooter;

	public:
		event System::EventHandler<SelectedIndexChangedArgs^> ^SelectedIndexChanged;

	private:
		System::Action<ListMenu ^> ^mActivateAction;

		UIRectangle ^mHeaderRect = nullptr, ^mFooterRect = nullptr;
		UIText ^mHeaderText = nullptr, ^mFooterText = nullptr;

		System::Collections::Generic::List<System::Tuple<System::String ^, System::String ^> ^> ^mItems;
		System::Collections::Generic::List<System::Tuple<UIRectangle ^, UIText ^> ^> ^mUIEntries;
		int mEntriesHeight = -1;
		int mSelectedIndex = -1;
		System::String ^mFooterDescription = "footer description";
	};

	public ref class MessageBox : MenuBase {
	public:
		MessageBox(System::String ^headerCaption, System::Action ^yesAction, System::Action ^noAction);

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
		property int Width;
		property int Height;
		property int ButtonHeight;

		/** Use Ok and Cancel instead of Yes and No */
		property bool OkCancel;

	private:
		UIRectangle ^mBodyRect = nullptr, ^mYesRect = nullptr, ^mNoRect = nullptr;
		UIText ^mText, ^mYesText, ^mNoText;
		System::Action ^mYesAction = nullptr, ^mNoAction = nullptr;
		bool mSelection = true;
	};
}