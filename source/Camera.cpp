#include "Camera.hpp"
#include "Ped.hpp"
#include "Native.hpp"

namespace GTA
{
	Camera::Camera(int handle) : mHandle(handle), mShakeType(CameraShake::Hand), mShakeAmplitude(1.0f)
	{
	}

	int Camera::Handle::get()
	{
		return this->mHandle;
	}

	bool Camera::Exists::get()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_CAM_EXIST, this->Handle);
	}
	bool Camera::IsActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_ACTIVE, this->Handle);
	}
	void Camera::IsActive::set(bool isActive)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ACTIVE, this->Handle, isActive);
	}
	Math::Vector3 Camera::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_CAM_COORD, this->Handle);
	}
	void Camera::Position::set(Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::SET_CAM_COORD, this->Handle, position.X, position.Y, position.Z);
	}
	Math::Vector3 Camera::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_CAM_ROT, this->Handle);
	}
	void Camera::Rotation::set(Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ROT, this->Handle, rotation.X, rotation.Y, rotation.Z);
	}
	float Camera::FOV::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FOV, this->Handle);
	}
	void Camera::FOV::set(float fov)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FOV, this->Handle, fov);
	}
	float Camera::NearClip::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_NEAR_CLIP, this->Handle);
	}
	void Camera::NearClip::set(float nearClip)
	{
		Native::Function::Call(Native::Hash::SET_CAM_NEAR_CLIP, this->Handle, nearClip);
	}
	float Camera::FarClip::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FAR_CLIP, this->Handle);
	}
	void Camera::FarClip::set(float farClip)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FAR_CLIP, this->Handle, farClip);
	}
	void Camera::NearDOF::set(float nearDOF)
	{
		Native::Function::Call(Native::Hash::SET_CAM_NEAR_DOF, this->Handle, nearDOF);
	}
	float Camera::FarDOF::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_CAM_FAR_DOF, this->Handle);
	}
	void Camera::FarDOF::set(float farDOF)
	{
		Native::Function::Call(Native::Hash::SET_CAM_FAR_DOF, this->Handle, farDOF);
	}
	void Camera::DOFStrength::set(float strength)
	{
		Native::Function::Call(Native::Hash::SET_CAM_DOF_STRENGTH, this->Handle, strength);
	}
	void Camera::MotionBlurStrength::set(float strength)
	{
		Native::Function::Call(Native::Hash::SET_CAM_MOTION_BLUR_STRENGTH, this->Handle, strength);
	}
	CameraShake Camera::ShakeType::get()
	{
		return this->mShakeType;
	}
	void Camera::ShakeType::set(CameraShake shakeType)
	{
		this->mShakeType = shakeType;

		if (this->IsShaking)
		{
			Native::Function::Call(Native::Hash::SHAKE_CAM, this->Handle, sShakeNames[static_cast<int>(this->ShakeType)], this->ShakeAmplitude);
		}
	}
	float Camera::ShakeAmplitude::get()
	{
		return mShakeAmplitude;
	}
	void Camera::ShakeAmplitude::set(float amplitude)
	{
		this->mShakeAmplitude = amplitude;
		Native::Function::Call(Native::Hash::SET_CAM_SHAKE_AMPLITUDE, this->Handle, amplitude);
	}
	bool Camera::IsShaking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_SHAKING, this->Handle);
	}
	void Camera::IsShaking::set(bool isShaking)
	{
		if (isShaking)
		{
			Native::Function::Call(Native::Hash::SHAKE_CAM, this->Handle, sShakeNames[static_cast<int>(this->ShakeType)], this->ShakeAmplitude);
		}
		else
		{
			Native::Function::Call(Native::Hash::STOP_CAM_SHAKING, this->Handle, true);
		}
	}
	bool Camera::IsInterpolating::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_CAM_INTERPOLATING, this->Handle);
	}
	void Camera::InterpTo(Camera ^to, int duration, bool easePosition, bool easeRotation)
	{
		Native::Function::Call(Native::Hash::SET_CAM_ACTIVE_WITH_INTERP, to->Handle, this->Handle, duration, easePosition, easeRotation);
	}
	void Camera::AttachTo(Entity ^entity, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::ATTACH_CAM_TO_ENTITY, this->Handle, entity->Handle, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::AttachTo(Ped ^ped, int boneIndex, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::ATTACH_CAM_TO_PED_BONE, this->Handle, ped->Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::Detach()
	{
		Native::Function::Call(Native::Hash::DETACH_CAM, this->Handle);
	}
	void Camera::Destroy()
	{
		Native::Function::Call(Native::Hash::DESTROY_CAM, this->Handle, 0);
	}
	void Camera::PointAt(Math::Vector3 target)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_COORD, this->Handle, target.X, target.Y, target.Z);
	}
	void Camera::PointAt(Entity ^target)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_ENTITY, this->Handle, target->Handle, 0.0f, 0.0f, 0.0f, true);
	}
	void Camera::PointAt(Entity ^target, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_ENTITY, this->Handle, target->Handle, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::PointAt(Ped ^target, int boneIndex)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_PED_BONE, this->Handle, target->Handle, boneIndex, 0.0f, 0.0f, 0.0f, true);
	}
	void Camera::PointAt(Ped ^target, int boneIndex, Math::Vector3 offset)
	{
		Native::Function::Call(Native::Hash::POINT_CAM_AT_PED_BONE, this->Handle, target->Handle, boneIndex, offset.X, offset.Y, offset.Z, true);
	}
	void Camera::StopPointing()
	{
		Native::Function::Call(Native::Hash::STOP_CAM_POINTING, this->Handle);
	}
}