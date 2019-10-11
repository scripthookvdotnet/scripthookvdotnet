//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public sealed class Blip : IEquatable<Blip>, IHandleable
	{
		public Blip(int handle)
		{
			Handle = handle;
		}

		public int Type
		{
			get => Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);
		}

		public int Alpha
		{
			get => Function.Call<int>(Hash.GET_BLIP_ALPHA, Handle);
			set => Function.Call(Hash.SET_BLIP_ALPHA, Handle, value);
		}

		public int Handle
		{
			get;
		}

		public BlipColor Color
		{
			get => (BlipColor)Function.Call<int>(Hash.GET_BLIP_COLOUR, Handle);
			set => Function.Call(Hash.SET_BLIP_COLOUR, Handle, (int)value);
		}

		public BlipSprite Sprite
		{
			get => (BlipSprite)Function.Call<int>(Hash.GET_BLIP_SPRITE, Handle);
			set => Function.Call(Hash.SET_BLIP_SPRITE, Handle, (int)value);
		}

		public string Name
		{
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "CELL_EMAIL_BCON");
				SHVDN.NativeFunc.PushLongString(value);
				Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, Handle);
			}
		}

		public Vector3 Position
		{
			get => Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, Handle);
			set => Function.Call(Hash.SET_BLIP_COORDS, Handle, value.X, value.Y, value.Z);
		}

		public int Rotation
		{
			set => Function.Call(Hash.SET_BLIP_ROTATION, Handle, value);
		}

		public float Scale
		{
			set => Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
		}

		public bool ShowRoute
		{
			set => Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
		}

		public bool IsFriendly
		{
			set => Function.Call(Hash.SET_BLIP_AS_FRIENDLY, Handle, value);
		}

		public bool IsFlashing
		{
			get => Function.Call<bool>(Hash.IS_BLIP_FLASHING, Handle);
			set => Function.Call(Hash.SET_BLIP_FLASHES, Handle, value);
		}

		public bool IsOnMinimap
		{
			get => Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);
		}

		public bool IsShortRange
		{
			get => Function.Call<bool>(Hash.IS_BLIP_SHORT_RANGE, Handle);
			set => Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, Handle, value);
		}

		public void HideNumber()
		{
			Function.Call(Hash.HIDE_NUMBER_ON_BLIP, Handle);
		}
		public void ShowNumber(int number)
		{
			Function.Call(Hash.SHOW_NUMBER_ON_BLIP, Handle, number);
		}

		public void Remove()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.REMOVE_BLIP, &handle);
			}
		}

		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
		}
		public static bool Exists(Blip blip)
		{
			return blip != null && blip.Exists();
		}

		public bool Equals(Blip obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return !(obj is null) && obj.GetType() == GetType() && Equals((Blip)obj);
		}

		public static bool operator ==(Blip left, Blip right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		public static bool operator !=(Blip left, Blip right)
		{
			return !(left == right);
		}

		public sealed override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
