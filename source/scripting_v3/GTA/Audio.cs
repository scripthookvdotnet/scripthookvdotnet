//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Methods to manipulate audio.
	/// </summary>
	public static class Audio
	{
		#region Music

		/// <summary>
		/// Plays music from the game's music files.
		/// </summary>
		/// <param name="musicFile">The music file to play.</param>
		public static void PlayMusic(string musicFile)
		{
			Function.Call(Hash.TRIGGER_MUSIC_EVENT, musicFile);
		}
		/// <summary>
		/// Cancels playing a music file.
		/// </summary>
		/// <param name="musicFile">The music file to stop.</param>
		public static void StopMusic(string musicFile)
		{
			Function.Call(Hash.CANCEL_MUSIC_EVENT, musicFile);
		}

		#endregion

		#region Sounds

		/// <summary>
		/// Gets a <see cref="ScriptSoundId"/> instance of a triggered sound.
		/// This method returns a new <see cref="ScriptSoundId"/>, which is used for keeping track of sounds after they've been triggered -
		/// use this if you need to control a sound after it's been started, for instance to stop a looping sound, or to change a sound's pitch midway through playback.
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// <para>
		/// SoundIds can be reused, without needing to release them and grab a new one. If a sound's finished playing,
		/// you can reuse its <see cref="ScriptSoundId"/> to kick off another one. If the sound's not finished playing,
		/// it'll be stopped first (fading out or whatever is set up in RAVE by the sound designer), and the new one kicked off;
		/// usually it is safer to just get a new <see cref="ScriptSoundId"/>.
		/// </para>
		/// <para>
		/// Identifiers of <see cref="ScriptSoundId"/> are always integral values greater than or equal to zero;
		/// if a playback function has a SoundId field but the sound doesn't need to be altered after triggering
		/// then call the forget method variants in <see cref="Audio"/> class, rather than getting a <see cref="ScriptSoundId"/>.
		/// </para>
		/// <para>
		/// Scripted sound id can be reserved up to 100 in the <c>audScriptAudioEntity</c> instance.
		/// The limit is shared among all scripts.
		/// </para>
		/// </remarks>
		/// <returns>
		/// A <see cref="ScriptSoundId"/> instance with the assigned id in the range of from 0 to 100 if the method successfully found a free id/index;
		/// otherwise, <see langword="null"/>.
		/// </returns>
		public static ScriptSoundId GetSoundId()
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			return id >= 0 ? new ScriptSoundId(id) : null;
		}

		/// <summary>
		/// Plays back a sound with the name <paramref name="soundName"/>, but do not track of sounds.
		/// If this is used to play a sound for which no pan or speakermask is set by the sound designer, then the sound will play from the map's origin -
		/// therefore this should only be used to play frontend sounds like menu bleeps or other artificially panned effects.
		/// </summary>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="enableOnReplay"><inheritdoc cref="ScriptSoundId.PlaySoundFrontend(string, string, bool)" path="/param[@name='enableOnReplay']"/></param>.
		public static void PlaySoundAndForget(string soundName, string setName, bool enableOnReplay = true)
		{
			Function.Call(Hash.PLAY_SOUND, -1, soundName, setName, enableOnReplay);
		}
		/// <summary>
		/// Plays back a sound "frontend" - at full volume, panned centrally, but do not track of sounds.
		/// </summary>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="enableOnReplay"><inheritdoc cref="ScriptSoundId.PlaySoundFrontend(string, string, bool)" path="/param[@name='enableOnReplay']"/></param>.
		/// <remarks>
		/// If the sound has a Pan or a SpeakerMask set by the sound designer then the it will play using these settings,
		/// otherwise it will play from dead ahead (0°).
		/// </remarks>
		public static void PlaySoundFrontendAndForget(string soundName, string setName, bool enableOnReplay = true)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, soundName, setName, enableOnReplay);
		}
		/// <summary>
		/// Plays back a sound from an <see cref="Entity"/>'s location, but do not track of sounds.
		/// The sound's position will track the <see cref="Entity"/>'s position as it moves.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to play the sound from.</param>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		public static void PlaySoundFromEntityAndForget(Entity entity, string soundName, string setName = null)
			=> Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, -1, soundName, entity.Handle, setName, false, 0);
		/// <summary>
		/// Plays back a sound from an absolute position, but do not track of sounds.
		/// </summary>
		/// <param name="position">The world coordinates to play the sound from.</param>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="isExteriorLoc">
		/// If <see langword="true"/>, the sound will use a portal occlusion environmentGroup.
		/// Only use this if the sound is playing outside and needs occlusion.
		/// </param>
		public static void PlaySoundFromPositionAndForget(Vector3 position, string soundName, string setName = null, bool isExteriorLoc = false)
			=> Function.Call(Hash.PLAY_SOUND_FROM_COORD, -1, soundName, position.X, position.Y, position.Z, setName, false, 0, isExteriorLoc);

		/// <summary>
		/// Plays a sound from the game's sound files at the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity to play the sound at.</param>
		/// <param name="soundFile">The sound file to play.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Audio.PlaySoundAt is obsolete, use ScriptSoundId.PlaySoundFromEntity or Audio.PlaySoundFromEntityAndForget.")]
		public static int PlaySoundAt(Entity entity, string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, id, soundFile, entity.Handle, 0, 0, 0);
			return id;
		}
		/// <summary>
		/// Plays a sound from the game's sound files at the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity to play the sound at.</param>
		/// <param name="soundFile">The sound file to play.</param>
		/// <param name="soundSet">The name of the sound inside the file.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Use ScriptSoundId.PlaySoundFromEntity or Audio.PlaySoundFromEntityAndForget instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static int PlaySoundAt(Entity entity, string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, id, soundFile, entity.Handle, soundSet, 0, 0);
			return id;
		}
		/// <summary>
		/// Plays a sound from the game's sound files at the specified <paramref name="position"/>.
		/// </summary>
		/// <param name="position">The world coordinates to play the sound at.</param>
		/// <param name="soundFile">The sound file to play.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Use ScriptSoundId.PlaySoundFromPosition or Audio.PlaySoundFromPositionAndForget instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static int PlaySoundAt(Vector3 position, string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, id, soundFile, position.X, position.Y, position.Z, 0, 0, 0, 0);
			return id;
		}
		/// <summary>
		/// Plays a sound from the game's sound files at the specified <paramref name="position"/>.
		/// </summary>
		/// <param name="position">The world coordinates to play the sound at.</param>
		/// <param name="soundFile">The sound file to play.</param>
		/// <param name="soundSet">The name of the sound inside the file.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Use ScriptSoundId.PlaySoundFromPosition or Audio.PlaySoundFromPositionAndForget instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static int PlaySoundAt(Vector3 position, string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FROM_COORD, id, soundFile, position.X, position.Y, position.Z, soundSet, 0, 0, 0);
			return id;
		}
		/// <summary>
		/// Plays a sound from the game's sound files without transformation.
		/// </summary>
		/// <param name="soundFile">The sound file to play.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Use ScriptSoundId.PlaySoundFrontend or Audio.PlaySoundFrontendAndForget instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static int PlaySoundFrontend(string soundFile)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FRONTEND, id, soundFile, 0, 0);
			return id;
		}
		/// <summary>
		/// Plays a sound from the game's sound files without transformation.
		/// </summary>
		/// <param name="soundFile">The sound file to play.</param>
		/// <param name="soundSet">The name of the sound inside the file.</param>
		/// <returns>The identifier of the active sound effect instance.</returns>
		[Obsolete("Use ScriptSoundId.PlaySoundFrontend or Audio.PlaySoundFrontendAndForget instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static int PlaySoundFrontend(string soundFile, string soundSet)
		{
			int id = Function.Call<int>(Hash.GET_SOUND_ID);
			Function.Call(Hash.PLAY_SOUND_FRONTEND, id, soundFile, soundSet, 0);
			return id;
		}

		/// <summary>
		/// Cancels playing the specified sound instance.
		/// </summary>
		/// <param name="id">The identifier of the active sound effect instance.</param>
		[Obsolete("Use ScriptSoundId.Stop instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static void StopSound(int id)
		{
			Function.Call(Hash.STOP_SOUND, id);
		}
		/// <summary>
		/// Releases the specified sound instance. Call this for every sound effect started.
		/// </summary>
		/// <param name="id">The identifier of the active sound effect instance.</param>
		[Obsolete("Use ScriptSoundId.Release instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static void ReleaseSound(int id)
		{
			Function.Call(Hash.RELEASE_SOUND_ID, id);
		}

		/// <summary>
		/// Gets a boolean indicating whether the specified sound instance has completed playing.
		/// </summary>
		/// <param name="id">The identifier of the active sound effect instance.</param>
		[Obsolete("Use ScriptSoundId.HasFinished instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static bool HasSoundFinished(int id)
		{
			return Function.Call<bool>(Hash.HAS_SOUND_FINISHED, id);
		}

		/// <summary>
		/// Sets an audio flag to modify subsequent sounds.
		/// </summary>
		/// <param name="flag"></param>
		/// <param name="toggle"></param>
		public static void SetAudioFlag(AudioFlags flag, bool toggle)
		{
			Function.Call(Hash.SET_AUDIO_FLAG, flag.ToString(), toggle);
		}

		#endregion
	}
}
