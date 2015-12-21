#include "Audio.hpp"
#include "Native.hpp"
#include "Entity.hpp"

namespace GTA
{
	int Audio::GetSoundID()
	{
		return Native::Function::Call<int>(Native::Hash::GET_SOUND_ID);
	}
	bool Audio::HasSoundFinished(int id)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_SOUND_FINISHED, id);
	}
	void Audio::PlaySoundFromCoord(System::String ^sound, Math::Vector3 pos)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, pos.X, pos.Y, pos.Z, 0, 0, 0, 0);
	}
	void Audio::PlaySoundFromCoord(System::String ^sound, Math::Vector3 pos, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, pos.X, pos.Y, pos.Z, set, 0, 0, 0);
	}
	void Audio::PlaySoundFromCoord(System::String ^sound, float posX, float posY, float posZ)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, posX, posY, posZ, 0, 0, 0, 0);
	}
	void Audio::PlaySoundFromCoord(System::String ^sound, float posX, float posY, float posZ, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_COORD, -1, sound, posX, posY, posZ, set, 0, 0, 0);
	}
	void Audio::PlaySoundFromEntity(System::String ^sound, Entity ^entity)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, -1, sound, entity, 0, 0, 0);
	}
	void Audio::PlaySoundFromEntity(System::String ^sound, Entity ^entity, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, -1, sound, entity, set, 0, 0);
	}
	void Audio::PlaySoundFromEntity(int id, System::String ^sound, Entity ^entity)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, id, sound, entity, 0, 0, 0);
	}
	void Audio::PlaySoundFromEntity(int id, System::String ^sound, Entity ^entity, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FROM_ENTITY, id, sound, entity, set, 0, 0);
	}
	void Audio::PlaySoundFrontend(System::String ^sound)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, -1, sound, 0, 0);
	}
	void Audio::PlaySoundFrontend(System::String ^sound, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, -1, sound, set, 0);
	}
	void Audio::PlaySoundFrontend(int id, System::String ^sound)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, id, sound, 0, 0);
	}
	void Audio::PlaySoundFrontend(int id, System::String ^sound, System::String ^set)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, id, sound, set, 0);
	}
	void Audio::PlaySoundFrontend(int id, System::String ^sound, System::String ^set, int i)
	{
		Native::Function::Call(Native::Hash::PLAY_SOUND_FRONTEND, id, sound, set, i);
	}
	void Audio::ReleaseSoundId(int id)
	{
		Native::Function::Call(Native::Hash::RELEASE_SOUND_ID, id);
	}
	void Audio::SetAudioFlag(AudioFlag flag, bool toggle)
	{
		SetAudioFlag(sAudioFlags[static_cast<int>(flag)], toggle);
	}
	void Audio::SetAudioFlag(System::String ^flag, bool toggle)
	{
		Native::Function::Call(Native::Hash::SET_AUDIO_FLAG, flag, toggle);
	}
	void Audio::StopSound(int id)
	{
		Native::Function::Call(Native::Hash::STOP_SOUND, id);
	}
}
