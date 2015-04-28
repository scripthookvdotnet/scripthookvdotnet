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
	Vector3 Entity::Position::get()
	{
		return Native::Function::Call<Vector3>(Native::Hash::GET_ENTITY_COORDS, this->ID, 0);
	}
	void Entity::Position::set(Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_COORDS, this->ID, value.X, value.Y, value.Z, 0, 0, 0, 1);
	}
	float Entity::Heading::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_HEADING, this->ID);
	}
	void Entity::Heading::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_ENTITY_HEADING, this->ID, value);
	}
	Vector3 Entity::Rotation::get()
	{
		return Native::Function::Call<Vector3>(Native::Hash::GET_ENTITY_ROTATION, this->ID, 0);
	}
	Vector3 Entity::Velocity::get()
	{
		return Native::Function::Call<Vector3>(Native::Hash::GET_ENTITY_VELOCITY, this->ID);
	}
	void Entity::Velocity::set(Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_VELOCITY, this->ID, value.X, value.Y, value.Z);
	}
	int Entity::Health::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_ENTITY_HEALTH, this->ID);
	}
	void Entity::Health::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_HEALTH, this->ID, value);
	}
	GTA::Model Entity::Model::get()
	{
		return GTA::Model(Native::Function::Call<int>(Native::Hash::GET_ENTITY_MODEL, this->ID));
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
	void Entity::IsOnFire::set(bool value)
	{
		if (value)
		{
			Native::Function::Call(Native::Hash::START_ENTITY_FIRE, this->ID);
		}
		else
		{
			Native::Function::Call(Native::Hash::STOP_ENTITY_FIRE, this->ID);
		}
	}
	bool Entity::IsRequiredForMission::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_MISSION_ENTITY, this->ID);
	}
	void Entity::IsRequiredForMission::set(bool value)
	{
		if (value)
		{
			Native::Function::Call(Native::Hash::SET_ENTITY_AS_MISSION_ENTITY, this->ID, true, false);
		}
		else
		{
			int handle = this->ID;
			Native::Function::Call(Native::Hash::SET_ENTITY_AS_NO_LONGER_NEEDED, &handle);
		}
	}

	bool Entity::Exists()
	{
		return Exists(this);
	}
	bool Entity::Exists(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entity->ID);
	}
}