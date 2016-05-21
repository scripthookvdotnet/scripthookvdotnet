#include "AnimationDictionary.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	AnimationDictionary::AnimationDictionary(System::String ^name)
	{
		if (name == nullptr)
		{
			throw gcnew System::ArgumentNullException("name");
		}

		_name = name;
	}

	System::String ^AnimationDictionary::Name::get()
	{
		return _name;
	}
	bool AnimationDictionary::IsLoaded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_ANIM_DICT_LOADED, Name);
	}
	bool AnimationDictionary::IsValid::get()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_ANIM_DICT_EXIST, Name);
	}

	void AnimationDictionary::Request()
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_DICT, Name);
	}
	bool AnimationDictionary::Request(int timeout)
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
	void AnimationDictionary::Dismiss()
	{
		Native::Function::Call(Native::Hash::REMOVE_ANIM_DICT, Name);
	}
	bool AnimationDictionary::Equals(Object ^value)
	{
		if (value == nullptr)
			return false;

		if (value->GetType() != GetType())
			return false;

		return Equals(safe_cast<AnimationDictionary ^>(value));
	}
	bool AnimationDictionary::Equals(AnimationDictionary ^animDict)
	{
		return Name == animDict->Name;
	}
}