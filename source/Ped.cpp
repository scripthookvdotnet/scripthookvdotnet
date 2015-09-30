#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Tasks.hpp"
#include "Weapon.hpp"
#include "World.hpp"
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
	void Ped::AlwaysKeepTask::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_KEEP_TASK, this->Handle, value);
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
	bool Ped::IsTryingToEnterALockedVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_TRYING_TO_ENTER_A_LOCKED_VEHICLE, this->Handle);
	}
	bool Ped::IsRagdoll::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RAGDOLL, this->Handle);
	}
	bool Ped::IsInjured::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_INJURED, this->Handle);
	}
	bool Ped::IsJacking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_JACKING, this->Handle);
	}
	bool Ped::IsBeingJacked::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_JACKED, this->Handle);
	}
	bool Ped::IsBeingStunned::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_STUNNED, this->Handle);
	}
	bool Ped::IsPerformingStealthKill::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_PERFORMING_STEALTH_KILL, this->Handle);
	}
	bool Ped::IsBeingStealthKilled::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_STEALTH_KILLED, this->Handle);
	}
	bool Ped::WasKilledByStealth::get()
	{
		return Native::Function::Call<bool>(Native::Hash::WAS_PED_KILLED_BY_STEALTH, this->Handle);
	}
	bool Ped::WasKilledByTakedown::get()
	{
		return Native::Function::Call<bool>(Native::Hash::WAS_PED_KILLED_BY_TAKEDOWN, this->Handle);
	}
	bool Ped::IsShooting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SHOOTING, this->Handle);
	}
	bool Ped::IsReloading::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RELOADING, this->Handle);
	}
	bool Ped::IsInCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COMBAT, this->Handle);
	}
	bool Ped::IsInMeleeCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_MELEE_COMBAT, this->Handle);
	}
	bool Ped::IsAimingFromCover::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_AIMING_FROM_COVER, this->Handle);
	}
	bool Ped::IsInCoverFacingLeft::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COVER_FACING_LEFT, this->Handle);
	}
	bool Ped::IsGoingIntoCover::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GOING_INTO_COVER, this->Handle);
	}
	bool Ped::IsDoingDriveBy::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_DOING_DRIVEBY, this->Handle);
	}
	bool Ped::IsInGroup::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_GROUP, this->Handle);
	}
	bool Ped::IsSwimming::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING, this->Handle);
	}
	bool Ped::IsSwimmingUnderWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING_UNDER_WATER, this->Handle);
	}
	bool Ped::IsOnFoot::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_ON_FOOT, this->Handle);
	}
	bool Ped::IsOnBike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_ON_ANY_BIKE, this->Handle);
	}
	bool Ped::IsInBoat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_BOAT, this->Handle);
	}
	bool Ped::IsInSub::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_SUB, this->Handle);
	}
	bool Ped::IsInHeli::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_HELI, this->Handle);
	}
	bool Ped::IsInPlane::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_PLANE, this->Handle);
	}
	bool Ped::IsInTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_TRAIN, this->Handle);
	}
	bool Ped::IsInFlyingVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_FLYING_VEHICLE, this->Handle);
	}
	bool Ped::IsInPoliceVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_POLICE_VEHICLE, this->Handle);
	}
	Vehicle ^Ped::CurrentVehicle::get()
	{
		if (!IsInVehicle())
		{
			return nullptr;
		}

		return Native::Function::Call<Vehicle ^>(Native::Hash::GET_VEHICLE_PED_IS_IN, this->Handle, false);
	}
	PedGroup ^Ped::CurrentPedGroup::get()
	{
		if (!IsInGroup)
		{
			return nullptr;
		}

		return Native::Function::Call<PedGroup ^>(Native::Hash::GET_PED_GROUP_INDEX, this->Handle, false);
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
	void Ped::CanSufferCriticalHits::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_SUFFERS_CRITICAL_HITS, this->Handle, value);
	}
	bool Ped::CanFlyThroughWindscreen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_PED_CONFIG_FLAG, this->Handle, 32, true);
	}
	void Ped::CanFlyThroughWindscreen::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CONFIG_FLAG, this->Handle, 32, value);
	}
	void Ped::CanBeKnockedOffBike::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, this->Handle, value);
	}
	void Ped::CanBeDraggedOutOfVehicle::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_DRAGGED_OUT, this->Handle, value);
	}
	void Ped::CanBeTargetted::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_TARGETTED, this->Handle, value);
	}
	void Ped::CanPlayGestures::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_PLAY_GESTURE_ANIMS, this->Handle, value);
	}
	bool Ped::IsStopped::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_STOPPED, this->Handle);
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
	void Ped::NeverLeavesGroup::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_NEVER_LEAVES_GROUP, this->Handle, value);
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
	void Ped::DrivingStyle::set(GTA::DrivingStyle value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_DRIVING_STYLE, this->Handle, static_cast<int>(value));
	}
	void Ped::FiringPattern::set(GTA::FiringPattern value)
	{
		Native::Function::Call(Native::Hash::SET_PED_FIRING_PATTERN, this->Handle, static_cast<int>(value));
	}
	void Ped::ShootRate::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_SHOOT_RATE, this->Handle, value);
	}
	void Ped::DiesInstantlyInWater::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_INSTANTLY_IN_WATER, this->Handle, value);
	}
	void Ped::DrownsInWater::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_IN_WATER, this->Handle, value);
	}
	void Ped::DrownsInSinkingVehicle::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_IN_SINKING_VEHICLE, this->Handle, value);
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
	void Ped::SetIntoVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		Native::Function::Call(Native::Hash::SET_PED_INTO_VEHICLE, this->Handle, vehicle->Handle, static_cast<int>(seat));
	}
	void Ped::WetnessHeight::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_PED_WETNESS_HEIGHT, this->Handle, value);
	}
	bool Ped::IsInCover()
	{
		return IsInCover(false);
	}
	bool Ped::IsInCover(bool expectUseWeapon)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COVER, this->Handle, expectUseWeapon);
	}

	WeaponCollection ^Ped::Weapons::get()
	{
		if (this->pWeapons == nullptr)
		{
			this->pWeapons = gcnew WeaponCollection(this);
		}
		return this->pWeapons;
	}

	Ped ^Ped::GetJacker()
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_PEDS_JACKER, this->Handle);
	}
	Ped ^Ped::GetJackTarget()
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_JACK_TARGET, this->Handle);
	}
	Entity ^Ped::GetKiller()
	{
		return Native::Function::Call<Entity ^>(Native::Hash::_GET_PED_KILLER, this->Handle);
	}

	void Ped::Kill()
	{
		Health = -1;
	}
	void Ped::ResetVisibleDamage()
	{
		Native::Function::Call(Native::Hash::RESET_PED_VISIBLE_DAMAGE, this->Handle);
	}
	void Ped::ClearBloodDamage()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_BLOOD_DAMAGE, this->Handle);
	}
	void Ped::ApplyDamage(int damageAmount)
	{
		Native::Function::Call(Native::Hash::APPLY_DAMAGE_TO_PED, this->Handle, damageAmount, true);
	}
	Math::Vector3 Ped::GetBoneCoord(Bone BoneID)
	{
		return GetBoneCoord(BoneID, Math::Vector3::Zero);
	}
	Math::Vector3 Ped::GetBoneCoord(Bone BoneID, Math::Vector3 Offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_PED_BONE_COORDS, this->Handle, (int)BoneID, Offset.X, Offset.Y, Offset.Z);
	}
	int Ped::GetBoneIndex(Bone BoneID)
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_BONE_INDEX, this->Handle, (int)BoneID);
	}

	PedGroup::PedGroup() : mHandle(Native::Function::Call<int>(Native::Hash::CREATE_GROUP, 0))
	{
	}
	PedGroup::PedGroup(int handle) : mHandle(handle)
	{
	}
	PedGroup::~PedGroup()
	{
		Native::Function::Call(Native::Hash::REMOVE_GROUP, this->Handle);
	}

	int PedGroup::Handle::get()
	{
		return this->mHandle;
	}
	int PedGroup::MemberCount::get()
	{
		int count, val1;
		Native::Function::Call(Native::Hash::SET_GROUP_SEPARATION_RANGE, this->Handle, &val1, &count);
		return count;
	}
	void PedGroup::SeparationRange::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_GROUP_SEPARATION_RANGE, this->Handle, value);
	}
	void PedGroup::FormationType::set(GTA::FormationType value)
	{
		Native::Function::Call(Native::Hash::SET_GROUP_FORMATION, this->Handle, static_cast<int>(value));
	}

	void PedGroup::Add(Ped ^ped, bool leader)
	{
		if (leader)
		{
			Native::Function::Call(Native::Hash::SET_PED_AS_GROUP_LEADER, ped->Handle, this->mHandle);
		}
		else
		{
			Native::Function::Call(Native::Hash::SET_PED_AS_GROUP_MEMBER, ped->Handle, this->Handle);
		}
	}
	void PedGroup::Remove(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::REMOVE_PED_FROM_GROUP, ped->Handle);
	}
	bool PedGroup::Exists()
	{
		return Exists(this);
	}
	bool PedGroup::Exists(PedGroup ^pedGroup)
	{
		return !Object::ReferenceEquals(pedGroup, nullptr) && Native::Function::Call<bool>(Native::Hash::DOES_GROUP_EXIST, pedGroup->Handle);
	}
	bool PedGroup::Contains(Ped ^ped)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GROUP_MEMBER, ped->Handle, this->Handle);
	}
}