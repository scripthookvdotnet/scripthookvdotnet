#pragma once

namespace GTA
{
	private ref class Log sealed abstract
	{
	public:
		static void Debug(... array<System::String ^> ^message);
		static void Error(... array<System::String ^> ^message);
		static void Debug(System::Reflection::Assembly ^assembly, ... array<System::String ^> ^message);
		static void Error(System::Reflection::Assembly ^assembly, ... array<System::String ^> ^message);
		static void LogToFile(System::String ^logLevel, ... array<System::String ^> ^message);
		static void LogToFile(System::Reflection::Assembly ^assembly, System::String ^logLevel, ... array<System::String ^> ^message);

	private:
	};
}