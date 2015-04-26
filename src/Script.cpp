#include "ScriptDomain.hpp"

namespace GTA
{
	Script::Script()
	{
		Interval = 0;
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