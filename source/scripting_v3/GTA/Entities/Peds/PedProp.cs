//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.ComponentModel;

namespace GTA
{
	public sealed class PedProp : IPedVariation
	{
		#region Fields
		readonly Ped _ped;
		#endregion

		internal PedProp(Ped ped, PedPropType propId)
		{
			_ped = ped;
			AnchorPoint = (PedPropAnchorPoint)propId;
		}

		internal PedProp(Ped ped, PedPropAnchorPoint anchorPoint)
		{
			_ped = ped;
			AnchorPoint = anchorPoint;
		}

		public string Name => AnchorPoint.ToString();

		[Obsolete("PedProp.Type is obsolete, use PedProp.AnchorPoint instead."),
		 EditorBrowsable(EditorBrowsableState.Never)]
		public PedPropType Type => (PedPropType)AnchorPoint;

		public PedPropAnchorPoint AnchorPoint
		{
			get;
		}

		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS, _ped.Handle, (int)AnchorPoint) + 1;

		public int Index
		{
			get => Function.Call<int>(Hash.GET_PED_PROP_INDEX, _ped.Handle, (int)AnchorPoint) + 1;
			set => SetVariation(value);
		}

		public int TextureCount => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS, _ped.Handle, (int)AnchorPoint, Index - 1);

		public int TextureIndex
		{
			get => Index == 0 ? 0 : Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, _ped.Handle, (int)AnchorPoint);
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
				Function.Call(Hash.CLEAR_PED_PROP, _ped.Handle, (int)AnchorPoint);
				return true;
			}

			if (!IsVariationValid(index, textureIndex))
			{
				return false;
			}

			Function.Call(Hash.SET_PED_PROP_INDEX, _ped.Handle, (int)AnchorPoint, index - 1, textureIndex, 1);
			return true;
		}

		public bool IsVariationValid(int index, int textureIndex = 0)
		{
			if (index == 0)
			{
				return true; // No prop is always valid
			}

			return Function.Call<bool>(Hash.SET_PED_PRELOAD_PROP_DATA, _ped.Handle, (int)AnchorPoint, index - 1, textureIndex);
		}

		public bool HasVariations => Count > 1;

		[Obsolete("PedProp.HasTextureVariations is obsolete because it does not make sense " +
				  "as texture count cannot be determined without specifying both prop position id and drawable id."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasTextureVariations => Count > 1 && TextureCount > 1;

		[Obsolete("PedProp.HasAnyVariations is obsolete because it does not make sense " +
		  "as texture count cannot be determined without specifying both prop position id and drawable id."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasAnyVariations => HasVariations;

		public override string ToString()
		{
			return AnchorPoint.ToString();
		}
	}
}
