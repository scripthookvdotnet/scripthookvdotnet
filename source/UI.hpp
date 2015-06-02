#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public enum class Font
	{
		ChaletLondon = 0,
		HouseScript = 1,
		Monospace = 2,
		ChaletComprimeCologne = 4,
		Pricedown = 7
	};
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

		static void Notify(System::String ^msg);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);
	};
}