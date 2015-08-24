#pragma once

#include <inttypes.h>

namespace GTA
{
	private ref class MemoryAccess abstract sealed
	{
	public:
		static MemoryAccess();

		static uintptr_t GetAddressOfEntity(int handle);
		
		static float GetVehicleRPM(int handle);
		static float GetVehicleAcceleration(int handle);
		static float GetVehicleSteering(int handle);
		static array<int> ^GetAllVehicleHandles();
		static array<int> ^GetAllPedHandles();
		static array<int> ^GetAllObjectHandles();
		static array<int> ^GetAllEntityHandles();

	private:
		static uintptr_t FindPattern(const char *pattern, const char *mask);
		static const char *EntityAddressPattern = "\x33\xFF\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x58";
		static const char *EntityAddressMask = "xxx????xxxxx";
		static uintptr_t EntityAddress;
	};
}