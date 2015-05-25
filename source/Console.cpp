#include "Console.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Windows::Forms;

	Console::Console(){
		Width = UI::WIDTH;
		Height = UI::HEIGHT / 4;
		Font = 0;
		TextScale = 0.2F;
		TextColor = System::Drawing::Color::White;
		RectColor = System::Drawing::Color::FromArgb(135, System::Drawing::Color::Black);
		mContent = "";
		mInput = "";
		mSeperator = "\n";
		mSpace = " ";
		mTotalLines = 8;
		mScrollIndex = 0;

		mContentRect = gcnew UIRectangle(System::Drawing::Point(), System::Drawing::Size(Width, Height / 100 * 80), RectColor);
		mInputRect = gcnew UIRectangle(System::Drawing::Point(0, Height / 100 * 80), System::Drawing::Size(Width, Height / 100 * 20), RectColor);
		mContentText = gcnew UIText("", System::Drawing::Point(), TextScale, TextColor);
		mInputText = gcnew UIText("", System::Drawing::Point(0, (Height / 100 * 80) + 5), TextScale, TextColor);
	}

	void Console::Draw()
	{
		Draw(System::Drawing::Size(0, 0));
	}

	void Console::Draw(System::Drawing::Size offset)
	{
		if (mContentRect == nullptr || mContentText == nullptr || mInputRect == nullptr || mInputText == nullptr){
			return;
		}
		if (Enabled){
			mContentRect->Draw(offset);
			mContentText->Caption = GetContent();
			mContentText->Draw(offset);
			mInputRect->Draw(offset);
			mInputText->Caption = mInput;
			mInputText->Draw(offset);
		}
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
		AddToContent("[" + DateTime::Now.ToString("HH:mm:ss") + "] [" + logLevel + "] " + msg);
	}

	void Console::AddToContent(System::String ^content)
	{
		mContent += content + mSeperator;
	}

	void Console::HandleInput(bool status, KeyEventArgs ^args){
		if (!status){
			return;
		}
		if (args->KeyCode == Keys::F10){
			Toggle();
			return;
		}
		if (Enabled){
			switch (args->KeyCode){
			case Keys::Enter:
				if (mInput->Length > 0){
					SubmitInput();
				}
				return;
			case Keys::Back:
				if (mInput->Length > 0){
					mInput = mInput->Substring(0, mInput->Length - 1);
				}
				return;
			case Keys::PageUp:
				if (mScrollIndex > 0){
					mScrollIndex--;
				}
				return;
			case Keys::PageDown:
				if (mScrollIndex + 1 <= mContent->Split(mSeperator->ToCharArray())->Length){
					mScrollIndex++;
				}
				return;
			}
			if (CheckInput(args)){
				mInput += args->KeyData.ToString();
			}
		}
	}

	bool Console::CheckInput(KeyEventArgs ^args){
		if ((args->KeyCode >= Keys::A && args->KeyCode <= Keys::Z) ||
			(args->KeyCode >= Keys::D0 && args->KeyCode <= Keys::D9) ||
			(args->KeyCode >= Keys::NumPad0 && args->KeyCode <= Keys::NumPad9)){
			return true;
		}
		else{
			return false;
		}
	}

	System::String^ Console::GetContent(){
		System::String^ output = "";
		array<System::String^>^ content = mContent->Split(mSeperator->ToCharArray());
		if (content->Length > 0){
			int total = mTotalLines;
			int index = mScrollIndex - (total + 1);
			if (index < 0){
				index = 0;
			}
			if (content->Length < total){
				total = content->Length;
			}
			for (int i = 0; i < total; i++){
				output += content[i + index];
				if (i != total - 1){
					output += mSeperator;
				}
			}
		}
		return output;
	}

	void Console::SubmitInput(){
		AddToContent(mInput);
		mScrollIndex = mContent->Split(mSeperator->ToCharArray())->Length;
		AddToContent(RunInput(mInput));
		mInput = "";
	}

	void Console::OnOpen(){
	}

	void Console::OnClose(){
	}

	void Console::AddCommand(System::String ^script, System::String ^name, System::String ^description, System::Func<array<System::String^>^, System::String^> ^callback){
		AddCommand(script, gcnew ConsoleCommand(name, description, callback));
	}

	void Console::AddCommand(System::String ^script, ConsoleCommand ^command){
		if (!Commands->ContainsKey(script)){
			Commands->Add(script, gcnew Dictionary<System::String^, ConsoleCommand^>());
		}
		Commands[script]->Add(command->Name, command);
	}

	System::String^ Console::RunInput(System::String ^input){
		array<System::String^>^ args = input->Split(mSpace->ToCharArray());
		if (Commands->ContainsKey(args[0])){
			Dictionary<System::String^, ConsoleCommand^>^ scriptCommands = Commands[args[0]];
			if (scriptCommands->ContainsKey(args[1])){
				args->Resize<System::String^>(args, args->Length - 2);
				return scriptCommands[args[1]]->Callback->Invoke(args);
			}
		}
		else{
			Dictionary<System::String^, ConsoleCommand^>^ scriptCommands = Commands["scripthookvdotnet"];
			if (scriptCommands->ContainsKey(args[0])){
				args->Resize<System::String^>(args, args->Length - 1);
				return scriptCommands[args[0]]->Callback->Invoke(args);
			}
		}
		return "Command not found";
	}

	ConsoleCommand::ConsoleCommand(System::String ^name, System::String ^description, System::Func<array<System::String^>^, System::String^> ^callback){
		mName = name;
		mDescription = description;
		mCallback = callback;
	}
}