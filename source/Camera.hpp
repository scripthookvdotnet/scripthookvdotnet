#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	ref class Entity;
	#pragma endregion

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
		property Math::Vector3 Direction
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
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
			void set(float amplitude);
		}

		void Shake(CameraShake shakeType, float amplitude);
		void StopShaking();

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
		static bool Exists(Camera ^camera);
		void Destroy();
		virtual bool Equals(Camera ^camera);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}
		static inline bool operator==(Camera ^left, Camera ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Camera ^left, Camera ^right)
		{
			return !operator==(left, right);
		}

	internal:
		static initonly array<System::String ^> ^_shakeNames = { "HAND_SHAKE", "SMALL_EXPLOSION_SHAKE", "MEDIUM_EXPLOSION_SHAKE", "LARGE_EXPLOSION_SHAKE", "JOLT_SHAKE", "VIBRATE_SHAKE", "ROAD_VIBRATION_SHAKE", "DRUNK_SHAKE", "SKY_DIVING_SHAKE", "FAMILY5_DRUG_TRIP_SHAKE", "DEATH_FAIL_IN_EFFECT_SHAKE" };

	private:
		int _handle;
	};
	public ref class GameplayCamera sealed abstract
	{
	public:
		static property Math::Vector3 Direction
		{
			Math::Vector3 get();
		}
		static property float FieldOfView
		{
			float get();
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