#include "Player.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Native.hpp"

namespace GTA
{
	Player::Player(int id) : mID(id), mPed(gcnew Ped(Native::Function::Call<int>(Native::Hash::GET_PLAYER_PED, id)))
	{
	}

	int Player::ID::get()
	{
		return this->mID;
	}
	System::String ^Player::Name::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_PLAYER_NAME, this->ID);
	}
	System::Drawing::Color Player::Color::get()
	{
		int r = 0, g = 0, b = 0;
		Native::Function::Call(Native::Hash::GET_PLAYER_RGB_COLOUR, this->ID, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	Ped ^Player::Character::get()
	{
		return this->mPed;
	}
	int Player::WantedLevel::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_WANTED_LEVEL, this->ID);
	}
	void Player::WantedLevel::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_WANTED_LEVEL, this->ID, value, false);
		Native::Function::Call(Native::Hash::SET_PLAYER_WANTED_LEVEL_NOW, this->ID, false);
	}
	int Player::RemainingSprintTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_SPRINT_TIME_REMAINING, this->ID);
	}
	int Player::RemainingUnderwaterTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_UNDERWATER_TIME_REMAINING, this->ID);
	}
	bool Player::IsDead::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_DEAD, this->ID);
	}
	bool Player::IsAlive::get()
	{
		return !IsDead;
	}
	bool Player::IsOnMission::get()
	{
		return !Native::Function::Call<bool>(Native::Hash::CAN_PLAYER_START_MISSION, this->ID);
	}
	bool Player::IsPlaying::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_PLAYING, this->ID);
	}
	bool Player::IsPressingHorn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_PRESSING_HORN, this->ID);
	}
	bool Player::IsRidingTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_RIDING_TRAIN, this->ID);
	}
	bool Player::IsClimbing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_CLIMBING, this->ID);
	}
	Vehicle ^Player::LastVehicle::get()
	{
		return gcnew Vehicle(Native::Function::Call<int>(Native::Hash::GET_PLAYERS_LAST_VEHICLE));
	}

	void Player::IgnoredByEveryone::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_EVERYONE_IGNORE_PLAYER, this->ID, value);
	}
	void Player::CanUseCover::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_CAN_USE_COVER, this->ID, value);
	}
	void Player::CanControlRagdoll::set(bool value)
	{
		Native::Function::Call(Native::Hash::GIVE_PLAYER_RAGDOLL_CONTROL, this->ID, value);
	}
	bool Player::CanControlCharacter::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_CONTROL_ON, this->ID);
	}
	void Player::CanControlCharacter::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_CONTROL, this->ID, value, 0);
	}

	bool Player::IsTargettingAnything::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_TARGETTING_ANYTHING, this->ID);
	}
	bool Player::IsTargetting(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_FREE_AIMING_AT_ENTITY, this->ID, entity->ID);
	}
	Entity ^Player::GetTargetedEntity()
	{
		int entity = 0;

		if (Native::Function::Call<bool>(Native::Hash::_GET_AIMED_ENTITY, this->ID, &entity))
		{
			if (!Native::Function::Call<bool>(Native::Hash::DOES_ENTITY_EXIST, entity))
			{
				return nullptr;
			}
			else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_PED, entity))
			{
				return gcnew Ped(entity);
			}
			else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_A_VEHICLE, entity))
			{
				return gcnew Vehicle(entity);
			}
		}

		return nullptr;
	}

	bool Player::Equals(Player ^player)
	{
		return !System::Object::ReferenceEquals(player, nullptr) && this->ID == player->ID;
	}

	int Player::GetHashCode()
	{
		return this->ID;
	}
}