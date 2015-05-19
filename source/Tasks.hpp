#pragma once

#include "Vector3.hpp"

namespace GTA
{
	ref class Ped;
	ref class Vehicle;
	enum class VehicleSeat;
	ref class TaskSequence;

	public ref class Tasks
	{
	public:
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
		void GuardCurrentPosition();
		void HandsUp(int duration);
		void LeaveVehicle();
		void LeaveVehicle(Vehicle ^vehicle, bool closeDoor);
		void LookAt(Entity ^target);
		void LookAt(Entity ^target, int duration);
		void LookAt(Math::Vector3 position);
		void LookAt(Math::Vector3 position, int duration);
		void ParachuteTo(Math::Vector3 position);
		void ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading);
		void PerformSequence(TaskSequence ^sequence);
		void PlayAnimation(System::String ^animSet, System::String ^animName, float speed, int loop, bool lastAnimation, float playbackRate);
		void PutAwayMobilePhone();
		void PutAwayParachute();
		void ReactAndFlee(Ped ^ped);
		void ReloadWeapon();
		void RunTo(Math::Vector3 position);
		void RunTo(Math::Vector3 position, bool ignorePaths);
		void RunTo(Math::Vector3 position, bool ignorePaths, int timeout);
		void ShootAt(Ped ^target);
		void ShootAt(Ped ^target, int duration);
		void ShootAt(Math::Vector3 position);
		void ShootAt(Math::Vector3 position, int duration);		
		void ShuffleToNextVehicleSeat(Vehicle ^vehicle);
		void Skydive();
		void StandStill(int duration);
		void SlideToCoord(Math::Vector3 coord, float heading);
		void SwapWeapon();
		void StartScenario(System::String ^name, Math::Vector3 position);
		void TurnTo(Entity ^target);
		void TurnTo(Entity ^target, int duration);
		void TurnTo(Math::Vector3 position);
		void TurnTo(Math::Vector3 position, int duration);
		void UseMobilePhone();
		void UseMobilePhone(int duration);
		void UseParachute();
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
		Ped ^mPed;
	};
}