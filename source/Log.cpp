#include "Log.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	void Log::Debug(... array<String ^> ^message)
	{
		LogToFile(Reflection::Assembly::GetExecutingAssembly(), "[DEBUG]", message);
	}
	void Log::Error(... array<String ^> ^message)
	{
		LogToFile(Reflection::Assembly::GetExecutingAssembly(), "[ERROR]", message);
	}

	void Log::Debug(Reflection::Assembly ^assembly, ... array<String ^> ^message)
	{
		LogToFile(assembly, "[DEBUG]", message);
	}
	void Log::Error(Reflection::Assembly ^assembly, ... array<String ^> ^message)
	{
		LogToFile(assembly, "[ERROR]", message);
	}

	void Log::LogToFile(String ^logLevel, ... array<String ^> ^message)
	{
		LogToFile(Reflection::Assembly::GetExecutingAssembly(), logLevel, message);
	}

	void Log::LogToFile(Reflection::Assembly ^assembly, String ^logLevel, ... array<String ^> ^message)
	{
		String ^logpath = IO::Path::ChangeExtension(assembly->CodeBase->Replace("file:///", "")->Replace("/", "\\"), ".log");

		try
		{
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

			try
			{
				sw->Write(String::Concat("[", DateTime::Now.ToString("HH:mm:ss"), "] ", logLevel, " "));

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