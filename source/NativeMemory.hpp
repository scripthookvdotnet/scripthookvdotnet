#pragma once

#include "Vector3.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	namespace Native
	{
		private ref class MemoryAccess abstract sealed
		{
		public:
			static MemoryAccess();

			static System::UInt64 GetAddressOfEntity(int handle);
			static System::UInt64 GetAddressOfPlayer(int handle);

			static float GetVehicleRPM(int handle);
			static float GetVehicleAcceleration(int handle);
			static float GetVehicleSteering(int handle);
			static array<int> ^GetVehicleHandles();
			static array<int> ^GetVehicleHandles(int modelhash);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetPedHandles();
			static array<int> ^GetPedHandles(int modelhash);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetPropHandles();
			static array<int> ^GetPropHandles(int modelhash);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius, int modelHash);
			static array<int> ^GetEntityHandles();
			static array<int> ^GetEntityHandles(Math::Vector3 position, float radius);

		internal:
			static System::UInt64 EntityAddressFunc, PlayerAddressFunc;
			static System::UInt64 EntityPoolAddress, VehiclePoolAddress, PedPoolAddress, ObjPoolAddress;
			static System::UInt64 EntityCoordsAddress, EntityModelAddress1, EntityModelAddress2;
			static System::UInt64 AddEntToPoolAddress;

		private:
			static System::UInt64 FindPattern(const char *pattern, const char *mask);
		};
	}
}