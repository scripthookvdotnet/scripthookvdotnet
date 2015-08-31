#include "GameplayCamera.hpp"
#include "Native.hpp"

namespace GTA
{
	Math::Vector3 GameplayCamera::Direction::get()
	{
		Math::Vector3 rot = Rotation;
		double rotX = rot.X / 57.295779513082320876798154814105;
		double rotZ = rot.Z / 57.295779513082320876798154814105;
		double multXY = System::Math::Abs(System::Math::Cos(rotX));

		return Math::Vector3(static_cast<float>(-System::Math::Sin(rotZ) * multXY), static_cast<float>(System::Math::Cos(rotZ) * multXY), static_cast<float>(System::Math::Sin(rotX)));
	}
	float GameplayCamera::FieldOfView::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_GAMEPLAY_CAM_FOV);
	}
	bool GameplayCamera::IsAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_AIM_CAM_ACTIVE);
	}
	bool GameplayCamera::IsFirstPersonAimCamActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_FIRST_PERSON_AIM_CAM_ACTIVE);
	}
	bool GameplayCamera::IsLookingBehind::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_LOOKING_BEHIND);
	}
	bool GameplayCamera::IsRendering::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_RENDERING);
	}
	bool GameplayCamera::IsShaking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_GAMEPLAY_CAM_SHAKING);
	}
	Math::Vector3 GameplayCamera::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_COORD);
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
	Math::Vector3 GameplayCamera::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_GAMEPLAY_CAM_ROT, 2);
	}
	void GameplayCamera::ShakeAmplitude::set(float amplitude)
	{
		Native::Function::Call(Native::Hash::SET_GAMEPLAY_CAM_SHAKE_AMPLITUDE, amplitude);
	}

	void GameplayCamera::Shake(CameraShake shakeType, float amplitude)
	{
		Native::Function::Call(Native::Hash::SHAKE_GAMEPLAY_CAM, Camera::sShakeNames[static_cast<int>(shakeType)], amplitude);
	}
	void GameplayCamera::StopShaking()
	{
		Native::Function::Call(Native::Hash::STOP_GAMEPLAY_CAM_SHAKING, true);
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