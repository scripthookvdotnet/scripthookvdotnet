#pragma once

#include "Vector3.hpp"
#include "ScriptDomain.hpp"

namespace GTA
{
	namespace Native
	{
		private ref class MemoryAccess abstract sealed
		{
		internal:
			static MemoryAccess();

			static System::UInt64(*GetAddressOfEntity)(int handle);
			static System::UInt64(*GetAddressOfPlayer)(int handle);

			static array<int> ^GetVehicleHandles();
			static array<int> ^GetVehicleHandles(int modelhash);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius);
			static array<int> ^GetVehicleHandles(Math::Vector3 position, float radius, int modelhash);
			static array<int> ^GetPedHandles();
			static array<int> ^GetPedHandles(int modelhash);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPedHandles(Math::Vector3 position, float radius, int modelhash);
			static array<int> ^GetPropHandles();
			static array<int> ^GetPropHandles(int modelhash);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius);
			static array<int> ^GetPropHandles(Math::Vector3 position, float radius, int modelhash);
			static array<int> ^GetEntityHandles();
			static array<int> ^GetEntityHandles(Math::Vector3 position, float radius);

			static bool IsModelAPed(int modelHash);

			static int(*AddEntityToPool)(System::UInt64 address);
			static System::UInt64(*GetEntityPos)(System::UInt64 address, float *position);
			static System::UInt64(*GetEntityModel1)(System::UInt64 address), (*GetEntityModel2)(System::UInt64 address);
			static System::UInt64 *EntityPoolAddress, *VehiclePoolAddress, *PedPoolAddress, *ObjectPoolAddress;
			static System::UInt64 SetNmIntAddress, SetNmFloatAddress, SetNmBoolAddress, SetNmStringAddress, SetNmVec3Address, CreateNmMessageFunc, GiveNmMessageFunc;
			static int currentGearOffset, highGearOffset, fuelLevelOffset, currentRPMOffset, accelerationOffset, steeringScaleOffset, steeringAngleOffset, wheelSpeedOffset;

		private:
			static System::UInt64 FindPattern(const char *pattern, const char *mask);
			static System::UInt64 FindCModelInfo(int modelHash);
			static int GetModelInfoClassType(System::UInt64 address);
			static System::UInt64 modelHashTable, modelNum2, modelNum3, modelNum4;
			static int modelNum1;
			static unsigned short modelHashEntries;
		};
	}
}
