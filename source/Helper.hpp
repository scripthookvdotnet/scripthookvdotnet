#pragma once

namespace GTA
{
	ref class Helper sealed
	{
	public:
		static System::Random ^Random = gcnew System::Random();

	private:
		Helper() {}
	};
}
