#include "UI.hpp"
#include "Native.hpp"

namespace GTA
{
	Notification::Notification(int Handle) : mHandle(Handle)
	{
	}

	void Notification::Hide()
	{
		Native::Function::Call(Native::Hash::_REMOVE_NOTIFICATION, this->mHandle);
	}

	Notification ^UI::Notify(System::String ^msg, bool Blinking)
	{
		Native::Function::Call(Native::Hash::_SET_NOTIFICATION_TEXT_ENTRY, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);

		return gcnew Notification(Native::Function::Call<int>(Native::Hash::_DRAW_NOTIFICATION, Blinking, 1));
	}
	void UI::ShowSubtitle(System::String ^msg)
	{
		ShowSubtitle(msg, 2500);
	}
	void UI::ShowSubtitle(System::String ^msg, int duration)
	{
		Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY_2, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);
		Native::Function::Call(Native::Hash::_0x9D77056A530643F6, duration, 1);
	}
	System::Drawing::Point UI::WorldToScreen(Math::Vector3 position)
	{
		float pointX, pointY;

		if (!Native::Function::Call<bool>(Native::Hash::_WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
		{
			return System::Drawing::Point();
		}

		return System::Drawing::Point((int)pointX * UI::WIDTH, (int)pointY * UI::HEIGHT);
	}
}