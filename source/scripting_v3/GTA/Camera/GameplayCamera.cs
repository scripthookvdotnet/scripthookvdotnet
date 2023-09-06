//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.ComponentModel;

namespace GTA
{
	/// <summary>
	/// Represents the gameplay camera director.
	/// The gameplay director is responsible for the follow and aim cameras.
	/// </summary>
	public static class GameplayCamera
	{
		/// <summary>
		/// Gets the memory address of the gameplay camera director.
		/// </summary>
		public static IntPtr MemoryAddress => SHVDN.NativeMemory.GetGameplayCameraAddress();

		/// <summary>
		/// Gets the matrix of the <see cref="GameplayCamera"/> director.
		/// </summary>
		public static Matrix Matrix => new(SHVDN.NativeMemory.ReadMatrix(MemoryAddress + 0x1F0));

		/// <summary>
		/// Gets the position of the <see cref="GameplayCamera"/> director.
		/// </summary>
		public static Vector3 Position => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);

		/// <summary>
		/// Gets the rotation of the <see cref="GameplayCamera"/> director.
		/// </summary>
		/// <value>
		/// The yaw, pitch and roll rotations measured in degrees.
		/// </value>
		public static Vector3 Rotation => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 2);

		/// <summary>
		/// Gets the direction of the <see cref="GameplayCamera"/> director is pointing in.
		/// </summary>
		public static Vector3 Direction => ForwardVector;

		/// <summary>
		/// Gets the up vector of the <see cref="GameplayCamera"/> director.
		/// </summary>
		public static Vector3 UpVector => new(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x210));

		/// <summary>
		/// Gets the right vector of the <see cref="GameplayCamera"/> director.
		/// </summary>
		public static Vector3 RightVector => new(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x1F0));

		/// <summary>
		/// Gets the forward vector of the <see cref="GameplayCamera"/> director, see also <seealso cref="Direction"/>.
		/// </summary>
		public static Vector3 ForwardVector => new(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x200));

		/// <summary>
		/// Gets the position in world coordinates of an offset relative to the <see cref="GameplayCamera"/> director.
		/// </summary>
		/// <param name="offset">The offset from the <see cref="GameplayCamera"/>.</param>
		public static Vector3 GetOffsetPosition(Vector3 offset)
		{
			return Matrix.TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of the <see cref="GameplayCamera"/> director from a world coordinates position.
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public static Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			return Matrix.InverseTransformPoint(worldCoords);
		}

		#region Third Person Camera

		public static void FollowCameraIgnoreAttachParentMovementThisUpdate()
			=> Function.Call(Hash.SET_FOLLOW_CAM_IGNORE_ATTACH_PARENT_MOVEMENT_THIS_UPDATE);

		/// <summary>
		/// Forces the active third person camera using the specified heading limits only for this update.
		/// </summary>
		/// <param name="minRelativeHeading">The minimum yaw value.</param>
		/// <param name="maxRelativeHeading">The maximum yaw value.</param>
		public static void SetThirdPersonCameraRelativeHeadingLimitsThisUpdate(float minRelativeHeading,
			float maxRelativeHeading)
			=> Function.Call(Hash.SET_THIRD_PERSON_CAM_RELATIVE_HEADING_LIMITS_THIS_UPDATE, minRelativeHeading,
				maxRelativeHeading);

		/// <summary>
		/// Forces the active third person camera using the specified pitch limits only for this update.
		/// </summary>
		/// <param name="minRelativePitch">The minimum pitch value.</param>
		/// <param name="maxRelativePitch">The maximum pitch value.</param>
		public static void SetThirdPersonCameraRelativePitchLimitsThisUpdate(float minRelativePitch,
			float maxRelativePitch)
			=> Function.Call(Hash.SET_THIRD_PERSON_CAM_RELATIVE_PITCH_LIMITS_THIS_UPDATE, minRelativePitch,
				maxRelativePitch);

		/// <summary>
		/// Forces the active third person camera using the specified pitch limits only for this update.
		/// </summary>
		/// <param name="minDistance">The minimum distance in meters.</param>
		/// <param name="maxDistance">The maximum distance in meters.</param>
		public static void SetThirdPersonCameraOrbitDistanceLimitsThisUpdate(float minDistance, float maxDistance)
			=> Function.Call(Hash.SET_THIRD_PERSON_CAM_ORBIT_DISTANCE_LIMITS_THIS_UPDATE, minDistance, maxDistance);

		#endregion

		/// <summary>
		/// Gets the <see cref="GameplayCamera"/>'s pitch relative to the target entity (ped or vehicle) in degrees.
		/// </summary>
		public static float RelativePitch
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_PITCH);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, value, 1f);
		}

		/// <summary>
		/// Sets the gameplay camera's pitch relative to the target entity (<see cref="Ped"/> or <see cref="Vehicle"/>).
		/// </summary>
		/// <param name="pitch">The relative pitch to set in degrees.</param>
		/// <param name="smoothRate">
		/// The rate at which the relative pitch should be attained. 1.0f is instant, 0.0f is infinite.
		/// </param>
		public static void SetRelativePitch(float pitch, float smoothRate)
			=> Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, pitch, smoothRate);

		/// <summary>
		/// Gets the <see cref="GameplayCamera"/>'s heading relative to the target entity (ped or vehicle) in degrees.
		/// </summary>
		public static float RelativeHeading
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_HEADING);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, value);
		}

		/// <summary>
		/// Sets the gameplay camera's pitch relative to the target entity (<see cref="Ped"/> or <see cref="Vehicle"/>).
		/// </summary>
		/// <param name="heading">The relative heading to set in degrees.</param>
		/// <param name="pitch">The relative pitch to set in degrees.</param>
		/// <param name="smoothRate">
		/// The rate at which the relative pitch should be attained. 1.0f is instant, 0.0f is infinite.
		/// </param>
		public static void ForceRelativeHeadingAndPitch(float heading, float pitch, float smoothRate)
			=> Function.Call(Hash.FORCE_CAMERA_RELATIVE_HEADING_AND_PITCH, heading, pitch, smoothRate);

		/// <summary>
		/// Gets the value that indicates a follow-ped camera is active.
		/// </summary>
		public static bool IsFollowPedCamActive => Function.Call<bool>(Hash.IS_FOLLOW_PED_CAM_ACTIVE);

		/// <summary>
		/// Gets the global view mode used by all follow-ped cameras.
		/// </summary>
		public static CamViewMode FollowPedCamViewMode
		{
			get => Function.Call<CamViewMode>(Hash.GET_FOLLOW_PED_CAM_VIEW_MODE);
			set => Function.Call(Hash.SET_FOLLOW_PED_CAM_VIEW_MODE, (int)value);
		}

		/// <summary>
		/// Gets the value that indicates a follow-vehicle camera is active.
		/// </summary>
		public static bool IsFollowVehicleCamActive => Function.Call<bool>(Hash.IS_FOLLOW_VEHICLE_CAM_ACTIVE);

		/// <summary>
		/// Gets or sets the view mode used by the follow-vehicle and vehicle-aim cameras associated with classes of vehicles
		/// that are not handled specially, such as cars.
		/// Use <see cref="GetCamViewModeForContext"/> or <see cref="SetCamViewModeForContext"/>
		/// to query the view mode applied for other classes of vehicle.
		/// </summary>
		public static CamViewMode FollowVehicleCamViewMode
		{
			get => Function.Call<CamViewMode>(Hash.GET_FOLLOW_VEHICLE_CAM_VIEW_MODE);
			set => Function.Call(Hash.SET_FOLLOW_VEHICLE_CAM_VIEW_MODE, (int)value);
		}

		/// <summary>
		///  Gets the camera view mode for the specified context.
		/// </summary>
		public static CamViewMode GetCamViewModeForContext(CamViewModeContext context)
			=> Function.Call<CamViewMode>(Hash.GET_CAM_VIEW_MODE_FOR_CONTEXT, (int)context);

		/// <summary>
		///	Sets the camera view mode for the specified context.
		/// </summary>
		public static CamViewMode SetCamViewModeForContext(CamViewModeContext context, CamViewMode viewMode)
			=> Function.Call<CamViewMode>(Hash.SET_CAM_VIEW_MODE_FOR_CONTEXT, (int)context, (int)viewMode);

		/// <summary>
		/// Returns the view mode context for the active gameplay camera.
		/// </summary>
		public static CamViewModeContext ActiveViewModeContext
			=> Function.Call<CamViewModeContext>(Hash.GET_CAM_ACTIVE_VIEW_MODE_CONTEXT);

		#region Hint Camera

		// All the 3 const values below are taken from the official cam header file
		public const int DefaultDwellTime = 2000;
		public const int DefaultInterpInTime = 2000;
		public const int DefaultInterpOutTime = 2000;

		/// <summary>
		///	Sets the gameplay to hint a coord.
		/// </summary>
		/// <param name="coord">The coordinate to hint (point at).</param>
		/// <param name="dwellTime">How long cam looks at the coordinate.</param>
		/// <param name="interpTo">How long the interp to the hint is.</param>
		/// <param name="interpFrom">How long the interp is from the interp.</param>
		/// <param name="overriddenHintType">The overridden hint type.</param>
		public static void SetCoordHint(Vector3 coord, int dwellTime = DefaultDwellTime,
			int interpTo = DefaultInterpInTime, int interpFrom = DefaultInterpOutTime,
			CameraHintHelperNameHash overriddenHintType = CameraHintHelperNameHash.None)
			=> Function.Call(Hash.SET_GAMEPLAY_COORD_HINT, coord.X, coord.Y, coord.Z, dwellTime, interpTo, interpFrom,
				(uint)overriddenHintType);

		/// <summary>
		///	Sets the gameplay to hint an <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to hint (point at).</param>
		/// <param name="offset">The offset from the <see cref="Entity"/>.</param>
		/// <param name="relativeOffset">Specifies whether the offset is relative to the <see cref="Entity"/>.</param>
		/// <param name="dwellTime">How long cam looks at the coordinate.</param>
		/// <param name="interpTo">How long the interp to the hint is.</param>
		/// <param name="interpFrom">How long the interp is from the interp.</param>
		/// <param name="overriddenHintType">The overridden hint type.</param>
		public static void SetEntityHint(Entity entity, Vector3 offset, bool relativeOffset = true,
			int dwellTime = DefaultDwellTime, int interpTo = DefaultInterpInTime, int interpFrom = DefaultInterpOutTime,
			CameraHintHelperNameHash overriddenHintType = CameraHintHelperNameHash.None)
			=> Function.Call(Hash.SET_GAMEPLAY_ENTITY_HINT, entity, offset.X, offset.Y, offset.Z, relativeOffset,
				dwellTime, interpTo, interpFrom, (uint)overriddenHintType);

		/// <summary>
		/// Gets the value that indicates whether a hint is running.
		/// In other words, the return value indicates whether the gameplay camera is zooming to
		/// some <see cref="Entity"/> or coordinate.
		/// </summary>
		public static bool IsHintActive => Function.Call<bool>(Hash.IS_GAMEPLAY_HINT_ACTIVE);

		/// <summary>
		/// Scales the cameras orbit distance between the camera and its attach <see cref="Entity"/> or coordinate.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Call at the start of the hint to avoid pops.
		/// This value will be cleared after a hint is finished.
		/// </para>
		/// </remarks>
		public static float HintFollowDistanceScaler
		{
			set => Function.Call(Hash.SET_GAMEPLAY_HINT_FOLLOW_DISTANCE_SCALAR, value);
		}

		/// <summary>
		/// Adjusts the pitch of camera relative to its attach parent.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Call at the start of the hint to avoid pops.
		/// This value will be cleared after a hint is finished.
		/// </para>
		/// <para>
		/// Changes nothing if the passed value is not in the range of [1f, 130f].
		/// </para>
		/// </remarks>
		public static float HintBaseOrbitPitchOffset
		{
			set => Function.Call(Hash.SET_GAMEPLAY_HINT_BASE_ORBIT_PITCH_OFFSET, value);
		}

		/// <summary>
		/// Sets an side offset relative attach parent in meters.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Call at the start of the hint to avoid pops.
		/// This value will be cleared after a hint is finished.
		/// </para>
		/// </remarks>
		public static float HintCameraRelativeSideOffset
		{
			set => Function.Call(Hash.SET_GAMEPLAY_HINT_CAMERA_RELATIVE_SIDE_OFFSET, value);
		}

		/// <summary>
		/// Sets an vertical offset relative attach parent in meters.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Call at the start of the hint to avoid pops.
		/// This value will be cleared after a hint is finished.
		/// </para>
		/// <para>
		/// This will cause the camera to pull back to frame the player correctly,
		/// may need to use in conjunction with the <see cref="HintFollowDistanceScaler"/>.
		/// </para>
		/// </remarks>
		public static float HintCameraRelativeVerticalOffset
		{
			set => Function.Call(Hash.SET_GAMEPLAY_HINT_CAMERA_RELATIVE_VERTICAL_OFFSET, value);
		}

		/// <summary>
		/// Sets the hint field of view override.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Call at the start of the hint to avoid pops.
		/// This value will be cleared after a hint is finished.
		/// </para>
		/// <para>
		/// Changes nothing if the passed value is not in the range of [1f, 130f].
		/// </para>
		/// </remarks>
		public static float HintFovOverride
		{
			set => Function.Call(Hash.SET_GAMEPLAY_HINT_FOV, value);
		}

		/// <summary>
		/// Stops the hint cam running except if its a code gameplay hint.
		/// </summary>
		/// <param name="stopImmediately">
		/// If <see langword="true"/>, the hint camera will stop immediately,
		/// otherwise it will enter its release phase.
		/// </param>
		public static void StopGameplayHint(bool stopImmediately)
			=> Function.Call(Hash.STOP_GAMEPLAY_HINT, stopImmediately);

		/// <summary>
		///  Stops any active code gameplay hint.
		/// </summary>
		/// <param name="stopImmediately">
		/// If <see langword="true"/>, the hint camera will stop immediately,
		/// otherwise it will enter its release phase.
		/// </param>
		public static void StopCodeGameplayHint(bool stopImmediately)
			=> Function.Call(Hash.STOP_CODE_GAMEPLAY_HINT, stopImmediately);

		/// <summary>
		/// Gets the value that indicates whether that a code gameplay hint is active.
		/// </summary>
		/// <remarks>
		/// A code gameplay hint is a hint activated by the game code (not by some script).
		/// Hints are based on a first come, fist served basis.
		/// </remarks>
		public static bool IsCodeHintActive => Function.Call<bool>(Hash.IS_CODE_GAMEPLAY_HINT_ACTIVE);

		/// <summary>
		/// Sets the camera exit/enter state for <see cref="Vehicle"/>s which defines
		/// if the follow vehicle or follow ped camera runs.
		/// You can use this method to get the camera to interpolate between the follow ped and follow vehicle cameras
		/// the intermediate states have to be called.
		/// </summary>
		/// <param name="vehicle">
		/// The vehicle for follow vehicle camera.
		/// You can pass <see langword="null"/> or a invalid <see cref="Vehicle"/>
		/// if <paramref name="inVehicleState"/> is set to <see cref="CamInVehicleState.OutsideVehicle"/>,
		/// since this value will not be used for <see cref="CamInVehicleState.OutsideVehicle"/>.
		/// </param>
		/// <param name="inVehicleState">The in vehicle camera state enum.</param>
		public static void SetInVehicleCameraStateThisUpdate(Vehicle vehicle, CamInVehicleState inVehicleState)
			=> Function.Call(Hash.SET_IN_VEHICLE_CAM_STATE_THIS_UPDATE, vehicle, (int)inVehicleState);

		/// <summary>
		/// Disables on foot first person view, must be called every update.
		/// </summary>
		public static void DisableOnFootFirstPersonViewThisUpdate()
			=> Function.Call(Hash.DISABLE_ON_FOOT_FIRST_PERSON_VIEW_THIS_UPDATE);

		/// <summary>
		/// Disables flash effects from first person transitions, must be called every update.
		/// </summary>
		public static void DisableFirstPersonFlashEffectThisUpdate()
			=> Function.Call(Hash.DISABLE_FIRST_PERSON_FLASH_EFFECT_THIS_UPDATE);

		#endregion

		/// <summary>
		/// Gets or sets the first-person ped aim zoom factor associated with equipped sniper scoped weapon,
		/// or the mobile phone camera, if active.
		/// </summary>
		/// <remarks>
		/// The specified zoom factor will be clamped to between 1.0 and the maximum zoom factor supported by the
		/// specific weapon/camera. The zoom factor will also automatically reset to 1.0 if the follow
		/// <see cref="Ped"/>'s equipped weapon changes or the mobile phone camera toggles on or off.
		/// </remarks>
		public static float FirstPersonAimCamZoomFactor
		{
			get => Function.Call<float>(Hash.GET_FIRST_PERSON_AIM_CAM_ZOOM_FACTOR);
			set => Function.Call(Hash.SET_FIRST_PERSON_AIM_CAM_ZOOM_FACTOR, value);
		}

		/// <summary>
		/// Gets the first-person ped aim zoom factor associated with equipped sniper scoped weapon,
		/// or the mobile phone camera, if active.
		/// </summary>
		[Obsolete("GameplayCamera.Zoom is obsolete since it does not suggest the value is relevant only when a first" +
			"person aim camera is used. Use GameplayCamera.FirstPersonAimCamZoomFactor instead."),
			EditorBrowsable(EditorBrowsableState.Never)]
		public static float Zoom => FirstPersonAimCamZoomFactor;

		/// <summary>
		/// Gets the field of view of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float FieldOfView => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_FOV);

		/// <summary>
		/// Gets a value indicating whether the gameplay director is the dominant rendering director.
		/// The gameplay director is responsible for the follow and aim cameras.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> is rendering; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsRendering => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_RENDERING);

		/// <summary>
		/// Gets a value indicating whether an aim camera is active.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if an aim camera is rendering; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsAimCamActive => Function.Call<bool>(Hash.IS_AIM_CAM_ACTIVE);

		/// <summary>
		/// Gets a value indicating whether a first person ped aim camera is active,
		/// which can be activated by using a sniper rifle scope or mobile phone camera (strictly checks if the gameplay
		/// director use a <c>camFirstPersonPedAimCamera</c> instance).
		/// Do not confuse with a first person shooter camera (<c>camFirstPersonShooterCamera</c>),
		/// which can be activated by switching the camera mode.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if a first person ped aim camera is active; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFirstPersonAimCamActive => Function.Call<bool>(Hash.IS_FIRST_PERSON_AIM_CAM_ACTIVE);

		/// <summary>
		/// Gets a value indicating whether the active gameplay camera is looking behind.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> director is looking behind; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsLookingBehind => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_LOOKING_BEHIND);

		/// <summary>
		/// Shakes the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="shakeType">Type of the shake to apply.</param>
		/// <param name="amplitude">The amplitude of the shaking.</param>
		public static void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_GAMEPLAY_CAM, Camera.s_shakeNames[(int)shakeType], amplitude);
		}

		/// <summary>
		/// Stops shaking the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="stopImmediately">Whether to stop shaking immediately; defaults to <see langword="false"/></param>
		public static void StopShaking(bool stopImmediately = false)
		{
			Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, stopImmediately);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is shaking.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> is shaking; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsShaking => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_SHAKING);

		/// <summary>
		/// Sets the overall amplitude scaling for an active gameplay camera shake.
		/// </summary>
		public static float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, value);
		}
	}
}
