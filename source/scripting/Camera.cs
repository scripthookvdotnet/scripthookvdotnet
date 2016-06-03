using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public enum CameraShake
	{
		Hand,
		SmallExplosion,
		MediumExplosion,
		LargeExplosion,
		Jolt,
		Vibrate,
		RoadVibration,
		Drunk,
		SkyDiving,
		FamilyDrugTrip,
		DeathFail
	}

	public sealed class Camera : PoolObject, IEquatable<Camera>, ISpatial
	{
		#region Fields
		internal static readonly string[] _shakeNames = {
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
		#endregion

		public Camera(int handle) : base(handle)
		{
		}

		public bool IsActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_CAM_ACTIVE, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_ACTIVE, Handle, value);
			}
		}

		public Vector3 Position
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_CAM_COORD, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_COORD, Handle, value.X, value.Y, value.Z);
			}
		}
		public Vector3 Rotation
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_CAM_ROT, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_ROT, Handle, value.X, value.Y, value.Z);
			}
		}
		public Vector3 Direction
		{
			get
			{
				Vector3 rotation = Rotation;
				double num1 = rotation.X * 0.017453292519943295;
				double num2 = rotation.Z * 0.017453292519943295;
				double num3 = System.Math.Abs(System.Math.Cos(num1));
				return new Vector3((float)(-(System.Math.Sin(num2) * num3)), (float)(System.Math.Cos(num2) * num3), (float)System.Math.Sin(num1));
			}
			set
			{
				value.Normalize();
				Vector3 vector1 = new Vector3(value.X, value.Y, 0f);
				Vector3 vector2 = new Vector3(value.Z, vector1.Length(), 0f);
				Vector3 vector3 = Vector3.Normalize(vector2);
				Vector3 rotation = new Vector3((float)(System.Math.Atan2(vector3.X, vector3.Y) * 57.295779513082323), 0f, (float)(System.Math.Atan2(value.X, value.Y) * -57.295779513082323));
				Rotation = rotation;
			}
		}

		public Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 direction = Direction;
			double num1 = System.Math.Cos(Rotation.Y * 0.017453292519943295);
			double num2 = System.Math.Cos(-Rotation.Z * 0.017453292519943295) * num1;
			double num3 = System.Math.Sin(Rotation.Z * 0.017453292519943295) * num1;
			double num4 = System.Math.Sin(-Rotation.Y * 0.017453292519943295);
			Vector3 forward = new Vector3((float)num2, (float)num3, (float)num4);
			return Position + forward * offset.X + direction * offset.Y + Vector3.Cross(forward, direction) * offset.Z;
		}
		public Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 direction = Direction;
			double num = System.Math.Cos((double)Rotation.Y * 0.017453292519943295);
			double num2 = System.Math.Cos((double)(-(double)Rotation.Z) * 0.017453292519943295) * num;
			double num3 = System.Math.Sin((double)Rotation.Z * 0.017453292519943295) * num;
			double num4 = System.Math.Sin((double)(-(double)Rotation.Y) * 0.017453292519943295);
			Vector3 left = new Vector3((float)num2, (float)num3, (float)num4);
			Vector3 left2 = Vector3.Cross(left, direction);
			Vector3 position = Position;
			Vector3 right = worldCoords - position;
			Vector3 result = new Vector3(Vector3.Dot(left, right), Vector3.Dot(direction, right), Vector3.Dot(left2, right));
			return result;
		}

		public float FieldOfView
		{
			get
			{
				return Function.Call<float>(Hash.GET_CAM_FOV, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_FOV, Handle, value);
			}
		}

		public float NearClip
		{
			get
			{
				return Function.Call<float>(Hash.GET_CAM_NEAR_CLIP, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_NEAR_CLIP, Handle, value);
			}
		}
		public float FarClip
		{
			get
			{
				return Function.Call<float>(Hash.GET_CAM_FAR_CLIP, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_FAR_CLIP, Handle, value);
			}
		}

		public float NearDepthOfField
		{
			set
			{
				Function.Call(Hash.SET_CAM_NEAR_DOF, Handle, value);
			}
		}
		public float FarDepthOfField
		{
			get
			{
				return Function.Call<float>(Hash.GET_CAM_FAR_DOF, Handle);
			}
			set
			{
				Function.Call(Hash.SET_CAM_FAR_DOF, Handle, value);
			}
		}
		public float DepthOfFieldStrength
		{
			set
			{
				Function.Call(Hash.SET_CAM_DOF_STRENGTH, Handle, value);
			}
		}
		public float MotionBlurStrength
		{
			set
			{
				Function.Call(Hash.SET_CAM_MOTION_BLUR_STRENGTH, Handle, value);
			}
		}

		public void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_CAM, Handle, _shakeNames[(int)shakeType], amplitude);
		}
		public void StopShaking()
		{
			Function.Call(Hash.STOP_CAM_SHAKING, Handle, true);
		}
		public bool IsShaking
		{
			get
			{
				return Function.Call<bool>(Hash.IS_CAM_SHAKING, Handle);
			}
		}
		public float ShakeAmplitude
		{
			set
			{
				Function.Call(Hash.SET_CAM_SHAKE_AMPLITUDE, Handle, value);
			}
		}

		public void PointAt(Entity target)
		{
			PointAt(target, Vector3.Zero);
		}
		public void PointAt(Entity target, Vector3 offset)
		{
			Function.Call(Hash.POINT_CAM_AT_ENTITY, Handle, target.Handle, offset.X, offset.Y, offset.Z, true);
		}
		public void PointAt(Ped target, int boneIndex)
		{
			PointAt(target, boneIndex, Vector3.Zero);
		}
		public void PointAt(Ped target, int boneIndex, Vector3 offset)
		{
			Function.Call(Hash.POINT_CAM_AT_PED_BONE, Handle, target.Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
		}
		public void PointAt(Vector3 target)
		{
			Function.Call(Hash.POINT_CAM_AT_COORD, Handle, target.X, target.Y, target.Z);
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
			get
			{
				return Function.Call<bool>(Hash.IS_CAM_INTERPOLATING, Handle);
			}
		}

		public void AttachTo(Entity entity, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_ENTITY, Handle, entity.Handle, offset.X, offset.Y, offset.Z, true);
		}
		public void AttachTo(Ped entity, int boneIndex, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_PED_BONE, Handle, entity.Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
		}
		public void Detach()
		{
			Function.Call(Hash.DETACH_CAM, Handle);
		}

		public void Destroy()
		{
			Function.Call(Hash.DESTROY_CAM, Handle, 0);
		}

		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_CAM_EXIST, Handle);
		}
		public static bool Exists(Camera camera)
		{
			return !ReferenceEquals(camera, null) && camera.Exists();
		}

		public bool Equals(Camera camera)
		{
			return !ReferenceEquals(camera, null) && Handle == camera.Handle;
		}
		public override bool Equals(object obj)
		{
			return !ReferenceEquals(obj, null) && obj.GetType() == GetType() && Equals((Camera)obj);
		}

		public override int GetHashCode()
		{
			return Handle;
		}

		public static bool operator ==(Camera left, Camera right)
		{
			return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
		}
		public static bool operator !=(Camera left, Camera right)
		{
			return !(left == right);
		}
	}
	public static class GameplayCamera
	{
		public static Vector3 Position
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
			}
		}
		public static Vector3 Rotation
		{
			get
			{
				return Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_ROT, 2);
			}
		}
		public static Vector3 Direction
		{
			get
			{
				var rot = Rotation;
				double rotX = rot.X * 0.017453292519943295;
				double rotZ = rot.Z * 0.017453292519943295;
				double multXY = System.Math.Abs(System.Math.Cos(rotX));

				return new Vector3((float)(-System.Math.Sin(rotZ) * multXY), (float)(System.Math.Cos(rotZ) * multXY), (float)System.Math.Sin(rotX));
			}
		}

		public static Vector3 GetOffsetInWorldCoords(Vector3 offset)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.017453292519943295;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			return Position + (Right * offset.X) + (Forward * offset.Y) + (Up * offset.Z);
		}
		public static Vector3 GetOffsetFromWorldCoords(Vector3 worldCoords)
		{
			Vector3 Forward = Direction;
			const double D2R = 0.017453292519943295;
			double num1 = System.Math.Cos(Rotation.Y * D2R);
			double x = num1 * System.Math.Cos(-Rotation.Z * D2R);
			double y = num1 * System.Math.Sin(Rotation.Z * D2R);
			double z = System.Math.Sin(-Rotation.Y * D2R);
			Vector3 Right = new Vector3((float)x, (float)y, (float)z);
			Vector3 Up = Vector3.Cross(Right, Forward);
			Vector3 Delta = worldCoords - Position;
			return new Vector3(Vector3.Dot(Right, Delta), Vector3.Dot(Forward, Delta), Vector3.Dot(Up, Delta));
		}

		public static float RelativePitch
		{
			get
			{
				return Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_PITCH);
			}
			set
			{
				Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, value);
			}
		}
		public static float RelativeHeading
		{
			get
			{
				return Function.Call<float>(Hash.GET_GAMEPLAY_CAM_RELATIVE_HEADING);
			}
			set
			{
				Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_HEADING, value);
			}
		}

		public static void ClampYaw(float min, float max)
		{
			Function.Call(Hash._CLAMP_GAMEPLAY_CAM_YAW, min, max);
		}
		public static void ClampPitch(float min, float max)
		{
			Function.Call(Hash._CLAMP_GAMEPLAY_CAM_PITCH, min, max);
		}

		public static float Zoom
		{
			get
			{
				return Function.Call<float>(Hash._GET_GAMEPLAY_CAM_ZOOM);
			}
		}
		public static float FieldOfView
		{
			get
			{
				return Function.Call<float>(Hash.GET_GAMEPLAY_CAM_FOV);
			}
		}

		public static bool IsRendering
		{

			get
			{
				return Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_RENDERING);
			}
		}

		public static bool IsAimCamActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_AIM_CAM_ACTIVE);
			}
		}
		public static bool IsFirstPersonAimCamActive
		{
			get
			{
				return Function.Call<bool>(Hash.IS_FIRST_PERSON_AIM_CAM_ACTIVE);
			}
		}
		public static bool IsLookingBehind
		{

			get
			{
				return Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_LOOKING_BEHIND);
			}
		}

		public static void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_GAMEPLAY_CAM, Camera._shakeNames[(int)shakeType], amplitude);
		}
		public static void StopShaking()
		{
			Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
		}
		public static bool IsShaking
		{
			get
			{
				return Function.Call<bool>(Hash.IS_GAMEPLAY_CAM_SHAKING);
			}
		}
		public static float ShakeAmplitude
		{
			set
			{
				Function.Call(Hash.SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, value);
			}
		}
	}
}
