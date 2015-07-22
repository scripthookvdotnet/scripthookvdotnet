#pragma once

#include <inttypes.h>

namespace GTA
{
	private struct MemoryPool
	{
		uintptr_t ListAddress;
		char *BoolAddress;
		int MaxCount;
		int ItemSize;
	};
	private ref class MemoryAccess abstract sealed
	{
	public:
		static MemoryAccess();

		static int HandleToIndex(int handle);

		static uintptr_t GetAddressOfEntity(int handle);
		static array<int> ^GetEntityHandleList();
		
		static float GetVehicleRPM(int handle);
		static float GetVehicleAcceleration(int handle);
		static float GetVehicleSteering(int handle);

	private:
		static uintptr_t FindPattern(const char *pattern, const char *mask);

		static uintptr_t GetAddressOfItemInPool(MemoryPool *PoolAddress, int Handle);
		static array<int> ^GetListOfHandlesInPool(MemoryPool *PoolAddress);

		static const char *EntityPoolOpcodeMask = "xxx????xxxxxxx";
		static const char *EntityPoolOpcodePattern = "\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08";
		static MemoryPool **sAddressEntityPool = nullptr;
	};
}