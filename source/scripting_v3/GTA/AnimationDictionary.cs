//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a dictionary struct for an animation/clip dictionary.
	/// Note that animation/clip dictionaries are different from clip sets, which is defined in
	/// <c>clip_sets.ymt</c> or <c>clip_sets.xml</c> files.
	/// </summary>
	/// <remarks>
	/// Despite the name, GTA V doesn't have game classes for animation dictionaries different from
	/// clip dictionaries while the exe has <c>rage::crClipDictionary</c> and
	/// <c>rage::fwClipDictionaryStore</c> classes.
	/// </remarks>
	public readonly struct AnimationDictionary : IEquatable<AnimationDictionary>, IStreamingResource
	{
		public AnimationDictionary(string name) : this()
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
		/// Gets a value indicating whether this <see cref="AnimationDictionary"/> exists
		/// in the <c>fwClipDictionaryStore</c> pool.
		/// </summary>
		public bool Exists => Function.Call<bool>(Hash.DOES_ANIM_DICT_EXIST, Name);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimationDictionary"/> is loaded
		/// so the animations of this <see cref="AnimationDictionary"/> are ready to use.
		/// </summary>
		public bool IsLoaded => Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, Name);

		/// <summary>
		/// Attempts to load this <see cref="AnimationDictionary"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Hash.REQUEST_ANIM_DICT, Name);
		}
		/// <summary>
		/// Attempts to load this <see cref="AnimationDictionary"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="AnimationDictionary"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="AnimationDictionary"/> is loaded; otherwise, <see langword="false" />.</returns>
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
		/// Tells the game we have finished using this <see cref="AnimationDictionary"/> and it can be freed from memory.
		/// </summary>
		public void MarkAsNoLongerNeeded()
		{
			Function.Call(Hash.REMOVE_ANIM_DICT, Name);
		}

		public bool Equals(AnimationDictionary animationDictionary)
		{
			return Name == animationDictionary.Name;
		}
		public override bool Equals(object obj)
		{
			if (obj is AnimationDictionary model)
			{
				return Equals(model);
			}

			return false;
		}

		public static bool operator ==(AnimationDictionary left, AnimationDictionary right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(AnimationDictionary left, AnimationDictionary right)
		{
			return !left.Equals(right);
		}

		public static implicit operator InputArgument(AnimationDictionary value)
		{
			return new InputArgument(value.Name);
		}
		public static implicit operator AnimationDictionary(string value)
		{
			return new AnimationDictionary(value);
		}
		public static explicit operator string(AnimationDictionary value)
		{
			return value.Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override string ToString() => Name.ToString();
	}
}
