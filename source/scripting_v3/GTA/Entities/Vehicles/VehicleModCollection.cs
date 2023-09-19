//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace GTA
{
	public sealed class VehicleModCollection
	{
		#region Fields
		readonly Vehicle _owner;
		readonly Dictionary<VehicleModType, VehicleMod> _vehicleMods = new();
		readonly Dictionary<VehicleToggleModType, VehicleToggleMod> _vehicleToggleMods = new();

		private static readonly ReadOnlyDictionary<VehicleWheelType, Tuple<string, string>> _wheelNames = new(
			new Dictionary<VehicleWheelType, Tuple<string, string>>
			{
				{VehicleWheelType.BikeWheels, new Tuple<string, string>("CMOD_WHE1_0", "Bike")},
				{VehicleWheelType.HighEnd, new Tuple<string, string>("CMOD_WHE1_1", "High End")},
				{VehicleWheelType.Lowrider, new Tuple<string, string>("CMOD_WHE1_2", "Lowrider")},
				{VehicleWheelType.Muscle, new Tuple<string, string>("CMOD_WHE1_3", "Muscle")},
				{VehicleWheelType.Offroad, new Tuple<string, string>("CMOD_WHE1_4", "Offroad")},
				{VehicleWheelType.Sport, new Tuple<string, string>("CMOD_WHE1_5", "Sport")},
				{VehicleWheelType.SUV, new Tuple<string, string>("CMOD_WHE1_6", "SUV")},
				{VehicleWheelType.Tuner, new Tuple<string, string>("CMOD_WHE1_7", "Tuner")},
				{VehicleWheelType.BennysOriginals, new Tuple<string, string>("CMOD_WHE1_8", "Benny's Originals")},
				{VehicleWheelType.BennysBespoke, new Tuple<string, string>("CMOD_WHE1_9", "Benny's Bespoke")}
			});
		#endregion

		internal VehicleModCollection(Vehicle owner)
		{
			_owner = owner;
		}

		public VehicleMod this[VehicleModType modType]
		{
			get
			{
				if (_vehicleMods.TryGetValue(modType, out VehicleMod vehicleMod))
				{
					return vehicleMod;
				}

				vehicleMod = new VehicleMod(_owner, modType);
				_vehicleMods.Add(modType, vehicleMod);

				return vehicleMod;
			}
		}

		public VehicleToggleMod this[VehicleToggleModType modType]
		{
			get
			{
				if (_vehicleToggleMods.TryGetValue(modType, out VehicleToggleMod vehicleToggleMod))
				{
					return vehicleToggleMod;
				}

				vehicleToggleMod = new VehicleToggleMod(_owner, modType);
				_vehicleToggleMods.Add(modType, vehicleToggleMod);

				return vehicleToggleMod;
			}
		}

		public bool Contains(VehicleModType type)
		{
			return Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, _owner.Handle, (int)type) > 0;
		}

		public VehicleMod[] ToArray()
		{
			return Enum.GetValues(typeof(VehicleModType)).Cast<VehicleModType>().Where(Contains).Select(modType => this[modType]).ToArray();
		}

		public VehicleWheelType WheelType
		{
			get => Function.Call<VehicleWheelType>(Hash.GET_VEHICLE_WHEEL_TYPE, _owner.Handle);
			set => Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, _owner.Handle, (int)value);
		}

		public VehicleWheelType[] AllowedWheelTypes
		{
			get
			{
				if (_owner.Model.IsBicycle || _owner.Model.IsBike)
				{
					return new VehicleWheelType[] { VehicleWheelType.BikeWheels };
				}

				if (!_owner.Model.IsCar)
				{
					return Array.Empty<VehicleWheelType>();
				}

				var res = new List<VehicleWheelType>()
				{
					VehicleWheelType.Sport,
					VehicleWheelType.Muscle,
					VehicleWheelType.Lowrider,
					VehicleWheelType.SUV,
					VehicleWheelType.Offroad,
					VehicleWheelType.Tuner,
					VehicleWheelType.HighEnd
				};
				switch ((VehicleHash)_owner.Model)
				{
					case VehicleHash.Faction2:
					case VehicleHash.Buccaneer2:
					case VehicleHash.Chino2:
					case VehicleHash.Moonbeam2:
					case VehicleHash.Primo2:
					case VehicleHash.Voodoo2:
					case VehicleHash.SabreGT2:
					case VehicleHash.Tornado5:
					case VehicleHash.Virgo2:
					case VehicleHash.Minivan2:
					case VehicleHash.SlamVan3:
					case VehicleHash.Faction3:
						res.AddRange(new VehicleWheelType[] { VehicleWheelType.BennysOriginals, VehicleWheelType.BennysBespoke });
						break;
					case VehicleHash.SultanRS:
					case VehicleHash.Banshee2:
						res.Add(VehicleWheelType.BennysOriginals);
						break;
				}
				return res.ToArray();
			}
		}

		public string LocalizedWheelTypeName => GetLocalizedWheelTypeName(WheelType);

		public string GetLocalizedWheelTypeName(VehicleWheelType wheelType)
		{
			if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
			{
				Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
				Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
			}

			if (!_wheelNames.ContainsKey(wheelType))
			{
				throw new ArgumentException("Wheel Type is undefined", nameof(wheelType));
			}

			if (!string.IsNullOrEmpty(Game.GetLocalizedString(_wheelNames[wheelType].Item1)))
			{
				return Game.GetLocalizedString(_wheelNames[wheelType].Item1);
			}
			return _wheelNames[wheelType].Item2;
		}

		public void InstallModKit()
		{
			Function.Call(Hash.SET_VEHICLE_MOD_KIT, _owner.Handle, 0);
		}

		public bool RequestAdditionTextFile(int timeout = 1000)
		{
			if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
			{
				Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
				Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
				int end = Game.GameTime + timeout;
				{
					while (Game.GameTime < end)
					{
						if (Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
						{
							return true;
						}

						Script.Yield();
					}
					return false;
				}
			}
			return true;
		}

		public int Livery
		{
			get
			{
				if (this[VehicleModType.Livery].Count > 0)
				{
					return this[VehicleModType.Livery].Index;
				}
				else
				{
					return Function.Call<int>(Hash.GET_VEHICLE_LIVERY, _owner.Handle);
				}
			}
			set
			{
				if (this[VehicleModType.Livery].Count > 0)
				{
					this[VehicleModType.Livery].Index = value;
				}
				else
				{
					Function.Call(Hash.SET_VEHICLE_LIVERY, _owner.Handle, value);
				}
			}
		}
		public int LiveryCount
		{
			get
			{
				int modCount = this[VehicleModType.Livery].Count;

				if (modCount > 0)
				{
					return modCount;
				}

				return Function.Call<int>(Hash.GET_VEHICLE_LIVERY_COUNT, _owner.Handle);
			}
		}

		public string LocalizedLiveryName
		{
			get
			{
				int modCount = this[VehicleModType.Livery].Count;

				if (modCount > 0)
				{
					return this[VehicleModType.Livery].LocalizedName;
				}
				return Game.GetLocalizedString(Function.Call<string>(Hash.GET_LIVERY_NAME, _owner.Handle, Livery));
			}
		}

		public VehicleWindowTint WindowTint
		{
			get => Function.Call<VehicleWindowTint>(Hash.GET_VEHICLE_WINDOW_TINT, _owner.Handle);
			set => Function.Call(Hash.SET_VEHICLE_WINDOW_TINT, _owner.Handle, (int)value);
		}

		public VehicleColor PrimaryColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, _owner.Handle, &color1, &color2);
				}

				return (VehicleColor)color1;
			}
			set => Function.Call(Hash.SET_VEHICLE_COLOURS, _owner.Handle, (int)value, (int)SecondaryColor);
		}
		public VehicleColor SecondaryColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_COLOURS, _owner.Handle, &color1, &color2);
				}

				return (VehicleColor)color2;
			}
			set => Function.Call(Hash.SET_VEHICLE_COLOURS, _owner.Handle, (int)PrimaryColor, (int)value);
		}

		public VehicleColor RimColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, _owner.Handle, &color1, &color2);
				}

				return (VehicleColor)color2;
			}
			set => Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, _owner.Handle, (int)PearlescentColor, (int)value);
		}
		public VehicleColor PearlescentColor
		{
			get
			{
				int color1, color2;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOURS, _owner.Handle, &color1, &color2);
				}

				return (VehicleColor)color1;
			}
			set => Function.Call(Hash.SET_VEHICLE_EXTRA_COLOURS, _owner.Handle, (int)value, (int)RimColor);
		}
		public VehicleColor TrimColor
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_505_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_505_2_Steam), nameof(VehicleModCollection), nameof(TrimColor));
				}

				int color;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOUR_5, _owner.Handle, &color);
				}

				return (VehicleColor)color;
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_505_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_505_2_Steam), nameof(VehicleModCollection), nameof(TrimColor));
				}

				Function.Call(Hash.SET_VEHICLE_EXTRA_COLOUR_5, _owner.Handle, (int)value);
			}
		}
		public VehicleColor DashboardColor
		{
			get
			{
				if (Game.Version < GameVersion.v1_0_505_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_505_2_Steam), nameof(VehicleModCollection), nameof(DashboardColor));
				}

				int color;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_EXTRA_COLOUR_6, _owner.Handle, &color);
				}

				return (VehicleColor)color;
			}
			set
			{
				if (Game.Version < GameVersion.v1_0_505_2_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_505_2_Steam), nameof(VehicleModCollection), nameof(DashboardColor));
				}

				Function.Call(Hash.SET_VEHICLE_EXTRA_COLOUR_6, _owner.Handle, (int)value);
			}
		}

		public int ColorCombination
		{
			get => Function.Call<int>(Hash.GET_VEHICLE_COLOUR_COMBINATION, _owner.Handle);
			set => Function.Call(Hash.SET_VEHICLE_COLOUR_COMBINATION, _owner.Handle, value);
		}

		public int ColorCombinationCount => Function.Call<int>(Hash.GET_NUMBER_OF_VEHICLE_COLOURS, _owner.Handle);

		public Color TireSmokeColor
		{
			get
			{
				int red, green, blue;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_TYRE_SMOKE_COLOR, _owner.Handle, &red, &green, &blue);
				}

				return Color.FromArgb(red, green, blue);
			}
			set => Function.Call(Hash.SET_VEHICLE_TYRE_SMOKE_COLOR, _owner.Handle, value.R, value.G, value.B);
		}
		public Color NeonLightsColor
		{
			get
			{
				int red, green, blue;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_NEON_COLOUR, _owner.Handle, &red, &green, &blue);
				}

				return Color.FromArgb(red, green, blue);
			}
			set => Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, _owner.Handle, value.R, value.G, value.B);
		}

		public bool HasNeonLight(VehicleNeonLight neonLight)
		{
			switch (neonLight)
			{
				case VehicleNeonLight.Left:
					return _owner.Bones.Contains("neon_l");
				case VehicleNeonLight.Right:
					return _owner.Bones.Contains("neon_r");
				case VehicleNeonLight.Front:
					return _owner.Bones.Contains("neon_f");
				case VehicleNeonLight.Back:
					return _owner.Bones.Contains("neon_b");
				default:
					return false;
			}
		}

		public bool HasNeonLights => Enum.GetValues(typeof(VehicleNeonLight)).Cast<VehicleNeonLight>().Any(HasNeonLight);

		public bool IsNeonLightsOn(VehicleNeonLight light)
		{
			return Function.Call<bool>(Hash.GET_VEHICLE_NEON_ENABLED, _owner.Handle, (int)light);
		}
		public void SetNeonLightsOn(VehicleNeonLight light, bool on)
		{
			Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, _owner.Handle, (int)light, on);
		}

		public Color CustomPrimaryColor
		{
			get
			{
				int red, green, blue;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_CUSTOM_PRIMARY_COLOUR, _owner.Handle, &red, &green, &blue);
				}

				return Color.FromArgb(red, green, blue);

			}
			set => Function.Call(Hash.SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, _owner.Handle, value.R, value.G, value.B);
		}
		public Color CustomSecondaryColor
		{
			get
			{
				int red, green, blue;
				unsafe
				{
					Function.Call(Hash.GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, _owner.Handle, &red, &green, &blue);
				}

				return Color.FromArgb(red, green, blue);
			}
			set => Function.Call(Hash.SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, _owner.Handle, value.R, value.G, value.B);
		}

		public bool IsPrimaryColorCustom => Function.Call<bool>(Hash.GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM, _owner.Handle);
		public bool IsSecondaryColorCustom => Function.Call<bool>(Hash.GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM, _owner.Handle);

		public void ClearCustomPrimaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, _owner.Handle);
		}
		public void ClearCustomSecondaryColor()
		{
			Function.Call(Hash.CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, _owner.Handle);
		}

		public string LicensePlate
		{
			get => Function.Call<string>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT, _owner.Handle);
			set => Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, _owner.Handle, value);
		}

		public LicensePlateType LicensePlateType => Function.Call<LicensePlateType>(Hash.GET_VEHICLE_PLATE_TYPE, _owner.Handle);

		public LicensePlateStyle LicensePlateStyle
		{
			get => Function.Call<LicensePlateStyle>(Hash.GET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, _owner.Handle);
			set => Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, _owner.Handle, (int)value);
		}
	}
}
