//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public sealed class Blip : PoolObject
	{
		public Blip(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets the type of this <see cref="Blip"/>.
		/// </summary>
		public int Type
		{
			get => Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);
		}

		/// <summary>
		/// Gets or sets the alpha of this <see cref="Blip"/> on the map.
		/// </summary>
		public int Alpha
		{
			get => Function.Call<int>(Hash.GET_BLIP_ALPHA, Handle);
			set => Function.Call(Hash.SET_BLIP_ALPHA, Handle, value);
		}

		/// <summary>
		/// Sets the priority of this <see cref="Blip"/>.
		/// </summary>
		public int Priority
		{
			set => Function.Call(Hash.SET_BLIP_PRIORITY, Handle, value);
		}

		/// <summary>
		/// Sets this <see cref="Blip"/>s label to the given number.
		/// </summary>
		public int NumberLabel
		{
			set => Function.Call(Hash.SHOW_NUMBER_ON_BLIP, Handle, value);
		}

		/// <summary>
		/// Removes the number label from this <see cref="Blip"/>.
		/// </summary>
		public void RemoveNumberLabel()
		{
			Function.Call(Hash.HIDE_NUMBER_ON_BLIP, Handle);
		}

		/// <summary>
		/// Gets or sets the color of this <see cref="Blip"/>.
		/// </summary>
		public BlipColor Color
		{
			get => (BlipColor)Function.Call<int>(Hash.GET_BLIP_COLOUR, Handle);
			set => Function.Call(Hash.SET_BLIP_COLOUR, Handle, value);
		}

		/// <summary>
		/// Gets or sets the sprite of this <see cref="Blip"/>.
		/// </summary>
		public BlipSprite Sprite
		{
			get => (BlipSprite)Function.Call<int>(Hash.GET_BLIP_SPRITE, Handle);
			set => Function.Call(Hash.SET_BLIP_SPRITE, Handle, value);
		}

		/// <summary>
		/// Sets this <see cref="Blip"/>s label to the given string.
		/// </summary>
		public string Name
		{
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, SHVDN.NativeMemory.String);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
				Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, Handle);
			}
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Blip"/>.
		/// </summary>
		public Vector3 Position
		{
			get => Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, Handle);
			set => Function.Call(Hash.SET_BLIP_COORDS, Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Sets the rotation of this <see cref="Blip"/> on the map.
		/// </summary>
		public int Rotation
		{
			set => Function.Call(Hash.SET_BLIP_ROTATION, Handle, value);
		}

		/// <summary>
		/// Sets the scale of this <see cref="Blip"/> on the map.
		/// </summary>
		public float Scale
		{
			set => Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Blip"/> is attached to.
		/// </summary>
		public Entity Entity
		{
			get => Entity.FromHandle(Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, Handle));
		}

		/// <summary>
		/// Sets a value indicating whether the route to this <see cref="Blip"/> should be shown on the map.
		/// </summary>
		/// <value>
		///   <c>true</c> to show the route; otherwise, <c>false</c>.
		/// </value>
		public bool ShowRoute
		{
			set => Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Blip"/> is friendly.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is friendly; otherwise, <c>false</c>.
		/// </value>
		public bool IsFriendly
		{
			set => Function.Call(Hash.SET_BLIP_AS_FRIENDLY, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is flashing.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is flashing; otherwise, <c>false</c>.
		/// </value>
		public bool IsFlashing
		{
			get => Function.Call<bool>(Hash.IS_BLIP_FLASHING, Handle);
			set => Function.Call(Hash.SET_BLIP_FLASHES, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Blip"/> is on minimap.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is on minimap; otherwise, <c>false</c>.
		/// </value>
		public bool IsOnMinimap
		{
			get => Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is short range.
		/// </summary>
		/// <value>
		/// <c>true</c> if this <see cref="Blip"/> is short range; otherwise, <c>false</c>.
		/// </value>
		public bool IsShortRange
		{
			get => Function.Call<bool>(Hash.IS_BLIP_SHORT_RANGE, Handle);
			set => Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, Handle, value);
		}

		/// <summary>
		/// Removes this <see cref="Blip"/>.
		/// </summary>
		public override void Delete()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.REMOVE_BLIP, &handle);
			}
			Handle = handle;
		}

		/// <summary>
		/// Determines if this <see cref="Blip"/> exists.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="Blip"/> exists; otherwise, <c>false</c>.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same blip as this <see cref="Blip"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><c>true</c> if the <paramref name="obj"/> is the same blip as this <see cref="Blip"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Blip blip)
			{
				return Handle == blip.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Blip"/>s refer to the same blip.
		/// </summary>
		/// <param name="left">The left <see cref="Pickup"/>.</param>
		/// <param name="right">The right <see cref="Pickup"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> is the same blip as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Blip left, Blip right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Blip"/>s don't refer to the same blip.
		/// </summary>
		/// <param name="left">The left <see cref="Pickup"/>.</param>
		/// <param name="right">The right <see cref="Pickup"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> is not the same blip as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(Blip left, Blip right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="Blip"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Blip value)
		{
			return new InputArgument((ulong)value.Handle);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
