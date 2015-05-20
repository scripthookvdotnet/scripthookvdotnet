#pragma once

namespace GTA
{
	public ref class Rope sealed
	{
	public:
		Rope(int handle);

		property int Handle
		{
			int get();
		}

		void Delete();

	private:
		int mHandle;
	};
}
