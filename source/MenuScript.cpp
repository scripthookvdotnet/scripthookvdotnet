#include "MenuScript.hpp"

namespace GTA
{
	MenuScript::MenuScript()
	{
		View = gcnew Viewport();
		Tick += gcnew System::EventHandler(this, &GTA::MenuScript::UpdateViewport);
		KeyUp += gcnew System::Windows::Forms::KeyEventHandler(this, &GTA::MenuScript::HandleViewportInput);
	}

	void MenuScript::UpdateViewport(Object ^sender, System::EventArgs ^e)
	{
		View->Draw();
	}
	
	void MenuScript::HandleViewportInput(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^e)
	{
		using namespace System::Windows::Forms;
		switch (e->KeyCode)
		{
		case Keys::NumPad5:
			this->View->HandleActivate();
			return;
		case Keys::NumPad0:
			this->View->HandleBack();
			return;
		case Keys::NumPad4:
			this->View->HandleChangeItem(false);
			return;
		case Keys::NumPad6:
			this->View->HandleChangeItem(true);
			return;
		case Keys::NumPad8:
			this->View->HandleChangeSelection(false);
			return;
		case Keys::NumPad2:
			this->View->HandleChangeSelection(true);
		}
	}
}
