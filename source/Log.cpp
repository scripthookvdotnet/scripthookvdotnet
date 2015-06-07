#include "Log.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	void Log::OnStart()
	{
		LogToFile("", "##########\n\n\n\n##########");
	}

	void Log::Debug(... array<String ^> ^message)
	{
		LogToFile("[DEBUG]", message);
	}
	void Log::Error(... array<String ^> ^message)
	{
		LogToFile("[ERROR]", message);
	}

	void Log::DeleteOldLogs()
	{
		DateTime now = DateTime::Now;
		for each(System::String^ path in IO::Directory::GetFiles(IO::Path::GetDirectoryName(Reflection::Assembly::GetExecutingAssembly()->Location), "*.log"))
		{
			DateTime logDate = DateTime::Parse(path->Substring(path->IndexOf("-") + 1, path->IndexOf(".log") - (path->IndexOf("-") + 1)));
			if ((now - logDate).Days >= maxLogAge){
				IO::File::Delete(path);
			}
		}
	}

	void Log::LogToFile(String ^logLevel, ... array<String ^> ^message)
	{
		DateTime now = DateTime::Now;
		String ^logpath = IO::Path::ChangeExtension(Reflection::Assembly::GetExecutingAssembly()->Location, ".log");

		logpath = logpath->Insert(logpath->IndexOf(".log"), "-" + now.ToString("yyyy-MM-dd"));

		try
		{
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

			try
			{
				sw->Write(String::Concat("[", now.ToString("HH:mm:ss"), "] ", logLevel, " "));

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