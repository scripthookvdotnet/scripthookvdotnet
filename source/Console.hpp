#include "UIRectangle.hpp"
#include "UIText.hpp"

namespace GTA
{
	using namespace System::Windows::Forms;
	using namespace System::Collections::Generic;

	public ref class ConsoleCommand
	{
	public:
		ConsoleCommand(System::String ^name, System::String ^description, System::Func<array<System::String ^>^, System::String^> ^callback);

		property System::String ^Name
		{
			System::String ^get(){
				return mName;
			}
			void set(System::String ^value){
				mName = value;
			}
		}
		property System::String ^Description
		{
			System::String ^get(){
				return mDescription;
			}
			void set(System::String ^value){
				mDescription = value;
			}
		}
		property System::Func<array<System::String ^>^, System::String^> ^Callback
		{
			System::Func<array<System::String ^>^, System::String^> ^get(){
				return mCallback;
			}
			void set(System::Func<array<System::String ^>^, System::String^> ^value){
				mCallback = value;
			}
		}

	private:
		System::String ^mName;
		System::String ^mDescription;
		System::Func<array<System::String ^>^, System::String^> ^mCallback;
	};

	public ref class Console
	{
	public:
		Console();

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Show();
		virtual void Hide();
		virtual void Toggle();
		virtual void Log(System::String ^logLevel, System::String ^msg);
		virtual void AddToContent(System::String ^content);
		virtual void HandleInput(bool status, KeyEventArgs ^args);
		virtual bool CheckInput(KeyEventArgs ^args);
		virtual System::String^ GetContent();
		virtual void SubmitInput();
		virtual void OnOpen();
		virtual void OnClose();
		virtual void AddCommand(System::String ^script, System::String ^name, System::String ^description, System::Func<array<System::String ^>^, System::String^> ^callback);
		virtual void AddCommand(System::String ^script, ConsoleCommand ^command);
		virtual System::String^ RunInput(System::String ^input);

		property bool Enabled;
		property int Width;
		property int Height;
		property int Font;
		property float TextScale;
		property System::Drawing::Color TextColor;
		property System::Drawing::Color RectColor;
		property Dictionary<System::String^, Dictionary<System::String^, ConsoleCommand^>^> ^Commands
		{
			Dictionary<System::String^, Dictionary<System::String^, ConsoleCommand^>^> ^get(){
				return mCommands;
			}
			void set(Dictionary<System::String^, Dictionary<System::String^, ConsoleCommand^>^> ^value){
				mCommands = value;
			}
		}

	private:
		UIRectangle ^mContentRect = nullptr, ^mInputRect = nullptr;
		UIText ^mContentText = nullptr, ^mInputText = nullptr;
		System::String ^mContent;
		System::String ^mInput;
		System::String ^mSeperator;
		System::String ^mSpace;
		int mTotalLines;
		int mScrollIndex;
		Dictionary<System::String^, Dictionary<System::String^, ConsoleCommand^>^> ^mCommands = gcnew Dictionary<System::String^, Dictionary<System::String^, ConsoleCommand^>^>();
	};
}