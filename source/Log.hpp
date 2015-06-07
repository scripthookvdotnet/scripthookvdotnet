#pragma once

namespace GTA
{
	private ref class Log sealed abstract
	{
	public:
		static void OnStart();
		static void Debug(... array<System::String ^> ^message);
		static void Error(... array<System::String ^> ^message);
		static void DeleteOldLogs();

	private:
		static void LogToFile(System::String ^logLevel, ... array<System::String ^> ^message);

		static int maxLogAge = 10;
	};
}