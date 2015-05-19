#pragma once

namespace GTA
{
	private ref class Log sealed abstract
	{
	public:
		static void Debug(... array<System::String ^> ^message);
		static void Error(... array<System::String ^> ^message);
		static void LogToFile(System::String ^logLevel, ... array<System::String ^> ^message);

	private:
	};
}