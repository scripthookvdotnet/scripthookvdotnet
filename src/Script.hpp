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
		event Windows::Forms::KeyEventHandler ^KeyUp;
		event Windows::Forms::KeyEventHandler ^KeyDown;

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

		bool IsKeyPressed(Windows::Forms::Keys key);

	internal:
		void RaiseTick(Object ^sender)
		{
			Tick(sender, EventArgs::Empty);
		}
		void RaiseKeyUp(Object ^sender, Windows::Forms::KeyEventArgs ^args)
		{
			KeyUp(sender, args);
		}
		void RaiseKeyDown(Object ^sender, Windows::Forms::KeyEventArgs ^args)
		{
			KeyDown(sender, args);
		}

		int mInterval;
		bool mRunning;
		DateTime mNextTick;
		ScriptDomain ^mScriptDomain;
		String ^mFilename;
	};
}