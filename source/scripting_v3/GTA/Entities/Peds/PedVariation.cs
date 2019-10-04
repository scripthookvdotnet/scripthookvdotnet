//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public interface IPedVariation
	{
		int Count
		{
			get;
		}
		int Index
		{
			get; set;
		}
		int TextureCount
		{
			get;
		}
		int TextureIndex
		{
			get; set;
		}

		bool IsVariationValid(int index, int textureIndex = 0);
		bool SetVariation(int index, int textureIndex = 0);

		bool HasVariations
		{
			get;
		}
		bool HasTextureVariations
		{
			get;
		}
		bool HasAnyVariations
		{
			get;
		}
		string Name
		{
			get;
		}
	}
}
