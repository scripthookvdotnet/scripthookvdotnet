#include "Camera.hpp"
#include "Ped.hpp"
#include "Native.hpp"

namespace GTA
{
	Camera::Camera(int handle) : _handle(handle)
	{
	}

	int Camera::Handle::get()
	{
		return _handle;
	}
	void Camera::DepthOfFieldStrength::set(float strength)
	{
		Native::Function::Call(Native::Hash::SET_CAM_DOF_STRENGTH, Handle, strength);
	}
	Math::Vector3 Camera::Direction::get()
	{
		Math::Vector3 rot = Rotation;
		double rotX = rot.X / 57.295779513082320876798154814105;
		double rotZ = rot.Z / 57.295779513082320876798154814105;
		double multXY = System::Math::Abs(System::Math::Cos(rotX));

		return Math::Vector3(static_cast<float>(-System::Math::Sin(rotZ) * multXY), static_cast<float>(System::Math::Cos(rotZ) * multXY), static_cast<float>(System::Math::Sin(rotX)));
	}
	Math::Vector3 GameplayCamera::Direction::get()
	{
		Math::Vector3 rot = Rotation;
		double rotX = rot.X / 57.295779513082320876798154814105;
		double rotZ = rot.Z / 57.295779513082320876798154814105;
		double multXY = System::Math::Abs(System::Math::Cos(rotX));

		return Math::Vector3(static_cast<float>(-System::Math::Sin(rotZ) * multXY), static_cast<float>(System::Math::Cos(rotZ) * multXY), static_cast<float>(System::Math::Sin(rotX)));
	}
	void Camera::Direction::set(Math::Vector3 value)
	{
		value.Normalize();

		Math::Vector3 v1 = Math::Vector3::Normalize(Math::Vector3(value.Z, Math::Vector3(value.X, value.Y, 0.0f).Length(), 0.0f));

		Rotation = Math::Vector3(static_cast<float>(System::Math::Atan2(v1.X, v1.Y) * 57.295779513082320876798154814105), 0.0f, static_cast<float>(-System::Math::Atan2(value.X, value.Y) * 57.295779513082320876798154814105));
	}
	float Camera::FieldOfView::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FOV, Handle);
	}
	float GameplayCamera::FieldOfView::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_GAMEPLAY_CAM_FOV);
	}
	void Camera::FieldOfView::set(float fov)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FOV, Handle, fov);
	}
	float Camera::FarClip::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FAR_CLIP, Handle);
	}
	void Camera::FarClip::set(float farClip)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FAR_CLIP, Handle, farClip);
	}
	float Camera::FarDepthOfField::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FAR_DOF, Handle);
	}
	void Camera::FarDepthOfField::set(float farDOF)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FAR_DOF, Handle, farDOF);
	}
	bool Camera::IsActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_ACTIVE, Handle);
	}
	void Camera::IsActive::set(bool isActive)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ACTIVE, Handle, isActive);
	}
	bool GameplayCamera::IsAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_AIM_CAM_ACTIVE);
	}
	bool GameplayCamera::IsFirstPersonAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_FIRST_PERSON_AIM_CAM_ACTIVE);
	}
	bool Camera::IsInterpolating::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_INTERPOLATING, Handle);
	}
	bool GameplayCamera::IsLookingBehind::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_LOOKING_BEHIND);
	}
	bool GameplayCamera::IsRendering::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_RENDERING);
	}
	bool Camera::IsShaking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_SHAKING, Handle);
	}
	bool GameplayCamera::IsShaking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_SHAKING);
	}
	void Camera::MotionBlurStrength::set(float strength)
	{
		Native::Function::Call(Native::Hash::SET_CAM_MOTION_BLUR_STRENGTH, Handle, strength);
	}
	float Camera::NearClip::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_NEAR_CLIP, Handle);
	}
	void Camera::NearClip::set(float nearClip)
	{
		Native::Function::Call(Native::Hash::SET_CAM_NEAR_CLIP, Handle, nearClip);
	}
	void Camera::NearDepthOfField::set(float nearDOF)
	{
		Native::Function::Call(Native::Hash::SET_CAM_NEAR_DOF, Handle, nearDOF);
	}
	Math::Vector3 Camera::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_CAM_COORD, Handle);
	}
	Math::Vector3 GameplayCamera::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_COORD);
	}
	void Camera::Position::set(Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::SET_CAM_COORD, Handle, position.X, position.Y, position.Z);
	}
	float GameplayCamera::RelativeHeading::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_GAMEPLAY_CAM_RELATIVE_HEADING);
	}
	void GameplayCamera::RelativeHeading::set(float relativeHeading)
	{
		Native::Function::Call(Native::Hash::SET_GAMEPLAY_CAM_RELATIVE_HEADING, relativeHeading);
	}
	float GameplayCamera::RelativePitch::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_GAMEPLAY_CAM_RELATIVE_PITCH);
	}
	void GameplayCamera::RelativePitch::set(float relativePitch)
	{
		Native::Function::Call(Native::Hash::SET_GAMEPLAY_CAM_RELATIVE_PITCH, relativePitch);
	}
	Math::Vector3 Camera::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_CAM_ROT, Handle);
	}
	Math::Vector3 GameplayCamera::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_ROT, 2);
	}
	void Camera::Rotation::set(Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ROT, Handle, rotation.X, rotation.Y, rotation.Z);
	}
	void Camera::ShakeAmplitude::set(float amplitude)
	{
		Native::Function::Call(Native::Hash::SET_CAM_SHAKE_AMPLITUDE, Handle, amplitude);
	}
	void GameplayCamera::ShakeAmplitude::set(float amplitude)
	{
		Native::Function::Call(Native::Hash::SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, amplitude);
	}

	void Camera::Shake(CameraShake shakeType, float amplitude)
	{
		Native::Function::Call(Native::Hash::SHAKE_CAM, Handle, _shakeNames[static_cast<int>(shakeType)], amplitude);
	}
	void GameplayCamera::Shake(CameraShake shakeType, float amplitude)
	{
		Native::Function::Call(Native::Hash::SHAKE_GAMEPLAY_CAM, Camera::_shakeNames[static_cast<int>(shakeType)], amplitude);
	}
	void Camera::StopShaking()
	{
		Native::Function::Call(Native::Hash::STOP_CAM_SHAKING, Handle, true);
	}
	void GameplayCamera::StopShaking()
	{
		Native::Function::Call(Native::Hash::STOP_GAMEPLAY_CAM_SHAKING, true);
	}

	void Camera::AttachTo(Entity ^entity, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::ATTACH_CAM_TO_ENTITY, Handle, entity->Handle, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::AttachTo(Ped ^ped, int boneIndex, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::ATTACH_CAM_TO_PED_BONE, Handle, ped->Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::Detach()
	{
		Native::Function::Call(Native::Hash::DETACH_CAM, Handle);
	}

	void Camera::InterpTo(Camera ^to, int duration, bool easePosition, bool easeRotation)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ACTIVE_WITH_INTERP, to->Handle, Handle, duration, easePosition, easeRotation);
	}

	void Camera::PointAt(Math::Vector3 target)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_COORD, Handle, target.X, target.Y, target.Z);
	}
	void Camera::PointAt(Entity ^target)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_ENTITY, Handle, target->Handle, 0.0f, 0.0f, 0.0f, true);
	}
	void Camera::PointAt(Entity ^target, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_ENTITY, Handle, target->Handle, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::PointAt(Ped ^target, int boneIndex)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_PED_BONE, Handle, target->Handle, boneIndex, 0.0f, 0.0f, 0.0f, true);
	}
	void Camera::PointAt(Ped ^target, int boneIndex, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_PED_BONE, Handle, target->Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::StopPointing()
	{
		Native::Function::Call(Native::Hash::STOP_CAM_POINTING, Handle);
	}

	bool Camera::Exists()
	{
		return Exists(this);
	}
	bool Camera::Exists(Camera ^camera)
	{
		return !Object::ReferenceEquals(camera, nullptr) && Native::Function::Call<bool>(Native::Hash::DOES_CAM_EXIST, camera->Handle);
	}
	void Camera::Destroy()
	{
		Native::Function::Call(Native::Hash::DESTROY_CAM, Handle, 0);
	}
	bool Camera::Equals(Camera ^camera)
	{
		return !System::Object::ReferenceEquals(camera, nullptr) && Handle == camera->Handle;
	}

	void GameplayCamera::ClampYaw(float min, float max)
	{
		Native::Function::Call(Native::Hash::_CLAMP_GAMEPLAY_CAM_YAW, min, max);
	}
	void GameplayCamera::ClampPitch(float min, float max)
	{
		Native::Function::Call(Native::Hash::_CLAMP_GAMEPLAY_CAM_PITCH, min, max);
	}
}