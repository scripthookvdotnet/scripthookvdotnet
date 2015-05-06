#pragma once

namespace GTA
{
	public interface class UIElement
	{
		void Draw();
		void Draw(int xMod, int yMod);

		property bool Enabled
		{
			void set(bool value);
			bool get();
		}
		property System::Drawing::Color Color
		{
			void set(System::Drawing::Color value);
			System::Drawing::Color get();
		}
		property System::Drawing::Point Location
		{
			void set(System::Drawing::Point value);
			System::Drawing::Point get();
		}
	};
}