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
		uintptr_t ListAddr;
		char* BoolAdr;
		int MaxCount;
		int ItemSize;
	};

	private ref class MemoryAccess sealed
	{
	private:
		static char* EntityPoolOpcodePattern = "\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08";
		static char* EntityPoolOpcodeMask = "xxx????xxxxxxx";

		static Pool** ADDRESS_ENTITYPOOL;

		static uintptr_t FindPattern(char pattern[], char mask[]);
	public:

		static MemoryAccess();

		static GameVersion GetGameVersion();

		static int HandleToIndex(int Handle);

		static uintptr_t GetAddressOfItemInPool(Pool* PoolAddress, int Handle);
		static array<int>^ GetListOfHandlesInPool(Pool* PoolAddress);

		static uintptr_t GetAddressOfEntity(int Handle);

		static array<int>^ GetEntityHandleList();
	};
}


