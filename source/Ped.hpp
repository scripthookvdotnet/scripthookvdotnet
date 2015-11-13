#pragma once

#include "Entity.hpp"
#include "Euphoria.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Tasks;
	ref class PedGroup;
	ref class Vehicle;
	ref class WeaponCollection;
	value class Model;
	enum class VehicleSeat;
	enum class Relationship;
	#pragma endregion

	public enum class Gender
	{
		Male,
		Female
	};
	public enum class DrivingStyle
	{
		Normal = 0xC00AB,
		IgnoreLights = 0x2C0025,
		SometimesOvertakeTraffic = 5,
		Rushed = 0x400C0025,
		AvoidTraffic = 0xC0024,
		AvoidTrafficExtremely = 6,
	};
	public enum class Bone
	{
		SKEL_ROOT = 0x0,
		SKEL_Pelvis = 0x2e28,
		SKEL_L_Thigh = 0xe39f,
		SKEL_L_Calf = 0xf9bb,
		SKEL_L_Foot = 0x3779,
		SKEL_L_Toe0 = 0x83c,
		IK_L_Foot = 0xfedd,
		PH_L_Foot = 0xe175,
		MH_L_Knee = 0xb3fe,
		SKEL_R_Thigh = 0xca72,
		SKEL_R_Calf = 0x9000,
		SKEL_R_Foot = 0xcc4d,
		SKEL_R_Toe0 = 0x512d,
		IK_R_Foot = 0x8aae,
		PH_R_Foot = 0x60e6,
		MH_R_Knee = 0x3fcf,
		RB_L_ThighRoll = 0x5c57,
		RB_R_ThighRoll = 0x192a,
		SKEL_Spine_Root = 0xe0fd,
		SKEL_Spine0 = 0x5c01,
		SKEL_Spine1 = 0x60f0,
		SKEL_Spine2 = 0x60f1,
		SKEL_Spine3 = 0x60f2,
		SKEL_L_Clavicle = 0xfcd9,
		SKEL_L_UpperArm = 0xb1c5,
		SKEL_L_Forearm = 0xeeeb,
		SKEL_L_Hand = 0x49d9,
		SKEL_L_Finger00 = 0x67f2,
		SKEL_L_Finger01 = 0xff9,
		SKEL_L_Finger02 = 0xffa,
		SKEL_L_Finger10 = 0x67f3,
		SKEL_L_Finger11 = 0x1049,
		SKEL_L_Finger12 = 0x104a,
		SKEL_L_Finger20 = 0x67f4,
		SKEL_L_Finger21 = 0x1059,
		SKEL_L_Finger22 = 0x105a,
		SKEL_L_Finger30 = 0x67f5,
		SKEL_L_Finger31 = 0x1029,
		SKEL_L_Finger32 = 0x102a,
		SKEL_L_Finger40 = 0x67f6,
		SKEL_L_Finger41 = 0x1039,
		SKEL_L_Finger42 = 0x103a,
		PH_L_Hand = 0xeb95,
		IK_L_Hand = 0x8cbd,
		RB_L_ForeArmRoll = 0xee4f,
		RB_L_ArmRoll = 0x1470,
		MH_L_Elbow = 0x58b7,
		SKEL_R_Clavicle = 0x29d2,
		SKEL_R_UpperArm = 0x9d4d,
		SKEL_R_Forearm = 0x6e5c,
		SKEL_R_Hand = 0xdead,
		SKEL_R_Finger00 = 0xe5f2,
		SKEL_R_Finger01 = 0xfa10,
		SKEL_R_Finger02 = 0xfa11,
		SKEL_R_Finger10 = 0xe5f3,
		SKEL_R_Finger11 = 0xfa60,
		SKEL_R_Finger12 = 0xfa61,
		SKEL_R_Finger20 = 0xe5f4,
		SKEL_R_Finger21 = 0xfa70,
		SKEL_R_Finger22 = 0xfa71,
		SKEL_R_Finger30 = 0xe5f5,
		SKEL_R_Finger31 = 0xfa40,
		SKEL_R_Finger32 = 0xfa41,
		SKEL_R_Finger40 = 0xe5f6,
		SKEL_R_Finger41 = 0xfa50,
		SKEL_R_Finger42 = 0xfa51,
		PH_R_Hand = 0x6f06,
		IK_R_Hand = 0x188e,
		RB_R_ForeArmRoll = 0xab22,
		RB_R_ArmRoll = 0x90ff,
		MH_R_Elbow = 0xbb0,
		SKEL_Neck_1 = 0x9995,
		SKEL_Head = 0x796e,
		IK_Head = 0x322c,
		FACIAL_facialRoot = 0xfe2c,
		FB_L_Brow_Out_000 = 0xe3db,
		FB_L_Lid_Upper_000 = 0xb2b6,
		FB_L_Eye_000 = 0x62ac,
		FB_L_CheekBone_000 = 0x542e,
		FB_L_Lip_Corner_000 = 0x74ac,
		FB_R_Lid_Upper_000 = 0xaa10,
		FB_R_Eye_000 = 0x6b52,
		FB_R_CheekBone_000 = 0x4b88,
		FB_R_Brow_Out_000 = 0x54c,
		FB_R_Lip_Corner_000 = 0x2ba6,
		FB_Brow_Centre_000 = 0x9149,
		FB_UpperLipRoot_000 = 0x4ed2,
		FB_UpperLip_000 = 0xf18f,
		FB_L_Lip_Top_000 = 0x4f37,
		FB_R_Lip_Top_000 = 0x4537,
		FB_Jaw_000 = 0xb4a0,
		FB_LowerLipRoot_000 = 0x4324,
		FB_LowerLip_000 = 0x508f,
		FB_L_Lip_Bot_000 = 0xb93b,
		FB_R_Lip_Bot_000 = 0xc33b,
		FB_Tongue_000 = 0xb987,
		RB_Neck_1 = 0x8b93,
		IK_Root = 0xdd1c
	};
	public enum class FiringPattern : System::UInt32
	{
		Default = 0,
		FullAuto = 0xC6EE6B4C,
		BurstFire = 0xD6FF6D61,
		BurstInCover = 0x026321F1,
		BurstFireDriveby = 0xD31265F2,
		FromGround = 0x2264E5D6,
		DelayFireByOneSec = 0x7A845691,
		SingleShot = 0x5D60E4E0,
		BurstFirePistol = 0xA018DB8A,
		BurstFireSMG = 0xD10DADEE,
		BurstFireRifle = 0x9C74B406,
		BurstFireMG = 0xB573C5B4,
		BurstFirePumpShotGun = 0x00BAC39B,
		BurstFireHeli = 0x914E786F,
		BurstFireMicro = 0x42EF03FD,
		BurstFireBursts = 0x42EF03FD,
		BurstFireTank = 0xE2CA3A71,
	};

	public ref class Ped sealed : public Entity
	{
	public:
		Ped(int handle);

		property int Accuracy
		{
			int get();
			void set(int value);
		}
		property bool AlwaysDiesOnLowHealth
		{
			void set(bool value);
		}
		property bool AlwaysKeepTask
		{
			void set(bool value);
		}
		property int Armor
		{
			int get();
			void set(int value);
		}
		property bool BlockPermanentEvents
		{
			void set(bool value);
		}
		property bool CanRagdoll
		{
			bool get();
			void set(bool value);
		}
		property bool CanSwitchWeapons
		{
			void set(bool value);
		}
		property bool CanSufferCriticalHits
		{
			void set(bool value);
		}
		property bool CanFlyThroughWindscreen
		{
			bool get();
			void set(bool value);
		}
		property bool CanBeKnockedOffBike
		{
			void set(bool value);
		}
		property bool CanBeDraggedOutOfVehicle
		{
			void set(bool value);
		}
		property bool CanBeTargetted
		{
			void set(bool value);
		}
		property bool CanPlayGestures
		{
			void set(bool value);
		}
		property PedGroup ^CurrentPedGroup
		{
			PedGroup ^get();
		}
		property Vehicle ^CurrentVehicle
		{
			Vehicle ^get();
		}
		property bool DiesInstantlyInWater
		{
			void set(bool value);
		}
		property float DrivingSpeed
		{
			void set(float value);
		}
		property DrivingStyle DrivingStyle
		{
			void set(GTA::DrivingStyle value);
		}
		property bool DrownsInWater
		{
			void set(bool value);
		}
		property bool DropsWeaponsOnDeath
		{
			void set(bool value);
		}
		property bool DrownsInSinkingVehicle
		{
			void set(bool value);
		}
		property NaturalMotion::Euphoria ^Euphoria
		{
			NaturalMotion::Euphoria ^get();
		}
		property FiringPattern FiringPattern
		{
			void set(GTA::FiringPattern value);
		}
		property GTA::Gender Gender
		{
			GTA::Gender get();
		}
		property bool IsAimingFromCover
		{
			bool get();
		}
		property bool IsBeingJacked
		{
			bool get();
		}
		property bool IsBeingStealthKilled
		{
			bool get();
		}
		property bool IsBeingStunned
		{
			bool get();
		}
		property bool IsDoingDriveBy
		{
			bool get();
		}
		property bool IsDucking
		{
			bool get();
			void set(bool value);
		}
		property bool IsEnemy
		{
			void set(bool value);
		}
		property bool IsHuman
		{
			bool get();
		}
		property bool IsIdle
		{
			bool get();
		}
		property bool IsProne
		{
			bool get();
		}
		property bool IsGettingUp
		{
			bool get();
		}
		property bool IsGettingIntoAVehicle
		{
			bool get();
		}
		property bool IsFalling
		{
			bool get();
		}
		property bool IsJumping
		{
			bool get();
		}
		property bool IsClimbing
		{
			bool get();
		}
		property bool IsVaulting
		{
			bool get();
		}
		property bool IsDiving
		{
			bool get();
		}
		property bool IsGoingIntoCover
		{
			bool get();
		}
		property bool IsFleeing
		{
			bool get();
		}
		property bool IsInjured
		{
			bool get();
		}
		property bool IsInBoat
		{
			bool get();
		}
		property bool IsInCombat
		{
			bool get();
		}
		property bool IsInCoverFacingLeft
		{
			bool get();
		}
		property bool IsInGroup
		{
			bool get();
		}
		property bool IsInFlyingVehicle
		{
			bool get();
		}
		property bool IsInHeli
		{
			bool get();
		}
		property bool IsInMeleeCombat
		{
			bool get();
		}
		property bool IsInPlane
		{
			bool get();
		}
		property bool IsInPoliceVehicle
		{
			bool get();
		}
		property bool IsInSub
		{
			bool get();
		}
		property bool IsInTrain
		{
			bool get();
		}
		property bool IsJacking
		{
			bool get();
		}
		property bool IsOnFoot
		{
			bool get();
		}
		property bool IsOnBike
		{
			bool get();
		}
		property bool IsPerformingStealthKill
		{
			bool get();
		}
		property bool IsPlayer
		{
			bool get();
		}
		property bool IsPriorityTargetForEnemies
		{
			void set(bool value);
		}
		property bool IsRagdoll
		{
			bool get();
		}
		property bool IsReloading
		{
			bool get();
		}
		property bool IsRunning
		{
			bool get();
		}
		property bool IsShooting
		{
			bool get();
		}
		property bool IsSprinting
		{
			bool get();
		}
		property bool IsStopped
		{
			bool get();
		}
		property bool IsSwimming
		{
			bool get();
		}
		property bool IsSwimmingUnderWater
		{
			bool get();
		}
		property bool IsTryingToEnterALockedVehicle
		{
			bool get();
		}
		property bool IsWalking
		{
			bool get();
		}
		property float MaxDrivingSpeed
		{
			void set(float value);
		}
		property int MaxHealth
		{
			int get() override;
			void set(int value) override;
		}
		property int Money
		{
			int get();
			void set(int value);
		}
		property bool NeverLeavesGroup
		{
			void set(bool value);
		}
		property int RelationshipGroup
		{
			int get();
			void set(int group);
		}
		property int ShootRate
		{
			void set(int value);
		}
		property Tasks ^Task
		{
			Tasks ^get();
		}
		property int TaskSequenceProgress
		{
			int get();
		}
		property bool WasKilledByStealth
		{
			bool get();
		}
		property bool WasKilledByTakedown
		{
			bool get();
		}
		property WeaponCollection ^Weapons
		{
			WeaponCollection ^get();
		}
		property float WetnessHeight
		{
			void set(float value);
		}

		bool IsInVehicle();
		bool IsInVehicle(Vehicle ^vehicle);
		bool IsSittingInVehicle();
		bool IsSittingInVehicle(Vehicle ^vehicle);
		Relationship GetRelationshipWithPed(Ped ^ped);
		void SetIntoVehicle(Vehicle ^vehicle, VehicleSeat seat);
		bool IsInCover();
		bool IsInCover(bool expectUseWeapon);
		bool IsInCombatAgainst(Ped ^target);
		bool IsHeadtracking(Entity ^entity);

		Ped ^GetJacker();
		Ped ^GetJackTarget();
		Entity ^GetKiller();
		void Kill();
		void ResetVisibleDamage();
		void ClearBloodDamage();
		void ApplyDamage(int damageAmount);
		Math::Vector3 GetBoneCoord(Bone BoneID);
		Math::Vector3 GetBoneCoord(Bone BoneID, Math::Vector3 Offset);
		int GetBoneIndex(Bone BoneID);

	private:
		Tasks ^_tasks;
		NaturalMotion::Euphoria ^_euphoria;
		WeaponCollection ^_weapons;
	};

	public enum class FormationType
	{
		Default = 0,
		Circle1 = 1,
		Circle2 = 2,
		Line = 3
	};

	public ref class PedGroup
	{
	public:
		PedGroup();
		PedGroup(int handle);
		~PedGroup();

		property int Handle
		{
			int get();
		}
		property Ped ^Leader
		{
			Ped ^get();
		}
		property int MemberCount
		{
			int get();
		}
		property float SeparationRange
		{
			void set(float value);
		}
		property FormationType FormationType
		{
			void set(GTA::FormationType value);
		}

		void Add(Ped ^ped, bool leader);
		void Remove(Ped ^ped);
		bool Exists();
		Ped ^GetMember(int index);
		static bool Exists(PedGroup ^pedGroup);
		bool Contains(Ped ^ped);
		virtual bool Equals(PedGroup ^pedGroup);

		System::Collections::Generic::List<Ped ^> ^ToList(bool includingLeader);

		virtual inline int GetHashCode() override
		{
			return Handle;
		}
		static inline bool operator==(PedGroup ^left, PedGroup ^right)
		{
			if (ReferenceEquals(left, nullptr))
			{
				return ReferenceEquals(right, nullptr);
			}

			return left->Equals(right);
		}
		static inline bool operator!=(PedGroup ^left, PedGroup ^right)
		{
			return !operator==(left, right);
		}

	private:
		int _handle;
	};
}