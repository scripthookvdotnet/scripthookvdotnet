//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	public class PedComponent : IPedVariation
	{
		#region Fields
		readonly Ped _ped;
		readonly PedComponents _componentdId;
		#endregion

		internal PedComponent(Ped ped, PedComponents componentId)
		{
			_ped = ped;
			_componentdId = componentId;
		}

		public PedComponents ComponentType => _componentdId;

		public string Name => _componentdId.ToString();

		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, _ped.Handle, _componentdId);

		public int Index
		{
			get
			{
				return Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, _ped.Handle, _componentdId);
			}
			set
			{
				SetVariation(value);
			}
		}

		public int TextureCount
		{
			get
			{
				int count = Function.Call<int>(Hash.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS, _ped.Handle, _componentdId, Index) + 1;
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
			get
			{
				return Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, _ped.Handle, _componentdId);
			}
			set
			{
				SetVariation(Index, value);
			}
		}

		public bool IsVariationValid(int index, int textureIndex = 0)
		{
			return Function.Call<bool>(Hash.IS_PED_COMPONENT_VARIATION_VALID, _ped.Handle, _componentdId, index, textureIndex);
		}

		public bool SetVariation(int index, int textureIndex = 0)
		{
			if (IsVariationValid(index, textureIndex))
			{
				Function.Call(Hash.SET_PED_COMPONENT_VARIATION, _ped.Handle, _componentdId, index, textureIndex, 0);
				return true;
			}
			return false;
		}

		public bool HasVariations => Count > 1;

		public bool HasTextureVariations => Count > 0 && TextureCount > 1;

		public bool HasAnyVariations => HasVariations || HasTextureVariations;

		public override string ToString()
		{
			return _componentdId.ToString();
		}
	}
}
