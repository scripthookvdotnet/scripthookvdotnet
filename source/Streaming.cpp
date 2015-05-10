#include "Streaming.hpp"
#include "Native.hpp"

namespace GTA
{
	void Streaming::RequestAnimDict(System::String ^animSet)
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_DICT, animSet);
	}

	bool Streaming::HasAnimDictLoaded(System::String ^animSet)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_ANIM_DICT_LOADED, animSet);
	}
}
