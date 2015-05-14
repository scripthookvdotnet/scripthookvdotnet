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

#include "Log.hpp"
#include "Settings.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	using namespace System;
	using namespace System::Windows::Forms;
	using namespace System::Collections::Concurrent;

	Script::Script() : mFiberHandle(nullptr), mKeyboardEvents(gcnew ConcurrentQueue<Tuple<bool, KeyEventArgs ^> ^>())
	{
		Interval = 0;

		View = gcnew Viewport();
		Tick += gcnew EventHandler(this, &Script::UpdateViewport);
		KeyUp += gcnew KeyEventHandler(this, &Script::HandleViewportInput);
	}
	Script::~Script()
	{
	}

	ScriptSettings ^Script::Settings::get()
	{
		if (Object::ReferenceEquals(this->mSettings, nullptr))
		{
			String ^path = IO::Path::ChangeExtension(this->mFilename, "ini");

			if (IO::File::Exists(path))
			{
				this->mSettings = ScriptSettings::Load(path);
			}
			else
			{
				this->mSettings = gcnew ScriptSettings();
			}
		}

		return this->mSettings;
	}
	int Script::Interval::get()
	{
		return this->mInterval;
	}
	void Script::Interval::set(int value)
	{
		if (value < 0)
		{
			value = 0;
		}

		this->mInterval = value;
		this->mNextTick = DateTime::Now + TimeSpan(0, 0, 0, 0, value);
	}

	void Script::Abort()
	{
		this->mScriptDomain->AbortScript(this);
	}
	void Script::Wait(int ms)
	{
		if (!this->mRunning)
		{
			throw gcnew InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
		}

		this->mNextTick = System::DateTime::Now + System::TimeSpan(0, 0, 0, 0, ms);

		this->mScriptDomain->YieldScript(this);
	}
	void Script::Yield()
	{
		Wait(0);
	}

	void Script::MainLoop()
	{
		Tuple<bool, KeyEventArgs ^> ^keyevent = nullptr;

		while (this->mRunning)
		{
			// Process events
			while (this->mKeyboardEvents->TryDequeue(keyevent))
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
					Log::Error("Caught unhandled exception:", Environment::NewLine, ex->ToString());
					break;
				}
			}

			// Fire tick event
			try
			{
				Tick(this, EventArgs::Empty);
			}
			catch (Exception ^ex)
			{
				Log::Error("Caught unhandled exception:", Environment::NewLine, ex->ToString());

				Abort();
				break;
			}

			// Yield execution to next tick
			Wait(this->mInterval);
		}
	}
	void Script::UpdateViewport(Object ^sender, EventArgs ^e)
	{
		View->Draw();
	}
	void Script::HandleViewportInput(Object ^sender, KeyEventArgs ^e)
	{
		if (e->KeyCode == ActivateKey) View->HandleActivate();
		else if (e->KeyCode == BackKey) View->HandleBack();
		else if (e->KeyCode == LeftKey) View->HandleChangeItem(false);
		else if (e->KeyCode == RightKey) View->HandleChangeItem(true);
		else if (e->KeyCode == UpKey) View->HandleChangeSelection(false);
		else if (e->KeyCode == DownKey) View->HandleChangeSelection(true);
	}
}