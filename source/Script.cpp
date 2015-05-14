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

#include "Settings.hpp"
#include "Native.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	Script::Script()
	{
		Interval = 0;
		View = gcnew Viewport();
		Tick += gcnew System::EventHandler(this, &GTA::Script::UpdateViewport);
		KeyUp += gcnew System::Windows::Forms::KeyEventHandler(this, &GTA::Script::HandleViewportInput);
	}
	Script::~Script()
	{
	}

	void Script::Wait(int ms)
	{
		ScriptDomain::CurrentDomain->Wait(ms);
	}

	ScriptSettings ^Script::Settings::get()
	{
		if (Object::ReferenceEquals(this->mSettings, nullptr))
		{
			System::String ^path = System::IO::Path::ChangeExtension(this->mFilename, "ini");

			if (System::IO::File::Exists(path))
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
		if (value > 0)
		{
			this->mInterval = value;
			this->mNextTick = System::DateTime::Now + System::TimeSpan(0, 0, 0, 0, value);
		}
		else
		{
			this->mInterval = 0;
			this->mNextTick = System::DateTime::MinValue;
		}
	}

	void Script::Abort()
	{
		this->mScriptDomain->AbortScript(this);
	}
	
	void Script::UpdateViewport(Object ^sender, System::EventArgs ^e)
	{
		View->Draw();
	}
	
	void Script::HandleViewportInput(System::Object ^sender, System::Windows::Forms::KeyEventArgs ^e)
	{
		if (e->KeyCode == ActivateKey) View->HandleActivate();
		else if (e->KeyCode == BackKey) View->HandleBack();
		else if (e->KeyCode == LeftKey) View->HandleChangeItem(false);
		else if (e->KeyCode == RightKey) View->HandleChangeItem(true);
		else if (e->KeyCode == UpKey) View->HandleChangeSelection(false);
		else if (e->KeyCode == DownKey) View->HandleChangeSelection(true);
	}
}