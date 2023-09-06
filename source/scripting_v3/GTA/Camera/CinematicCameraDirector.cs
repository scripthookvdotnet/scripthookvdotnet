//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
	/// <summary>
	/// Represents the cinematic camera director.
	/// The cinematic director is responsible for idle, vehicle cinematic, vehicle first person, and nose cameras.
	/// </summary>
	public static class CinematicCameraDirector
	{
		/// <summary>
		/// Invalidates the cinematic idle camera, restarting the associated idle counter.
		/// </summary>
		public static void InvalidateIdleCam() => Function.Call(Hash.INVALIDATE_IDLE_CAM);

		/// <summary>
		/// Invalidates the vehicle cinematic idle camera, restarting the associated idle counter.
		/// </summary>
		public static void InvalidateVehicleIdleCam() => Function.Call(Hash.INVALIDATE_CINEMATIC_VEHICLE_IDLE_MODE);

		/// <summary>
		/// Gets a value indicating whether a first person vehicle interior camera is active.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/> is a first person vehicle camera; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFirstPersonVehicleInteriorCamRendering => Function.Call<bool>(Hash.IS_CINEMATIC_FIRST_PERSON_VEHICLE_INTERIOR_CAM_RENDERING);

		/// <summary>
		/// Gets a value indicating whether the vehicle bonnet cam is rendering.
		/// For example, this will return true if using the weapon cam on the Hydra.
		/// Only available in v1.0.372.2 or later versions.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/> is a vehicle bonnet camera; otherwise, <see langword="false" />.
		/// </value>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in game versions earlier than v1.0.372.2.
		/// </exception>
		public static bool IsVehicleBonnetCamRendering
		{
			get
			{
				GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_372_2_Steam, nameof(CinematicCameraDirector), nameof(IsVehicleBonnetCamRendering));

				return Function.Call<bool>(Hash.IS_BONNET_CINEMATIC_CAM_RENDERING);
			}
		}

		/// <summary>
		/// Gets a value indicating whether any cinematic camera is rendering.
		/// Note that this will also return true if in first-person view while inside a vehicle.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/> is a cinematic camera; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsRendering => Function.Call<bool>(Hash.IS_CINEMATIC_CAM_RENDERING);

		/// <summary>
		/// Gets a value indicating whether an idle cinematic camera is rendering.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/> is an idle cinematic camera; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsIdleCamRendering => Function.Call<bool>(Hash.IS_CINEMATIC_IDLE_CAM_RENDERING);

		/// <summary>
		/// Gets a value indicating whether the cinematic camera mode switch is active.
		/// Only available in v1.0.1493.0 or later game versions.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/> mode switch is active; otherwise, <see langword="false" />.
		/// </value>
		/// <exception cref="GameVersionNotSupportedException">
		/// Thrown if called in game versions earlier than v1.0.1493.0.
		/// </exception>
		public static bool IsCinematicModeActive
		{
			get
			{
				GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1493_0_Steam, nameof(CinematicCameraDirector), nameof(IsCinematicModeActive));

				return Function.Call<bool>(Hash.IS_CINEMATIC_CAM_INPUT_ACTIVE);
			}
		}

		/// <summary>
		/// Shakes the <see cref="CinematicCameraDirector"/>'s active camera.
		/// </summary>
		/// <param name="shakeType">Type of the shake to apply.</param>
		/// <param name="amplitude">The amplitude of the shaking.</param>
		public static void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_CINEMATIC_CAM, Camera.s_shakeNames[(int)shakeType], amplitude);
		}

		/// <summary>
		/// Stops shaking the <see cref="CinematicCameraDirector"/>'s active camera.
		/// </summary>
		/// <param name="stopImmediately">Whether to stop shaking immediately; defaults to <see langword="false"/></param>
		public static void StopShaking(bool stopImmediately = false)
		{
			Function.Call(Hash.STOP_CINEMATIC_CAM_SHAKING, stopImmediately);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="CinematicCameraDirector"/>'s active camera is shaking.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CinematicCameraDirector"/>'s active camera is shaking; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsShaking => Function.Call<bool>(Hash.IS_CINEMATIC_CAM_SHAKING);

		/// <summary>
		/// Sets the overall amplitude scaling for an active cinematic camera shake.
		/// </summary>
		public static float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_CINEMATIC_CAM_SHAKE_AMPLITUDE, value);
		}
	}
}
