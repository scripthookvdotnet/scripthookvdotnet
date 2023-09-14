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
	public readonly struct ClipDictionary : IEquatable<ClipDictionary>, IScriptStreamingResource
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
		/// Computes the hash of <see cref="Name"/> in the same way as how the game calculates hashes for clip
		/// dictionaries to store in the global <c>rage::fwClipDictionaryStore</c> and as how
		/// <see cref="Game.GenerateHash(string)"/> calculates.
		/// May be useful when you want to get the identifier in the same way as how the game handles texture
		/// dictionaries or when you investigate game memory to see how clips are stored in clip dictionaries.
		/// </summary>
		/// <returns>The hash value calculated from <see cref="Name"/>.</returns>
		public int HashName() => Game.GenerateHash(Name);

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

		/// <summary>
		/// Returns <see langword="true"/> if the Jenkins-one-at-a-time (joaat) hash value of <see cref="Name"/>
		/// matches that of <paramref name="other"/>, as the game uses joaat hashes as identifiers of clip dictionaries.
		/// </summary>
		public bool Equals(ClipDictionary other)
		{
			return HashName() == other.HashName();
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
			return HashName();
		}

		public override string ToString() => Name;
	}
}
