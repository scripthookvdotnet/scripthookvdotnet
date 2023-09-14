//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	/// <summary>
	/// Represents a struct that contains a <see cref="GTA.ClipDictionary"/> and a animation clip name
	/// <see cref="string"/> so you can find a animation clip (an instance of a subclass of <c>rage::crClip</c>, which
	/// is an abstract one).
	/// </summary>
	/// <remarks>
	/// <para>
	/// This struct does not specify an target subclass out of all 3 subclasses <c>rage::crClip</c>, which are,
	/// <c>rage::crClipAnimation</c>, <c>rage::crClipAnimations</c>, and <c>rage::crClipAnimationExpression</c>
	/// (the one that is presumed to not be used), as the internal function
	/// <c>fwAnimManager::GetClipIfExistsByDictIndex</c>, which gets a <c>rage::crClip</c> by an index for a clip
	/// dictionary and an animation hash, does not distinguish animation types, and thus how animation natives accept
	/// any of <c>rage::crClip</c> subclasses.
	/// </para>
	/// <para>
	/// You should not use the default constructor. The fallback behavior can be changed from filling in the 2 values
	/// with <see langword="null"/> after the codebase of SHVDN starts to use C# 10 or later C# version.
	/// </para>
	/// </remarks>
	public readonly struct CrClipAsset : IEquatable<CrClipAsset>
	{
		public CrClipAsset(string clipDictName, string animName) :
			this(new ClipDictionary(clipDictName), animName)
		{
		}
		public CrClipAsset(ClipDictionary clipDict, string animName)
		{
			ClipDictionary = clipDict;
			ClipName = animName;
		}

		/// <summary>
		/// Gets the <see cref="GTA.ClipDictionary"/> struct of clip/animation dictionary name.
		/// </summary>
		public ClipDictionary ClipDictionary
		{
			get; init;
		}
		/// <summary>
		/// Gets the clip name. Do not confuse with animation names, where a clip can contain multiple animations if
		/// the clip class is <c>rage::crClipAnimations</c>.
		/// </summary>
		public string ClipName
		{
			get; init;
		}

		public bool Equals(CrClipAsset other)
			=> ClipDictionary == other.ClipDictionary && ClipName == other.ClipName;
		public override bool Equals(object obj)
		{
			if (obj is CrClipAsset crClipAsset)
			{
				return Equals(crClipAsset);
			}

			return false;
		}

		public static bool operator ==(CrClipAsset left, CrClipAsset right)
			=> left.Equals(right);
		public static bool operator !=(CrClipAsset left, CrClipAsset right)
			=> !left.Equals(right);

		public override int GetHashCode() => ClipDictionary.GetHashCode() * 17 + ClipName.GetHashCode();

		public void Deconstruct(out ClipDictionary clipDict, out string clipName)
		{
			clipDict = ClipDictionary;
			clipName = ClipName;
		}
	}
}
