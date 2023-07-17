//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a dictionary struct for a clip/animation set, which should represent a key name for <c>fwClipSet</c>.
	/// Clip/Animation sets are defined in <c>clip_sets.ymt</c> (compiled file of <c>clip_sets.pso.meta</c> according to
	/// the official scripting headers) or <c>clip_sets.xml</c> files.
	/// Note that clip/animation sets are different from clip/animation dictionaries, which is created from <c>ycd</c> files.
	/// </summary>
	public readonly struct ClipSet : IEquatable<ClipSet>, IStreamingResource
	{
		public ClipSet(string name) : this()
		{
			Name = name;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ClipSet"/> is loaded
		/// so the animations of this <see cref="ClipSet"/> are ready to use.
		/// </summary>
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, Name);

		/// <summary>
		/// Attempts to load this <see cref="ClipSet"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Hash.REQUEST_CLIP_SET, Name);
		}
		/// <summary>
		/// Attempts to load this <see cref="ClipSet"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="ClipSet"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="ClipSet"/> is loaded; otherwise, <see langword="false" />.</returns>
		public bool Request(int timeout)
		{
			Request();

			int startTime = Environment.TickCount;
			int maxElapsedTime = timeout >= 0 ? timeout : int.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				Request();

				if (Environment.TickCount - startTime >= maxElapsedTime)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Tells the game we have finished using this <see cref="ClipSet"/> and it can be freed from memory.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash.REMOVE_CLIP_SET, Name);
		}

		public bool Equals(ClipSet other)
		{
			return Name == other.Name;
		}
		public override bool Equals(object obj)
		{
			if (obj is ClipSet model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(ClipSet left, ClipSet right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(ClipSet left, ClipSet right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(ClipSet value)
		{
			return new InputArgument(value.Name);
		}
		public static explicit operator ClipSet(string value)
		{
			return new ClipSet(value);
		}
		public static explicit operator string(ClipSet value)
		{
			return value.Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override string ToString() => Name;
	}
}
