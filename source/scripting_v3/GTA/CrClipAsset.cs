//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Xml.Linq;

namespace GTA
{
	/// <summary>
	/// Represents a struct that contains a <see cref="GTA.CrClipDictionary"/> and a animation clip name
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
			this(new CrClipDictionary(clipDictName), animName)
		{
		}
		public CrClipAsset(CrClipDictionary clipDict, string animName)
		{
			ClipDictionary = clipDict;
			ClipName = animName;
		}

		/// <summary>
		/// Gets the <see cref="GTA.CrClipDictionary"/> struct of clip/animation dictionary name.
		/// </summary>
		public CrClipDictionary ClipDictionary
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

		/// <summary>
		/// Computes the hash of <see cref="ClipName"/> in the same way as how the game calculates hashes for clip
		/// names to store in a <c>rage::crClipDictionary</c> and as how <see cref="Game.GenerateHash(string)"/>
		/// calculates.
		/// May be useful when you want to get the identifier in the same way as how the game handles texture
		/// dictionaries or when you investigate game memory to see how clips are stored in the clip dictionary.
		/// </summary>
		/// <returns>The hash value calculated from <see cref="ClipName"/>.</returns>
		public int HashClipName() => Game.GenerateHash(ClipName);

		/// <summary>
		/// Returns <see langword="true"/> if the Jenkins-one-at-a-time (joaat) hash values of both
		/// <see cref="ClipDictionary"/> and <see cref="ClipName"/> match those of <paramref name="other"/>,
		/// as the game uses joaat hashes as identifiers of clip dictionaries and clip names.
		/// </summary>
		public bool Equals(CrClipAsset other)
			=> ClipDictionary == other.ClipDictionary && HashClipName() == other.HashClipName();
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

		public override int GetHashCode() => ClipDictionary.GetHashCode() * 17 + HashClipName();

		public void Deconstruct(out CrClipDictionary clipDict, out string clipName)
		{
			clipDict = ClipDictionary;
			clipName = ClipName;
		}
	}
}
