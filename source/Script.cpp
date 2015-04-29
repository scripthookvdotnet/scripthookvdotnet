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
#include "ScriptDomain.hpp"

namespace GTA
{
	Script::Script()
	{
		Interval = 0;
	}

	void Script::Wait(int ms)
	{
		ScriptDomain::CurrentDomain->Wait(ms);
	}
	bool Script::IsKeyPressed(System::Windows::Forms::Keys key)
	{
		return ScriptDomain::CurrentDomain->IsKeyPressed(key);
	}

	ScriptSettings ^Script::Settings::get()
	{
		if (Object::ReferenceEquals(this->mSettings, nullptr))
		{
			this->mSettings = ScriptSettings::Load(System::IO::Path::ChangeExtension(this->mFilename, "ini"));
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
}