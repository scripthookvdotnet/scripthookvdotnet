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
		void Hide();

	internal:
		Notification(int Handle);

	private:
		int mHandle;
	};

	public ref class UI sealed abstract
	{
	public:
		static const int WIDTH = 1280;
		static const int HEIGHT = 720;

		static Notification ^Notify(System::String ^msg);
		static Notification ^Notify(System::String ^msg, bool blinking);

		static void ShowSubtitle(System::String ^msg);
		static void ShowSubtitle(System::String ^msg, int duration);

		static System::Drawing::Point WorldToScreen(Math::Vector3 position);

		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size);
		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::Size size, float rotation, System::Drawing::Color color);
		static void DrawTexture(System::String ^filename, int index, int level, int time, System::Drawing::Point pos, System::Drawing::PointF center, System::Drawing::Size size, float rotation, System::Drawing::Color color);
	internal:
		static System::Collections::Generic::Dictionary<System::String ^, int> ^sTextures = gcnew System::Collections::Generic::Dictionary<System::String ^, int>();
	};
}