#include "UI.hpp"
#include "Native.hpp"

namespace GTA
{
	void UI::Notify(System::String ^msg)
	{
		Native::Function::Call(Native::Hash::_0x202709F4C58A0424, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);
		Native::Function::Call(Native::Hash::_0x2ED7843F8F801023, 0, 1);
	}
	void UI::ShowSubtitle(System::String ^msg)
	{
		ShowSubtitle(msg, 2500);
	}
	void UI::ShowSubtitle(System::String ^msg, int duration)
	{
		Native::Function::Call(Native::Hash::_0xB87A37EEB7FAA67D, "STRING");
		Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, msg);
		Native::Function::Call(Native::Hash::_0x9D77056A530643F6, duration, 1);
	}
	System::Drawing::Point UI::WorldToScreen(Math::Vector3 position)
	{
		float pointX, pointY;
		if (!Native::Function::Call<bool>(Native::Hash::_0x34E82F05DF2974F5, position.X, position.Y, position.Z, &pointX, &pointY))
			return System::Drawing::Point();
		return System::Drawing::Point(pointX * UI::WIDTH, pointY * UI::HEIGHT);
	}
}