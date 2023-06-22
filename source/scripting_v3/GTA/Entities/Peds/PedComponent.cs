//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public sealed class PedComponent : IPedVariation
	{
		#region Fields
		readonly Ped _ped;
		#endregion

		internal PedComponent(Ped ped, PedComponentType componentId)
		{
			_ped = ped;
			Type = componentId;
		}

		public string Name => Type.ToString();

		public PedComponentType Type
		{
			get;
		}

		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, _ped.Handle, (int)Type);

		public int Index
		{
			get => Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, _ped.Handle, (int)Type);
			set => SetVariation(value);
		}

		public int TextureCount
		{
			get
			{
				int count = Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, _ped.Handle, (int)Type, Index) + 1;
				while (count > 0)
				{
					if (IsVariationValid(Index, count - 1))
					{
						break;
					}
					count--;
				}
				return count;
			}
		}

		public int TextureIndex
		{
			get => Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, _ped.Handle, (int)Type);
			set => SetVariation(Index, value);
		}

		public bool SetVariation(int index, int textureIndex = 0)
		{
			if (!IsVariationValid(index, textureIndex))
			{
				return false;
			}

			Function.Call(Hash.SET_PED_COMPONENT_VARIATION, _ped.Handle, (int)Type, index, textureIndex, 0);
			return true;
		}

		public bool IsVariationValid(int index, int textureIndex = 0)
		{
			return Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, _ped.Handle, (int)Type, index, textureIndex);
		}

		public bool HasVariations => Count > 1;

		public bool HasTextureVariations => Count > 0 && TextureCount > 1;

		public bool HasAnyVariations => HasVariations || HasTextureVariations;

		public override string ToString()
		{
			return Type.ToString();
		}
	}
}
