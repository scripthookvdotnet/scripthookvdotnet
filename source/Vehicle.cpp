#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Game.hpp"
#include "Native.hpp"
#include "NativeMemory.hpp"

namespace GTA
{
	Vehicle::Vehicle(int handle) : Entity(handle)
	{
	}

	bool Vehicle::HasRoof::get()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_VEHICLE_HAVE_ROOF, Handle);
	}
	int Vehicle::PassengerSeats::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, Handle);
	}
	System::String ^Vehicle::DisplayName::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, Model.Hash);
	}
	System::String ^Vehicle::FriendlyName::get()
	{
		return GTA::Game::GetGXTEntry(DisplayName);
	}
	System::String ^Vehicle::NumberPlate::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_VEHICLE_NUMBER_PLATE_TEXT, Handle);
	}
	void Vehicle::NumberPlate::set(System::String ^value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NUMBER_PLATE_TEXT, Handle, value);
	}
	bool Vehicle::IsConvertible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_A_CONVERTIBLE, Handle, 0);
	}
	bool Vehicle::IsStolen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_STOLEN, Handle);
	}
	void Vehicle::IsStolen::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_IS_STOLEN, Handle, value);
	}
	bool Vehicle::IsDriveable::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_DRIVEABLE, Handle, 0);
	}
	void Vehicle::IsDriveable::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_UNDRIVEABLE, Handle, !value);
	}
	bool Vehicle::IsStopped::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_STOPPED, Handle);
	}
	bool Vehicle::IsOnAllWheels::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_ON_ALL_WHEELS, Handle);
	}
	float Vehicle::Speed::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_SPEED, Handle);
	}
	void Vehicle::Speed::set(float value)
	{
		if (Model.IsTrain)
		{
			Native::Function::Call(Native::Hash::SET_TRAIN_SPEED, Handle, value);
			Native::Function::Call(Native::Hash::SET_TRAIN_CRUISE_SPEED, Handle, value);
		}
		else
		{
			Native::Function::Call(Native::Hash::SET_VEHICLE_FORWARD_SPEED, Handle, value);
		}
	}
	float Vehicle::DirtLevel::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_DIRT_LEVEL, Handle);
	}
	void Vehicle::DirtLevel::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DIRT_LEVEL, Handle, value);
	}
	VehicleRoofState Vehicle::RoofState::get()
	{
		return static_cast<VehicleRoofState>(Native::Function::Call<int>(Native::Hash::GET_CONVERTIBLE_ROOF_STATE, Handle));
	}
	void Vehicle::RoofState::set(VehicleRoofState value)
	{
		switch (value)
		{
		case VehicleRoofState::Closed:
		case VehicleRoofState::Closing:
			Native::Function::Call(Native::Hash::RAISE_CONVERTIBLE_ROOF, Handle, 0);
			break;
		case VehicleRoofState::Opened:
		case VehicleRoofState::Opening:
			Native::Function::Call(Native::Hash::LOWER_CONVERTIBLE_ROOF, Handle, 0);
			break;
		}
	}
	float Vehicle::BodyHealth::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_BODY_HEALTH, Handle);
	}
	void Vehicle::BodyHealth::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_BODY_HEALTH, Handle, value);
	}
	float Vehicle::EngineHealth::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_ENGINE_HEALTH, Handle);
	}
	void Vehicle::EngineHealth::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_ENGINE_HEALTH, Handle, value);
	}
	float Vehicle::PetrolTankHealth::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_PETROL_TANK_HEALTH, Handle);
	}
	void Vehicle::PetrolTankHealth::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_PETROL_TANK_HEALTH, Handle, value);
	}
	bool Vehicle::SirenActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SIREN_ON, Handle);
	}
	void Vehicle::SirenActive::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_SIREN, Handle, value);
	}
	VehicleColor Vehicle::PrimaryColor::get()
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, Handle, &color1, &color2);

		return static_cast<VehicleColor>(color1);
	}
	void Vehicle::PrimaryColor::set(VehicleColor value)
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, Handle, &color1, &color2);
		Native::Function::Call(Native::Hash::SET_VEHICLE_COLOURS, Handle, static_cast<int>(value), color2);
	}
	VehicleColor Vehicle::SecondaryColor::get()
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, Handle, &color1, &color2);

		return static_cast<VehicleColor>(color2);
	}
	void Vehicle::SecondaryColor::set(VehicleColor value)
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, Handle, &color1, &color2);
		Native::Function::Call(Native::Hash::SET_VEHICLE_COLOURS, Handle, color1, static_cast<int>(value));
	}
	VehicleColor Vehicle::RimColor::get()
	{
		int pearlescentColor, rimColor;
		Native::Function::Call(Native::Hash::GET_VEHICLE_EXTRA_COLOURS, Handle, &pearlescentColor, &rimColor);
		return static_cast<VehicleColor>(rimColor);
	}
	void Vehicle::RimColor::set(VehicleColor value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_EXTRA_COLOURS, Handle, static_cast<int>(PearlescentColor), static_cast<int>(value));
	}
	VehicleColor Vehicle::PearlescentColor::get()
	{
		int pearlescentColor, rimColor;
		Native::Function::Call(Native::Hash::GET_VEHICLE_EXTRA_COLOURS, Handle, &pearlescentColor, &rimColor);
		return static_cast<VehicleColor>(pearlescentColor);
	}
	void Vehicle::PearlescentColor::set(VehicleColor value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_EXTRA_COLOURS, Handle, static_cast<int>(value), static_cast<int>(RimColor));
	}
	VehicleWheelType Vehicle::WheelType::get()
	{
		return static_cast<VehicleWheelType>(Native::Function::Call<int>(Native::Hash::GET_VEHICLE_WHEEL_TYPE, Handle));
	}
	void Vehicle::WheelType::set(VehicleWheelType wheelType)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_WHEEL_TYPE, Handle, static_cast<int>(wheelType));
	}
	VehicleWindowTint Vehicle::WindowTint::get()
	{
		return static_cast<VehicleWindowTint>(Native::Function::Call<int>(Native::Hash::GET_VEHICLE_WINDOW_TINT, Handle));
	}
	void Vehicle::WindowTint::set(VehicleWindowTint windowTint)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_WINDOW_TINT, Handle, static_cast<int>(windowTint));
	}
	bool Vehicle::IsPrimaryColorCustom::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_IS_VEHICLE_PRIMARY_COLOUR_CUSTOM, Handle);
	}
	bool Vehicle::IsSecondaryColorCustom::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_IS_VEHICLE_SECONDARY_COLOUR_CUSTOM, Handle);
	}

	void Vehicle::IsWanted::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_IS_WANTED, Handle, value);
	}
	bool Vehicle::EngineRunning::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_IS_VEHICLE_ENGINE_ON, Handle);
	}
	void Vehicle::EngineRunning::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_ENGINE_ON, Handle, value, true);
	}
	void Vehicle::EnginePowerMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::_SET_VEHICLE_ENGINE_POWER_MULTIPLIER, Handle, value);
	}
	void Vehicle::EngineTorqueMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::_SET_VEHICLE_ENGINE_TORQUE_MULTIPLIER, Handle, value);
	}
	void Vehicle::EngineCanDegrade::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_ENGINE_CAN_DEGRADE, Handle, value);
	}
	void Vehicle::LightsOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_LIGHTS, Handle, value ? 3 : 4);
	}
	bool Vehicle::LightsOn::get()
	{
		int lightState1, lightState2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_LIGHTS_STATE, Handle, &lightState1, &lightState2);
		return lightState1 == 1;
	}
	bool Vehicle::HighBeamsOn::get()
	{
		int lightState1, lightState2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_LIGHTS_STATE, Handle, &lightState1, &lightState2);
		return lightState2 == 1;
	}
	void Vehicle::LightsMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_LIGHT_MULTIPLIER, Handle, value);
	}
	bool Vehicle::LeftHeadLightBroken::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_IS_HEADLIGHT_L_BROKEN, Handle);
	}
	void Vehicle::LeftHeadLightBroken::set(bool value)
	{
		unsigned char *const address = reinterpret_cast<unsigned char *>(Native::MemoryAccess::GetAddressOfEntity(Handle));

		if (address == nullptr)
		{
			return;
		}

		const unsigned char mask = 1 << 0;

		if (value)
		{
			*(address + 1916) |= mask;
		}
		else
		{
			*(address + 1916) &= ~mask;
		}
	}
	bool Vehicle::RightHeadLightBroken::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_IS_HEADLIGHT_R_BROKEN, Handle);
	}
	void Vehicle::RightHeadLightBroken::set(bool value)
	{
		unsigned char *const address = reinterpret_cast<unsigned char *>(Native::MemoryAccess::GetAddressOfEntity(Handle));

		if (address == nullptr)
		{
			return;
		}

		const unsigned char mask = 1 << 1;

		if (value)
		{
			*(address + 1916) |= mask;
		}
		else
		{
			*(address + 1916) &= ~mask;
		}
	}
	void Vehicle::BrakeLightsOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_BRAKE_LIGHTS, Handle, value);
	}
	void Vehicle::HandbrakeOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_HANDBRAKE, Handle, value);
	}
	void Vehicle::LeftIndicatorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INDICATOR_LIGHTS, Handle, true, value);
	}
	void Vehicle::RightIndicatorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INDICATOR_LIGHTS, Handle, false, value);
	}
	bool Vehicle::TaxiLightOn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_TAXI_LIGHT_ON, Handle);
	}
	void Vehicle::TaxiLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_TAXI_LIGHTS, Handle, value);
	}
	bool Vehicle::SearchLightOn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SEARCHLIGHT_ON, Handle);
	}
	void Vehicle::SearchLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_SEARCHLIGHT, Handle, value, 0);
	}
	void Vehicle::InteriorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INTERIORLIGHT, Handle, value);
	}
	void Vehicle::NeedsToBeHotwired::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, Handle, value);
	}
	bool Vehicle::CanTiresBurst::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_VEHICLE_TYRES_CAN_BURST, Handle);
	}
	void Vehicle::CanTiresBurst::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRES_CAN_BURST, Handle, value);
	}
	void Vehicle::CanBeVisiblyDamaged::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, Handle, value);
	}
	void Vehicle::PreviouslyOwnedByPlayer::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, Handle, value);
	}
	void Vehicle::CustomPrimaryColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::CustomPrimaryColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle, &r, &g, &b);
		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::CustomSecondaryColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::CustomSecondaryColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle, &r, &g, &b);
		return System::Drawing::Color::FromArgb(r, g, b);
	}
	System::Drawing::Color Vehicle::NeonLightsColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::_GET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::NeonLightsColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::_SET_VEHICLE_NEON_LIGHTS_COLOUR, Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::TireSmokeColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_TYRE_SMOKE_COLOR, Handle, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::TireSmokeColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_SMOKE_COLOR, Handle, color.R, color.G, color.B);
	}
	int Vehicle::Livery::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_LIVERY, Handle);
	}
	void Vehicle::Livery::set(int liveryIndex)
	{
		return Native::Function::Call(Native::Hash::SET_VEHICLE_LIVERY, Handle, liveryIndex);
	}
	int Vehicle::LiveryCount::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_LIVERY_COUNT, Handle);
	}
	void Vehicle::HasAlarm::set(bool value)
	{
		return Native::Function::Call(Native::Hash::SET_VEHICLE_ALARM, Handle, value);
	}
	bool Vehicle::AlarmActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_ALARM_ACTIVATED, Handle);
	}
	float Vehicle::CurrentRPM::get()
	{
		const System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		int offset = (static_cast<int>(Game::Version) > 3 ? 2004 : 1988);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + offset);
	}
	float Vehicle::Acceleration::get()
	{
		const System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2020);
	}
	float Vehicle::Steering::get()
	{
		const System::UInt64 address = Native::MemoryAccess::GetAddressOfEntity(Handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2212);
	}

	int Vehicle::GetMod(VehicleMod modType)
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_MOD, Handle, static_cast<int>(modType));
	}
	void Vehicle::SetMod(VehicleMod modType, int modIndex, bool variations)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_MOD, Handle, static_cast<int>(modType), modIndex, variations);
	}
	void Vehicle::ToggleMod(VehicleToggleMod toggleMod, bool toggle)
	{
		Native::Function::Call(Native::Hash::TOGGLE_VEHICLE_MOD, Handle, static_cast<int>(toggleMod), toggle);
	}
	bool Vehicle::IsToggleModOn(VehicleToggleMod toggleMod)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_TOGGLE_MOD_ON, Handle, static_cast<int>(toggleMod));
	}
	System::String ^Vehicle::GetModTypeName(VehicleMod modType)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_SLOT_NAME, Handle, static_cast<int>(modType));
	}
	System::String ^Vehicle::GetToggleModTypeName(VehicleToggleMod toggleModType)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_SLOT_NAME, Handle, static_cast<int>(toggleModType));
	}
	System::String ^Vehicle::GetModName(VehicleMod modType, int modValue)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_TEXT_LABEL, Handle, static_cast<int>(modType), modValue);
	}
	void Vehicle::ClearCustomPrimaryColor()
	{
		Native::Function::Call(Native::Hash::CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, Handle);
	}
	void Vehicle::ClearCustomSecondaryColor()
	{
		Native::Function::Call(Native::Hash::CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, Handle);
	}
	Ped ^Vehicle::GetPedOnSeat(VehicleSeat seat)
	{
		return Native::Function::Call<Ped ^>(Native::Hash::GET_PED_IN_VEHICLE_SEAT, Handle, static_cast<int>(seat));
	}
	bool Vehicle::IsSeatFree(VehicleSeat seat)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SEAT_FREE, Handle, static_cast<int>(seat));
	}

	void Vehicle::Repair()
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_FIXED, Handle);
	}
	void Vehicle::Explode()
	{
		Native::Function::Call(Native::Hash::EXPLODE_VEHICLE, Handle, true, false);
	}
	bool Vehicle::PlaceOnGround()
	{
		return Native::Function::Call<bool>(Native::Hash::SET_VEHICLE_ON_GROUND_PROPERLY, Handle);
	}
	void Vehicle::PlaceOnNextStreet()
	{
		const Math::Vector3 pos = Position;
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		for (int i = 1; i < 40; i++)
		{
			float heading;
			float val;
			Native::Function::Call(Native::Hash::GET_NTH_CLOSEST_VEHICLE_NODE_WITH_HEADING, pos.X, pos.Y, pos.Z, i, outPos, &heading, &val, 1, 0x40400000, 0);
			const Math::Vector3 newPos = outPos->GetResult<Math::Vector3>();

			if (!Native::Function::Call<bool>(Native::Hash::IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
			{
				Position = newPos;
				PlaceOnGround();
				Heading = heading;
				break;
			}
		}
	}
	void Vehicle::OpenDoor(VehicleDoor door, bool loose, bool instantly)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DOOR_OPEN, Handle, static_cast<int>(door), loose, instantly);
	}
	void Vehicle::CloseDoor(VehicleDoor door, bool instantly)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DOOR_SHUT, Handle, static_cast<int>(door), instantly);
	}
	void Vehicle::BreakDoor(VehicleDoor door)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DOOR_BROKEN, Handle, static_cast<int>(door));
	}
	bool Vehicle::IsDoorBroken(VehicleDoor door)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_DOOR_DAMAGED, Handle, static_cast<int>(door));
	}
	void Vehicle::SetDoorBreakable(VehicleDoor door, bool isBreakable)
	{
		Native::Function::Call(Native::Hash::_SET_VEHICLE_DOOR_BREAKABLE, Handle, static_cast<int>(door), isBreakable);
	}
	void Vehicle::FixWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::FIX_VEHICLE_WINDOW, Handle, static_cast<int>(window));
	}
	void Vehicle::SmashWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::SMASH_VEHICLE_WINDOW, Handle, static_cast<int>(window));
	}
	void Vehicle::RollUpWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::ROLL_UP_WINDOW, Handle, static_cast<int>(window));
	}
	void Vehicle::RollDownWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::ROLL_DOWN_WINDOW, Handle, static_cast<int>(window));
	}
	void Vehicle::RollDownWindows()
	{
		Native::Function::Call(Native::Hash::ROLL_DOWN_WINDOWS, Handle);
	}
	void Vehicle::RemoveWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::REMOVE_VEHICLE_WINDOW, Handle, static_cast<int>(window));
	}
	void Vehicle::SetNeonLightsOn(VehicleNeonLight light, bool on)
	{
		Native::Function::Call(Native::Hash::_SET_VEHICLE_NEON_LIGHT_ENABLED, Handle, static_cast<int>(light), on);
	}
	bool Vehicle::IsNeonLightsOn(VehicleNeonLight light)
	{
		return Native::Function::Call<bool>(Native::Hash::_IS_VEHICLE_NEON_LIGHT_ENABLED, Handle, static_cast<int>(light));
	}

	void Vehicle::SoundHorn(int duration)
	{
		int heldDownHash = Game::GenerateHash("HELDDOWN");
		Native::Function::Call(Native::Hash::START_VEHICLE_HORN, Handle, duration, heldDownHash, 0);
	}

	void Vehicle::SetHeliYawPitchRollMult(float mult)
	{

		if (Model.IsHelicopter)
		{
			if (mult >= 0.0f && mult <= 1.0f)
			{
				Native::Function::Call(Native::Hash::_0x6E0859B530A365CC, Handle, mult);
			}
		}
	}
	void Vehicle::DropCargobobHook(CargobobHook hookType)
	{
		if (Model.IsCargobob)
		{
			Native::Function::Call(Native::Hash::_0x7BEB0C7A235F6F3B, Handle, static_cast<int>(hookType));
		}
	}
	bool Vehicle::IsCargobobHookActive()
	{
		if (Model.IsCargobob)
		{
			return Native::Function::Call<bool>(Native::Hash::_0x1821D91AD4B56108, Handle) || Native::Function::Call<bool>(Native::Hash::_0x6E08BF5B3722BAC9, Handle);
		}
		return false;
	}
	bool Vehicle::IsCargobobHookActive(CargobobHook hookType)
	{
		if (Model.IsCargobob)
		{
			switch (hookType)
			{
			case CargobobHook::Hook:
				return Native::Function::Call<bool>(Native::Hash::_0x1821D91AD4B56108, Handle);
			case CargobobHook::Magnet:
				return Native::Function::Call<bool>(Native::Hash::_0x6E08BF5B3722BAC9, Handle);
			}
		}
		return false;
	}
	void Vehicle::RemoveCargobobHook()
	{
		if (Model.IsCargobob)
		{
			Native::Function::Call(Native::Hash::_0x9768CF648F54C804, Handle);
		}
	}
	void Vehicle::CargoBobMagnetGrabVehicle()
	{
		if (IsCargobobHookActive(CargobobHook::Magnet))
		{
			Native::Function::Call(Native::Hash::_0x9A665550F8DA349B, Handle, true);
		}
	}
	void Vehicle::CargoBobMagnetReleaseVehicle()
	{
		if (IsCargobobHookActive(CargobobHook::Magnet))
		{
			Native::Function::Call(Native::Hash::_0x9A665550F8DA349B, Handle, false);
		}
	}

	bool Vehicle::IsTireBurst(int wheel)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_TYRE_BURST, Handle, wheel, false);
	}
	void Vehicle::BurstTire(int wheel)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_BURST, Handle, wheel, 1, 1000.0f);
	}
	void Vehicle::FixTire(int wheel)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_FIXED, Handle, wheel);
	}
	bool Vehicle::IsInBurnout()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_IN_BURNOUT, Handle);
	}
	void Vehicle::StartAlarm()
	{
		Native::Function::Call(Native::Hash::START_VEHICLE_ALARM, Handle);
	}
	void Vehicle::ApplyDamage(Math::Vector3 loc, float damageAmount, float radius)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DAMAGE, loc.X, loc.Y, loc.Z, damageAmount, radius, true);
	}

	Ped ^Vehicle::CreatePedOnSeat(VehicleSeat seat, GTA::Model model)
	{
		if (!model.IsPed || !model.Request(1000))
		{
			return nullptr;
		}

		return Native::Function::Call<Ped ^>(Native::Hash::CREATE_PED_INSIDE_VEHICLE, Handle, 26, model.Hash, static_cast<int>(seat), 1, 1);
	}
	Ped ^Vehicle::CreateRandomPedOnSeat(VehicleSeat seat)
	{
		if (seat == VehicleSeat::Driver)
		{
			return Native::Function::Call<Ped ^>(Native::Hash::CREATE_RANDOM_PED_AS_DRIVER, Handle, true);
		}
		else
		{
			Ped ^ped = Native::Function::Call<Ped ^>(Native::Hash::CREATE_RANDOM_PED, 0.0f, 0.0f, 0.0f);
			Native::Function::Call(Native::Hash::SET_PED_INTO_VEHICLE, ped->Handle, Handle, static_cast<int>(seat));
			return ped;
		}
	}
}
