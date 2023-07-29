//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	public sealed class Blip : PoolObject
	{
		public Blip(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Gets the memory address where the <see cref="GTA.Entity"/> is stored in memory.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetBlipAddress(Handle);

		/// <summary>
		/// Gets the type of this <see cref="Blip"/>.
		/// </summary>
		public int Type => Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);

		/// <summary>
		/// Gets or sets the display type of this <see cref="Blip"/>.
		/// </summary>
		public BlipDisplayType DisplayType
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				int offset = gameVersion >= GameVersion.v1_0_944_2_Steam ? 0x5E : gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x5C : 0x58;

				return (BlipDisplayType)SHVDN.NativeMemory.ReadByte(address + offset);
			}
			set => Function.Call<int>(Hash.SET_BLIP_DISPLAY, Handle, (int)value);
		}

		/// <summary>
		/// Gets or sets the category type of this <see cref="Blip"/>.
		/// </summary>
		public BlipCategoryType CategoryType
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				int offset = gameVersion >= GameVersion.v1_0_944_2_Steam ? 0x60 : gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x5E : 0x5A;

				return (BlipCategoryType)SHVDN.NativeMemory.ReadByte(address + offset);
			}
			set => Function.Call<int>(Hash.SET_BLIP_CATEGORY, Handle, (int)value);
		}

		/// <summary>
		/// Gets or sets the alpha of this <see cref="Blip"/> on the map.
		/// The value is up to 255.
		/// </summary>
		public int Alpha
		{
			get => Function.Call<int>(Hash.GET_BLIP_ALPHA, Handle);
			set => Function.Call(Hash.SET_BLIP_ALPHA, Handle, (int)value);
		}

		/// <summary>
		/// Gets or sets the priority of this <see cref="Blip"/>.
		/// Overlapping <see cref="Blip"/>s with a higher priority cover those with a smaller one.
		/// The value is up to 255.
		/// </summary>
		public int Priority
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				int offset = gameVersion >= GameVersion.v1_0_944_2_Steam ? 0x5D : gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x5B : 0x57;

				return SHVDN.NativeMemory.ReadByte(address + offset);
			}
			set => Function.Call(Hash.SET_BLIP_PRIORITY, Handle, value);
		}

		/// <summary>
		/// Gets or sets this <see cref="Blip"/>s label to the given number.
		/// </summary>
		/// <remarks>returns <c>-1</c> if the internal value of this property value is between <c>0x80</c> to <c>0xFF</c>.</remarks>
		public int NumberLabel
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				int offset = gameVersion >= GameVersion.v1_0_944_2_Steam ? 0x61 : gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x5F : 0x5B;

				int returnValue = (int)SHVDN.NativeMemory.ReadByte(address + offset);

				// the game does not show a number label on the blip if the value is between 0x80 to 0xFF
				if ((returnValue & 0x80) != 0)
				{
					return -1;
				}

				return returnValue;
			}
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
			set => Function.Call(Hash.SET_BLIP_COLOUR, Handle, (int)value);
		}

		/// <summary>
		/// Gets or sets the secondary color of this <see cref="Blip"/>.
		/// </summary>
		public Color SecondaryColor
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					// The same value is set when the game creates a blip
					return System.Drawing.Color.FromArgb(unchecked((int)0xFF5DB6E5));
				}

				return System.Drawing.Color.FromArgb(SHVDN.NativeMemory.ReadInt32(address + 0x4C));
			}
			set => Function.Call(Hash.SET_BLIP_SECONDARY_COLOUR, Handle, value.R, value.G, value.B);
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
		/// <remarks>
		/// Returns <see langword="null" /> if the <see cref="Blip"/> does not exist.
		/// Setting <see cref="Sprite"/> will clear this name.
		/// </remarks>
		/// <seealso cref="DisplayNameHash"/>
		public string Name
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				ushort nameLength = (ushort)SHVDN.NativeMemory.ReadInt16(address + 0x30);

				// Assume the name string pointer is accessible, since the game will crash if the name length is not 0 and does not have access to the name string pointer
				if (nameLength != 0)
				{
					return SHVDN.StringMarshal.PtrToStringUtf8(SHVDN.NativeMemory.ReadAddress(address + 0x28));
				}

				return string.Empty;
			}
			set
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_SET_BLIP_NAME, SHVDN.NativeMemory.String);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, value);
				Function.Call(Hash.END_TEXT_COMMAND_SET_BLIP_NAME, Handle);
			}
		}

		/// <summary>
		/// Get or sets this <see cref="Blip"/>s display name hash.
		/// When <see cref="Name"/> is not set, the game will show the localized <see cref="string"/> from the games language files with a specified GXT key hash.
		/// </summary>
		/// <remarks>Setting <see cref="Sprite"/> will reset this value.</remarks>
		public int DisplayNameHash
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt32(address + 0x38);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteInt32(address + 0x38, value);
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
		/// <remarks>
		/// Use <see cref="RotationFloat"/> instead if you need to get or set the value precisely,
		/// since a rotation value of a <see cref="Blip"/> are stored as a <see cref="float"/> in v1.0.944.2 or later versions.
		/// </remarks>
		/// <seealso cref="RotationFloat"/>
		public int Rotation
		{
			get
			{
				if (Game.Version >= GameVersion.v1_0_2060_1_Steam)
				{
					return Function.Call<int>(Hash.GET_BLIP_ROTATION, Handle);
				}

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				if (gameVersion >= GameVersion.v1_0_944_2_Steam)
				{
					return (int)SHVDN.NativeMemory.ReadFloat(address + 0x58);
				}
				else
				{
					int offset = gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x58 : 0x54;
					return SHVDN.NativeMemory.ReadInt16(address + offset);
				}
			}
			set => Function.Call(Hash.SET_BLIP_ROTATION, Handle, value);
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Blip"/> on the map as a <see cref="float"/>.
		/// The value does not have any decimal places in v1.0.877.1 or earlier versions because the value is stored as <see cref="ushort"/> in these versions.
		/// </summary>
		/// <value>
		/// The rotation as a <see cref="float"/>.
		/// </value>
		public float RotationFloat
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				GameVersion gameVersion = Game.Version;
				if (gameVersion >= GameVersion.v1_0_944_2_Steam)
				{
					return SHVDN.NativeMemory.ReadFloat(address + 0x58);
				}
				else
				{
					int offset = gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x58 : 0x54;
					return (float)SHVDN.NativeMemory.ReadInt16(address + offset);
				}
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				float valueNormalized = value % 360;
				if (valueNormalized < 0)
				{
					valueNormalized += 360;
				}

				GameVersion gameVersion = Game.Version;
				if (gameVersion >= GameVersion.v1_0_944_2_Steam)
				{
					SHVDN.NativeMemory.WriteFloat(address + 0x58, valueNormalized);
					return;
				}
				else
				{
					int offset = gameVersion >= GameVersion.v1_0_463_1_Steam ? 0x58 : 0x54;
					SHVDN.NativeMemory.WriteInt16(address + offset, (short)valueNormalized);
				}
			}
		}

		/// <summary>
		/// Sets the scale of this <see cref="Blip"/> on the map.
		/// </summary>
		public float Scale
		{
			set => Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the x-axis scale of this <see cref="Blip"/> on the map.
		/// The value is the same as <see cref="ScaleY"/> in v1.0.393.4 or earlier versions.
		/// </summary>
		public float ScaleX
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadFloat(address + 0x50);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + 0x50, value);
			}
		}

		/// <summary>
		/// Gets or sets the y-axis scale of this <see cref="Blip"/> on the map.
		/// The value is the same as <see cref="ScaleX"/> in v1.0.393.4 or earlier versions.
		/// </summary>
		public float ScaleY
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				int offset = Game.Version >= GameVersion.v1_0_463_1_Steam ? 0x54 : 0x50;
				return SHVDN.NativeMemory.ReadFloat(address + offset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return;
				}

				int offset = Game.Version >= GameVersion.v1_0_463_1_Steam ? 0x54 : 0x50;
				SHVDN.NativeMemory.WriteFloat(address + offset, value);
			}
		}

		/// <summary>
		/// Gets or sets the interval in ms between each blip flashing.
		/// The value is up to 65535.
		/// </summary>
		public int FlashInterval
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return SHVDN.NativeMemory.ReadInt16(address + 0x44);
			}
			set => Function.Call(Hash.SET_BLIP_FLASH_INTERVAL, Handle, value);
		}

		/// <summary>
		/// Gets or sets the flash time left in ms before this <see cref="Blip"/> stops flashing.
		/// The max value is up to 65534.
		/// Set <c>-1</c> to let the <see cref="Blip"/> flash forever.
		/// </summary>
		/// <remarks>returns <c>-1</c> if the internal value of this property value is set to <c>65535</c>, which indicates that the flash timer is explicitly not set.</remarks>
		public int FlashTimeLeft
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				int returnValue = SHVDN.NativeMemory.ReadInt16(address + 0x44);
				if (returnValue == 0xFFFF)
				{
					return -1;
				}

				return returnValue;
			}
			set => Function.Call(Hash.SET_BLIP_FLASH_TIMER, Handle, value);
		}

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Blip"/> is attached to.
		/// </summary>
		public Entity Entity => Entity.FromHandle(Function.Call<int>(Hash.GET_BLIP_INFO_ID_ENTITY_INDEX, Handle));

		/// <summary>
		/// Gets or sets a value indicating whether the route to this <see cref="Blip"/> should be shown on the map.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show the route; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowRoute
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 4);
			}
			set => Function.Call(Hash.SET_BLIP_ROUTE, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> shows the dollar sign at the top left corner of the <see cref="Blip"/>.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show the dollar sign; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowsDollarSign
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 16);
			}
			set => Function.Call(Hash.SHOW_TICK_ON_BLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> shows the heading indicator used for normal players in GTA: Online.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show the heading indicator; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowsHeadingIndicator
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 17);
			}
			set => Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> shows outline.
		/// The outline color can be changed by setting <see cref="SecondaryColor"/>.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show outline; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowsOutlineIndicator
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 18);
			}
			set => Function.Call(Hash.SHOW_HEADING_INDICATOR_ON_BLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> shows friend indicator, which highlights the <see cref="Blip"/> by a right half cyan circle.
		/// The right half cyan circle indicator is used to indicate friends in GTA: Online.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show friend indicator; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowsFriendIndicator
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 19);
			}
			set => Function.Call(Hash.SHOW_FRIEND_INDICATOR_ON_BLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Blip"/> shows crew member indicator, which highlights the <see cref="Blip"/> by a left half cyan circle.
		/// The right half cyan circle indicator is used to indicate crew members in GTA: Online.
		/// </summary>
		/// <value>
		///   <see langword="true" /> to show crew member indicator; otherwise, <see langword="false" />.
		/// </value>
		public bool ShowsCrewIndicator
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 20);
			}
			set => Function.Call(Hash.SHOW_CREW_INDICATOR_ON_BLIP, Handle, value);
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
		public bool IsOnMinimap => Function.Call<bool>(Hash.IS_BLIP_ON_MINIMAP, Handle);

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
		/// Gets or sets a value indicating whether this <see cref="Blip"/> is hidden on the map legend.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Blip"/> is hidden on the map legend; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHiddenOnLegend
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + 0x20, 14);
			}
			set => Function.Call(Hash.SET_BLIP_HIDDEN_ON_LEGEND, Handle, value);
		}

		/// <summary>
		/// Gets the appropriate name of this <see cref="Blip"/> in the same way the game does.
		/// </summary>
		/// <value>
		/// The same <see cref="string"/> as <see cref="Name"/> if the custom string is set;
		/// otherwise, the localized <see cref="string"/> from the games language files with the same GXT key hash as <see cref="DisplayNameHash"/>.
		/// </value>
		/// Returns <see langword="null" /> if the <see cref="Blip"/> does not exist.
		public string GetAppropriateName()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return null;
			}

			ushort nameLength = (ushort)SHVDN.NativeMemory.ReadInt16(address + 0x30);

			// Assume the name string pointer is accessible, since the game will crash if the name length is not 0 and does not have access to the name string pointer
			if (nameLength != 0)
			{
				return SHVDN.StringMarshal.PtrToStringUtf8(SHVDN.NativeMemory.ReadAddress(address + 0x28));
			}

			return Game.GetLocalizedString(SHVDN.NativeMemory.ReadInt32(address + 0x38));
		}

		/// <summary>
		/// Sets the name of this <see cref="Blip"/> based on its current <see cref="Sprite"/>.
		/// </summary>
		public void ResetName()
		{
			// Resetting sprite also resets color
			BlipColor color = Color;
			Sprite = Sprite;
			Color = color;
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
		/// <returns><see langword="true" /> if this <see cref="Blip"/> exists; otherwise, <see langword="false" />.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_BLIP_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same blip as this <see cref="Blip"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same blip as this <see cref="Blip"/>; otherwise, <see langword="false" />.</returns>
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

		/// <summary>
		/// Converts a <see cref="Blip"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Blip value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
