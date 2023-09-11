//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using GTA.NaturalMotion;
using System;
using System.ComponentModel;
using System.Linq;

namespace GTA
{
	public sealed class Ped : Entity
	{
		#region Fields
		TaskInvoker _tasks;
		Euphoria _euphoria;
		WeaponCollection _weapons;
		Style _style;
		PedBoneCollection _pedBones;

		internal static readonly string[] _speechModifierNames = {
			"SPEECH_PARAMS_STANDARD",
			"SPEECH_PARAMS_ALLOW_REPEAT",
			"SPEECH_PARAMS_BEAT",
			"SPEECH_PARAMS_FORCE",
			"SPEECH_PARAMS_FORCE_FRONTEND",
			"SPEECH_PARAMS_FORCE_NO_REPEAT_FRONTEND",
			"SPEECH_PARAMS_FORCE_NORMAL",
			"SPEECH_PARAMS_FORCE_NORMAL_CLEAR",
			"SPEECH_PARAMS_FORCE_NORMAL_CRITICAL",
			"SPEECH_PARAMS_FORCE_SHOUTED",
			"SPEECH_PARAMS_FORCE_SHOUTED_CLEAR",
			"SPEECH_PARAMS_FORCE_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY",
			"SPEECH_PARAMS_MEGAPHONE",
			"SPEECH_PARAMS_HELI",
			"SPEECH_PARAMS_FORCE_MEGAPHONE",
			"SPEECH_PARAMS_FORCE_HELI",
			"SPEECH_PARAMS_INTERRUPT",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED_CLEAR",
			"SPEECH_PARAMS_INTERRUPT_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_INTERRUPT_NO_FORCE",
			"SPEECH_PARAMS_INTERRUPT_FRONTEND",
			"SPEECH_PARAMS_INTERRUPT_NO_FORCE_FRONTEND",
			"SPEECH_PARAMS_ADD_BLIP",
			"SPEECH_PARAMS_ADD_BLIP_ALLOW_REPEAT",
			"SPEECH_PARAMS_ADD_BLIP_FORCE",
			"SPEECH_PARAMS_ADD_BLIP_SHOUTED",
			"SPEECH_PARAMS_ADD_BLIP_SHOUTED_FORCE",
			"SPEECH_PARAMS_ADD_BLIP_INTERRUPT",
			"SPEECH_PARAMS_ADD_BLIP_INTERRUPT_FORCE",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CLEAR",
			"SPEECH_PARAMS_FORCE_PRELOAD_ONLY_SHOUTED_CRITICAL",
			"SPEECH_PARAMS_SHOUTED",
			"SPEECH_PARAMS_SHOUTED_CLEAR",
			"SPEECH_PARAMS_SHOUTED_CRITICAL",
		};
		#endregion

		internal Ped(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Spawn an identical clone of this <see cref="Ped"/>.
		/// </summary>
		/// <param name="heading">The direction the clone should be facing.</param>
		public Ped Clone(float heading = 0.0f)
		{
			return new Ped(Function.Call<int>(Hash.CLONE_PED, Handle, heading, false, false));
		}

		/// <summary>
		/// Kills this <see cref="Ped"/> immediately.
		/// </summary>
		public void Kill()
		{
			Health = 0;
		}

		/// <summary>
		/// Resurrects this <see cref="Ped"/> from death.
		/// </summary>
		public void Resurrect()
		{
			int health = MaxHealth;
			bool isCollisionEnabled = IsCollisionEnabled;

			Function.Call(Hash.RESURRECT_PED, Handle);
			Health = MaxHealth = health;
			IsCollisionEnabled = isCollisionEnabled;
			Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, Handle);
		}

		/// <summary>
		/// Determines if this <see cref="Ped"/> exists.
		/// You should ensure <see cref="Ped"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Ped"/> exists; otherwise, <see langword="false" /></returns>
		/// <seealso cref="Entity.IsDead"/>
		/// <seealso cref="IsInjured"/>
		public new bool Exists()
		{
			return EntityType == EntityType.Ped;
		}

		private IntPtr PedIntelligenceAddress
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}

