#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Tasks.hpp"
#include "Native.hpp"

namespace GTA
{
	Tasks::Tasks(Ped ^ped) : mPed(ped)
	{
	}
	void Tasks::AimAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_ENTITY, this->mPed->ID, target->ID, duration, 0);
	}
	void Tasks::AimAt(Math::Vector3 target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_COORD, this->mPed->ID, target.X, target.Y, target.Z, duration, 0, 0);
	}
	void Tasks::Arrest(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_ARREST_PED, this->mPed->ID, ped->ID);
	}
	void Tasks::ChatTo(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_CHAT_TO_PED, this->mPed->ID, ped->ID, 16, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
	}
	void Tasks::Climb()
	{
		Native::Function::Call(Native::Hash::TASK_CLIMB, this->mPed->ID, true);
	}
	void Tasks::Cower(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COWER, this->mPed->ID, duration);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed)
	{
		CruiseWithVehicle(vehicle, speed, 0);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_WANDER, this->mPed->ID, vehicle->ID, speed, drivingstyle);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed)
	{
		DriveTo(vehicle, position, radius, speed, 0);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, this->mPed->ID, vehicle->ID, position.X, position.Y, position.Z, speed, drivingstyle, radius);
	}
	void Tasks::EnterVehicle()
	{
		EnterVehicle(Vehicle::Any, VehicleSeat::Any, -1);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		EnterVehicle(vehicle, seat, -1);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout)
	{
		Native::Function::Call(Native::Hash::TASK_ENTER_VEHICLE, this->mPed->ID, vehicle->ID, timeout, static_cast<int>(seat), 0.0f, 0, 0);
	}
	void Tasks::EveryoneLeaveVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_EVERYONE_LEAVE_VEHICLE, vehicle->ID);
	}
	void Tasks::FightAgainst(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED, this->mPed->ID, target->ID, 0, 16);
	}
	void Tasks::FightAgainst(Ped ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED_TIMED, this->mPed->ID, target->ID, duration, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED, this->mPed->ID, radius, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, this->mPed->ID, radius, duration, 0);
	}
	void Tasks::FleeFrom(Ped ^target)
	{
		FleeFrom(target, -1);
	}
	void Tasks::FleeFrom(Ped ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_PED, this->mPed->ID, target->ID, 100.0f, duration, 0, 0);
	}
	void Tasks::FleeFrom(Math::Vector3 position)
	{
		FleeFrom(position, -1);
	}
	void Tasks::FleeFrom(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_COORD, this->mPed->ID, position.X, position.Y, position.Z, 100.0f, duration, 0, 0);
	}
	void Tasks::FollowPointRoute(... array<Math::Vector3> ^points)
	{
		Native::Function::Call(Native::Hash::TASK_FLUSH_ROUTE);

		for each (Math::Vector3 point in points)
		{
			Native::Function::Call(Native::Hash::TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
		}

		Native::Function::Call(Native::Hash::TASK_FOLLOW_POINT_ROUTE, this->mPed->ID, 1.0f, 0);
	}
	void Tasks::GoTo(Entity ^target)
	{
		GoTo(target, Math::Vector3::Zero, -1);
	}
	void Tasks::GoTo(Entity ^target, Math::Vector3 offset)
	{
		GoTo(target, offset, -1);
	}
	void Tasks::GoTo(Entity ^target, Math::Vector3 offset, int timeout)
	{
		Native::Function::Call(Native::Hash::TASK_GOTO_ENTITY_OFFSET_XY, this->mPed->ID, target->ID, timeout, offset.X, offset.Y, offset.Z, 2.0f, true);
	}
	void Tasks::GoTo(Math::Vector3 position)
	{
		GoTo(position, false, -1);
	}
	void Tasks::GoTo(Math::Vector3 position, bool ignorePaths)
	{
		GoTo(position, ignorePaths, -1);
	}
	void Tasks::GoTo(Math::Vector3 position, bool ignorePaths, int timeout)
	{
		if (ignorePaths)
		{
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, this->mPed->ID, position.X, position.Y, position.Z, 2.0f, timeout, 0.0f /* heading */, 0.0f /* Radius */);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, this->mPed->ID, position.X, position.Y, position.Z, 2.0f, timeout, 0.0f, 0, 0.0f);
		}
	}
	void Tasks::GuardCurrentPosition()
	{
		Native::Function::Call(Native::Hash::TASK_GUARD_CURRENT_POSITION, this->mPed->ID, 15.0f, 10.0f, true);
	}
	void Tasks::HandsUp(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_HANDS_UP, this->mPed->ID, duration, 0, -1, false);
	}
	void Tasks::LeaveVehicle()
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_ANY_VEHICLE, this->mPed->ID, 0, 0 /* flags */);
	}
	void Tasks::LeaveVehicle(Vehicle ^vehicle, bool closeDoor)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, this->mPed->ID, vehicle->ID, closeDoor ? 0 : 1 << 8);
	}
	void Tasks::LookAt(Entity ^target)
	{
		LookAt(target, -1);
	}
	void Tasks::LookAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_ENTITY, this->mPed->ID, target->ID, duration, 0 /* flags */, 2);
	}
	void Tasks::LookAt(Math::Vector3 position)
	{
		LookAt(position, -1);
	}
	void Tasks::LookAt(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_COORD, this->mPed->ID, position.X, position.Y, position.Z, duration, 0 /* flags */, 2);
	}
	void Tasks::ParachuteTo(Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE_TO_TARGET, this->mPed->ID, position.X, position.Y, position.Z);
	}
	void Tasks::ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_PARK, this->mPed->ID, vehicle->ID, position.X, position.Y, position.Z, heading, 1, 0.0f, false);
	}
	void Tasks::PutAwayMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, this->mPed->ID, false);
	}
	void Tasks::PutAwayParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, this->mPed->ID, false);
	}
	void Tasks::ReloadWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_RELOAD_WEAPON, this->mPed->ID, true);
	}
	void Tasks::RunTo(Math::Vector3 position)
	{
		RunTo(position, false, -1);
	}
	void Tasks::RunTo(Math::Vector3 position, bool ignorePaths)
	{
		RunTo(position, ignorePaths, -1);
	}
	void Tasks::RunTo(Math::Vector3 position, bool ignorePaths, int timeout)
	{
		if (ignorePaths)
		{
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, this->mPed->ID, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f /* heading */, 0.0f);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, this->mPed->ID, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f, 0, 0.0f);
		}
	}
	void Tasks::ShootAt(Ped ^target)
	{
		ShootAt(target, -1);
	}
	void Tasks::ShootAt(Ped ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_ENTITY, this->mPed->ID, target->ID, duration, 0);
	}
	void Tasks::ShootAt(Math::Vector3 position)
	{
		ShootAt(position, -1);
	}
	void Tasks::ShootAt(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_COORD, this->mPed->ID, position.X, position.Y, position.Z, duration, 0);
	}
	void Tasks::ShuffleToNextVehicleSeat(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, this->mPed->ID, vehicle->ID);
	}
	void Tasks::StandStill(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_STAND_STILL, this->mPed->ID, duration);
	}
	void Tasks::SwapWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_SWAP_WEAPON, this->mPed->ID, false);
	}
	void Tasks::StartScenario(System::String ^name, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_START_SCENARIO_AT_POSITION, this->mPed->ID, name, position.X, position.Y, position.Z, 0.0f, 0, 0, 1);
	}
	void Tasks::TurnTo(Entity ^target)
	{
		TurnTo(target, -1);
	}
	void Tasks::TurnTo(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_ENTITY, this->mPed->ID, target->ID, duration);
	}
	void Tasks::TurnTo(Math::Vector3 position)
	{
		TurnTo(position, -1);
	}
	void Tasks::TurnTo(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_COORD, this->mPed->ID, position.X, position.Y, position.Z, duration);
	}
	void Tasks::UseMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, this->mPed->ID, true);
	}
	void Tasks::UseMobilePhone(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE_TIMED, this->mPed->ID, duration);
	}
	void Tasks::UseParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, this->mPed->ID, true);
	}
	void Tasks::Wait(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_PAUSE, this->mPed->ID, duration);
	}
	void Tasks::WanderAround()
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_STANDARD, this->mPed->ID, 0, 0);
	}
	void Tasks::WanderAround(Math::Vector3 position, float radius)
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_IN_AREA, this->mPed->ID, position.X, position.Y, position.Z, radius, 0.0f, 0.0f);
	}
	void Tasks::WarpIntoVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		Native::Function::Call(Native::Hash::TASK_WARP_PED_INTO_VEHICLE, this->mPed->ID, vehicle->ID, static_cast<int>(seat));
	}
	void Tasks::WarpOutOfVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, this->mPed->ID, vehicle->ID, 16);
	}
	void Tasks::ClearAll()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS, this->mPed->ID);
	}
	void Tasks::ClearAllImmediately()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS_IMMEDIATELY, this->mPed->ID);
	}
	void Tasks::ClearLookAt()
	{
		Native::Function::Call(Native::Hash::TASK_CLEAR_LOOK_AT, this->mPed->ID);
	}
	void Tasks::ClearSecondary()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_SECONDARY_TASK, this->mPed->ID);
	}
}