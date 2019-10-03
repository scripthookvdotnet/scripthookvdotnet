#pragma once

namespace GTA
{
	public ref class Notification
	{
	public:
		void Hide();

	internal:
		Notification(int handle);

	private:
		int _handle;
	};
}
