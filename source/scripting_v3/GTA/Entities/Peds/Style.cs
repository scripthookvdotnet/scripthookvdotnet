//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using GTA.Native;

namespace GTA
{
	public class Style
	{
		#region Fields
		Ped _ped;
		Dictionary<PedComponents, PedComponent> _pedComponents = new Dictionary<PedComponents, PedComponent>();
		Dictionary<PedProps, PedProp> _pedProps = new Dictionary<PedProps, PedProp>();
		#endregion

		internal Style(Ped ped)
		{
			_ped = ped;
		}

		public PedComponent this[PedComponents componentId]
		{
			get
			{
				PedComponent variation = null;
				if (!_pedComponents.TryGetValue(componentId, out variation))
				{
					variation = new PedComponent(_ped, componentId);
					_pedComponents.Add(componentId, variation);
				}
				return variation;
			}
		}

		public PedProp this[PedProps propId]
		{
			get
			{
				PedProp prop = null;
				if (!_pedProps.TryGetValue(propId, out prop))
				{
					prop = new PedProp(_ped, propId);
					_pedProps.Add(propId, prop);
				}
				return prop;
			}
		}

		public PedComponent[] GetAllComponents()
		{
			List<PedComponent> components = new List<PedComponent>();
			foreach (PedComponents componentId in Enum.GetValues(typeof(PedComponents)))
			{
				PedComponent component = this[componentId];
				if (component.HasAnyVariations)
				{
					components.Add(component);
				}
			}
			return components.ToArray();
		}

		public PedProp[] GetAllProps()
		{
			List<PedProp> props = new List<PedProp>();
			foreach (PedProps propId in Enum.GetValues(typeof(PedProps)))
			{
				PedProp prop = this[propId];
				if (prop.HasAnyVariations)
				{
					props.Add(prop);
				}
			}
			return props.ToArray();
		}

		public IPedVariation[] GetAllVariations()
		{
			List<IPedVariation> variations = new List<IPedVariation>();
			variations.AddRange(GetAllComponents());
			variations.AddRange(GetAllProps());
			return variations.ToArray();
		}

		public IEnumerator<IPedVariation> GetEnumerator()
		{
			return (GetAllVariations() as IEnumerable<IPedVariation>).GetEnumerator();
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

		public void RandomizeProps()
		{
			Function.Call(Hash.SET_PED_RANDOM_PROPS, _ped.Handle);
		}

		public void ClearProps()
		{
			Function.Call(Hash.CLEAR_ALL_PED_PROPS, _ped.Handle);
		}
	}
}
