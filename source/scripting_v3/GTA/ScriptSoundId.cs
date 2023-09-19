//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a script sound id, which is used for script audio sounds processed via the static <c>audScriptAudioEntity</c> instance.
	/// </summary>
	public class ScriptSoundId : IEquatable<ScriptSoundId>
	{
		internal ScriptSoundId(int id)
		{
			Id = id;
		}

		/// <summary>
		/// Gets the script sound id/index.
		/// </summary>
		public int Id
		{
			get;
		}

		/// <summary>
		/// Returns <see langword="true"/> if the sound <see cref="Id"/> is negative,
		/// which indicates this <see cref="ScriptSoundId"/> is not valid.
		/// </summary>
		public bool IsNull => Id < 0;

		/// <summary>
		/// Plays back a sound with the name <paramref name="soundName"/>.
		/// If this is used to play a sound for which no pan or speakermask is set by the sound designer, then the sound will play from the map's origin -
		/// therefore this should only be used to play frontend sounds like menu bleeps or other artificially panned effects.
		/// </summary>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="enableOnReplay"><inheritdoc cref="PlaySoundFrontend(string, string, bool)" path="/param[@name='enableOnReplay']"/></param>.
		public void PlaySound(string soundName, string setName, bool enableOnReplay = true)
		{
			Function.Call(Hash.PLAY_SOUND, Id, soundName, setName, enableOnReplay);
		}
		/// <summary>
		/// Plays back a sound "frontend" - at full volume, panned centrally.
		/// </summary>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="enableOnReplay">
		/// <para>
		/// The name is taken from the official definition, but the effect is unknown.
		/// </para>
		/// <para>
		/// Will be internally disabled if the hash calculated from <paramref name="soundName"/> (with <see cref="Game.GenerateHash(string)"/>) is one of the values
		/// <c>[0x5A23F3D5, 0xDF84A53C, 0xFD4C28, 0x832CAA0F, 0x17BD10F1, 0xFA4A5AA0, 0x2B8F97E3, 0x1D46A6A2, 0xF5E3A26A, 0xF35C567B,
		/// 0x71F56AB4, 0xC55C68A0, 0x54C522AD, 0xD382DF7C, 0x2A508F9C, 0xE8F24AFD, 0x8DDBFC96, 0x28C8633, 0x596B8EBB, 0x8A73028A,
		/// 0x578FE4D7, 0xE52306DE, 0x10109BEB]</c>.
		/// </para>
		/// </param>
		/// <remarks>
		/// If the sound has a Pan or a SpeakerMask set by the sound designer then the it will play using these settings,
		/// otherwise it will play from dead ahead (0°).
		/// </remarks>
		public void PlaySoundFrontend(string soundName, string setName, bool enableOnReplay = true)
		{
			Function.Call(Hash.PLAY_SOUND_FRONTEND, Id, soundName, setName, enableOnReplay);
		}
		/// <summary>
		/// Plays back a sound from an <see cref="Entity"/>'s location.
		/// The sound's position will track the <see cref="Entity"/>'s position as it moves.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to play the sound from.</param>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		public void PlaySoundFromEntity(Entity entity, string soundName, string setName = null)
			=> Function.Call(Hash.PLAY_SOUND_FROM_ENTITY, Id, soundName, entity.Handle, setName, false, 0);
		/// <summary>
		/// Plays back a sound from an absolute position.
		/// </summary>
		/// <param name="position">The world coordinates to play the sound from.</param>
		/// <param name="soundName">The sound name to play.</param>
		/// <param name="setName">The optional sound set name that contains the sound.</param>
		/// <param name="isExteriorLoc">
		/// If <see langword="true"/>, the sound will use a portal occlusion environmentGroup.
		/// Only use this if the sound is playing outside and needs occlusion.
		/// </param>
		public void PlaySoundFromPosition(Vector3 position, string soundName, string setName = null, bool isExteriorLoc = false)
			=> Function.Call(Hash.PLAY_SOUND_FROM_COORD, Id, soundName, position.X, position.Y, position.Z, setName, false, 0, isExteriorLoc);

		/// <summary>
		/// <para>
		/// Sets a variable on a sound.
		/// </para>
		/// <para>
		/// This method allows to communicate with the sound engine in complex ways,
		///	by passing a floating point value to a specific sound object. This allows some nice effects such as adjusting the pitch of a sample being to be played back,
		///	or varying a lowpass cutoff. The VariableName parameter must be set up in RAVE (the audio scripting tool) as well as instruction on its usage on a case-by-case
		///	basis therefore a sound designer must be consulted with before using this command.
		///	</para>
		/// </summary>
		public void UpdatePosition(string variableName, float variableValue)
			=> Function.Call(Hash.SET_VARIABLE_ON_SOUND, Id, variableName, variableValue);
		/// <summary>
		/// Updates a playing sounds absolute position.
		/// Currently not available in v1.0.617.1 or earlier game versions.
		/// </summary>
		/// <param name="position">The new position.</param>
		public void UpdatePosition(Vector3 position)
		{
			if (Game.Version < GameVersion.v1_0_678_1_Steam)
			{
				GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_678_1_Steam), nameof(ScriptSoundId), nameof(UpdatePosition));
			}

			Function.Call(Hash.UPDATE_SOUND_COORD, Id, position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Gets a boolean indicating whether the specified sound instance has completed playing.
		/// </summary>
		public bool HasFinished()
		{
			return Function.Call<bool>(Hash.HAS_SOUND_FINISHED, Id);
		}

		/// <summary>
		/// Stops a playing sound.
		/// Calling this method on this <see cref="ScriptSoundId"/> that has finished playing will have no ill effects in any case
		/// as long as the <see cref="ScriptSoundId"/> has not been released.
		/// </summary>
		public void Stop()
		{
			Function.Call(Hash.STOP_SOUND, Id);
		}
		/// <summary>
		/// Releases this <see cref="ScriptSoundId"/>.
		/// This should be called once a sound has finished being manipulated by the script so that its <see cref="ScriptSoundId"/>
		/// can be released and re-used.
		/// </summary>
		public void Release()
		{
			Function.Call(Hash.RELEASE_SOUND_ID, Id);
		}

		public bool Equals(ScriptSoundId other)
		{
			return Id == other.Id;
		}
		public override bool Equals(object obj)
		{
			if (obj is ScriptSoundId model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(ScriptSoundId left, ScriptSoundId right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(ScriptSoundId left, ScriptSoundId right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(ScriptSoundId value)
		{
			return new InputArgument((ulong)value.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
