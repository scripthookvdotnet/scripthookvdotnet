//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Graphics;
using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Random = System.Random;

namespace GTA
{
	public static class World
	{
		#region Fields
		static readonly string[] s_weatherNames = {
			"EXTRASUNNY",
			"CLEAR",
			"CLOUDS",
			"SMOG",
			"FOGGY",
			"OVERCAST",
			"RAIN",
			"THUNDER",
			"CLEARING",
			"NEUTRAL",
			"SNOW",
			"BLIZZARD",
			"SNOWLIGHT",
			"XMAS",
			"HALLOWEEN"
		};

		static readonly GregorianCalendar s_calendar = new();

		// removes gang and animal ped models just like CREATE_RANDOM_PED does
		static readonly Func<Model, bool> s_defaultPredicateForCreateRandomPed = (x => x.IsHumanPed && !x.IsGangPed);
		#endregion

		#region Time & Day

		/// <inheritdoc cref="Clock.IsPaused"/>
		public static bool IsClockPaused
		{
			get => SHVDN.NativeMemory.IsClockPaused;
			set => Function.Call(Hash.PAUSE_CLOCK, value);
		}

		/// <summary>
		/// Pauses or resumes the in-game clock.
		/// </summary>
		/// <param name="value">Pauses the game clock if set to <see langword="true" />; otherwise, resumes the game clock.</param>
		[Obsolete("The World.PauseClock is obsolete, use World.IsClockPaused instead.")]
		public static void PauseClock(bool value)
		{
			IsClockPaused = value;
		}

		/// <summary>
		/// Gets or sets the current date and time in the GTA world.
		/// </summary>
		/// <value>
		/// The current date and time.
		/// </value>
		[Obsolete("World.CurrentDate is obsolete because DateTime can represent the years only in the range of 1 to 9999, while the game supports wider range of years." +
			"Use properties or methods of GTA.Clock instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static DateTime CurrentDate
		{
			get
			{
				int year = Function.Call<int>(Hash.GET_CLOCK_YEAR);
				int month = Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1;
				int day = System.Math.Min(Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH), s_calendar.GetDaysInMonth(year, month));
				int hour = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minute = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int second = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new DateTime(year, month, day, hour, minute, second);
			}
			set
			{
				Function.Call(Hash.SET_CLOCK_DATE, value.Day, value.Month - 1, value.Year);
				Function.Call(Hash.SET_CLOCK_TIME, value.Hour, value.Minute, value.Second);
			}
		}

		/// <inheritdoc cref="Clock.TimeOfDay"/>
		public static TimeSpan CurrentTimeOfDay
		{
			get
			{
				int hours = Function.Call<int>(Hash.GET_CLOCK_HOURS);
				int minutes = Function.Call<int>(Hash.GET_CLOCK_MINUTES);
				int seconds = Function.Call<int>(Hash.GET_CLOCK_SECONDS);

				return new TimeSpan(hours, minutes, seconds);
			}
			set => Function.Call(Hash.SET_CLOCK_TIME, value.Hours, value.Minutes, value.Seconds);
		}

		/// <inheritdoc cref="Clock.MillisecondsPerGameMinute"/>
		public static int MillisecondsPerGameMinute
		{
			get => Function.Call<int>(Hash.GET_MILLISECONDS_PER_GAME_MINUTE);
			set => SHVDN.NativeMemory.MillisecondsPerGameMinute = value;
		}

		#endregion

		#region Weather & Effects

		/// <summary>
		/// Sets a value indicating whether artificial lights in the <see cref="World"/> should be rendered.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if blackout; otherwise, <see langword="false" />.
		/// </value>
		public static bool Blackout
		{
			set => Function.Call(Hash.SET_ARTIFICIAL_LIGHTS_STATE, value);
		}

		/// <summary>
		/// Gets or sets the previous weather.
		/// </summary>
		/// <value>
		/// The previous weather.
		/// </value>
		public static Weather Weather
		{
			get
			{
				for (int i = 0; i < s_weatherNames.Length; i++)
				{
					if (Function.Call<int>(Hash.GET_PREV_WEATHER_TYPE_HASH_NAME) == Game.GenerateHash(s_weatherNames[i]))
					{
						return (Weather)i;
					}
				}

				return Weather.Unknown;
			}
			set
			{
				if (Enum.IsDefined(typeof(Weather), value) && value != Weather.Unknown)
				{
					Function.Call(Hash.SET_WEATHER_TYPE_NOW, s_weatherNames[(int)value]);
				}
			}
		}
		/// <summary>
		/// Gets or sets the next weather.
		/// </summary>
		/// <value>
		/// The next weather.
		/// </value>
		public static Weather NextWeather
		{
			get
			{
				for (int i = 0; i < s_weatherNames.Length; i++)
				{
					if (Function.Call<bool>(Hash.IS_NEXT_WEATHER_TYPE, s_weatherNames[i]))
					{
						return (Weather)i;
					}
				}

				return Weather.Unknown;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Weather), value) || value == Weather.Unknown)
				{
					return;
				}

				int currentWeatherHash, nextWeatherHash;
				float weatherTransition;
				unsafe
				{
					Function.Call(Hash.GET_CURR_WEATHER_STATE, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
				}
				Function.Call(Hash.SET_CURR_WEATHER_STATE, currentWeatherHash, Game.GenerateHash(s_weatherNames[(int)value]), 0.0f);
			}
		}

		/// <summary>
		/// Transitions to weather.
		/// </summary>
		/// <param name="weather">The weather.</param>
		/// <param name="duration">The duration.</param>
		public static void TransitionToWeather(Weather weather, float duration)
		{
			if (Enum.IsDefined(typeof(Weather), weather) && weather != Weather.Unknown)
			{
				Function.Call(Hash.SET_WEATHER_TYPE_OVERTIME_PERSIST, s_weatherNames[(int)weather], duration);
			}
		}

		/// <summary>
		/// Transitions the weather to a random state.
		/// </summary>
		public static void SetRandomWeather() => Function.Call(Hash.SET_RANDOM_WEATHER_TYPE);

		/// <summary>
		/// Sets the gravity level for all <see cref="World"/> objects.
		/// </summary>
		/// <value>
		/// The gravity level:
		/// 9.8f - Default gravity.
		/// 2.4f - Moon gravity.
		/// 0.1f - Very low gravity.
		/// 0.0f - No gravity.
		/// </value>
		public static float GravityLevel
		{
			get => SHVDN.NativeMemory.WorldGravity;
			set
			{
				// Write the value you want to the first item in the array where the native reads the gravity level choices from
				SHVDN.NativeMemory.WorldGravity = value;
				// Call set_gravity_level normally using 0 as gravity type
				// The native will then set the gravity level to what we just wrote
				Function.Call(Hash.SET_GRAVITY_LEVEL, 0);
				// Reset the array item back to 9.8 so as to restore behavior of the native
				SHVDN.NativeMemory.WorldGravity = 9.800000f;
			}
		}

		/// <summary>
		/// Forces a flash of lightning and its accompanying thunder to occur at a random location.
		/// </summary>
		public static void ForceLightningFlash() => Function.Call(Hash.FORCE_LIGHTNING_FLASH);

		/// <summary>
		/// Gets the rain level.
		/// </summary>
		/// <value>
		/// The rain level.
		/// </value>
		public static float RainLevel => Function.Call<float>(Hash.GET_RAIN_LEVEL);

		/// <summary>
		/// Sets the rain level.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Level higher than <c>'0.5f'</c>, only the creation of puddles gets faster, rain and rain sound won't increase after that.
		/// </para>
		/// <para>
		/// Level of <c>'0.0f'</c> rain and rain sounds are disabled and there won't be any new puddles.
		/// </para>
		/// <para>
		/// To use the rain level of the current weather, set this to <c>'-1f'</c>.
		/// </para>
		/// </remarks>
		/// <value>
		/// The rain level.
		/// </value>
		public static float RainLevelOverride
		{
			set => Function.Call(Hash.SET_RAIN, value);
		}

		/// <summary>
		/// Gets the snow level.
		/// </summary>
		/// <value>
		/// The snow level.
		/// </value>
		public static float SnowLevel => Function.Call<float>(Hash.GET_SNOW_LEVEL);

		/// <summary>
		/// Sets the snow level.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Set level to <c>'0f'</c> disables snow effects.
		/// </para>
		/// <para>
		/// Set level to <c>'-1f'</c> the snow effects are set to the current weather.
		/// </para>
		/// </remarks>
		/// <value>
		/// The snow level.
		/// </value>
		public static float SnowLevelOverride
		{
			set => Function.Call(Hash.SET_SNOW, value);
		}
		#endregion

		#region Wind

		/// <summary>
		/// Gets the current wind speed in m/s. The value is between 0 to 12.
		/// </summary>
		public static float WindSpeed => Function.Call<float>(Hash.GET_WIND_SPEED);

		/// <summary>
		/// Sets the wind speed override by percentage, where 1.0 sets the current wind speed to 12.0 in m/s.
		/// Sets an negative value to stop using the override and let the game calculates the current wind speed as usual.
		/// </summary>
		/// <remarks>
		/// Although this property does not clamp the override value at all, the game clamps the wind speed between
		/// 0 and 12.0.
		/// </remarks>
		public static float WindSpeedOverride
		{
			set => Function.Call(Hash.SET_WIND, value);
		}

		/// <summary>
		/// Sets the wind speed override by speed. The value divided by 12 will be set if the result is lower than 1.0.
		/// Otherwise, the value will be set to 1.0.
		/// Sets an negative value to stop using the override and let the game calculates the current wind speed as usual.
		/// </summary>
		/// <remarks>
		/// Basically do the same as <see cref="set_WindSpeedOverride"/> does but with a upper bound and one division.
		/// Use <see cref="set_WindSpeedOverride"/> to set the value precisely (and avoid one division for performance).
		/// </remarks>
		public static void SetWindSpeedOverrideBySpeed(float windSpeed)
			=> Function.Call(Hash.SET_WIND_SPEED, windSpeed);

		/// <summary>
		/// Gets the current wind direction with a unit vector.
		/// </summary>
		public static Vector3 WindDirection => Function.Call<Vector3>(Hash.GET_WIND_DIRECTION);

		#endregion

		#region Blips

		/// <summary>
		/// Gets the waypoint blip.
		/// </summary>
		/// <returns>The <see cref="Vector3"/> coordinates of the Waypoint <see cref="Blip"/></returns>
		/// <remarks>
		/// Returns <see langword="null" /> if a waypoint <see cref="Blip"/> hasn't been set
		/// </remarks>
		public static Blip WaypointBlip
		{
			get
			{
				int handle = SHVDN.NativeMemory.GetWaypointBlip();

				return handle != 0 ? new Blip(handle) : null;
			}
		}

		/// <summary>
		/// Removes the waypoint.
		/// </summary>
		public static void RemoveWaypoint()
		{
			Function.Call(Hash.SET_WAYPOINT_OFF);
		}

		/// <summary>
		/// Gets or sets the waypoint position.
		/// </summary>
		/// <returns>The <see cref="Vector3"/> coordinates of the Waypoint <see cref="Blip"/></returns>
		/// <remarks>
		/// Returns an empty <see cref="Vector3"/> if a waypoint <see cref="Blip"/> hasn't been set
		/// If the game engine cant extract height information the Z component will be 0.0f
		/// </remarks>
		public static Vector3 WaypointPosition
		{
			get
			{
				Blip waypointBlip = WaypointBlip;
				if (waypointBlip == null)
				{
					return Vector3.Zero;
				}

				Vector3 position = waypointBlip.Position;
				GetGroundHeight(new Vector3(position.X, position.Y, 1000f), out float groundHeight);
				position.Z = groundHeight; // will be zero if test failed since values will be initialized with zero by default in C#
				return position;
			}
			set => Function.Call(Hash.SET_NEW_WAYPOINT, value.X, value.Y);
		}

