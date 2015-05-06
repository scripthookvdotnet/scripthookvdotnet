#pragma once

#include "UIRectangle.hpp"
#include "UIText.hpp"
#include "MenuItem.hpp"

namespace GTA
{
	interface class MenuItem;

	public interface class MenuBase
	{
		/** Draws the menu */
		void Draw();

		//Drawing with an offset implies that this menu is not the active one
		/** Draws the menu with an offset */
		void Draw(System::Drawing::Point offset);

		/** Called when the menu is first added to the Viewport */
		void Initialize();

		/** Called when the menu gains or regains focus */
		void OnOpen();

		/** Called when the user hits the back button or unfocuses from this menu */
		void OnClose();

		/** Called when the user hits the activate button */
		void OnActivate();

		/** Called when the user changes what element is selected (i.e. up and down) */
		void OnChangeSelection(bool down);

		/** Called when the user changes the current element (i.e. left and right) */
		void OnChangeItem(bool right);

		property System::Drawing::Color HeaderColor;
		property System::Drawing::Color HeaderTextColor;
		property int HeaderFont;
		property float HeaderTextScale;
		property bool HeaderCentered;
		property System::Drawing::Color FooterColor;		
		property System::Drawing::Color FooterTextColor;
		property int FooterFont;
		property float FooterTextScale;
		property bool FooterCentered;
		property System::Drawing::Color SelectedItemColor;
		property System::Drawing::Color UnselectedItemColor;
		property System::Drawing::Color SelectedTextColor;
		property System::Drawing::Color UnselectedTextColor;
		property int ItemFont;
		property float ItemTextScale;
		property bool ItemTextCentered;
	};

	public ref class Menu : MenuBase
	{
	public:
		Menu(System::String ^headerCaption, array<MenuItem ^> ^items);

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Point offset);
		virtual void Initialize();
		virtual void OnOpen();
		virtual void OnClose();
		virtual void OnActivate();
		virtual void OnChangeSelection(bool down);
		virtual void OnChangeItem(bool right);

	public:
		virtual property System::Drawing::Color HeaderColor;
		virtual property System::Drawing::Color HeaderTextColor;
		virtual property int HeaderFont;
		virtual property float HeaderTextScale;
		virtual property bool HeaderCentered;
		virtual property System::Drawing::Color FooterColor;		
		virtual property System::Drawing::Color FooterTextColor;
		virtual property int FooterFont;
		virtual property float FooterTextScale;
		virtual property bool FooterCentered;
		virtual property System::Drawing::Color SelectedItemColor;
		virtual property System::Drawing::Color UnselectedItemColor;
		virtual property System::Drawing::Color SelectedTextColor;
		virtual property System::Drawing::Color UnselectedTextColor;
		virtual property int ItemFont;
		virtual property float ItemTextScale;
		virtual property bool ItemTextCentered;

		property System::String ^Caption;
		property System::Drawing::Point Position;

		property int Width;
		property int HeaderHeight;
		property int FooterHeight;
		property int ItemHeight;
		property bool HasFooter;

	private:
		UIRectangle ^mHeaderRect = nullptr, ^mFooterRect = nullptr, ^mOverlayRect = nullptr;
		UIText ^mHeaderText = nullptr, ^mFooterText = nullptr;

		System::Collections::Generic::List<MenuItem ^> ^mItems = gcnew System::Collections::Generic::List<MenuItem ^>();
		int mSelectedIndex = -1;
		System::String ^mFooterDescription = "footer description";
	};
}