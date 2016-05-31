#include "UI.hpp"
#include "Native.hpp"

namespace GTA
{
	namespace UI
	{

		using namespace System;
		using namespace System::Drawing;

		Notification::Notification(int handle) : _handle(handle)
		{
		}

		void Notification::Hide()
		{
			Native::Function::Call(Native::Hash::_REMOVE_NOTIFICATION, _handle);
		}

		Notification ^Screen::Notify(String ^message)
		{
			return Notify(message, false);
		}
		Notification ^Screen::Notify(String ^message, bool blinking)
		{
			Native::Function::Call(Native::Hash::_SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");
			const int strLen = 99;
			for (int i = 0; i < message->Length; i += strLen)
			{
				System::String ^substr = message->Substring(i, System::Math::Min(strLen, message->Length - i));
				Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, substr);
			}

			return gcnew Notification(Native::Function::Call<int>(Native::Hash::_DRAW_NOTIFICATION, blinking, 1));
		}

		void Screen::ShowSubtitle(String ^message)
		{
			ShowSubtitle(message, 2500);
		}
		void Screen::ShowSubtitle(String ^message, int duration)
		{
			Native::Function::Call(Native::Hash::_SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");
			const int strLen = 99;
			for (int i = 0; i < message->Length; i += strLen)
			{
				System::String ^substr = message->Substring(i, System::Math::Min(strLen, message->Length - i));
				Native::Function::Call(Native::Hash::_ADD_TEXT_COMPONENT_STRING, substr);
			}
			Native::Function::Call(Native::Hash::_DRAW_SUBTITLE_TIMED, duration, 1);
		}

		bool Screen::IsHudComponentActive(HudComponent component)
		{
			return Native::Function::Call<bool>(Native::Hash::IS_HUD_COMPONENT_ACTIVE, static_cast<int>(component));
		}
		void Screen::ShowHudComponentThisFrame(HudComponent component)
		{
			Native::Function::Call(Native::Hash::SHOW_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
		}
		void Screen::HideHudComponentThisFrame(HudComponent component)
		{
			Native::Function::Call(Native::Hash::HIDE_HUD_COMPONENT_THIS_FRAME, static_cast<int>(component));
		}

		PointF Screen::WorldToScreen(Math::Vector3 position)
		{
			float pointX, pointY;

			if (!Native::Function::Call<bool>(Native::Hash::_WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
			{
				return Point();
			}

			return PointF(pointX * Screen::WIDTH, pointY * Screen::HEIGHT);
		}
	}
}