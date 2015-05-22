#pragma once
#include "Vector3.hpp"
namespace GTA
{
	public ref class UI sealed abstract
	{
	public:
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		static void Notify(System::String ^msg);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::PointF WorldToScreen(Math::Vector3 position);
	};
}