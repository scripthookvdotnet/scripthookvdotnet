using GTA.Native;

namespace GTA
{
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

		public PedProps PropType => _propId;

		public string Name => _propId.ToString();

		public int Count => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS, _ped.Handle, _propId) + 1;

		public int Index
		{
			get
			{
				return Function.Call<int>(Hash.GET_PED_PROP_INDEX, _ped.Handle, _propId) + 1;
			}
			set
			{
				SetVariation(value);
			}
		}

		public int TextureCount => Function.Call<int>(Hash.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS, _ped.Handle, _propId, Index - 1);

		public int TextureIndex
		{
			get
			{
				return Index == 0 ? 0 : Function.Call<int>(Hash.GET_PED_PROP_TEXTURE_INDEX, _ped.Handle, _propId);
			}
			set
			{
				if (Index > 0)
					SetVariation(Index, value);
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

		public bool HasVariations => Count > 1;

		public bool HasTextureVariations => Count > 1 && TextureCount > 1;

		public bool HasAnyVariations => HasVariations;

		public override string ToString()
		{
			return _propId.ToString();
		}
	}
}
