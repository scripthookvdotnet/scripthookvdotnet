#include "Console.hpp"

namespace GTA
{
	Console::Console(){
		Width = UI::WIDTH / 2;
		Height = UI::HEIGHT / 4;
		Font = 0;
		TextScale = 0.2F;
		TextColor = System::Drawing::Color::White;
		RectColor = System::Drawing::Color::Black;
	}

	void Console::Draw()
	{
		Draw(System::Drawing::Size(0, 0));
	}

	void Console::Draw(System::Drawing::Size offset)
	{
		if (mConsoleRect == nullptr || mConsoleText == nullptr || mInputRect == nullptr || mInputText == nullptr){
			return;
		}
		mConsoleRect->Draw(offset);
		mConsoleText->Draw(offset);
		mInputRect->Draw(offset);
		mInputText->Draw(offset);
	}

	void Console::Initialize(){
		mConsoleRect = gcnew UIRectangle(System::Drawing::Point(), System::Drawing::Size(Width, Height / 100 * 80), RectColor);
		mInputRect = gcnew UIRectangle(System::Drawing::Point(0, Height / 100 * 80), System::Drawing::Size(Width, Height / 100 * 20), RectColor);
		mConsoleText = gcnew UIText("TESTING 1234", System::Drawing::Point(), TextScale, TextColor);
		mInputText = gcnew UIText("INPUT 1234", System::Drawing::Point(0, Height / 100 * 80), TextScale, TextColor);
	}

	void Console::Show(){
		OnOpen();
	}

	void Console::Hide(){
		OnClose();
	}

	void Console::Log(System::String ^msg){
	}

	void Console::OnOpen(){
	}

	void Console::OnClose(){
	}
}