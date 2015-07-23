#pragma once

#include "Vector3.hpp"

namespace GTA
{
	ref class Ped;
	ref class Entity;

	public enum class CameraShake
	{
		Hand = 0,
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
	};

	public ref class Camera sealed
	{
	public:
		Camera(int handle);

		property int Handle
		{
			int get();
		}
		property float DepthOfFieldStrength
		{
			void set(float strength);
		}
		property float FieldOfView
		{
			float get();
			void set(float fov);
		}
		property float FarClip
		{
			float get();
			void set(float farClip);
		}
		property float FarDepthOfField
		{
			float get();
			void set(float farDOF);
		}
		property bool IsActive
		{
			bool get();
			void set(bool isActive);
		}
		property bool IsInterpolating
		{
			bool get();
		}
		property bool IsShaking
		{
			bool get();
			void set(bool isShaking);
		}
		property float MotionBlurStrength
		{
			void set(float strength);
		}
		property float NearClip
		{
			float get();
			void set(float nearClip);
		}
		property float NearDepthOfField
		{
			void set(float nearDOF);
		}
		property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 position);
		}
		property Math::Vector3 Rotation
		{
			Math::Vector3 get();
			void set(Math::Vector3 rotation);
		}
		property float ShakeAmplitude
		{
			float get();
			void set(float amplitude);
		}
		property CameraShake ShakeType
		{
			CameraShake get();
			void set(CameraShake shakeType);
		}

		void AttachTo(Entity ^entity, Math::Vector3 offset);
		void AttachTo(Ped ^entity, int boneIndex, Math::Vector3 offset);
		void Detach();

		void InterpTo(Camera ^to, int duration, bool easePosition, bool easeRotation);

		void PointAt(Math::Vector3 target);
		void PointAt(Entity ^target);
		void PointAt(Entity ^target, Math::Vector3 offset);
		void PointAt(Ped ^target, int boneIndex);
		void PointAt(Ped ^target, int boneIndex, Math::Vector3 offset);
		void StopPointing();

		bool Exists();
		void Destroy();

	internal:
		static initonly array<System::String ^> ^sShakeNames = { "HAND_SHAKE", "SMALL_EXPLOSION_SHAKE", "MEDIUM_EXPLOSION_SHAKE", "LARGE_EXPLOSION_SHAKE", "JOLT_SHAKE", "VIBRATE_SHAKE", "ROAD_VIBRATION_SHAKE", "DRUNK_SHAKE", "SKY_DIVING_SHAKE", "FAMILY5_DRUG_TRIP_SHAKE", "DEATH_FAIL_IN_EFFECT_SHAKE" };

	private:
		int mHandle;
		float mShakeAmplitude;
		CameraShake mShakeType;
	};
}