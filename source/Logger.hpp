#pragma once

namespace GTA
{
	public ref class Logger
	{
	public:
		Logger();
		Logger(System::String^ name);

		void OnStart();
		void Debug(... array<System::String ^> ^message);
		void Error(... array<System::String ^> ^message);
		void DeleteOldLogs();

	private:
		void LogToFile(System::String ^logLevel, bool showTimeStamp, ... array<System::String ^> ^message);

		System::String^ defaultPath;
		System::String^ path;
		int maxLogAge = 10;
	};
}