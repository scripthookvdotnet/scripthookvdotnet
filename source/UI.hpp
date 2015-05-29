#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class UI sealed abstract
	{
	public:

		const int WIDTH = 1280;
		const int HEIGHT = 720;

		static int Notify(System::String ^msg);
		static void RemoveNotification(int notification);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);
	};
}
