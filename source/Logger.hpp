#pragma once

namespace GTA
{
	public ref class Logger
	{
	public:
		Logger();
		Logger(System::Reflection::Assembly ^assembly);

		void OnStart();
		void Debug(... array<System::String ^> ^message);
		void Error(... array<System::String ^> ^message);
		void DeleteOldLogs();

	private:
		void LogToFile(System::String ^logLevel, ... array<System::String ^> ^message);
		System::String^ GetAssemblyPath();

		System::Reflection::Assembly ^assembly;
		int maxLogAge = 10;
	};
}