#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Tasks.hpp"
#include "Native.hpp"

namespace GTA
{
	Ped::Ped(int handle) : Entity(handle), mTasks(gcnew Tasks(this))
	{
	}

	int Ped::Accuracy::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_ACCURACY, this->Handle);
	}
	void Ped::Accuracy::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_ACCURACY, this->Handle, value);
	}
	Tasks ^Ped::Task::get()
	{
		return this->mTasks;
	}
	int Ped::TaskSequenceProgress::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_SEQUENCE_PROGRESS, this->Handle);
	}
	GTA::Gender Ped::Gender::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_MALE, this->Handle) ? GTA::Gender::Male : GTA::Gender::Female;
	}
	int Ped::Armor::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_ARMOUR, this->Handle);
	}
	void Ped::Armor::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_ARMOUR, this->Handle, value);
	}
	int Ped::Money::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_MONEY, this->Handle);
	}
	void Ped::Money::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_MONEY, this->Handle, value);
	}
	bool Ped::IsPlayer::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_A_PLAYER, this->Handle);
	}
	bool Ped::IsHuman::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_HUMAN, this->Handle);
	}
	bool Ped::IsIdle::get()
	{
		return !IsInjured && !IsRagdoll && !IsInAir && !IsOnFire && !IsDucking && !IsGettingIntoAVehicle && !IsInCombat && !IsInMeleeCombat && !(IsInVehicle() && !IsSittingInVehicle());
	}
	bool Ped::IsProne::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_PRONE, this->Handle);
	}
	bool Ped::IsDucking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_DUCKING, this->Handle);
	}
	void Ped::IsDucking::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DUCKING, this->Handle, value);
	}
	bool Ped::IsGettingUp::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GETTING_UP, this->Handle);
	}
	bool Ped::IsGettingIntoAVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GETTING_INTO_A_VEHICLE, this->Handle);
	}
	bool Ped::IsRagdoll::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RAGDOLL, this->Handle);
	}
	bool Ped::IsInjured::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_INJURED, this->Handle);
	}
	bool Ped::IsShooting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SHOOTING, this->Handle);
	}
	bool Ped::IsInCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COMBAT, this->Handle);
	}
	bool Ped::IsInMeleeCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_MELEE_COMBAT, this->Handle);
	}
	bool Ped::IsSwimming::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING, this->Handle);
	}
	bool Ped::IsSwimmingUnderWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING_UNDER_WATER, this->Handle);
	}
	Vehicle ^Ped::CurrentVehicle::get()
	{
		if (!IsInVehicle())
		{
			return nullptr;
		}

		return gcnew Vehicle(Native::Function::Call<int>(Native::Hash::GET_VEHICLE_PED_IS_IN, this->Handle, false));
	}
	void Ped::IsEnemy::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_AS_ENEMY, this->Handle, value);
	}
	void Ped::IsPriorityTargetForEnemies::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_IS_TARGET_PRIORITY, this->Handle, value, 0);
	}
	void Ped::AlwaysDiesOnLowHealth::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_WHEN_INJURED, this->Handle, value);
	}
	void Ped::BlockPermanentEvents::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, this->Handle, value);
	}
	bool Ped::CanRagdoll::get()
	{
		return Native::Function::Call<bool>(Native::Hash::CAN_PED_RAGDOLL, this->Handle);
	}
	void Ped::CanRagdoll::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_RAGDOLL, this->Handle, value);
	}
	void Ped::CanSwitchWeapons::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_SWITCH_WEAPON, this->Handle, value);
	}
	void Ped::CanBeKnockedOffBike::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, this->Handle, value);
	}
	void Ped::CanBeDraggedOutOfVehicle::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_DRAGGED_OUT, this->Handle, value);
	}
	void Ped::CanPlayGestures::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_PLAY_GESTURE_ANIMS, this->Handle, value);
	}
	bool Ped::IsWalking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_WALKING, this->Handle);
	}
	bool Ped::IsRunning::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RUNNING, this->Handle);
	}
	bool Ped::IsSprinting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SPRINTING, this->Handle);
	}
	int Ped::RelationshipGroup::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_RELATIONSHIP_GROUP_HASH, this->Handle);
	}
	void Ped::RelationshipGroup::set(int group)
	{
		Native::Function::Call(Native::Hash::SET_PED_RELATIONSHIP_GROUP_HASH, this->Handle, group);
	}
	void Ped::DrivingSpeed::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_CRUISE_SPEED, this->Handle, value);
	}
	void Ped::MaxDrivingSpeed::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_MAX_CRUISE_SPEED, this->Handle, value);
	}
	void Ped::DrivingStyle::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_DRIVING_STYLE, this->Handle, value);
	}

	bool Ped::IsInVehicle()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_VEHICLE, this->Handle, 0);
	}
	bool Ped::IsInVehicle(Vehicle ^vehicle)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_VEHICLE, this->Handle, vehicle->Handle, 0);
	}
	bool Ped::IsSittingInVehicle()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SITTING_IN_ANY_VEHICLE, this->Handle);
	}
	bool Ped::IsSittingInVehicle(Vehicle ^vehicle)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SITTING_IN_VEHICLE, this->Handle, vehicle->Handle);
	}
	Relationship Ped::GetRelationshipWithPed(Ped ^ped)
	{
		return static_cast<Relationship>(Native::Function::Call<int>(Native::Hash::GET_RELATIONSHIP_BETWEEN_PEDS, this->Handle, ped->Handle));
	}

	void Ped::Kill()
	{
		Health = -1;
	}
}