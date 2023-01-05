//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a animation dictionary struct.
	/// </summary>
	public struct AnimationDictionary : IEquatable<AnimationDictionary>, IStreamingResource
	{
		public AnimationDictionary(string name) : this()
		{
			Name = name;
		}

		/// <summary>
		/// Gets the animation dictionary name.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimationDictionary"/> is valid.
		/// </summary>
		public bool IsValid => Function.Call<bool>(Native.Hash.DOES_ANIM_DICT_EXIST, Name);

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnimationDictionary"/> is loaded so the animations of this <see cref="AnimationDictionary"/> are ready to use.
		/// </summary>
		public bool IsLoaded => Function.Call<bool>(Native.Hash.HAS_ANIM_DICT_LOADED, Name);

		/// <summary>
		/// Attempts to load this <see cref="AnimationDictionary"/> into memory.
		/// </summary>
		public void Request()
		{
			Function.Call(Native.Hash.REQUEST_ANIM_DICT, Name);
		}
		/// <summary>
		/// Attempts to load this <see cref="AnimationDictionary"/> into memory for a given period of time.
		/// </summary>
		/// <param name="timeout">The time (in milliseconds) before giving up trying to load this <see cref="AnimationDictionary"/>.</param>
		/// <returns><see langword="true" /> if this <see cref="AnimationDictionary"/> is loaded; otherwise, <see langword="false" />.</returns>
		public bool Request(int timeout)
		{
			Request();

			DateTime endtime = timeout >= 0 ? DateTime.UtcNow + new TimeSpan(0, 0, 0, 0, timeout) : DateTime.MaxValue;

			while (!IsLoaded)
			{
				Script.Yield();
				Request();

				if (DateTime.UtcNow >= endtime)
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
			Function.Call(Native.Hash.REMOVE_ANIM_DICT, Name);
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
