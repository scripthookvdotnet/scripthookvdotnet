#pragma once

namespace GTA
{
	ref class Ped;

	public ref class Animation
	{
	public:
		bool IsPlaying(System::String ^animSet, System::String ^animName);

		void Play(System::String ^animSet, System::String ^animName, float speed, int loop, bool lastAnimation, float playbackRate);
		void Stop(System::String ^animSet, System::String ^animName);

	internal:
		Animation(Ped ^ped);

	private:
		Ped ^mPed;
	};
}