#include "Entity.hpp"
#include "Blip.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"

namespace GTA
{
	Entity::Entity(int handle) : _handle(handle)
	{
	}

	int Entity::Handle::get()
	{
		return _handle;
	}
	int Entity::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_ALPHA, Handle);
	}
	void Entity::Alpha::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_ALPHA, Handle, value, false);
	}
	Blip ^Entity::CurrentBlip::get()
	{
		return Native::Function::Call<Blip ^>(Native::Hash::GET_BLIP_FROM_ENTITY, Handle);
	}
	Math::Vector3 Entity::ForwardVector::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_FORWARD_VECTOR, Handle);
	}
	void Entity::FreezePosition::set(bool value)
	{
		Native::Function::Call(Native::Hash::FREEZE_ENTITY_POSITION, Handle, value);
	}
	void Entity::HasCollision::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COLLISION, Handle, value, false);
	}
	void Entity::HasGravity::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_HAS_GRAVITY, Handle, value);
	}
	float Entity::Heading::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEADING, Handle);
	}
	void Entity::Heading::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_ENTITY_HEADING, Handle, value);
	}
	int Entity::Health::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_HEALTH, Handle) / 2;
	}
	void Entity::Health::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_HEALTH, Handle, value + 100);
	}
	float Entity::HeightAboveGround::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEIGHT_ABOVE_GROUND, Handle);
	}
	bool Entity::IsAlive::get()
	{
		return !IsDead;
	}
	bool Entity::IsBulletProof::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 4)) != 0;
	}
	void Entity::IsBulletProof::set(bool value)
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		if (address == 0)
		{
			return;
		}

		if (value)
		{
			*reinterpret_cast<int *>(address + 392) |= (1 << 4);
		}
		else
		{
			*reinterpret_cast<int *>(address + 392) &= ~(1 << 4);
		}
	}
	bool Entity::IsCollisionProof::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 6)) != 0;
	}
	void Entity::IsCollisionProof::set(bool value)
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		if (address == 0)
		{
			return;
		}

		if (value)
		{
			*reinterpret_cast<int *>(address + 392) |= (1 << 6);
		}
		else
		{
			*reinterpret_cast<int *>(address + 392) &= ~(1 << 6);
		}
	}
	bool Entity::IsDead::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_DEAD, Handle);
	}
	bool Entity::IsExplosionProof::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 10)) != 0;
	}
	void Entity::IsExplosionProof::set(bool value)
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		if (address == 0)
		{
			return;
		}

		if (value)
		{
			*reinterpret_cast<int *>(address + 392) |= (1 << 10);
		}
		else
		{
			*reinterpret_cast<int *>(address + 392) &= ~(1 << 10);
		}
	}
	bool Entity::IsFireProof::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 5)) != 0;
	}
	void Entity::IsFireProof::set(bool value)
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		if (address == 0)
		{
			return;
		}

		if (value)
		{
			*reinterpret_cast<int *>(address + 392) |= (1 << 5);
		}
		else
		{
			*reinterpret_cast<int *>(address + 392) &= ~(1 << 5);
		}
	}
	bool Entity::IsInAir::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_AIR, Handle);
	}
	bool Entity::IsInWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_WATER, Handle);
	}
	bool Entity::IsInvincible::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 8)) != 0;
	}
	void Entity::IsInvincible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_INVINCIBLE, Handle, value);
	}
	bool Entity::IsMeleeProof::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 7)) != 0;
	}
	void Entity::IsMeleeProof::set(bool value)
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		if (address == 0)
		{
			return;
		}

		if (value)
		{
			*reinterpret_cast<int *>(address + 392) |= (1 << 7);
		}
		else
		{
			*reinterpret_cast<int *>(address + 392) &= ~(1 << 7);
		}
	}
	bool Entity::IsOccluded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_OCCLUDED, Handle);
	}
	bool Entity::IsOnFire::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_FIRE, Handle);
	}
	bool Entity::IsOnlyDamagedByPlayer::get()
	{
		System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? false : (*reinterpret_cast<int *>(address + 392) & (1 << 9)) != 0;
	}
	void Entity::IsOnlyDamagedByPlayer::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_ONLY_DAMAGED_BY_PLAYER, Handle, value);
	}
	bool Entity::IsOnScreen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_SCREEN, Handle);
	}
	bool Entity::IsPersistent::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_MISSION_ENTITY, Handle);
	}
	void Entity::IsPersistent::set(bool value)
	{
		if (value)
		{
			Native::Function::Call(Native::Hash::SET_ENTITY_AS_MISSION_ENTITY, Handle, true, false);
		}
		else
		{
			MarkAsNoLongerNeeded();
		}
	}
	bool Entity::IsUpright::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPRIGHT, Handle);
	}
	bool Entity::IsUpsideDown::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPSIDEDOWN, Handle);
	}
	bool Entity::IsVisible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_VISIBLE, Handle);
	}
	void Entity::IsVisible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VISIBLE, Handle, value);
	}
	int Entity::LodDistance::get()
	{
		return Native::Function::Call<int>(Native::Hash::_GET_ENTITY_LOD_DIST, Handle);
	}
	void Entity::LodDistance::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_LOD_DIST, Handle, value);
	}
	int Entity::MaxHealth::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MAX_HEALTH, Handle) - 100;
	}
	void Entity::MaxHealth::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_MAX_HEALTH, Handle, value + 100);
	}
	void Entity::MaxSpeed::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_MAX_SPEED, Handle, value);
	}
	GTA::Model Entity::Model::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, Handle);
	}
	Math::Vector3 Entity::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, Handle, 0);
	}
	void Entity::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS, Handle, value.X, value.Y, value.Z, 0, 0, 0, 1);
	}
	void Entity::PositionNoOffset::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS_NO_OFFSET, Handle, value.X, value.Y, value.Z, 1, 1, 1);
	}
	Math::Quaternion Entity::Quaternion::get()
	{
		float x, y, z, w;
		Native::Function::Call(Native::Hash::GET_ENTITY_QUATERNION, Handle, &x, &y, &z, &w);
		return Math::Quaternion(x, y, z, w);
	}
	void Entity::Quaternion::set(Math::Quaternion value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_QUATERNION, Handle, value.X, value.Y, value.Z, value.W);
	}
	Math::Vector3 Entity::RightVector::get()
	{
		return Math::Vector3::Cross(ForwardVector, Math::Vector3(0, 0, 1));
	}
	Math::Vector3 Entity::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_ROTATION, Handle, 0);
	}
	void Entity::Rotation::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_ROTATION, Handle, value.X, value.Y, value.Z, 2, 1);
	}
	Math::Vector3 Entity::UpVector::get()
	{
		return Math::Vector3::Cross(RightVector, ForwardVector);
	}
	Math::Vector3 Entity::Velocity::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_VELOCITY, Handle);
	}
	void Entity::Velocity::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VELOCITY, Handle, value.X, value.Y, value.Z);
	}

	bool Entity::IsInRangeOf(Math::Vector3 position, float distance)
	{
		return Math::Vector3::Subtract(Position, position).Length() < distance;
	}
	bool Entity::IsInArea(Math::Vector3 pos1, Math::Vector3 pos2)
	{
		return Entity::IsInArea(pos1, pos2, 0);
	}
	bool Entity::IsInArea(Math::Vector3 pos1, Math::Vector3 pos2, float angle)
	{
		return (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_ANGLED_AREA, Handle, pos1.X, pos1.Y, pos1.Z, pos2.X, pos2.Y, pos2.Z, angle, true, true, true));
	}
	bool Entity::IsNearEntity(Entity^ entity, Math::Vector3 distance)
	{
		return (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AT_ENTITY, Handle, entity->Handle, distance.X, distance.Y, distance.Z, 0, 1, 0));
	}
	bool Entity::IsTouching(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_TOUCHING_ENTITY, Handle, entity->Handle);
	}
	bool Entity::HasBeenDamagedBy(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, Handle, entity->Handle, 1);
	}
	Math::Vector3 Entity::GetOffsetInWorldCoords(Math::Vector3 offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, Handle, offset.X, offset.Y, offset.Z);
	}
	Math::Vector3 Entity::GetOffsetFromWorldCoords(Math::Vector3 offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, Handle, offset.X, offset.Y, offset.Z);
	}

	bool Entity::IsAttached()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ATTACHED, Handle);
	}
	bool Entity::IsAttachedTo(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ATTACHED_TO_ENTITY, Handle, entity->Handle);
	}
	Entity ^Entity::GetEntityAttachedTo()
	{
		return Native::Function::Call<Entity^>(Native::Hash::GET_ENTITY_ATTACHED_TO, Handle);
	}
	void Entity::Detach()
	{
		Native::Function::Call(Native::Hash::DETACH_ENTITY, Handle, 1, 1);
	}
	void Entity::AttachTo(Entity ^entity, int boneIndex)
	{
		AttachTo(entity, boneIndex, Math::Vector3::Zero, Math::Vector3::Zero);
	}
	void Entity::AttachTo(Entity ^entity, int boneIndex, Math::Vector3 position, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::ATTACH_ENTITY_TO_ENTITY, Handle, entity->Handle, boneIndex, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
	}

	Blip ^Entity::AddBlip()
	{
		return Native::Function::Call<Blip ^>(Native::Hash::ADD_BLIP_FOR_ENTITY, Handle);
	}

	void Entity::ApplyForce(Math::Vector3 direction)
	{
		ApplyForce(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForce(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction)
	{
		ApplyForceRelative(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
	}

	void Entity::ResetAlpha()
	{
		Native::Function::Call(Native::Hash::RESET_ENTITY_ALPHA, Handle);
	}

	void Entity::Delete()
	{
		int handle = Handle;
		Native::Function::Call(Native::Hash::SET_ENTITY_AS_MISSION_ENTITY, handle, true, false);
		Native::Function::Call(Native::Hash::DELETE_ENTITY, &handle);
	}
	bool Entity::Exists()
	{
		return Exists(this);
	}
	bool Entity::Exists(Entity ^entity)
	{
		return !Object::ReferenceEquals(entity, nullptr) && Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entity->Handle);
	}
	void Entity::MarkAsNoLongerNeeded()
	{
		int handle = Handle;
		Native::Function::Call(Native::Hash::SET_ENTITY_AS_NO_LONGER_NEEDED, &handle);
	}
	bool Entity::Equals(Entity ^entity)
	{
		return !System::Object::ReferenceEquals(entity, nullptr) && Handle == entity->Handle;
	}
}
