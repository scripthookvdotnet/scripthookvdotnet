#include "Ped.hpp" // <-- Include before Tasks.hpp to solve "Duplicate managed types have different visibilities" error on VS2015, because reasons.
#include "Tasks.hpp"
#include "Vehicle.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	Tasks::Tasks(Ped ^ped) : _ped(ped)
	{
	}

	void Tasks::AchieveHeading(float heading)
	{
		AchieveHeading(heading, 0);
	}
	void Tasks::AchieveHeading(float heading, int timeout)
	{
		Native::Function::Call(Native::Hash::TASK_ACHIEVE_HEADING, _ped->Handle, heading, timeout);
	}
	void Tasks::AimAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_ENTITY, _ped->Handle, target->Handle, duration, 0);
	}
	void Tasks::AimAt(Math::Vector3 target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_COORD, _ped->Handle, target.X, target.Y, target.Z, duration, 0, 0);
	}
	void Tasks::Arrest(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_ARREST_PED, _ped->Handle, ped->Handle);
	}
	void Tasks::ChatTo(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_CHAT_TO_PED, _ped->Handle, ped->Handle, 16, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
	}
	void Tasks::Climb()
	{
		Native::Function::Call(Native::Hash::TASK_CLIMB, _ped->Handle, true);
	}
	void Tasks::Cower(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COWER, _ped->Handle, duration);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed)
	{
		CruiseWithVehicle(vehicle, speed, 0);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_WANDER, _ped->Handle, vehicle->Handle, speed, drivingstyle);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed)
	{
		DriveTo(vehicle, position, radius, speed, 0);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, _ped->Handle, vehicle->Handle, position.X, position.Y, position.Z, speed, drivingstyle, radius);
	}
	void Tasks::EnterVehicle()
	{
		EnterVehicle(gcnew Vehicle(0), VehicleSeat::Any, -1, 0.0f, 0);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		EnterVehicle(vehicle, seat, -1, 0.0f, 0);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout)
	{
		EnterVehicle(vehicle, seat, timeout, 0.0f, 0);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout, float speed)
	{
		EnterVehicle(vehicle, seat, timeout, speed, 0);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout, float speed, int flag)
	{
		Native::Function::Call(Native::Hash::TASK_ENTER_VEHICLE, _ped->Handle, vehicle->Handle, timeout, static_cast<int>(seat), speed, flag, 0);
	}
	void Tasks::EveryoneLeaveVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_EVERYONE_LEAVE_VEHICLE, vehicle->Handle);
	}
	void Tasks::FightAgainst(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED, _ped->Handle, target->Handle, 0, 16);
	}
	void Tasks::FightAgainst(Ped ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED_TIMED, _ped->Handle, target->Handle, duration, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED, _ped->Handle, radius, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, _ped->Handle, radius, duration, 0);
	}
	void Tasks::FleeFrom(Ped ^ped)
	{
		FleeFrom(ped, -1);
	}
	void Tasks::FleeFrom(Ped ^ped, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_PED, _ped->Handle, ped->Handle, 100.0f, duration, 0, 0);
	}
	void Tasks::FleeFrom(Math::Vector3 position)
	{
		FleeFrom(position, -1);
	}
	void Tasks::FleeFrom(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_COORD, _ped->Handle, position.X, position.Y, position.Z, 100.0f, duration, 0, 0);
	}
	void Tasks::FollowPointRoute(... array<Math::Vector3> ^points)
	{
		Native::Function::Call(Native::Hash::TASK_FLUSH_ROUTE);

		for each (Math::Vector3 point in points)
		{
			Native::Function::Call(Native::Hash::TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
		}

		Native::Function::Call(Native::Hash::TASK_FOLLOW_POINT_ROUTE, _ped->Handle, 1.0f, 0);
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
		Native::Function::Call(Native::Hash::TASK_GOTO_ENTITY_OFFSET_XY, _ped->Handle, target->Handle, timeout, offset.X, offset.Y, offset.Z, 1.0f, true);
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
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, _ped->Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f /* heading */, 0.0f);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, _ped->Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0, 0.0f);
		}
	}
	void Tasks::FollowToOffsetFromEntity(Entity ^target, Math::Vector3 offset, int timeout, float stoppingRange)
	{
		FollowToOffsetFromEntity(target, offset, 1.0f, timeout, stoppingRange, true);
	}
	void Tasks::FollowToOffsetFromEntity(Entity ^target, Math::Vector3 offset, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing)
	{
		Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, _ped->Handle, target->Handle, offset.X, offset.Y, offset.Z, movementSpeed, timeout, stoppingRange, persistFollowing);
	}
	void Tasks::GuardCurrentPosition()
	{
		Native::Function::Call(Native::Hash::TASK_GUARD_CURRENT_POSITION, _ped->Handle, 15.0f, 10.0f, true);
	}
	void Tasks::HandsUp(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_HANDS_UP, _ped->Handle, duration, 0, -1, false);
	}
	void Tasks::Jump()
	{
		Native::Function::Call(Native::Hash::TASK_JUMP, _ped->Handle, true);
	}
	void Tasks::LeaveVehicle()
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_ANY_VEHICLE, _ped->Handle, 0, 0 /* flags */);
	}
	void Tasks::LeaveVehicle(Vehicle ^vehicle, bool closeDoor)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, _ped->Handle, vehicle->Handle, closeDoor ? 0 : 1 << 8);
	}
	void Tasks::LeaveVehicle(LeaveVehicleFlags flags)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_ANY_VEHICLE, _ped->Handle, 0, static_cast<int>(flags));
	}
	void Tasks::LeaveVehicle(Vehicle ^vehicle, LeaveVehicleFlags flags)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, _ped->Handle, vehicle->Handle, static_cast<int>(flags));
	}
	void Tasks::LookAt(Entity ^target)
	{
		LookAt(target, -1);
	}
	void Tasks::LookAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_ENTITY, _ped->Handle, target->Handle, duration, 0 /* flags */, 2);
	}
	void Tasks::LookAt(Math::Vector3 position)
	{
		LookAt(position, -1);
	}
	void Tasks::LookAt(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_COORD, _ped->Handle, position.X, position.Y, position.Z, duration, 0 /* flags */, 2);
	}
	void Tasks::ParachuteTo(Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE_TO_TARGET, _ped->Handle, position.X, position.Y, position.Z);
	}
	void Tasks::ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_PARK, _ped->Handle, vehicle->Handle, position.X, position.Y, position.Z, heading, 1, 20.0f, false);
	}
	void Tasks::ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading, float radius)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_PARK, _ped->Handle, vehicle->Handle, position.X, position.Y, position.Z, heading, 1, radius, false);
	}
	void Tasks::ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading, float radius, bool keepEngineOn)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_PARK, _ped->Handle, vehicle->Handle, position.X, position.Y, position.Z, heading, 1, radius, keepEngineOn);
	}
	void Tasks::PerformSequence(TaskSequence ^sequence)
	{
		if (!sequence->IsClosed)
		{
			sequence->Close();
		}

		ClearAll();

		_ped->BlockPermanentEvents = true;

		Native::Function::Call(Native::Hash::TASK_PERFORM_SEQUENCE, _ped->Handle, sequence->Handle);
	}
	void Tasks::PlayAnimation(System::String ^animDict, System::String ^animName, float speed, int duration, bool loop, float playbackRate)
	{
		PlayAnimation(animDict, animName, speed, -8.0f, duration, loop ? AnimationFlags::Loop : AnimationFlags::None, playbackRate);
	}
	void Tasks::PlayAnimation(System::String ^animDict, System::String ^animName)
	{
		PlayAnimation(animDict, animName, 8.0f, -8.0f, -1, AnimationFlags::None, 0.0f);
	}
	void Tasks::PlayAnimation(System::String ^animDict, System::String ^animName, float blendInSpeed, int duration, AnimationFlags flags)
	{
		PlayAnimation(animDict, animName, blendInSpeed, -8.0f, duration, flags, 0.0f);
	}
	void Tasks::PlayAnimation(System::String ^animDict, System::String ^animName, float blendInSpeed, float blendOutSpeed, int duration, AnimationFlags flags, float playbackRate)
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_DICT, animDict);

		const System::DateTime endtime = System::DateTime::UtcNow + System::TimeSpan(0, 0, 0, 0, 1000);

		while (!Native::Function::Call<bool>(Native::Hash::HAS_ANIM_DICT_LOADED, animDict))
		{
			Script::Yield();

			if (System::DateTime::UtcNow >= endtime)
			{
				return;
			}			
		}

		Native::Function::Call(Native::Hash::TASK_PLAY_ANIM, _ped->Handle, animDict, animName, blendInSpeed, blendOutSpeed, duration, static_cast<int>(flags), playbackRate, 0, 0, 0);
	}
	void Tasks::PutAwayMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, _ped->Handle, false);
	}
	void Tasks::PutAwayParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, _ped->Handle, false);
	}
	void Tasks::ReactAndFlee(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_REACT_AND_FLEE_PED, _ped->Handle, ped->Handle);
	}
	void Tasks::ReloadWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_RELOAD_WEAPON, _ped->Handle, true);
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
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, _ped->Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f /* heading */, 0.0f);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, _ped->Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f, 0, 0.0f);
		}
	}
	void Tasks::ShootAt(Ped ^target)
	{
		ShootAt(target, -1, FiringPattern::Default);
	}
	void Tasks::ShootAt(Ped ^target, int duration)
	{
		ShootAt(target, duration, FiringPattern::Default);
	}
	void Tasks::ShootAt(Ped ^target, int duration, FiringPattern pattern)
	{
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_ENTITY, _ped->Handle, target->Handle, duration, static_cast<int>(pattern));
	}
	void Tasks::ShootAt(Math::Vector3 position)
	{
		ShootAt(position, -1, FiringPattern::Default);
	}
	void Tasks::ShootAt(Math::Vector3 position, int duration)
	{
		ShootAt(position, duration, FiringPattern::Default);
	}
	void Tasks::ShootAt(Math::Vector3 position, int duration, FiringPattern pattern)
	{
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_COORD, _ped->Handle, position.X, position.Y, position.Z, duration, static_cast<int>(pattern));
	}
	void Tasks::ShuffleToNextVehicleSeat(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, _ped->Handle, vehicle->Handle);
	}
	void Tasks::Skydive()
	{
		Native::Function::Call(Native::Hash::TASK_SKY_DIVE, _ped->Handle);
	}
	void Tasks::SlideTo(Math::Vector3 position, float heading)
	{
		Native::Function::Call(Native::Hash::TASK_PED_SLIDE_TO_COORD, _ped->Handle, position.X, position.Y, position.Z, heading, 0.7f);
	}
	void Tasks::StandStill(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_STAND_STILL, _ped->Handle, duration);
	}
	void Tasks::StartScenario(System::String ^name, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_START_SCENARIO_AT_POSITION, _ped->Handle, name, position.X, position.Y, position.Z, 0.0f, 0, 0, 1);
	}
	void Tasks::SwapWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_SWAP_WEAPON, _ped->Handle, false);
	}
	void Tasks::TurnTo(Entity ^target)
	{
		TurnTo(target, -1);
	}
	void Tasks::TurnTo(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_ENTITY, _ped->Handle, target->Handle, duration);
	}
	void Tasks::TurnTo(Math::Vector3 position)
	{
		TurnTo(position, -1);
	}
	void Tasks::TurnTo(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_COORD, _ped->Handle, position.X, position.Y, position.Z, duration);
	}
	void Tasks::UseMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, _ped->Handle, true);
	}
	void Tasks::UseMobilePhone(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE_TIMED, _ped->Handle, duration);
	}
	void Tasks::UseParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, _ped->Handle, true);
	}
	void Tasks::VehicleChase(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_CHASE, _ped->Handle, target->Handle);
	}
	void Tasks::VehicleShootAtPed(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_SHOOT_AT_PED, _ped->Handle, target->Handle, 20.0f);
	}
	void Tasks::Wait(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_PAUSE, _ped->Handle, duration);
	}
	void Tasks::WanderAround()
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_STANDARD, _ped->Handle, 0, 0);
	}
	void Tasks::WanderAround(Math::Vector3 position, float radius)
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_IN_AREA, _ped->Handle, position.X, position.Y, position.Z, radius, 0.0f, 0.0f);
	}
	void Tasks::WarpIntoVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		Native::Function::Call(Native::Hash::TASK_WARP_PED_INTO_VEHICLE, _ped->Handle, vehicle->Handle, static_cast<int>(seat));
	}
	void Tasks::WarpOutOfVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, _ped->Handle, vehicle->Handle, 16);
	}

	void Tasks::ClearAll()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS, _ped->Handle);
	}
	void Tasks::ClearAllImmediately()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS_IMMEDIATELY, _ped->Handle);
	}
	void Tasks::ClearLookAt()
	{
		Native::Function::Call(Native::Hash::TASK_CLEAR_LOOK_AT, _ped->Handle);
	}
	void Tasks::ClearSecondary()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_SECONDARY_TASK, _ped->Handle);
	}
	void Tasks::ClearAnimation(System::String ^animSet, System::String ^animName)
	{
		Native::Function::Call(Native::Hash::STOP_ANIM_TASK, _ped->Handle, animSet, animName, -4.0f);
	}

	TaskSequence::TaskSequence() : _count(0), _isClosed(false)
	{
		int handle = 0;
		Native::Function::Call(Native::Hash::OPEN_SEQUENCE_TASK, &handle);

		_handle = handle;

		if (ReferenceEquals(_nullPed, nullptr))
		{
			_nullPed = gcnew Ped(0);
		}
	}
	TaskSequence::TaskSequence(int handle) : _handle(handle), _count(0), _isClosed(false)
	{
		if (ReferenceEquals(_nullPed, nullptr))
		{
			_nullPed = gcnew Ped(0);
		}
	}
	TaskSequence::~TaskSequence()
	{
		int handle = _handle;
		Native::Function::Call(Native::Hash::CLEAR_SEQUENCE_TASK, &handle);
	}

	int TaskSequence::Handle::get()
	{
		return _handle;
	}
	int TaskSequence::Count::get()
	{
		return _count;
	}
	bool TaskSequence::IsClosed::get()
	{
		return _isClosed;
	}
	Tasks ^TaskSequence::AddTask::get()
	{
		if (_isClosed)
		{
			throw gcnew System::Exception("You can't add tasks to a closed sequence!");
		}

		_count++;

		return _nullPed->Task;
	}

	void TaskSequence::Close()
	{
		Close(false);
	}
	void TaskSequence::Close(bool repeat)
	{
		if (_isClosed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::SET_SEQUENCE_TO_REPEAT, Handle, repeat);
		Native::Function::Call(Native::Hash::CLOSE_SEQUENCE_TASK, Handle);

		_isClosed = true;
	}
}