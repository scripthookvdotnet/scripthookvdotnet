#pragma once

#include "Model.hpp"
#include "Vector3.hpp"
#include "Blip.hpp"

namespace GTA
{
	public ref class Entity abstract
	{
	public:
		Entity(int handle);

		[System::ObsoleteAttribute("Entity.ID is obsolete, please use Entity.Handle instead.")]
		property int ID
		{
			int get()
			{
				return Handle;
			}
		}
		property int Handle
		{
			int get();
		}

		property Math::Vector3 Position
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property float HeightAboveGround
		{
			float get();
		}
		property float Heading
		{
			float get();
			void set(float value);
		}
		property Math::Vector3 Rotation
		{
			Math::Vector3 get();
			void set(Math::Vector3 value);
		}
		property Math::Vector3 ForwardVector
		{
			Math::Vector3 get();
		}
		property Math::Vector3 RightVector
		{
			Math::Vector3 get();
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
		property bool FreezePosition
		{
			void set(bool value);
		}
		property int Health
		{
			int get();
			void set(int value);
		}
		property int MaxHealth
		{
			int get();
			void set(int value);
		}
		property GTA::Model Model
		{
			GTA::Model get();
		}
		property bool IsDead
		{
			bool get();
		}
		property bool IsAlive
		{
			bool get();
		}
		property bool IsInvincible
		{
			void set(bool value);
		}
		property bool IsVisible
		{
			bool get();
			void set(bool value);
		}
		property bool IsOccluded
		{
			bool get();
		}
		property bool IsOnScreen
		{
			bool get();
		}
		property bool IsUpright
		{
			bool get();
		}
		property bool IsUpsideDown
		{
			bool get();
		}
		property bool IsInAir
		{
			bool get();
		}
		property bool IsInWater
		{
			bool get();
		}
		property bool IsOnFire
		{
			bool get();
		}
		property bool IsPersistent
		{
			bool get();
			void set(bool value);
		}
		property Blip ^CurrentBlip
		{
			Blip ^get();
		}

		void ApplyForce(Math::Vector3 direction);
		void ApplyForce(Math::Vector3 direction, Math::Vector3 rotation);
		void ApplyForceRelative(Math::Vector3 direction);
		void ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation);
		Blip ^AddBlip();
		
		bool IsNearEntity(Entity^ entity, Math::Vector3 distance);
		bool IsInRangeOf(Math::Vector3 position, float range);
		bool IsInArea(Math::Vector3 pos1, Math::Vector3 pos2);
		bool IsInArea(Math::Vector3 pos1, Math::Vector3 pos2, float angle);
		bool IsAttached();
		void AttachTo(Entity^ entity, int boneIndex);
		void AttachTo(Entity^ entity, int boneIndex, Math::Vector3 position, Math::Vector3 rotation);
		Math::Vector3 GetOffsetInWorldCoords(Math::Vector3 offset);
		void Detach();

		void Delete();
		bool Exists();
		static bool Exists(Entity ^entity);
		void MarkAsNoLongerNeeded();
		virtual bool Equals(Entity ^entity);

		bool IsTouching(Entity ^entity);
		bool HasBeenDamagedBy(Entity ^entity);

		virtual int GetHashCode() override;
		static inline bool operator ==(Entity ^left, Entity ^right)
		{
			if (Object::ReferenceEquals(left, nullptr))
			{
				return Object::ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator !=(Entity ^left, Entity ^right)
		{
			return !operator ==(left, right);
		}

	private:
		int mHandle;
	};
}