#include "Ped.hpp"
#include "Animation.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	Animation::Animation(Ped ^ped) : mPed(ped)
	{
	}

	bool Animation::IsPlaying(System::String ^animSet, System::String ^animName)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_PLAYING_ANIM, this->mPed->ID, animSet, animName, 3);
	}

	void Animation::Play(System::String ^animSet, System::String ^animName, float speed, int loop, bool lastAnimation, float playbackRate)
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_DICT, animSet);

		const System::DateTime endtime = System::DateTime::Now + System::TimeSpan(0, 0, 0, 0, 1000);

		while (!Native::Function::Call<bool>(Native::Hash::HAS_ANIM_DICT_LOADED, animSet))
		{
			Script::Wait(0);

			if (System::DateTime::Now >= endtime)
			{
				return;
			}
		}

		Native::Function::Call(Native::Hash::TASK_PLAY_ANIM, this->mPed->ID, animSet, animName, speed, -8.0f, loop, lastAnimation, playbackRate, 0, 0, 0);
	}
	void Animation::Stop(System::String ^animSet, System::String ^animName)
	{
		Native::Function::Call(Native::Hash::STOP_ANIM_TASK, this->mPed->ID, animSet, animName, -4.0f);
	}
}