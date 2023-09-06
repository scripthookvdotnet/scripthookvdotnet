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
	/// Represents a scripted camera.
	/// </summary>
	public sealed class Camera : PoolObject, ISpatial
	{
		internal static readonly string[] s_shakeNames =
		{
			"HAND_SHAKE", "SMALL_EXPLOSION_SHAKE", "MEDIUM_EXPLOSION_SHAKE", "LARGE_EXPLOSION_SHAKE", "JOLT_SHAKE",
			"VIBRATE_SHAKE", "ROAD_VIBRATION_SHAKE", "DRUNK_SHAKE", "SKY_DIVING_SHAKE", "FAMILY5_DRUG_TRIP_SHAKE",
			"DEATH_FAIL_IN_EFFECT_SHAKE", "WOBBLY_SHAKE",
		};

		public Camera(int handle) : base(handle)
		{
		}

		/// <inheritdoc cref="Create(ScriptedCameraNameHash, Vector3, Vector3, float, bool, EulerRotationOrder)"/>
		public static Camera Create(ScriptedCameraNameHash cameraNameHash, bool startActivated = false)
		{
			int handle = Function.Call<int>(Hash.CREATE_CAMERA, (uint)cameraNameHash, startActivated);
			return handle > 0 ? new Camera(handle) : null;
		}
		/// <inheritdoc cref="Create(string, Vector3, Vector3, float, bool, EulerRotationOrder)"/>
		public static Camera Create(string cameraName, bool startActivated = false)
		{
			int handle = Function.Call<int>(Hash.CREATE_CAM, cameraName, startActivated);
			return handle > 0 ? new Camera(handle) : null;
		}
		/// <summary>
		/// Creates a scripted <see cref="Camera"/> of a given name hash.
		/// </summary>
		/// <param name="cameraNameHash">
		/// The camera name hash.
		/// Passing a invalid name hash will result in the <see langword="null"/> return value.
		/// It would result in some unintended behaviors such as the camera position not being able to change
		/// if you pass a camera name hash whose metadata is not designed for scripted cameras.
		/// </param>
		/// <param name="position">The position of the camera.</param>
		/// <param name="rotation">The rotation of the camera.</param>
		/// <param name="fov">The field of view of the camera.</param>
		/// <param name="startActivated">
		/// If <see langword="true"/>, the created camera will be activated upon creation.
		/// </param>
		/// <param name="rotOrder">The rotation order in world space.</param>
		/// <returns>
		/// A new <see cref="Camera"/> instance if the method successfully created a new <see cref="Camera"/>;
		/// otherwise, <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// The method will fail to create a scripted <see cref="Camera"/> if the passed camera name is invalid
		/// or the camera pool is full. The method will return <see langword="null"/> in said conditions.
		/// </remarks>
		public static Camera Create(ScriptedCameraNameHash cameraNameHash, Vector3 position, Vector3 rotation,
			float fov = 65.0f, bool startActivated = false, EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
		{
			int handle = Function.Call<int>(Hash.CREATE_CAMERA_WITH_PARAMS, (uint)cameraNameHash, position.X,
				position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, startActivated, (int)rotOrder);
			return handle > 0 ? new Camera(handle) : null;
		}
		/// <summary>
		/// Creates a scripted <see cref="Camera"/> of a given name.
		/// </summary>
		/// <param name="cameraName">
		/// The camera name.
		/// Passing a invalid name will result in the <see langword="null"/> return value.
		/// It would result in some unintended behaviors such as the camera position not being able to change
		/// if you pass a camera name hash whose metadata is not designed for scripted cameras.
		/// </param>
		/// <param name="position">The position of the camera.</param>
		/// <param name="rotation">The rotation of the camera.</param>
		/// <param name="fov">The field of view of the camera.</param>
		/// <param name="startActivated">
		/// If <see langword="true"/>, the created camera will be activated upon creation.
		/// </param>
		/// <param name="rotOrder">The rotation order in world space.</param>
		/// <returns>
		/// A new <see cref="Camera"/> instance if the method successfully created a new <see cref="Camera"/>;
		/// otherwise, <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// The method will fail to create a scripted <see cref="Camera"/> if the passed camera name is invalid
		/// or the camera pool is full. The method will return <see langword="null"/> in said conditions.
		/// </remarks>
		public static Camera Create(string cameraName, Vector3 position, Vector3 rotation,
			float fov = 65.0f, bool startActivated = false, EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
		{
			int handle = Function.Call<int>(Hash.CREATE_CAM_WITH_PARAMS, cameraName, position.X,
				position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, fov, startActivated, (int)rotOrder);
			return handle > 0 ? new Camera(handle) : null;
		}

		/// <summary>
		/// Gets the memory address of this <see cref="Camera"/>.
		/// </summary>
		public IntPtr MemoryAddress => SHVDN.NativeMemory.GetCameraAddress(Handle);

		/// <summary>
		/// Gets the memory address of the matrix for this <see cref="Camera"/>.
		/// </summary>
		IntPtr MatrixAddress
		{
			get
			{
				IntPtr address = MemoryAddress;
				if (address == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}

				return (SHVDN.NativeMemory.ReadByte(address + 0x220) & 1) == 0 ? address + 0x30 : address + 0x110;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Camera"/> is active.
		/// Returning <see langword="false"/> means this <see cref="Camera"/> is not rendering to the game screen,
		/// but returning <see langword="true"/> does not mean it's rendering to the screen.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if this <see cref="Camera"/> is active; otherwise, <see langword="false" />.
		/// </value>
		public bool IsActive
		{
			get => Function.Call<bool>(Hash.IS_CAM_ACTIVE, Handle);
			set => Function.Call(Hash.SET_CAM_ACTIVE, Handle, value);
		}

		/// <summary>
		/// Gets the matrix of this <see cref="Camera"/>.
		/// </summary>
		public Matrix Matrix
		{
			get
			{
				IntPtr address = MatrixAddress;
				if (address == IntPtr.Zero)
				{
					return new Matrix();
				}

				return new Matrix(SHVDN.NativeMemory.ReadMatrix(address));
			}
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="Camera"/>.
		/// </summary>
		public Vector3 Position
		{
			get => Function.Call<Vector3>(Hash.GET_CAM_COORD, Handle);
			set => Function.Call(Hash.SET_CAM_COORD, Handle, value.X, value.Y, value.Z);
		}

		/// <summary>
		/// Gets or sets the rotation of this <see cref="Camera"/>.
		/// </summary>
		/// <value>
		/// The yaw, pitch and roll rotations measured in degrees.
		/// </value>
		public Vector3 Rotation
		{
			get => Function.Call<Vector3>(Hash.GET_CAM_ROT, Handle, 2);
			set => Function.Call(Hash.SET_CAM_ROT, Handle, value.X, value.Y, value.Z, 2);
		}

		/// <summary>
		/// Gets or sets the direction this <see cref="Camera"/> is pointing in.
		/// </summary>
		public Vector3 Direction
		{
			get => ForwardVector;
			set
			{
				value.Normalize();

				var vector1 = new Vector3(value.X, value.Y, 0f);
				var vector2 = new Vector3(value.Z, vector1.Length(), 0f);
				var vector3 = Vector3.Normalize(vector2);
				Rotation = new Vector3((float)(System.Math.Atan2(vector3.X, vector3.Y) * 57.295779513082323), Rotation.Y, (float)(System.Math.Atan2(value.X, value.Y) * -57.295779513082323));
			}
		}

		/// <summary>
		/// Gets the up vector of this <see cref="Camera"/>.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				IntPtr address = MatrixAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeTop;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x20));
			}
		}

		/// <summary>
		/// Gets the right vector of this <see cref="Camera"/>.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				IntPtr address = MatrixAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeRight;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address));
			}
		}

		/// <summary>
		/// Gets the forward vector of this <see cref="Camera"/>, see also <seealso cref="Direction"/>.
		/// </summary>
		public Vector3 ForwardVector
		{
			get
			{
				IntPtr address = MatrixAddress;
				if (address == IntPtr.Zero)
				{
					return Vector3.RelativeFront;
				}

				return new Vector3(SHVDN.NativeMemory.ReadVector3(address + 0x10));
			}
		}

		/// <summary>
		/// Gets the position in world coordinates of an offset relative to this <see cref="Camera"/>
		/// </summary>
		/// <param name="offset">The offset from this <see cref="Camera"/>.</param>
		public Vector3 GetOffsetPosition(Vector3 offset)
		{
			return Matrix.TransformPoint(offset);
		}

		/// <summary>
		/// Gets the relative offset of this <see cref="Camera"/> from a world coordinates position
		/// </summary>
		/// <param name="worldCoords">The world coordinates.</param>
		public Vector3 GetPositionOffset(Vector3 worldCoords)
		{
			return Matrix.InverseTransformPoint(worldCoords);
		}
		/// <summary>
		/// Sets the camera's position, rotation and field of view.
		/// If <paramref name="duration"/> is greater than zero,
		/// the <see cref="Camera"/> will interp to the specified settings.
		/// </summary>
		/// <param name="position">The target position.</param>
		/// <param name="rotation">The target rotation.</param>
		/// <param name="fov">The target field of view.</param>
		/// <param name="duration">The duration of any interpolation in milliseconds.</param>
		/// <param name="graphTypePos">The camera graph type for position transition.</param>
		/// <param name="graphTypeRot">The camera graph type for rotation transition.</param>
		/// <param name="rotOrder">The rotation order in world space.</param>
		public void TransitionTo(Vector3 position, Vector3 rotation, float fov, int duration,
			CameraGraphType graphTypePos = CameraGraphType.SinAccelDecel,
			CameraGraphType graphTypeRot = CameraGraphType.SinAccelDecel,
			EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.SET_CAM_PARAMS, Handle, position.X, position.Y, position.Z, rotation.X, rotation.Y,
				rotation.Z,
				fov, duration, (int)graphTypePos, (int)graphTypeRot, (int)rotOrder);
		}

		/// <summary>
		/// Gets or sets the far clip distance of this <see cref="Camera"/> in meters.
		/// </summary>
		public float FarClip
		{
			get => Function.Call<float>(Hash.GET_CAM_FAR_CLIP, Handle);
			set => Function.Call(Hash.SET_CAM_FAR_CLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets the near clip distance of this <see cref="Camera"/> in meters.
		/// </summary>
		public float NearClip
		{
			get => Function.Call<float>(Hash.GET_CAM_NEAR_CLIP, Handle);
			set => Function.Call(Hash.SET_CAM_NEAR_CLIP, Handle, value);
		}

		/// <summary>
		/// Gets or sets the field of view of this <see cref="Camera"/> in degrees.
		/// </summary>
		public float FieldOfView
		{
			get => Function.Call<float>(Hash.GET_CAM_FOV, Handle);
			set => Function.Call(Hash.SET_CAM_FOV, Handle, value);
		}

		/// <summary>
		/// Gets or sets the far depth of field distance of this <see cref="Camera"/> in meters.
		/// </summary>
		public float FarDepthOfField
		{
			get => Function.Call<float>(Hash.GET_CAM_FAR_DOF, Handle);
			set => Function.Call(Hash.SET_CAM_FAR_DOF, Handle, value);
		}

		/// <summary>
		/// Sets the near depth of field distance for this <see cref="Camera"/> in meters.
		/// </summary>
		public float NearDepthOfField
		{
			set => Function.Call(Hash.SET_CAM_NEAR_DOF, Handle, value);
		}

		/// <summary>
		/// Sets the strength of the motion blur for this <see cref="Camera"/>.
		/// </summary>
		public float MotionBlurStrength
		{
			set => Function.Call(Hash.SET_CAM_MOTION_BLUR_STRENGTH, Handle, value);
		}

		/// <summary>
		/// Sets the depth of field strength for this <see cref="Camera"/>.
		/// </summary>
		public float DepthOfFieldStrength
		{
			set => Function.Call(Hash.SET_CAM_DOF_STRENGTH, Handle, value);
		}

		/// <summary>
		/// Apples a predefined shake to the camera.
		/// </summary>
		/// <param name="shakeType">Type of the shake to apply.</param>
		/// <param name="amplitude">The amplitude of the shaking.</param>
		public void Shake(CameraShake shakeType, float amplitude)
		{
			Function.Call(Hash.SHAKE_CAM, Handle, s_shakeNames[(int)shakeType], amplitude);
		}
		/// <summary>
		/// Apples a predefined shake to the camera.
		/// </summary>
		/// <param name="shakeName">
		/// The shake name to apply.
		/// You can find shake names in cameras.ymt, search for <c>camShakeMetadata</c>.
		/// </param>
		/// <param name="amplitudeScaler">The amplitude of the shaking.</param>
		public void Shake(string shakeName, float amplitudeScaler)
		{
			Function.Call(Hash.SHAKE_CAM, Handle, shakeName, amplitudeScaler);
		}

		/// <summary>
		/// Immediately stops shaking this <see cref="Camera"/>.
		/// </summary>
		public void StopShaking() => StopShaking(true);
		/// <summary>
		/// Stops shaking this <see cref="Camera"/>.
		/// </summary>
		/// <param name="stopImmediately">
		/// If <see langword="true"/>, the shake will stop immediately, otherwise it will enter its release phase and fade out.
		/// </param>
		public void StopShaking(bool stopImmediately)
			=> Function.Call(Hash.STOP_CAM_SHAKING, Handle, stopImmediately);

		/// <summary>
		/// Gets a value indicating whether this <see cref="Camera"/> is shaking.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Camera"/> is shaking; otherwise, <see langword="false" />.
		/// </value>
		public bool IsShaking => Function.Call<bool>(Hash.IS_CAM_SHAKING, Handle);

		/// <summary>
		/// Sets the shake amplitude for this <see cref="Camera"/>.
		/// </summary>
		public float ShakeAmplitude
		{
			set => Function.Call(Hash.SET_CAM_SHAKE_AMPLITUDE, Handle, value);
		}

		/// <summary>
		/// Points this <see cref="Camera"/> at a specified <see cref="Entity"/>.
		/// </summary>
		/// <param name="target">The <see cref="Entity"/> to point at.</param>
		/// <param name="offset">The offset from the <paramref name="target"/> to point at.</param>
		public void PointAt(Entity target, Vector3 offset = default)
		{
			Function.Call(Hash.POINT_CAM_AT_ENTITY, Handle, target.Handle, offset.X, offset.Y, offset.Z, true);
		}
		/// <summary>
		/// Points this <see cref="Camera"/> at a specified <see cref="PedBone"/>.
		/// </summary>
		/// <param name="target">The <see cref="PedBone"/> to point at.</param>
		/// <param name="offset">The offset from the <paramref name="target"/> to point at</param>
		public void PointAt(PedBone target, Vector3 offset = default)
		{
			Function.Call(Hash.POINT_CAM_AT_PED_BONE, Handle, target.Owner.Handle, target, offset.X, offset.Y, offset.Z, true);
		}
		/// <summary>
		/// Points this <see cref="Camera"/> at a specified position.
		/// </summary>
		/// <param name="target">The position to point at.</param>
		public void PointAt(Vector3 target)
		{
			Function.Call(Hash.POINT_CAM_AT_COORD, Handle, target.X, target.Y, target.Z);
		}

		/// <summary>
		/// Stops this <see cref="Camera"/> pointing at a specific target.
		/// </summary>
		public void StopPointing()
		{
			Function.Call(Hash.STOP_CAM_POINTING, Handle);
		}

		/// <summary>
		/// Sets a cam active which will be interpolated too from this <see cref="Camera"/>.
		/// </summary>
		/// <param name="destinationCam">The <see cref="Camera"/> that will be interpolated to.</param>
		/// <param name="duration">The duration of any interpolation in milliseconds.</param>
		/// <param name="graphTypePos">The camera graph type for position transition.</param>
		/// <param name="graphTypeRot">The camera graph type for rotation transition.</param>
		/// <param name="rotOrder">The rotation order in world space.</param>
		public void InterpTo(Camera destinationCam, int duration,
			CameraGraphType graphTypePos = CameraGraphType.SinAccelDecel,
			CameraGraphType graphTypeRot = CameraGraphType.SinAccelDecel,
			EulerRotationOrder rotOrder = EulerRotationOrder.YXZ)
		{
			Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, destinationCam.Handle, Handle, duration, (int)graphTypePos,
				(int)graphTypeRot, (int)rotOrder);
		}
		/// <summary>
		/// Sets a cam active which will be interpolated too from this <see cref="Camera"/>.
		/// </summary>
		[Obsolete("Use Camera.InterpTo(Camera, int, CameraGraphType, CameraGraphType, EulerRotationOrder) instead."),
		EditorBrowsable(EditorBrowsableState.Never)]
		public void InterpTo(Camera to, int duration, int easePosition, int easeRotation)
		{
			Function.Call(Hash.SET_CAM_ACTIVE_WITH_INTERP, to.Handle, Handle, duration, easePosition, easeRotation);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Camera"/> is interpolating.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if this <see cref="Camera"/> is interpolating; otherwise, <see langword="false" />.
		/// </value>
		public bool IsInterpolating => Function.Call<bool>(Hash.IS_CAM_INTERPOLATING, Handle);

		/// <summary>
		/// Attaches this <see cref="Camera"/> to a specific <see cref="Entity"/>.
		/// </summary>
		/// <param name="entity">The <see cref="Entity"/> to attach to.</param>
		/// <param name="offset">The relative offset from the <paramref name="entity"/> to attach to.</param>
		public void AttachTo(Entity entity, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_ENTITY, Handle, entity.Handle, offset.X, offset.Y, offset.Z, true);
		}
		/// <summary>
		/// Attaches this <see cref="Camera"/> to a specific <see cref="PedBone"/>.
		/// </summary>
		/// <param name="pedBone">The <see cref="PedBone"/> to attach to.</param>
		/// <param name="offset">The relative offset from the <paramref name="pedBone"/> to attach to.</param>
		public void AttachTo(PedBone pedBone, Vector3 offset)
		{
			Function.Call(Hash.ATTACH_CAM_TO_PED_BONE, Handle, pedBone.Owner.Handle, pedBone, offset.X, offset.Y, offset.Z, true);
		}

		/// <summary>
		/// Attaches this <see cref="Camera"/> to a specific bone on a specific <see cref="Vehicle"/>.
		/// the camera will only copy translation components of the transformation matrix of
		/// <paramref name="vehicleBone"/>.
		/// </summary>
		/// <param name="vehicleBone">The <see cref="Vehicle"/> bone to attach the camera to.</param>
		/// <param name="positionOffset">The additional offset to be applied from the attach position.</param>
		/// <param name="offsetIsRelative">
		/// If true, <paramref name="positionOffset"/> are applied relative to the transform of the attached
		/// <see cref="Vehicle"/> (not the bone), rather than in world-space. Effective as long as the
		/// <see cref="Camera"/> is attached.
		/// </param>
		/// <exception cref="GameVersionNotSupportedException">Thrown when called in v1.0.1290.1 or earlier game versions.</exception>
		public void AttachToVehicleBone(EntityBone vehicleBone, Vector3 positionOffset, bool offsetIsRelative = true)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1290_1_Steam, nameof(Camera), nameof(AttachToVehicleBone));

			// The rotational offsets have effect only if hard attachment flag is set
			Function.Call(Hash.ATTACH_CAM_TO_VEHICLE_BONE, Handle, vehicleBone.Owner.Handle, vehicleBone.Index,
				false /* hardAttachment */, 0f, 0f, 0f, positionOffset.X, positionOffset.Y, positionOffset.Z,
				offsetIsRelative);
		}

		/// <summary>
		/// Attaches this <see cref="Camera"/> to a specific bone on a specific <see cref="Vehicle"/>.
		/// Tthe camera will have both translation and rotation vector components of its transformation matrix match
		/// that of <paramref name="vehicleBone"/>.
		/// </summary>
		/// <param name="vehicleBone">The <see cref="Vehicle"/> bone to attach the camera to.</param>
		/// <param name="rotationOffset">
		/// An additional rotational offset to be applied from the attach bone rotation (x=yaw, y=pitch, z=roll).
		/// </param>
		/// <param name="positionOffset">The additional offset to be applied from the attach position.</param>
		/// <param name="offsetIsRelative">
		/// If true, <paramref name="positionOffset"/> are applied relative to the transform of the attached
		/// <see cref="Vehicle"/> (not the bone), rather than in world-space. Effective as long as the
		/// <see cref="Camera"/> is attached. <paramref name="rotationOffset"/> is always relative to the transform of
		/// the bone even if this parameter is set to <see langword="true"/>.
		/// </param>
		/// <exception cref="GameVersionNotSupportedException">Thrown when called in v1.0.1290.1 or earlier game versions.</exception>
		public void HardAttachToVehicleBone(EntityBone vehicleBone, Vector3 rotationOffset,
			Vector3 positionOffset, bool offsetIsRelative = true)
		{
			GameVersionNotSupportedException.ThrowIfNotSupported(GameVersion.v1_0_1290_1_Steam, nameof(Camera), nameof(AttachToVehicleBone));

			Function.Call(Hash.ATTACH_CAM_TO_VEHICLE_BONE, Handle, vehicleBone.Owner.Handle, vehicleBone.Index,
				true /* hardAttachment */, rotationOffset.X, rotationOffset.Y, rotationOffset.Z, positionOffset.X,
				positionOffset.Y, positionOffset.Z, offsetIsRelative);
		}

		/// <summary>
		/// Detaches this <see cref="Camera"/> from any <see cref="Entity"/> or <see cref="PedBone"/> it may be attached to.
		/// </summary>
		public void Detach()
		{
			Function.Call(Hash.DETACH_CAM, Handle);
		}

		/// <summary>
		/// Destroys this <see cref="Camera"/>.
		/// </summary>
		public override void Delete()
		{
			Function.Call(Hash.DESTROY_CAM, Handle, 0);
		}
		/// <summary>
		/// Destroys this <see cref="Camera"/>.
		/// </summary>
		/// <param name="shouldApplyAcrossAllThreads">
		/// If <see langword="true"/>, a request to stop rendering will be enforced irrespective of whether other
		/// script threads (<c>GtaThread</c>s) expect rendering to be active.
		/// Note that this can result in conflicts between concurrent script threads, so this must be used with caution.
		/// </param>
		public void Delete(bool shouldApplyAcrossAllThreads)
		{
			Function.Call(Hash.DESTROY_CAM, Handle, shouldApplyAcrossAllThreads);
		}
		/// <summary>
		/// Destroys all scripted <see cref="Camera"/>s.
		/// </summary>
		/// <param name="shouldApplyAcrossAllThreads">
		/// If <see langword="true"/>, a request to stop rendering will be enforced irrespective of whether other
		/// script threads (<c>GtaThread</c>s) expect rendering to be active.
		/// Note that this can result in conflicts between concurrent script threads, so this must be used with caution.
		/// </param>
		public static void DeleteAllCameras(bool shouldApplyAcrossAllThreads = false)
		{
			Function.Call(Hash.DESTROY_ALL_CAMS, shouldApplyAcrossAllThreads);
		}

		/// <summary>
		/// Determines if this <see cref="Camera"/> exists.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Camera"/> exists; otherwise, <see langword="false" />.</returns>
		public override bool Exists()
		{
			return Function.Call<bool>(Hash.DOES_CAM_EXIST, Handle);
		}

		/// <summary>
		/// Determines if an <see cref="object"/> refers to the same camera as this <see cref="Camera"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><see langword="true" /> if the <paramref name="obj"/> is the same camera as this <see cref="Camera"/>; otherwise, <see langword="false" />.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Camera camera)
			{
				return Handle == camera.Handle;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="Camera"/>s refer to the same camera.
		/// </summary>
		/// <param name="left">The left <see cref="Camera"/>.</param>
		/// <param name="right">The right <see cref="Camera"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is the same camera as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator ==(Camera left, Camera right)
		{
			return left?.Equals(right) ?? right is null;
		}
		/// <summary>
		/// Determines if two <see cref="Checkpoint"/>s don't refer to the same camera.
		/// </summary>
		/// <param name="left">The left <see cref="Camera"/>.</param>
		/// <param name="right">The right <see cref="Camera"/>.</param>
		/// <returns><see langword="true" /> if <paramref name="left"/> is not the same camera as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
		public static bool operator !=(Camera left, Camera right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts a <see cref="Camera"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(Camera value)
		{
			return new InputArgument((ulong)(value?.Handle ?? 0));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
