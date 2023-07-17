//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public static class GameplayCamera
	{
		public static Vector3 Position => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);

		public static Vector3 Rotation => Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 2);

		public static Vector3 Direction
		{
			get
			{
				Vector3 rot = Rotation;
				double rotX = rot.X / 57.295779513082320876798154814105;
				double rotZ = rot.Z / 57.295779513082320876798154814105;
				double multXY = System.Math.Abs(System.Math.Cos(rotX));

				return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)System.Math.Sin(rotX));
			}
		}

		public static Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			var right = new Vector3((float)x, (float)y, (float)z);
			var up = Vector3.Cross(right, forward);
			return Position + (right * offset.X) + (forward * offset.Y) + (up * offset.Z);
		}

		public static Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 forward = Direction;
			const double D2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			var right = new Vector3((float)x, (float)y, (float)z);
			var up = Vector3.Cross(right, forward);
			Vector3 delta = worldCoords - Position;
			return new Vector3(Vector3.Dot(right, delta), Vector3.Dot(forward, delta), Vector3.Dot(up, delta));
		}

		public static void ClampYaw(float min, float max)
		{
			Function.Call(Hash._CLAMP_GAMEPLAY_CAM_YAW, min, max);
		}
		public static void ClampPitch(float min, float max)
		{
			Function.Call(Hash._CLAMP_GAMEPLAY_CAM_PITCH, min, max);
		}

		public static float RelativePitch
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_PITCH);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, value, 1f);
		}

		public static float RelativeHeading
		{
			get => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_HEADING);
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, value);
		}

		public static float Zoom => Function.Call<float>(Hash._GET_GAMEPLAY_CAM_ZOOM);
		public static float FieldOfView => Function.Call<float>(Hash.GET_GAMEPLAY_CAM_FOV);

		public static bool IsRendering => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_RENDERING);

		public static bool IsAimCamActive => Function.Call<bool>(Hash.IS_AIM_CAM_ACTIVE);

		public static bool IsFirstPersonAimCamActive => Function.Call<bool>(Hash.IS_FIRST_PERSON_AIM_CAM_ACTIVE);

		public static bool IsLookingBehind => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_LOOKING_BEHIND);

		public static void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_GAMEPLAY_CAM, Camera.s_shakeNames[(int)shakeType], amplitude);
		}
		public static void StopShaking()
		{
			Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
		}

		public static bool IsShaking => Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_SHAKING);

		public static float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, value);
		}
	}
}
