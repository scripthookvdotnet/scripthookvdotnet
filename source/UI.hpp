#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class UI sealed abstract
	{
	public:
		ref class Notification
		{
		public:
			Notification(System::String ^Text) : Notification(Text, false){}
			Notification(System::String ^Text, bool blinking);

			void Hide();
		private:
			int mHandle;

		};
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		static void Notify(System::String ^msg);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);
	};
}
