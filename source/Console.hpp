#include "UIRectangle.hpp"
#include "UIText.hpp"

namespace GTA
{
	public ref class Console
	{
	public:
		Console();

	public:
		virtual void Draw();
		virtual void Draw(System::Drawing::Size offset);
		virtual void Initialize();
		virtual void Show();
		virtual void Hide();
		virtual void Log(System::String ^msg);
		virtual void OnOpen();
		virtual void OnClose();

	public:
		property int Width;
		property int Height;
		property int Font;
		property float TextScale;
		property System::Drawing::Color TextColor;
		property System::Drawing::Color RectColor;

	private:
		UIRectangle ^mConsoleRect = nullptr, ^mInputRect = nullptr;
		UIText ^mConsoleText = nullptr, ^mInputText = nullptr;
	};
}