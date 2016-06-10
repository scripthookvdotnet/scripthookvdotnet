/**
 * Copyright (C) 2015 crosire
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

#include "ScriptDomain.hpp"
#include "Settings.hpp"

using namespace System;
using namespace System::Threading;
using namespace System::Collections::Concurrent;
namespace WinForms = System::Windows::Forms;

namespace GTA
{
	extern void HandleUnhandledException(Object ^sender, UnhandledExceptionEventArgs ^args);

	Script::Script() : _filename(ScriptDomain::CurrentDomain->LookupScriptFilename(this)), _scriptdomain(ScriptDomain::CurrentDomain)
	{
	}

	ScriptSettings ^Script::Settings::get()
	{
		if (Object::ReferenceEquals(_settings, nullptr))
		{
			String ^path = IO::Path::ChangeExtension(_filename, ".ini");

			_settings = ScriptSettings::Load(path);
		}

		return _settings;
	}
	int Script::Interval::get()
	{
		return _interval;
	}
	void Script::Interval::set(int value)
	{
		if (value < 0)
		{
			value = 0;
		}

		_interval = value;
	}

	void Script::Abort()
	{
		try
		{
			Aborted(this, EventArgs::Empty);
		}
		catch (Exception ^ex)
		{
			HandleUnhandledException(this, gcnew UnhandledExceptionEventArgs(ex, true));
		}

		_waitEvent->Set();

		_scriptdomain->AbortScript(this);
	}
	void Script::Wait(int ms)
	{
		Script ^script = ScriptDomain::ExecutingScript;

		if (Object::ReferenceEquals(script, nullptr) || !script->_running)
		{
			throw gcnew InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
		}

		const auto resume = DateTime::UtcNow + TimeSpan::FromMilliseconds(ms);

		do
		{
			script->_waitEvent->Set();
			script->_continueEvent->WaitOne();
		}
		while (DateTime::UtcNow < resume);
	}
	void Script::Yield()
	{
		Wait(0);
	}

	void Script::MainLoop()
	{
		// Wait for domain to run scripts
		_continueEvent->WaitOne();

		// Run main loop
		while (_running)
		{
			Tuple<bool, WinForms::KeyEventArgs ^> ^keyevent = nullptr;

			// Process events
			while (_keyboardEvents->TryDequeue(keyevent))
			{
				try
				{
					if (keyevent->Item1)
					{
						KeyDown(this, keyevent->Item2);
					}
					else
					{
						KeyUp(this, keyevent->Item2);
					}
				}
				catch (Exception ^ex)
				{
					HandleUnhandledException(this, gcnew UnhandledExceptionEventArgs(ex, false));
					break;
				}
			}

			try
			{
				Tick(this, EventArgs::Empty);
			}
			catch (Exception ^ex)
			{
				HandleUnhandledException(this, gcnew UnhandledExceptionEventArgs(ex, true));

				Abort();
				break;
			}

			// Yield execution to next tick
			Wait(_interval);
		}
	}
}
