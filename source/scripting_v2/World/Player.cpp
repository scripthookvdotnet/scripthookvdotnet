#include "Game.hpp"
#include "Player.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Native.hpp"
#include "Model.hpp"

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
	int Player::MaxArmor::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PLAYER_MAX_ARMOUR, Handle);
	}
	void Player::MaxArmor::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_MAX_ARMOUR, Handle, value);
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
	Math::Vector3 Player::WantedCenterPosition::get()
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_PLAYER_WANTED_CENTRE_POSITION, Handle);
	}
	void Player::WantedCenterPosition::set(Math::Vector3 value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_WANTED_CENTRE_POSITION, Handle, value.X, value.Y, value.Z);
	}
	ParachuteTint Player::PrimaryParachuteTint::get()
	{
		int tint = 0;
		Native::Function::Call(Native::Hash::GET_PLAYER_PARACHUTE_TINT_INDEX, Handle, &tint);
		return static_cast<ParachuteTint>(tint);
	}
	void Player::PrimaryParachuteTint::set(ParachuteTint value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_PARACHUTE_TINT_INDEX, Handle, static_cast<int>(value));
	}
	ParachuteTint Player::ReserveParachuteTint::get()
	{
		int tint = 0;
		Native::Function::Call(Native::Hash::GET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, &tint);
		return static_cast<ParachuteTint>(tint);
	}
	void Player::ReserveParachuteTint::set(ParachuteTint value)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_RESERVE_PARACHUTE_TINT_INDEX, Handle, static_cast<int>(value));
	}
	bool Player::ChangeModel(Model model)
	{
		if (!model.IsInCdImage && !model.IsPed)
		{
			return false;
		}
		if (model.Request(1000))
		{
			Native::Function::Call(Native::Hash::SET_PLAYER_MODEL, Handle, model.Hash);
			model.MarkAsNoLongerNeeded();
			return true;
		}
		return false;
		
	}
	void Player::RefillSpecialAbility()
	{
		Native::Function::Call(Native::Hash::SPECIAL_ABILITY_FILL_METER, Handle, 1);
	}
		
	void Player::DisableFiringThisFrame()
	{
		Native::Function::Call(Native::Hash::DISABLE_PLAYER_FIRING, Handle, 0);
	}
	bool Player::IsTargetting(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PLAYER_FREE_AIMING_AT_ENTITY, Handle, entity->Handle);
	}
	void Player::SetRunSpeedMultThisFrame(float value)
	{
		Native::Function::Call(Native::Hash::SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, Handle, value > 1.499f ? 1.499f : value);
	}
	void Player::SetSwimSpeedMultThisFrame(float value)
	{
		Native::Function::Call(Native::Hash::SET_SWIM_MULTIPLIER_FOR_PLAYER, Handle, value > 1.499f ? 1.499f : value);
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
	void Player::SetMayOnlyEnterThisVehicleThisFrame(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_MAY_ONLY_ENTER_THIS_VEHICLE, Handle, vehicle);
	}
	void Player::SetMayNotEnterAnyVehicleThisFrame()
	{
		Native::Function::Call(Native::Hash::SET_PLAYER_MAY_NOT_ENTER_ANY_VEHICLE, Handle);
	}
	void Player::SetExplosiveAmmoThisFrame()
	{
		Native::Function::Call(Native::Hash::SET_EXPLOSIVE_AMMO_THIS_FRAME, Handle);
	}
	void Player::SetExplosiveMeleeThisFrame()
	{
		Native::Function::Call(Native::Hash::SET_EXPLOSIVE_MELEE_THIS_FRAME, Handle);
	}
	void Player::SetSuperJumpThisFrame()
	{
		Native::Function::Call(Native::Hash::SET_SUPER_JUMP_THIS_FRAME, Handle);
	}
	void Player::SetFireAmmoThisFrame()
	{
		Native::Function::Call(Native::Hash::SET_FIRE_AMMO_THIS_FRAME, Handle);
	}

	bool Player::Equals(Object ^value)
	{
		if (value == nullptr || value->GetType() != GetType())
			return false;

		return Equals(safe_cast<Entity ^>(value));
	}
	bool Player::Equals(Player ^player)
	{
		return !Object::ReferenceEquals(player, nullptr) && Handle == player->Handle;
	}
}