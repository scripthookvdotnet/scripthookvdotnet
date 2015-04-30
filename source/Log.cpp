#include "Log.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	void Log::Debug(... array<System::String ^> ^message)
	{
		LogToFile("[DEBUG]", message);
	}

	void Log::Error(... array<System::String ^> ^message)
	{
		LogToFile("[ERROR]", message);
	}

	void Log::LogToFile(System::String ^logLevel, ... array<System::String ^> ^message)
	{
		String ^logpath = IO::Path::ChangeExtension(Reflection::Assembly::GetExecutingAssembly()->Location, ".log");
		IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
		IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

		sw->Write(String::Concat(logLevel, " [", DateTime::Now.ToString("HH:mm:ss"), "] "));

		for each (String ^string in message)
		{
			sw->Write(string);
		}

		sw->WriteLine();

		sw->Close();
		fs->Close();
	}
}