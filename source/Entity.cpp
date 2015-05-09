#include "Entity.hpp"
#include "Native.hpp"

namespace GTA
{
	Entity::Entity(int id) : mID(id)
	{
	}

	int Entity::ID::get()
	{
		return this->mID;
	}
	Math::Vector3 Entity::Position::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_COORDS, this->ID, 0);
	}
	void Entity::Position::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS, this->ID, value.X, value.Y, value.Z, 0, 0, 0, 1);
	}
	float Entity::HeightAboveGround::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEIGHT_ABOVE_GROUND, this->ID);
	}
	float Entity::Heading::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEADING, this->ID);
	}
	void Entity::Heading::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_ENTITY_HEADING, this->ID, value);
	}
	Math::Vector3 Entity::Rotation::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_ROTATION, this->ID, 0);
	}
	Math::Vector3 Entity::ForwardVector::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_FORWARD_VECTOR, this->ID);
	}
	Math::Vector3 Entity::Velocity::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_ENTITY_VELOCITY, this->ID);
	}
	void Entity::Velocity::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VELOCITY, this->ID, value.X, value.Y, value.Z);
	}
	void Entity::FreezePosition::set(bool value)
	{
		Native::Function::Call(Native::Hash::FREEZE_ENTITY_POSITION, this->ID, value);
	}
	int Entity::Health::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_HEALTH, this->ID) - 100;
	}
	void Entity::Health::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_HEALTH, this->ID, value + 100);
	}
	int Entity::MaxHealth::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MAX_HEALTH, this->ID) - 100;
	}
	void Entity::MaxHealth::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_MAX_HEALTH, this->ID, value + 100);
	}
	GTA::Model Entity::Model::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, this->ID);
	}
	bool Entity::IsDead::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_DEAD, this->ID);
	}
	bool Entity::IsAlive::get()
	{
		return !IsDead;
	}
	void Entity::IsInvincible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_INVINCIBLE, this->ID, value);
	}
	bool Entity::IsVisible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_VISIBLE, this->ID);
	}
	void Entity::IsVisible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VISIBLE, this->ID, value);
	}
	bool Entity::IsOccluded::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_OCCLUDED, this->ID);
	}
	bool Entity::IsOnScreen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_SCREEN, this->ID);
	}
	bool Entity::IsUpright::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPRIGHT, this->ID);
	}
	bool Entity::IsUpsideDown::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_UPSIDEDOWN, this->ID);
	}
	bool Entity::IsInAir::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GETTING_INTO_A_VEHICLE, this->ID);
	}
	bool Entity::IsInWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_IN_WATER, this->ID);
	}
	bool Entity::IsOnFire::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_ON_FIRE, this->ID);
	}
	bool Entity::IsPersistent::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_MISSION_ENTITY, this->ID);
	}
	void Entity::IsPersistent::set(bool value)
	{
		if (value)
		{
			Native::Function::Call(Native::Hash::SET_ENTITY_AS_MISSION_ENTITY, this->ID, true, false);
		}
		else
		{
			MarkAsNoLongerNeeded();
		}
	}

	void Entity::ApplyForce(Math::Vector3 direction)
	{
		ApplyForce(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForce(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, this->ID, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, false, true, true, false, true);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction)
	{
		ApplyForceRelative(direction, Math::Vector3::Zero);
	}
	void Entity::ApplyForceRelative(Math::Vector3 direction, Math::Vector3 rotation)
	{
		Native::Function::Call(Native::Hash::APPLY_FORCE_TO_ENTITY, this->ID, 3, direction.X, direction.Y, direction.Z, rotation.X, rotation.Y, rotation.Z, false, true, true, true, false, true);
	}

	bool Entity::Exists()
	{
		return Exists(this);
	}
	bool Entity::Exists(Entity ^entity)
	{
		return !Object::ReferenceEquals(entity, nullptr) && Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entity->ID);
	}
	void Entity::MarkAsNoLongerNeeded()
	{
		int id = this->ID;
		Native::Function::Call(Native::Hash::SET_ENTITY_AS_NO_LONGER_NEEDED, &id);
	}
	bool Entity::Equals(Entity ^entity)
	{
		return !System::Object::ReferenceEquals(entity, nullptr) && this->ID == entity->ID;
	}
	float Entity::GetDistanceTo(Math::Vector3 coordinate)
	{
		return Native::Function::Call<float>(Native::Hash::GET_DISTANCE_BETWEEN_COORDS, this->Position.X, this->Position.Y, this->Position.Z, coordinate.X, coordinate.Y, coordinate.Z, 1);
	}
	int Entity::GetHashCode()
	{
		return this->ID;
	}
}