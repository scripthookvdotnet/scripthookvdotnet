#pragma once

#include "Vector3.hpp"

namespace GTA
{
	enum class CameraShake;

	public ref class GameplayCamera sealed abstract
	{
	public:
		static property Math::Vector3 Position
		{
			Math::Vector3 get();
		}
		static property Math::Vector3 Rotation
		{
			Math::Vector3 get();
		}
		static property float FOV
		{
			float get();
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
		static property bool IsRendering
		{
			bool get();
		}
		static property bool IsLookingBehind
		{
			bool get();
		}
		static property bool IsAimCamActive
		{
			bool get();
		}
		static property bool IsFirstPersonAimCamActive
		{
			bool get();
		}
		
		static void Shake(CameraShake shakeType, float amplitude);
		static property bool IsShaking
		{
			bool get();
		}
		static property float ShakeAmplitude
		{
			void set(float amplitude);
		}
		static void StopShaking();

		static void ClampYaw(float min, float max);
		static void ClampPitch(float min, float max);

	};
}