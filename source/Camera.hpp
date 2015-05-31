#pragma once

#include "Ped.hpp"
#include "Vector3.hpp"

namespace GTA
{
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
		property bool Exists
		{
			bool get();
		}
		property bool IsActive
		{
			bool get();
			void set(bool isActive);
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
		property float FOV
		{
			float get();
			void set(float fov);
		}
		property float NearClip
		{
			float get();
			void set(float nearClip);
		}
		property float FarClip
		{
			float get();
			void set(float farClip);
		}
		property float NearDOF
		{
			void set(float nearDOF);
		}
		property float FarDOF
		{
			float get();
			void set(float farDOF);
		}
		property float DOFStrength
		{
			void set(float strength);
		}
		property float MotionBlurStrength
		{
			void set(float strength);
		}
		property bool IsInterpolating
		{
			bool get();
		}
		property CameraShake ShakeType
		{
			CameraShake get();
			void set(CameraShake shakeType);
		}
		property float ShakeAmplitude
		{
			float get();
			void set(float amplitude);
		}
		property bool IsShaking
		{
			bool get();
			void set(bool isShaking);
		}

		void InterpTo(Camera ^to, int duration, bool easePosition, bool easeRotation);

		void AttachTo(Entity ^entity, Math::Vector3 offset);
		void AttachTo(Ped ^entity, int boneIndex, Math::Vector3 offset);
		void Detach();

		void PointAt(Math::Vector3 target);
		void PointAt(Entity ^target);
		void PointAt(Entity ^target, Math::Vector3 offset);
		void PointAt(Ped ^target, int boneIndex);
		void PointAt(Ped ^target, int boneIndex, Math::Vector3 offset);
		void StopPointing();

		void Destroy();

	internal:
		static initonly array<System::String ^> ^sShakeNames = { "HAND_SHAKE", "SMALL_EXPLOSION_SHAKE", "MEDIUM_EXPLOSION_SHAKE", "LARGE_EXPLOSION_SHAKE", "JOLT_SHAKE", "VIBRATE_SHAKE", "ROAD_VIBRATION_SHAKE", "DRUNK_SHAKE", "SKY_DIVING_SHAKE", "FAMILY5_DRUG_TRIP_SHAKE", "DEATH_FAIL_IN_EFFECT_SHAKE" };

	private:
		int mHandle;
		CameraShake mShakeType;
		float mShakeAmplitude;
	};
}