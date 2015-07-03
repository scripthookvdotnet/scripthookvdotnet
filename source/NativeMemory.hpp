#pragma once

#include <inttypes.h>

namespace GTA
{
	private struct MemoryPool
	{
		uintptr_t ListAddr;
		char *BoolAdr;
		int MaxCount;
		int ItemSize;
	};
	private ref class MemoryAccess abstract sealed
	{
	public:
		static MemoryAccess();

		static int HandleToIndex(int Handle);

		static uintptr_t GetAddressOfEntity(int Handle);
		static array<int> ^GetEntityHandleList();

	private:
		static uintptr_t FindPattern(const char *pattern, const char *mask);

		static uintptr_t GetAddressOfItemInPool(MemoryPool *PoolAddress, int Handle);
		static array<int> ^GetListOfHandlesInPool(MemoryPool *PoolAddress);

		static const char *EntityPoolOpcodeMask = "xxx????xxxxxxx";
		static const char *EntityPoolOpcodePattern = "\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08";
		static MemoryPool **sAddressEntityPool = nullptr;
	};
}