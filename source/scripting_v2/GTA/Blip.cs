//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
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

		/// <summary>
		/// Gets the type of this <see cref="Blip"/>.
		/// </summary>
		public int Type
		{
			get => Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);
		}

		/// <summary>
		/// Gets or sets the alpha of this <see cref="Blip"/> on the map.
		/// The value is up to 255.
		/// </summary>
		public int Alpha
		{
			get => Function.Call<int>(Hash.GET_BLIP_ALPHA, Handle);
			set => Function.Call(Hash.SET_BLIP_ALPHA, Handle, value);
		}

		public int Handle
		{
			get;
		}

		/// <summary>
		/// Gets or sets the color of this <see cref="Blip"/>.
		/// </summary>
		public BlipColor Color
		{
			get => (BlipColor)Function.Call<int>(Hash.GET_BLIP_COLOUR, Handle);
			set => Function.Call(Hash.SET_BLIP_COLOUR, Handle, (int)value);
		}

		/// <summary>
		/// Gets or sets the sprite of this <see cref="Blip"/>.
		/// </summary>
		public BlipSprite Sprite
		{
			get => (BlipSprite)Function.Call<int>(Hash.GET_BLIP_SPRITE, Handle);
			set => Function.Call(Hash.SET_BLIP_SPRITE, Handle, (int)value);
		}

		/// <summary>
		/// Get or sets the custom name of this <see cref="Blip"/>.
		/// The custom name will appear in the legends list on the map after a string is set via this property.
		/// </summary>
		public string Name
		{
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, "STRING");
				Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, value);
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
		/// Gets or sets the rotation of this <see cref="Blip"/> on the map as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The rotation as an <see cref="int"/>.
		/// </value>
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
		/// Gets or sets a value indicating whether the route to this <see cref="Blip"/> should be shown on the map.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show the route; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowRoute
		{
			set => Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Blip"/> is friendly.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Blip"/> is friendly; otherwise, <see langword="false" />.
		/// </value>
		public bool IsFriendly
		{
			set => Function.Call(Hash.SET_BLIP_AS_FRIENDLY, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is flashing.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Blip"/> is flashing; otherwise, <see langword="false" />.
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
		/// <see langword="true" /> if this <see cref="Blip"/> is on minimap; otherwise, <see langword="false" />.
		/// </value>
		public bool IsOnMinimap
		{
			get => Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is short range.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Blip"/> is short range; otherwise, <see langword="false" />.
		/// </value>
		public bool IsShortRange
		{
			get => Function.Call<bool>(Hash.IS_BLIP_SHORT_RANGE, Handle);
			set => Function.Call(Hash.SET_BLIP_AS_SHORT_RANGE, Handle, value);
		}

		/// <summary>
		/// Removes the number label from this <see cref="Blip"/>.
		/// </summary>
		public void HideNumber()
		{
			Function.Call(Hash.HIDE_NUMBER_ON_BLIP, Handle);
		}
		/// <summary>
		/// Sets this <see cref="Blip"/>s label to the given number.
		/// </summary>
		public void ShowNumber(int number)
		{
			Function.Call(Hash.SHOW_NUMBER_ON_BLIP, Handle, number);
		}

		/// <summary>
		/// Removes this <see cref="Blip"/>.
		/// </summary>
		public void Remove()
		{
			int handle = Handle;
			unsafe
			{
				Function.Call(Hash.REMOVE_BLIP, &handle);
			}
		}

		/// <summary>
		/// Determines if this <see cref="Blip"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Blip"/> exists; otherwise, <see langword="false" />.</returns>
		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
		}
		/// <summary>
		/// Determines if the specified <see cref="Blip"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if the specified <see cref="Blip"/> exists; otherwise, <see langword="false" />.</returns>

		public static bool Exists(Blip blip)
		{
			return blip != null && blip.Exists();
		}

		/// <summary>
		/// Determines if <paramref name="obj"/> refers to the same blip as this <see cref="Blip"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Blip"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same blip as this <see cref="Blip"/>; otherwise, <see langword="false" />.</returns>

		public bool Equals(Blip obj)
		{
			return obj is not null && Handle == obj.Handle;
		}
		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same blip as this <see cref="Blip"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same blip as this <see cref="Blip"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			return obj is not null && obj.GetType() == GetType() && Equals((Blip)obj);
		}

		/// <summary>
		/// Determines if two <see cref="Blip"/>s refer to the same blip.
		/// </summary>
		/// <param name="left">The left <see cref="Blip"/>.</param>
		/// <param name="right">The right <see cref="Blip"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same blip as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Blip left, Blip right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="Blip"/>s don't refer to the same blip.
		/// </summary>
		/// <param name="left">The left <see cref="Blip"/>.</param>
		/// <param name="right">The right <see cref="Blip"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same blip as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Blip left, Blip right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
