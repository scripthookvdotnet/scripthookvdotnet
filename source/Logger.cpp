#include "Logger.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	Logger::Logger()
	{
		this->assembly = Reflection::Assembly::GetExecutingAssembly();
	}

	Logger::Logger(Reflection::Assembly ^assembly)
	{
		this->assembly = assembly;
	}

	void Logger::OnStart()
	{
		LogToFile("", false, "##########\n\n\n\n##########");
	}

	void Logger::Debug(... array<String ^> ^message)
	{
		LogToFile("[DEBUG]", true, message);
	}
	void Logger::Error(... array<String ^> ^message)
	{
		LogToFile("[ERROR]", true, message);
	}

	void Logger::DeleteOldLogs()
	{
		DateTime now = DateTime::Now;
		for each(System::String^ path in IO::Directory::GetFiles(IO::Path::GetDirectoryName(GetAssemblyPath()), "*.log"))
		{
			if (path->Contains(IO::Path::GetFileNameWithoutExtension(GetAssemblyPath()))){
				DateTime logDate = DateTime::Parse(path->Substring(path->IndexOf("-") + 1, path->IndexOf(".log") - (path->IndexOf("-") + 1)));
				if ((now - logDate).Days >= maxLogAge){
					IO::File::Delete(path);
				}
			}
		}
	}

	void Logger::LogToFile(String ^logLevel, bool showTimeStamp, ... array<String ^> ^message)
	{
		DateTime now = DateTime::Now;
		String ^logpath = IO::Path::ChangeExtension(GetAssemblyPath(), ".log");

		logpath = logpath->Insert(logpath->IndexOf(".log"), "-" + now.ToString("yyyy-MM-dd"));

		try
		{
			IO::FileStream ^fs = gcnew IO::FileStream(logpath, IO::FileMode::Append, IO::FileAccess::Write, IO::FileShare::Read);
			IO::StreamWriter ^sw = gcnew IO::StreamWriter(fs);

			try
			{
				if (showTimeStamp)
				{
					sw->Write(String::Concat("[", now.ToString("HH:mm:ss"), "] ", logLevel, " "));
				}
				else
				{
					sw->Write(String::Concat(logLevel, " "));
				}

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

	System::String^ Logger::GetAssemblyPath()
	{
		return assembly->CodeBase->Replace("file:///", "")->Replace("/", "\\");
	}
}