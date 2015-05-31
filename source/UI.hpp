#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class Notification
	{
	public:
		static Notification ^Show(System::String ^Message){ return Show(Message, false); }
		static Notification ^Show(System::String ^Message, bool Blinking);
		void Hide();
	private:
		Notification(int Handle);
		int mHandle;
	};
	public ref class UI sealed abstract
	{
	public:
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		[System::ObsoleteAttribute("UI.Notify is obsolete, please use Notification.Show instead.")]
		static void Notify(System::String ^msg);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);
	};
}
