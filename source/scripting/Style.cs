using System;
using System.Collections.Generic;
using GTA.Native;

namespace GTA
{
	public enum PedComponents
	{
		Face,
		Head,
		Hair,
		Torso,
		Legs,
		Hands,
		Shoes,
		Special1,
		Special2,
		Special3,
		Textures,
		Torso2
	}

	public enum PedProps
	{
		Hats,
		Glasses,
		EarPieces,
		Unknown3,
		Unknown4,
		Unknown5,
		Watches,
		Wristbands,
		Unknown8,
		Unknown9,
	}
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
	}

	public interface IPedVariation
	{
		int Count { get; }
		int Index { get; set; }
		int TextureCount { get; }
		int TextureIndex { get; set; }
		bool IsVariationValid(int index, int textureIndex = 0);
		bool SetVariation(int index, int textureIndex = 0);
		bool HasVariations { get; }
		bool HasTextureVariations { get; }
		bool HasAnyVariations { get; }

	}
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

		public int Count
		{
			get { return Function.Call<int>(Hash.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS, _ped.Handle, _componentdId); }
		}

		public int Index
		{
			get { return Function.Call<int>(Hash.GET_PED_DRAWABLE_VARIATION, _ped.Handle, _componentdId); }
			set { SetVariation(value); }
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
			get { return Function.Call<int>(Hash.GET_PED_TEXTURE_VARIATION, _ped.Handle, _componentdId); }
			set { SetVariation(Index, value); }
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

		public bool HasVariations
		{
			get { return Count > 1; }
		}

		public bool HasTextureVariations
		{
			get { return Count > 0 && TextureCount > 1; }
		}

		public bool HasAnyVariations
		{
			get { return HasVariations || HasTextureVariations; }
		}

	}

	public class PedProp : IPedVariation
	{
		#region Fields
		readonly Ped _ped;
		readonly PedProps _propId;
		#endregion

		internal PedProp(Ped ped, PedProps propId)
		{
			_ped = ped;
			_propId = propId;
		}

		public int Count
		{
			get { return Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS, _ped.Handle, _propId) + 1; }//+1 to accomodate for no prop selected(value = -1);
		}

		public int Index
		{
			get { return Function.Call<int>(Hash.GET_PED_PROP_INDEX, _ped.Handle, _propId) + 1; }
			set { SetVariation(value); }
		}

		public int TextureCount
		{
			get { return Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS, _ped.Handle, _propId, Index - 1); }
		}

		public int TextureIndex
		{
			get { return Index == 0 ? 0 : Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, _ped.Handle, _propId); }
			set
			{
				if (Index > 0) SetVariation(Index, value);
			}
		}

		public bool IsVariationValid(int index, int textureIndex = 0)
		{
			if (index == 0)
			{
				return true;//no prop always valid
			}
			return Function.Call<bool>(Hash._IS_PED_PROP_VALID, _ped.Handle, _propId, index - 1, textureIndex);
		}
		public bool SetVariation(int index, int textureIndex = 0)
		{
			if (index == 0)
			{
				Function.Call(Hash.CLEAR_PED_PROP, _ped.Handle, _propId);
				return true;
			}
			if (IsVariationValid(index, textureIndex))
			{
				Function.Call(Hash.SET_PED_PROP_INDEX, _ped.Handle, _propId, index - 1, textureIndex, 1);
				return true;
			}
			return false;
		}

		public bool HasVariations
		{
			get { return Count > 1; }
		}

		public bool HasTextureVariations
		{
			get { return Count > 1 && TextureCount > 1; }
		}

		public bool HasAnyVariations
		{
			get { return HasVariations; }
		}


	}

}