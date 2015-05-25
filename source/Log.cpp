#include "Log.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	void Log::Debug(... array<String ^> ^message)
	{
		LogToFile("DEBUG", message);
	}
	void Log::Error(... array<String ^> ^message)
	{
		LogToFile("ERROR", message);
	}

	void Log::LogToFile(String ^logLevel, ... array<String ^> ^message)
	{
		String ^logpath = IO::Path::ChangeExtension(Reflection::Assembly::GetExecutingAssembly()->CodeBase->Replace("file:///", "")->Replace("/", "\\"), ".log");

		try
		{
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

			try
			{
				sw->Write(String::Concat("[", DateTime::Now.ToString("HH:mm:ss"), "] [", logLevel, "] "));

				for each (String ^string in message)
				{
					sw->Write(string);
				}

				sw->WriteLine();
			}
			finally
			{
				sw->Close();
				fs->Close();
			}
		}
		catch (...)
		{
			return;
		}
	}
}