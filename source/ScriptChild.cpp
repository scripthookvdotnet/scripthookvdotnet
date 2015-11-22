#include "ScriptChild.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	using namespace System;

	extern void Log(String ^logLevel, ... array<String ^> ^message);

	ScriptChild::ScriptChild() 
	{
		_parent = ScriptDomain::ExecutingScript;
		if (_parent == nullptr)
		{
			throw gcnew Exception("Unable to determine the owning Script for this object!");
		}
	}

	Script ^ScriptChild::Parent::get()
	{
		return _parent;
	}
}