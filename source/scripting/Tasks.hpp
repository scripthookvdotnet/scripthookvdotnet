#pragma once

#include "Vector3.hpp"

namespace GTA
{
	#pragma region Forward Declarations
	ref class Ped;
	ref class Vehicle;
	enum class VehicleSeat;
	ref class Entity;
	ref class TaskSequence;
	enum class FiringPattern : System::UInt32;
	#pragma endregion

	[System::Flags]
	public enum class AnimationFlags
	{
		None = 0,
		Loop = 1,
		StayInEndFrame = 2,
		UpperBodyOnly = 16,
		AllowRotation = 32,
		CancelableWithMovement = 128,
		RagdollOnCollision = 4194304,
	};

	[System::Flags]
	public enum class LeaveVehicleFlags
	{
		None = 0,
		WarpOut = 16,
		LeaveDoorOpen = 256,
		BailOut = 4096,
	};

	public ref class Tasks
	{
	public:
		void AchieveHeading(float heading);
		void AchieveHeading(float heading, int timeout);
		void AimAt(Entity ^target, int duration);
		void AimAt(Math::Vector3 target, int duration);
		void Arrest(Ped ^ped);
		void ChatTo(Ped ^ped);
		void Climb();
		void Cower(int duration);
		void CruiseWithVehicle(Vehicle ^vehicle, float speed);
		void CruiseWithVehicle(Vehicle ^vehicle, float speed, int drivingstyle);
		void DriveTo(Vehicle ^vehicle, Math::Vector3 target, float radius, float speed);
		void DriveTo(Vehicle ^vehicle, Math::Vector3 target, float radius, float speed, int drivingstyle);
		void EnterVehicle();
		void EnterVehicle(Vehicle ^vehicle, VehicleSeat seat);
		void EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout);
		void EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout, float speed);
		void EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout, float speed, int flag);
		static void EveryoneLeaveVehicle(Vehicle ^vehicle);
		void FightAgainst(Ped ^target);
		void FightAgainst(Ped ^target, int duration);
		void FightAgainstHatedTargets(float radius);
		void FightAgainstHatedTargets(float radius, int duration);
		void FleeFrom(Ped ^ped);
		void FleeFrom(Ped ^ped, int duration);
		void FleeFrom(Math::Vector3 position);
		void FleeFrom(Math::Vector3 position, int duration);
		void FollowPointRoute(... array<Math::Vector3> ^points);
		void GoTo(Entity ^target);
		void GoTo(Entity ^target, Math::Vector3 offset);
		void GoTo(Entity ^target, Math::Vector3 offset, int timeout);
		void GoTo(Math::Vector3 position);
		void GoTo(Math::Vector3 position, bool ignorePaths);
		void GoTo(Math::Vector3 position, bool ignorePaths, int timeout);
		void FollowToOffsetFromEntity(Entity ^target, Math::Vector3 offset, int timeout, float stoppingRange);
		void FollowToOffsetFromEntity(Entity ^target, Math::Vector3 offset, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing);
		void GuardCurrentPosition();
		void HandsUp(int duration);
		void Jump();
		void LeaveVehicle();
		void LeaveVehicle(Vehicle ^vehicle, bool closeDoor);
		void LeaveVehicle(LeaveVehicleFlags flags);
		void LeaveVehicle(Vehicle ^vehicle, LeaveVehicleFlags flags);
		void LookAt(Entity ^target);
		void LookAt(Entity ^target, int duration);
		void LookAt(Math::Vector3 position);
		void LookAt(Math::Vector3 position, int duration);
		void ParachuteTo(Math::Vector3 position);
		void ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading);
		void ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading, float radius);
		void ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading, float radius, bool keepEngineOn);
		void PerformSequence(TaskSequence ^sequence);

		void PlayAnimation(System::String ^animDict, System::String ^animName, float speed, int duration, bool lastAnimation, float playbackRate);
		/// <summary>
		/// Starts an animation.
		/// </summary>
		/// <param name="animDict">The animation dictionary.</param>
		/// <param name="animName">The animation name.</param>
		void PlayAnimation(System::String ^animDict, System::String ^animName);
		/// <summary>
		/// Starts an animation.
		/// </summary>
		/// <param name="animDict">The animation dictionary.</param>
		/// <param name="animName">The animation name.</param>
		/// <param name="blendInSpeed">Normal value is 8.0.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="flags">The animation flags.</param>
		void PlayAnimation(System::String ^animDict, System::String ^animName, float blendInSpeed, int duration, AnimationFlags flags);
		/// <summary>
		/// Starts an animation.
		/// </summary>
		/// <param name="animDict">The animation dictionary.</param>
		/// <param name="animName">The animation name.</param>
		/// <param name="blendInSpeed">Normal value is 8.0.</param>
		/// <param name="blendOutSpeed">Normal value is -8.0.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="flags">The animation flags.</param>
		/// <param name="playbackRate">Values are between 0.0 and 1.0.</param>
		void PlayAnimation(System::String ^animDict, System::String ^animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate);
		void PutAwayMobilePhone();
		void PutAwayParachute();
		void ReactAndFlee(Ped ^ped);
		void ReloadWeapon();
		void RunTo(Math::Vector3 position);
		void RunTo(Math::Vector3 position, bool ignorePaths);
		void RunTo(Math::Vector3 position, bool ignorePaths, int timeout);
		void ShootAt(Ped ^target);
		void ShootAt(Ped ^target, int duration);
		void ShootAt(Ped ^target, int duration, FiringPattern pattern);
		void ShootAt(Math::Vector3 position);
		void ShootAt(Math::Vector3 position, int duration);
		void ShootAt(Math::Vector3 position, int duration, FiringPattern pattern);
		void ShuffleToNextVehicleSeat(Vehicle ^vehicle);
		void Skydive();
		void SlideTo(Math::Vector3 position, float heading);
		void StandStill(int duration);
		void StartScenario(System::String ^name, Math::Vector3 position);
		void SwapWeapon();
		void TurnTo(Entity ^target);
		void TurnTo(Entity ^target, int duration);
		void TurnTo(Math::Vector3 position);
		void TurnTo(Math::Vector3 position, int duration);
		void UseMobilePhone();
		void UseMobilePhone(int duration);
		void UseParachute();
		void VehicleChase(Ped ^target);
		void VehicleShootAtPed(Ped ^target);
		void Wait(int duration);
		void WanderAround();
		void WanderAround(Math::Vector3 position, float radius);
		void WarpIntoVehicle(Vehicle ^vehicle, VehicleSeat seat);
		void WarpOutOfVehicle(Vehicle ^vehicle);

		void ClearAll();
		void ClearAllImmediately();
		void ClearLookAt();
		void ClearSecondary();
		void ClearAnimation(System::String ^animSet, System::String ^animName);

	internal:
		Tasks(Ped ^ped);

	private:
		Ped ^_ped;
	};

	public ref class TaskSequence sealed
	{
	public:
		TaskSequence();
		TaskSequence(int handle);
		~TaskSequence();

		property int Handle
		{
			int get();
		}
		property int Count
		{
			int get();
		}
		property bool IsClosed
		{
			bool get();
		}
		property Tasks ^AddTask
		{
			Tasks ^get();
		}

		void Close();
		void Close(bool repeat);

	private:
		static Ped ^_nullPed;
		int _handle, _count;
		bool _isClosed;
	};
}