#include "Console.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Windows::Forms;

	Console::Console(){
		Width = UI::WIDTH / 2;
		Height = UI::HEIGHT / 4;
		Font = 0;
		TextScale = 0.2F;
		TextColor = System::Drawing::Color::White;
		RectColor = System::Drawing::Color::Black;
		mConsole = "";
		mInput = "";
	}

	void Console::Draw()
	{
		Draw(System::Drawing::Size(UI::WIDTH / 4, 0));
	}

	void Console::Draw(System::Drawing::Size offset)
	{
		if (mConsoleRect == nullptr || mConsoleText == nullptr || mInputRect == nullptr || mInputText == nullptr){
			return;
		}
		if (Enabled){
			mConsoleRect->Draw(offset);
			mConsoleText->Caption = mConsole;
			mConsoleText->Draw(offset);
			mInputRect->Draw(offset);
			mInputText->Caption = mInput;
			mInputText->Draw(offset);
		}
	}

	void Console::Initialize(){
		mConsoleRect = gcnew UIRectangle(System::Drawing::Point(), System::Drawing::Size(Width, Height / 100 * 80), RectColor);
		mInputRect = gcnew UIRectangle(System::Drawing::Point(0, Height / 100 * 80), System::Drawing::Size(Width, Height / 100 * 20), RectColor);
		mConsoleText = gcnew UIText("", System::Drawing::Point(), TextScale, TextColor);
		mInputText = gcnew UIText("", System::Drawing::Point(0, (Height / 100 * 80) + 5), TextScale, TextColor);
	}

	void Console::Show(){
		Enabled = true;
		OnOpen();
	}

	void Console::Hide(){
		Enabled = false;
		OnClose();
	}

	void Console::Toggle(){
		if (!Enabled){
			Show();
		}
		else{
			Hide();
		}
	}

	void Console::Log(System::String ^logLevel, System::String ^msg){
		mConsole += "\n[" + DateTime::Now.ToString("HH:mm:ss") + "] [" + logLevel + "] " + msg;
	}

	void Console::HandleInput(KeyEventArgs ^args){
		if (args->KeyCode == Keys::Tab){
			Toggle();
			return;
		}
		if (Enabled){
			switch (args->KeyCode){
			case Keys::Enter:
				SubmitInput();
				return;
			case Keys::Back:
				mInput = mInput->Substring(0, mInput->Length - 1);
				return;
			case Keys::PageUp:
				return;
			case Keys::PageDown:
				return;
			}
			mInput += args->KeyData.ToString();
		}
	}

	void Console::SubmitInput(){
		mConsole += "\n" + mInput;
		mInput = "";
	}

	void Console::OnOpen(){
	}

	void Console::OnClose(){
	}
}