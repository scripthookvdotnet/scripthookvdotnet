//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public static class Audio
	{
		public static int PlaySoundAt(Entity entity, string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, id, soundFile, entity.Handle, 0, 0, 0);
			return id;
		}
		public static int PlaySoundAt(Entity entity, string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, id, soundFile, entity.Handle, soundSet, 0, 0);
			return id;
		}
		public static int PlaySoundAt(Vector3 position, string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, id, soundFile, position.X, position.Y, position.Z, 0, 0, 0, 0);
			return id;
		}
		public static int PlaySoundAt(Vector3 position, string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, id, soundFile, position.X, position.Y, position.Z, soundSet, 0, 0, 0);
			return id;
		}
		public static int PlaySoundFromEntity(Entity entity, string sound)
			=> PlaySoundAt(entity, sound);
		public static int PlaySoundFromEntity(Entity entity, string sound, string set)
			=> PlaySoundAt(entity, sound, set);

		public static int PlaySoundFrontend(string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FRONTEND, id, soundFile, 0, 0);
			return id;
		}
		public static int PlaySoundFrontend(string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FRONTEND, id, soundFile, soundSet, 0);
			return id;
		}

		public static void StopSound(int id)
		{
			Function.Call(Hash.STOP_SOUND, id);
		}
		public static void ReleaseSound(int id)
		{
			Function.Call(Hash.RELEASE_SOUND_ID, id);
		}
		public static bool HasSoundFinished(int id)
		{
			return Function.Call<bool>(Hash.HAS_SOUND_FINISHED, id);
		}

		public static void SetAudioFlag(string flag, bool toggle)
		{
			Function.Call(Hash.SET_AUDIO_FLAG, flag, toggle);
		}
		public static void SetAudioFlag(AudioFlag flag, bool toggle)
		{
			Function.Call(Hash.SET_AUDIO_FLAG, flag.ToString(), toggle);
		}
	}
}
