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
    public static partial class Audio
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
        /// Gets a <see cref="ScriptSound"/> instance of a triggered sound.
        /// This method returns a new <see cref="ScriptSound"/>, which is used for keeping track of sounds after they've been triggered -
        /// use this if you need to control a sound after it's been started, for instance to stop a looping sound, or to change a sound's pitch midway through playback.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <para>
        /// SoundIds can be reused, without needing to release them and grab a new one. If a sound's finished playing,
        /// you can reuse its <see cref="ScriptSound"/> to kick off another one. If the sound's not finished playing,
        /// it'll be stopped first (fading out or whatever is set up in RAVE by the sound designer), and the new one kicked off;
        /// usually it is safer to just get a new <see cref="ScriptSound"/>.
        /// </para>
        /// <para>
        /// Identifiers of <see cref="ScriptSound"/> are always integral values greater than or equal to zero;
        /// if a playback function has a SoundId field but the sound doesn't need to be altered after triggering
        /// then call the forget method variants in <see cref="Audio"/> class, rather than getting a <see cref="ScriptSound"/>.
        /// </para>
        /// <para>
        /// Scripted sound can be reserved up to 100 in the <c>audScriptAudioEntity</c> instance.
        /// The limit is shared among all scripts.
        /// </para>
        /// </remarks>
        /// <returns>
        /// A <see cref="ScriptSound"/> instance with the assigned id in the range of from 0 to 100 if the method successfully found a free id/index;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public static ScriptSound GetSoundId()
        {
            int id = Function.Call<int>(Hash.GET_SOUND_ID);
            return id >= 0 ? new ScriptSound(id) : null;
        }

        /// <summary>
        /// Plays back a sound with the name <paramref name="soundName"/>, but do not track of sounds.
        /// If this is used to play a sound for which no pan or speakermask is set by the sound designer, then the sound will play from the map's origin -
        /// therefore this should only be used to play frontend sounds like menu bleeps or other artificially panned effects.
        /// </summary>
        /// <param name="soundName">The sound name to play.</param>
        /// <param name="setName">The optional sound set name that contains the sound.</param>
        /// <param name="enableOnReplay"><inheritdoc cref="ScriptSound.PlaySoundFrontend(string, string, bool)" path="/param[@name='enableOnReplay']"/></param>.
        public static void PlaySoundAndForget(string soundName, string setName, bool enableOnReplay = true)
        {
            Function.Call(Hash.PLAY_SOUND, -1, soundName, setName, /* bOverNetwork */ false, /* NetworkRange */ 0, enableOnReplay);
        }
        /// <summary>
        /// Plays back a sound "frontend" - at full volume, panned centrally, but do not track of sounds.
        /// </summary>
        /// <param name="soundName">The sound name to play.</param>
        /// <param name="setName">The optional sound set name that contains the sound.</param>
        /// <param name="enableOnReplay"><inheritdoc cref="ScriptSound.PlaySoundFrontend(string, string, bool)" path="/param[@name='enableOnReplay']"/></param>.
        /// <remarks>
        /// If the sound has a Pan or a SpeakerMask set by the sound designer then it will play using these settings,
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
