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

#include "Menu.hpp"
#include "Settings.hpp"
#include "Script.hpp"

#using "ScriptHookVDotNet.asi"

using namespace System;
using namespace System::Threading;
using namespace System::Collections::Concurrent;
namespace WinForms = System::Windows::Forms;

namespace GTA
{
	extern void HandleUnhandledException(Object ^sender, UnhandledExceptionEventArgs ^args);

	Script::Script()
	{
		_filename = SHVDN::ScriptDomain::CurrentDomain->LookupScriptFilename(this->GetType());
	}
	Script::~Script()
	{
	}

	Viewport ^Script::View::get()
	{
		if (Object::ReferenceEquals(_viewport, nullptr))
		{
			_viewport = gcnew GTA::Viewport();

			Tick += gcnew EventHandler(this, &Script::HandleViewportDraw);
			KeyUp += gcnew WinForms::KeyEventHandler(this, &Script::HandleViewportInput);
		}

		return _viewport;
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
		return SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Interval;
	}
	void Script::Interval::set(int value)
	{
		if (value < 0)
			value = 0;
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Interval = value;
	}

	void Script::Abort()
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Abort();
	}
	void Script::Wait(int ms)
	{
		SHVDN::Script ^script = SHVDN::ScriptDomain::CurrentDomain->ExecutingScript;

		if (ReferenceEquals(script, nullptr) || !script->IsRunning)
			throw gcnew InvalidOperationException("Illegal call to 'Script.Wait()' outside main loop!");
		
		script->Wait(ms);
	}
	void Script::Yield()
	{
		Wait(0);
	}

	void Script::HandleViewportDraw(Object ^sender, EventArgs ^e)
	{
		_viewport->Draw();
	}
	void Script::HandleViewportInput(Object ^sender, WinForms::KeyEventArgs ^e)
	{
		if (e->KeyCode == ActivateKey) _viewport->HandleActivate();
		else if (e->KeyCode == BackKey) _viewport->HandleBack();
		else if (e->KeyCode == LeftKey) _viewport->HandleChangeItem(false);
		else if (e->KeyCode == RightKey) _viewport->HandleChangeItem(true);
		else if (e->KeyCode == UpKey) _viewport->HandleChangeSelection(false);
		else if (e->KeyCode == DownKey) _viewport->HandleChangeSelection(true);
	}

	void Script::Tick::add(System::EventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Tick += value;
	}
	void Script::Tick::remove(System::EventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Tick -= value;
	}

	void Script::KeyUp::add(System::Windows::Forms::KeyEventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->KeyUp += value;
	}
	void Script::KeyUp::remove(System::Windows::Forms::KeyEventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->KeyUp -= value;
	}

	void Script::KeyDown::add(System::Windows::Forms::KeyEventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->KeyDown += value;
	}
	void Script::KeyDown::remove(System::Windows::Forms::KeyEventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->KeyDown -= value;
	}

	void Script::Aborted::add(System::EventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Aborted += value;
	}
	void Script::Aborted::remove(System::EventHandler ^value)
	{
		SHVDN::ScriptDomain::CurrentDomain->LookupScript(this)->Aborted -= value;
	}
}