#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Prop.hpp"
#include "Tasks.hpp"
#include "Weapon.hpp"
#include "World.hpp"
#include "Native.hpp"

namespace GTA
{
	using namespace System::Collections::Generic;

	Ped::Ped(int handle) : Entity(handle), _tasks(gcnew Tasks(this)), _euphoria(gcnew NaturalMotion::Euphoria(this))
	{
	}

	int Ped::Accuracy::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_ACCURACY, Handle);
	}
	void Ped::Accuracy::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_ACCURACY, Handle, value);
	}
	void Ped::AlwaysKeepTask::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_KEEP_TASK, Handle, value);
	}
	Tasks ^Ped::Task::get()
	{
		return _tasks;
	}
	int Ped::TaskSequenceProgress::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_SEQUENCE_PROGRESS, Handle);
	}
	GTA::Gender Ped::Gender::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_MALE, Handle) ? GTA::Gender::Male : GTA::Gender::Female;
	}
	int Ped::Armor::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_ARMOUR, Handle);
	}
	void Ped::Armor::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_ARMOUR, Handle, value);
	}
	NaturalMotion::Euphoria ^Ped::Euphoria::get()
	{
		return _euphoria;
	}
	int Ped::Money::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_MONEY, Handle);
	}
	void Ped::Money::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_MONEY, Handle, value);
	}
	bool Ped::IsPlayer::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_A_PLAYER, Handle);
	}
	bool Ped::IsHuman::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_HUMAN, Handle);
	}
	bool Ped::IsIdle::get()
	{
		return !IsInjured && !IsRagdoll && !IsInAir && !IsOnFire && !IsDucking && !IsGettingIntoAVehicle && !IsInCombat && !IsInMeleeCombat && !(IsInVehicle() && !IsSittingInVehicle());
	}
	bool Ped::IsProne::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_PRONE, Handle);
	}
	bool Ped::IsDucking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_DUCKING, Handle);
	}
	void Ped::IsDucking::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DUCKING, Handle, value);
	}
	bool Ped::IsGettingUp::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GETTING_UP, Handle);
	}
	bool Ped::IsGettingIntoAVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GETTING_INTO_A_VEHICLE, Handle);
	}
	bool Ped::IsFalling::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_FALLING, Handle);
	}
	bool Ped::IsJumping::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_JUMPING, Handle);
	}
	bool Ped::IsClimbing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_CLIMBING, Handle);
	}
	bool Ped::IsVaulting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_VAULTING, Handle);
	}
	bool Ped::IsDiving::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_DIVING, Handle);
	}
	bool Ped::IsFleeing::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_FLEEING, Handle);
	}
	bool Ped::IsTryingToEnterALockedVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_TRYING_TO_ENTER_A_LOCKED_VEHICLE, Handle);
	}
	bool Ped::IsRagdoll::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RAGDOLL, Handle);
	}
	bool Ped::IsInjured::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_INJURED, Handle);
	}
	bool Ped::IsJacking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_JACKING, Handle);
	}
	bool Ped::IsBeingJacked::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_JACKED, Handle);
	}
	bool Ped::IsBeingStunned::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_STUNNED, Handle);
	}
	bool Ped::IsPerformingStealthKill::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_PERFORMING_STEALTH_KILL, Handle);
	}
	bool Ped::IsBeingStealthKilled::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_BEING_STEALTH_KILLED, Handle);
	}
	bool Ped::WasKilledByStealth::get()
	{
		return Native::Function::Call<bool>(Native::Hash::WAS_PED_KILLED_BY_STEALTH, Handle);
	}
	bool Ped::WasKilledByTakedown::get()
	{
		return Native::Function::Call<bool>(Native::Hash::WAS_PED_KILLED_BY_TAKEDOWN, Handle);
	}
	bool Ped::IsShooting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SHOOTING, Handle);
	}
	bool Ped::IsReloading::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RELOADING, Handle);
	}
	bool Ped::IsInCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COMBAT, Handle);
	}
	bool Ped::IsInMeleeCombat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_MELEE_COMBAT, Handle);
	}
	bool Ped::IsAimingFromCover::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_AIMING_FROM_COVER, Handle);
	}
	bool Ped::IsInCoverFacingLeft::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COVER_FACING_LEFT, Handle);
	}
	bool Ped::IsGoingIntoCover::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GOING_INTO_COVER, Handle);
	}
	bool Ped::IsDoingDriveBy::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_DOING_DRIVEBY, Handle);
	}
	bool Ped::IsInGroup::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_GROUP, Handle);
	}
	bool Ped::IsSwimming::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING, Handle);
	}
	bool Ped::IsSwimmingUnderWater::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SWIMMING_UNDER_WATER, Handle);
	}
	bool Ped::IsOnFoot::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_ON_FOOT, Handle);
	}
	bool Ped::IsOnBike::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_ON_ANY_BIKE, Handle);
	}
	bool Ped::IsInBoat::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_BOAT, Handle);
	}
	bool Ped::IsInSub::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_SUB, Handle);
	}
	bool Ped::IsInHeli::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_HELI, Handle);
	}
	bool Ped::IsInPlane::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_PLANE, Handle);
	}
	bool Ped::IsInTrain::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_TRAIN, Handle);
	}
	bool Ped::IsInFlyingVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_FLYING_VEHICLE, Handle);
	}
	bool Ped::IsInPoliceVehicle::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_POLICE_VEHICLE, Handle);
	}
	Vehicle ^Ped::CurrentVehicle::get()
	{
		if (!IsInVehicle())
		{
			return nullptr;
		}

		return Native::Function::Call<Vehicle ^>(Native::Hash::GET_VEHICLE_PED_IS_IN, Handle, false);
	}
	PedGroup ^Ped::CurrentPedGroup::get()
	{
		if (!IsInGroup)
		{
			return nullptr;
		}

		return Native::Function::Call<PedGroup ^>(Native::Hash::GET_PED_GROUP_INDEX, Handle, false);
	}
	void Ped::IsEnemy::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_AS_ENEMY, Handle, value);
	}
	void Ped::IsPriorityTargetForEnemies::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_ENTITY_IS_TARGET_PRIORITY, Handle, value, 0);
	}
	void Ped::AlwaysDiesOnLowHealth::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_WHEN_INJURED, Handle, value);
	}
	void Ped::BlockPermanentEvents::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_BLOCKING_OF_NON_TEMPORARY_EVENTS, Handle, value);
	}
	bool Ped::CanRagdoll::get()
	{
		return Native::Function::Call<bool>(Native::Hash::CAN_PED_RAGDOLL, Handle);
	}
	void Ped::CanRagdoll::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_RAGDOLL, Handle, value);
	}
	void Ped::CanSwitchWeapons::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_SWITCH_WEAPON, Handle, value);
	}
	void Ped::CanSufferCriticalHits::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_SUFFERS_CRITICAL_HITS, Handle, value);
	}
	bool Ped::CanFlyThroughWindscreen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_PED_CONFIG_FLAG, Handle, 32, true);
	}
	void Ped::CanFlyThroughWindscreen::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CONFIG_FLAG, Handle, 32, value);
	}
	void Ped::CanBeKnockedOffBike::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, Handle, value);
	}
	void Ped::CanBeDraggedOutOfVehicle::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_DRAGGED_OUT, Handle, value);
	}
	void Ped::CanBeTargetted::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_BE_TARGETTED, Handle, value);
	}
	void Ped::CanPlayGestures::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_CAN_PLAY_GESTURE_ANIMS, Handle, value);
	}
	bool Ped::IsStopped::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_STOPPED, Handle);
	}
	bool Ped::IsWalking::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_WALKING, Handle);
	}
	bool Ped::IsRunning::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_RUNNING, Handle);
	}
	bool Ped::IsSprinting::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SPRINTING, Handle);
	}
	void Ped::NeverLeavesGroup::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_NEVER_LEAVES_GROUP, Handle, value);
	}
	int Ped::RelationshipGroup::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_RELATIONSHIP_GROUP_HASH, Handle);
	}
	void Ped::RelationshipGroup::set(int group)
	{
		Native::Function::Call(Native::Hash::SET_PED_RELATIONSHIP_GROUP_HASH, Handle, group);
	}
	void Ped::DrivingSpeed::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_CRUISE_SPEED, Handle, value);
	}
	void Ped::MaxDrivingSpeed::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_MAX_CRUISE_SPEED, Handle, value);
	}
	int Ped::MaxHealth::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_MAX_HEALTH, Handle);
	}
	void Ped::MaxHealth::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_MAX_HEALTH, Handle, value);
	}
	void Ped::DrivingStyle::set(GTA::DrivingStyle value)
	{
		Native::Function::Call(Native::Hash::SET_DRIVE_TASK_DRIVING_STYLE, Handle, static_cast<int>(value));
	}
	void Ped::FiringPattern::set(GTA::FiringPattern value)
	{
		Native::Function::Call(Native::Hash::SET_PED_FIRING_PATTERN, Handle, static_cast<int>(value));
	}
	void Ped::ShootRate::set(int value)
	{
		Native::Function::Call(Native::Hash::SET_PED_SHOOT_RATE, Handle, value);
	}
	void Ped::DropsWeaponsOnDeath::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DROPS_WEAPONS_WHEN_DEAD, Handle, value);
	}
	void Ped::DiesInstantlyInWater::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_INSTANTLY_IN_WATER, Handle, value);
	}
	void Ped::DrownsInWater::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_IN_WATER, Handle, value);
	}
	void Ped::DrownsInSinkingVehicle::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_PED_DIES_IN_SINKING_VEHICLE, Handle, value);
	}
	WeaponCollection ^Ped::Weapons::get()
	{
		if (ReferenceEquals(_weapons, nullptr))
		{
			_weapons = gcnew WeaponCollection(this);
		}

		return _weapons;
	}

	bool Ped::IsInVehicle()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_ANY_VEHICLE, Handle, 0);
	}
	bool Ped::IsInVehicle(Vehicle ^vehicle)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_VEHICLE, Handle, vehicle->Handle, 0);
	}
	bool Ped::IsSittingInVehicle()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SITTING_IN_ANY_VEHICLE, Handle);
	}
	bool Ped::IsSittingInVehicle(Vehicle ^vehicle)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_SITTING_IN_VEHICLE, Handle, vehicle->Handle);
	}
	Relationship Ped::GetRelationshipWithPed(Ped ^ped)
	{
		return static_cast<Relationship>(Native::Function::Call<int>(Native::Hash::GET_RELATIONSHIP_BETWEEN_PEDS, Handle, ped->Handle));
	}
	void Ped::SetIntoVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		Native::Function::Call(Native::Hash::SET_PED_INTO_VEHICLE, Handle, vehicle->Handle, static_cast<int>(seat));
	}
	void Ped::WetnessHeight::set(float value)
	{
		Native::Function::Call<float>(Native::Hash::SET_PED_WETNESS_HEIGHT, Handle, value);
	}
	bool Ped::IsInCover()
	{
		return IsInCover(false);
	}
	bool Ped::IsInCover(bool expectUseWeapon)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COVER, Handle, expectUseWeapon);
	}
	bool Ped::IsInCombatAgainst(Ped ^target)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_IN_COMBAT, Handle, target);
	}
	bool Ped::IsHeadtracking(Entity ^entity)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_HEADTRACKING_ENTITY, Handle, entity);
	}

	Ped ^Ped::GetJacker()
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_PEDS_JACKER, Handle);
	}
	Ped ^Ped::GetJackTarget()
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_JACK_TARGET, Handle);
	}
	Entity ^Ped::GetKiller()
	{
		return Native::Function::Call<Entity ^>(Native::Hash::_GET_PED_KILLER, Handle);
	}

	void Ped::Kill()
	{
		Health = -1;
	}
	void Ped::ResetVisibleDamage()
	{
		Native::Function::Call(Native::Hash::RESET_PED_VISIBLE_DAMAGE, Handle);
	}
	void Ped::ClearBloodDamage()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_BLOOD_DAMAGE, Handle);
	}
	void Ped::ApplyDamage(int damageAmount)
	{
		Native::Function::Call(Native::Hash::APPLY_DAMAGE_TO_PED, Handle, damageAmount, true);
	}
	Math::Vector3 Ped::GetBoneCoord(Bone BoneID)
	{
		return GetBoneCoord(BoneID, Math::Vector3::Zero);
	}
	Math::Vector3 Ped::GetBoneCoord(Bone BoneID, Math::Vector3 Offset)
	{
		return Native::Function::Call<Math::Vector3>(Native::Hash::GET_PED_BONE_COORDS, Handle, (int)BoneID, Offset.X, Offset.Y, Offset.Z);
	}
	int Ped::GetBoneIndex(Bone BoneID)
	{
		return Native::Function::Call<int>(Native::Hash::GET_PED_BONE_INDEX, Handle, (int)BoneID);
	}

	PedGroup::PedGroup() : _handle(Native::Function::Call<int>(Native::Hash::CREATE_GROUP, 0))
	{
	}
	PedGroup::PedGroup(int handle) : _handle(handle)
	{
	}
	PedGroup::~PedGroup()
	{
		Native::Function::Call(Native::Hash::REMOVE_GROUP, _handle);
	}

	int PedGroup::Handle::get()
	{
		return _handle;
	}
	Ped ^PedGroup::Leader::get()
	{
		return Native::Function::Call<Ped ^>(Native::Hash::_0x5CCE68DBD5FE93EC, Handle);
	}
	int PedGroup::MemberCount::get()
	{
		int count, val1;
		Native::Function::Call(Native::Hash::GET_GROUP_SIZE, Handle, &val1, &count);
		return count;
	}
	void PedGroup::SeparationRange::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_GROUP_SEPARATION_RANGE, Handle, value);
	}
	void PedGroup::FormationType::set(GTA::FormationType value)
	{
		Native::Function::Call(Native::Hash::SET_GROUP_FORMATION, Handle, static_cast<int>(value));
	}

	void PedGroup::Add(Ped ^ped, bool leader)
	{
		if (leader)
		{
			Native::Function::Call(Native::Hash::SET_PED_AS_GROUP_LEADER, ped->Handle, Handle);
		}
		else
		{
			Native::Function::Call(Native::Hash::SET_PED_AS_GROUP_MEMBER, ped->Handle, Handle);
		}
	}
	void PedGroup::Remove(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::REMOVE_PED_FROM_GROUP, ped->Handle);
	}
	Ped ^PedGroup::GetMember(int index)
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_PED_AS_GROUP_MEMBER, Handle, index);
	}
	bool PedGroup::Exists()
	{
		return Exists(this);
	}
	bool PedGroup::Exists(PedGroup ^pedGroup)
	{
		return !ReferenceEquals(pedGroup, nullptr) && Native::Function::Call<bool>(Native::Hash::DOES_GROUP_EXIST, pedGroup->Handle);
	}
	bool PedGroup::Contains(Ped ^ped)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_PED_GROUP_MEMBER, ped->Handle, Handle);
	}
	bool PedGroup::Equals(PedGroup ^pedGroup)
	{
		return !System::Object::ReferenceEquals(pedGroup, nullptr) && Handle == pedGroup->Handle;
	}

	System::Collections::Generic::List<Ped ^> ^PedGroup::ToList(bool includingLeader)
	{
		List<Ped ^> ^list = gcnew List<Ped ^>();

		if (includingLeader)
		{
			Ped ^leader = Leader;

			if (!ReferenceEquals(leader, nullptr) && leader->Exists())
			{
				list->Add(leader);
			}
		}

		for (int i = 0; i < MemberCount; i++)
		{
			Ped ^ped = GetMember(i);

			if (!Object::ReferenceEquals(ped, nullptr) && ped->Exists())
			{
				list->Add(ped);
			}
		}

		return list;
	}
}