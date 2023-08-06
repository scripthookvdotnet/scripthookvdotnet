//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents a struct that contains a <see cref="GTA.ClipDictionary"/> and a animation name <see cref="string"/>.
	/// </summary>
	/// <remarks>
	/// You should not use the default constructor. The fallback behavior can be changed from filling in the 2 values
	/// with <see langword="null"/> after the codebase of SHVDN starts to use C# 10 or later C# version.
	/// </remarks>
	public readonly struct ClipDictAndAnimNamePair : IEquatable<ClipDictAndAnimNamePair>
	{
		public ClipDictAndAnimNamePair(string clipDictName, string animName) :
			this(new ClipDictionary(clipDictName), animName)
		{
		}
		public ClipDictAndAnimNamePair(ClipDictionary clipDict, string animName)
		{
			ClipDictionary = clipDict;
			AnimationName = animName;
		}

		/// <summary>
		/// Gets the <see cref="GTA.ClipDictionary"/> struct of clip/animation dictionary name.
		/// </summary>
		public ClipDictionary ClipDictionary
		{
			get; init;
		}
		/// <summary>
		/// Gets the texture name.
		/// </summary>
		public string AnimationName
		{
			get; init;
		}

		public bool Equals(ClipDictAndAnimNamePair other)
			=> ClipDictionary == other.ClipDictionary && AnimationName == other.AnimationName;
		public override bool Equals(object obj)
		{
			if (obj is ClipDictAndAnimNamePair clipDictAndAnimNamePair)
			{
				return Equals(clipDictAndAnimNamePair);
			}

			return false;
		}

		public static bool operator ==(ClipDictAndAnimNamePair left, ClipDictAndAnimNamePair right)
			=> left.Equals(right);
		public static bool operator !=(ClipDictAndAnimNamePair left, ClipDictAndAnimNamePair right)
			=> !left.Equals(right);

		public override int GetHashCode() => ClipDictionary.GetHashCode() * 17 + AnimationName.GetHashCode();

		public void Deconstruct(out ClipDictionary clipDict, out string animName)
		{
			clipDict = ClipDictionary;
			animName = AnimationName;
		}
	}
}
