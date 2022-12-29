//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public class PedProp : IPedVariation
	{
		#region Fields
		readonly Ped _ped;
		#endregion

		internal PedProp(Ped ped, PedPropType propId)
		{
			_ped = ped;
			AnchorPosition = (PedPropAnchorPosition)propId;
		}

		internal PedProp(Ped ped, PedPropAnchorPosition anchorPosition)
		{
			_ped = ped;
			AnchorPosition = anchorPosition;
		}

		public string Name => AnchorPosition.ToString();

		[Obsolete("PedProp.Type is obsolete, use PedProp.AnchorPosition instead.")]
		public PedPropType Type => (PedPropType)AnchorPosition;

		public PedPropAnchorPosition AnchorPosition
		{
			get;
		}

		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS, _ped.Handle, AnchorPosition) + 1;

		public int Index
		{
			get => Function.Call<int>(Hash.GET_PED_PROP_INDEX, _ped.Handle, AnchorPosition) + 1;
			set => SetVariation(value);
		}

		public int TextureCount => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS, _ped.Handle, AnchorPosition, Index - 1);

		public int TextureIndex
		{
			get
			{
				return Index == 0 ? 0 : Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, _ped.Handle, AnchorPosition);
			}
			set
			{
				if (Index > 0)
				{
					SetVariation(Index, value);
				}
			}
		}

		public bool SetVariation(int index, int textureIndex = 0)
		{
			if (index == 0)
			{
				Function.Call(Hash.CLEAR_PED_PROP, _ped.Handle, AnchorPosition);
				return true;
			}

			if (!IsVariationValid(index, textureIndex))
			{
				return false;
			}

			Function.Call(Hash.SET_PED_PROP_INDEX, _ped.Handle, AnchorPosition, index - 1, textureIndex, 1);
			return true;
		}

		public bool IsVariationValid(int index, int textureIndex = 0)
		{
			if (index == 0)
			{
				return true; // No prop is always valid
			}

			return Function.Call<bool>(Hash.SET_PED_PRELOAD_PROP_DATA, _ped.Handle, AnchorPosition, index - 1, textureIndex);
		}

		public bool HasVariations => Count > 1;

		public bool HasTextureVariations => Count > 1 && TextureCount > 1;

		public bool HasAnyVariations => HasVariations;

		public override string ToString()
		{
			return AnchorPosition.ToString();
		}
	}
}
