//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;
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

		/// <summary>
		/// Returns the number of available drawable models for the component type.
		/// </summary>
		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, _ped.Handle, (int)Type);

		/// <summary>
		/// The drawable id/index.
		/// </summary>
		public int Index
		{
			get => Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, _ped.Handle, (int)Type);
			set => SetVariation(value);
		}

		/// <summary>
		/// The texture count property for current <see cref="Index"/>.
		/// </summary>
		/// <remarks>
		/// You need to set a drawable id via <see cref="Index"/> before you can get the correct number of textures
		/// for a drawable model.
		/// </remarks>
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

		/// <summary>
		/// The texture index for the current drawable <see cref="Index"/>.
		/// </summary>
		/// <remarks>
		/// You need to set a drawable id via <see cref="Index"/> before you can get of set the texture index properly.
		/// </remarks>
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

		/// <summary>
		/// Returns <see langword="true"/> if there are textures for current drawable id (<see cref="Index"/>).
		/// </summary>
		[Obsolete("PedComponent.HasTextureVariations is obsolete because it does not make sense " +
		          "as texture count cannot be determined without specifying both component id and drawable id."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasTextureVariations => Count > 0 && TextureCount > 1;

		[Obsolete("PedComponent.HasAnyVariation is obsolete because it does not make sense " +
		          "as texture count cannot be determined without specifying both component id and drawable id."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasAnyVariations => HasVariations || HasTextureVariations;

		public override string ToString()
		{
			return Type.ToString();
		}
	}
}