		/// <summary>
		/// Gets an <c>array</c> of all the <see cref="Blip"/>s on the map with a given <see cref="BlipSprite"/>.
		/// </summary>
		/// <param name="blipTypes">The blip types to include, leave blank to get all <see cref="Blip"/>s.</param>
		public static Blip[] GetAllBlips(params BlipSprite[] blipTypes)
		{
			int[] blipTypesInt = Array.ConvertAll(blipTypes, blipType => (int)blipType);
			return Array.ConvertAll(SHVDN.NativeMemory.GetNonCriticalRadarBlipHandles(blipTypesInt), handle => new Blip(handle));
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Blip"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Blip"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Blip"/>s.</param>
		/// <param name="blipTypes">The blip types to include, leave blank to get all <see cref="Blip"/>s.</param>
		public static Blip[] GetNearbyBlips(Vector3 position, float radius, params BlipSprite[] blipTypes)
		{
			int[] blipTypesInt = Array.ConvertAll(blipTypes, blipType => (int)blipType);
			return Array.ConvertAll(SHVDN.NativeMemory.GetNonCriticalRadarBlipHandles(position.ToInternalFVector3(), radius, blipTypesInt), handle => new Blip(handle));
		}

		/// <summary>
		/// Creates a <see cref="Blip"/> at the given position on the map.
		/// </summary>
		/// <param name="position">The position of the blip on the map.</param>
		public static Blip CreateBlip(Vector3 position)
		{
			return new Blip(Function.Call<int>(Hash.ADD_BLIP_FOR_COORD, position.X, position.Y, position.Z));
		}
		/// <summary>
		/// Creates a <see cref="Blip"/> for a circular area at the given position on the map.
		/// </summary>
		/// <param name="position">The position of the blip on the map.</param>
		/// <param name="radius">The radius of the area on the map.</param>
		public static Blip CreateBlip(Vector3 position, float radius)
		{
			return new Blip(Function.Call<int>(Hash.ADD_BLIP_FOR_RADIUS, position.X, position.Y, position.Z, radius));
		}

		#endregion

		#region Entities

		/// <summary>
		/// A fast way to get the total number of <see cref="Vehicle"/>s spawned in the world.
		/// </summary>
		public static int VehicleCount => SHVDN.NativeMemory.GetVehicleCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="Ped"/>s spawned in the world.
		/// </summary>
		public static int PedCount => SHVDN.NativeMemory.GetPedCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="Prop"/>s spawned in the world.
		/// </summary>
		public static int PropCount => SHVDN.NativeMemory.GetObjectCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="Prop"/>s in the world associated with a <see cref="Pickup"/>.
		/// </summary>
		public static int PickupObjectCount => SHVDN.NativeMemory.GetPickupObjectCount();

		/// <summary>
		/// A fast way to get the total number of <see cref="Building"/>s spawned in the world.
		/// </summary>
		public static int BuildingCount => SHVDN.NativeMemory.GetBuildingCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="AnimatedBuilding"/>s spawned in the world.
		/// </summary>
		public static int AnimatedBuildingCount => SHVDN.NativeMemory.GetAnimatedBuildingCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="InteriorInstance"/>s spawned in the world.
		/// </summary>
		public static int InteriorInstanceCount => SHVDN.NativeMemory.GetInteriorInstCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="InteriorProxy"/>s managed in the <see cref="InteriorProxy"/> pool.
		/// </summary>
		public static int InteriorProxyCount => SHVDN.NativeMemory.GetInteriorProxyCount();

		/// <summary>
		/// A fast way to get the total number of <see cref="Projectile"/>s spawned in the world.
		/// </summary>
		public static int ProjectileCount => SHVDN.NativeMemory.GetProjectileCount();

		/// <summary>
		/// Returns the total number of <see cref="Entity"/> colliders used.
		/// </summary>
		public static int EntityColliderCount => SHVDN.NativeMemory.GetEntityColliderCount();

		/// <summary>
		/// The total number of <see cref="Vehicle"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Vehicle"/> is the same as this limit and the game tries to create a <see cref="Vehicle"/>.</remarks>
		public static int VehicleCapacity => SHVDN.NativeMemory.GetVehicleCapacity();
		/// <summary>
		/// The total number of <see cref="Ped"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Ped"/> is the same as this limit and the game tries to create a <see cref="Ped"/>.</remarks>
		public static int PedCapacity => SHVDN.NativeMemory.GetPedCapacity();
		/// <summary>
		/// The total number of <see cref="Prop"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Prop"/> is the same as this limit and the game tries to create a <see cref="Prop"/>.</remarks>
		public static int PropCapacity => SHVDN.NativeMemory.GetObjectCapacity();
		/// <summary>
		/// The total number of <see cref="Prop"/>s in the world associated with a <see cref="Pickup"/> that can exist in the world.
		/// </summary>
		public static int PickupObjectCapacity => SHVDN.NativeMemory.GetPickupObjectCapacity();
		/// <summary>
		/// The total number of <see cref="Projectile"/>s that can exist in the world.
		/// Always returns 50 currently since the limit is hard-coded in the exe.
		/// </summary>
		public static int ProjectileCapacity => SHVDN.NativeMemory.GetProjectileCapacity();
		/// <summary>
		/// The total number of <see cref="Building"/>s that can exist in the world.
		/// </summary>
		public static int BuildingCapacity => SHVDN.NativeMemory.GetBuildingCapacity();
		/// <summary>
		/// The total number of <see cref="AnimatedBuilding"/>s that can exist in the world.
		/// </summary>
		public static int AnimatedBuildingCapacity => SHVDN.NativeMemory.GetAnimatedBuildingCapacity();
		/// <summary>
		/// The total number of <see cref="InteriorInstance"/>s that can exist in the world.
		/// </summary>
		public static int InteriorInstanceCapacity => SHVDN.NativeMemory.GetInteriorInstCapacity();
		/// <summary>
		/// The total number of <see cref="InteriorProxy"/>s the game can manage at the same time in the <see cref="InteriorProxy"/> pool.
		/// </summary>
		public static int InteriorProxyCapacity => SHVDN.NativeMemory.GetInteriorProxyCapacity();
		/// <summary>
		/// <para>The total number of <see cref="Entity"/> colliders can be used. The return value can be different in different versions.</para>
		/// <para>When <see cref="EntityColliderCount"/> reaches this value, no more <see cref="Entity"/> will not be able to be physically moved
		/// and <see cref="Vehicle"/>s and <see cref="Prop"/>s will not be able to detach fragment parts properly.</para>
		/// </summary>
		public static int EntityColliderCapacity => SHVDN.NativeMemory.GetEntityColliderCapacity();

		/// <summary>
		/// Gets the closest <see cref="Ped"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Ped"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Ped"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Ped"/>s to get, leave blank for all <see cref="Ped"/> <see cref="Model"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Ped"/> was in the given region.</remarks>
		public static Ped GetClosestPed(Vector3 position, float radius, params Model[] models)
		{
			return GetClosest(position, GetNearbyPeds(position, radius, models));
		}

		/// <summary>
		/// Gets an <c>array</c>of all <see cref="Ped"/>s in the World.
		/// </summary>
		/// <param name="models">The <see cref="Model"/> of <see cref="Ped"/>s to get, leave blank for all <see cref="Ped"/> <see cref="Model"/>s.</param>
		public static Ped[] GetAllPeds(params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(hashes), handle => new Ped(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Ped"/>s near a given <see cref="Ped"/> in the world
		/// </summary>
		/// <param name="ped">The ped to check.</param>
		/// <param name="radius">The maximum distance from the <paramref name="ped"/> to detect <see cref="Ped"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Ped"/>s to get, leave blank for all <see cref="Ped"/> <see cref="Model"/>s.</param>
		/// <remarks>Doesnt include the <paramref name="ped"/> in the result</remarks>
		public static Ped[] GetNearbyPeds(Ped ped, float radius, params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			int[] handles = SHVDN.NativeMemory.GetPedHandles(ped.Position.ToInternalFVector3(), radius, hashes);

			if (handles.Length == 0)
			{
				return Array.Empty<Ped>();
			}

			var result = new List<Ped>(handles.Length - 1);

			foreach (int handle in handles)
			{
				if (handle == ped.Handle)
				{
					continue;
				}

				result.Add(new Ped(handle));
			}

			return result.ToArray();
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Ped"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Ped"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Ped"/>s to get, leave blank for all <see cref="Ped"/> <see cref="Model"/>s.</param>
		public static Ped[] GetNearbyPeds(Vector3 position, float radius, params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(position.ToInternalFVector3(), radius, hashes), handle => new Ped(handle));
		}

		/// <summary>
		/// Gets the closest <see cref="Vehicle"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Vehicle"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Vehicle"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Vehicle"/>s to get, leave blank for all <see cref="Vehicle"/> <see cref="Model"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Vehicle"/> was in the given region.</remarks>
		public static Vehicle GetClosestVehicle(Vector3 position, float radius, params Model[] models)
		{
			return GetClosest(position, GetNearbyVehicles(position, radius, models));
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s in the World.
		/// </summary>
		/// <param name="models">The <see cref="Model"/> of <see cref="Vehicle"/>s to get, leave blank for all <see cref="Vehicle"/> <see cref="Model"/>s.</param>
		public static Vehicle[] GetAllVehicles(params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(hashes), handle => new Vehicle(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s near a given <see cref="Ped"/> in the world
		/// </summary>
		/// <param name="ped">The ped to check.</param>
		/// <param name="radius">The maximum distance from the <paramref name="ped"/> to detect <see cref="Vehicle"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Vehicle"/>s to get, leave blank for all <see cref="Vehicle"/> <see cref="Model"/>s.</param>
		/// <remarks>Doesnt include the <see cref="Vehicle"/> the <paramref name="ped"/> is using in the result</remarks>
		public static Vehicle[] GetNearbyVehicles(Ped ped, float radius, params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			int[] handles = SHVDN.NativeMemory.GetVehicleHandles(ped.Position.ToInternalFVector3(), radius, hashes);

			var result = new List<Vehicle>();
			Vehicle ignore = ped.CurrentVehicle;

			foreach (int handle in handles)
			{
				if (ignore != null && handle == ignore.Handle)
				{
					continue;
				}

				result.Add(new Vehicle(handle));
			}

			return result.ToArray();
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Vehicle"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Vehicle"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Vehicle"/>s to get, leave blank for all <see cref="Vehicle"/> <see cref="Model"/>s.</param>
		public static Vehicle[] GetNearbyVehicles(Vector3 position, float radius, params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(position.ToInternalFVector3(), radius, hashes), handle => new Vehicle(handle));
		}

		/// <summary>
		/// Gets the closest <see cref="Prop"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Prop"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Prop"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Prop"/>s to get, leave blank for all <see cref="Prop"/> <see cref="Model"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Prop"/> was in the given region.</remarks>
		public static Prop GetClosestProp(Vector3 position, float radius, params Model[] models)
		{
			return GetClosest(position, GetNearbyProps(position, radius, models));
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in the World.
		/// </summary>
		/// <param name="models">The <see cref="Model"/> of <see cref="Prop"/>s to get, leave blank for all <see cref="Prop"/> <see cref="Model"/>s.</param>
		public static Prop[] GetAllProps(params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(hashes), handle => new Prop(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Prop"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Prop"/>s.</param>
		/// <param name="models">The <see cref="Model"/> of <see cref="Prop"/>s to get, leave blank for all <see cref="Prop"/> <see cref="Model"/>s.</param>
		public static Prop[] GetNearbyProps(Vector3 position, float radius, params Model[] models)
		{
			int[] hashes = Array.ConvertAll(models, model => model.Hash);
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(position.ToInternalFVector3(), radius, hashes), handle => new Prop(handle));
		}

		/// <summary>
		/// Gets the closest <see cref="PickupObject"/> to a given position in the World associated with a <see cref="Pickup"/>.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="PickupObject"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="PickupObject"/>s.</param>
		/// <remarks>
		/// <para>
		/// Returns <see langword="null" /> if no <see cref="PickupObject"/> was in the given region.
		/// </para>
		/// <para>
		/// Although this method returns a <see cref="PickupObject"/> instance, it specifies the base class
		/// <see cref="Prop"/> type as the return type for compatibility for scripts built against v3.6.0 or earlier
		/// versions of SHVDN. If you need to use the return value as <see cref="PickupObject"/>, you can cast it
		/// into <see cref="PickupObject"/>.
		/// </para>
		/// </remarks>
		public static Prop GetClosestPickupObject(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyPickupObjects(position, radius));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="PickupObject"/>s in the World associated with a <see cref="Pickup"/>.
		/// </summary>
		/// <remarks>
		/// Although this method returns an array of <see cref="PickupObject"/> instances, it specifies an array of
		/// the base class <see cref="Prop"/> type as the return type for compatibility for scripts built against
		/// v3.6.0 or earlier versions of SHVDN. If you need to use the return value as an array of
		/// <see cref="PickupObject"/>, you can cast it into an array of <see cref="PickupObject"/>.
		/// </remarks>
		public static Prop[] GetAllPickupObjects()
		{
			return Array.ConvertAll<int, Prop>(SHVDN.NativeMemory.GetPickupObjectHandles(), handle => new PickupObject(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="PickupObject"/>s in a given region in the World associated with a <see cref="Pickup"/>.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Entity"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Prop"/>s.</param>
		/// <remarks>
		/// Although this method returns an array of <see cref="PickupObject"/> instances, it specifies an array of
		/// the base class <see cref="Prop"/> type as the return type for compatibility for scripts built against
		/// v3.6.0 or earlier versions of SHVDN. If you need to use the return value as an array of
		/// <see cref="PickupObject"/>, you can cast it into an array of <see cref="PickupObject"/>.
		/// </remarks>
		public static Prop[] GetNearbyPickupObjects(Vector3 position, float radius)
		{
			return Array.ConvertAll<int, Prop>(SHVDN.NativeMemory.GetPickupObjectHandles(position.ToInternalFVector3(), radius), handle => new PickupObject(handle));
		}
		/// <summary>
		/// Gets the closest <see cref="Projectile"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Projectile"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Projectile"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Projectile"/> was in the given region.</remarks>
		public static Projectile GetClosestProjectile(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyProjectiles(position, radius));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Projectile"/>s in the World.
		/// </summary>
		public static Projectile[] GetAllProjectiles()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetProjectileHandles(), handle => new Projectile(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Projectile"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Projectile"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Projectile"/>s.</param>
		public static Projectile[] GetNearbyProjectiles(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetProjectileHandles(position.ToInternalFVector3(), radius), handle => new Projectile(handle));
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Entity"/>s in the World.
		/// </summary>
		public static Entity[] GetAllEntities()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetEntityHandles(), Entity.FromHandle);
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Entity"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Entity"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Entity"/>s.</param>
		public static Entity[] GetNearbyEntities(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetEntityHandles(position.ToInternalFVector3(), radius), Entity.FromHandle);
		}

		public static Building[] GetAllBuildings()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetBuildingHandles(), Building.FromHandle);
		}
		public static Building[] GetNearbyBuildings(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetBuildingHandles(position.ToInternalFVector3(), radius), Building.FromHandle);
		}
		public static Building GetClosestBuilding(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyBuildings(position, radius));
		}

		public static AnimatedBuilding[] GetAllAnimatedBuildings()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetAnimatedBuildingHandles(), AnimatedBuilding.FromHandle);
		}
		public static AnimatedBuilding[] GetNearbyAnimatedBuildings(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetAnimatedBuildingHandles(position.ToInternalFVector3(), radius), AnimatedBuilding.FromHandle);
		}
		public static AnimatedBuilding GetClosestAnimatedBuilding(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyAnimatedBuildings(position, radius));
		}

		public static InteriorInstance[] GetAllInteriorInstances()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetInteriorInstHandles(), InteriorInstance.FromHandle);
		}
		public static InteriorInstance[] GetNearbyInteriorInstances(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetInteriorInstHandles(position.ToInternalFVector3(), radius), InteriorInstance.FromHandle);
		}
		public static InteriorInstance GetClosestInteriorInstance(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyInteriorInstances(position, radius));
		}

		public static InteriorProxy[] GetAllInteriorProxies()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetInteriorProxyHandles(), InteriorProxy.FromHandle);
		}
		public static InteriorProxy[] GetNearbyInteriorProxies(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetInteriorProxyHandles(position.ToInternalFVector3(), radius), InteriorProxy.FromHandle);
		}
		public static InteriorProxy GetClosestInteriorProxy(Vector3 position, float radius)
		{
			return GetClosest(position, GetNearbyInteriorProxies(position, radius));
		}

		/// <summary>
		/// Gets the closest <see cref="ISpatial"/> to a given position in the World.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="position">The position to check against.</param>
		/// <param name="spatials">The spatials to check.</param>
		/// <returns>The closest <see cref="ISpatial"/> to the <paramref name="position"/></returns>
		public static T GetClosest<T>(Vector3 position, params T[] spatials) where T : ISpatial
		{
			var closest = default(T);
			float closestDistance = 3e38f;

			foreach (T spatial in spatials)
			{
				float distance = position.DistanceToSquared(spatial.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = spatial;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="ISpatial"/> to a given position in the World ignoring height.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="position">The position to check against.</param>
		/// <param name="spatials">The spatials to check.</param>
		/// <returns>The closest <see cref="ISpatial"/> to the <paramref name="position"/></returns>
		public static T GetClosest<T>(Vector2 position, params T[] spatials) where T : ISpatial
		{
			var closest = default(T);
			float closestDistance = 3e38f;
			var position3D = new Vector3(position.X, position.Y, 0.0f);

			foreach (T spatial in spatials)
			{
				float distance = position3D.DistanceToSquared2D(spatial.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = spatial;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="Building"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="buildings">The buildings to check.</param>
		/// <returns>The closest <see cref="Building"/> to the <paramref name="position"/></returns>
		public static Building GetClosest(Vector3 position, params Building[] buildings)
		{
			Building closest = null;
			float closestDistance = 3e38f;

			foreach (Building building in buildings)
			{
				float distance = position.DistanceToSquared(building.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = building;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="Building"/> to a given position in the World ignoring height.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="buildings">The buildings to check.</param>
		/// <returns>The closest <see cref="Building"/> to the <paramref name="position"/></returns>
		public static Building GetClosest(Vector2 position, params Building[] buildings)
		{
			Building closest = null;
			float closestDistance = 3e38f;
			var position3D = new Vector3(position.X, position.Y, 0.0f);

			foreach (Building building in buildings)
			{
				float distance = position3D.DistanceToSquared2D(building.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = building;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="AnimatedBuilding"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="animatedBuildings">The animated building to check.</param>
		/// <returns>The closest <see cref="AnimatedBuilding"/> to the <paramref name="position"/></returns>
		public static AnimatedBuilding GetClosest(Vector3 position, params AnimatedBuilding[] animatedBuildings)
		{
			AnimatedBuilding closest = null;
			float closestDistance = 3e38f;

			foreach (AnimatedBuilding animatedBuilding in animatedBuildings)
			{
				float distance = position.DistanceToSquared(animatedBuilding.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = animatedBuilding;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="AnimatedBuilding"/> to a given position in the World ignoring height.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="animatedBuildings">The animated building to check.</param>
		/// <returns>The closest <see cref="AnimatedBuilding"/> to the <paramref name="position"/></returns>
		public static AnimatedBuilding GetClosest(Vector2 position, params AnimatedBuilding[] animatedBuildings)
		{
			AnimatedBuilding closest = null;
			float closestDistance = 3e38f;
			var position3D = new Vector3(position.X, position.Y, 0.0f);

			foreach (AnimatedBuilding animatedBuilding in animatedBuildings)
			{
				float distance = position3D.DistanceToSquared2D(animatedBuilding.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = animatedBuilding;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="InteriorInstance"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="interiorInstances">The spatials to check.</param>
		/// <returns>The closest <see cref="InteriorInstance"/> to the <paramref name="position"/></returns>
		public static InteriorInstance GetClosest(Vector3 position, params InteriorInstance[] interiorInstances)
		{
			InteriorInstance closest = null;
			float closestDistance = 3e38f;

			foreach (InteriorInstance interiorInstance in interiorInstances)
			{
				float distance = position.DistanceToSquared(interiorInstance.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = interiorInstance;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="InteriorInstance"/> to a given position in the World ignoring height.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="interiorInstances">The interior instances to check.</param>
		/// <returns>The closest <see cref="InteriorInstance"/> to the <paramref name="interiorInstances"/></returns>
		public static InteriorInstance GetClosest(Vector2 position, params InteriorInstance[] interiorInstances)
		{
			InteriorInstance closest = null;
			float closestDistance = 3e38f;
			var position3D = new Vector3(position.X, position.Y, 0.0f);

			foreach (InteriorInstance interiorInstance in interiorInstances)
			{
				float distance = position3D.DistanceToSquared2D(interiorInstance.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = interiorInstance;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="InteriorProxy"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="interiorProxies">The spatials to check.</param>
		/// <returns>The closest <see cref="InteriorProxy"/> to the <paramref name="position"/></returns>
		public static InteriorProxy GetClosest(Vector3 position, params InteriorProxy[] interiorProxies)
		{
			InteriorProxy closest = null;
			float closestDistance = 3e38f;

			foreach (InteriorProxy interiorProxy in interiorProxies)
			{
				float distance = position.DistanceToSquared(interiorProxy.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = interiorProxy;
				closestDistance = distance;
			}
			return closest;
		}
		/// <summary>
		/// Gets the closest <see cref="InteriorProxy"/> to a given position in the World ignoring height.
		/// </summary>
		/// <param name="position">The position to check against.</param>
		/// <param name="interiorProxies">The spatials to check.</param>
		/// <returns>The closest <see cref="InteriorProxy"/> to the <paramref name="position"/></returns>
		public static InteriorProxy GetClosest(Vector2 position, params InteriorProxy[] interiorProxies)
		{
			InteriorProxy closest = null;
			float closestDistance = 3e38f;
			var position3D = new Vector3(position.X, position.Y, 0.0f);

			foreach (InteriorProxy interiorProxy in interiorProxies)
			{
				float distance = position3D.DistanceToSquared2D(interiorProxy.Position);
				if (!(distance <= closestDistance))
				{
					continue;
				}

				closest = interiorProxy;
				closestDistance = distance;
			}
			return closest;
		}

		/// <summary>
		/// Sets the density of all ambient vehicles in the world.
		/// Must be called each frame to remain in effect.
		/// Values are 0.0f to 1.0f inclusive; any value outside this range will cause this function to do nothing.
		/// </summary>
		/// <param name="densityMult">The density multiplier, between 0 and 1.</param>
		public static void SetAmbientVehicleDensityMultiplierThisFrame(float densityMult)
		{
			Function.Call(Hash.SET_VEHICLE_DENSITY_MULTIPLIER_THIS_FRAME, densityMult);
		}

		/// <summary>
		/// Sets the density of ambient peds in the world.
		/// Must be called each frame to remain in effect.
		/// Values are 0.0f to 1.0f inclusive; any value outside this range will cause this function to do nothing.
		/// </summary>
		/// <param name="densityMult">The density multiplier, between 0 and 1.</param>
		public static void SetAmbientPedDensityMultiplierThisFrame(float densityMult) => Function.Call(Hash.SET_PED_DENSITY_MULTIPLIER_THIS_FRAME, densityMult);

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Ped"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Ped CreatePed(Model model, Vector3 position, float heading = 0f)
		{
			if (PedCount >= PedCapacity || !model.IsPed || !model.Request(1000))
			{
				return null;
			}

			// The first parameter "PedType" does not have actual effect, the function always eventually uses the "Pedtype"
			// value for the model (in peds.ymt or peds.meta) instead
			// Actually the value is read when the function is called but eventually overwritten before getting used in a meaningful way
			return new Ped(Function.Call<int>(Hash.CREATE_PED, 26, model.Hash, position.X, position.Y, position.Z, heading, false, false));
		}
		/// <summary>
		/// Spawns a <see cref="Ped"/> of a random <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		public static Ped CreateRandomPed(Vector3 position)
		{
			if (PedCount >= PedCapacity)
			{
				return null;
			}

			return new Ped(Function.Call<int>(Hash.CREATE_RANDOM_PED, position.X, position.Y, position.Z));
		}
		/// <summary>
		/// Spawns a <see cref="Ped"/> of a random <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <param name="predicate">
		/// The method that determines whether a model should be considered when choosing a random model for the <see cref="Ped"/>.
		/// If <see langword="null" /> is set, gangster and animal models will not be chosen, just like CREATE_PED does.
		/// </param>
		public static Ped CreateRandomPed(Vector3 position, float heading, Func<Model, bool> predicate = null)
		{
			if (PedCount >= PedCapacity)
			{
				return null;
			}

			IEnumerable<Model> loadedAppropriatePedModels = SHVDN.NativeMemory.GetLoadedAppropriatePedHashes().Select(x => new Model(x));
			Model[] filteredPedModels = predicate != null
				? loadedAppropriatePedModels.Where(predicate).ToArray()
				: loadedAppropriatePedModels.Where(s_defaultPredicateForCreateRandomPed).ToArray();
			int filteredModelCount = filteredPedModels.Length;
			if (filteredModelCount == 0)
			{
				return null;
			}

			Random rand = Math.Random.Instance;
			Model pickedModel = filteredPedModels.ElementAt(rand.Next(filteredModelCount));

			// the model should be loaded at this moment, so call CREATE_PED immediately
			var createdPed = new Ped(Function.Call<int>(Hash.CREATE_PED, 26, pickedModel, position.X, position.Y, position.Z, heading, false, false));

			// Randomize clothes and ped props just like CREATE_RANDOM_PED does
			Function.Call(Hash.SET_PED_RANDOM_COMPONENT_VARIATION, createdPed.Handle, 0);
			Function.Call(Hash.SET_PED_RANDOM_PROPS, createdPed.Handle);

			return createdPed;
		}

		/// <summary>
		/// Spawns a <see cref="Vehicle"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Vehicle"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Vehicle"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Vehicle"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Vehicle"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Vehicle CreateVehicle(Model model, Vector3 position, float heading = 0f)
		{
			if (VehicleCount >= VehicleCapacity || !model.IsVehicle || !model.Request(1000))
			{
				return null;
			}

			return new Vehicle(Function.Call<int>(Hash.CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false));
		}
		/// <summary>
		/// Spawns a <see cref="Vehicle"/> of a random <see cref="Model"/> at the position specified.
		/// </summary>
		/// <param name="position">The position to spawn the <see cref="Vehicle"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Vehicle"/>.</param>
		/// <param name="predicate">The method that determines whether a model should be considered when choosing a random model for the <see cref="Vehicle"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Vehicle"/> could not be spawned.</remarks>
		public static Vehicle CreateRandomVehicle(Vector3 position, float heading = 0f, Func<Model, bool> predicate = null)
		{
			if (VehicleCount >= VehicleCapacity)
			{
				return null;
			}

			IEnumerable<Model> loadedAppropriateVehModels = SHVDN.NativeMemory.GetLoadedAppropriateVehicleHashes().Select(x => new Model(x));
			Model[] filteredVehModels = predicate != null ? loadedAppropriateVehModels.Where(predicate).ToArray() : loadedAppropriateVehModels.ToArray();
			int filteredModelCount = filteredVehModels.Length;
			if (filteredModelCount == 0)
			{
				return null;
			}

			Random rand = Math.Random.Instance;
			Model pickedModel = filteredVehModels.ElementAt(rand.Next(filteredModelCount));

			// the model should be loaded at this moment, so call CREATE_VEHICLE immediately
			return new Vehicle(Function.Call<int>(Hash.CREATE_VEHICLE, pickedModel, position.X, position.Y, position.Z, heading, false, false));
		}

		/// <inheritdoc cref="CreateProp(Model, Vector3, Vector3, bool, bool)"/>
		public static Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround)
		{
			if (PropCount >= PropCapacity || !model.Request(1000))
			{
				return null;
			}

			if (placeOnGround)
			{
				GetGroundHeight(position, out float groundHeight);
				position.Z = groundHeight; // will be zero if the test failed since values will be initialized with zero by default in C#
			}

			return new Prop(Function.Call<int>(Hash.CREATE_OBJECT, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic));
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
		/// <param name="rotation">The rotation of the <see cref="Prop"/>.</param>
		/// <param name="dynamic">
		/// <para>
		/// If <see langword="true"/>, the <see cref="Prop"/> will always be forced to be an regular prop type (<c>CObject</c>). This applies when creating a <see cref="Prop"/> that uses a door <see cref="Model"/>.
		/// If this is <see langword="false"/>, the <see cref="Prop"/> will be created as a door type (<c>CDoor</c>) and it will work as a door.
		/// </para>
		/// <para>Although "dynamic" is an incorrectly named parameter, the name is retained for scripts that use the method with named parameters.</para>
		/// </param>
		/// <param name="placeOnGround">if set to <see langword="true" /> place the prop on the ground nearest to the <paramref name="position"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Prop"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Prop CreateProp(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround)
		{
			Prop prop = CreateProp(model, position, dynamic, placeOnGround);

			if (prop != null)
			{
				prop.Rotation = rotation;
			}

			return prop;
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position without any offset.
		/// </summary>
		/// <inheritdoc cref="CreateProp(Model, Vector3, Vector3, bool, bool)"/>
		public static Prop CreatePropNoOffset(Model model, Vector3 position, bool dynamic)
		{
			if (PropCount >= PropCapacity || !model.Request(1000))
			{
				return null;
			}

			return new Prop(Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic));
		}
		/// <summary>
		/// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position without any offset.
		/// </summary>
		/// <inheritdoc cref="CreateProp(Model, Vector3, Vector3, bool, bool)"/>
		public static Prop CreatePropNoOffset(Model model, Vector3 position, Vector3 rotation, bool dynamic)
		{
			Prop prop = CreatePropNoOffset(model, position, dynamic);

			if (prop != null)
			{
				prop.Rotation = rotation;
			}

			return prop;
		}

		/// <summary>
		/// Creates a pickup <see cref="Prop"/> similar to those dropped by dead <see cref="Ped"/>s.
		/// These types of pickups are part of the ambient population and will get removed if the player moves too far away from them.
		/// </summary>
		/// <param name="type">The pickup type hash.</param>
		/// <param name="position">The pickup position to place in world space.</param>
		/// <param name="placementFlags">The pickup placement flags.</param>
		/// <param name="amount">
		/// A variable amount that can be specified for some pickups, such as money or ammo.
		/// Leave this parameter as <c>-1</c> to apply the default amount.
		/// </param>
		/// <param name="customModel">
		/// If set to non-zero value, this model will be used for the pickup instead of the default one.
		/// </param>
		/// <param name="createAsScriptObject">
		/// If <see langword="true"/>, the pickup will be treated as a script object and persist until the SHVDN runtime terminates, or the pickup <see cref="Prop"/> is marked as no longer needed.
		/// </param>
		public static Prop CreateAmbientPickup(
			PickupType type,
			Vector3 position,
			PickupPlacementFlags placementFlags = PickupPlacementFlags.None,
			int amount = -1,
			Model customModel = default,
			bool createAsScriptObject = false)
		{
			if (customModel.Hash != 0 && !customModel.Request(1000))
			{
				return null;
			}

			int handle = Function.Call<int>(Hash.CREATE_AMBIENT_PICKUP,
				(int)type,
				position.X,
				position.Y,
				position.Z,
				(int)placementFlags,
				amount,
				customModel.Hash,
				createAsScriptObject,
				true);

			return handle == 0 ? null : new Prop(handle);
		}

		/// <summary>
		/// Creates a pickup <see cref="Prop"/> similar to those dropped by dead <see cref="Ped"/>s.
		/// These types of pickups are part of the ambient population and will get removed if the player moves too far away from them.
		/// </summary>
		[Obsolete("The World.CreateAmbientPickup overload with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
			"Use World.CreateAmbientPickup(PickupType, Vector3, PickupPlacementFlags, int, Model, bool) instead.")]
		public static Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value)
			=> CreateAmbientPickup(type, position, PickupPlacementFlags.None, value, model, false);

		/// <inheritdoc cref="CreatePickup(PickupType, Vector3, Vector3, PickupPlacementFlags, int, EulerRotationOrder, Model)"/>
		public static Pickup CreatePickup(
			PickupType type,
			Vector3 position,
			PickupPlacementFlags placementFlags = PickupPlacementFlags.None,
			int amount = -1,
			Model customModel = default)
		{
			if (customModel.Hash != 0 && !customModel.Request(1000))
			{
				return null;
			}

			// The 2nd last argument is named ScriptHostObject, so just set to true as most SP scripts do
			int handle = Function.Call<int>(Hash.CREATE_PICKUP,
				(int)type,
				position.X,
				position.Y,
				position.Z,
				(int)placementFlags,
				amount,
				true,
				customModel.Hash);

			return handle == 0 ? null : new Pickup(handle);
		}
		/// <summary>
		/// Creates a pickup spawner (a <see cref="Pickup"/> instance) which can be referenced by the script and will spawn a pickup whenever the player gets near.
		/// This spawner can also regenerate the pickup after it is collected.
		/// The spawner is removed when the script terminates.
		/// </summary>
		/// <param name="type">The pickup type hash.</param>
		/// <param name="position">The pickup position to place in world space.</param>
		/// <param name="rotation">The pickup orientation.</param>
		/// <param name="placementFlags">The pickup placement flags.</param>
		/// <param name="amount">
		/// A variable amount that can be specified for some pickups, such as money or ammo.
		/// Leave this parameter as <c>-1</c> to apply the default amount.
		/// </param>
		/// <param name="rotOrder">The rotation order in world space.</param>
		/// <param name="customModel">
		/// If set to non-zero value, this model will be used for the pickup instead of the default one.
		/// </param>
		public static Pickup CreatePickup(
			PickupType type,
			Vector3 position,
			Vector3 rotation,
			PickupPlacementFlags placementFlags = PickupPlacementFlags.None,
			int amount = -1,
			EulerRotationOrder rotOrder = EulerRotationOrder.YXZ,
			Model customModel = default
			)
		{
			if (customModel.Hash != 0 && !customModel.Request(1000))
			{
				return null;
			}

			// The 2nd last argument is named ScriptHostObject, so just set to true as most SP scripts do
			int handle = Function.Call<int>(Hash.CREATE_PICKUP_ROTATE,
				(int)type,
				position.X,
				position.Y,
				position.Z,
				rotation.X,
				rotation.Y,
				rotation.Z,
				(int)placementFlags,
				amount,
				(int)rotOrder,
				true,
				customModel.Hash);

			return handle == 0 ? null : new Pickup(handle);
		}

		/// <summary>
		/// Spawns a <see cref="Pickup"/> at the specified position.
		/// </summary>
		[Obsolete("The World.CreatePickup overloads with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
			"Use a World.CreatePickup overload with optional placement flags and amount parameters instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static Pickup CreatePickup(PickupType type, Vector3 position, Model model, int value)
			=> CreatePickup(type, position, PickupPlacementFlags.None, value, model);
		/// <summary>
		/// Spawns a <see cref="Pickup"/> at the specified position.
		/// </summary>
		[Obsolete("The World.CreatePickup overloads with non-optional custom model and amount (named \"value\") parameters are obsolete since they can lead to confusion in custom model parameter (which is actually not mandatory)." +
			"Use a World.CreatePickup overload with optional placement flags and amount parameters instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static Pickup CreatePickup(PickupType type, Vector3 position, Vector3 rotation, Model model, int value)
			=> CreatePickup(type, position, rotation, PickupPlacementFlags.None, value, EulerRotationOrder.YXZ, model);

		#endregion

		#region Checkpoints

		/// <summary>
		/// Gets an <c>array</c> of all the <see cref="Checkpoint"/>s.
		/// </summary>
		public static Checkpoint[] GetAllCheckpoints()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetCheckpointHandles(), element => new Checkpoint(element));
		}

		/// <summary>
		/// Creates a <see cref="Checkpoint"/> in the world.
		/// </summary>
		/// <param name="icon">The <see cref="CheckpointIcon"/> to display inside the <see cref="Checkpoint"/>.</param>
		/// <param name="position">The position in the World.</param>
		/// <param name="pointTo">The position in the world where this <see cref="Checkpoint"/> should point.</param>
		/// <param name="radius">The radius of the <see cref="Checkpoint"/>.</param>
		/// <param name="color">The color of the <see cref="Checkpoint"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Checkpoint"/> could not be created</remarks>
		public static Checkpoint CreateCheckpoint(CheckpointIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color)
		{
			int handle = Function.Call<int>(Hash.CREATE_CHECKPOINT,
				(int)icon,
				position.X,
				position.Y,
				position.Z,
				pointTo.X,
				pointTo.Y,
				pointTo.Z,
				radius,
				color.R,
				color.G,
				color.B,
				color.A,
				0);
			return handle != 0 ? new Checkpoint(handle) : null;
		}
		/// <summary>
		/// Creates a <see cref="Checkpoint"/> in the world.
		/// </summary>
		/// <param name="icon">The <see cref="CheckpointCustomIcon"/> to display inside the <see cref="Checkpoint"/>.</param>
		/// <param name="position">The position in the World.</param>
		/// <param name="pointTo">The position in the world where this <see cref="Checkpoint"/> should point.</param>
		/// <param name="radius">The radius of the <see cref="Checkpoint"/>.</param>
		/// <param name="color">The color of the <see cref="Checkpoint"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Checkpoint"/> could not be created</remarks>
		public static Checkpoint CreateCheckpoint(CheckpointCustomIcon icon, Vector3 position, Vector3 pointTo, float radius, Color color)
		{
			int handle = Function.Call<int>(Hash.CREATE_CHECKPOINT,
				44,
				position.X,
				position.Y,
				position.Z,
				pointTo.X,
				pointTo.Y,
				pointTo.Z,
				radius,
				color.R,
				color.G,
				color.B,
				color.A,
				icon);
			return handle != 0 ? new Checkpoint(handle) : null;
		}

		#endregion

		#region Cameras

		/// <summary>
		/// Destroys all scripted <see cref="Camera"/>s.
		/// </summary>
		[Obsolete("World.DestroyAllCameras is obsolete. Use Camera.DeleteAllCameras instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static void DestroyAllCameras()
		{
			Function.Call(Hash.DESTROY_ALL_CAMS, 0);
		}

		/// <summary>
		/// Creates a <see cref="Camera"/>, use <see cref="ScriptCameraDirector.StartRendering(bool)"/> to switch to
		/// this camera.
		/// </summary>
		/// <param name="position">The position of the camera.</param>
		/// <param name="rotation">The rotation of the camera.</param>
		/// <param name="fov">The field of view of the camera.</param>
		/// <remarks>
		/// This overload (<see cref="World.CreateCamera(Vector3, Vector3, float)"/>) does not return <see langword="null"/>
		/// even if the method fails to create and <c>CREATE_CAM_WITH_PARAMS</c> returns -1 due to the camera pool being full.
		/// This is done for compatibility for scripts built against v3.6.0 or earlier.
		/// </remarks>
		[Obsolete("World.CreateCamera is obsolete. Use Camera.Create instead."),
		 EditorBrowsable(EditorBrowsableState.Never)]
		public static Camera CreateCamera(Vector3 position, Vector3 rotation, float fov)
		{
			return new Camera(Function.Call<int>(Hash.CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2));
		}

		/// <summary>
		/// Gets or sets the rendering camera.
		/// </summary>
		/// <value>
		/// The rendering <see cref="Camera"/>.
		/// </value>
		/// <remarks>
		/// Setting to <see langword="null" /> sets the rendering <see cref="Camera"/> to <see cref="GameplayCamera"/>.
		/// The getter will return a invalid <see cref="Camera"/> where <see cref="PoolObject.Handle"/> is -1 if the
		/// rendering camera does not match any scripted cameras the scripted camera director is managing.
		/// </remarks>
		[Obsolete("World.RenderingCamera is obsolete. " +
			"Use ScriptCameraDirector.RenderingCam to get the rendering scripted camera. " +
			"Use ScriptCameraDirector.StartRendering or ScriptCameraDirector.StopRendering to tell the game to render " +
			"or stop rendering a scripted camera."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static Camera RenderingCamera
		{
			get => new(Function.Call<int>(Hash.GET_RENDERING_CAM));
			set
			{
				if (value == null)
				{
					Function.Call(Hash.RENDER_SCRIPT_CAMS, false, 0, 3000, 1, 0);
				}
				else
				{
					value.IsActive = true;
					Function.Call(Hash.RENDER_SCRIPT_CAMS, true, 0, 3000, 1, 0);
				}
			}
		}

		#endregion

		#region Area Clearance

		/// <summary>
		/// Clears non-mission <see cref="Entity"/>s and cleans <see cref="Building"/>s and <see cref="AnimatedBuilding"/>s within the defined sphere.
		/// All fires and explosions in the area are also cleared.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		/// <param name="deleteProjectiles">
		/// If <see langword="true"/>, all <see cref="Projectile"/>s in the area will also be cleared
		/// except for ones that are on <see cref="Ped"/>s' hands or rocket <see cref="Projectile"/>s that are attached to rocket weapon <see cref="Prop"/>s.
		/// </param>
		/// <param name="leaveCarGenCars">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s generated by vehicle generators will be cleared even if they have been entered.
		/// </param>
		/// <param name="clearLowPriorityPickupsOnly">
		/// If <see langword="true"/>, none of pickup <see cref="Prop"/>s will get deleted except for low priority ones (although the exact condition is unknown).
		/// </param>
		/// <remarks>
		/// Does not delete non-mission <see cref="Entity"/>s that is protected by conditions other than <see cref="Entity.PopulationType"/>.
		/// For example, this method does not delete the <see cref="Vehicle"/> the player is in or automobiles (excluding amphibious or submarine cars), bikes, and helicopters (excluding blimps) in some garages.
		/// This method does not delete <see cref="Ped"/> who are members of the player group as they are protected, either.
		/// </remarks>
		public static void ClearArea(Vector3 position, float radius, bool deleteProjectiles, bool leaveCarGenCars = false, bool clearLowPriorityPickupsOnly = false)
			=> Function.Call(Hash.CLEAR_AREA, position.X, position.Y, position.Z, radius, deleteProjectiles, leaveCarGenCars, clearLowPriorityPickupsOnly, false);


		/// <summary>
		/// Clears <see cref="Projectile"/> within the defined sphere except for ones that are on <see cref="Ped"/>s' hands or rocket <see cref="Projectile"/>s that are attached to rocket weapon <see cref="Prop"/>s.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		public static void ClearAreaOfProjectiles(Vector3 position, float radius) => Function.Call(Hash.CLEAR_AREA_OF_PROJECTILES, position.X, position.Y, position.Z, radius, false);

		/// <summary>
		/// Clears non-mission <see cref="Vehicle"/>s within the defined sphere.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		/// <param name="leaveCarGenCars">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s generated by vehicle generators will be cleared even if they have been entered.
		/// </param>
		/// <param name="checkViewFrustum">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s on screen will be cleared.
		/// </param>
		/// <param name="ifWrecked">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s on which <see cref="Entity.IsDead"/> returns <see langword="true"/> will be cleared.
		/// </param>
		/// <param name="ifAbandoned">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s that has a non-mission driver <see cref="Ped"/> will be cleared.
		/// None of <see cref="Vehicle"/>s has a mission driver <see cref="Ped"/> will be cleared even if this parameter is set to <see langword="false"/>.
		/// </param>
		/// <param name="ifEngineOnFire">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s on which <see cref="Vehicle.EngineHealth"/> return a value more than zero will be cleared.
		/// </param>
		/// <remarks>
		/// Does not delete non-mission <see cref="Entity"/>s that is protected by conditions other than <see cref="Entity.PopulationType"/>.
		/// For example, this method does not delete the <see cref="Vehicle"/> the player is in or automobiles (excluding amphibious or submarine cars), bikes, and helicopters (excluding blimps) in some garages.
		/// </remarks>
		public static void ClearAreaOfVehicles(Vector3 position, float radius, bool leaveCarGenCars = false, bool checkViewFrustum = false, bool ifWrecked = false, bool ifAbandoned = false, bool ifEngineOnFire = false)
			// 11th parameter is supposed to be KeepScriptTrains (available since b2545), but does not seem to change the behavior for trains in any way (does not clear of trains regardless of the 11th argument)
			=> Function.Call(Hash.CLEAR_AREA_OF_VEHICLES, position.X, position.Y, position.Z, radius, leaveCarGenCars, checkViewFrustum, ifWrecked, ifAbandoned, false, ifEngineOnFire, false);

		/// <summary>
		/// <para>
		/// Clears the non axis aligned area of non-mission <see cref="Vehicle"/>s.
		/// </para>
		/// <para>
		/// <paramref name="position1"/> and <paramref name="position2"/> define the midpoints of two parallel sides and <paramref name="areaWidth"/> is the width of these sides.
		/// </para>
		/// </summary>
		/// <param name="position1">One of the midpoints of two parallel sides, which should be different from <paramref name="position2"/>.</param>
		/// <param name="position2">One of the midpoints of two parallel sides, which should be different from <paramref name="position1"/>.</param>
		/// <param name="areaWidth">The width of these sides that defines <paramref name="position1"/> and <paramref name="position2"/>.</param>
		/// <param name="leaveCarGenCars">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s generated by vehicle generators will be cleared even if they have been entered.
		/// </param>
		/// <param name="checkViewFrustum">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s on screen will be cleared.
		/// </param>
		/// <param name="ifWrecked">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s on which <see cref="Entity.IsDead"/> returns <see langword="true"/> will be cleared.
		/// </param>
		/// <param name="ifAbandoned">
		/// If <see langword="true"/>, none of <see cref="Vehicle"/>s that has a non-mission driver <see cref="Ped"/> will be cleared.
		/// None of <see cref="Vehicle"/>s has a mission driver <see cref="Ped"/> will be cleared even if this parameter is set to <see langword="false"/>.
		/// </param>
		/// <param name="ifEngineOnFire">
		/// If <see langword="true"/> and the game version is v1.0.1180.2 or later, none of <see cref="Vehicle"/>s on which <see cref="Vehicle.EngineHealth"/> return a value more than zero will be cleared.
		/// </param>
		/// <remarks>
		/// Does not delete non-mission <see cref="Entity"/>s that is protected by conditions other than <see cref="Entity.PopulationType"/>.
		/// For example, this method does not delete the <see cref="Vehicle"/> the player is in or automobiles (excluding amphibious or submarine cars), bikes, and helicopters (excluding blimps) in some garages.
		/// </remarks>
		public static void ClearAngledAreaOfVehicles(Vector3 position1, Vector3 position2, float areaWidth, bool leaveCarGenCars = false, bool checkViewFrustum = false, bool ifWrecked = false, bool ifAbandoned = false, bool ifEngineOnFire = false)
			// 14th parameter is supposed to be KeepScriptTrains (available since b2545), but does not seem to change the behavior for trains in any way (does not clear of trains regardless of the 14th argument)
			=> Function.Call(Hash.CLEAR_ANGLED_AREA_OF_VEHICLES, position1.X, position1.Y, position1.Z, position2.X, position2.Y, position2.Z, areaWidth, leaveCarGenCars, checkViewFrustum, ifWrecked, ifAbandoned, false, ifEngineOnFire, false);

		/// <summary>
		/// Clears non-mission <see cref="Prop"/>s within the defined sphere. Does not clear pickup <see cref="Prop"/>s or <see cref="Projectile"/>s.
		/// As calling <see cref="Entity.Delete"/> on random <see cref="Prop"/>s (most likely on which population types are set to <see cref="EntityPopulationType.Unknown"/>)
		/// will result in the almost immediate game crash, you should clear area of ambient <see cref="Prop"/>s with this method instead of the said way.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		/// <param name="flags">The flags for area clearance.</param>
		public static void ClearAreaOfProps(Vector3 position, float radius, ClearPropsFlags flags)
			=> Function.Call(Hash.CLEAR_AREA_OF_OBJECTS, position.X, position.Y, position.Z, radius, (int)flags);

		/// <summary>
		/// Clears non-mission <see cref="Ped"/>s within the defined sphere.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		/// <remarks>
		/// Does not delete <see cref="Ped"/> who are members of the player group as they are protected.
		/// </remarks>
		public static void ClearAreaOfPeds(Vector3 position, float radius)
			=> Function.Call(Hash.CLEAR_AREA_OF_PEDS, position.X, position.Y, position.Z, radius, false);

		/// <summary>
		/// Clears non-mission cop <see cref="Ped"/>s within the defined sphere.
		/// </summary>
		/// <param name="position">The center position.</param>
		/// <param name="radius">The radius for area clearance.</param>
		/// <remarks>
		/// Does not delete <see cref="Ped"/> who are members of the player group as they are protected.
		/// </remarks>
		public static void ClearAreaOfCops(Vector3 position, float radius)
			=> Function.Call(Hash.CLEAR_AREA_OF_COPS, position.X, position.Y, position.Z, radius, false);

		/// <summary>
		/// Clears all currently present trains from the world.
		/// </summary>
		public static void DeleteAllTrains() => Function.Call(Hash.DELETE_ALL_TRAINS);

		#endregion

		#region Particle Effects

		/// <summary>
		/// Starts a Particle Effect that runs once at a given position then is destroyed.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="pos">The World position where the effect is.</param>
		/// <param name="rot">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		/// <returns><see langword="true" />If the effect was able to start; otherwise, <see langword="false" />.</returns>
		public static bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Vector3 pos, Vector3 rot = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			if (!asset.UseNext())
			{
				return false;
			}

			bool invertAxisFlagX = HasFlagFast(invertAxis, InvertAxisFlags.X);
			bool invertAxisFlagY = HasFlagFast(invertAxis, InvertAxisFlags.Y);
			bool invertAxisFlagZ = HasFlagFast(invertAxis, InvertAxisFlags.Z);

			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_AT_COORD, effectName, pos.X, pos.Y, pos.Z, rot.X, rot.Y, rot.Z, scale, invertAxisFlagX, invertAxisFlagY, invertAxisFlagZ);
		}
		/// <summary>
		/// Starts a Particle Effect on an <see cref="Entity"/> that runs once then is destroyed.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entity"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entity"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exhaust you may need to flip in the Y Axis</param>
		/// <returns><see langword="true" />If the effect was able to start; otherwise, <see langword="false" />.</returns>
		public static bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 off = default, Vector3 rot = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			if (!asset.UseNext())
			{
				return false;
			}

			bool invertAxisFlagX = HasFlagFast(invertAxis, InvertAxisFlags.X);
			bool invertAxisFlagY = HasFlagFast(invertAxis, InvertAxisFlags.Y);
			bool invertAxisFlagZ = HasFlagFast(invertAxis, InvertAxisFlags.Z);

			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY, effectName, entity.Handle, off.X, off.Y, off.Z, rot.X, rot.Y, rot.Z, scale, invertAxisFlagX, invertAxisFlagY, invertAxisFlagZ);
		}
		/// <summary>
		/// Starts a Particle Effect on an <see cref="EntityBone"/> that runs once then is destroyed.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">the name of the effect.</param>
		/// <param name="entityBone">The <see cref="EntityBone"/> the effect is attached to.</param>
		/// <param name="off">The offset from the <paramref name="entityBone"/> to attach the effect.</param>
		/// <param name="rot">The rotation, relative to the <paramref name="entityBone"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exhaust you may need to flip in the Y Axis</param>
		/// <returns><see langword="true" />If the effect was able to start; otherwise, <see langword="false" />.</returns>
		public static bool CreateParticleEffectNonLooped(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 off = default, Vector3 rot = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			if (!asset.UseNext())
			{
				return false;
			}

			bool invertAxisFlagX = HasFlagFast(invertAxis, InvertAxisFlags.X);
			bool invertAxisFlagY = HasFlagFast(invertAxis, InvertAxisFlags.Y);
			bool invertAxisFlagZ = HasFlagFast(invertAxis, InvertAxisFlags.Z);

			return Function.Call<bool>(Hash.START_PARTICLE_FX_NON_LOOPED_ON_ENTITY_BONE, effectName, entityBone.Owner.Handle, off.X, off.Y, off.Z, rot.X, rot.Y, rot.Z, entityBone, scale, invertAxisFlagX, invertAxisFlagY, invertAxisFlagZ);
		}

		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> on an <see cref="Entity"/> that runs looped.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">The name of the Effect</param>
		/// <param name="entity">The <see cref="Entity"/> the effect is attached to.</param>
		/// <param name="offset">The offset from the <paramref name="entity"/> to attach the effect.</param>
		/// <param name="rotation">The rotation, relative to the <paramref name="entity"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exhaust you may need to flip in the Y Axis.</param>
		public static ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Entity entity, Vector3 offset = default, Vector3 rotation = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			return CreateParticleEffect(asset, effectName, entity.Bones.Core, offset, rotation, scale, invertAxis);
		}
		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> on an <see cref="EntityBone"/> that runs looped.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">The name of the Effect</param>
		/// <param name="entityBone">The <see cref="EntityBone"/> the effect is attached to.</param>
		/// <param name="offset">The offset from the <paramref name="entityBone"/> to attach the effect.</param>
		/// <param name="rotation">The rotation, relative to the <paramref name="entityBone"/>, the effect has.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in. For a car side exhaust you may need to flip in the Y Axis.</param>
		public static ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, EntityBone entityBone, Vector3 offset = default, Vector3 rotation = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			if (!asset.UseNext())
			{
				return null;
			}

			bool invertAxisFlagX = HasFlagFast(invertAxis, InvertAxisFlags.X);
			bool invertAxisFlagY = HasFlagFast(invertAxis, InvertAxisFlags.Y);
			bool invertAxisFlagZ = HasFlagFast(invertAxis, InvertAxisFlags.Z);

			int handle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_ON_ENTITY_BONE, effectName, entityBone.Owner.Handle, offset.X, offset.Y, offset.Z, rotation.X, rotation.Y, rotation.Z, entityBone.Index, scale, invertAxisFlagX, invertAxisFlagY, invertAxisFlagZ);
			if (handle == 0)
			{
				return null;
			}

			return new ParticleEffect(handle, asset.AssetName, effectName, entityBone);
		}
		/// <summary>
		/// Creates a <see cref="ParticleEffect"/> at a position that runs looped.
		/// </summary>
		/// <param name="asset">The effect asset to use.</param>
		/// <param name="effectName">The name of the effect.</param>
		/// <param name="position">The world coordinates where the effect is.</param>
		/// <param name="rotation">What rotation to apply to the effect.</param>
		/// <param name="scale">How much to scale the size of the effect by.</param>
		/// <param name="invertAxis">Which axis to flip the effect in.</param>
		public static ParticleEffect CreateParticleEffect(ParticleEffectAsset asset, string effectName, Vector3 position, Vector3 rotation = default, float scale = 1.0f, InvertAxisFlags invertAxis = InvertAxisFlags.None)
		{
			if (!asset.UseNext())
			{
				return null;
			}

			bool invertAxisFlagX = HasFlagFast(invertAxis, InvertAxisFlags.X);
			bool invertAxisFlagY = HasFlagFast(invertAxis, InvertAxisFlags.Y);
			bool invertAxisFlagZ = HasFlagFast(invertAxis, InvertAxisFlags.Z);

			int handle = Function.Call<int>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, effectName, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, scale, invertAxisFlagX, invertAxisFlagY, invertAxisFlagZ, false);
			return handle == 0 ? null : new ParticleEffect(handle, asset.AssetName, effectName, null);
		}

		private static bool HasFlagFast(InvertAxisFlags flagValues, InvertAxisFlags flag) => (flagValues & flag) == flag;

		/// <summary>
		/// Stops all particle effects in a range.
		/// </summary>
		/// <param name="pos">The position in the world to stop particle effects.</param>
		/// <param name="range">The maximum distance from the <paramref name="pos"/> to stop particle effects.</param>
		public static void RemoveAllParticleEffectsInRange(Vector3 pos, float range)
		{
			Function.Call(Hash.REMOVE_PARTICLE_FX_IN_RANGE, pos.X, pos.Y, pos.Z, range);
		}

		#endregion

		#region Others

		/// <summary>
		/// Spawns a <see cref="Rope"/>.
		/// </summary>
		/// <param name="type">The type of <see cref="Rope"/>.</param>
		/// <param name="position">The position of the <see cref="Rope"/>.</param>
		/// <param name="rotation">The rotation of the <see cref="Rope"/>.</param>
		/// <param name="length">The length of the <see cref="Rope"/>.</param>
		/// <param name="minLength">The minimum length of the <see cref="Rope"/>.</param>
		/// <param name="breakable">if set to <see langword="true" /> the <see cref="Rope"/> will break if shot.</param>
		public static Rope AddRope(RopeType type, Vector3 position, Vector3 rotation, float length, float minLength, bool breakable)
		{
			Function.Call(Hash.ROPE_LOAD_TEXTURES);

			return new Rope(Function.Call<int>(Hash.ADD_ROPE,
				position.X,
				position.Y,
				position.Z,
				rotation.X,
				rotation.Y,
				rotation.Z,
				length,
				(int)type,
				length,
				minLength,
				0.5f,
				false,
				false,
				true,
				1.0f,
				breakable,
				0));
		}

		/// <summary>
		/// Fires a single bullet in the world.
		/// </summary>
		/// <param name="sourcePosition">Where the bullet is fired from.</param>
		/// <param name="targetPosition">Where the bullet is fired to.</param>
		/// <param name="owner">The <see cref="Ped"/> who fired the bullet, leave <see langword="null" /> for no one.</param>
		/// <param name="weaponAsset">The weapon that the bullet is fired from.</param>
		/// <param name="damage">The damage the bullet will cause.</param>
		/// <param name="speed">The speed, only affects projectile weapons, leave -1 for default.</param>
		[Obsolete("Use ShootSingleBullet instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, WeaponAsset weaponAsset, int damage, float speed = -1f)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, weaponAsset.Hash, (owner == null ? 0 : owner.Handle), 1, 0, speed);
		}
		/// <summary>
		/// Fires an instant hit bullet between the two points or a projectile that goes toward
		/// <paramref name="endPosition"/>.
		/// </summary>
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float, Entity, bool, bool, Entity, bool, bool, bool)"/>
		public static void ShootSingleBullet(Vector3 startPosition, Vector3 endPosition, int damage, WeaponAsset weapon,
			Ped owner = null, bool perfectAccuracy = true, bool createTraceVfx = true, bool allowRumble = true,
			float initialVelocity = -1f)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, startPosition.X, startPosition.Y, startPosition.Z,
				endPosition.X, endPosition.Y, endPosition.Z, damage, perfectAccuracy, weapon, owner, createTraceVfx,
				allowRumble, initialVelocity);
		}

		/// <param name="startPosition">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param"/>
		/// </param>
		/// <param name="endPosition">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='endPosition']"/>
		/// </param>
		/// <param name="damage">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='damage']"/>
		/// </param>
		/// <param name="weapon">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='weapon']"/>
		/// </param>
		/// <param name="owner">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='owner']"/>
		/// </param>
		/// <param name="perfectAccuracy">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='perfectAccurary']"/>
		/// </param>
		/// <param name="createTraceVfx">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='createTraceVfx']"/>
		/// </param>
		/// <param name="allowRumble">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='allowRumble']"/>
		/// </param>
		/// <param name="initialVelocity">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='initialVelocity']"/>
		/// </param>
		/// <param name="ignoreEntity">
		/// <para>
		/// The <see cref="Entity"/> who the bullet or projectile disable damage against. Must not be the same as
		/// <paramref name="owner"/>, as the bullet will not collide with the owner or the <see cref="Vehicle"/> the
		/// owner is in.
		/// </para>
		/// <para>
		/// To prevent created projectiles from colliding with this <see cref="Entity"/>, you will need to use
		/// <see cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)"/> and set the bool parameter <c>ignoreCollisionEntity</c>
		/// to <see langword="true"/>.
		/// </para>
		/// </param>
		/// <param name="targetEntity">
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)" path="/param[@name='targetEntity']"/>
		/// </param>
		///
		/// <inheritdoc cref="ShootSingleBulletIgnoreEntityNew(Vector3, Vector3, int, WeaponAsset, Ped, bool, bool, bool, float,
		/// Entity, bool, bool, Entity, bool, bool, bool)"/>
		public static void ShootSingleBulletIgnoreEntity(Vector3 startPosition, Vector3 endPosition, int damage,
			WeaponAsset weapon, Ped owner = null, bool perfectAccuracy = true, bool createTraceVfx = true,
			bool allowRumble = true, float initialVelocity = -1f, Entity ignoreEntity = null,
			Entity targetEntity = null)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS_IGNORE_ENTITY, startPosition.X, startPosition.Y,
				startPosition.Z, endPosition.X, endPosition.Y, endPosition.Z, damage, perfectAccuracy, weapon, owner,
				createTraceVfx, allowRumble, initialVelocity, ignoreEntity, targetEntity);
		}

		/// <summary>
		/// Fires an instant hit bullet between the two points or a projectile that goes toward
		/// <paramref name="endPosition"/> taking into account an entity to ignore for damage.
		/// </summary>
		/// <param name="startPosition">
		/// Where the bullet or projectile is fired from.
		/// </param>
		/// <param name="endPosition">
		/// Where the bullet is fired to.
		/// If the <paramref name="weapon"/> specifies a projectile weapon, the produced projectile will go
		/// in the direction of this parameter from <paramref name="startPosition"/>.
		/// </param>
		/// <param name="damage">
		/// The damage the bullet will cause.
		/// Must not be negative; otherwise, this method will not fire a bullet.
		/// </param>
		/// <param name="weapon">
		/// The weapon type to fire a bullet or projectile.
		/// You might want to request the asset and keep in memory unless you know the weapon info is already streamed,
		/// such as when <paramref name="owner"/> has a weapon of this parameter hash. If the weapon info is not
		/// streamed, this method will not fire a bullet or projectile.
		/// </param>
		/// <param name="owner">
		/// The owner <see cref="Ped"/> who fires the bullet or projectile.
		/// Leaving <see langword="null" /> will result in absent of owner of the produced bullet or projectile.
		/// </param>
		/// <param name="perfectAccuracy">
		/// If <see langword="true"/>, the bullet will go to the exact point where <paramref name="endPosition"/> is.
		/// If <see langword="false"/>, the method will apply a spread to the bullet.
		/// </param>
		/// <param name="createTraceVfx">
		/// If <see langword="true"/>, the bullet trace visual effect will not be prevented from being created for
		/// not hitting an <see cref="Entity"/>.
		/// Note that there are other conditions for where bullet comes from in both position and direction
		/// before bullet trace can be created (this parameter has no effect in these conditions).
		/// </param>
		/// <param name="allowRumble">
		/// If <see langword="true"/>, <paramref name="owner"/> is the player, and you are using a controller (pad),
		/// the controller can rumble/vibrate due to the corresponding <c>CWeaponInfo</c> setting.
		/// </param>
		/// <param name="initialVelocity">
		/// The initial velocity for the produced projectile, leave -1 (or some negative value) for the default speed.
		/// Not used for bullets.
		/// </param>
		/// <param name="ignoreEntity">
		/// <para>
		/// The <see cref="Entity"/> who the bullet or projectile disable damage against. Must not be the same as
		/// <paramref name="owner"/>, as the bullet will not collide with the owner or the <see cref="Vehicle"/> the
		/// owner is in.
		/// </para>
		/// <para>
		/// To prevent created projectiles from colliding with this <see cref="Entity"/>, you need to set
		/// <paramref name="ignoreCollisionEntity"/> to <see langword="true"/> (the bool parameter has no effect
		/// in the game versions earlier than v1.0.1103.2).
		/// </para>
		/// </param>
		/// <param name="forceCreateNewProjectileObject">
		/// If <see langword="true"/>, <paramref name="owner"/> is set, and <paramref name="weapon"/> is a projectile
		/// weapon, this method will create a NEW projectile and don't use any equipped <see cref="Projectile"/>s from
		/// a weapon <see cref="Prop"/> of <paramref name="owner"/>.
		/// </param>
		/// <param name="disablePlayerCoverStartAdjustment">
		/// Although this parameter name follows the canonical one <c>bDisablePlayerCoverStartAdjustment</c>,
		/// the actual effect is not known enough to describe (there is only one occurrence where this parameter is set
		/// to true out of all ysc scripts in v1.0.2944.0).
		/// </param>
		/// <param name="targetEntity">
		/// The <see cref="Entity"/> who the produced rocket will home in.
		/// Must not be the same as <paramref name="owner"/> or <paramref name="ignoreEntity"/>.
		/// Only used if <paramref name="weapon"/> is a rocket weapon and the homing flag is set in the corresponding
		/// <c>CWeaponInfo</c> data. For example, <c>VEHICLE_WEAPON_SPACE_ROCKET</c> and <c>VEHICLE_WEAPON_PLANE_ROCKET</c>
		/// can be used to home in. However, <see cref="WeaponHash.HomingLauncher"/> is not appropriate to home in
		/// using this method even though <c>Homing</c> flag is set in the <c>CWeaponInfo</c> data.
		/// </param>
		/// <param name="freezeProjectileWaitingOnCollision">
		/// If <see langword="true"/> and the game version is v1.0.1103.2 or later,
		/// the created projectile will freeze waiting for collision if absent.
		/// </param>
		/// <param name="ignoreCollisionEntity">
		/// <para>
		/// If <see langword="true"/> and the game version is v1.0.1103.2 or later,
		/// the created projectile will not collide with <paramref name="ignoreEntity"/>.
		/// </para>
		/// <para>
		/// Cannot be used in conjunction with <paramref name="ignoreCollisionResetNoBB"/>
		/// and <paramref name="ignoreCollisionResetNoBB"/> takes precedence if both are set to <see langword="true"/>
		/// and the game version is v1.0.2189.0 or later.
		/// </para>
		/// </param>
		/// <param name="ignoreCollisionResetNoBB">
		/// <para>
		/// If <see langword="true"/> and if <paramref name="startPosition"/> is inside the BoundingBox of
		/// <paramref name="ignoreEntity"/>, the created projectile will ignore collision until it leaves the
		/// BoundingBox.
		/// </para>
		/// <para>
		///	Only available in the game version is v1.0.2189.0 or later.
		/// Cannot be used in conjunction with <paramref name="ignoreCollisionEntity"/>
		/// and this parameter takes precedence if both are set to <see langword="true"/>.
		/// </para>
		/// </param>
		public static void ShootSingleBulletIgnoreEntityNew(Vector3 startPosition, Vector3 endPosition, int damage,
			WeaponAsset weapon, Ped owner = null, bool perfectAccuracy = true, bool createTraceVfx = true,
			bool allowRumble = true, float initialVelocity = -1f, Entity ignoreEntity = null,
			bool forceCreateNewProjectileObject = false, bool disablePlayerCoverStartAdjustment = false,
			Entity targetEntity = null, bool freezeProjectileWaitingOnCollision = false,
			bool ignoreCollisionEntity = false, bool ignoreCollisionResetNoBB = false)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS_IGNORE_ENTITY_NEW, startPosition.X, startPosition.Y,
				startPosition.Z, endPosition.X, endPosition.Y, endPosition.Z, damage, perfectAccuracy, weapon, owner,
				createTraceVfx, allowRumble, initialVelocity, ignoreEntity, forceCreateNewProjectileObject,
				disablePlayerCoverStartAdjustment, targetEntity,
				true /* named bDoDeadCheck in the official header, which is not used in the internal function */,
				freezeProjectileWaitingOnCollision, ignoreCollisionEntity, ignoreCollisionResetNoBB);
		}

		/// <summary>
		/// Creates an explosion in the world.
		/// </summary>
		/// <param name="position">The position of the explosion.</param>
		/// <param name="type">The type of explosion.</param>
		/// <param name="radius">The radius of the explosion.</param>
		/// <param name="cameraShake">The amount of camera shake to apply to nearby cameras.</param>
		/// <param name="owner">The <see cref="Ped"/> who caused the explosion, leave null if no one caused the explosion.</param>
		/// <param name="aubidble">If set to <see langword="true" />, explosion can be heard.</param>
		/// <param name="invisible">If set to <see langword="true" />, explosion will not create particle effects.</param>
		public static void AddExplosion(Vector3 position, ExplosionType type, float radius, float cameraShake, Ped owner = null, bool aubidble = true, bool invisible = false)
		{
			if (owner?.Exists() == true)
			{
				Function.Call(Hash.ADD_OWNED_EXPLOSION,
					owner.Handle,
					position.X,
					position.Y,
					position.Z,
					(int)type,
					radius,
					aubidble,
					invisible,
					cameraShake);
			}
			else
			{
				Function.Call(Hash.ADD_EXPLOSION,
					position.X,
					position.Y,
					position.Z,
					(int)type,
					radius,
					aubidble,
					invisible,
					cameraShake);
			}
		}

		/// <summary>
		/// Creates a <see cref="RelationshipGroup"/> with the given name.
		/// </summary>
		/// <param name="name">The name of the relationship group.</param>
		public static RelationshipGroup AddRelationshipGroup(string name)
		{
			int resultArg;
			unsafe
			{
				Function.Call(Hash.ADD_RELATIONSHIP_GROUP, name, &resultArg);
			}

			return new RelationshipGroup(resultArg);
		}

		/// <summary>
		/// Sets the intensity of the "shaking" effect of all vehicles at speed.
		/// </summary>
		/// <param name="multiplier">Intensity of the shaking effect between 0f and 1f.</param>
		public static void VehicleHighSpeedBumpMultiplier(float multiplier) => Function.Call(Hash.SET_CAR_HIGH_SPEED_BUMP_SEVERITY_MULTIPLIER, multiplier);

		#endregion

		#region Drawing

		/// <summary>
		/// Draws a marker in the world, this needs to be done on a per frame basis
		/// </summary>
		/// <param name="type">The type of marker.</param>
		/// <param name="pos">The position of the marker.</param>
		/// <param name="dir">The direction the marker points in.</param>
		/// <param name="rot">The rotation of the marker.</param>
		/// <param name="scale">The amount to scale the marker by.</param>
		/// <param name="color">The color of the marker.</param>
		/// <param name="bobUpAndDown">if set to <see langword="true" /> the marker will bob up and down.</param>
		/// <param name="faceCamera">if set to <see langword="true" /> the marker will always face the camera, regardless of its rotation.</param>
		/// <param name="rotateY">
		/// if set to <see langword="true" /> rotates only on the z axis (heading).
		/// Incorrectly named as &quot;rotateY&quot;, but the name is kept for source compatibility.
		/// </param>
		/// <param name="textueDict">Name of texture dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="textureName">Name of texture inside the dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="drawOnEntity">if set to <see langword="true" /> draw on any <see cref="Entity"/> that intersects the marker.</param>
		/// <remarks>
		/// There is no overload that takes <see cref="Nullable{T}"/> <see cref="TextureAsset"/> as a default
		/// parameter, since trying to provide one will result in ambiguious call resolution.
		/// </remarks>
		public static void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown = false, bool faceCamera = false, bool rotateY = false, string textueDict = null, string textureName = null, bool drawOnEntity = false)
		{
			if (!string.IsNullOrEmpty(textueDict) && !string.IsNullOrEmpty(textureName))
			{
				Function.Call(Hash.DRAW_MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X,
					scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamera, 2, rotateY, textueDict,
					textureName, drawOnEntity);
			}
			else
			{
				Function.Call(Hash.DRAW_MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X,
					scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamera, 2, rotateY, 0, 0, drawOnEntity);
			}
		}
		/// <summary>
		/// Draws a marker this frame with 2 extra parameters.
		/// Not supported in the game versions earlier than v1.0.573.1.
		/// </summary>
		/// <param name="type">The type of marker.</param>
		/// <param name="pos">The position of the marker.</param>
		/// <param name="dir">The direction the marker points in.</param>
		/// <param name="rot">The rotation of the marker.</param>
		/// <param name="scale">The amount to scale the marker by.</param>
		/// <param name="color">The color of the marker.</param>
		/// <param name="bounce">if set to <see langword="true" /> the marker will bounce up and down.</param>
		/// <param name="faceCamera">if set to <see langword="true" /> the marker will always face the camera, regardless of its rotation.</param>
		/// <param name="rotOrder">The rotation order.</param>
		/// <param name="rotate">if set to <see langword="true" /> rotates only on the z axis(heading).</param>
		/// <param name="texAsset">
		/// The <see cref="TextureAsset"/> to use a custom texture.
		/// Leave <see langword="null"/> to use the texture for <paramref name="type"/>.
		/// </param>
		/// <param name="renderInverted">
		/// if set to <see langword="true"/> the marker will be drawed in the reverse order.
		/// Marker vertices will shown on any <c>CEntity</c> (which includes but not limited to <see cref="Entity"/>
		/// and <see cref="Building"/>), that are intersected.
		/// </param>
		/// <param name="usePreAlphaDepth">
		/// If <see langword="false"/>, the marker will not use pre-alpha depth, making the marker invisible behind
		/// translucent <see cref="Vehicle"/>s and translucent <see cref="Prop"/>s with smooth opacity (which is
		/// different from <see cref="DrawMarker"/>.
		/// </param>
		/// <param name="matchEntityRotOrder">
		/// If <see langword="true"/>, the marker will rotate in the same way how <see cref="Entity"/>s are rotated
		/// (which is different from <see cref="DrawMarker"/>.
		/// </param>
		public static void DrawMarkerEx(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale,
			Color color, bool bounce = false, bool faceCamera = false,
			EulerRotationOrder rotOrder = EulerRotationOrder.YXZ, bool rotate = false,
			TextureAsset? texAsset = null, bool renderInverted = false,
			bool usePreAlphaDepth = true, bool matchEntityRotOrder = false)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_573_1_Steam, nameof(World),
				nameof(DrawMarkerEx));

			if (texAsset == null)
			{
				Function.Call(Hash.DRAW_MARKER_EX, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y,
					rot.Z, scale.X, scale.Y, scale.Z, color.R, color.G, color.B, color.A, bounce, faceCamera,
					(int)rotOrder, rotate, null, null, renderInverted, usePreAlphaDepth, matchEntityRotOrder);
			}
			else
			{
				(Txd txd, string texName) = texAsset.GetValueOrDefault();
				Function.Call(Hash.DRAW_MARKER_EX, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y,
					rot.Z, scale.X, scale.Y, scale.Z, color.R, color.G, color.B, color.A, bounce, faceCamera,
					(int)rotOrder, rotate, txd, texName, renderInverted, usePreAlphaDepth, matchEntityRotOrder);
			}
		}

		/// <summary>
		/// Draws light around a region.
		/// </summary>
		/// <param name="position">The position to center the light around.</param>
		/// <param name="color">The color of the light.</param>
		/// <param name="range">How far the light should extend to.</param>
		/// <param name="intensity">The intensity: should be positive.</param>
		public static void DrawLightWithRange(Vector3 position, Color color, float range, float intensity)
		{
			Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, position.X, position.Y, position.Z, color.R, color.G, color.B, range,
				intensity);
		}

		public static void DrawSpotLight(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout)
		{
			Function.Call(Hash.DRAW_SPOT_LIGHT, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
		}

		public static void DrawSpotLightWithShadow(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout)
		{
			Function.Call(Hash.DRAW_SHADOWED_SPOT_LIGHT, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
		}

		public static void DrawLine(Vector3 start, Vector3 end, Color color)
		{
			Function.Call(Hash.DRAW_LINE, start.X, start.Y, start.Z, end.X, end.Y, end.Z, color.R, color.G, color.B, color.A);
		}

		public static void DrawPolygon(Vector3 vertexA, Vector3 vertexB, Vector3 vertexC, Color color)
		{
			Function.Call(Hash.DRAW_POLY, vertexA.X, vertexA.Y, vertexA.Z, vertexB.X, vertexB.Y, vertexB.Z, vertexC.X, vertexC.Y, vertexC.Z, color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Draws a box that occupies the angled area.
		/// An angled area is an X-Z oriented rectangle with three parameters: origin, extent, and width.
		/// </summary>
		/// <param name="originEdge">The mid-point along a base edge of the rectangle.</param>
		/// <param name="extentEdge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="width">The length of the base edge.</param>
		/// <param name="color">The color of the box.</param>
		/// <param name="drawFlags">Which sides to draw.</param>
		public static void DrawBoxForAngledArea(Vector3 originEdge, Vector3 extentEdge, float width, Color color, DrawBoxFlags drawFlags = DrawBoxFlags.OutsideOnly)
		{
			if ((drawFlags & DrawBoxFlags.InsideOnly) == DrawBoxFlags.InsideOnly)
			{
				DrawBoxForAngledAreaInsideInternal(originEdge, extentEdge, width, color);
			}
			if ((drawFlags & DrawBoxFlags.OutsideOnly) == DrawBoxFlags.OutsideOnly)
			{
				DrawBoxForAngledAreaOutsideInternal(originEdge, extentEdge, width, color);
			}
		}

		private static void DrawBoxForAngledAreaOutsideInternal(Vector3 origin, Vector3 extent, float width, Color color)
		{
			Vector3 point1 = origin;
			Vector3 point2 = extent;
			var point3 = new Vector3(point2.X, point2.Y, point1.Z);
			Vector3 normalVector = Vector3.Cross(point2 - point1, point3 - point1).Normalized * (width / 2);

			var point4 = new Vector3(point1.X, point1.Y, point2.Z);

			DrawQuadPolygonInternal(point1 + normalVector, point2 + normalVector, point3 + normalVector, point4 + normalVector, color);
			DrawQuadPolygonInternal(point2 - normalVector, point1 - normalVector, point3 - normalVector, point4 - normalVector, color);

			DrawQuadPolygonInternal(point1 + normalVector, point3 - normalVector, point1 - normalVector, point3 + normalVector, color);
			DrawQuadPolygonInternal(point2 + normalVector, point4 - normalVector, point2 - normalVector, point4 + normalVector, color);

			DrawQuadPolygonInternal(point2 + normalVector, point3 - normalVector, point3 + normalVector, point2 - normalVector, color);
			DrawQuadPolygonInternal(point1 + normalVector, point4 - normalVector, point4 + normalVector, point1 - normalVector, color);
		}

		private static void DrawBoxForAngledAreaInsideInternal(Vector3 origin, Vector3 extent, float width, Color color)
		{
			Vector3 point1 = origin;
			Vector3 point2 = extent;
			var point3 = new Vector3(point2.X, point2.Y, point1.Z);
			Vector3 normalVector = Vector3.Cross(point2 - point1, point3 - point1).Normalized * (width / 2);

			var point4 = new Vector3(point1.X, point1.Y, point2.Z);

			DrawQuadPolygonInternal(point2 + normalVector, point1 + normalVector, point3 + normalVector, point4 + normalVector, color);
			DrawQuadPolygonInternal(point1 - normalVector, point2 - normalVector, point3 - normalVector, point4 - normalVector, color);

			DrawQuadPolygonInternal(point3 - normalVector, point1 + normalVector, point1 - normalVector, point3 + normalVector, color);
			DrawQuadPolygonInternal(point4 - normalVector, point2 + normalVector, point2 - normalVector, point4 + normalVector, color);

			DrawQuadPolygonInternal(point3 - normalVector, point2 + normalVector, point3 + normalVector, point2 - normalVector, color);
			DrawQuadPolygonInternal(point4 - normalVector, point1 + normalVector, point4 + normalVector, point1 - normalVector, color);
		}

		private static void DrawQuadPolygonInternal(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Color color)
		{
			DrawPolygon(point1, point2, point3, color);
			DrawPolygon(point2, point1, point4, color);
		}

		#endregion

		#region Raycasting

		/// <summary>
		/// Creates a raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="target">The target of the raycast.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
		public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectFlags options, Entity ignoreEntity = null)
		{
			return new RaycastResult(Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE,
				source.X,
				source.Y,
				source.Z,
				target.X,
				target.Y,
				target.Z,
				(int)options,
				ignoreEntity == null
					? 0
					: ignoreEntity.Handle,
				7));
		}
		/// <summary>
		/// Creates a raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="direction">The direction of the raycast.</param>
		/// <param name="maxDistance">How far the raycast should go out to.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectFlags options, Entity ignoreEntity = null)
		{
			Vector3 target = source + direction * maxDistance;

			return new RaycastResult(Function.Call<int>(Hash.START_EXPENSIVE_SYNCHRONOUS_SHAPE_TEST_LOS_PROBE,
				source.X,
				source.Y,
				source.Z,
				target.X,
				target.Y,
				target.Z,
				(int)options,
				ignoreEntity == null
					? 0
					: ignoreEntity.Handle,
				7));
		}

		/// <summary>
		/// Creates a 3D raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="target">The target of the raycast.</param>
		/// <param name="radius">The radius of the raycast.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
		[Obsolete("World.RaycastCapsule is obsolete because the result may not be made in the same frame you call the method. " +
		          "Use ShapeTest.StartTestCapsule instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectFlags options, Entity ignoreEntity = null)
		{
			return new RaycastResult(Function.Call<int>(Hash.START_SHAPE_TEST_CAPSULE,
				source.X,
				source.Y,
				source.Z,
				target.X,
				target.Y,
				target.Z,
				radius,
				(int)options,
				ignoreEntity == null
					? 0
					: ignoreEntity.Handle,
				7));
		}
		/// <summary>
		/// Creates a 3D raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="direction">The direction of the raycast.</param>
		/// <param name="radius">The radius of the raycast.</param>
		/// <param name="maxDistance">How far the raycast should go out to.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore, leave null for no entities ignored.</param>
		[Obsolete("World.RaycastCapsule is obsolete because the result may not be made in the same frame you call the method. " +
		          "Use ShapeTest.StartTestCapsule instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectFlags options, Entity ignoreEntity = null)
		{
			Vector3 target = source + direction * maxDistance;

			return new RaycastResult(Function.Call<int>(Hash.START_SHAPE_TEST_CAPSULE,
				source.X,
				source.Y,
				source.Z,
				target.X,
				target.Y,
				target.Z,
				radius,
				(int)options,
				ignoreEntity == null
					? 0
					: ignoreEntity.Handle,
				7));
		}

		/// <summary>
		/// Determines where the crosshair intersects with the world.
		/// </summary>
		/// <returns>A <see cref="RaycastResult"/> containing information about where the crosshair intersects with the world.</returns>
		public static RaycastResult GetCrosshairCoordinates()
		{
			return Raycast(GameplayCamera.Position, GameplayCamera.GetOffsetPosition(new Vector3(0f, 1000f, 0f)), IntersectFlags.Everything, null);
		}
		/// <summary>
		/// Determines where the crosshair intersects with the world.
		/// </summary>
		/// <param name="intersectOptions">Type of <see cref="IntersectFlags">environment</see> the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Prevent the raycast detecting a specific <see cref="Entity"/>.</param>
		/// <returns>A <see cref="RaycastResult"/> containing information about where the crosshair intersects with the world.</returns>
		public static RaycastResult GetCrosshairCoordinates(IntersectFlags intersectOptions = IntersectFlags.Everything, Entity ignoreEntity = null)
		{
			return Raycast(GameplayCamera.Position, GameplayCamera.GetOffsetPosition(new Vector3(0f, 1000f, 0f)), intersectOptions, ignoreEntity);
		}

		#endregion

		#region Positioning

		/// <summary>
		/// Gets the straight line distance between 2 positions.
		/// </summary>
		/// <param name="origin">The origin.</param>
		/// <param name="destination">The destination.</param>
		/// <returns>The distance.</returns>
		public static float GetDistance(Vector3 origin, Vector3 destination)
		{
			return Function.Call<float>(Hash.GET_DISTANCE_BETWEEN_COORDS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z, 1);
		}
		/// <summary>
		/// Calculates the travel distance using roads and paths between 2 positions.
		/// </summary>
		/// <param name="origin">The origin.</param>
		/// <param name="destination">The destination.</param>
		/// <returns>The travel distance.</returns>
		public static float CalculateTravelDistance(Vector3 origin, Vector3 destination)
		{
			return Function.Call<float>(Hash.CALCULATE_TRAVEL_DISTANCE_BETWEEN_POINTS, origin.X, origin.Y, origin.Z, destination.X, destination.Y, destination.Z);
		}

		/// <summary>
		/// Tries to store the z coordinate of the highest ground below the given point.
		/// Detects <see cref="Building"/>s and some of static <see cref="Prop"/>s that are on the ground,
		/// such as "prop_portacabin01" at (916.3843, -3242.021, 4.886292).
		/// </summary>
		/// <param name="position">The start position to test.</param>
		/// <param name="height">
		/// When this method returns, contains the Z coordinate of the highest ground below <paramref name="position"/>,
		/// if the highest ground is found. This parameter is passed uninitialized.
		/// </param>
		/// <param name="mode">
		/// The test mode. Does not make any difference in earlier game versions such as v1.0.372.2.
		/// </param>
		/// <returns><see langword="true"/> if it finds collision; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// This test does not include all kind of static <see cref="Prop"/>s, and this test excludes any fragment <see cref="Prop"/>s.
		/// </remarks>
		public static bool GetGroundHeight(Vector3 position, out float height, GetGroundHeightMode mode = GetGroundHeightMode.Normal)
		{
			bool foundCollision;

			// ignoreDistToWaterLevelCheck (the original name of 6th arg) will be ignored if waterAsGround (the original name of 5th arg) is not set
			bool waterAsGround = false;
			bool ignoreDistToWaterLevelCheck = false;
			switch (mode)
			{
				case GetGroundHeightMode.ConsiderWaterAsGround:
					waterAsGround = true;
					break;
				case GetGroundHeightMode.ConsiderWaterAsGroundNoWaves:
					waterAsGround = true;
					ignoreDistToWaterLevelCheck = true;
					break;
				case GetGroundHeightMode.Normal:
				default:
					break;
			}

			unsafe
			{
				float returnZ;
				foundCollision = Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &returnZ, waterAsGround, ignoreDistToWaterLevelCheck);
				height = returnZ;
			}
			return foundCollision;
		}
		/// <summary>
		/// Tries to store the z coordinate and surface normal of the highest ground below the given point.
		/// Detects <see cref="Building"/>s and some of static <see cref="Prop"/>s that are on the ground,
		/// such as "prop_portacabin01" at (916.3843, -3242.021, 4.886292).
		/// </summary>
		/// <param name="position">The start position to test.</param>
		/// <param name="height">
		/// When this method returns, contains the Z coordinate of the highest ground below <paramref name="position"/>,
		/// if the highest ground is found. This parameter is passed uninitialized.
		/// </param>
		/// <param name="normal">
		/// When this method returns, contains the surface normal of the highest ground below <paramref name="position"/>,
		/// if the highest ground is found. This parameter is passed uninitialized.
		/// </param>
		/// <returns><see langword="true"/> if it finds collision; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// This test does not include all kind of static <see cref="Prop"/>s, and this test excludes any fragment <see cref="Prop"/>s.
		/// </remarks>
		public static bool GetGroundHeightAndNormal(Vector3 position, out float height, out Vector3 normal)
		{
			bool foundCollision;

			unsafe
			{
				float returnZ;
				NativeVector3 outNormal;
				foundCollision = Function.Call<bool>(Hash.GET_GROUND_Z_AND_NORMAL_FOR_3D_COORD, position.X, position.Y, position.Z, &returnZ, &outNormal);
				height = returnZ;
				normal = outNormal;
			}

			return foundCollision;
		}
		/// <summary>
		/// <para>
		/// Tries to store the z coordinate of the highest ground below the given point.
		/// This test excludes any <see cref="Prop"/>s that are on the ground (so only <see cref="Building"/>s can be detected).
		/// </para>
		/// <para>
		/// Not available in v1.0.463.1 or earlier game versions (currently).
		/// </para>
		/// </summary>
		/// <param name="position">The start position to test.</param>
		/// <param name="height">
		/// When this method returns, contains the Z coordinate of the highest ground below <paramref name="position"/>,
		/// if the highest ground is found. This parameter is passed uninitialized.
		/// </param>
		/// <param name="mode">
		/// The test mode. May not make any difference in very earlier game versions (not confirmed if there are any).
		/// </param>
		/// <returns><see langword="true"/> if it finds collision; otherwise, <see langword="false"/>.</returns>
		/// <remarks>
		/// This test excludes any fragment <see cref="Prop"/>s.
		/// </remarks>
		/// <exception cref="GameVersionNotSupportedException">Thrown when called in v1.0.463.1 or earlier game versions.</exception>
		public static bool GetGroundHeightExcludingProps(Vector3 position, out float height, GetGroundHeightMode mode = GetGroundHeightMode.Normal)
		{
			if (Game.Version < GameVersion.v1_0_505_2_Steam)
			{
				GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_505_2_Steam), nameof(GetGroundHeightExcludingProps), nameof(GetGroundHeightExcludingProps));
			}

			bool foundCollision;

			// ignoreDistToWaterLevelCheck (the original name of 6th arg) will be ignored if waterAsGround (the original name of 5th arg) is not set
			bool waterAsGround = false;
			bool ignoreDistToWaterLevelCheck = false;
			switch (mode)
			{
				case GetGroundHeightMode.ConsiderWaterAsGround:
					waterAsGround = true;
					break;
				case GetGroundHeightMode.ConsiderWaterAsGroundNoWaves:
					waterAsGround = true;
					ignoreDistToWaterLevelCheck = true;
					break;
				case GetGroundHeightMode.Normal:
				default:
					break;
			}

			unsafe
			{
				float returnZ;
				foundCollision = Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &returnZ, waterAsGround, ignoreDistToWaterLevelCheck);
				height = returnZ;
			}

			return foundCollision;
		}

		/// <summary>
		/// Gets the height of the ground at a given position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>The height measured in meters</returns>
		[Obsolete("Use GetGroundHeight(Vector3, out float, GetGroundHeightMode) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static float GetGroundHeight(Vector2 position)
		{
			return GetGroundHeight(new Vector3(position.X, position.Y, 1000f));
		}
		/// <summary>
		/// Gets the height of the ground at a given position.
		/// Note : If the Vector3 is already below the ground, this will return 0.
		/// You may want to use the other overloaded function to be safe.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>The height measured in meters</returns>
		[Obsolete("Use GetGroundHeight(Vector3, out float, GetGroundHeightMode) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public static float GetGroundHeight(Vector3 position)
		{
			float resultArg;
			unsafe
			{
				Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &resultArg, false);
			}
			return resultArg;
		}

		/// <summary>
		/// Returns an approximate height at the 2d coordinate in meters.
		/// This is based on a coarse grid compiled from collision data.
		/// </summary>
		/// <remarks>A coarse grids has a 50-meter x 50-meter tile.</remarks>
		public static float GetApproxHeightForPoint(Vector2 position)
			=> Function.Call<float>(Hash.GET_APPROX_HEIGHT_FOR_POINT, position.X, position.Y);
		/// <summary>
		/// Returns an approximate height for the area in meters.
		/// This is based on a coarse grid compiled from collision data.
		/// </summary>
		/// <returns>The approximate height for the area, which is the maximum height in that area.</returns>
		/// <remarks>A coarse grids has a 50-meter x 50-meter tile.</remarks>
		public static float GetApproxHeightForArea(Vector2 minPosition, Vector2 maxPosition)
			=> Function.Call<float>(Hash.GET_APPROX_HEIGHT_FOR_AREA, minPosition.X, minPosition.Y, maxPosition.X,
				maxPosition.Y);
		/// <summary>
		/// Returns an approximate floor at the 2d coordinate in meters.
		/// This is based on a coarse grid compiled from collision data.
		/// </summary>
		/// <remarks>A coarse grids has a 50-meter x 50-meter tile.</remarks>
		public static float GetApproxFloorForPoint(Vector2 position)
			=> Function.Call<float>(Hash.GET_APPROX_FLOOR_FOR_POINT, position.X, position.Y);
		/// <summary>
		/// Returns an approximate floor for the area in meters.
		/// This is based on a coarse grid compiled from collision data.
		/// </summary>
		/// <returns>The approximate floor for the area, which is the maximum height in that area.</returns>
		/// <remarks>A coarse grids has a 50-meter x 50-meter tile.</remarks>
		public static float GetApproxFloorForArea(Vector2 minPosition, Vector2 maxPosition)
			=> Function.Call<float>(Hash.GET_APPROX_FLOOR_FOR_AREA, minPosition.X, minPosition.Y, maxPosition.X,
				maxPosition.Y);

		/// <summary>
		/// Gets the height of the water below the position including the waves.
		/// This method takes the waves into account so the result may be different depending on the exact frame of calling.
		/// </summary>
		/// <returns></returns>
		public static bool GetWaterHeight(Vector3 position, out float height)
		{
			unsafe
			{
				float returnZ;
				bool foundWater =
					Function.Call<bool>(Hash.GET_WATER_HEIGHT, position.X, position.Y, position.Z, &returnZ);

				height = returnZ;
				return foundWater;
			}
		}
		/// <summary>
		/// Gets the height of the water below the position excluding the waves.
		/// This method does not take the waves into account so the result will be the same between different frames.
		/// </summary>
		/// <returns></returns>
		public static bool GetWaterHeightNoWaves(Vector3 position, out float height)
		{
			unsafe
			{
				float returnZ;
				bool foundWater = Function.Call<bool>(Hash.GET_WATER_HEIGHT_NO_WAVES, position.X, position.Y,
					position.Z, &returnZ);

				height = returnZ;
				return foundWater;
			}
		}

		/// <summary>
		/// Test a directed probe against the water.
		/// </summary>
		/// <param name="startPos">The start of the probe.</param>
		/// <param name="endPos">The end of the probe.</param>
		/// <param name="intersectionPos">
		/// When this method returns, contains the intersection position on the water, the probe hits water before
		/// hitting land. This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the probe hits water before hitting land; otherwise, <see langword="false"/>.
		/// </returns>
		public static bool TestProbeAgainstWater(Vector3 startPos, Vector3 endPos, out Vector3 intersectionPos)
		{
			unsafe
			{
				NativeVector3 outPos;
				bool hitWaterBeforeLand = Function.Call<bool>(Hash.TEST_PROBE_AGAINST_WATER, startPos.X, startPos.Y,
					startPos.Z, endPos.X, endPos.Y, endPos.Z,  &outPos);

				intersectionPos = outPos;
				return hitWaterBeforeLand;
			}
		}

		/// <summary>
		/// Checks to see if it can find a safe bit of ground to place a <see cref="Ped"/>.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="safePosition">If successful, will be filled with the position to check around.</param>
		/// <param name="flags">The flags to determine how the method searches for positions.</param>
		/// <returns>
		/// <see langword="true"/> if successfully found a safe bit of ground to place a <see cref="Ped"/>; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// Use this carefully since it can have a considerable performance hit, having to stall the game whilst it queries navmesh polygons.
		/// </remarks>
		public static bool GetSafePositionForPed(Vector3 position, out Vector3 safePosition, GetSafePositionFlags flags = GetSafePositionFlags.Default)
		{
			NativeVector3 outPos;
			unsafe
			{
				// the 4th position sets the internal initial bitflag value to 3 instead of 2 if set,
				// making it like the 1nd bit flag of 6th parameter is always set
				bool foundSafePos = Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED,
					position.X,
					position.Y,
					position.Z,
					false,
					&outPos,
					(int)flags);
				safePosition = outPos;
				return foundSafePos;
			}
		}

		/// <summary>
		/// Gets the nearest safe coordinate to position a <see cref="Ped"/>.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="sidewalk">if set to <see langword="true" /> Only find positions on the sidewalk.</param>
		/// <param name="flags">The flags.</param>
		[Obsolete("World.GetSafeCoordForPed is obsolete since there is no way to check if the method is failed while GET_SAFE_COORD_FOR_PED provides one." +
		"Use GetSafePositionForPed instead."), EditorBrowsable(EditorBrowsableState.Never)]
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk = true, int flags = 0)
		{
			NativeVector3 outPos;
			unsafe
			{
				if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, sidewalk, &outPos, flags))
				{
					return outPos;
				}
			}
			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed.  Considers switched off nodes, where ambient vehicles will not spawn.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="unoccupied">if set to <see langword="true" /> only find positions that dont already have a vehicle in them.</param>
		public static Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied = false)
		{
			return GetNextPositionOnStreet(new Vector3(position.X, position.Y, 0f), unoccupied);
		}
		/// <summary>
		/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed. Considers switched off nodes, where ambient vehicles will not spawn.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="unoccupied">if set to <see langword="true" /> only find positions that dont already have a vehicle in them.</param>
		public static Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied = false)
		{
			if (unoccupied)
			{
				for (int i = 1; i < 40; i++)
				{
					if (!PathFind.GetNthClosestVehicleNodePosition(position, i, out position, GetClosestVehicleNodeFlags.IncludeSwitchedOffNodes))
					{
						continue;
					}

					if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, position.X, position.Y, position.Z, 5.0f,
						5.0f, 5.0f, 0))
					{
						return position;
					}
				}
			}
			else if (!PathFind.GetNthClosestVehicleNodePosition(position, 1, out position, GetClosestVehicleNodeFlags.IncludeSwitchedOffNodes))
			{
				return position;
			}

			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the next position on the street where a <see cref="Ped"/> can be placed.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		public static Vector3 GetNextPositionOnSidewalk(Vector2 position)
		{
			return GetNextPositionOnSidewalk(new Vector3(position.X, position.Y, 0f));
		}
		/// <summary>
		/// Gets the next position on the street where a <see cref="Ped"/> can be placed.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		public static Vector3 GetNextPositionOnSidewalk(Vector3 position)
		{
			NativeVector3 outPos;

			unsafe
			{
				if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, true, &outPos, 0))
				{
					return outPos;
				}
				else if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, false, &outPos, 0))
				{
					return outPos;
				}
			}

			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the nearest roadside point to the given coordinates. Returns true on success.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="direction">Which direction to check relative to the given position.</param>
		/// <param name="output">The variable which will receive the output position of this function.</param>
		public static bool GetPositionOnRoadside(Vector3 position, Direction direction, out Vector3 output)
		{
			Vector3 ret;
			bool result;
			unsafe
			{
				result = Function.Call<bool>(Hash.GET_POSITION_BY_SIDE_OF_ROAD, position.X, position.Y, position.Z, (int)direction, &ret);
			}
			output = ret;
			return result;
		}

		/// <summary>
		/// Determines the name of the street which is the closest to the given coordinates.
		/// </summary>
		public static string GetStreetName(Vector2 position)
		{
			return GetStreetName(new Vector3(position.X, position.Y, 0f));
		}
		/// <summary>
		/// Determines the name of the street which is the closest to the given coordinates.
		/// </summary>
		public static string GetStreetName(Vector3 position)
		{
			int streetHash, crossingHash;
			unsafe
			{
				Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, &streetHash, &crossingHash);
			}
			return Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, streetHash);
		}
		/// <summary>
		/// Determines the name of the street which is the closest to the given coordinates.
		/// </summary>
		/// <param name="position">The coordinates of the street</param>
		/// <param name="crossingRoadName">If the coordinates are on an intersection, the name of the crossing road</param>
		/// <returns>Returns the name of the street the coordinates are on.</returns>
		public static string GetStreetName(Vector3 position, out string crossingRoadName)
		{
			int streetHash, crossingHash;
			unsafe
			{
				Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, &streetHash, &crossingHash);
			}
			crossingRoadName = Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, crossingHash);
			return Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, streetHash);
		}

		/// <summary>
		/// Gets the display name of the a zone in the map.
		/// Use <see cref="Game.GetLocalizedString(string)"/> to convert to the localized name.
		/// </summary>
		/// <param name="position">The position on the map.</param>
		public static string GetZoneDisplayName(Vector2 position)
		{
			return GetZoneDisplayName(new Vector3(position.X, position.Y, 0f));
		}
		/// <summary>
		/// Gets the display name of the a zone in the map.
		/// Use <see cref="Game.GetLocalizedString(string)"/> to convert to the localized name.
		/// </summary>
		/// <param name="position">The position on the map.</param>
		public static string GetZoneDisplayName(Vector3 position)
		{
			return Function.Call<string>(Hash.GET_NAME_OF_ZONE, position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Gets the localized name of the a zone in the map.
		/// </summary>
		/// <param name="position">The position on the map.</param>
		public static string GetZoneLocalizedName(Vector2 position)
		{
			return GetZoneLocalizedName(new Vector3(position.X, position.Y, 0f));
		}
		/// <summary>
		/// Gets the localized name of the a zone in the map.
		/// </summary>
		/// <param name="position">The position on the map.</param>
		public static string GetZoneLocalizedName(Vector3 position)
		{
			return Game.GetLocalizedString(GetZoneDisplayName(position));
		}

		/// <summary>
		/// Determines whether the specified point is in the angled area.
		/// An angled area is an X-Z oriented rectangle with three parameters: origin, extent, and width.
		/// </summary>
		/// <param name="point">The point to check whether is in the angled area.</param>
		/// <param name="originEdge">The mid-point along a base edge of the rectangle.</param>
		/// <param name="extentEdge">The mid-point of opposite base edge on the other Z.</param>
		/// <param name="width">The length of the base edge.</param>
		/// <param name="includeZAxis">
		/// If set to <see langword="true" />, the method will also check if the point is in area in Z axis as well as X and Y axes.
		/// If set to <see langword="false" />, the method will only check if the point is in area in X and Y axes.
		/// </param>
		///  <returns>
		///   <see langword="true" /> if the specified point is in the specified angled area; otherwise, <see langword="false" />.
		/// </returns>
		public static bool IsPointInAngledArea(Vector3 point, Vector3 originEdge, Vector3 extentEdge, float width, bool includeZAxis = true)
		{
			return Function.Call<bool>(Hash.IS_POINT_IN_ANGLED_AREA, point.X, point.Y, point.Z, originEdge.X, originEdge.Y, originEdge.Z, extentEdge.X, extentEdge.Y, extentEdge.Z, width, false, includeZAxis);
		}

		#endregion
	}
}
