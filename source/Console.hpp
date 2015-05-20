#include "UIRectangle.hpp"
#include "UIText.hpp"

namespace GTA
{
	using namespace System::Windows::Forms;

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
		virtual void HandleInput(bool status, KeyEventArgs ^args);
		virtual bool CheckInput(KeyEventArgs ^args);
		virtual System::String^ GetContent();
		virtual void SubmitInput();
		virtual void OnOpen();
		virtual void OnClose();

		property bool Enabled;
		property int Width;
		property int Height;
		property int Font;
		property float TextScale;
		property System::Drawing::Color TextColor;
		property System::Drawing::Color RectColor;

	private:
		UIRectangle ^mContentRect = nullptr, ^mInputRect = nullptr;
		UIText ^mContentText = nullptr, ^mInputText = nullptr;
		System::String ^mContent;
		System::String ^mInput;
		System::String ^mSeperator;
		int mTotalLines;
		int mScrollIndex;
	};
}