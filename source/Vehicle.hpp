#pragma once

#include "Entity.hpp"
#include "Model.hpp"
#include "Vector.hpp"

namespace GTA
{
	public enum class VehicleRoofState
	{
		Closed,
		Opening,
		Opened,
		Closing,
	};

	public ref class Vehicle sealed : public Entity
	{
	public:
		Vehicle(int id);

		property bool HasRoof
		{
			bool get();
		}
		property int PassengerSeats
		{
			int get();
		}
		property System::String ^NumberPlate
		{
			System::String ^get();
			void set(System::String ^value);
		}
		property bool IsConvertible
		{
			bool get();
		}
		property bool IsStolen
		{
			bool get();
			void set(bool value);
		}
		property bool IsDriveable
		{
			bool get();
			void set(bool value);
		}
		property bool IsOnAllWheels
		{
			bool get();
		}
		property float Speed
		{
			float get();
			void set(float value);
		}
		property float DirtLevel
		{
			float get();
			void set(float value);
		}
		property VehicleRoofState RoofState
		{
			VehicleRoofState get();
			void set(VehicleRoofState value);
		}
		property float EngineHealth
		{
			float get();
			void set(float value);
		}
		property float PetrolTankHealth
		{
			float get();
			void set(float value);
		}
		property bool SirenActive
		{
			bool get();
			void set(bool value);
		}

		property bool IsWanted
		{
			void set(bool value);
		}
		property bool EngineRunning
		{
			void set(bool value);
		}
		property bool LightsOn
		{
			void set(bool value);
		}
		property bool BrakeLightsOn
		{
			void set(bool value);
		}
		property bool LeftIndicatorLightOn
		{
			void set(bool value);
		}
		property bool RightIndicatorLightOn
		{
			void set(bool value);
		}
		property bool TaxiLightOn
		{
			bool get();
			void set(bool value);
		}
		property bool SearchLightOn
		{
			bool get();
			void set(bool value);
		}
		property bool InteriorLightOn
		{
			void set(bool value);
		}
		property bool NeedsToBeHotwired
		{
			void set(bool value);
		}
		property bool CanTiresBurst
		{
			void set(bool value);
		}
		property bool CanBeVisiblyDamaged
		{
			void set(bool value);
		}
		property bool PreviouslyOwnedByPlayer
		{
			void set(bool value);
		}

		void Repair();
		void Explode();
		bool SetOnGround();

		static Vehicle ^Create(GTA::Model, Math::Vector3, float, bool, bool);

	private:
		int mID;
	};
}