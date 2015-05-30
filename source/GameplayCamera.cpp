#include "Camera.hpp"
#include "GameplayCamera.hpp"
#include "Native.hpp"

namespace GTA
{
	Math::Vector3 GameplayCamera::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_COORD);
	}
	Math::Vector3 GameplayCamera::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_ROT, 2);
	}
	float GameplayCamera::FOV::get()
	{ 
		return Native::Function::Call<float>(Native::Hash::GET_GAMEPLAY_CAM_FOV);
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
	bool GameplayCamera::IsRendering::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_RENDERING);
	}
	bool GameplayCamera::IsLookingBehind::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_LOOKING_BEHIND);
	}
	bool GameplayCamera::IsAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_AIM_CAM_ACTIVE);
	}
	bool GameplayCamera::IsFirstPersonAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_FIRST_PERSON_AIM_CAM_ACTIVE);
	}
	void GameplayCamera::Shake(CameraShake shakeType, float amplitude)
	{
		Native::Function::Call(Native::Hash::SHAKE_GAMEPLAY_CAM, Camera::sShakeNames[static_cast<int>(shakeType)], amplitude);
	}
	bool GameplayCamera::IsShaking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_SHAKING);
	}
	void GameplayCamera::ShakeAmplitude::set(float amplitude)
	{
		Native::Function::Call(Native::Hash::SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, amplitude);
	}
	void GameplayCamera::StopShaking()
	{
		Native::Function::Call(Native::Hash::STOP_GAMEPLAY_CAM_SHAKING, true);
	}
	void GameplayCamera::ClampYaw(float min, float max)
	{
		Native::Function::Call(Native::Hash::CLAMP_GAMEPLAY_CAM_YAW, min, max);
	}
	void GameplayCamera::ClampPitch(float min, float max)
	{
		Native::Function::Call(Native::Hash::CLAMP_GAMEPLAY_CAM_PITCH, min, max);
	}
}