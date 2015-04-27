#pragma once

namespace GTA
{
	ref class ScriptDomain;

	public ref class Script abstract
	{
	public:
		Script();

		event System::EventHandler ^Tick;
		event System::Windows::Forms::KeyEventHandler ^KeyUp;
		event System::Windows::Forms::KeyEventHandler ^KeyDown;

		property System::String ^Name
		{
			System::String ^get()
			{
				return GetType()->FullName;
			}
		}
		property System::String ^Filename
		{
			System::String ^get()
			{
				return this->mFilename;
			}
		}

		void Abort();

		virtual System::String ^ToString() override
		{
			return Name;
		}

	protected:
		property int Interval
		{
			int get();
			void set(int value);
		}

		bool IsKeyPressed(System::Windows::Forms::Keys key);

	internal:
		void RaiseTick(System::Object ^sender)
		{
			Tick(sender, System::EventArgs::Empty);
		}
		void RaiseKeyUp(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^args)
		{
			KeyUp(sender, args);
		}
		void RaiseKeyDown(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^args)
		{
			KeyDown(sender, args);
		}

		int mInterval;
		bool mRunning;
		System::DateTime mNextTick;
		ScriptDomain ^mScriptDomain;
		System::String ^mFilename;
	};
}