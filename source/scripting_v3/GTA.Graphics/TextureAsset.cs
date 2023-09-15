//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA.Graphics
{
	/// <summary>
	/// Represents a struct that contains a <see cref="Txd"/> and a texture name <see cref="string"/> to use a texture
	/// (<c>rage::grcTexturePC11</c>).
	/// </summary>
	/// <remarks>
	/// You should not use the default constructor. The fallback behavior can be changed from filling in the 2 values
	/// with <see langword="null"/> after the codebase of SHVDN starts to use C# 10 or later C# version.
	/// </remarks>
	public readonly struct TextureAsset : IEquatable<TextureAsset>
	{
		public TextureAsset(string txdName, string texName) : this(new Txd(txdName), texName)
		{
		}
		public TextureAsset(Txd txd, string texName)
		{
			Txd = txd;
			TextureName = texName;
		}

		/// <summary>
		/// Gets the <see cref="Txd"/> struct of texture dictionary name.
		/// </summary>
		public Txd Txd
		{
			get; init;
		}
		/// <summary>
		/// Gets the texture name.
		/// </summary>
		public string TextureName
		{
			get; init;
		}

		/// <summary>
		/// Computes the hash of <see cref="TextureName"/> in the same way as how the game calculates hashes for
		/// texture names to store in a <c>rage::fwAssetStore&lt;rage::pgDictionary&lt;rage::grcTexture&gt;,rage::fwTxdDef&gt;</c>
		/// and as how <see cref="Game.GenerateHash(string)"/> calculates.
		/// May be useful when you want to get the identifier in the same way as how the game handles texture
		/// dictionaries or when you investigate game memory to see how textures are stored in the texture dictionary.
		/// </summary>
		/// <returns>The hash value calculated from <see cref="TextureName"/>.</returns>
		public int HashTextureName() => Game.GenerateHash(TextureName);

		public bool Equals(TextureAsset other)
			=> Txd == other.Txd && HashTextureName() == other.HashTextureName();
		public override bool Equals(object obj)
		{
			if (obj is TextureAsset texAsset)
			{
				return Equals(texAsset);
			}

			return false;
		}

		public static bool operator ==(TextureAsset left, TextureAsset right)
			=> left.Equals(right);
		public static bool operator !=(TextureAsset left, TextureAsset right)
			=> !left.Equals(right);

		public override int GetHashCode() => Txd.GetHashCode() * 17 + HashTextureName();

		public void Deconstruct(out Txd txd, out string texName)
		{
			txd = Txd;
			texName = TextureName;
		}
	}
}
