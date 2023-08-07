//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GTA
{
	public sealed class Style
	{
		#region Fields

		readonly Ped _ped;
		Dictionary<PedPropAnchorPoint, PedProp> _pedProps = new();
		Dictionary<PedComponentType, PedComponent> _pedComponents = new();

		#endregion

		/// <summary>
		/// The max component count.
		/// At least GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS will return zero if the 2nd argument is 0xC or higher as uint32_t.
		/// </summary>
		private const int NumPedComponent = 12;

		/// <summary>
		/// The practical max ped prop count.
		/// </summary>
		private const int NumPedProps = 9;

		internal Style(Ped ped)
		{
			_ped = ped;
		}

		public PedProp this[PedPropAnchorPoint anchorPoint]
		{
			get
			{
				if (_pedProps.TryGetValue(anchorPoint, out PedProp prop))
				{
					return prop;
				}

				prop = new PedProp(_ped, anchorPoint);
				_pedProps.Add(anchorPoint, prop);
				return prop;
			}
		}

		public PedComponent this[PedComponentType componentId]
		{
			get
			{
				if (_pedComponents.TryGetValue(componentId, out PedComponent variation))
				{
					return variation;
				}

				variation = new PedComponent(_ped, componentId);
				_pedComponents.Add(componentId, variation);
				return variation;
			}
		}

		[Obsolete("Use the indexer overload with the type PedPropAnchorPoint instead."),
		 EditorBrowsable(EditorBrowsableState.Never)]
		public PedProp this[PedPropType propId] => this[(PedPropAnchorPoint)propId];

		public PedProp[] GetAllProps()
		{
			var props = new List<PedProp>();
			for (int i = 0; i < NumPedProps; i++)
			{
				PedPropAnchorPoint anchorPoint = (PedPropAnchorPoint)i;
				PedProp pedProp = this[anchorPoint];
				if (pedProp.HasVariations)
				{
					props.Add(pedProp);
				}
			}

			return props.ToArray();
		}

		public PedComponent[] GetAllComponents()
		{
			var components = new List<PedComponent>();
			for (int i = 0; i < NumPedComponent; i++)
			{
				PedComponentType componentType = (PedComponentType)i;
				PedComponent component = this[componentType];
				if (component.HasVariations)
				{
					components.Add(component);
				}
			}

			return components.ToArray();
		}

		public IPedVariation[] GetAllVariations()
		{
			var variations = new List<IPedVariation>();
			variations.AddRange(GetAllComponents());
			variations.AddRange(GetAllProps());
			return variations.ToArray();
		}

		/// <summary>
		/// Sets a component variation to preload into memory, without applying it on this <see cref="Ped"/>.
		/// </summary>
		/// <param name="componentType">The ped component type/id.</param>
		/// <param name="drawableId">The drawable id.</param>
		/// <param name="textureId">The texture id.</param>
		public void PreloadVariationData(PedComponentType componentType, int drawableId, int textureId)
			=> Function.Call(Hash.SET_PED_PRELOAD_VARIATION_DATA, _ped, (int)componentType, drawableId, textureId);

		/// <summary>
		/// Returns true if the preload data set with <see cref="PreloadVariationData"/> is in memory.
		/// </summary>
		public bool HasLoadedPreloadVariationData()
			=> Function.Call<bool>(Hash.HAS_PED_PRELOAD_VARIATION_DATA_FINISHED, _ped);

		/// <summary>
		/// Releases the assets set with <see cref="PreloadVariationData"/>.
		/// </summary>
		/// <remarks>
		/// Note that variation data set with <see cref="PreloadVariationData"/> counts towards
		/// the script memory budget of the SHVDN runtime script.
		/// For this reason, it is important to use this method to release these assets as soon as you don't need them
		/// anymore.
		/// In fact, you can call this method as soon as you have set the same variation with
		/// <see cref="PedComponent.SetVariation(int, int)"/> since at that point the assets will be rendered on the ped
		/// and have references to keep them in memory.
		/// </remarks>
		public void ReleasePreloadVariationData()
			=> Function.Call(Hash.RELEASE_PED_PRELOAD_VARIATION_DATA, _ped);

		/// <summary>
		/// Sets a prop to preload into memory, without applying it on this <see cref="Ped"/>.
		/// </summary>
		/// <param name="anchor">The ped component type/id.</param>
		/// <param name="propId">The prop id.</param>
		/// <param name="textureId">The texture id.</param>
		public void PreloadPropData(PedPropAnchorPoint anchor, int propId, int textureId)
			=> Function.Call(Hash.SET_PED_PRELOAD_PROP_DATA, _ped, (int)anchor, propId, textureId);

		/// <summary>
		/// Returns true if the preload prop set with <see cref="PreloadPropData"/> is in memory.
		/// </summary>
		public bool HasLoadedPreloadPropData()
			=> Function.Call<bool>(Hash.HAS_PED_PRELOAD_PROP_DATA_FINISHED, _ped);

		/// <summary>
		/// Releases the assets set with <see cref="PreloadPropData"/>.
		/// </summary>
		/// <remarks>
		/// Note that variation data set with <see cref="PreloadPropData"/> counts towards
		/// the script memory budget of the SHVDN runtime script.
		/// For this reason, it is important to use this method to release these assets as soon as you don't need them
		/// anymore.
		/// In fact, you can call this method as soon as you have set the same variation with
		/// <see cref="PedProp.SetVariation(int, int)"/> since at that point the assets will be rendered on the ped
		/// and have references to keep them in memory.
		/// </remarks>
		public void ReleasePreloadPropData()
			=> Function.Call(Hash.RELEASE_PED_PRELOAD_PROP_DATA, _ped);

		public IEnumerator<IPedVariation> GetEnumerator()
		{
			return (GetAllVariations() as IEnumerable<IPedVariation>).GetEnumerator();
		}

		public void ClearProps()
		{
			Function.Call(Hash.CLEAR_ALL_PED_PROPS, _ped.Handle);
		}

		public void RandomizeProps()
		{
			Function.Call(Hash.SET_PED_RANDOM_PROPS, _ped.Handle);
		}

		public void RandomizeOutfit()
		{
			switch ((PedHash)_ped.Model.Hash)
			{
				case PedHash.Michael:
				case PedHash.Franklin:
				case PedHash.Trevor:
				case PedHash.FreemodeMale01:
				case PedHash.FreemodeFemale01:
					return;//these models freeze when randomized
			}
			Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, _ped.Handle, false);
		}

		public void SetDefaultClothes()
		{
			Function.Call(Hash.SET_PED_DEFAULT_COMPONENT_VARIATION, _ped.Handle);
		}
	}
}
