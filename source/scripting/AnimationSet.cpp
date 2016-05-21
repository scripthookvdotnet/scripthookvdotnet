#include "AnimationSet.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	AnimationSet::AnimationSet(System::String ^name)
	{
		if (name == nullptr)
		{
			throw gcnew System::ArgumentNullException("name");
		}

		_name = name;
	}

	System::String ^AnimationSet::Name::get()
	{
		return _name;
	}
	bool AnimationSet::IsLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_ANIM_SET_LOADED, Name);
	}

	void AnimationSet::Request()
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_SET, Name);
	}
	bool AnimationSet::Request(int timeout)
	{
		Request();

		const System::DateTime endtime = timeout >= 0 ? System::DateTime::UtcNow + System::TimeSpan(0, 0, 0, 0, timeout) : System::DateTime::MaxValue;

		while (!IsLoaded)
		{
			Script::Yield();

			if (System::DateTime::UtcNow >= endtime)
			{
				return false;
			}
		}

		return true;
	}
	void AnimationSet::Dismiss()
	{
		Native::Function::Call(Native::Hash::REMOVE_ANIM_SET, Name);
	}
	bool AnimationSet::Equals(Object ^value)
	{
		if (value == nullptr)
			return false;

		if (value->GetType() != GetType())
			return false;

		return Equals(safe_cast<AnimationSet ^>(value));
	}
	bool AnimationSet::Equals(AnimationSet ^animDict)
	{
		return Name == animDict->Name;
	}
}