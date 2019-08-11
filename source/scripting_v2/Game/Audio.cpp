#include "Audio.hpp"
#include "Native.hpp"
#include "Entity.hpp"

namespace GTA
{
	int Audio::PlaySoundAt(Math::Vector3 position, System::String ^sound)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, position.X, position.Y, position.Z, 0, 0, 0, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	int Audio::PlaySoundAt(Math::Vector3 position, System::String ^sound, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, position.X, position.Y, position.Z, set, 0, 0, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	int Audio::PlaySoundFromEntity(Entity ^entity, System::String ^sound)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, -1, sound, entity, 0, 0, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	int Audio::PlaySoundFromEntity(Entity ^entity, System::String ^sound, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, -1, sound, entity, set, 0, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	int Audio::PlaySoundFrontend(System::String ^sound)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, -1, sound, 0, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	int Audio::PlaySoundFrontend(System::String ^sound, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, -1, sound, set, 0);

		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}

	void Audio::StopSound(int id)
	{
		Native::Function::Call(Native::Hash::STOP_SOUND, id);
	}
	void Audio::ReleaseSound(int id)
	{
		Native::Function::Call(Native::Hash::RELEASE_SOUND_ID, id);
	}
	bool Audio::HasSoundFinished(int id)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_SOUND_FINISHED, id);
	}

	void Audio::SetAudioFlag(AudioFlag flag, bool toggle)
	{
		SetAudioFlag(_audioFlags[static_cast<int>(flag)], toggle);
	}
	void Audio::SetAudioFlag(System::String ^flag, bool toggle)
	{
		Native::Function::Call(Native::Hash::SET_AUDIO_FLAG, flag, toggle);
	}
}
