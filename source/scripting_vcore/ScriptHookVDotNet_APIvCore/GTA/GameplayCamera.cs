//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public static class GameplayCamera
	{
		/// <summary>
		/// Gets the memory address of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static IntPtr MemoryAddress => SHVDN.NativeMemory.GetGameplayCameraAddress();

		/// <summary>
		/// Gets the matrix of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Matrix Matrix => new Matrix(SHVDN.NativeMemory.ReadMatrix(MemoryAddress + 0x1F0));

		/// <summary>
		/// Gets the position of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Vector3 Position => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);

		/// <summary>
		/// Gets the rotation of the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <value>
		/// The yaw, pitch and roll rotations measured in degrees.
		/// </value>
		public static Vector3 Rotation => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 2);

		/// <summary>
		/// Gets the direction the <see cref="GameplayCamera"/> is pointing in.
		/// </summary>
		public static Vector3 Direction => ForwardVector;

		/// <summary>
		/// Gets the up vector of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Vector3 UpVector => new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x210));

		/// <summary>
		/// Gets the right vector of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static Vector3 RightVector => new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x1F0));

		/// <summary>
		/// Gets the forward vector of the <see cref="GameplayCamera"/>, see also <seealso cref="Direction"/>.
		/// </summary>
		public static Vector3 ForwardVector => new Vector3(SHVDN.NativeMemory.ReadVector3(MemoryAddress + 0x200));

		/// <summary>
		/// Gets the position in world coordinates of an offset relative to the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="offset">The offset from the <see cref="GameplayCamera"/>.</param>
		public static Vector3 GetOffsetPosition(Vector3 offset)
		{
			return Matrix.TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of the <see cref="GameplayCamera"/> from a world coordinates position.
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public static Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			return Matrix.InverseTransformPoint(worldCoords);
		}

		/// <summary>
		/// Clamps the yaw of the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="min">The minimum yaw value.</param>
		/// <param name="max">The maximum yaw value.</param>
		public static void ClampYaw(float min, float max)
		{
			Function.Call(Hash.SET_THIRD_PERSON_CAM_RELATIVE_HEADING_LIMITS_THIS_UPDATE, min, max);
		}
		/// <summary>
		/// Clamps the pitch of the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="min">The minimum pitch value.</param>
		/// <param name="max">The maximum pitch value.</param>
		public static void ClampPitch(float min, float max)
		{
			Function.Call(Hash.SET_THIRD_PERSON_CAM_RELATIVE_PITCH_LIMITS_THIS_UPDATE, min, max);
		}

		/// <summary>
		/// Gets or sets the relative pitch of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float RelativePitch
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_PITCH);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, value, 1f);
		}

		/// <summary>
		/// Gets or sets the relative heading of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float RelativeHeading
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_HEADING);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, value);
		}

		/// <summary>
		/// Gets the zoom of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float Zoom => Function.Call<float>(Hash.GET_FIRST_PERSON_AIM_CAM_ZOOM_FACTOR);

		/// <summary>
		/// Gets the field of view of the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float FieldOfView => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_FOV);

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is rendering.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> is rendering; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsRendering => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_RENDERING);

		/// <summary>
		/// Gets a value indicating whether the aiming camera is rendering.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the aiming camera is rendering; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsAimCamActive => Function.Call<bool>(Hash.IS_AIM_CAM_ACTIVE);

		/// <summary>
		/// Gets a value indicating whether the first person aiming camera is rendering.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the aiming camera is rendering; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsFirstPersonAimCamActive => Function.Call<bool>(Hash.IS_FIRST_PERSON_AIM_CAM_ACTIVE);

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is looking behind.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> is looking behind; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsLookingBehind => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_LOOKING_BEHIND);

		/// <summary>
		/// Shakes the <see cref="GameplayCamera"/>.
		/// </summary>
		/// <param name="shakeType">Type of the shake to apply.</param>
		/// <param name="amplitude">The amplitude of the shaking.</param>
		public static void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_GAMEPLAY_CAM, Camera.shakeNames[(int)shakeType], amplitude);
		}

		/// <summary>
		/// Stops shaking the <see cref="GameplayCamera"/>.
		/// </summary>
		public static void StopShaking()
		{
			Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="GameplayCamera"/> is shaking.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="GameplayCamera"/> is shaking; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsShaking => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_SHAKING);

		/// <summary>
		/// Sets the shake amplitude for the <see cref="GameplayCamera"/>.
		/// </summary>
		public static float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, value);
		}
	}
}
