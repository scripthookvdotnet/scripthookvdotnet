#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class UI sealed abstract
	{
	public:

		static property int WIDTH
		{
			int get();
		}
		static property int HEIGHT
		{
			int get();
		}

		static int Notify(System::String ^msg);
		static void RemoveNotification(int notification);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);
	};
}