				return SHVDN.NativeMemory.ReadAddress(address + SHVDN.NativeMemory.Ped.PedIntelligenceOffset);
			}
		}

		#region Styling

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> is human.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> is human; otherwise, <see langword="false" />.
		/// </value>
		public bool IsHuman => Function.Call<bool>(Hash.IS_PED_HUMAN, Handle);

		public bool IsCuffed => Function.Call<bool>(Hash.IS_PED_CUFFED, Handle);

		/// <summary>
		/// Sets a value that indicates whether this <see cref="Ped"/> will use a helmet on their own.
		/// </summary>
		public bool CanWearHelmet
		{
			set => Function.Call(Hash.SET_PED_HELMET, Handle, value);
		}

		/// <summary>
		/// Sets a value that indicates whether this <see cref="Ped"/> is currently wearing a helmet.
		/// </summary>
		public bool IsWearingHelmet => Function.Call<bool>(Hash.IS_PED_WEARING_HELMET, Handle);

		/// <summary>
		/// Sets a value that indicates whether this <see cref="Ped"/> is currently taking off their helmet.
		/// </summary>
		public bool IsTakingOffHelmet => Function.Call<bool>(Hash.IS_PED_TAKING_OFF_HELMET, Handle);

		public void ClearBloodDamage()
		{
			Function.Call(Hash.CLEAR_PED_BLOOD_DAMAGE, Handle);
		}

		public void ClearVisibleDamage()
		{
			Function.Call(Hash.RESET_PED_VISIBLE_DAMAGE, Handle);
		}

		[Obsolete("Use Ped.GiveHelmet(bool, HelmetPropFlags, int) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public void GiveHelmet(bool canBeRemovedByPed, Helmet helmetType, int textureIndex)
		{
			Function.Call(Hash.GIVE_PED_HELMET, Handle, !canBeRemovedByPed, (int)helmetType, textureIndex);
		}

		/// <summary>
		/// Gives this <see cref="Ped"/> a helmet.
		/// </summary>
		/// <param name="dontTakeOffHelmet">If <see langword="true"/>, the <see cref="Ped"/> will not take off their helmet automatically.</param>
		/// <param name="helmetPropFlags">
		/// The helmet prop flags to test. If none of helmets for this <see cref="Ped"/> do not meet the requirements specified by the flags,
		/// The <see cref="Ped"/> will not have a helmet.
		/// </param>
		/// <param name="overwriteHelmetTextureId">
		/// If negative, a random texture will be used.
		/// If non-negative and the specified texture id is present, the texture with specified id will be used.
		/// If non-negative and the specified texture id is not present, the previous texture will be used
		/// (texture with zero id will be used if the <see cref="Ped"/> has not been given a helmet before).
		/// </param>
		/// <remarks>
		/// This method will not give the <see cref="Ped"/> a new helmet if they already has one.
		/// </remarks>
		public void GiveHelmet(bool dontTakeOffHelmet = true, HelmetPropFlags helmetPropFlags = HelmetPropFlags.DefaultHelmet, int overwriteHelmetTextureId = -1)
		{
			Function.Call(Hash.GIVE_PED_HELMET, Handle, dontTakeOffHelmet, (uint)helmetPropFlags, overwriteHelmetTextureId);
		}

		/// <summary>
		/// Removes a helmet from this <see cref="Ped"/>.
		/// </summary>
		/// <param name="instantly">If <see langword="true"/>, the helmet will be immediately removed without an animation.</param>
		public void RemoveHelmet(bool instantly)
		{
			Function.Call(Hash.REMOVE_PED_HELMET, Handle, instantly);
		}

		/// <summary>
		/// Opens a list of clothing and prop configurations that this <see cref="Ped"/> can wear.
		/// </summary>
		public Style Style => _style ?? (_style = new Style(this));

		/// <summary>
		/// Gets the gender of this <see cref="Ped"/>.
		/// </summary>
		public Gender Gender => Function.Call<bool>(Hash.IS_PED_MALE, Handle) ? Gender.Male : Gender.Female;

		/// <summary>
		/// Gets or sets the how much sweat should be rendered on this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The sweat from 0 to 100, 0 being no sweat, 100 being saturated.
		/// </value>
		public float Sweat
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SweatOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.SweatOffset);
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 100)
				{
					value = 100;
				}

				Function.Call(Hash.SET_PED_SWEAT, Handle, value);
			}
		}

		/// <summary>
		/// Sets how high up on this <see cref="Ped"/>s body water should be visible.
		/// </summary>
		/// <value>
		/// The height offset ranges from -2f to 1.99f inclusive, -2f being no water visible, 1.99f being fully covered in water.
		/// </value>
		/// <remarks>
		/// Although zero sets the height offset of the water line to zero in meters on water height members of <c>CPed</c>,
		/// This property will clear the wet/soaked effect if the value is set to the zero for the compatibility of scripts built against v3.6.0.
		/// </remarks>
		[Obsolete("Ped.WetnessHeight is obsolete because it does not indicate that it clears the wetness effect from the ped if the value is exactly zero," +
			"while the value can take any values in the range of -2f to 1.99f inclusive. Please use Ped.Wet or Ped.ClearWetnessEffect instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public float WetnessHeight
		{
			set
			{
				if (value == 0.0f)
				{
					Function.Call(Hash.CLEAR_PED_WETNESS, Handle);
				}
				else
				{
					Function.Call(Hash.SET_PED_WETNESS_HEIGHT, Handle, value);
				}
			}
		}

		/// <summary>
		/// Gets or sets the lower wetness height of this <see cref="Ped"/>.
		/// The value should be in the range of 0 and 1 inclusive.
		/// </summary>
		/// <remarks>
		/// <para>
		/// <see cref="UpperWetnessLevel"/> and <see cref="UpperWetnessHeight"/> must be set before this value can have affect.
		/// If <see cref="UpperWetnessLevel"/> is <c>-2f</c> or less or and <see cref="UpperWetnessHeight"/> is zero or less,
		/// this value will be transferred to <see cref="UpperWetnessHeight"/> and then this value will be set to <c>-2f</c>.
		/// </para>
		/// <para>
		/// The value must be less than <see cref="UpperWetnessHeight"/>, otherwise the value will be reset to <c>-2f</c> in a few frames.
		/// </para>
		/// </remarks>
		public float LowerWetnessHeight
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.LowerWetnessHeightOffset == 0)
				{
					return -2f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.LowerWetnessHeightOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.LowerWetnessHeightOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.LowerWetnessHeightOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the upper wetness height of this <see cref="Ped"/>.
		/// The value should be in the range of 0 and 1 inclusive.
		/// </summary>
		public float UpperWetnessHeight
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.UpperWetnessHeightOffset == 0)
				{
					return -2f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.UpperWetnessHeightOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.UpperWetnessHeightOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.UpperWetnessHeightOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the lower wetness level of this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The height offset of the lower water line in meters.
		/// Should be in the range of <c>-2f</c> exclusive and <c>1.99f</c> inclusive.
		/// If the value is <c>-2f</c> or less, the wetness effect will be cleared in a few frames.
		/// If the value is more than <c>1.99f</c> (not <c>2f</c>), the value will be clamped to <c>1.99f</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// If the <see cref="Ped"/> does not exist, this method will return <c>-2f</c>,
		/// which is the default value that indicates the <see cref="Ped"/> is not wet.
		/// </para>
		/// <para>
		/// <see cref="UpperWetnessLevel"/> and <see cref="UpperWetnessHeight"/> must be set before this value can have affect.
		/// If <see cref="UpperWetnessLevel"/> is <c>-2f</c> or less or and <see cref="UpperWetnessHeight"/> is zero or less,
		/// this value will be transferred to <see cref="UpperWetnessLevel"/> and then this value will be set to zero.
		/// </para>
		/// </remarks>
		public float LowerWetnessLevel
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.LowerWetnessLevelOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.LowerWetnessLevelOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.LowerWetnessLevelOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.LowerWetnessLevelOffset, value);
			}
		}
		/// <summary>
		/// Gets or sets the upper wetness level of this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The height offset of the upper water line in meters.
		/// Should be in the range of <c>-2f</c> exclusive and <c>1.99f</c> inclusive.
		/// If the value is <c>-2f</c> or less, the wetness effect will be cleared in a few frames.
		/// If the value is more than <c>1.99f</c> (not <c>2f</c>), the value will be clamped to <c>1.99f</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// If the <see cref="Ped"/> does not exist, this method will return <c>-2f</c>,
		/// which is the default value that indicates the <see cref="Ped"/> is not wet.
		/// </para>
		/// <para>
		/// This value and <see cref="UpperWetnessHeight"/> must be set before <see cref="LowerWetnessLevel"/> can have affect.
		/// </para>
		/// </remarks>
		public float UpperWetnessLevel
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset == 0)
				{
					return 0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset, value);
			}
		}

		/// <summary>
		/// Gets the value that indicates this <see cref="Ped"/> is wet at all.
		/// Strictly, this method checks if the bit is set that determines wet/soaked effect is being used on this <see cref="Ped"/>.
		/// </summary>
		public bool IsWet
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.IsUsingWetEffectOffset == 0)
				{
					return false;
				}

				return SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.IsUsingWetEffectOffset, 0);
			}
		}

		/// <remarks>
		/// <para>
		/// This method changes <see cref="UpperWetnessHeight"/> to <paramref name="height"/> and <see cref="UpperWetnessLevel"/> to <c>1f</c>,
		/// but <see cref="LowerWetnessHeight"/> will be set to <c>-2f</c> and <see cref="LowerWetnessLevel"/> will be set to zero.
		/// </para>
		/// <para>
		/// If there are some cloth controllers (<c>rage::characterClothController</c>) of this <see cref="Ped"/> for physics,
		/// the wind multiplier for physics will be set to 0.3 instead of 1.0 for the normal state.
		/// </para>
		/// </remarks>
		/// <inheritdoc cref="Wet(float, float)"/>
		public void Wet(float height) => Function.Call(Hash.SET_PED_WETNESS_HEIGHT, Handle, height);
		/// <summary>
		/// Makes this <see cref="Ped"/> wet.
		/// </summary>
		/// <param name="height">
		/// The height offset of the water line in meters. Should be in the range of <c>-2f</c> exclusive and <c>1.99f</c> inclusive.
		/// If the value is <c>-2f</c> or less, the wetness effect will be cleared in a few frames.
		/// If the value is more than <c>1.99f</c> (not <c>2f</c>), the value will be clamped to <c>1.99f</c>.
		/// </param>
		/// <param name="wetLevel">The wet level between 0 and 1.</param>
		/// <remarks>
		/// <para>
		/// This method changes <see cref="UpperWetnessHeight"/> to <paramref name="height"/> and <see cref="UpperWetnessLevel"/> to <paramref name="wetLevel"/>,
		/// but <see cref="LowerWetnessHeight"/> will be set to <c>-2f</c> and <see cref="LowerWetnessLevel"/> will be set to zero.
		/// </para>
		/// <para>
		/// If there are some cloth controllers (<c>rage::characterClothController</c>) of this <see cref="Ped"/> for physics,
		/// the wind multiplier for physics will be set to 0.3 instead of 1.0 for the normal state.
		/// </para>
		/// </remarks>
		public void Wet(float height, float wetLevel)
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return;
			}

			Wet(height);

			if (SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset != 0)
			{
				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.UpperWetnessLevelOffset, wetLevel);
			}
		}
		/// <summary>
		/// Dries this <see cref="Ped"/>. In other words, clears the wet/soaked effect from the <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// If there are some cloth controllers (<c>rage::characterClothController</c>) of this <see cref="Ped"/> for physics,
		/// the wind multiplier for physics will be set to 1.0 for the normal state.
		/// </remarks>
		public void DryOff() => Function.Call(Hash.CLEAR_PED_WETNESS, Handle);
		/// <summary>
		/// Enables a non-player <see cref="Ped"/> to get wet this frame from systems that it otherwise wouldn't (e.g. particle effects).
		/// Since the system let the player <see cref="Ped"/> wet without this method, you do not need to call this method on the player <see cref="Ped"/>.
		/// </summary>
		public void SetWetnessEnabledThisFrame() => Function.Call(Hash.SET_PED_WETNESS_ENABLED_THIS_FRAME, Handle);

		#endregion

		#region Configuration

		/// <summary>
		/// Gets or sets how much armor this <see cref="Ped"/> is wearing as an <see cref="int"/>.
		/// </summary>
		/// <remarks>if you need to get or set the value precisely, use <see cref="ArmorFloat"/> instead.</remarks>
		/// <value>
		/// The armor as an <see cref="int"/>.
		/// </value>
		public int Armor
		{
			get => Function.Call<int>(Hash.GET_PED_ARMOUR, Handle);
			set => Function.Call(Hash.SET_PED_ARMOUR, Handle, value);
		}

		/// <summary>
		/// Gets or sets how much Armor this <see cref="Ped"/> is wearing as a <see cref="float"/>.
		/// </summary>
		/// <value>
		/// The armor as a <see cref="float"/>.
		/// </value>
		public float ArmorFloat
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.ArmorOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.ArmorOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.ArmorOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.ArmorOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets how much money this <see cref="Ped"/> is carrying.
		/// The max value is 65535.
		/// </summary>
		public int Money
		{
			get => Function.Call<int>(Hash.GET_PED_MONEY, Handle);
			set => Function.Call(Hash.SET_PED_MONEY, Handle, value);
		}

		/// <summary>
		/// Gets or sets the maximum health of this <see cref="Ped"/> as an <see cref="int"/>.
		/// </summary>
		/// <value>
		/// The maximum health as an <see cref="int"/>.
		/// </value>
		/// <remarks>
		/// <para>
		/// <see cref="Player.MaxHealth"/> will be changed when the setter is called if the <see cref="Ped"/> is one for a player.
		/// </para>
		/// <para>
		/// You should not set a value larger than <c>65535</c> or a negative value for the player ped(s) as the game uses the 16-bit unsigned integer value for the max health of the player ped(s) on <c>CPlayerInfo</c>
		/// and it is used when respawning and in <c>SET_ENTITY_MAX_HEALTH</c> as the max limit.
		/// Setting a value larger than <c>65535</c> will result in the overflow of the 16-bit unsigned integer value for the max health of <c>CPlayerInfo</c>.
		/// </para>
		/// </remarks>
		public override int MaxHealth
		{
			get => Function.Call<int>(Hash.GET_PED_MAX_HEALTH, Handle);
			set => Function.Call(Hash.SET_PED_MAX_HEALTH, Handle, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is a player <see cref="Ped"/>, who has a <c>CPlayerInfo</c> pointer value.
		/// Returns <see langword="true"/> only on up to one <see cref="Ped"/>.
		/// </summary>
		public bool IsPlayer => Function.Call<bool>(Hash.IS_PED_A_PLAYER, Handle);

		/// <summary>
		/// Gets the config flag bit on this <see cref="Ped"/>.
		/// </summary>
		public bool GetConfigFlag(PedConfigFlags configFlag)
		{
			return Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, (int)configFlag, false);
		}
		/// <summary>
		/// Sets the config flag bit on this <see cref="Ped"/>.
		/// </summary>
		public void SetConfigFlag(PedConfigFlags configFlag, bool value)
		{
			Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, (int)configFlag, value);
		}

		/// <summary>
		/// Gets the reset flag bit on this <see cref="Ped"/>.
		/// You will need to call this method every frame you want to get, since the values of <see cref="PedConfigFlags"/> are reset every frame.
		/// </summary>
		public bool GetResetFlag(PedResetFlags configFlag)
		{
			return Function.Call<bool>(Hash.GET_PED_RESET_FLAG, Handle, (int)configFlag, false);
		}
		/// <summary>
		/// Sets the reset flag bit on this <see cref="Ped"/>.
		/// You will need to call this method every frame you want to set, since the values of <see cref="PedConfigFlags"/> are reset every frame.
		/// </summary>
		public void SetResetFlag(PedResetFlags configFlag, bool value)
		{
			Function.Call(Hash.SET_PED_RESET_FLAG, Handle, (int)configFlag, value);
		}

		/// <inheritdoc cref="GetConfigFlag(PedConfigFlags)"/>
		[Obsolete("Use GetConfigFlag(PedConfigFlags) instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public bool GetConfigFlag(int flagID) => GetConfigFlag((PedConfigFlags)flagID);
		/// <inheritdoc cref="GetConfigFlag(PedConfigFlags)"/>
		[Obsolete("Use SetConfigFlag(PedConfigFlags, bool) instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public void SetConfigFlag(int flagID, bool value) => SetConfigFlag((PedConfigFlags)flagID, value);

		/// <summary>
		/// Do not use this method and use <see cref="Ped.SetResetFlag(PedResetFlags, bool)"/> or <see cref="Ped.GetResetFlag(PedResetFlags)"/> instead,
		/// because <c>SET_PED_RESET_FLAG</c> uses different flag IDs from the IDs <see cref="GetConfigFlag(int)"/> and <see cref="SetConfigFlag(int, bool)"/> use.
		/// </summary>
		[Obsolete("Ped.ResetConfigFlag is obsolete since SET_PED_RESET_FLAG uses different flag IDs from the IDs GET_PED_CONFIG_FLAG and SET_PED_CONFIG_FLAG use " +
			"and the said overload always set the flag (2nd argument of SET_PED_RESET_FLAG) to true. Use Ped.SetResetFlag or Ped.GetResetFlag instead", true)]
		public void ResetConfigFlag(int flagID)
		{
			Function.Call(Hash.SET_PED_RESET_FLAG, Handle, flagID, true);
		}

		/// <summary>
		/// Sets a value indicating whether this <see cref="Entity"/> is persistent.
		/// Unlike <see cref="Entity.IsPersistent"/>, calling this method does not affect assigned tasks.
		/// </summary>
		public void SetIsPersistentNoClearTask(bool value)
		{
			if (value)
			{
				PopulationType = EntityPopulationType.Mission;
			}
			else
			{
				PopulationType = EntityPopulationType.RandomAmbient;
			}
		}

		/// <summary>
		/// Gets a collection of the <see cref="PedBone"/>s in this <see cref="Ped"/>.
		/// </summary>
		public new PedBoneCollection Bones => _pedBones ?? (_pedBones = new PedBoneCollection(this));

		#endregion

		#region Tasks

		public bool IsIdle => !IsInjured && !IsRagdoll && !IsInAir && !IsOnFire && !IsDucking && !IsGettingIntoVehicle && !IsInCombat && !IsInMeleeCombat && (!IsInVehicle() || IsSittingInVehicle());

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is basically lying on the ground.
		/// </summary>
		public bool IsProne => Function.Call<bool>(Hash.IS_PED_PRONE, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is getting up.
		/// </summary>
		public bool IsGettingUp => Function.Call<bool>(Hash.IS_PED_GETTING_UP, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently diving (includes jump launch/clamber phase).
		/// </summary>
		public bool IsDiving => Function.Call<bool>(Hash.IS_PED_DIVING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently jumping.
		/// </summary>
		public bool IsJumping => Function.Call<bool>(Hash.IS_PED_JUMPING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently falling.
		/// </summary>
		public bool IsFalling => Function.Call<bool>(Hash.IS_PED_FALLING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently climbing or vaulting or doing a drop down.
		/// </summary>
		public bool IsVaulting => Function.Call<bool>(Hash.IS_PED_VAULTING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently climbing.
		/// </summary>
		public bool IsClimbing => Function.Call<bool>(Hash.IS_PED_CLIMBING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently climbing a ladder.
		/// </summary>
		public bool IsClimbingLadder => Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, Handle, 1 /* CTaskClimbLadder */);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently walking.
		/// </summary>
		public bool IsWalking => Function.Call<bool>(Hash.IS_PED_WALKING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently running.
		/// </summary>
		public bool IsRunning => Function.Call<bool>(Hash.IS_PED_RUNNING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is currently sprinting.
		/// </summary>
		public bool IsSprinting => Function.Call<bool>(Hash.IS_PED_SPRINTING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is stood still or in a stationary vehicle.
		/// </summary>
		public bool IsStopped => Function.Call<bool>(Hash.IS_PED_STOPPED, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is swimming.
		/// </summary>
		public bool IsSwimming => Function.Call<bool>(Hash.IS_PED_SWIMMING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is swimming underwater.
		/// </summary>
		public bool IsSwimmingUnderWater => Function.Call<bool>(Hash.IS_PED_SWIMMING_UNDER_WATER, Handle);

		/// <summary>
		/// Gets or sets whether this <see cref="Ped"/> is ducking (crouching).
		/// </summary>
		/// <remarks>
		/// You need to let <see cref="Ped"/>s duck by setting <c>AllowCrouchedMovement</c> to <c>CB_TRUE</c> (and setting <c>AllowStealthMovement</c> to <c>CB_FALSE</c>) in <c>gameconfig.xml</c>
		/// or changing the values for crouching with a script such as Zolika1351's Trainer before this property can return <see langword="true"/> or setting this property to <see langword="true"/> can actually make a <see cref="Ped"/> crouch.
		/// For clarification, changing stance of the player <see cref="Ped"/> with Stance by jedijosh920 does not make this property return <see langword="true"/> as it only changes strafe clipset.
		/// </remarks>
		public bool IsDucking
		{
			get => Function.Call<bool>(Hash.IS_PED_DUCKING, Handle);
			set => Function.Call(Hash.SET_PED_DUCKING, Handle, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is looking at the target entity.
		/// </summary>
		public bool IsHeadtracking(Entity entity)
		{
			return Function.Call<bool>(Hash.IS_PED_HEADTRACKING_ENTITY, Handle, entity.Handle);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> keeps their tasks when they are marked as no longer needed by <see cref="Entity.MarkAsNoLongerNeeded"/>.
		/// </summary>
		/// <value>
		/// <para>
		/// If set to <see langword="false" />, this <see cref="Ped"/>'s task will be immediately cleared and start some ambient tasks
		/// (most likely start wandering) when they are marked as no longer needed.
		/// </para>
		/// <para>
		/// If set to <see langword="true" />, this <see cref="Ped"/> will keep their scripted task.
		/// Once this <see cref="Ped"/> has no script tasks, their task will clear and they'll start some ambient tasks (one-time-only).
		/// </para>
		/// </value>
		public bool KeepTaskWhenMarkedAsNoLongerNeeded
		{
			set => Function.Call(Hash.SET_PED_KEEP_TASK, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> keeps their tasks when they are marked as no longer needed by <see cref="Entity.MarkAsNoLongerNeeded"/>.
		/// Despite the property name, this property does not determine whether permanent events can interrupt the <see cref="Ped"/>'s tasks (e.g. seeing hated peds or getting shot at).
		/// </summary>
		/// <inheritdoc cref="KeepTaskWhenMarkedAsNoLongerNeeded"/>
		[Obsolete("Ped.AlwaysKeepTask is obsolete because it does not indicate that it only affects when the ped is marked as no longer needed. " +
		          "Use Ped.KeepTaskWhenMarkedAsNoLongerNeeded instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public bool AlwaysKeepTask
		{
			set => KeepTaskWhenMarkedAsNoLongerNeeded = value;
		}

		/// <summary>
		/// Opens a list of <see cref="TaskInvoker"/> that this <see cref="Ped"/> can carry out.
		/// </summary>
		public TaskInvoker Task => _tasks ?? (_tasks = new TaskInvoker(this));

		/// <summary>
		/// Gets the stage of the <see cref="TaskSequence"/> this <see cref="Ped"/> is currently executing.
		/// </summary>
		public int TaskSequenceProgress => Function.Call<int>(Hash.GET_SEQUENCE_PROGRESS, Handle);

		/// <summary>
		/// Gets the script task status of specified scripted task on this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The value of the current script task status if the <see cref="Ped"/> exists and has their intelligence instance,
		/// and then <paramref name="taskNameHash"/> matches the current task name hash or <see cref="ScriptTaskNameHash.Any"/>;
		/// otherwise, <see cref="ScriptTaskStatus.Finished"/>.
		/// </value>
		public ScriptTaskStatus GetScriptTaskStatus(ScriptTaskNameHash taskNameHash)
			// Although GET_SCRIPT_TASK_STATUS does additional check if the game is multiplier mode and the hash does not match, we won't encounter such case
			=> Function.Call<ScriptTaskStatus>(Hash.GET_SCRIPT_TASK_STATUS, Handle, (uint)taskNameHash);

		/// <summary>
		/// Gets the current script task name hash and status on this <see cref="Ped"/>.
		/// </summary>
		/// <param name="nameHash">
		/// When this method returns, contains the value of the current script task name hash, if the <see cref="Ped"/> exists and has their intelligence instance;
		/// otherwise, <see cref="ScriptTaskNameHash.Invalid"/> as it is internally used in the game code outside native functions.
		/// This parameter is passed uninitialized.
		/// </param>
		/// <param name="status">
		/// When this method returns, contains the value of the current script task status, if the <see cref="Ped"/> exists and has their intelligence instance;
		/// otherwise, <see cref="ScriptTaskStatus.Vacant"/> as it is internally used in the game code outside native functions.
		/// This parameter is passed uninitialized.
		/// </param>
		public void GetCurrentScriptTaskNameHashAndStatus(out ScriptTaskNameHash nameHash, out ScriptTaskStatus status)
		{
			SHVDN.NativeMemory.Ped.GetScriptTaskHashAndStatus(Handle, out uint nameHashUInt, out uint statusUInt);
			nameHash = (ScriptTaskNameHash)nameHashUInt;
			status = (ScriptTaskStatus)statusUInt;
		}

		/// <summary>
		/// Gets the current script task name hash on this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The value of the current script task name hash if the <see cref="Ped"/> exists and has their intelligence instance;
		/// otherwise, <see cref="ScriptTaskNameHash.Invalid"/> as it is internally used in the game code outside native functions.
		/// </value>
		public ScriptTaskNameHash CurrentScriptTaskNameHash
		{
			get
			{
				SHVDN.NativeMemory.Ped.GetScriptTaskHashAndStatus(Handle, out uint nameHash, out _);
				return (ScriptTaskNameHash)nameHash;
			}
		}
		/// <summary>
		/// Gets the current script task status on this <see cref="Ped"/>.
		/// </summary>
		/// <value>
		/// The value of the current script task status if the <see cref="Ped"/> exists and has their intelligence instance;
		/// otherwise, <see cref="ScriptTaskStatus.Vacant"/> as it is internally used in the game code outside native functions.
		/// </value>
		public ScriptTaskStatus CurrentScriptTaskStatus
		{
			get
			{
				SHVDN.NativeMemory.Ped.GetScriptTaskHashAndStatus(Handle, out _, out uint status);
				return (ScriptTaskStatus)status;
			}
		}

		/// <summary>
		/// Gets Returns the state of any active <see cref="TaskInvoker.FollowNavMeshTo(GTA.Math.Vector3, PedMoveBlendRatio, int, float, FollowNavMeshFlags, float, float, float, float)"/>
		/// task running on this <see cref="Ped"/>.
		/// </summary>
		public NavMeshRouteResult GetNavMeshRouteResult() => Function.Call<NavMeshRouteResult>(Hash.GET_NAVMESH_ROUTE_RESULT, Handle);

		/// <summary>
		/// Sets or unsets any <see cref="VehicleChaseBehaviorFlags"/> that the <see cref="Ped"/> will use for an active
		/// vehicle chase task (<c>CTaskVehicleChase</c>).
		/// A vehicle chase task (<c>CTaskVehicleChase</c>) on the <see cref="Ped"/> must be run, which can be created
		/// by calling <see cref="TaskInvoker.VehicleChase(Ped)"/>, before calling this method or it will have no
		/// effect.
		/// </summary>
		/// <param name="flags">The flag to set or unset.</param>
		/// <param name="value">
		/// <see langword="true"/> to set the flags; otherwise <see langword="false"/> to unset flags.
		/// </param>
		public void SetVehicleChaseBehaviorFlags(VehicleChaseBehaviorFlags flags, bool value)
		{
			Function.Call(Hash.SET_TASK_VEHICLE_CHASE_BEHAVIOR_FLAG, Handle, flags, value);
		}

		/// <summary>
		/// Sets the ideal pursuit distance when chasing a <see cref="Vehicle"/> for an active vehicle chase task
		/// (<c>CTaskVehicleChase</c>).
		/// A vehicle chase task (<c>CTaskVehicleChase</c>) on the <see cref="Ped"/> must be run, which can be created
		/// by calling <see cref="TaskInvoker.VehicleChase(Ped)"/>, before calling this method or it will have no
		/// effect.
		/// </summary>
		public void SetVehicleChaseIdealPursuitDistance(float distance)
		{
			Function.Call(Hash.SET_TASK_VEHICLE_CHASE_IDEAL_PURSUIT_DISTANCE, Handle, distance);
		}

		#endregion

		#region Events

		/// <summary>
		/// Gets or sets the decision maker of this <see cref="Ped"/>, which determines what and how this <see cref="Ped"/> should response to events.
		/// Events can cause <see cref="Ped"/>s to start certain tasks. You can see how decision makers are configured in <c>events.meta</c>.
		/// </summary>
		public DecisionMaker DecisionMaker
		{
			get
			{
				if (PedIntelligenceAddress == IntPtr.Zero || SHVDN.NativeMemory.Ped.PedIntelligenceDecisionMakerHashOffset == 0)
				{
					return default;
				}

				return new DecisionMaker(SHVDN.NativeMemory.ReadInt32(PedIntelligenceAddress + SHVDN.NativeMemory.Ped.PedIntelligenceDecisionMakerHashOffset));
			}
			set => Function.Call(Hash.SET_DECISION_MAKER, Handle, (uint)value.Hash);
		}

		/// <summary>
		/// Sets whether permanent events are blocked for this <see cref="Ped"/>.
		/// <para>
		/// If set to <see langword="true" />, this <see cref="Ped"/> will no longer react to permanent events and will only do as they're told.
		/// For example, the <see cref="Ped"/> will not flee when get shot at and they will not begin combat even if <see cref="DecisionMaker"/> specifies that seeing a hated ped should.
		/// However, the <see cref="Ped"/> will still respond to temporary events like walking around other peds or vehicles even if this property is set to <see langword="true" />.
		/// </para>
		/// </summary>
		/// <value>
		///   <see langword="true" /> if permanent events are blocked; otherwise, <see langword="false" />.
		/// </value>
		public bool BlockPermanentEvents
		{
			set => Function.Call(Hash.SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> has received the event of <paramref name="eventType"/>.
		/// <see cref="EventType.Invalid"/> can be used to test if the <see cref="Ped"/> has received any event.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if the <see cref="Ped"/> has received the event  of <paramref name="eventType"/>; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>This is similar to <see cref="IsRespondingToEvent(EventType)"/>, but will work with blocking of non-temporary events with <see cref="BlockPermanentEvents"/>.</remarks>

		public bool HasReceivedEvent(EventType eventType)
		{
			if ((int)Game.Version < (int)GameVersion.v1_0_1868_0_Steam)
			{
				return Function.Call<bool>(Hash.HAS_PED_RECEIVED_EVENT, Handle, GetEventTypeIndexForB1737OrOlder(eventType));
			}

			return Function.Call<bool>(Hash.HAS_PED_RECEIVED_EVENT, Handle, (int)eventType);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is responding to an event of <paramref name="eventType"/>.
		/// <see cref="EventType.Invalid"/> can be used to test if the <see cref="Ped"/> is responding to any event.
		/// </summary>
		/// <value>
		///   <see langword="true"/> if this <see cref="Ped"/> is responding to an event of <paramref name="eventType"/> and subsequent tasks are running; otherwise, <see langword="false" />.
		/// </value>
		public bool IsRespondingToEvent(EventType eventType)
		{
			if ((int)Game.Version < (int)GameVersion.v1_0_1868_0_Steam)
			{
				return Function.Call<bool>(Hash.IS_PED_RESPONDING_TO_EVENT, Handle, GetEventTypeIndexForB1737OrOlder(eventType));
			}

			return Function.Call<bool>(Hash.IS_PED_RESPONDING_TO_EVENT, Handle, (int)eventType);
		}

		private int GetEventTypeIndexForB1737OrOlder(EventType eventType)
		{
			if (eventType == EventType.Incapacitated)
			{
				throw new ArgumentException("EventType.Incapacitated is not available in the game versions prior to v1.0.1868.0.", nameof(eventType));
			}
			if (eventType == EventType.ShockingBrokenGlass)
			{
				throw new ArgumentException("EventType.ShockingBrokenGlass is not available in the game versions prior to v1.0.1868.0.", nameof(eventType));
			}

			int eventTypeCorrected = (int)eventType;
			if (eventTypeCorrected >= (int)EventType.ShockingCarAlarm)
			{
				eventTypeCorrected -= 2;
			}
			else if (eventTypeCorrected >= (int)EventType.LeaderEnteredCarAsDriver)
			{
				--eventTypeCorrected;
			}
			return eventTypeCorrected;
		}

		#endregion

		#region Euphoria & Ragdoll

		/// <summary>
		/// Switches this <see cref="Ped"/> to a ragdoll by starting a ragdoll task and applying to this <see cref="Ped"/>.
		/// If <paramref name="ragdollType"/> is not set to <see cref="RagdollType.Relax"/> or <see cref="RagdollType.ScriptControl"/>, the ragdoll behavior for <see cref="RagdollType.Balance"/> will be used.
		/// </summary>
		/// <param name="duration">
		/// The duration how long the ragdoll task will run in milliseconds.
		/// </param>
		/// <param name="ragdollType">The ragdoll type.</param>
		public void Ragdoll(int duration = -1, RagdollType ragdollType = RagdollType.Relax)
		{
			CanRagdoll = true;
			SetToRagdoll(duration, duration, ragdollType, false);
		}
		/// <summary>
		/// Switches this <see cref="Ped"/> to a ragdoll by starting a ragdoll task and applying to this <see cref="Ped"/>.
		/// If <paramref name="ragdollType"/> is not set to <see cref="RagdollType.Relax"/> or <see cref="RagdollType.ScriptControl"/>, the ragdoll behavior for <see cref="RagdollType.Balance"/> will be used.
		/// </summary>
		/// <param name="minTime">
		/// The duration at least how long the ragdoll task will run in milliseconds.
		/// Not used for <see cref="RagdollType.ScriptControl"/>.
		/// </param>
		/// <param name="maxTime">
		/// The duration at most how long the ragdoll task will run in milliseconds.
		/// Not used for <see cref="RagdollType.Balance"/>.
		/// </param>
		/// <param name="ragdollType">The ragdoll type.</param>
		/// <param name="forceScriptControl">
		/// Specifies whether this <see cref="Ped"/> will not get injured or killed by being lower health than <see cref="InjuryHealthThreshold"/> or <see cref="FatalInjuryHealthThreshold"/>.
		/// If ped's health goes lower than <see cref="InjuryHealthThreshold"/>, the ragdoll task will keep their health to <see cref="InjuryHealthThreshold"/> plus 5.0 until the task ends.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if this <see cref="Ped"/> has successfully started a new ragdoll task; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method will not start a new NaturalMotion(NM) ragdoll task if some NM task is already running on the <see cref="Ped"/>
		/// (except for <see cref="RagdollType.ScriptControl"/>).
		/// </para>
		/// <para>
		/// Unlike <see cref="Ragdoll(int, RagdollType)"/>, this method does not automatically set <see cref="CanRagdoll"/> to <see langword="true"/>.
		/// Set the property on your own if necessary.
		/// </para>
		/// </remarks>
		public bool SetToRagdoll(int minTime, int maxTime, RagdollType ragdollType, bool forceScriptControl = false)
		{
			// Looks like 4th and 5th parameter are completely unused
			return Function.Call<bool>(Hash.SET_PED_TO_RAGDOLL, Handle, minTime, maxTime, (int)ragdollType, false, false, forceScriptControl);
		}

		/// <summary>
		/// Gives this <see cref="Ped"/> to a specific NaturalMotion (NM) ragdoll task for fall down,
		/// which controls them to fall off a high place, fall down stairs etc.
		/// </summary>
		/// <param name="minTime">
		/// The duration at least how long the ragdoll task will run in milliseconds.
		/// After the NM task executed for longer than this value, it may end itself if the <see cref="Ped"/> is fallen down.
		/// </param>
		/// <param name="maxTime">
		/// The duration at most how long the ragdoll task will run in milliseconds.
		/// The task will be forced to stop if it has been executed for longer this value,
		/// even if the <see cref="Ped"/> is still not fallen down.
		/// </param>
		/// <param name="fallType">The ragdoll fall type.</param>
		/// <param name="direction">The direction to which the ped should stagger and fall.</param>
		/// <param name="groundHeight">The height the <see cref="Ped"/> is expected to fall down to.</param>
		/// <returns>
		/// <see langword="true"/> if this <see cref="Ped"/> has successfully started a new ragdoll task; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method can start a new NaturalMotion (NM) ragdoll task even if some NM task is already running on
		/// the <see cref="Ped"/>.
		/// </para>
		/// </remarks>
		public bool SetToRagdollWithFall(int minTime, int maxTime, RagdollFallType fallType, Vector3 direction, float groundHeight)
		{
			// Looks like 9th to 14th parameter are supposed to be grab points, but are they unused? (not confirmed)
			return Function.Call<bool>(
				Hash.SET_PED_TO_RAGDOLL_WITH_FALL,
				Handle,
				minTime,
				maxTime,
				(int)fallType,
				direction.X,
				direction.Y,
				direction.Z,
				groundHeight,
				0f,
				0f,
				0f,
				0f,
				0f,
				0f
				);
		}

		/// <summary>Stops this <see cref="Ped"/> ragdolling.</summary>
		public void CancelRagdoll()
		{
			Function.Call(Hash.SET_PED_TO_RAGDOLL, Handle, 1, 1, 1, false, false, false);
		}

		/// <summary>
		/// Blocks ragdoll reactions from various forms of damage.
		/// </summary>
		public void SetRagdollBlockingFlags(RagdollBlockingFlags flags = RagdollBlockingFlags.BulletImpact)
		{
			Function.Call(Hash.SET_RAGDOLL_BLOCKING_FLAGS, Handle, (int)flags);
		}
		/// <summary>
		/// Re-enables ragdoll reactions from various forms of damage.
		/// </summary>
		public void ClearRagdollBlockingFlags(RagdollBlockingFlags flags = RagdollBlockingFlags.BulletImpact)
		{
			Function.Call(Hash.CLEAR_RAGDOLL_BLOCKING_FLAGS, Handle, (int)flags);
		}

		/// <summary>
		/// Gets or sets whether this <see cref="Ped"/> is ragdolling.
		/// </summary>
		/// <remarks>
		/// Will return <see langword="false"/> when the <see cref="Ped"/> is getting up or writhing as a part of a ragdoll task.
		/// </remarks>
		public bool IsRagdoll => Function.Call<bool>(Hash.IS_PED_RAGDOLL, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is running a ragdoll task which manages its ragdoll.
		/// </summary>
		/// <remarks>
		/// Will return <see langword="true"/> when <see cref="IsRagdoll"/> returns <see langword="true"/> or the <see cref="Ped"/> is getting up or writhing as a part of a ragdoll task.
		/// </remarks>
		public bool IsRunningRagdollTask => Function.Call<bool>(Hash.IS_PED_RUNNING_RAGDOLL_TASK, Handle);

		/// <summary>
		/// Gets or sets whether this <see cref="Ped"/> can be set into a ragdoll state.
		/// </summary>
		/// <remarks>
		/// <see cref="Ped"/>s will only switch to a ragdoll if they are onscreen and within range of the player.
		/// </remarks>
		public bool CanRagdoll
		{
			get => Function.Call<bool>(Hash.CAN_PED_RAGDOLL, Handle);
			set => Function.Call(Hash.SET_PED_CAN_RAGDOLL, Handle, value);
		}

		/// <summary>
		/// Opens a list of <see cref="GTA.NaturalMotion.Euphoria"/> Helpers which can be applied to this <see cref="Ped"/>.
		/// </summary>
		public Euphoria Euphoria => _euphoria ?? (_euphoria = new Euphoria(this));

		#endregion

		#region Combat Configs

		/// <summary>
		/// Gets or sets how accurate this <see cref="Ped"/>s shooting ability is.
		/// The higher the value of this property is, the more likely it is that this <see cref="Ped"/> will shoot at exactly where they are aiming at.
		/// </summary>
		/// <value>
		/// The accuracy from 0 to 100, 0 being very inaccurate, which means this <see cref="Ped"/> cannot shoot at exactly where they are aiming at,
		/// 100 being perfectly accurate.
		/// </value>
		/// <remarks>
		/// The ped accuracy is internally stored as a <see cref="float"/>. To read/write the exact value, use
		/// <see cref="SetCombatFloatAttribute(CombatFloatAttributes, float)"/> or
		/// <see cref="GetCombatFloatAttribute(CombatFloatAttributes)"/> with
		/// <see cref="CombatFloatAttributes.WeaponAccuracy"/>.
		/// </remarks>
		public int Accuracy
		{
			get => Function.Call<int>(Hash.GET_PED_ACCURACY, Handle);
			set => Function.Call(Hash.SET_PED_ACCURACY, Handle, value);
		}

		/// <summary>
		/// Sets the rate this <see cref="Ped"/> will shoot at.
		/// </summary>
		/// <value>
		/// The shoot rate from 0 to 1000, 100 is the default value.
		/// </value>
		/// <remarks>
		/// The value will be internally stored as a <see cref="float"/>.
		/// This property internally sets the value divided by 100 (so the value will be internally 1.0 if you set 100
		/// to this property).
		/// </remarks>
		public int ShootRate
		{
			set => Function.Call(Hash.SET_PED_SHOOT_RATE, Handle, value);
		}

		/// <summary>
		/// Gets of sets the pattern this <see cref="Ped"/> uses to fire weapons.
		/// </summary>
		public FiringPattern FiringPattern
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.FiringPatternOffset == 0)
				{
					return 0;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0;
				}

				return (FiringPattern)SHVDN.NativeMemory.ReadInt32(address + SHVDN.NativeMemory.Ped.FiringPatternOffset);
			}
			set => Function.Call(Hash.SET_PED_FIRING_PATTERN, Handle, (uint)value);
		}

		/// <summary>
		/// Activates or deactivates the combat attributes.
		/// </summary>
		public void SetCombatAttribute(CombatAttributes attribute, bool activeSkill)
			=> Function.Call(Hash.SET_PED_COMBAT_ATTRIBUTES, Handle, (int)attribute, activeSkill);
		/// <summary>
		/// Sets a combat float attributes.
		/// </summary>
		/// <remarks>
		/// To write the shoot rate, use <see cref="ShootRate"/>.
		/// </remarks>
		public void SetCombatFloatAttribute(CombatFloatAttributes attribute, float newValue)
			=> Function.Call(Hash.SET_COMBAT_FLOAT, Handle, (int)attribute, newValue);
		/// <summary>
		/// Gets a combat float attributes.
		/// </summary>
		/// <remarks>
		/// To read the shoot rate, use <see cref="ShootRate"/>.
		/// </remarks>
		public float GetCombatFloatAttribute(CombatFloatAttributes attribute)
			=> Function.Call<float>(Hash.GET_COMBAT_FLOAT, Handle, (int)attribute);
		/// <summary>
		/// Activates or deactivates the flee attributes.
		/// </summary>
		public void SetFleeAttributes(FleeAttributes attributes, bool activeSkill)
			=> Function.Call(Hash.SET_PED_FLEE_ATTRIBUTES, Handle, (int)attributes, activeSkill);

		public CombatMovement CombatMovement
		{
			get => Function.Call<CombatMovement>(Hash.GET_PED_COMBAT_MOVEMENT, Handle);
			set => Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, Handle, (int)value);
		}
		public CombatAbility CombatAbility
		{
			set => Function.Call(Hash.SET_PED_COMBAT_ABILITY, Handle, (int)value);
		}
		public CombatRange CombatRange
		{
			get => Function.Call<CombatRange>(Hash.GET_PED_COMBAT_RANGE, Handle);
			set => Function.Call(Hash.SET_PED_COMBAT_RANGE, Handle, (int)value);
		}
		public TargetLossResponse TargetLossResponse
		{
			set => Function.Call(Hash.SET_PED_TARGET_LOSS_RESPONSE, Handle, (int)value);
		}

		#endregion

		#region Weapon Interaction

		/// <summary>
		/// Gets a collection of all this <see cref="Ped"/>s <see cref="Weapon"/>s.
		/// </summary>
		public WeaponCollection Weapons => _weapons ?? (_weapons = new WeaponCollection(this));

		/// <summary>
		/// Gets the vehicle weapon this <see cref="Ped"/> is using on <see cref="CurrentVehicle"/>.
		/// </summary>
		/// <returns>
		/// The vehicle weapon this <see cref="Ped"/> is currently using ifsuccessfully found a vehicle weapon;
		/// otherwise, <see cref="VehicleWeaponHash.Invalid"/>
		/// </returns>
		/// <remarks>
		/// Despite the interface, this property will eventually get or set the value on
		/// <see cref="CurrentVehicle"/> if it exists.
		/// </remarks>
		public VehicleWeaponHash VehicleWeapon
		{
			get
			{
				unsafe
				{
					int hash;
					return Function.Call<bool>(Hash.GET_CURRENT_PED_VEHICLE_WEAPON, Handle, &hash) ?
						(VehicleWeaponHash)hash : VehicleWeaponHash.Invalid;
				}
			}
			set => Function.Call(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, Handle, (uint)value);
		}

		/// <summary>
		/// Attempts to set the vehicle weapon this <see cref="Ped"/> is using on <see cref="CurrentVehicle"/>.
		/// </summary>
		/// <param name="hash">The vehicle weapon hash.</param>
		/// <returns>
		/// <see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Despite the interface, this property will set the value on <see cref="CurrentVehicle"/> if it exists and
		/// <paramref name="hash"/> is listed on the <see cref="VehicleWeaponHandlingData"/> for the current vehicle.
		/// </remarks>
		public bool TrySetVehicleWeapon(VehicleWeaponHash hash)
			=> Function.Call<bool>(Hash.SET_CURRENT_PED_VEHICLE_WEAPON, Handle, (uint)hash);

		/// <summary>
		/// Fires this <see cref="Ped"/>'s current vehicle weapon on the current <see cref="Vehicle"/>
		/// at the target <see cref="Entity"/> this frame.
		/// </summary>
		/// <param name="target">
		/// The target <see cref="Entity"/>.
		/// If the current vehicle weapon is a rocket weapon and supports homing,
		/// the fired rocket will home in the target.
		/// If the target does not exist, this method will not ignore or throw an exception and instead
		/// the <see cref="Vehicle"/> will fire at (0f, 0f, 0f) as <c>SET_VEHICLE_SHOOT_AT_TARGET</c> does.
		/// </param>
		/// <remarks>
		/// <para>
		/// If this <see cref="Ped"/> is not the player, the vehicle weapon will not follow the angle constraints.
		/// If this <see cref="Ped"/> is the player, the ped will shoot in the direction of vehicle is facing
		/// (does not disable homing in such case).
		/// </para>
		/// <para>
		/// The appropriate <c>CVehicleWeapon</c> of the current <see cref="Vehicle"/> shoots at the target
		/// if the prerequisite is satisfied (retrieves via this <see cref="Ped"/>'s <c>CPedWeaponManager</c>).
		/// </para>
		/// </remarks>
		public void FireVehicleWeaponAt(Entity target)
			=> Function.Call<bool>(Hash.SET_VEHICLE_SHOOT_AT_TARGET, Handle, target, 0f, 0f, 0f);
		/// <summary>
		/// Fires this <see cref="Ped"/>'s current vehicle weapon on the current <see cref="Vehicle"/>
		/// at the target coordinates this frame.
		/// </summary>
		/// <param name="target">
		/// The target coordinates.
		/// </param>
		/// <remarks>
		/// <para>
		/// If this <see cref="Ped"/> is not the player, the vehicle weapon will not follow the angle constraints.
		/// If this <see cref="Ped"/> is the player, the ped will shoot in the direction of vehicle is facing.
		/// </para>
		/// <para>
		/// The appropriate <c>CVehicleWeapon</c> of the current <see cref="Vehicle"/> shoots at the target
		/// if the prerequisite is satisfied (retrieves via this <see cref="Ped"/>'s <c>CPedWeaponManager</c>).
		/// </para>
		/// </remarks>
		public void FireVehicleWeaponAt(Vector3 target)
			=> Function.Call<bool>(Hash.SET_VEHICLE_SHOOT_AT_TARGET, Handle, null, target.X, target.Y, target.Z);

		/// <summary>
		/// Sets if this <see cref="Ped"/> can switch between different weapons.
		/// </summary>
		public bool CanSwitchWeapons
		{
			set => Function.Call(Hash.SET_PED_CAN_SWITCH_WEAPON, Handle, value);
		}

		#endregion

		#region Vehicle Interaction

		public bool IsOnBike => Function.Call<bool>(Hash.IS_PED_ON_ANY_BIKE, Handle);

		public bool IsOnFoot => Function.Call<bool>(Hash.IS_PED_ON_FOOT, Handle);

		public bool IsInSub => Function.Call<bool>(Hash.IS_PED_IN_ANY_SUB, Handle);

		public bool IsInTaxi => Function.Call<bool>(Hash.IS_PED_IN_ANY_TAXI, Handle);

		public bool IsInTrain => Function.Call<bool>(Hash.IS_PED_IN_ANY_TRAIN, Handle);

		public bool IsInHeli => Function.Call<bool>(Hash.IS_PED_IN_ANY_HELI, Handle);

		public bool IsInPlane => Function.Call<bool>(Hash.IS_PED_IN_ANY_PLANE, Handle);

		/// <summary>
		/// Indicates whether is in a plane or helicopter.
		/// </summary>
		public bool IsInFlyingVehicle => Function.Call<bool>(Hash.IS_PED_IN_FLYING_VEHICLE, Handle);

		public bool IsInBoat => Function.Call<bool>(Hash.IS_PED_IN_ANY_BOAT, Handle);

		public bool IsInPoliceVehicle => Function.Call<bool>(Hash.IS_PED_IN_ANY_POLICE_VEHICLE, Handle);

		/// <summary>
		/// Indicates whether is getting into a <see cref="Vehicle"/> but not sitting in a <see cref="Vehicle"/>.
		/// </summary>
		public bool IsGettingIntoVehicle => Function.Call<bool>(Hash.IS_PED_GETTING_INTO_A_VEHICLE, Handle);

		/// <summary>
		/// Indicates whether is currently exiting a <see cref="Vehicle"/>.
		/// </summary>
		public bool IsExitingVehicle => Function.Call<bool>(Hash.GET_IS_TASK_ACTIVE, Handle, 2 /* CTaskExitVehicle */);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> is jumping out of their vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Ped"/> is jumping out of their vehicle; otherwise, <see langword="false" />.
		/// </value>
		public bool IsJumpingOutOfVehicle => Function.Call<bool>(Hash.IS_PED_JUMPING_OUT_OF_VEHICLE, Handle);

		public bool IsTryingToEnterALockedVehicle => Function.Call<bool>(Hash.IS_PED_TRYING_TO_ENTER_A_LOCKED_VEHICLE, Handle);

		public bool CanBeDraggedOutOfVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_DRAGGED_OUT, Handle, value);
		}

		/// <summary>
		/// Sets the vehicle knock off type that determines how easy this <see cref="Ped"/> can be knocked off (fall off) a <see cref="Vehicle"/>.
		/// </summary>
		public KnockOffVehicleType KnockOffVehicleType
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.KnockOffVehicleTypeOffset == 0)
				{
					return KnockOffVehicleType.Default;
				}

				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return KnockOffVehicleType.Default;
				}

				// The knock off vehicle type value uses the first 2 bits
				return (KnockOffVehicleType)(SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.Ped.KnockOffVehicleTypeOffset) & 3);
			}
			set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, (int)value);
		}

		/// <summary>
		/// Get the value that indicates whether this <see cref="Ped"/> is in a bike and <see cref="KnockOffVehicleType"/> is not set to <see cref="GTA.KnockOffVehicleType.Never"/>
		/// so the <see cref="Ped"/> can be be knocked off (fall off) a <see cref="Vehicle"/>.
		/// </summary>
		public bool CanBeKnockedOffVehicle
		{
			get => Function.Call<bool>(Hash.CAN_KNOCK_PED_OFF_VEHICLE, Handle);
		}

		/// <summary>
		/// Sets the value that indicates whether this <see cref="Ped"/> can be knocked off a <see cref="Vehicle"/> (not limited to a bike despite the property name).
		/// </summary>
		[Obsolete("Use Ped.KnockOffVehicleType instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public bool CanBeKnockedOffBike
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, !value);
		}

		public bool CanFlyThroughWindscreen
		{
			get => Function.Call<bool>(Hash.GET_PED_CONFIG_FLAG, Handle, 32, true);
			set => Function.Call(Hash.SET_PED_CONFIG_FLAG, Handle, 32, value);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in or getting out any <see cref="Vehicle"/>.
		/// </summary>
		public bool IsInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, Handle, 0);
		}
		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in or getting out the specified <see cref="Vehicle"/>.
		/// </summary>
		public bool IsInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_VEHICLE, Handle, vehicle.Handle, 0);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in any <see cref="Vehicle"/>.
		/// </summary>
		public bool IsSittingInVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
		}
		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is sitting in the specified <see cref="Vehicle"/>.
		/// </summary>
		public bool IsSittingInVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_SITTING_IN_VEHICLE, Handle, vehicle.Handle);
		}

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is standing on any <see cref="Vehicle"/>.
		/// </summary>
		public bool IsStandingOnVehicle()
		{
			return Function.Call<bool>(Hash.IS_PED_ON_VEHICLE, Handle);
		}
		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is standing on the specified <see cref="Vehicle"/>.
		/// </summary>
		public bool IsStandingOnVehicle(Vehicle vehicle)
		{
			return Function.Call<bool>(Hash.IS_PED_ON_SPECIFIC_VEHICLE, Handle, vehicle.Handle);
		}

		/// <summary>
		/// Sets this <see cref="Ped"/> into the relevant seat of the specified <see cref="Vehicle"/>.
		/// </summary>
		/// <param name="vehicle">The vehicle this <see cref="Ped"/> will be set into.</param>
		/// <param name="seat">The seat to set.</param>
		public void SetIntoVehicle(Vehicle vehicle, VehicleSeat seat)
		{
			Function.Call(Hash.SET_PED_INTO_VEHICLE, Handle, vehicle.Handle, (int)seat);
		}

		/// <summary>
		/// Gets the last <see cref="Vehicle"/> this <see cref="Ped"/> used.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if the last vehicle doesn't exist.</remarks>
		public Vehicle LastVehicle
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				// GET_VEHICLE_PED_IS_IN isn't reliable at getting last vehicle since it returns 0 when the ped is going to a door of some vehicle or opening one.
				// Also, the native returns the vehicle's handle the ped is getting in when ped is getting in it (which is not the last vehicle), though the 2nd parameter name is supposed to be "ConsiderEnteringAsInVehicle" as a leaked header suggests.
				int vehicleHandle = SHVDN.NativeMemory.Ped.GetLastVehicleHandle(address);
				return vehicleHandle != 0 ? new Vehicle(vehicleHandle) : null;
			}
		}

		/// <summary>
		/// Gets the current <see cref="Vehicle"/> this <see cref="Ped"/> is using.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="Ped"/> isn't in a <see cref="Vehicle"/>.</remarks>
		public Vehicle CurrentVehicle
		{
			get
			{
				// In b2699 or later, GET_VEHICLE_PED_IS_IN always returns the last vehicle without checking the driving flag even when the 2nd argument is set to false.
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				int vehicleHandle = SHVDN.NativeMemory.Ped.GetVehicleHandlePedIsIn(address);
				return vehicleHandle != 0 ? new Vehicle(vehicleHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="Vehicle"/> this <see cref="Ped"/> is trying to enter.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="Ped"/> isn't trying to enter a <see cref="Vehicle"/>.</remarks>
		public Vehicle VehicleTryingToEnter
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_VEHICLE_PED_IS_TRYING_TO_ENTER, Handle);
				return handle != 0 ? new Vehicle(handle) : null;
			}
		}

		/// <summary>
		/// Gets the current <see cref="Vehicle"/> this <see cref="Ped"/> is standing on.
		/// </summary>
		/// <remarks>returns <see langword="null" /> if this <see cref="Ped"/> is not standing on a <see cref="Vehicle"/>.</remarks>
		public Vehicle VehicleStandingOn
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return null;
				}

				int vehicleHandle = SHVDN.NativeMemory.Ped.GetVehicleHandlePedIsStandingOn(address);
				return vehicleHandle != 0 ? new Vehicle(vehicleHandle) : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="VehicleSeat"/> this <see cref="Ped"/> is in.
		/// </summary>
		/// <value>
		/// The <see cref="VehicleSeat"/> this <see cref="Ped"/> is in if this <see cref="Ped"/> is in a <see cref="Vehicle"/>; otherwise, <see cref="VehicleSeat.None"/>.
		/// </value>
		public VehicleSeat SeatIndex
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SeatIndexOffset == 0)
				{
					return VehicleSeat.None;
				}

				int seatIndex = (sbyte)SHVDN.NativeMemory.ReadByte(address + SHVDN.NativeMemory.Ped.SeatIndexOffset);
				return (seatIndex >= 0 && IsInVehicle()) ? (VehicleSeat)(seatIndex - 1) : VehicleSeat.None;
			}
		}

		#endregion

		#region Driving

		/// <summary>
		/// <para>
		/// Sets the driving speed this <see cref="Ped"/> drives at.
		/// </para>
		/// <para>
		/// this <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the cruise speed field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public float DrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_CRUISE_SPEED, Handle, value);
		}

		/// <summary>
		/// <para>
		/// Sets the maximum driving speed this <see cref="Ped"/> can drive at.
		/// </para>
		/// <para>
		/// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the maximum cruise speed field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public float MaxDrivingSpeed
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_MAX_CRUISE_SPEED, Handle, value);
		}

		/// <summary>
		/// <para>
		/// Sets the the drive tasks driving style.
		/// </para>
		/// <para>
		/// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the driving flags field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		[Obsolete("Use Ped.VehicleDrivingFlags instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public DrivingStyle DrivingStyle
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_DRIVING_STYLE, Handle, (int)value);
		}

		/// <summary>
		/// <para>
		/// Sets the the drive tasks driving style.
		/// </para>
		/// <para>
		/// This <see cref="Ped"/> must be on a <see cref="Vehicle"/> as a driver and the drive task running on this <see cref="Ped"/> must be active before setting the value can actually affect.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Despite the interface, this actually changes the driving flags field on <c>CTaskVehicleMissionBase</c>, which is not for <see cref="Ped"/> but for <see cref="Vehicle"/>.
		/// </remarks>
		public VehicleDrivingFlags VehicleDrivingFlags
		{
			set => Function.Call(Hash.SET_DRIVE_TASK_DRIVING_STYLE, Handle, (uint)value);
		}

		/// <summary>
		/// Sets how aggressive a driver this <see cref="Ped"/> will be. Range is 0f to 1f, with 0f being no aggression and 1f being maximum aggression.
		/// </summary>
		public float DrivingAggressiveness
		{
			set => Function.Call(Hash.SET_DRIVER_AGGRESSIVENESS, Handle, value);
		}

		#endregion

		#region Jacking

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is in the process of dragging another <see cref="Ped"/> from a <see cref="Vehicle"/>.
		/// </summary>
		public bool IsJacking => Function.Call<bool>(Hash.IS_PED_JACKING, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is being dragged from their <see cref="Vehicle"/> by another <see cref="Ped"/>.
		/// </summary>
		public bool IsBeingJacked => Function.Call<bool>(Hash.IS_PED_BEING_JACKED, Handle);

		/// <summary>
		/// Sets a value indicating whether this <see cref="Ped"/> will stay in the vehicle when the driver gets jacked.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if <see cref="Ped"/> stays in vehicle when jacked; otherwise, <see langword="false" />.
		/// </value>
		public bool StaysInVehicleWhenJacked
		{
			set => Function.Call(Hash.SET_PED_STAY_IN_VEHICLE_WHEN_JACKED, Handle, value);
		}

		public Ped Jacker
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_PEDS_JACKER, Handle);
				return handle != 0 ? new Ped(handle) : null;
			}
		}

		public Ped JackTarget
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_JACK_TARGET, Handle);
				return handle != 0 ? new Ped(handle) : null;
			}
		}

		#endregion

		#region Parachuting

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> is in free-fall and ready to use a parachute.
		/// </summary>
		public bool IsInParachuteFreeFall => Function.Call<bool>(Hash.IS_PED_IN_PARACHUTE_FREE_FALL, Handle);

		/// <summary>
		/// Indicates whether this <see cref="Ped"/> running parachute task to open their parachute.
		/// </summary>
		public void OpenParachute()
		{
			Function.Call(Hash.FORCE_PED_TO_OPEN_PARACHUTE, Handle);
		}

		/// <summary>
		/// Gets the current state of this parachuting <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// Returns <see cref="ParachuteState.None"/> if this <see cref="Ped"/> is not parachuting.
		/// </remarks>
		public ParachuteState ParachuteState => Function.Call<ParachuteState>(Hash.GET_PED_PARACHUTE_STATE, Handle);

		/// <summary>
		/// Gets the current landing type of this parachuting <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// Returns <see cref="ParachuteState.None"/> if this <see cref="Ped"/> is not landing.
		/// </remarks>
		public ParachuteLandingType ParachuteLandingType => Function.Call<ParachuteLandingType>(Hash.GET_PED_PARACHUTE_LANDING_TYPE, Handle);

		#endregion

		#region Combat

		public bool IsEnemy
		{
			set => Function.Call(Hash.SET_PED_AS_ENEMY, Handle, value);
		}

		public bool IsPriorityTargetForEnemies
		{
			set => Function.Call(Hash.SET_ENTITY_IS_TARGET_PRIORITY, Handle, value, 0);
		}

		public bool IsFleeing => Function.Call<bool>(Hash.IS_PED_FLEEING, Handle);

		/// <summary>
		/// <para>
		/// Gets a value indicating whether this <see cref="Ped"/> is injured (<see cref="Entity.Health"/> of the <see cref="Ped"/> is lower than <see cref="InjuryHealthThreshold"/>) or does not exist.
		/// </para>
		/// <para>
		/// Since <see cref="Ped"/>s cannot start any scripted tasks if you try to give injured <see cref="Ped"/>s some of them,
		/// this property should be used to determine if the <see cref="Ped"/> is able to do anything in the game (i.e. run scripted tasks) instead of <see cref="Entity.IsDead"/>.
		/// You can reproduce the case where you give some <see cref="Ped"/> a scripted task but it will not start by modifying <see cref="InjuryHealthThreshold"/> and then giving them a scripted task.
		/// <see cref="Entity.IsDead"/> should be used only if you want to specifically know that the <see cref="Ped"/> is dead.
		/// </para>
		/// <para>
		/// Can be safely called to check if <see cref="Ped"/>s exist and are not injured without calling <see cref="Exists"/>.
		/// </para>
		/// </summary>
		/// <value>
		///   <see langword="true" /> this <see cref="Ped"/> is injured or does not exist; otherwise, <see langword="false" />.
		/// </value>
		/// <seealso cref="Entity.IsDead"/>
		/// <seealso cref="Exists"/>
		/// <remarks>
		/// Since GTA IV, Rockstar Games use the equivalent native function (<c>IS_PED_INJURED</c> in GTA V) to check if some <see cref="Ped"/> is (almost) dead
		/// instead of the equivalent one (<c>IS_ENTITY_DEAD</c> in GTA V) of <see cref="Entity.IsDead"/> in most cases.
		/// </remarks>
		public bool IsInjured => Function.Call<bool>(Hash.IS_PED_INJURED, Handle);

		public bool IsInStealthMode => Function.Call<bool>(Hash.GET_PED_STEALTH_MOVEMENT, Handle);

		public bool IsInCombat => Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle);

		/// <summary>
		/// Gets a value that indicates whether this <see cref="Ped"/> is in melee combat (doing a melee task, which
		/// is <c>CTaskMelee</c>).
		/// </summary>
		public bool IsInMeleeCombat => Function.Call<bool>(Hash.IS_PED_IN_MELEE_COMBAT, Handle);

		public bool IsAiming => GetConfigFlag(PedConfigFlags.IsAimingGun);

		public bool IsPlantingBomb => Function.Call<bool>(Hash.IS_PED_PLANTING_BOMB, Handle);

		public bool IsShooting => Function.Call<bool>(Hash.IS_PED_SHOOTING, Handle);

		public bool IsReloading => Function.Call<bool>(Hash.IS_PED_RELOADING, Handle);

		public bool IsDoingDriveBy => Function.Call<bool>(Hash.IS_PED_DOING_DRIVEBY, Handle);

		public bool IsGoingIntoCover => Function.Call<bool>(Hash.IS_PED_GOING_INTO_COVER, Handle);

		public bool IsAimingFromCover => Function.Call<bool>(Hash.IS_PED_AIMING_FROM_COVER, Handle);

		public bool IsBeingStunned => Function.Call<bool>(Hash.IS_PED_BEING_STUNNED, Handle);

		public bool IsInCover => Function.Call<bool>(Hash.IS_PED_IN_COVER, Handle, false);

		public bool IsInCoverFacingLeft => Function.Call<bool>(Hash.IS_PED_IN_COVER_FACING_LEFT, Handle);

		public bool CanBeTargetted
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset, 9);
			}
			set => Function.Call(Hash.SET_PED_CAN_BE_TARGETTED, Handle, value);
		}

		/// <summary>
		/// Sets if this <see cref="Ped"/> can take damage inflicted by regular bullets (not stun gun bullets) while in a <see cref="Vehicle"/>.
		/// </summary>
		public bool CanBeShotInVehicle
		{
			set => Function.Call(Hash.SET_PED_CAN_BE_SHOT_IN_VEHICLE, Handle, value);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> was killed by a stealth attack.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> was killed by stealth; otherwise, <see langword="false" />.
		/// </value>
		public bool WasKilledByStealth => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_STEALTH, Handle);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Ped"/> was killed by a takedown.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Ped"/> was killed by a takedown; otherwise, <see langword="false" />.
		/// </value>
		public bool WasKilledByTakedown => Function.Call<bool>(Hash.WAS_PED_KILLED_BY_TAKEDOWN, Handle);

		/// <summary>
		/// Gets the combat target <see cref="Ped"/> who this <see cref="Ped"/> is in combat with for a
		/// <c>CTaskCombat</c> of this <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// Although <c>GET_PED_TARGET_FROM_COMBAT_PED</c> is not present in v1.0.2245.0 or earlier game versions,
		/// this property supports all game versions. Unlike the native, this property does not check if the
		/// <see cref="Ped"/> is running <c>CTaskCombat</c> before this property can return a valid <see cref="Ped"/>.
		/// The native would return a valid <see cref="Ped"/> if it is in the case where a <c>CTaskCombat</c> would be
		/// found on an inactive task tree of <c>CTaskTreePed</c>, however, since it traverse the first
		/// <c>CTaskInfo</c> and its nodes on <c>CPedIntelligence</c>, not the active task tree.
		/// </remarks>
		public Ped CombatTarget
		{
			get
			{
				int targetEntityHandle = SHVDN.NativeMemory.Ped.GetCombatTargetPedHandleFromCombatPed(Handle);
				return targetEntityHandle != 0 ? new Ped(targetEntityHandle) : null;
			}
		}

		public bool IsInCombatAgainst(Ped target)
		{
			return Function.Call<bool>(Hash.IS_PED_IN_COMBAT, Handle, target.Handle);
		}

		#region Melee Action Result

		/// <summary>
		/// Returns whether this <see cref="Ped"/> is currently performing any type of melee action (attack, block,
		/// stealth kill, takedown, dodge, etc).
		/// </summary>
		/// <remarks>
		/// This property returns <see langword="true"/> when the <see cref="Ped"/> is executing a
		/// <c>CTaskMeleeActionResult</c> on the active task tree of the <c>CTaskTreePed</c> on <c>CPedIntelligence</c>.
		/// </remarks>
		public bool IsPerformingMeleeAction => Function.Call<bool>(Hash.IS_PED_PERFORMING_MELEE_ACTION, Handle);
		public bool IsPerformingStealthKill => Function.Call<bool>(Hash.IS_PED_PERFORMING_STEALTH_KILL, Handle);
		public bool IsPerformingCounterAttack => Function.Call<bool>(Hash.IS_PED_PERFORMING_A_COUNTER_ATTACK, Handle);
		/// <summary>
		/// Returns whether this <see cref="Ped"/> is currently being killed by a melee stealth action.
		/// </summary>
		public bool IsBeingStealthKilled => Function.Call<bool>(Hash.IS_PED_BEING_STEALTH_KILLED, Handle);

		/// <summary>
		/// Gets the melee target for this <see cref="Ped"/>. This <see cref="Ped"/> must be performing any type of
		/// melee action (attack, block, stealth kill, takedown, dodge, etc) before this property can return a valid
		/// <see cref="Ped"/>.
		/// </summary>
		/// <remarks>
		/// This property access the field of the target <see cref="Ped"/> address of a <c>CTaskMeleeActionResult</c>
		/// that is executed by this <see cref="Ped"/>.
		/// </remarks>
		public Ped MeleeTarget
		{
			get
			{
				int handle = Function.Call<int>(Hash.GET_MELEE_TARGET_FOR_PED, Handle);
				return handle != 0 ? new Ped(handle) : null;
			}
		}

		#endregion

		/// <summary>
		/// Gets the <see cref="Entity"/> that killed this <see cref="Ped"/>.
		/// </summary>
		public Entity Killer => FromHandle(Function.Call<int>(Hash.GET_PED_SOURCE_OF_DEATH, Handle));

		/// <summary>
		/// Gets the <see cref="WeaponHash"/> that this <see cref="Ped"/> is killed with. The return value is not necessarily a weapon hash for a human <see cref="Ped"/>s (e.g. can be the hash of <c>WEAPON_COUGAR</c>).
		/// </summary>
		public WeaponHash CauseOfDeath => Function.Call<WeaponHash>(Hash.GET_PED_CAUSE_OF_DEATH, Handle);

		/// <summary>
		/// Gets the time when this <see cref="Ped"/> is killed. This value determines how this <see cref="Ped"/> is rendered when <see cref="Game.IsThermalVisionActive"/> is <see langword="true" /> and the <see cref="Ped"/> is dead.
		/// </summary>
		public int TimeOfDeath => Function.Call<int>(Hash.GET_PED_TIME_OF_DEATH, Handle);

		/// <summary>
		/// <para>Clears the <see cref="Entity"/> record that killed this <see cref="Ped"/>. Can be useful after resurrecting this <see cref="Ped"/>.</para>
		/// <para>Internally, when a <see cref="Ped"/> killed and the value for the source of death in the instance of this <see cref="Ped"/> is not <c>0</c> (not <see langword="null" />), the game does not write the memory address of the <see cref="Ped"/> that killed this <see cref="Ped"/>.</para>
		/// </summary>
		public void ClearKillerRecord()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SourceOfDeathOffset == 0)
			{
				return;
			}

			SHVDN.NativeMemory.WriteAddress(address + SHVDN.NativeMemory.Ped.SourceOfDeathOffset, IntPtr.Zero);
		}

		/// <summary>
		/// <para>Clears the record of the cause of death that killed this <see cref="Ped"/> with. Can be useful after resurrecting this <see cref="Ped"/>.</para>
		/// <para>Internally, when a <see cref="Ped"/> killed and the value for the cause of death in the instance of this <see cref="Ped"/> is not <c>0</c>, the game does not write the weapon hash value for the cause of death.</para>
		/// </summary>
		public void ClearCauseOfDeathRecord()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.CauseOfDeathOffset == 0)
			{
				return;
			}

			SHVDN.NativeMemory.WriteInt32(address + SHVDN.NativeMemory.Ped.CauseOfDeathOffset, 0);
		}

		/// <summary>
		/// <para>Clears the time record when this <see cref="Ped"/> is killed. Can be useful after resurrecting this <see cref="Ped"/>.</para>
		/// <para>Internally, when a <see cref="Ped"/> killed and the value for the time of death in the instance of this <see cref="Ped"/> is not <c>0</c>, the game does not write the game time value for the time of death.</para>
		/// </summary>
		public void ClearTimeOfDeathRecord()
		{
			IntPtr address = MemoryAddress;
			if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.TimeOfDeathOffset == 0)
			{
				return;
			}

			SHVDN.NativeMemory.WriteInt32(address + SHVDN.NativeMemory.Ped.TimeOfDeathOffset, 0);
		}

		#endregion

		#region Damaging

		public bool CanWrithe
		{
			get => !GetConfigFlag(PedConfigFlags.DisableGoToWritheWhenInjured);
			set => SetConfigFlag(PedConfigFlags.DisableGoToWritheWhenInjured, !value);
		}

		/// <summary>
		/// Gets or Sets whether this <see cref="Ped"/> can suffer critical damage (which deals 1000 times base damages to non-player characters with default weapon configs) when bullets hit this <see cref="Ped"/>'s head bone or its child bones.
		/// If <see langword="false"/>, they will take base damage of weapons when bullets hit their head bone or its child bones, just like when bullets hit a bone other than their head bone, its child bones, or limb bones.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Ped"/> can suffer critical damage; otherwise, <see langword="false" />.
		/// </value>
		public bool CanSufferCriticalHits
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.SuffersCriticalHitOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.SuffersCriticalHitOffset, 2);
			}
			set => Function.Call(Hash.SET_PED_SUFFERS_CRITICAL_HITS, Handle, value);
		}

		/// <summary>
		/// Intended to set whether this <see cref="Ped"/> will die when injured, but practically do nothing meaningful.
		/// </summary>
		public bool DiesOnLowHealth
		{
			set => Function.Call(Hash.SET_PED_DIES_WHEN_INJURED, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> will die instantly if they find themselves in a body of water.
		/// </summary>
		/// <remarks>
		/// The complete submersion into water does not guarantee this <see cref="Ped"/> will die if this <see cref="Ped"/> is the player one.
		/// </remarks>
		public bool DiesInstantlyInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_INSTANTLY_IN_WATER, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> can take damage for being deep water.
		/// If this <see cref="Ped"/> is the player one, setting to <see langword="false"/> will enable the player to be deep water without taking <c>WEAPON_DROWNING</c> damage.
		/// </summary>
		public bool DrownsInWater
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_WATER, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> can take <c>WEAPON_DROWNING</c> damage in a sinking vehicle.
		/// If this <see cref="Ped"/> is the player one, setting to <see langword="false"/> will enable the player to be in a sinking vehicle without taking <c>WEAPON_DROWNING</c> damage.
		/// </summary>
		public bool DrownsInSinkingVehicle
		{
			set => Function.Call(Hash.SET_PED_DIES_IN_SINKING_VEHICLE, Handle, value);
		}

		/// <summary>
		/// Sets whether this <see cref="Ped"/> will drop the current weapon when they get killed.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if <see cref="Ped"/> drops the current weapon when killed; otherwise, <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <see cref="Ped"/>s will drop only their current weapon when they get killed.
		/// </remarks>
		public bool DropsEquippedWeaponOnDeath
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset == 0)
				{
					return false;
				}

				return !SHVDN.NativeMemory.IsBitSet(address + SHVDN.NativeMemory.Ped.DropsWeaponsWhenDeadOffset, 14);
			}
			set => Function.Call(Hash.SET_PED_DROPS_WEAPONS_WHEN_DEAD, Handle, value);
		}

		public void ApplyDamage(int damageAmount)
		{
			Function.Call(Hash.APPLY_DAMAGE_TO_PED, Handle, damageAmount, true);
		}

		public override bool HasBeenDamagedBy(WeaponHash weapon)
		{
			return Function.Call<bool>(Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, Handle, (uint)weapon, 0);
		}

		public override bool HasBeenDamagedByAnyWeapon()
		{
			return Function.Call<bool>(Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, Handle, 0, 2);
		}

		public override bool HasBeenDamagedByAnyMeleeWeapon()
		{
			return Function.Call<bool>(Hash.HAS_PED_BEEN_DAMAGED_BY_WEAPON, Handle, 0, 1);
		}

		public override void ClearLastWeaponDamage()
		{
			Function.Call(Hash.CLEAR_PED_LAST_WEAPON_DAMAGE, Handle);
		}

		/// <summary>
		/// Gets or sets the injury health threshold for this <see cref="Ped"/>.
		/// The pedestrian is considered injured and cannot start any scripted tasks when its health drops below this value.
		/// The pedestrian dies on attacks when its health is below this value.
		/// </summary>
		/// <value>
		/// The injury health threshold. Should be below <see cref="Entity.MaxHealth"/>.
		/// </value>
		/// <remarks>
		/// Note on player controlled pedestrians: One of the game scripts will consider the player wasted when their health drops below this setting value.
		/// </remarks>
		public float InjuryHealthThreshold
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.InjuryHealthThresholdOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.InjuryHealthThresholdOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.InjuryHealthThresholdOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.InjuryHealthThresholdOffset, value);
			}
		}

		/// <summary>
		/// Gets or sets the fatal injury health threshold for this <see cref="Ped"/>.
		/// The pedestrian health will be set to 0.0 when it drops below this value.
		/// </summary>
		/// <value>
		/// The fatal injury health threshold. Should be below <see cref="Entity.MaxHealth"/>.
		/// </value>
		/// <remarks>
		/// Note on player controlled pedestrians: One of the game scripts will consider the player wasted when their health drops below <see cref="Ped.InjuryHealthThreshold"/>, regardless of this setting.
		/// </remarks>
		public float FatalInjuryHealthThreshold
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.FatalInjuryHealthThresholdOffset == 0)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.FatalInjuryHealthThresholdOffset);
			}
			set
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero || SHVDN.NativeMemory.Ped.FatalInjuryHealthThresholdOffset == 0)
				{
					return;
				}

				SHVDN.NativeMemory.WriteFloat(address + SHVDN.NativeMemory.Ped.FatalInjuryHealthThresholdOffset, value);
			}
		}

		/// <summary>
		/// Gets the last position a weapon of this <see cref="Ped"/> was impacted at this frame.
		/// </summary>
		/// <remarks>
		/// This property should be called every frame as the the last valid result lasts only the frame a weapon of this <see cref="Ped"/>
		/// was impacted at and else it returns <see cref="Vector3.Zero"/>.
		/// </remarks>
		public Vector3 LastWeaponImpactPosition
		{
			get
			{
				NativeVector3 position;
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_PED_LAST_WEAPON_IMPACT_COORD, Handle, &position))
					{
						return position;
					}
				}
				return Vector3.Zero;
			}
		}

		#endregion

		#region Relationship

		/// <summary>
		/// Gets the relationship between this <see cref="Ped"/> and <paramref name="ped"/>.
		/// </summary>
		/// <param name="ped"></param>
		/// <remarks>
		/// This property returns <see cref="Relationship.Pedestrians"/> if the relationship is not set.
		/// </remarks>
		public Relationship GetRelationshipWithPed(Ped ped)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped.Handle);
		}

		/// <summary>
		/// Gets or sets the <see cref="RelationshipGroup"/> this <see cref="Ped"/> belongs to.
		/// </summary>
		public RelationshipGroup RelationshipGroup
		{
			get => new(Function.Call<int>(Hash.GET_PED_RELATIONSHIP_GROUP_HASH, Handle));
			set => Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, Handle, value.Hash);
		}

		#endregion

		#region Perception

		/// <summary>
		/// Gets or sets how far this <see cref="Ped"/> can see.
		/// </summary>
		public float SeeingRange
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.SeeingRangeOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.SeeingRangeOffset);
			}
			set => Function.Call(Hash.SET_PED_SEEING_RANGE, Handle, value);
		}

		/// <summary>
		/// Gets or sets how far this <see cref="Ped"/> can hear.
		/// </summary>
		public float HearingRange
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.HearingRangeOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.HearingRangeOffset);
			}
			set => Function.Call(Hash.SET_PED_HEARING_RANGE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the minimum horizontal field of view for this <see cref="Ped"/>.
		/// Should be negative.
		/// </summary>
		public float VisualFieldMinAngle
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldMinAngleOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldMinAngleOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_MIN_ANGLE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the maximum horizontal field of view for this <see cref="Ped"/>.
		/// Should be positive.
		/// </summary>
		public float VisualFieldMaxAngle
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldMaxAngleOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldMaxAngleOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_MAX_ANGLE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the minimum vertical field of view for this <see cref="Ped"/>.
		/// Should be negative.
		/// </summary>
		public float VisualFieldMinElevationAngle
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldMinElevationAngleOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldMinElevationAngleOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_MIN_ELEVATION_ANGLE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the maximum vertical field of view for this <see cref="Ped"/>.
		/// Should be positive.
		/// </summary>
		public float VisualFieldMaxElevationAngle
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldMaxElevationAngleOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldMaxElevationAngleOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_MAX_ELEVATION_ANGLE, Handle, value);
		}

		/// <summary>
		/// Gets or sets how far the perhiperal vision of this <see cref="Ped"/> extends.
		/// </summary>
		public float VisualFieldPeripheralRange
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldPeripheralRangeOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldPeripheralRangeOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_PERIPHERAL_RANGE, Handle, value);
		}

		/// <summary>
		/// Gets or sets the central visual field angle of this <see cref="Ped"/>.
		/// </summary>
		public float VisualFieldCenterAngle
		{
			get
			{
				if (SHVDN.NativeMemory.Ped.VisualFieldCenterAngleOffset == 0)
				{
					return 0.0f;
				}

				IntPtr address = PedIntelligenceAddress;
				if (address == IntPtr.Zero)
				{
					return 0.0f;
				}

				return SHVDN.NativeMemory.ReadFloat(address + SHVDN.NativeMemory.Ped.VisualFieldCenterAngleOffset);
			}
			set => Function.Call(Hash.SET_PED_VISUAL_FIELD_CENTER_ANGLE, Handle, value);
		}

		#endregion

		#region Group

		/// <summary>
		/// Gets if this <see cref="Ped"/> is in a <see cref="PedGroup"/>.
		/// </summary>
		public bool IsInGroup => Function.Call<bool>(Hash.IS_PED_IN_GROUP, Handle);

		public void LeaveGroup()
		{
			Function.Call(Hash.REMOVE_PED_FROM_GROUP, Handle);
		}

		public bool NeverLeavesGroup
		{
			set => Function.Call(Hash.SET_PED_NEVER_LEAVES_GROUP, Handle, value);
		}

		/// <summary>
		/// Gets the PedGroup this <see cref="Ped"/> is in.
		/// </summary>
		public PedGroup PedGroup => IsInGroup ? new PedGroup(Function.Call<int>(Hash.GET_PED_GROUP_INDEX, Handle, false)) : null;

		#endregion

		#region Speeches
		// This region is for properties and methods that access an audSpeechAudioEntity instance

		public bool IsPainAudioEnabled
		{
			set => Function.Call(Hash.DISABLE_PED_PAIN_AUDIO, Handle, !value);
		}

		public bool IsAmbientSpeechPlaying => Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_PLAYING, Handle);

		public bool IsScriptedSpeechPlaying => Function.Call<bool>(Hash.IS_SCRIPTED_SPEECH_PLAYING, Handle);

		public bool IsAnySpeechPlaying => Function.Call<bool>(Hash.IS_ANY_SPEECH_PLAYING, Handle);

		public bool IsAmbientSpeechEnabled => !Function.Call<bool>(Hash.IS_AMBIENT_SPEECH_DISABLED, Handle);

		public void PlayAmbientSpeech(string speechName, SpeechModifier modifier = SpeechModifier.Standard)
		{
			if (modifier < 0 || (int)modifier >= _speechModifierNames.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(modifier));
			}

			Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_NATIVE, Handle, speechName, _speechModifierNames[(int)modifier]);
		}
		public void PlayAmbientSpeech(string speechName, string voiceName, SpeechModifier modifier = SpeechModifier.Standard)
		{
			if (modifier < 0 || (int)modifier >= _speechModifierNames.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(modifier));
			}

			Function.Call(Hash.PLAY_PED_AMBIENT_SPEECH_WITH_VOICE_NATIVE, Handle, speechName, voiceName, _speechModifierNames[(int)modifier], 0);
		}
		/// <summary>
		/// Stops currently playing speech (pain, ambient, scripted, breathing).
		/// </summary>
		public void StopCurrentPlayingSpeech() => Function.Call(Hash.STOP_CURRENT_PLAYING_SPEECH, Handle);
		/// <summary>
		/// Stops currently playing ambient speech.
		/// </summary>
		/// <remarks>
		/// Does nothing if the currently playing speech is not an ambient one.
		/// </remarks>
		public void StopCurrentPlayingAmbientSpeech() => Function.Call(Hash.STOP_CURRENT_PLAYING_AMBIENT_SPEECH, Handle);

		/// <summary>
		/// Sets the ambient voice to use when this <see cref="Ped"/> speaks.
		/// </summary>
		/// <remarks>
		/// The voice name will be stored as a joaat hash converted in the same way as <see cref="Game.GenerateHash(string)"/> does.
		/// </remarks>
		public string Voice
		{
			set => Function.Call(Hash.SET_AMBIENT_VOICE_NAME, Handle, value);
		}

		#endregion

		#region Animation

		/// <summary>
		/// Sets the value that indicates this <see cref="Ped"/> can play gesture animations.
		/// </summary>
		public bool CanPlayGestures
		{
			set => Function.Call(Hash.SET_PED_CAN_PLAY_GESTURE_ANIMS, Handle, value);
		}

		/// <summary>
		/// Sets the movement clip/animation set this <see cref="Ped"/> should use or <see langword="null"/>
		/// to reset to the default value defined in <c>peds.meta</c> under <c>&lt;MovementClipSet&gt;</c>.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When the value is set to <see langword="null"/>, <see cref="TaskInvoker.ClearAll"/> will be called
		/// right after resetting the movement clipset.
		/// </para>
		/// <para>
		/// Despite what the doc for this property said in between v3.0.0 in v3.6.0, the loading state of some animation
		/// dictionaries has nothing to do with this property. Specifying a string only registered as a clip/animation
		/// dictionary will result in the setter failure.
		/// </para>
		/// </remarks>
		[Obsolete("Use Ped.SetMovementClipSet or Ped.ResetMovementClipSet instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public string MovementAnimationSet
		{
			set
			{
				if (value == null)
				{
					Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Handle, 0.25f);
					Task.ClearAll();
				}
				else
				{
					Function.Call(Hash.REQUEST_CLIP_SET, value);
					int startTime = Environment.TickCount;

					while (!Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, value))
					{
						Script.Yield();

						if (Environment.TickCount - startTime >= 1000)
						{
							return;
						}
					}

					Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Handle, value, 0.25f);
				}
			}
		}

		/// <summary>
		/// Sets the movement clipset this <see cref="Ped"/> should use.
		/// Do not forget to stream in/load the clipset you want to load, or the method silently will fail.
		/// </summary>
		/// <remarks>
		/// Unlike any methods that requires some resource to be loaded before the main operation and that are present in v3.6.0,
		/// this method does not load the clipset automatically. Load the clipset you want to load beforehand.
		/// </remarks>
		public void SetMovementClipSet(ClipSet clipSet, float blendDuration = 0.25f)
			=> Function.Call(Hash.SET_PED_MOVEMENT_CLIPSET, Handle, clipSet, blendDuration);

		/// <summary>
		/// Resets the movement clipset to the default value defined in <c>peds.meta</c> under <c>&lt;MovementClipSet&gt;</c>.
		/// Do not forget to unstream the clipset if no longer needed.
		/// </summary>
		public void ResetMovementClipSet(float blendDuration = 0.25f)
			=> Function.Call(Hash.RESET_PED_MOVEMENT_CLIPSET, Handle, blendDuration);

		/// <summary>
		/// Sets the strafe clipset this <see cref="Ped"/> should use.
		/// Do not forget to stream in/load the clipset you want to load, or the method silently will fail.
		/// </summary>
		/// <remarks>
		/// Unlike any methods that requires some resource to be loaded before the main operation and that are present in v3.6.0,
		/// this method does not load the clipset automatically. Load the clipset you want to load beforehand.
		/// </remarks>
		public void SetStrafeClipSet(ClipSet clipSet)
			=> Function.Call(Hash.SET_PED_STRAFE_CLIPSET, Handle, clipSet);

		/// <summary>
		/// Resets the strafe clipset to the default value defined in <c>peds.meta</c> under <c>&lt;StrafeClipSet&gt;</c>.
		/// Do not forget to unstream the clipset if no longer needed.
		/// </summary>
		public void ResetStrafeClipSet()
			=> Function.Call(Hash.RESET_PED_STRAFE_CLIPSET, Handle);

		/// <summary>
		/// Sets the weapon movement clipset this <see cref="Ped"/> should use.
		/// Do not forget to stream in/load the clipset you want to load, or the method silently will fail.
		/// </summary>
		/// <remarks>
		/// Unlike any methods that requires some resource to be loaded before the main operation and that are present in v3.6.0,
		/// this method does not load the clipset automatically. Load the clipset you want to load beforehand.
		/// </remarks>
		public void SetWeaponMovementClipSet(ClipSet clipSet)
			=> Function.Call(Hash.SET_PED_WEAPON_MOVEMENT_CLIPSET, Handle, clipSet);

		/// <summary>
		/// Resets the weapon movement clipset to the default value defined in <c>peds.meta</c> under <c>&lt;StrafeClipSet&gt;</c>.
		/// Do not forget to unstream the clipset if no longer needed.
		/// </summary>
		public void ResetWeaponMovementClipSet()
			=> Function.Call(Hash.RESET_PED_WEAPON_MOVEMENT_CLIPSET, Handle);

		#endregion

		#region Forces

		/// <summary>
		/// Applies a world force to this <see cref="Entity"/> using world offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, RagdollComponent, bool, bool, bool, bool, bool)"/>
		public void ApplyWorldForceWorldOffset(Vector3 force, Vector3 offset, ForceType forceType, RagdollComponent component, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, component, false, false, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a world force to this <see cref="Entity"/> using relative offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, RagdollComponent, bool, bool, bool, bool, bool)"/>
		public void ApplyWorldForceRelativeOffset(Vector3 force, Vector3 offset, ForceType forceType, RagdollComponent component, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, component, false, true, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a relative force to this <see cref="Entity"/> using world offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, RagdollComponent, bool, bool, bool, bool, bool)"/>
		public void ApplyRelativeForceWorldOffset(Vector3 force, Vector3 offset, ForceType forceType, RagdollComponent component, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, component, true, false, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a relative force to this <see cref="Entity"/> using relative offset.
		/// </summary>
		/// <inheritdoc cref="ApplyForceInternal(Vector3, Vector3, ForceType, RagdollComponent, bool, bool, bool, bool, bool)"/>
		public void ApplyRelativeForceRelativeOffset(Vector3 force, Vector3 offset, ForceType forceType, RagdollComponent component, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			ApplyForceInternal(force, offset, forceType, component, true, true, scaleByMass, triggerAudio, scaleByTimeScale);
		}
		/// <summary>
		/// Applies a force to this <see cref="Entity"/>.
		/// </summary>
		/// <param name="force">The force to be applied.</param>
		/// <param name="offset">The offset from center of entity at which to apply force.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		/// <param name="component">Component of the entity to apply the force.</param>
		/// <param name="relativeForce">
		/// Specifies whether the force vector passed in is in relative or world coordinates.
		/// Relative coordinates (<see langword="true"/>) means the force will get automatically transformed into world space before being applied.
		/// </param>
		/// <param name="relativeOffset">Specifies whether the offset passed in is in relative or world coordinates.</param>
		/// <param name="scaleByMass">
		/// <para>Specifies whether to scale the force by mass.</para>
		/// <para>If <see langword="true"/>, force will be multiplied by mass. For example, force passed in is in fact an acceleration rate in <c>m/s*s</c> (force) or velocity change in <c>m/s</c> (impulse).</para>
		/// <para>If <see langword="false"/>, force will be applied directly and it's effect will depend on the mass of the entity. For example, force passed in is a proper force in Newtons (force) or a step change in momentum <c>kg*m/s</c> (impulse).</para>
		/// <para>
		/// In other words, scaling by mass is probably easier in most situations -
		/// if the mass of the object changes it's behaviour shouldn't, and it's easier to picture the effect because an acceleration rate of <c>10.0</c> is approximately the same as gravity (<c>9.81</c> to be more precise).
		/// </para>
		/// </param>
		/// <param name="triggerAudio">
		/// <para>Specifies whether to play audio events related to the force being applied. The sound will play only if the entity type is <see cref="Vehicle"/> and will play a suspension squeal depending on the magnitude of the force.</para>
		/// <para>The sound will play even if regardless of <see cref="ForceType"/> (even with a value other than between 0 to 5).</para>
		/// </param>
		/// <param name="scaleByTimeScale">
		/// <para>Specifies whether scale the force by the current time scale (max: <c>1.0f</c>).</para>
		///	<para>Only affects when <paramref name="forceType"/> is <see cref="ForceType.InternalImpulse"/> or <see cref="ForceType.ExternalImpulse"/>.</para>
		/// </param>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="component"/> not a value defined in <see cref="RagdollComponent"/>.</exception>
		private void ApplyForceInternal(Vector3 force, Vector3 offset, ForceType forceType, RagdollComponent component, bool relativeForce, bool relativeOffset, bool scaleByMass, bool triggerAudio = false, bool scaleByTimeScale = true)
		{
			// The game can crash the game if component value is out of bound
			// The native doesn't check the current frag type child count when access to the frag type child for the corresponding component index if the entity is ped
			if ((int)component < (int)RagdollComponent.Buttocks || (int)component > (int)RagdollComponent.Head)
			{
				throw new ArgumentOutOfRangeException(nameof(component));
			}

			Function.Call(Hash.APPLY_FORCE_TO_ENTITY, Handle, (int)forceType, force.X, force.Y, force.Z, offset.X, offset.Y, offset.Z, (int)component, relativeForce, relativeOffset, scaleByMass, triggerAudio, scaleByTimeScale);
		}

		/// <summary>
		/// Applies a world force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <inheritdoc cref="ApplyForceCenterOfMassInternal(Vector3, ForceType, RagdollComponent, bool, bool, bool)"/>
		public void ApplyWorldForceCenterOfMass(Vector3 force, ForceType forceType, RagdollComponent component, bool scaleByMass, bool applyToChildren = false)
		{
			ApplyForceCenterOfMassInternal(force, forceType, component, false, scaleByMass, applyToChildren);
		}
		/// <summary>
		/// Applies a relative force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <inheritdoc cref="ApplyForceCenterOfMassInternal(Vector3, ForceType, RagdollComponent, bool, bool, bool)"/>
		public void ApplyRelativeForceCenterOfMass(Vector3 force, ForceType forceType, RagdollComponent component, bool scaleByMass, bool applyToChildren = false)
		{
			ApplyForceCenterOfMassInternal(force, forceType, component, true, scaleByMass, applyToChildren);
		}
		/// <summary>
		/// Applies a force to the center of mass of this <see cref="Entity"/>.
		/// <paramref name="forceType"/> must not be <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>.
		/// </summary>
		/// <param name="force">The force to be applied.</param>
		/// <param name="forceType">Type of the force to apply.</param>
		/// <param name="component">Component of the entity to apply the force.</param>
		/// <param name="relativeForce">
		/// Specifies whether the force vector passed in is in relative or world coordinates.
		/// Relative coordinates (<see langword="true"/>) means the force will get automatically transformed into world space before being applied.
		/// </param>
		/// <param name="scaleByMass">
		/// <para>Specifies whether to scale the force by mass.</para>
		/// <para>If <see langword="true"/>, force will be multiplied by mass. For example, force passed in is in fact an acceleration rate in <c>m/s*s</c> (force) or velocity change in <c>m/s</c> (impulse).</para>
		/// <para>If <see langword="false"/>, force will be applied directly and it's effect will depend on the mass of the entity. For example, force passed in is a proper force in Newtons (force) or a step change in momentum <c>kg*m/s</c> (impulse).</para>
		/// <para>
		/// In other words, scaling by mass is probably easier in most situations -
		/// if the mass of the object changes it's behaviour shouldn't, and it's easier to picture the effect because an acceleration rate of <c>10.0</c> is approximately the same as gravity (<c>9.81</c> to be more precise).
		/// </para>
		/// </param>
		/// <param name="applyToChildren">Specifies whether to apply force to children components as well as the speficied component.</param>
		/// <exception cref="System.ArgumentException">Thrown when <paramref name="forceType"/> is set to <see cref="ForceType.ExternalForce"/> or <see cref="ForceType.ExternalImpulse"/>, which is not supported by this method.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="component"/> not a value defined in <see cref="RagdollComponent"/>.</exception>
		private void ApplyForceCenterOfMassInternal(Vector3 force, ForceType forceType, RagdollComponent component, bool relativeForce, bool scaleByMass, bool applyToChildren = false)
		{
			// The native won't apply the force if apply force type is one of the external types
			if (forceType == ForceType.ExternalForce && forceType == ForceType.ExternalImpulse)
			{
				throw new ArgumentException(nameof(forceType), "ForceType.ExternalForce and ForceType.ExternalImpulse are not supported.");
			}
			// The game can crash the game if component value is out of bound
			// The native doesn't check the current frag type child count when access to the frag type child for the corresponding component index if the entity is ped
			if ((int)component < (int)RagdollComponent.Buttocks || (int)component > (int)RagdollComponent.Head)
			{
				throw new ArgumentOutOfRangeException(nameof(component));
			}

			Function.Call(Hash.APPLY_FORCE_TO_ENTITY_CENTER_OF_MASS, Handle, (int)forceType, force.X, force.Y, force.Z, (int)component, relativeForce, scaleByMass, applyToChildren);
		}

		#endregion

		public static PedHash[] GetAllModels()
		{
			return SHVDN.NativeMemory.PedModels.Select(x => (PedHash)x).ToArray();
		}
		/// <summary>
		/// Gets an array of all loaded <see cref="PedHash"/>s that are appropriate to spawn as ambient peds.
		/// The result array can contain animal hashes and gang ped hashes, which CREATE_RANDOM_PED excludes from spawning.
		/// All the model hashes of the elements are loaded and the <see cref="Ped"/>s with the model hashes can be spawned immediately.
		/// </summary>
		public static PedHash[] GetAllLoadedModelsAppropriateForAmbientPeds()
		{
			return SHVDN.NativeMemory.GetLoadedAppropriatePedHashes()
				.Select(x => (PedHash)x)
				.ToArray();
		}
	}
}
