#include "Tasks.hpp"
#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Native.hpp"
#include "Script.hpp"

namespace GTA
{
	Tasks::Tasks(Ped ^ped) : mPed(ped)
	{
	}

	void Tasks::AimAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_ENTITY, this->mPed->Handle, target->Handle, duration, 0);
	}
	void Tasks::AimAt(Math::Vector3 target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_AIM_GUN_AT_COORD, this->mPed->Handle, target.X, target.Y, target.Z, duration, 0, 0);
	}
	void Tasks::Arrest(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_ARREST_PED, this->mPed->Handle, ped->Handle);
	}
	void Tasks::ChatTo(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_CHAT_TO_PED, this->mPed->Handle, ped->Handle, 16, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
	}
	void Tasks::Climb()
	{
		Native::Function::Call(Native::Hash::TASK_CLIMB, this->mPed->Handle, true);
	}
	void Tasks::Cower(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COWER, this->mPed->Handle, duration);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed)
	{
		CruiseWithVehicle(vehicle, speed, 0);
	}
	void Tasks::CruiseWithVehicle(Vehicle ^vehicle, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_WANDER, this->mPed->Handle, vehicle->Handle, speed, drivingstyle);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed)
	{
		DriveTo(vehicle, position, radius, speed, 0);
	}
	void Tasks::DriveTo(Vehicle ^vehicle, Math::Vector3 position, float radius, float speed, int drivingstyle)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_DRIVE_TO_COORD_LONGRANGE, this->mPed->Handle, vehicle->Handle, position.X, position.Y, position.Z, speed, drivingstyle, radius);
	}
	void Tasks::EnterVehicle()
	{
		EnterVehicle(gcnew Vehicle(0), VehicleSeat::Any, -1);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		EnterVehicle(vehicle, seat, -1);
	}
	void Tasks::EnterVehicle(Vehicle ^vehicle, VehicleSeat seat, int timeout)
	{
		Native::Function::Call(Native::Hash::TASK_ENTER_VEHICLE, this->mPed->Handle, vehicle->Handle, timeout, static_cast<int>(seat), 0.0f, 0, 0);
	}
	void Tasks::EveryoneLeaveVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_EVERYONE_LEAVE_VEHICLE, vehicle->Handle);
	}
	void Tasks::FightAgainst(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED, this->mPed->Handle, target->Handle, 0, 16);
	}
	void Tasks::FightAgainst(Ped ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_PED_TIMED, this->mPed->Handle, target->Handle, duration, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED, this->mPed->Handle, radius, 0);
	}
	void Tasks::FightAgainstHatedTargets(float radius, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_COMBAT_HATED_TARGETS_AROUND_PED_TIMED, this->mPed->Handle, radius, duration, 0);
	}
	void Tasks::FleeFrom(Ped ^ped)
	{
		FleeFrom(ped, -1);
	}
	void Tasks::FleeFrom(Ped ^ped, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_PED, this->mPed->Handle, ped->Handle, 100.0f, duration, 0, 0);
	}
	void Tasks::FleeFrom(Math::Vector3 position)
	{
		FleeFrom(position, -1);
	}
	void Tasks::FleeFrom(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_SMART_FLEE_COORD, this->mPed->Handle, position.X, position.Y, position.Z, 100.0f, duration, 0, 0);
	}
	void Tasks::FollowPointRoute(... array<Math::Vector3> ^points)
	{
		Native::Function::Call(Native::Hash::TASK_FLUSH_ROUTE);

		for each (Math::Vector3 point in points)
		{
			Native::Function::Call(Native::Hash::TASK_EXTEND_ROUTE, point.X, point.Y, point.Z);
		}

		Native::Function::Call(Native::Hash::TASK_FOLLOW_POINT_ROUTE, this->mPed->Handle, 1.0f, 0);
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
		Native::Function::Call(Native::Hash::TASK_GOTO_ENTITY_OFFSET_XY, this->mPed->Handle, target->Handle, timeout, offset.X, offset.Y, offset.Z, 1.0f, true);
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
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, this->mPed->Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f /* heading */, 0.0f);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, this->mPed->Handle, position.X, position.Y, position.Z, 1.0f, timeout, 0.0f, 0, 0.0f);
		}
	}
	void Tasks::GuardCurrentPosition()
	{
		Native::Function::Call(Native::Hash::TASK_GUARD_CURRENT_POSITION, this->mPed->Handle, 15.0f, 10.0f, true);
	}
	void Tasks::HandsUp(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_HANDS_UP, this->mPed->Handle, duration, 0, -1, false);
	}
	void Tasks::Jump()
	{
		Native::Function::Call(Native::Hash::TASK_JUMP, this->mPed->Handle, true);
	}
	void Tasks::LeaveVehicle()
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_ANY_VEHICLE, this->mPed->Handle, 0, 0 /* flags */);
	}
	void Tasks::LeaveVehicle(Vehicle ^vehicle, bool closeDoor)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, this->mPed->Handle, vehicle->Handle, closeDoor ? 0 : 1 << 8);
	}
	void Tasks::LookAt(Entity ^target)
	{
		LookAt(target, -1);
	}
	void Tasks::LookAt(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_ENTITY, this->mPed->Handle, target->Handle, duration, 0 /* flags */, 2);
	}
	void Tasks::LookAt(Math::Vector3 position)
	{
		LookAt(position, -1);
	}
	void Tasks::LookAt(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_LOOK_AT_COORD, this->mPed->Handle, position.X, position.Y, position.Z, duration, 0 /* flags */, 2);
	}
	void Tasks::ParachuteTo(Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE_TO_TARGET, this->mPed->Handle, position.X, position.Y, position.Z);
	}
	void Tasks::ParkVehicle(Vehicle ^vehicle, Math::Vector3 position, float heading)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_PARK, this->mPed->Handle, vehicle->Handle, position.X, position.Y, position.Z, heading, 1, 0.0f, false);
	}
	void Tasks::PerformSequence(TaskSequence ^sequence)
	{
		if (!sequence->IsClosed)
		{
			sequence->Close();
		}

		ClearAll();

		this->mPed->BlockPermanentEvents = true;

		Native::Function::Call(Native::Hash::TASK_PERFORM_SEQUENCE, this->mPed->Handle, sequence->Handle);
	}
	void Tasks::PlayAnimation(System::String ^animSet, System::String ^animName, float speed, int duration, bool lastAnimation, float playbackRate)
	{
		Native::Function::Call(Native::Hash::REQUEST_ANIM_DICT, animSet);

		const System::DateTime endtime = System::DateTime::Now + System::TimeSpan(0, 0, 0, 0, 1000);

		while (!Native::Function::Call<bool>(Native::Hash::HAS_ANIM_DICT_LOADED, animSet))
		{
			Script::Yield();

			if (System::DateTime::Now >= endtime)
			{
				return;
			}
		}

		Native::Function::Call(Native::Hash::TASK_PLAY_ANIM, this->mPed->Handle, animSet, animName, speed, -8.0f, duration, lastAnimation, playbackRate, 0, 0, 0);
	}
	void Tasks::PutAwayMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, this->mPed->Handle, false);
	}
	void Tasks::PutAwayParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, this->mPed->Handle, false);
	}
	void Tasks::ReactAndFlee(Ped ^ped)
	{
		Native::Function::Call(Native::Hash::TASK_REACT_AND_FLEE_PED, this->mPed->Handle, ped->Handle);
	}
	void Tasks::ReloadWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_RELOAD_WEAPON, this->mPed->Handle, true);
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
			Native::Function::Call(Native::Hash::TASK_GO_STRAIGHT_TO_COORD, this->mPed->Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f /* heading */, 0.0f);
		}
		else
		{
			Native::Function::Call(Native::Hash::TASK_FOLLOW_NAV_MESH_TO_COORD, this->mPed->Handle, position.X, position.Y, position.Z, 4.0f, timeout, 0.0f, 0, 0.0f);
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
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_ENTITY, this->mPed->Handle, target->Handle, duration, static_cast<int>(pattern));
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
		Native::Function::Call(Native::Hash::TASK_SHOOT_AT_COORD, this->mPed->Handle, position.X, position.Y, position.Z, duration, static_cast<int>(pattern));
	}
	void Tasks::ShuffleToNextVehicleSeat(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_SHUFFLE_TO_NEXT_VEHICLE_SEAT, this->mPed->Handle, vehicle->Handle);
	}
	void Tasks::Skydive()
	{
		Native::Function::Call(Native::Hash::TASK_SKY_DIVE, this->mPed->Handle);
	}
	void Tasks::SlideTo(Math::Vector3 position, float heading)
	{
		Native::Function::Call(Native::Hash::TASK_PED_SLIDE_TO_COORD, this->mPed->Handle, position.X, position.Y, position.Z, heading, 0.7f);
	}
	void Tasks::StandStill(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_STAND_STILL, this->mPed->Handle, duration);
	}
	void Tasks::StartScenario(System::String ^name, Math::Vector3 position)
	{
		Native::Function::Call(Native::Hash::TASK_START_SCENARIO_AT_POSITION, this->mPed->Handle, name, position.X, position.Y, position.Z, 0.0f, 0, 0, 1);
	}
	void Tasks::SwapWeapon()
	{
		Native::Function::Call(Native::Hash::TASK_SWAP_WEAPON, this->mPed->Handle, false);
	}
	void Tasks::TurnTo(Entity ^target)
	{
		TurnTo(target, -1);
	}
	void Tasks::TurnTo(Entity ^target, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_ENTITY, this->mPed->Handle, target->Handle, duration);
	}
	void Tasks::TurnTo(Math::Vector3 position)
	{
		TurnTo(position, -1);
	}
	void Tasks::TurnTo(Math::Vector3 position, int duration)
	{
		Native::Function::Call(Native::Hash::TASK_TURN_PED_TO_FACE_COORD, this->mPed->Handle, position.X, position.Y, position.Z, duration);
	}
	void Tasks::UseMobilePhone()
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE, this->mPed->Handle, true);
	}
	void Tasks::UseMobilePhone(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_USE_MOBILE_PHONE_TIMED, this->mPed->Handle, duration);
	}
	void Tasks::UseParachute()
	{
		Native::Function::Call(Native::Hash::TASK_PARACHUTE, this->mPed->Handle, true);
	}
	void Tasks::VehicleChase(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_CHASE, this->mPed->Handle, target->Handle);
	}
	void Tasks::VehicleShootAtPed(Ped ^target)
	{
		Native::Function::Call(Native::Hash::TASK_VEHICLE_SHOOT_AT_PED, this->mPed->Handle, target->Handle, 20.0f);
	}
	void Tasks::Wait(int duration)
	{
		Native::Function::Call(Native::Hash::TASK_PAUSE, this->mPed->Handle, duration);
	}
	void Tasks::WanderAround()
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_STANDARD, this->mPed->Handle, 0, 0);
	}
	void Tasks::WanderAround(Math::Vector3 position, float radius)
	{
		Native::Function::Call(Native::Hash::TASK_WANDER_IN_AREA, this->mPed->Handle, position.X, position.Y, position.Z, radius, 0.0f, 0.0f);
	}
	void Tasks::WarpIntoVehicle(Vehicle ^vehicle, VehicleSeat seat)
	{
		Native::Function::Call(Native::Hash::TASK_WARP_PED_INTO_VEHICLE, this->mPed->Handle, vehicle->Handle, static_cast<int>(seat));
	}
	void Tasks::WarpOutOfVehicle(Vehicle ^vehicle)
	{
		Native::Function::Call(Native::Hash::TASK_LEAVE_VEHICLE, this->mPed->Handle, vehicle->Handle, 16);
	}

	void Tasks::ClearAll()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS, this->mPed->Handle);
	}
	void Tasks::ClearAllImmediately()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_TASKS_IMMEDIATELY, this->mPed->Handle);
	}
	void Tasks::ClearLookAt()
	{
		Native::Function::Call(Native::Hash::TASK_CLEAR_LOOK_AT, this->mPed->Handle);
	}
	void Tasks::ClearSecondary()
	{
		Native::Function::Call(Native::Hash::CLEAR_PED_SECONDARY_TASK, this->mPed->Handle);
	}
	void Tasks::ClearAnimation(System::String ^animSet, System::String ^animName)
	{
		Native::Function::Call(Native::Hash::STOP_ANIM_TASK, this->mPed->Handle, animSet, animName, -4.0f);
	}

	TaskSequence::TaskSequence() : mCount(0), mIsClosed(false)
	{
		int handle = 0;
		Native::Function::Call(Native::Hash::OPEN_SEQUENCE_TASK, &handle);

		this->mHandle = handle;

		if (System::Object::ReferenceEquals(sNullPed, nullptr))
		{
			sNullPed = gcnew Ped(0);
		}
	}
	TaskSequence::TaskSequence(int handle) : mHandle(handle), mCount(0), mIsClosed(false)
	{
		if (System::Object::ReferenceEquals(sNullPed, nullptr))
		{
			sNullPed = gcnew Ped(0);
		}
	}
	TaskSequence::~TaskSequence()
	{
		int handle = this->mHandle;
		Native::Function::Call(Native::Hash::CLEAR_SEQUENCE_TASK, &handle);
	}

	int TaskSequence::Handle::get()
	{
		return this->mHandle;
	}
	int TaskSequence::Count::get()
	{
		return this->mCount;
	}
	bool TaskSequence::IsClosed::get()
	{
		return this->mIsClosed;
	}
	Tasks ^TaskSequence::AddTask::get()
	{
		if (this->mIsClosed)
		{
			throw gcnew System::Exception("You can't add tasks to a closed sequence!");
		}

		this->mCount++;

		return this->sNullPed->Task;
	}

	void TaskSequence::SetRepeatAndClose()
	{
		if (this->mIsClosed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::SET_SEQUENCE_TO_REPEAT, this->Handle, true);
		Native::Function::Call(Native::Hash::CLOSE_SEQUENCE_TASK, this->mHandle);

		this->mIsClosed = true;
	}
	void TaskSequence::Close()
	{
		if (this->mIsClosed)
		{
			return;
		}

		Native::Function::Call(Native::Hash::CLOSE_SEQUENCE_TASK, this->mHandle);

		this->mIsClosed = true;
	}
}