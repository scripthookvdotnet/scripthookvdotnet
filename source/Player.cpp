#include "Game.hpp"
#include "Player.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"

namespace GTA
{
	Player::Player(int handle) : _handle(handle)
	{
	}

	int Player::Handle::get()
	{
		return _handle;
	}
	bool Player::CanControlCharacter::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_CONTROL_ON, Handle);
	}
	void Player::CanControlCharacter::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_CONTROL, Handle, value, 0);
	}
	void Player::CanControlRagdoll::set(bool value)
	{
		Native::Function::Call(Native::Hash::GIVE_PLAYER_RAGDOLL_CONTROL, Handle, value);
	}
	bool Player::CanStartMission::get()
	{
		return Native::Function::Call<bool>(Native::Hash::CAN_PLAYER_START_MISSION, Handle);
	}
	void Player::CanUseCover::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_CAN_USE_COVER, Handle, value);
	}
	Ped ^Player::Character::get()
	{
		int pedHandle = Native::Function::Call<int>(Native::Hash::GET_PLAYER_PED, Handle);

		if (ReferenceEquals(_ped, nullptr) || pedHandle != _ped->Handle)
		{
			_ped = gcnew Ped(pedHandle);
		}

		return _ped;
	}
	System::Drawing::Color Player::Color::get()
	{
		int r = 0, g = 0, b = 0;
		Native::Function::Call(Native::Hash::GET_PLAYER_RGB_COLOUR, Handle, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Player::IgnoredByEveryone::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_EVERYONE_IGNORE_PLAYER, Handle, value);
	}
	void Player::IgnoredByPolice::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_POLICE_IGNORE_PLAYER, Handle, value);
	}
	bool Player::IsAiming::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_FREE_AIMING, Handle);
	}
	bool Player::IsAlive::get()
	{
		return !IsDead;
	}
	bool Player::IsClimbing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_CLIMBING, Handle);
	}
	bool Player::IsDead::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_DEAD, Handle);
	}
	bool Player::IsInvincible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_PLAYER_INVINCIBLE, Handle);
	}
	void Player::IsInvincible::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_INVINCIBLE, Handle, value);
	}
	bool Player::IsPlaying::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_PLAYING, Handle);
	}
	bool Player::IsPressingHorn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_PRESSING_HORN, Handle);
	}
	bool Player::IsRidingTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_RIDING_TRAIN, Handle);
	}
	bool Player::IsTargettingAnything::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_TARGETTING_ANYTHING, Handle);
	}
	Vehicle ^Player::LastVehicle::get()
	{
		return Native::Function::Call<Vehicle ^>(Native::Hash::GET_PLAYERS_LAST_VEHICLE);
	}
	int Player::Money::get()
	{
		int hash;

		switch (static_cast<Native::PedHash>(Character->Model.Hash))
		{
			case Native::PedHash::Michael:
				hash = Game::GenerateHash("SP0_TOTAL_CASH");
				break;
			case Native::PedHash::Franklin:
				hash = Game::GenerateHash("SP1_TOTAL_CASH");
				break;
			case Native::PedHash::Trevor:
				hash = Game::GenerateHash("SP2_TOTAL_CASH");
				break;
			default:
				return 0;
		}

		int value = 0;
		Native::Function::Call(Native::Hash::STAT_GET_INT, hash, &value, -1);

		return value;
	}
	void Player::Money::set(int value)
	{
		int hash;

		switch (static_cast<Native::PedHash>(Character->Model.Hash))
		{
			case Native::PedHash::Michael:
				hash = Game::GenerateHash("SP0_TOTAL_CASH");
				break;
			case Native::PedHash::Franklin:
				hash = Game::GenerateHash("SP1_TOTAL_CASH");
				break;
			case Native::PedHash::Trevor:
				hash = Game::GenerateHash("SP2_TOTAL_CASH");
				break;
			default:
				return;
		}

		Native::Function::Call(Native::Hash::STAT_SET_INT, hash, value, 1);
	}
	System::String ^Player::Name::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_PLAYER_NAME, Handle);
	}
	int Player::RemainingSprintTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_SPRINT_TIME_REMAINING, Handle);
	}
	int Player::RemainingUnderwaterTime::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_UNDERWATER_TIME_REMAINING, Handle);
	}
	int Player::WantedLevel::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_WANTED_LEVEL, Handle);
	}
	void Player::WantedLevel::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_WANTED_LEVEL, Handle, value, false);
		Native::Function::Call(Native::Hash::SET_PLAYER_WANTED_LEVEL_NOW, Handle, false);
	}

	bool Player::IsTargetting(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity->Handle);
	}
	Entity ^Player::GetTargetedEntity()
	{
		int entity = 0;

		if (Native::Function::Call<bool>(Native::Hash::_GET_AIMED_ENTITY, Handle, &entity))
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
			else if (Native::Function::Call<bool>(Native::Hash::IS_ENTITY_AN_OBJECT, entity))
			{
				return gcnew Prop(entity);
			}
		}

		return nullptr;
	}

	bool Player::Equals(Player ^player)
	{
		return !ReferenceEquals(player, nullptr) && Handle == player->Handle;
	}
}