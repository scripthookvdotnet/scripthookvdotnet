#pragma once

#include "Camera.hpp"

namespace GTA
{
	public ref class GameplayCamera sealed abstract
	{
	public:
		static property float FieldOfView
		{
			float get();
		}
		[System::ObsoleteAttribute("GameplayCamera.FOV is obsolete, please use GameplayCamera.FieldOfView instead")]
		static property float FOV
		{
			float get()
			{
				return FieldOfView;
			}
		}
		static property bool IsAimCamActive
		{
			bool get();
		}
		static property bool IsFirstPersonAimCamActive
		{
			bool get();
		}
		static property bool IsLookingBehind
		{
			bool get();
		}
		static property bool IsRendering
		{
			bool get();
		}
		static property bool IsShaking
		{
			bool get();
		}
		static property Math::Vector3 Position
		{
			Math::Vector3 get();
		}
		static property float RelativeHeading
		{
			float get();
			void set(float relativeHeading);
		}
		static property float RelativePitch
		{
			float get();
			void set(float relativePitch);
		}
		static property Math::Vector3 Rotation
		{
			Math::Vector3 get();
		}
		static property float ShakeAmplitude
		{
			void set(float amplitude);
		}

		static void Shake(CameraShake shakeType, float amplitude);
		static void StopShaking();

		static void ClampYaw(float min, float max);
		static void ClampPitch(float min, float max);
	};
}