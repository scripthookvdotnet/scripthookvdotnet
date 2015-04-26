#pragma once

namespace GTA
{
	ref class ScriptDomain;

	using namespace System;

	public ref class Script abstract
	{
	public:
		Script();

		event EventHandler ^Tick;

		property String ^Name
		{
			String ^get()
			{
				return GetType()->FullName;
			}
		}
		property String ^Filename
		{
			String ^get()
			{
				return this->mFilename;
			}
		}

		void Abort();

		virtual String ^ToString() override
		{
			return Name;
		}

	protected:
		property int Interval
		{
			int get();
			void set(int value);
		}

	internal:
		void DoTick()
		{
			Tick(this, EventArgs::Empty);
		}

		int mInterval;
		bool mRunning;
		DateTime mNextTick;
		ScriptDomain ^mScriptDomain;
		String ^mFilename;
	};
}