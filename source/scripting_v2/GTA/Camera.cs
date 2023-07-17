//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;

namespace GTA
{
	public sealed class Camera : IEquatable<Camera>, IHandleable, ISpatial
	{
		internal static readonly string[] s_shakeNames = {
			"HAND_SHAKE",
			"SMALL_EXPLOSION_SHAKE",
			"MEDIUM_EXPLOSION_SHAKE",
			"LARGE_EXPLOSION_SHAKE",
			"JOLT_SHAKE",
			"VIBRATE_SHAKE",
			"ROAD_VIBRATION_SHAKE",
			"DRUNK_SHAKE",
			"SKY_DIVING_SHAKE",
			"FAMILY5_DRUG_TRIP_SHAKE",
			"DEATH_FAIL_IN_EFFECT_SHAKE"
		};

		public Camera(int handle)
		{
			Handle = handle;
		}

		public int Handle
		{
			get;
		}

		public bool IsActive
		{
			get => Function.Call<bool>(Hash.IS_CAM_ACTIVE, Handle);
			set => Function.Call(Hash.SET_CAM_ACTIVE, Handle, value);
		}

		public Vector3 Position
		{
			get => Function.Call<Vector3>(Hash.GET_CAM_COORD, Handle);
			set => Function.Call(Hash.SET_CAM_COORD, Handle, value.X, value.Y, value.Z);
		}

		public Vector3 Rotation
		{
			get => Function.Call<Vector3>(Hash.GET_CAM_ROT, Handle, 2);
			set => Function.Call(Hash.SET_CAM_ROT, Handle, value.X, value.Y, value.Z, 2);
		}

		public Vector3 Direction
		{
			get
			{
				Vector3 rot = Rotation;
				double rotX = rot.X / 57.295779513082320876798154814105;
				double rotZ = rot.Z / 57.295779513082320876798154814105;
				double multXY = System.Math.Abs(System.Math.Cos(rotX));

				return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)System.Math.Sin(rotX));
			}
			set
			{
				value.Normalize();
				var vector1 = new Vector3(value.X, value.Y, 0.0f);
				var vector2 = new Vector3(value.Z, vector1.Length(), 0.0f);
				var vector3 = Vector3.Normalize(vector2);
				Rotation = new Vector3((float)(System.Math.Atan2(vector3.X, vector3.Y) * 57.295779513082320876798154814105), 0.0f, (float)(-System.Math.Atan2(value.X, value.Y) * 57.295779513082320876798154814105));
			}
	}

		public Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 forward = Direction;
			const double d2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * d2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * d2R);
			double y = num1 * System.Math.Sin(Rotation.Z * d2R);
			double z = System.Math.Sin(-Rotation.Y * d2R);
			var right = new Vector3((float)x, (float)y, (float)z);
			var up = Vector3.Cross(right, forward);
			return Position + (right * offset.X) + (forward * offset.Y) + (up * offset.Z);
		}

		public Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 forward = Direction;
			const double d2R = 0.01745329251994329576923690768489;
			double num1 = System.Math.Cos(Rotation.Y * d2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * d2R);
			double y = num1 * System.Math.Sin(Rotation.Z * d2R);
			double z = System.Math.Sin(-Rotation.Y * d2R);
			var right = new Vector3((float)x, (float)y, (float)z);
			var up = Vector3.Cross(right, forward);
			Vector3 delta = worldCoords - Position;
			return new Vector3(Vector3.Dot(right, delta), Vector3.Dot(forward, delta), Vector3.Dot(up, delta));
		}

		public float FarClip
		{
			get => Function.Call<float>(Hash.GET_CAM_FAR_CLIP, Handle);
			set => Function.Call(Hash.SET_CAM_FAR_CLIP, Handle, value);
		}

		public float NearClip
		{
			get => Function.Call<float>(Hash.GET_CAM_NEAR_CLIP, Handle);
			set => Function.Call(Hash.SET_CAM_NEAR_CLIP, Handle, value);
		}

		public float FieldOfView
		{
			get => Function.Call<float>(Hash.GET_CAM_FOV, Handle);
			set => Function.Call(Hash.SET_CAM_FOV, Handle, value);
		}

		public float FarDepthOfField
		{
			get => Function.Call<float>(Hash.GET_CAM_FAR_DOF, Handle);
			set => Function.Call(Hash.SET_CAM_FAR_DOF, Handle, value);
		}

		public float NearDepthOfField
		{
			set => Function.Call(Hash.SET_CAM_NEAR_DOF, Handle, value);
		}

		public float MotionBlurStrength
		{
			set => Function.Call(Hash.SET_CAM_MOTION_BLUR_STRENGTH, Handle, value);
		}

		public float DepthOfFieldStrength
		{
			set => Function.Call(Hash.SET_CAM_DOF_STRENGTH, Handle, value);
		}

		public void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_CAM, Handle, s_shakeNames[(int)shakeType], amplitude);
		}

		public void StopShaking()
		{
			Function.Call(Hash.STOP_CAM_SHAKING, Handle, true);
		}

		public bool IsShaking
		{
			get => Function.Call<bool>(Hash.IS_CAM_SHAKING, Handle);
		}

		public float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_CAM_SHAKE_AMPLITUDE, Handle, value);
		}

		public void PointAt(Vector3 target)
		{
			Function.Call(Hash.POINT_CAM_AT_COORD, Handle, target.X, target.Y, target.Z);
		}
		public void PointAt(Entity target)
		{
			Function.Call(Hash.POINT_CAM_AT_ENTITY, Handle, target.Handle, 0.0f, 0.0f, 0.0f, true);
		}
		public void PointAt(Entity target, Vector3 offset)
		{
			Function.Call(Hash.POINT_CAM_AT_ENTITY, Handle, target.Handle, offset.X, offset.Y, offset.Z, true);
		}
		public void PointAt(Ped target, int boneIndex)
		{
			Function.Call(Hash.POINT_CAM_AT_PED_BONE, Handle, target.Handle, boneIndex, 0.0f, 0.0f, 0.0f, true);
		}
		public void PointAt(Ped target, int boneIndex, Vector3 offset)
		{
			Function.Call(Hash.POINT_CAM_AT_PED_BONE, Handle, target.Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
		}

		public void StopPointing()
		{
			Function.Call(Hash.STOP_CAM_POINTING, Handle);
		}

		public void InterpTo(Camera to, int duration, bool easePosition, bool easeRotation)
		{
			Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, to.Handle, Handle, duration, easePosition, easeRotation);
		}

		public bool IsInterpolating
		{
			get => Function.Call<bool>(Hash.IS_CAM_INTERPOLATING, Handle);
		}

		public void AttachTo(Entity entity, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_ENTITY, Handle, entity.Handle, offset.X, offset.Y, offset.Z, true);
		}
		public void AttachTo(Ped ped, int boneIndex, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_PED_BONE, Handle, ped.Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
		}

		public void Detach()
		{
			Function.Call(Hash.DETACH_CAM, Handle);
		}

		public void Destroy()
		{
			Function.Call(Hash.DESTROY_CAM, Handle, 0);
		}

		public bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_CAM_EXIST, Handle);
		}
		public static bool Exists(Camera camera)
		{
			return camera != null && camera.Exists();
		}

		public bool Equals(Camera obj)
		{
			return !(obj is null) && Handle == obj.Handle;
		}
		public override bool Equals(object obj)
		{
			return obj is not null && obj.GetType() == GetType() && Equals((Camera)obj);
		}

		public static bool operator ==(Camera left, Camera right)
		{
			return left?.Equals(right) ?? right is null;
		}
		public static bool operator !=(Camera left, Camera right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
