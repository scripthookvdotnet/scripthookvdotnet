#pragma once

#include <inttypes.h>


namespace GTA
{
	public enum class GameVersion
	{
		UnknownVersion = 0,
		v1_0_335_2_STEAM,
		v1_0_335_2_NOSTEAM,
		v1_0_350_1_STEAM,
		v1_0_350_2_NOSTEAM,
		v1_0_372_2_STEAM,
		v1_0_372_2_NOSTEAM, // is that the right name for a version?
	};

	private struct Pool
	{
		intptr_t ListAddr;
		char* BoolAdr;
		int MaxCount;
		int ItemSize;
	};

	private ref class MemoryAccess sealed
	{
	private:
		static Pool** ADDRESS_ENTITYPOOL;
	public:

		static MemoryAccess();

		static GameVersion GetGameVersion();

		static int HandleToIndex(int Handle);

		static intptr_t GetAddressOfItemInPool(Pool* PoolAddress, int Handle);
		static array<int>^ GetListOfHandlesInPool(Pool* PoolAddress);

		static intptr_t GetAddressOfEntity(int Handle);

		static array<int>^ GetEntityHandleList();
	};
}


