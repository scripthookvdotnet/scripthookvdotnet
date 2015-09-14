#include "Entity.hpp"
#include "Blip.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"

namespace GTA
{
	Entity::Entity(int handle) : mHandle(handle)
	{
	}

	int Entity::Handle::get()
	{
		return this->mHandle;
	}

	int Entity::Alpha::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_ALPHA, this->Handle);
	}
	void Entity::Alpha::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_ALPHA, this->Handle, value, false);
	}
	Blip ^Entity::CurrentBlip::get()
	{
		return Native::Function::Call<Blip ^>(Native::Hash::GET_BLIP_FROM_ENTITY, this->Handle);
	}
	Math::Vector3 Entity::ForwardVector::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_FORWARD_VECTOR, this->Handle);
	}
	void Entity::FreezePosition::set(bool value)
	{
		Native::Function::Call(Native::Hash::FREEZE_ENTITY_POSITION, this->Handle, value);
	}
	float Entity::Heading::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEADING, this->Handle);
	}
	void Entity::Heading::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_ENTITY_HEADING, this->Handle, value);
	}
	int Entity::Health::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_HEALTH, this->Handle) - 100;
	}
	void Entity::Health::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_HEALTH, this->Handle, value + 100);
	}
	float Entity::HeightAboveGround::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEIGHT_ABOVE_GROUND, this->Handle);
	}
	bool Entity::IsAlive::get()
	{
		return !IsDead;
	}
	bool Entity::IsDead::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_DEAD, this->Handle);
	}
	bool Entity::IsInAir::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_AIR, this->Handle);
	}
	bool Entity::IsInWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_WATER, this->Handle);
	}
	bool Entity::IsInvincible::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address, 8);

	}
	void Entity::IsInvincible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_INVINCIBLE, this->Handle, value);
	}
	bool Entity::IsOccluded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_OCCLUDED, this->Handle);
	}
	bool Entity::IsOnFire::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_FIRE, this->Handle);
	}
	bool Entity::IsOnScreen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_SCREEN, this->Handle);
	}
	bool Entity::IsPersistent::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_MISSION_ENTITY, this->Handle);
	}
	void Entity::IsPersistent::set(bool value)
	{
		if (value)
		{
			Native::Function::Call(Native::Hash::SET_ENTITY_AS_MISSION_ENTITY, this->Handle, true, false);
		}
		else
		{
			MarkAsNoLongerNeeded();
		}
	}
	bool Entity::IsUpright::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPRIGHT, this->Handle);
	}
	bool Entity::IsUpsideDown::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPSIDEDOWN, this->Handle);
	}
	bool Entity::IsVisible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_VISIBLE, this->Handle);
	}
	void Entity::IsVisible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VISIBLE, this->Handle, value);
	}
	int Entity::MaxHealth::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MAX_HEALTH, this->Handle) - 100;
	}
	void Entity::MaxHealth::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_MAX_HEALTH, this->Handle, value + 100);
	}
	GTA::Model Entity::Model::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, this->Handle);
	}
	Math::Vector3 Entity::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, this->Handle, 0);
	}
	void Entity::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS, this->Handle, value.X, value.Y, value.Z, 0, 0, 0, 1);
	}
	void Entity::PositionNoOffset::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS_NO_OFFSET, this->Handle, value.X, value.Y, value.Z, 1, 1, 1);
	}
	Math::Quaternion Entity::Quaternion::get()
	{
		float x, y, z, w;
		Native::Function::Call(Native::Hash::GET_ENTITY_QUATERNION, this->Handle, &x, &y, &z, &w);
		return Math::Quaternion(x, y, z, w);
	}
	void Entity::Quaternion::set(Math::Quaternion value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_QUATERNION, this->Handle, value.X, value.Y, value.Z, value.W);
	}
	Math::Vector3 Entity::RightVector::get()
	{
		return Math::Vector3::Cross(ForwardVector, Math::Vector3(0, 0, 1));
	}
	Math::Vector3 Entity::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_ROTATION, this->Handle, 0);
	}
	void Entity::Rotation::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_ROTATION, this->Handle, value.X, value.Y, value.Z, 2, 1);
	}
	Math::Vector3 Entity::UpVector::get()
	{
		return Math::Vector3::Cross(RightVector, ForwardVector);
	}
	Math::Vector3 Entity::Velocity::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_VELOCITY, this->Handle);
	}
	void Entity::Velocity::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VELOCITY, this->Handle, value.X, value.Y, value.Z);
	}

	bool Entity::IsInRangeOf(Math::Vector3 position, float distance)
	{
		return ((Math::Vector3::Subtract(this->Position, position).Length()) < distance);
	}
	bool Entity::IsInArea(Math::Vector3 pos1, Math::Vector3 pos2)
	{
		return Entity::IsInArea(pos1, pos2, 0);
	}
	bool Entity::IsInArea(Math::Vector3 pos1, Math::Vector3 pos2, float angle)
	{
		return (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_ANGLED_AREA, this->Handle, pos1.X, pos1.Y, pos1.Z, pos2.X, pos2.Y, pos2.Z, angle, true, true, true));
	}
	bool Entity::IsNearEntity(Entity^ entity, Math::Vector3 distance)
	{
		return (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AT_ENTITY, this->Handle, entity->Handle, distance.X, distance.Y, distance.Z, 0, 1, 0));
	}
	bool Entity::IsTouching(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_TOUCHING_ENTITY, this->Handle, entity->Handle);
	}
	bool Entity::HasBeenDamagedBy(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::HAS_ENTITY_BEEN_DAMAGED_BY_ENTITY, this->Handle, entity->Handle, 1);
	}
	Math::Vector3 Entity::GetOffsetInWorldCoords(Math::Vector3 offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, this->Handle, offset.X, offset.Y, offset.Z);
	}
	Math::Vector3 Entity::GetOffsetFromWorldCoords(Math::Vector3 offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_OFFSET_FROM_ENTITY_GIVEN_WORLD_COORDS, this->Handle, offset.X, offset.Y, offset.Z);
	}

	bool Entity::IsAttached()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ATTACHED, this->Handle);
	}
	bool Entity::IsAttachedTo(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ATTACHED_TO_ENTITY, this->Handle, entity->Handle);
	}
	Entity ^Entity::GetEntityAttachedTo()
	{
		return Native::Function::Call<Entity^>(Native::Hash::GET_ENTITY_ATTACHED_TO, this->Handle);
	}
	void Entity::Detach()
	{
		Native::Function::Call(Native::Hash::DETACH_ENTITY, this->Handle, 1, 1);
	}
	void Entity::AttachTo(Entity^ entity, int boneIndex)
	{
		this->AttachTo(entity, boneIndex, Math::Vector3::Zero, Math::Vector3::Zero);
	}
	void Entity::AttachTo(Entity^ entity, int boneIndex, Math::Vector3 position, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::ATTACH_ENTITY_TO_ENTITY, this, entity, boneIndex, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 0, 0, 0, 0, 2, 1);
	}

	Blip ^Entity::AddBlip()
	{
		return Native::Function::Call<Blip ^>(Native::Hash::ADD_BLIP_FOR_ENTITY, this->Handle);
	}

	void Entity::ApplyForce(Math::Vector3 direction)
	{
		ApplyForce(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForce(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, this->Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction)
	{
		ApplyForceRelative(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, this->Handle, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
	}
	bool Entity::IsBulletProof::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address,4);
	}
	void Entity::IsBulletProof::set(bool value)
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		if (value)
			SetBit(Address,4);
		else
			ResetBit(Address,4);
	}
	bool Entity::IsFireProof::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address,5);
	}
	void Entity::IsFireProof::set(bool value)
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		if (value)
			SetBit(Address,5);
		else
			ResetBit(Address, 5);
	}
	bool Entity::IsExplosionProof::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address, 10);
	}
	void Entity::IsExplosionProof::set(bool value)
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		if (value)
			SetBit(Address, 10);
		else
			ResetBit(Address, 10);
	}
	bool Entity::IsCollisionProof::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address, 6);
	}
	void Entity::IsCollisionProof::set(bool value)
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		if (value)
			SetBit(Address, 6);
		else
			ResetBit(Address, 6);
	}
	bool Entity::IsMeleeProof::get()
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		return IsBitSet(Address, 7);
	}
	void Entity::IsMeleeProof::set(bool value)
	{
		unsigned long long Address = Native::MemoryAccess::GetAddressOfEntity(this->Handle);
		if (Address != 0)
			Address += 392;
		if (value)
			SetBit(Address, 7);
		else
			ResetBit(Address, 7);
	}


	void Entity::ResetAlpha()
	{
		Native::Function::Call(Native::Hash::RESET_ENTITY_ALPHA, this->Handle);
	}

	void Entity::Delete()
	{
		int handle = this->Handle;
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
		int handle = this->Handle;
		Native::Function::Call(Native::Hash::SET_ENTITY_AS_NO_LONGER_NEEDED, &handle);
	}
	bool Entity::Equals(Entity ^entity)
	{
		return !System::Object::ReferenceEquals(entity, nullptr) && this->Handle == entity->Handle;
	}

	int Entity::GetHashCode()
	{
		return this->Handle;
	}
}