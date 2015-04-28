/**
 * Copyright (C) 2015 Crosire
 *
 * This software is  provided 'as-is', without any express  or implied  warranty. In no event will the
 * authors be held liable for any damages arising from the use of this software.
 * Permission  is granted  to anyone  to use  this software  for  any  purpose,  including  commercial
 * applications, and to alter it and redistribute it freely, subject to the following restrictions:
 *
 *   1. The origin of this software must not be misrepresented; you must not claim that you  wrote the
 *      original  software. If you use this  software  in a product, an  acknowledgment in the product
 *      documentation would be appreciated but is not required.
 *   2. Altered source versions must  be plainly  marked as such, and  must not be  misrepresented  as
 *      being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */

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