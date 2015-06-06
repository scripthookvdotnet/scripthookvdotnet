#include "Ped.hpp"
#include "Vehicle.hpp"
#include "Native.hpp"
#include "Game.hpp"

namespace GTA
{
	ref class Random
	{
	public:
		static System::Random ^Instance = gcnew System::Random();
	};

	Vehicle::Vehicle(int handle) : Entity(handle)
	{
	}

	bool Vehicle::HasRoof::get()
	{
		return Native::Function::Call<bool>(Native::Hash::DOES_VEHICLE_HAVE_ROOF, this->Handle);
	}
	int Vehicle::PassengerSeats::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, this->Handle);
	}
	System::String ^Vehicle::DisplayName::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_DISPLAY_NAME_FROM_VEHICLE_MODEL, this->Model.Hash);
	}
	System::String ^Vehicle::FriendlyName::get()
	{
		return GTA::Game::GetGXTEntry(DisplayName);
	}
	System::String ^Vehicle::NumberPlate::get()
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_VEHICLE_NUMBER_PLATE_TEXT, this->Handle);
	}
	void Vehicle::NumberPlate::set(System::String ^value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NUMBER_PLATE_TEXT, this->Handle, value);
	}
	bool Vehicle::IsConvertible::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_A_CONVERTIBLE, this->Handle, 0);
	}
	bool Vehicle::IsStolen::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_STOLEN, this->Handle);
	}
	void Vehicle::IsStolen::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_IS_STOLEN, this->Handle, value);
	}
	bool Vehicle::IsDriveable::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_DRIVEABLE, this->Handle, 0);
	}
	void Vehicle::IsDriveable::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_UNDRIVEABLE, this->Handle, !value);
	}
	bool Vehicle::IsOnAllWheels::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_ON_ALL_WHEELS, this->Handle);
	}
	float Vehicle::Speed::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_ENTITY_SPEED, this->Handle);
	}
	void Vehicle::Speed::set(float value)
	{
		if (this->Model.IsTrain)
		{
			Native::Function::Call(Native::Hash::SET_TRAIN_SPEED, this->Handle, value);
			Native::Function::Call(Native::Hash::SET_TRAIN_CRUISE_SPEED, this->Handle, value);
		}
		else
		{
			Native::Function::Call(Native::Hash::SET_VEHICLE_FORWARD_SPEED, this->Handle, value);
		}
	}
	float Vehicle::DirtLevel::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_DIRT_LEVEL, this->Handle);
	}
	void Vehicle::DirtLevel::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DIRT_LEVEL, this->Handle, value);
	}
	VehicleRoofState Vehicle::RoofState::get()
	{
		return static_cast<VehicleRoofState>(Native::Function::Call<int>(Native::Hash::GET_CONVERTIBLE_ROOF_STATE, this->Handle));
	}
	void Vehicle::RoofState::set(VehicleRoofState value)
	{
		switch (value)
		{
		case VehicleRoofState::Closed:
		case VehicleRoofState::Closing:
			Native::Function::Call(Native::Hash::RAISE_CONVERTIBLE_ROOF, this->Handle, 0);
			break;
		case VehicleRoofState::Opened:
		case VehicleRoofState::Opening:
			Native::Function::Call(Native::Hash::LOWER_CONVERTIBLE_ROOF, this->Handle, 0);
			break;
		}
	}
	float Vehicle::EngineHealth::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_ENGINE_HEALTH, this->Handle);
	}
	void Vehicle::EngineHealth::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_ENGINE_HEALTH, this->Handle, value);
	}
	float Vehicle::PetrolTankHealth::get()
	{
		return Native::Function::Call<float>(Native::Hash::GET_VEHICLE_PETROL_TANK_HEALTH, this->Handle);
	}
	void Vehicle::PetrolTankHealth::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_PETROL_TANK_HEALTH, this->Handle, value);
	}
	bool Vehicle::SirenActive::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SIREN_ON, this->Handle);
	}
	void Vehicle::SirenActive::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_SIREN, this->Handle, value);
	}
	VehicleColor Vehicle::PrimaryColor::get()
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, this->Handle, &color1, &color2);

		return static_cast<VehicleColor>(color1);
	}
	void Vehicle::PrimaryColor::set(VehicleColor value)
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, this->Handle, &color1, &color2);
		Native::Function::Call(Native::Hash::SET_VEHICLE_COLOURS, this->Handle, static_cast<int>(value), color2);
	}
	VehicleColor Vehicle::SecondaryColor::get()
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, this->Handle, &color1, &color2);

		return static_cast<VehicleColor>(color2);
	}
	void Vehicle::SecondaryColor::set(VehicleColor value)
	{
		int color1, color2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_COLOURS, this->Handle, &color1, &color2);
		Native::Function::Call(Native::Hash::SET_VEHICLE_COLOURS, this->Handle, color1, static_cast<int>(value));
	}
	VehicleColor Vehicle::RimColor::get()
	{
		int pearlescentColor, rimColor;
		Native::Function::Call(Native::Hash::GET_VEHICLE_EXTRA_COLOURS, this->Handle, &pearlescentColor, &rimColor);
		return static_cast<VehicleColor>(rimColor);
	}
	void Vehicle::RimColor::set(VehicleColor value)
	{
		Native::Function::Call(
			Native::Hash::SET_VEHICLE_EXTRA_COLOURS, 
			this->Handle, 
			static_cast<int>(this->PearlescentColor),
			static_cast<int>(value)
		);
	}
	VehicleColor Vehicle::PearlescentColor::get()
	{
		int pearlescentColor, rimColor;
		Native::Function::Call(Native::Hash::GET_VEHICLE_EXTRA_COLOURS, this->Handle, &pearlescentColor, &rimColor);
		return static_cast<VehicleColor>(pearlescentColor);
	}
	void Vehicle::PearlescentColor::set(VehicleColor value)
	{
		Native::Function::Call(
			Native::Hash::SET_VEHICLE_EXTRA_COLOURS, 
			this->Handle, 
			static_cast<int>(value), 
			static_cast<int>(this->RimColor)
		);
	}
	VehicleWheelType Vehicle::WheelType::get()
	{
		return static_cast<VehicleWheelType>(Native::Function::Call<int>(Native::Hash::GET_VEHICLE_WHEEL_TYPE, this->Handle));
	}
	void Vehicle::WheelType::set(VehicleWheelType wheelType)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_WHEEL_TYPE, this->Handle, static_cast<int>(wheelType));
	}
	VehicleWindowTint Vehicle::WindowTint::get()
	{
		return static_cast<VehicleWindowTint>(Native::Function::Call<int>(Native::Hash::GET_VEHICLE_WINDOW_TINT, this->Handle));
	}
	void Vehicle::WindowTint::set(VehicleWindowTint windowTint)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_WINDOW_TINT, this->Handle, static_cast<int>(windowTint));
	}
	bool Vehicle::IsPrimaryColorCustom::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_DOES_VEHICLE_HAVE_SECONDARY_COLOUR, this->Handle);
	}
	bool Vehicle::IsSecondaryColorCustom::get()
	{
		return Native::Function::Call<bool>(Native::Hash::_0x910A32E7AAD2656C, this->Handle);
	}

	void Vehicle::IsWanted::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_IS_WANTED, this->Handle, value);
	}
	void Vehicle::EngineRunning::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_ENGINE_ON, this->Handle, value, true);
	}
	void Vehicle::LightsOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_LIGHTS, this->Handle, value ? 3 : 4);
	}
	bool Vehicle::LightsOn::get()
	{
		int lightState1, lightState2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_LIGHTS_STATE, this->Handle, &lightState1, &lightState2);
		return lightState1 == 1;
	}
	bool Vehicle::HighBeamsOn::get()
	{
		int lightState1, lightState2;
		Native::Function::Call(Native::Hash::GET_VEHICLE_LIGHTS_STATE, this->Handle, &lightState1, &lightState2);
		return lightState2 == 1;
	}
	void Vehicle::LightsMultiplier::set(float value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_LIGHT_MULTIPLIER, this->Handle, value);
	}
	void Vehicle::BrakeLightsOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_BRAKE_LIGHTS, this->Handle, value);
	}
	void Vehicle::HandbrakeOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_HANDBRAKE, this->Handle, value);
	}
	void Vehicle::LeftIndicatorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INDICATOR_LIGHTS, this->Handle, true, value);
	}
	void Vehicle::RightIndicatorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INDICATOR_LIGHTS, this->Handle, false, value);
	}
	bool Vehicle::TaxiLightOn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_TAXI_LIGHT_ON, this->Handle);
	}
	void Vehicle::TaxiLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_TAXI_LIGHTS, this->Handle, value);
	}
	bool Vehicle::SearchLightOn::get()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SEARCHLIGHT_ON, this->Handle);
	}
	void Vehicle::SearchLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_SEARCHLIGHT, this->Handle, value, 0);
	}
	void Vehicle::InteriorLightOn::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_INTERIORLIGHT, this->Handle, value);
	}
	void Vehicle::NeedsToBeHotwired::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NEEDS_TO_BE_HOTWIRED, this->Handle, value);
	}
	bool Vehicle::CanTiresBurst::get()
	{
		return Native::Function::Call<bool>(Native::Hash::GET_VEHICLE_TYRES_CAN_BURST, this->Handle);
	}
	void Vehicle::CanTiresBurst::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRES_CAN_BURST, this->Handle, value);
	}
	void Vehicle::CanBeVisiblyDamaged::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CAN_BE_VISIBLY_DAMAGED, this->Handle, value);
	}
	void Vehicle::PreviouslyOwnedByPlayer::set(bool value)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_HAS_BEEN_OWNED_BY_PLAYER, this->Handle, value);
	}
	void Vehicle::CustomPrimaryColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CUSTOM_PRIMARY_COLOUR, this->Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::CustomPrimaryColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_CUSTOM_PRIMARY_COLOUR, this->Handle, &r, &g, &b);
		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::CustomSecondaryColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_CUSTOM_SECONDARY_COLOUR, this->Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::CustomSecondaryColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_CUSTOM_SECONDARY_COLOUR, this->Handle, &r, &g, &b);
		return System::Drawing::Color::FromArgb(r, g, b);
	}
	System::Drawing::Color Vehicle::NeonLightsColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_NEON_LIGHTS_COLOUR, this->Handle, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::NeonLightsColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NEON_LIGHTS_COLOUR, this->Handle, color.R, color.G, color.B);
	}
	System::Drawing::Color Vehicle::TireSmokeColor::get()
	{
		int r, g, b;
		Native::Function::Call(Native::Hash::GET_VEHICLE_TYRE_SMOKE_COLOR, this->Handle, &r, &g, &b);

		return System::Drawing::Color::FromArgb(r, g, b);
	}
	void Vehicle::TireSmokeColor::set(System::Drawing::Color color)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_SMOKE_COLOR, this->Handle, color.R, color.G, color.B);
	}
	int Vehicle::Livery::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_LIVERY, this->Handle);
	}
	void Vehicle::Livery::set(int liveryIndex)
	{
		return Native::Function::Call(Native::Hash::SET_VEHICLE_LIVERY, this->Handle, liveryIndex);
	}
	int Vehicle::LiveryCount::get()
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_LIVERY_COUNT, this->Handle);
	}

	int Vehicle::GetMod(VehicleMod modType)
	{
		return Native::Function::Call<int>(Native::Hash::GET_VEHICLE_MOD, this->Handle, static_cast<int>(modType));
	}
	void Vehicle::SetMod(VehicleMod modType, int modIndex, bool variations)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_MOD, this->Handle, static_cast<int>(modType), modIndex, variations);
	}
	void Vehicle::ToggleMod(VehicleToggleMod toggleMod, bool toggle)
	{
		Native::Function::Call(Native::Hash::TOGGLE_VEHICLE_MOD, this->Handle, static_cast<int>(toggleMod), toggle);
	}
	bool Vehicle::IsToggleModOn(VehicleToggleMod toggleMod)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_TOGGLE_MOD_ON, this->Handle, static_cast<int>(toggleMod));
	}
	System::String ^Vehicle::GetModTypeName(VehicleMod modType)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_SLOT_NAME, this->Handle, static_cast<int>(modType));
	}
	System::String ^Vehicle::GetToggleModTypeName(VehicleToggleMod toggleModType)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_SLOT_NAME, this->Handle, static_cast<int>(toggleModType));
	}
	System::String ^Vehicle::GetModName(VehicleMod modType, int modValue)
	{
		return Native::Function::Call<System::String ^>(Native::Hash::GET_MOD_TEXT_LABEL, this->Handle, static_cast<int>(modType), modValue);
	}
	void Vehicle::ClearCustomPrimaryColor()
	{
		Native::Function::Call(Native::Hash::CLEAR_VEHICLE_CUSTOM_PRIMARY_COLOUR, this->Handle);
	}
	void Vehicle::ClearCustomSecondaryColor()
	{
		Native::Function::Call(Native::Hash::CLEAR_VEHICLE_CUSTOM_SECONDARY_COLOUR, this->Handle);
	}
	Ped ^Vehicle::GetPedOnSeat(VehicleSeat seat)
	{
		const int handle = Native::Function::Call<int>(Native::Hash::GET_PED_IN_VEHICLE_SEAT, this->Handle, static_cast<int>(seat));

		if (handle == 0)
		{
			return nullptr;
		}

		return gcnew Ped(handle);
	}
	bool Vehicle::IsSeatFree(VehicleSeat seat)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_SEAT_FREE, this->Handle, static_cast<int>(seat));
	}

	void Vehicle::Repair()
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_FIXED, this->Handle);
	}
	void Vehicle::Explode()
	{
		Native::Function::Call(Native::Hash::EXPLODE_VEHICLE, this->Handle, true, false);
	}
	bool Vehicle::PlaceOnGround()
	{
		return Native::Function::Call<bool>(Native::Hash::SET_VEHICLE_ON_GROUND_PROPERLY, this->Handle);
	}
	void Vehicle::PlaceOnNextStreet()
	{
		const Math::Vector3 pos = this->Position;
		Native::OutputArgument ^outPos = gcnew Native::OutputArgument();

		for (int i = 1; i < 40; i++)
		{
			Native::Function::Call(Native::Hash::GET_NTH_CLOSEST_VEHICLE_NODE, pos.X, pos.Y, pos.Z, i, outPos, 1, 0x40400000, 0);
			const Math::Vector3 newPos = outPos->GetResult<Math::Vector3>();

			if (!Native::Function::Call<bool>(Native::Hash::IS_POINT_OBSCURED_BY_A_MISSION_ENTITY, newPos.X, newPos.Y, newPos.Z, 5.0f, 5.0f, 5.0f, 0))
			{
				this->Position = newPos;
				this->PlaceOnGround();
				break;
			}
		}
	}
	void Vehicle::OpenDoor(VehicleDoor door, bool loose, bool instantly)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DOOR_OPEN, this->Handle, static_cast<int>(door), loose, instantly);
	}
	void Vehicle::CloseDoor(VehicleDoor door, bool instantly)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_DOOR_SHUT, this->Handle, static_cast<int>(door), instantly);
	}
	void Vehicle::FixWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::FIX_VEHICLE_WINDOW, this->Handle, static_cast<int>(window));
	}
	void Vehicle::SmashWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::SMASH_VEHICLE_WINDOW, this->Handle, static_cast<int>(window));
	}
	void Vehicle::RollUpWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::ROLL_UP_WINDOW, this->Handle, static_cast<int>(window));
	}
	void Vehicle::RollDownWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::ROLL_DOWN_WINDOW, this->Handle, static_cast<int>(window));
	}
	void Vehicle::RollDownWindows()
	{
		Native::Function::Call(Native::Hash::ROLL_DOWN_WINDOWS, this->Handle);
	}
	void Vehicle::RemoveWindow(VehicleWindow window)
	{
		Native::Function::Call(Native::Hash::REMOVE_VEHICLE_WINDOW, this->Handle, static_cast<int>(window));
	}
	void Vehicle::SetNeonLightsOn(VehicleNeonLight light, bool on)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_NEON_LIGHTS_ON, this->Handle, static_cast<int>(light), on);
	}
	bool Vehicle::IsNeonLightsOn(VehicleNeonLight light)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_NEON_LIGHT_ON, this->Handle, static_cast<int>(light));
	}

	void Vehicle::SoundHorn(int duration)
	{
		int heldDownHash = Native::Function::Call<int>(Native::Hash::GET_HASH_KEY, "HELDDOWN");
		Native::Function::Call(Native::Hash::START_VEHICLE_HORN, this->ID, duration, heldDownHash, 0);
	}

	bool Vehicle::IsTireBurst(int wheel)
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_TYRE_BURST, this->Handle, wheel, false);
	}
	void Vehicle::BurstTire(int wheel)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_BURST, this->Handle, wheel, 1, 1000.0f);
	}
	void Vehicle::FixTire(int wheel)
	{
		Native::Function::Call(Native::Hash::SET_VEHICLE_TYRE_FIXED, this->Handle, wheel);
	}
	bool Vehicle::IsInBurnout()
	{
		return Native::Function::Call<bool>(Native::Hash::IS_VEHICLE_IN_BURNOUT, this->Handle);
	}

	Ped ^Vehicle::CreatePedOnSeat(VehicleSeat seat, GTA::Model model)
	{
		if (!model.IsPed || !model.Request(1000))
		{
			return nullptr;
		}
		int pedHandle = Native::Function::Call<int>(Native::Hash::CREATE_PED_INSIDE_VEHICLE, this->Handle, 26, model.Hash, static_cast<int>(seat), 1, 1);
		if (pedHandle == 0)
		{
			return nullptr;
		}
		return gcnew Ped(pedHandle);
	}
	Ped ^Vehicle::CreateRandomPedOnSeat(VehicleSeat seat)
	{
		System::Array ^modelValues = System::Enum::GetValues(Native::PedHash::typeid);
		GTA::Model model = *gcnew GTA::Model(static_cast<Native::PedHash>(modelValues->GetValue(Random::Instance->Next(modelValues->Length))));
		return this->CreatePedOnSeat(seat, model);
	}
}
