using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public static partial class Audio
    {
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
    }
}
