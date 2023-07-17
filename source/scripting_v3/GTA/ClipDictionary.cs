//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a dictionary struct for an clip/animation dictionary, which is created from a ycd file.
	/// Note that clip/animation dictionaries are different from clip sets, which is defined in
	/// <c>clip_sets.ymt</c> or <c>clip_sets.xml</c> files.
	/// </summary>
	/// <remarks>
	/// Alghough some natives have the string "ANIM_DICT" but there's no ones that have "CLIP_DICT" on the other hand,
	/// GTA V doesn't have game classes for animation dictionaries different from clip dictionaries
	/// while the exe has <c>rage::crClipDictionary</c> and <c>rage::fwClipDictionaryStore</c> classes.
	/// </remarks>
	public readonly struct ClipDictionary : IEquatable<ClipDictionary>, IStreamingResource
	{
		public ClipDictionary(string name) : this()
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
		/// Gets a value indicating whether this <see cref="ClipDictionary"/> exists
		/// in the <c>fwClipDictionaryStore</c> pool.
		/// </summary>
		public bool Exists => Function.Call<bool>(Hash.DOES_ANIM_DICT_EXIST, Name);

		/// <summary>
		/// Gets a value indicating whether this <see cref="ClipDictionary"/> is loaded
		/// so the animations of this <see cref="ClipDictionary"/> are ready to use.
		/// </summary>
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, Name);

		/// <summary>
		/// Attempts to load this <see cref="ClipDictionary"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, Name);
		}
		/// <summary>
		/// Attempts to load this <see cref="ClipDictionary"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="ClipDictionary"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="ClipDictionary"/> is loaded; otherwise, <see langword="false" />.</returns>
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
		/// Tells the game we have finished using this <see cref="ClipDictionary"/> and it can be freed from memory.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash.REMOVE_ANIM_DICT, Name);
		}

		public bool Equals(ClipDictionary other)
		{
			return Name == other.Name;
		}
		public override bool Equals(object obj)
		{
			if (obj is ClipDictionary model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(ClipDictionary left, ClipDictionary right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(ClipDictionary left, ClipDictionary right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(ClipDictionary value)
		{
			return new InputArgument(value.Name);
		}
		public static explicit operator ClipDictionary(string value)
		{
			return new ClipDictionary(value);
		}
		public static explicit operator string(ClipDictionary value)
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
