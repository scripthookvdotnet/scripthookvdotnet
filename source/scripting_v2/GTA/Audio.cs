//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public static class Audio
	{
		public static int PlaySoundAt(Entity entity, string soundFile)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, soundFile, entity.Handle, 0, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}
		public static int PlaySoundAt(Entity entity, string soundFile, string soundSet)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, soundFile, entity.Handle, soundSet, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}
		public static int PlaySoundAt(Vector3 position, string soundFile)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, soundFile, position.X, position.Y, position.Z, 0, 0, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}
		public static int PlaySoundAt(Vector3 position, string soundFile, string soundSet)
		{
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, soundFile, position.X, position.Y, position.Z, soundSet, 0, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}

		public static int PlaySoundFrontend(string soundFile)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundFile, 0, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
		}
		public static int PlaySoundFrontend(string soundFile, string soundSet)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundFile, soundSet, 0);
			return Function.Call<int>(Hash.GET_SOUND_ID);
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
