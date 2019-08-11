#pragma once

#include "Model.hpp"
#include "Vector3.hpp"
#include "Quaternion.hpp"
#include "Interface.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Blip;
	#pragma endregion
	public enum class ForceType{

		MinForce = 0,
		MaxForceRot = 1,
		MinForce2 = 2,
		MaxForceRot2 = 3, //stable, good for custom handling
		ForceNoRot = 4,
		ForceRotPlusForce = 5
	};

	public ref class Entity abstract : System::IEquatable<Entity ^>, IHandleable, ISpatial
	{
	public:
		Entity(int handle);

		virtual property int Handle
		{
			int get();
		}
		property int Alpha
		{
			int get();
			void set(int value);
		}
		property Blip ^CurrentBlip
		{
			Blip ^get();
		}
		property Math::Vector3 ForwardVector
		{
			Math::Vector3 get();
		}
		property bool FreezePosition
		{
			bool get();
			void set(bool value);
		}
		property bool HasCollision
		{
			bool get();
			void set(bool value);
		}
		property bool HasGravity
		{
			void set(bool value);
		}
		property bool HasCollidedWithAnything
		{
			bool get();
		}
		property float Heading
		{
			float get();
			void set(float value);
		}
		property int Health
		{
			int get();
			void set(int value);
		}
		property float HeightAboveGround
		{
			float get();
		}
		property bool IsAlive
		{
			bool get();
		}
		property bool IsBulletProof
		{
			bool get();
			void set(bool value);
		}
		property bool IsCollisionProof
		{
			bool get();
			void set(bool value);
		}
		property bool IsDead
		{
			bool get();
		}
		property bool IsExplosionProof
		{
			bool get();
			void set(bool value);
		}
		property bool IsFireProof
		{
			bool get();
			void set(bool value);
		}
		property bool IsInAir
		{
			bool get();
		}
		property bool IsInWater
		{
			bool get();
		}
		property bool IsInvincible
		{
			bool get();
			void set(bool value);
		}
		property bool IsMeleeProof
		{
			bool get();
			void set(bool value);
		}
		property bool IsOccluded
		{
			bool get();
		}
		property bool IsOnFire
		{
			bool get();
		}
		property bool IsOnlyDamagedByPlayer
		{
			bool get();
			void set(bool value);
		}
		property bool IsOnScreen
		{
			bool get();
		}
		property bool IsPersistent
		{
			bool get();
			void set(bool value);
		}
		property bool IsUpright
		{
			bool get();
		}
		property bool IsUpsideDown
		{
			bool get();
		}
		property bool IsVisible
		{
			bool get();
			void set(bool value);
		}
		property int LodDistance
		{
			int get();
			void set(int value);
		}
		virtual property int MaxHealth
		{
			int get();
			void set(int value);
		}
		property float MaxSpeed
		{
			void set(float value);
		}
		property int *MemoryAddress
		{
			int *get();
		}
		property GTA::Model Model
		{
			GTA::Model get();
		}
		virtual property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property Math::Vector3 PositionNoOffset
		{
			void set(Math::Vector3 value);
		}
		property Math::Quaternion Quaternion
		{
			Math::Quaternion get();
			void set(Math::Quaternion value);
		}
		property Math::Vector3 RightVector
		{
			Math::Vector3 get();
		}
		virtual property Math::Vector3 Rotation
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property Math::Vector3 UpVector
		{
			Math::Vector3 get();
		}
		property Math::Vector3 Velocity
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}

		bool IsInRangeOf(Math::Vector3 position, float range);
		bool IsInArea(Math::Vector3 minBounds, Math::Vector3 maxBounds);
		[System::ObsoleteAttribute("The Entity.IsInArea(Vector3, Vector3, float) is obsolete, use Entity.IsInAngledArea(Vector3, Vector3, float) instead.")]
		bool IsInArea(Math::Vector3 pos1, Math::Vector3 pos2, float angle);
		bool IsInAngledArea(Math::Vector3 Origin, Math::Vector3 Edge, float angle);
		bool IsNearEntity(Entity^ entity, Math::Vector3 distance);
		bool IsTouching(Entity ^entity);
		bool IsTouching(GTA::Model model);
		bool HasBeenDamagedBy(Entity ^entity);
		Math::Vector3 GetOffsetInWorldCoords(Math::Vector3 offset);
		Math::Vector3 GetOffsetFromWorldCoords(Math::Vector3 worldCoords);
		bool IsAttached();
		bool IsAttachedTo(Entity ^entity);
		Entity ^GetEntityAttachedTo();
		void SetNoCollision(Entity^ entity, bool toggle);
		void AttachTo(Entity^ entity, int boneIndex);
		void AttachTo(Entity^ entity, int boneIndex, Math::Vector3 position, Math::Vector3 rotation);
		void Detach();

		Blip ^AddBlip();

		void ApplyForce(Math::Vector3 direction);
		void ApplyForce(Math::Vector3 direction, Math::Vector3 rotation);
		void ApplyForce(Math::Vector3 direction, Math::Vector3 rotation, ForceType forceType);
		void ApplyForceRelative(Math::Vector3 direction);
		void ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation);
		void ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation, ForceType forceType);

		void ResetAlpha();

		Math::Vector3 GetBoneCoord(int boneIndex);
		Math::Vector3 GetBoneCoord(System::String ^boneName);
		int GetBoneIndex(System::String ^boneName);
		bool HasBone(System::String ^boneName);

		void Delete();
		void MarkAsNoLongerNeeded();

		virtual bool Exists();
		static bool Exists(Entity ^entity);

		virtual bool Equals(System::Object ^obj) override;
		virtual bool Equals(Entity ^entity);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}

		static inline bool operator==(Entity ^left, Entity ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(Entity ^left, Entity ^right)
		{
			return !operator==(left, right);
		}
		
	private:
		int _handle;
	};
}