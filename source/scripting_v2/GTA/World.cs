//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace GTA
{
	public static class World
	{
		#region Fields

		private static readonly string[] weatherNames = {
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

		private static readonly GregorianCalendar calendar = new();
		#endregion

		#region Time & Day

		/// <summary>
		/// Gets or sets the current date and time in the GTA World.
		/// </summary>
		/// <value>
		/// The current date and time.
		/// </value>
		public static DateTime CurrentDate
		{
			get
			{
				int year = Function.Call<int>(Hash.GET_CLOCK_YEAR);
				int month = Function.Call<int>(Hash.GET_CLOCK_MONTH) + 1;
				int day = System.Math.Min(Function.Call<int>(Hash.GET_CLOCK_DAY_OF_MONTH), calendar.GetDaysInMonth(year, month));
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

		/// <summary>
		/// Gets or sets the current time of day in the GTA World.
		/// </summary>
		/// <value>
		/// The current time of day
		/// </value>
		public static TimeSpan CurrentDayTime
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

		#endregion

		#region Weather & Effects

		/// <summary>
		/// Sets a value indicating whether artificial lights in the <see cref="World"/> should be rendered.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if blackout; otherwise, <see langword="false" />.
		/// </value>
		public static void SetBlackout(bool enable)
		{
			Function.Call(Hash._SET_BLACKOUT, enable);
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
				for (int i = 0; i < weatherNames.Length; i++)
				{
					if (Function.Call<int>(Hash._GET_CURRENT_WEATHER_TYPE) == Game.GenerateHash(weatherNames[i]))
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
					Function.Call(Hash.SET_WEATHER_TYPE_NOW, weatherNames[(int)value]);
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
				for (int i = 0; i < weatherNames.Length; i++)
				{
					if (Function.Call<bool>(Hash.IS_NEXT_WEATHER_TYPE, weatherNames[i]))
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

				int currentWeatherHash;
				unsafe
				{
					int nextWeatherHash;
					float weatherTransition;
					Function.Call(Hash._GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
				}
				Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, currentWeatherHash, Game.GenerateHash(weatherNames[(int)value]), 0.0f);
			}
		}

		/// <summary>
		/// Transitions to weather.
		/// </summary>
		/// <param name="value">The weather.</param>
		/// <param name="duration">The duration.</param>
		public static void TransitionToWeather(Weather value, float duration)
		{
			if (Enum.IsDefined(value.GetType(), value) && value != Weather.Unknown)
			{
				Function.Call(Hash._SET_WEATHER_TYPE_OVER_TIME, weatherNames[(int)value], duration);
			}
		}

		public static float WeatherTransition
		{
			get
			{
				float weatherTransition;
				unsafe
				{
					int currentWeatherHash, nextWeatherHash;
					Function.Call(Hash._GET_WEATHER_TYPE_TRANSITION, &currentWeatherHash, &nextWeatherHash, &weatherTransition);
				}
				return weatherTransition;
			}
			set => Function.Call(Hash._SET_WEATHER_TYPE_TRANSITION, 0, 0, value);
		}

		/// <summary>
		/// Sets the gravity level for all <see cref="World"/> objects.
		/// </summary>
		/// <value>
		/// The gravity level:
		/// 0 - Default gravity (9.8f).
		/// 1 - Moon gravity (2.4f).
		/// 2 - Very low gravity (0.1f).
		/// 3 - No gravity (0.0f).
		/// </value>
		public static int GravityLevel
		{
			set => Function.Call(Hash.SET_GRAVITY_LEVEL, value);
		}

		#endregion

		#region Blips

		/// <summary>
		/// Gets the waypoint position.
		/// </summary>
		/// <returns>The <see cref="Vector3"/> coordinates of the Waypoint <see cref="Blip"/></returns>
		/// <remarks>
		/// Returns an empty <see cref="Vector3"/> if a waypoint <see cref="Blip"/> hasn't been set
		/// If the game engine cant extract height information the Z component will be 0.0f
		/// </remarks>
		public static Vector3 GetWaypointPosition()
		{
			if (!Game.IsWaypointActive)
			{
				return Vector3.Zero;
			}

			bool blipFound = false;
			Vector3 position = Vector3.Zero;

			int waypointBlipHandle = SHVDN.NativeMemory.GetWaypointBlip();

			if (waypointBlipHandle != 0)
			{
				position = Function.Call<Vector3>(Hash.GET_BLIP_INFO_ID_COORD, waypointBlipHandle);
				blipFound = true;
			}

			if (!blipFound)
			{
				return position;
			}

			bool groundFound = false;
			float height = 0.0f;

			for (int i = 800; i >= 0; i -= 50)
			{
				unsafe
				{
					if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, (float)i, &height))
					{
						groundFound = true;
						position.Z = height;
						break;
					}
				}

				Script.Wait(100);
			}

			if (!groundFound)
			{
				position.Z = 1000.0f;
			}

			return position;
		}

		/// <summary>
		/// Creates a <see cref="Blip"/> at the given position on the map.
		/// </summary>
		/// <param name="position">The position of the blip on the map.</param>
		public static Blip CreateBlip(Vector3 position)
		{
			return Function.Call<Blip>(Hash.ADD_BLIP_FOR_COORD, position.X, position.Y, position.Z);
		}
		/// <summary>
		/// Creates a <see cref="Blip"/> for a circular area at the given position on the map.
		/// </summary>
		/// <param name="position">The position of the blip on the map.</param>
		/// <param name="radius">The radius of the area on the map.</param>
		public static Blip CreateBlip(Vector3 position, float radius)
		{
			return Function.Call<Blip>(Hash.ADD_BLIP_FOR_RADIUS, position.X, position.Y, position.Z, radius);
		}

		#endregion

		#region Entities

		/// <summary>
		/// Gets an <c>array</c>of all <see cref="Ped"/>s in the World.
		/// </summary>
		public static Ped[] GetAllPeds()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(), handle => new Ped(handle));
		}

		/// <summary>
		/// Gets an <c>array</c>of all <see cref="Ped"/>s in the World.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of <see cref="Ped"/>s to get.</param>
		public static Ped[] GetAllPeds(Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(new[] { model.Hash }), handle => new Ped(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Ped"/>s near a given <see cref="Ped"/> in the world
		/// </summary>
		/// <param name="ped">The ped to check.</param>
		/// <param name="radius">The maximum distance from the <paramref name="ped"/> to detect <see cref="Ped"/>s.</param>
		/// <remarks>Doesnt include the <paramref name="ped"/> in the result</remarks>

		public static Ped[] GetNearbyPeds(Ped ped, float radius)
		{
			int[] handles = SHVDN.NativeMemory.GetPedHandles(ped.Position.ToInternalFVector3(), radius);

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
		public static Ped[] GetNearbyPeds(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(position.ToInternalFVector3(), radius), handle => new Ped(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Ped"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Ped"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Ped"/>s.</param>
		/// <param name="model">The <see cref="Model"/> of <see cref="Ped"/>s to get.</param>
		public static Ped[] GetNearbyPeds(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(position.ToInternalFVector3(), radius, new[] { model.Hash }), handle => new Ped(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s in the World.
		/// </summary>
		public static Vehicle[] GetAllVehicles()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(), handle => new Vehicle(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s in the World.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of <see cref="Vehicle"/>s to get.</param>
		public static Vehicle[] GetAllVehicles(Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(new[] { model.Hash }), handle => new Vehicle(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s near a given <see cref="Ped"/> in the world
		/// </summary>
		/// <param name="ped">The ped to check.</param>
		/// <param name="radius">The maximum distance from the <paramref name="ped"/> to detect <see cref="Vehicle"/>s.</param>
		/// <remarks>Doesnt include the <see cref="Vehicle"/> the <paramref name="ped"/> is using in the result</remarks>

		public static Vehicle[] GetNearbyVehicles(Ped ped, float radius)
		{
			int[] handles = SHVDN.NativeMemory.GetVehicleHandles(ped.Position.ToInternalFVector3(), radius);

			if (handles.Length == 0)
			{
				return Array.Empty<Vehicle>();
			}

			var result = new List<Vehicle>(handles.Length - 1);
			Vehicle ignore = ped.CurrentVehicle;
			int ignoreHandle = Entity.Exists(ignore) ? ignore.Handle : 0;

			foreach (int handle in handles)
			{
				if (handle == ignoreHandle)
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
		public static Vehicle[] GetNearbyVehicles(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(position.ToInternalFVector3(), radius), handle => new Vehicle(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Vehicle"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Vehicle"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Vehicle"/>s.</param>
		/// <param name="model">The <see cref="Model"/> of <see cref="Vehicle"/>s to get.</param>
		public static Vehicle[] GetNearbyVehicles(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(position.ToInternalFVector3(), radius, new[] { model.Hash }), handle => new Vehicle(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in the World.
		/// </summary>
		public static Prop[] GetAllProps()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(), handle => new Prop(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in the World.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of <see cref="Prop"/>s to get.</param>
		public static Prop[] GetAllProps(Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(new[] { model.Hash }), handle => new Prop(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Prop"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Prop"/>s.</param>
		public static Prop[] GetNearbyProps(Vector3 position, float radius)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(position.ToInternalFVector3(), radius), handle => new Prop(handle));
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Prop"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Prop"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Prop"/>s.</param>
		/// <param name="model">The <see cref="Model"/> of <see cref="Prop"/>s to get.</param>
		public static Prop[] GetNearbyProps(Vector3 position, float radius, Model model)
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetPropHandles(position.ToInternalFVector3(), radius, new[] { model.Hash }), handle => new Prop(handle));
		}

		/// <summary>
		/// Gets an <c>array</c> of all the <see cref="Blip"/>s on the map.
		/// </summary>
		public static Blip[] GetActiveBlips()
		{
			return Array.ConvertAll(SHVDN.NativeMemory.GetNonCriticalRadarBlipHandles(), handle => new Blip(handle));
		}

		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Entity"/>s in the World.
		/// </summary>
		public static Entity[] GetAllEntities()
		{
			return Array.ConvertAll<int, Entity>(SHVDN.NativeMemory.GetEntityHandles(), Entity.FromHandle);
		}
		/// <summary>
		/// Gets an <c>array</c> of all <see cref="Entity"/>s in a given region in the World.
		/// </summary>
		/// <param name="position">The position to check the <see cref="Entity"/> against.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Entity"/>s.</param>
		public static Entity[] GetNearbyEntities(Vector3 position, float radius)
		{
			return Array.ConvertAll<int, Entity>(SHVDN.NativeMemory.GetEntityHandles(position.ToInternalFVector3(), radius), Entity.FromHandle);
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
			return (T)closest;
		}
		/// <summary>
		/// Gets the closest <see cref="Ped"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Ped"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Ped"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Ped"/> was in the given region.</remarks>
		public static Ped GetClosestPed(Vector3 position, float radius)
		{
			Ped[] peds = Array.ConvertAll(SHVDN.NativeMemory.GetPedHandles(position.ToInternalFVector3(), radius), handle => new Ped(handle));
			return GetClosest(position, peds);
		}
		/// <summary>
		/// Gets the closest <see cref="Vehicle"/> to a given position in the World.
		/// </summary>
		/// <param name="position">The position to find the nearest <see cref="Vehicle"/>.</param>
		/// <param name="radius">The maximum distance from the <paramref name="position"/> to detect <see cref="Vehicle"/>s.</param>
		/// <remarks>Returns <see langword="null" /> if no <see cref="Vehicle"/> was in the given region.</remarks>
		public static Vehicle GetClosestVehicle(Vector3 position, float radius)
		{
			Vehicle[] vehicles = Array.ConvertAll(SHVDN.NativeMemory.GetVehicleHandles(position.ToInternalFVector3(), radius), handle => new Vehicle(handle));
			return GetClosest(position, vehicles);

		}

		/// <summary>
		/// A fast way to get the total number of <see cref="Vehicle"/>s spawned in the world.
		/// </summary>
		private static int VehicleCount => SHVDN.NativeMemory.GetVehicleCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="Ped"/>s spawned in the world.
		/// </summary>
		private static int PedCount => SHVDN.NativeMemory.GetPedCount();
		/// <summary>
		/// A fast way to get the total number of <see cref="Prop"/>s spawned in the world.
		/// </summary>
		private static int PropCount => SHVDN.NativeMemory.GetObjectCount();

		/// <summary>
		/// The total number of <see cref="Vehicle"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Vehicle"/> is the same as this limit and the game tries to create a <see cref="Vehicle"/>.</remarks>
		private static int VehicleCapacity => SHVDN.NativeMemory.GetVehicleCapacity();
		/// <summary>
		/// The total number of <see cref="Ped"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Ped"/> is the same as this limit and the game tries to create a <see cref="Ped"/>.</remarks>
		private static int PedCapacity => SHVDN.NativeMemory.GetPedCapacity();
		/// <summary>
		/// The total number of <see cref="Prop"/>s that can exist in the world.
		/// </summary>
		/// <remarks>The game will crash when the number of <see cref="Prop"/> is the same as this limit and the game tries to create a <see cref="Prop"/>.</remarks>
		private static int PropCapacity => SHVDN.NativeMemory.GetObjectCapacity();

		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Ped"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Ped CreatePed(Model model, Vector3 position)
		{
			return CreatePed(model, position, 0.0f);
		}
		/// <summary>
		/// Spawns a <see cref="Ped"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Ped"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Ped"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Ped"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Ped"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Ped CreatePed(Model model, Vector3 position, float heading)
		{
			if (PedCount >= PedCapacity || !model.IsPed || !model.Request(1000))
			{
				return null;
			}

			// The first parameter "PedType" does not have actual effect, the function always eventually uses the "Pedtype"
			// value for the model (in peds.ymt or peds.meta) instead
			// Actually the value is read when the function is called but eventually overwritten before getting used in a meaningful way
			return Function.Call<Ped>(Hash.CREATE_PED, 26, model.Hash, position.X, position.Y, position.Z, heading, false, false);
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

			return Function.Call<Ped>(Hash.CREATE_RANDOM_PED, position.X, position.Y, position.Z);
		}

		/// <summary>
		/// Spawns a <see cref="Vehicle"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Vehicle"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Vehicle"/> at.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Vehicle"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Vehicle CreateVehicle(Model model, Vector3 position)
		{
			return CreateVehicle(model, position, 0.0f);
		}
		/// <summary>
		/// Spawns a <see cref="Vehicle"/> of the given <see cref="Model"/> at the position and heading specified.
		/// </summary>
		/// <param name="model">The <see cref="Model"/> of the <see cref="Vehicle"/>.</param>
		/// <param name="position">The position to spawn the <see cref="Vehicle"/> at.</param>
		/// <param name="heading">The heading of the <see cref="Vehicle"/>.</param>
		/// <remarks>returns <see langword="null" /> if the <see cref="Vehicle"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
		public static Vehicle CreateVehicle(Model model, Vector3 position, float heading)
		{
			if (VehicleCount >= VehicleCapacity || !model.IsVehicle || !model.Request(1000))
			{
				return null;
			}

			return Function.Call<Vehicle>(Hash.CREATE_VEHICLE, model.Hash, position.X, position.Y, position.Z, heading, false, false);
		}

		/// <inheritdoc cref="CreateProp(Model, Vector3, Vector3, bool, bool)"/>
		public static Prop CreateProp(Model model, Vector3 position, bool dynamic, bool placeOnGround)
		{
			if (PropCount >= PropCapacity)
			{
				return null;
			}

			if (placeOnGround)
			{
				position.Z = GetGroundHeight(position);
			}

			if (!model.Request(1000))
			{
				return null;
			}

			return Function.Call<Prop>(Hash.CREATE_OBJECT, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic);
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
			Prop p = CreateProp(model, position, dynamic, placeOnGround);

			if (p == null)
			{
				return null;
			}

			p.Rotation = rotation;

			return p;
		}

		/// <summary>
		/// Spawns a pickup <see cref="Prop"/> at the specified position.
		/// </summary>
		public static Prop CreateAmbientPickup(PickupType type, Vector3 position, Model model, int value)
		{
			if (!model.Request(1000))
			{
				return null;
			}

			int handle = Function.Call<int>(Hash.CREATE_AMBIENT_PICKUP, (int)type, position.X, position.Y, position.Z, 0, value, model.Hash, false, true);
			if (handle == 0)
			{
				return null;
			}

			return new Prop(handle);
		}
		/// <summary>
		/// Spawns a <see cref="Pickup"/> at the specified position.
		/// </summary>
		public static Pickup CreatePickup(PickupType type, Vector3 position, Model model, int value)
		{
			if (!model.Request(1000))
			{
				return null;
			}

			int handle = Function.Call<int>(Hash.CREATE_PICKUP, (int)type, position.X, position.Y, position.Z, 0, value, true, model.Hash);
			if (handle == 0)
			{
				return null;
			}

			return new Pickup(handle);
		}
		/// <summary>
		/// Spawns a <see cref="Pickup"/> at the specified position.
		/// </summary>
		public static Pickup CreatePickup(PickupType type, Vector3 position, Vector3 rotation, Model model, int value)
		{
			if (!model.Request(1000))
			{
				return null;
			}

			int handle = Function.Call<int>(Hash.CREATE_PICKUP_ROTATE, (int)type, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, value, 2, true, model.Hash);
			if (handle == 0)
			{
				return null;
			}

			return new Pickup(handle);
		}

		#endregion

		#region Cameras

		/// <summary>
		/// Destroys all user created <see cref="Camera"/>s.
		/// </summary>
		public static void DestroyAllCameras()
		{
			Function.Call(Hash.DESTROY_ALL_CAMS, 0);
		}

		/// <summary>
		/// Creates a <see cref="Camera"/>, use <see cref="World.RenderingCamera"/> to switch to this camera
		/// </summary>
		/// <param name="position">The position of the camera.</param>
		/// <param name="rotation">The rotation of the camera.</param>
		/// <param name="fov">The field of view of the camera.</param>
		public static Camera CreateCamera(Vector3 position, Vector3 rotation, float fov)
		{
			return Function.Call<Camera>(Hash.CREATE_CAM_WITH_PARAMS, "DEFAULT_SCRIPTED_CAMERA", position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, 1, 2);
		}

		/// <summary>
		/// Gets or sets the rendering camera.
		/// </summary>
		/// <value>
		/// The rendering <see cref="Camera"/>.
		/// </value>
		/// <remarks>
		/// Setting to <see langword="null" /> sets the rendering <see cref="Camera"/> to <see cref="GameplayCamera"/>.
		/// </remarks>
		public static Camera RenderingCamera
		{
			get => new Camera(Function.Call<int>(Hash.GET_RENDERING_CAM));
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
			return Function.Call<Rope>(Hash.ADD_ROPE, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, length, (int)type, length, minLength, 0.5f, false, false, true, 1.0f, breakable, 0);
		}

		/// <inheritdoc cref="ShootBullet(Vector3, Vector3, Ped, Model, int, float)"/>
		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage)
		{
			ShootBullet(sourcePosition, targetPosition, owner, model, damage, -1.0f);
		}
		/// <summary>
		/// Fires a single bullet in the world.
		/// </summary>
		/// <param name="sourcePosition">Where the bullet is fired from.</param>
		/// <param name="targetPosition">Where the bullet is fired to.</param>
		/// <param name="owner">The <see cref="Ped"/> who fired the bullet, leave <see langword="null" /> for no one.</param>
		/// <param name="model">The weapon hash that the bullet is fired from. The <see cref="Model"/> type is incorrectly used here.</param>
		/// <param name="damage">The damage the bullet will cause.</param>
		/// <param name="speed">The speed, only affects projectile weapons, leave -1 for default.</param>

		public static void ShootBullet(Vector3 sourcePosition, Vector3 targetPosition, Ped owner, Model model, int damage, float speed)
		{
			Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, sourcePosition.X, sourcePosition.Y, sourcePosition.Z, targetPosition.X, targetPosition.Y, targetPosition.Z, damage, 1, model.Hash, owner.Handle, 1, 0, speed);
		}

		/// <summary>
		/// Creates an explosion in the world.
		/// </summary>
		/// <param name="position">The position of the explosion.</param>
		/// <param name="type">The type of explosion.</param>
		/// <param name="radius">The radius of the explosion.</param>
		/// <param name="cameraShake">The amount of camera shake to apply to nearby cameras.</param>
		public static void AddExplosion(Vector3 position, ExplosionType type, float radius, float cameraShake)
		{
			Function.Call(Hash.ADD_EXPLOSION, position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		/// <summary>
		/// Creates an explosion in the world.
		/// </summary>
		/// <param name="position">The position of the explosion.</param>
		/// <param name="type">The type of explosion.</param>
		/// <param name="radius">The radius of the explosion.</param>
		/// <param name="cameraShake">The amount of camera shake to apply to nearby cameras.</param>
		/// <param name="Aubidble">If set to <see langword="true" />, explosion can be heard.</param>
		/// <param name="Invis">If set to <see langword="true" />, explosion will not create particle effects.</param>

		public static void AddExplosion(Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			Function.Call(Hash.ADD_EXPLOSION, position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}
		/// <summary>
		/// Creates an explosion in the world owned by the specified <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> who caused the explosion.</param>
		/// <param name="position">The position of the explosion.</param>
		/// <param name="type">The type of explosion.</param>
		/// <param name="radius">The radius of the explosion.</param>
		/// <param name="cameraShake">The amount of camera shake to apply to nearby cameras.</param>

		public static void AddOwnedExplosion(Ped ped, Vector3 position, ExplosionType type, float radius, float cameraShake)
		{
			Function.Call(Hash.ADD_OWNED_EXPLOSION, ped.Handle, position.X, position.Y, position.Z, (int)type, radius, true, false, cameraShake);
		}
		/// <summary>
		/// Creates an explosion in the world owned by the specified <see cref="Ped"/>.
		/// </summary>
		/// <param name="ped">The <see cref="Ped"/> who caused the explosion.</param>
		/// <param name="position">The position of the explosion.</param>
		/// <param name="type">The type of explosion.</param>
		/// <param name="radius">The radius of the explosion.</param>
		/// <param name="cameraShake">The amount of camera shake to apply to nearby cameras.</param>
		/// <param name="Aubidble">If set to <see langword="true" />, explosion can be heard.</param>
		/// <param name="Invis">If set to <see langword="true" />, explosion will not create particle effects.</param>

		public static void AddOwnedExplosion(Ped ped, Vector3 position, ExplosionType type, float radius, float cameraShake, bool Aubidble, bool Invis)
		{
			Function.Call(Hash.ADD_OWNED_EXPLOSION, ped.Handle, position.X, position.Y, position.Z, (int)type, radius, Aubidble, Invis, cameraShake);
		}

		/// <summary>
		/// Creates a relationship group with the given name.
		/// </summary>
		/// <param name="groupName">The name of the relationship group.</param>
		/// <returns>The hash of the created relationship group.</returns>
		public static int AddRelationshipGroup(string groupName)
		{
			int handle = 0;
			unsafe
			{
				Function.Call(Hash.ADD_RELATIONSHIP_GROUP, groupName, &handle);
			};
			return handle;
		}
		/// <summary>
		/// Removes a relationship group with the given name.
		/// </summary>
		/// <param name="group">The hash of the relationship group.</param>
		public static void RemoveRelationshipGroup(int group)
		{
			Function.Call(Hash.REMOVE_RELATIONSHIP_GROUP, group);
		}
		public static Relationship GetRelationshipBetweenGroups(int group1, int group2)
		{
			return (Relationship)Function.Call<int>(Hash.GET_RELATIONSHIP_BETWEEN_GROUPS, group1, group2);
		}
		public static void SetRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
		{
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.SET_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}
		public static void ClearRelationshipBetweenGroups(Relationship relationship, int group1, int group2)
		{
			Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group1, group2);
			Function.Call(Hash.CLEAR_RELATIONSHIP_BETWEEN_GROUPS, (int)relationship, group2, group1);
		}

		#endregion

		#region Drawing

		/// <inheritdoc cref="DrawMarker(MarkerType, Vector3, Vector3, Vector3, Vector3, Color, bool, bool, int, bool, string, string, bool)"/>
		public static void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color)
		{
			DrawMarker(type, pos, dir, rot, scale, color, false, false, 2, false, null, null, false);
		}
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
		/// <param name="faceCamY">if set to <see langword="true" /> the marker will always face the camera, regardless of its rotation.</param>
		/// <param name="unk2">The euler rotation order. You would like to set 2, which will rotate Y, X, Z axis in word space in that order.</param>
		/// <param name="rotateY">if set to <see langword="true" /> rotates only on the y axis(heading).</param>
		/// <param name="textueDict">Name of texture dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="textureName">Name of texture inside the dictionary to load the texture from, leave null for no texture in the marker.</param>
		/// <param name="drawOnEnt">if set to <see langword="true" /> draw on any <see cref="Entity"/> that intersects the marker.</param>
		public static void DrawMarker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color color, bool bobUpAndDown, bool faceCamY, int unk2, bool rotateY, string textueDict, string textureName, bool drawOnEnt)
		{
			InputArgument dict = new InputArgument(0), name = new InputArgument(0);

			if (textueDict != null && textureName != null)
			{
				if (textueDict.Length > 0 && textureName.Length > 0)
				{
					dict = new InputArgument(textueDict);
					name = new InputArgument(textureName);
				}
			}

			Function.Call(Hash.DRAW_MARKER, (int)type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, color.R, color.G, color.B, color.A, bobUpAndDown, faceCamY, unk2, rotateY, dict, name, drawOnEnt);
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
			Function.Call(Hash.DRAW_LIGHT_WITH_RANGE, position.X, position.Y, position.Z, color.R, color.G, color.B, range, intensity);
		}
		public static void DrawSpotLight(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout)
		{
			Function.Call(Hash.DRAW_SPOT_LIGHT, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
		}
		public static void DrawSpotLightWithShadow(Vector3 pos, Vector3 dir, Color color, float distance, float brightness, float roundness, float radius, float fadeout)
		{
			Function.Call(Hash._DRAW_SPOT_LIGHT_WITH_SHADOW, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, color.R, color.G, color.B, distance, brightness, roundness, radius, fadeout);
		}

		#endregion

		#region Raycasting

		/// <inheritdoc cref="Raycast(Vector3, Vector3, IntersectOptions, Entity)"/>
		public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectOptions options)
		{
			return Raycast(source, target, options, null);
		}
		/// <summary>
		/// Creates a raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="target">The target of the raycast.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore.</param>
		public static RaycastResult Raycast(Vector3 source, Vector3 target, IntersectOptions options, Entity ignoreEntity)
		{
			return new RaycastResult(Function.Call<int>(Hash._CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		/// <inheritdoc cref="Raycast(Vector3, Vector3, float, IntersectOptions, Entity)"/>
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options)
		{
			return Raycast(source, direction, maxDistance, options, null);
		}
		/// <summary>
		/// Creates a raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="direction">The direction of the raycast.</param>
		/// <param name="maxDistance">How far the raycast should go out to.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore.</param>
		public static RaycastResult Raycast(Vector3 source, Vector3 direction, float maxDistance, IntersectOptions options, Entity ignoreEntity)
		{
			Vector3 target = source + (direction * maxDistance);
			return new RaycastResult(Function.Call<int>(Hash._CAST_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		/// <inheritdoc cref="RaycastCapsule(Vector3, Vector3, float, IntersectOptions, Entity)"/>
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectOptions options)
		{
			return RaycastCapsule(source, target, radius, options, null);
		}
		/// <summary>
		/// Creates a 3D raycast between 2 points.
		/// </summary>
		/// <param name="source">The source of the raycast.</param>
		/// <param name="target">The target of the raycast.</param>
		/// <param name="radius">The radius of the raycast.</param>
		/// <param name="options">What type of objects the raycast should intersect with.</param>
		/// <param name="ignoreEntity">Specify an <see cref="Entity"/> that the raycast should ignore.</param>
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 target, float radius, IntersectOptions options, Entity ignoreEntity)
		{
			return new RaycastResult(Function.Call<int>(Hash._CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}
		/// <inheritdoc cref="RaycastCapsule(Vector3, Vector3, float, float, IntersectOptions, Entity)"/>
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectOptions options)
		{
			return RaycastCapsule(source, direction, maxDistance, radius, options, null);
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
		public static RaycastResult RaycastCapsule(Vector3 source, Vector3 direction, float maxDistance, float radius, IntersectOptions options, Entity ignoreEntity)
		{
			Vector3 target = source + (direction * maxDistance);
			return new RaycastResult(Function.Call<int>(Hash._CAST_3D_RAY_POINT_TO_POINT, source.X, source.Y, source.Z, target.X, target.Y, target.Z, radius, (int)options, ignoreEntity == null ? 0 : ignoreEntity.Handle, 7));
		}

		/// <summary>
		/// Determines where the crosshair intersects with the world.
		/// </summary>
		/// <returns>A <see cref="RaycastResult"/> containing information about where the crosshair intersects with the world.</returns>
		public static RaycastResult GetCrosshairCoordinates()
		{
			return Raycast(GameplayCamera.Position, GameplayCamera.Direction, 1000.0f, IntersectOptions.Everything);
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
		/// Gets the height of the ground at a given position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>The height measured in meters</returns>
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
		public static float GetGroundHeight(Vector3 position)
		{
			float resultArg;

			unsafe
			{
				Function.Call(Hash.GET_GROUND_Z_FOR_3D_COORD, position.X, position.Y, position.Z, &resultArg, false);
			}

			return resultArg;
		}

		/// <inheritdoc cref="GetSafeCoordForPed(Vector3, bool, int)"/>
		public static Vector3 GetSafeCoordForPed(Vector3 position)
		{
			return GetSafeCoordForPed(position, true, 0);
		}
		/// <inheritdoc cref="GetSafeCoordForPed(Vector3, bool, int)"/>
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk)
		{
			return GetSafeCoordForPed(position, sidewalk, 0);
		}
		/// <summary>
		/// Gets the nearest safe coordinate to position a <see cref="Ped"/>.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="sidewalk">if set to <see langword="true" /> Only find positions on the sidewalk.</param>
		/// <param name="flags">The flags.</param>
		public static Vector3 GetSafeCoordForPed(Vector3 position, bool sidewalk, int flags)
		{
			var outPos = new OutputArgument();

			if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, sidewalk, outPos, flags))
			{
				return outPos.GetResult<Vector3>();
			}

			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed. Considers switched off nodes, where ambient vehicles will not spawn.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		public static Vector3 GetNextPositionOnStreet(Vector3 position)
		{
			return GetNextPositionOnStreet(position, false);
		}
		/// <summary>
		/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed.  Considers switched off nodes, where ambient vehicles will not spawn.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="unoccupied">if set to <see langword="true" /> only find positions that dont already have a vehicle in them.</param>
		public static Vector3 GetNextPositionOnStreet(Vector2 position, bool unoccupied)
		{
			return GetNextPositionOnStreet(new Vector3(position.X, position.Y, 0), unoccupied);
		}
		/// <summary>
		/// Gets the next position on the street where a <see cref="Vehicle"/> can be placed. Considers switched off nodes, where ambient vehicles will not spawn.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		/// <param name="unoccupied">if set to <see langword="true" /> only find positions that dont already have a vehicle in them.</param>
		public static Vector3 GetNextPositionOnStreet(Vector3 position, bool unoccupied)
		{
			var outPos = new OutputArgument();

			if (unoccupied)
			{
				for (int i = 1; i < 40; i++)
				{
					Function.Call(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, i, outPos, 1, 0x40400000, 0);
					Vector3 newPos = outPos.GetResult<Vector3>();

					if (!Function.Call<bool>(Hash.IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
					{
						return newPos;
					}
				}
			}
			else if (Function.Call<bool>(Hash.GET_NTH_CLOSEST_VEHICLE_NODE, position.X, position.Y, position.Z, 1, outPos, 1, 0x40400000, 0))
			{
				return outPos.GetResult<Vector3>();
			}

			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the next position on the street where a <see cref="Ped"/> can be placed.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		public static Vector3 GetNextPositionOnSidewalk(Vector2 position)
		{
			return GetNextPositionOnSidewalk(new Vector3(position.X, position.Y, 0));
		}
		/// <summary>
		/// Gets the next position on the street where a <see cref="Ped"/> can be placed.
		/// </summary>
		/// <param name="position">The position to check around.</param>
		public static Vector3 GetNextPositionOnSidewalk(Vector3 position)
		{
			var outPos = new OutputArgument();

			if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, true, outPos, 0))
			{
				return outPos.GetResult<Vector3>();
			}
			else if (Function.Call<bool>(Hash.GET_SAFE_COORD_FOR_PED, position.X, position.Y, position.Z, false, outPos, 0))
			{
				return outPos.GetResult<Vector3>();
			}
			else
			{
				return Vector3.Zero;
			}
		}

		public static string GetZoneName(Vector2 position)
		{
			return GetZoneName(new Vector3(position.X, position.Y, 0));
		}
		public static string GetZoneName(Vector3 position)
		{
			if (Enum.TryParse(GetZoneNameLabel(position), out ZoneID zoneID))
			{
				switch (zoneID)
				{
					case ZoneID.AIRP:
						return "Los Santos International Airport";
					case ZoneID.ALAMO:
						return "Alamo Sea";
					case ZoneID.ALTA:
						return "Alta";
					case ZoneID.ARMYB:
						return "Fort Zancudo";
					case ZoneID.BANHAMC:
						return "Banham Canyon";
					case ZoneID.BANNING:
						return "Banning";
					case ZoneID.BAYTRE:
						return "Baytree Canyon";
					case ZoneID.BEACH:
						return "Vespucci Beach";
					case ZoneID.BHAMCA:
						return "Banham Canyon";
					case ZoneID.BRADP:
						return "Braddock Pass";
					case ZoneID.BRADT:
						return "Braddock Tunnel";
					case ZoneID.BURTON:
						return "Burton";
					case ZoneID.CALAFB:
						return "Calafia Bridge";
					case ZoneID.CANNY:
						return "Raton Canyon";
					case ZoneID.CCREAK:
						return "Cassidy Creek";
					case ZoneID.CHAMH:
						return "Chamberlain Hills";
					case ZoneID.CHIL:
						return "Vinewood Hills";
					case ZoneID.CHU:
						return "Chumash";
					case ZoneID.CMSW:
						return "Chiliad Mountain State Wilderness";
					case ZoneID.CYPRE:
						return "Cypress Flats";
					case ZoneID.DAVIS:
						return "Davis";
					case ZoneID.DELBE:
						return "Del Perro Beach";
					case ZoneID.DELPE:
						return "Del Perro";
					case ZoneID.DELSOL:
						return "Puerto Del Sol";
					case ZoneID.DESRT:
						return "Grand Senora Desert";
					case ZoneID.DOWNT:
						return "Downtown";
					case ZoneID.DTVINE:
						return "Downtown Vinewood";
					case ZoneID.EAST_V:
						return "East Vinewood";
					case ZoneID.EBURO:
						return "El Burro Heights";
					case ZoneID.ELGORL:
						return "El Gordo Lighthouse";
					case ZoneID.ELYSIAN:
						return "Elysian Island";
					case ZoneID.GALFISH:
						return "Galilee";
					case ZoneID.GALLI:
						return "Galileo Park";
					case ZoneID.golf:
						return "GWC and Golfing Society";
					case ZoneID.GRAPES:
						return "Grapeseed";
					case ZoneID.GREATC:
						return "Great Chaparral";
					case ZoneID.HARMO:
						return "Harmony";
					case ZoneID.HAWICK:
						return "Hawick";
					case ZoneID.HORS:
						return "Vinewood Racetrack";
					case ZoneID.HUMLAB:
						return "Humane Labs and Research";
					case ZoneID.JAIL:
						return "Bolingbroke Penitentiary";
					case ZoneID.KOREAT:
						return "Little Seoul";
					case ZoneID.LACT:
						return "Land Act Reservoir";
					case ZoneID.LAGO:
						return "Lago Zancudo";
					case ZoneID.LDAM:
						return "Land Act Dam";
					case ZoneID.LEGSQU:
						return "Legion Square";
					case ZoneID.LMESA:
						return "La Mesa";
					case ZoneID.LOSPUER:
						return "La Puerta";
					case ZoneID.MIRR:
						return "Mirror Park";
					case ZoneID.MORN:
						return "Morningwood";
					case ZoneID.MOVIE:
						return "Richards Majestic";
					case ZoneID.MTCHIL:
						return "Mount Chiliad";
					case ZoneID.MTGORDO:
						return "Mount Gordo";
					case ZoneID.MTJOSE:
						return "Mount Josiah";
					case ZoneID.MURRI:
						return "Murrieta Heights";
					case ZoneID.NCHU:
						return "North Chumash";
					case ZoneID.NOOSE:
						return "N.O.O.S.E.";
					case ZoneID.OCEANA:
						return "Pacific Ocean";
					case ZoneID.PALCOV:
						return "Paleto Cove";
					case ZoneID.PALETO:
						return "Paleto Bay";
					case ZoneID.PALFOR:
						return "Paleto Forest";
					case ZoneID.PALHIGH:
						return "Palomino Highlands";
					case ZoneID.PALMPOW:
						return "Palmer-Taylor Power Station";
					case ZoneID.PBLUFF:
						return "Pacific Bluffs";
					case ZoneID.PBOX:
						return "Pillbox Hill";
					case ZoneID.PROCOB:
						return "Procopio Beach";
					case ZoneID.RANCHO:
						return "Rancho";
					case ZoneID.RGLEN:
						return "Richman Glen";
					case ZoneID.RICHM:
						return "Richman";
					case ZoneID.ROCKF:
						return "Rockford Hills";
					case ZoneID.RTRAK:
						return "Redwood Lights Track";
					case ZoneID.SanAnd:
						return "San Andreas";
					case ZoneID.SANCHIA:
						return "San Chianski Mountain Range";
					case ZoneID.SANDY:
						return "Sandy Shores";
					case ZoneID.SKID:
						return "Mission Row";
					case ZoneID.SLAB:
						return "Stab City";
					case ZoneID.STAD:
						return "Maze Bank Arena";
					case ZoneID.STRAW:
						return "Strawberry";
					case ZoneID.TATAMO:
						return "Tataviam Mountains";
					case ZoneID.TERMINA:
						return "Terminal";
					case ZoneID.TEXTI:
						return "Textile City";
					case ZoneID.TONGVAH:
						return "Tongva Hills";
					case ZoneID.TONGVAV:
						return "Tongva Valley";
					case ZoneID.VCANA:
						return "Vespucci Canals";
					case ZoneID.VESP:
						return "Vespucci";
					case ZoneID.VINE:
						return "Vinewood";
					case ZoneID.WINDF:
						return "RON Alternates Wind Farm";
					case ZoneID.WVINE:
						return "West Vinewood";
					case ZoneID.ZANCUDO:
						return "Zancudo River";
					case ZoneID.ZP_ORT:
						return "Port of South Los Santos";
					case ZoneID.ZQ_UAR:
						return "Davis Quartz";
				}
			}
			return string.Empty;
		}

		public static string GetZoneNameLabel(Vector2 position)
		{
			return GetZoneNameLabel(new Vector3(position.X, position.Y, 0));
		}
		public static string GetZoneNameLabel(Vector3 position)
		{
			return Function.Call<string>(Hash.GET_NAME_OF_ZONE, position.X, position.Y, position.Z);
		}

		public static string GetStreetName(Vector2 position)
		{
			return GetStreetName(new Vector3(position.X, position.Y, 0));
		}
		public static string GetStreetName(Vector3 position)
		{
			int streetHash = 0, crossingHash = 0;
			unsafe
			{
				Function.Call(Hash.GET_STREET_NAME_AT_COORD, position.X, position.Y, position.Z, &streetHash, &crossingHash);
			}

			return Function.Call<string>(Hash.GET_STREET_NAME_FROM_HASH_KEY, streetHash);
		}

		#endregion
	}
}
