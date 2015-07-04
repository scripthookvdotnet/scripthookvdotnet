#include "NativeMemory.hpp"

#include <Main.h>
#include <Psapi.h>

namespace GTA
{
	using namespace System::Collections::Generic;

	static MemoryAccess::MemoryAccess()
	{
		const uintptr_t patternAddress = FindPattern(EntityPoolOpcodePattern, EntityPoolOpcodeMask);

		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		sAddressEntityPool = reinterpret_cast<MemoryPool **>(*reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7);
	}

	int MemoryAccess::HandleToIndex(int Handle)
	{
		return Handle >> 8; // == Handle / 256
	}

	uintptr_t MemoryAccess::GetAddressOfEntity(int Handle)
	{
		return *reinterpret_cast<uintptr_t*>(GetAddressOfItemInPool(*sAddressEntityPool, Handle) + 8);
	}
	array<int> ^MemoryAccess::GetEntityHandleList()
	{
		return GetListOfHandlesInPool(*sAddressEntityPool);
	}
	float MemoryAccess::GetVehicleRPM(int handle)
	{
		uintptr_t addr = GetAddressOfEntity(handle);
		if (addr == 0)
		{
			return 0.0f;
		}
		return *(float*)(addr + 2004);
	}

	uintptr_t MemoryAccess::FindPattern(const char *pattern, const char *mask)
	{
		MODULEINFO modInfo = { 0 };
		GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &modInfo, sizeof(MODULEINFO));

		const char *start_offset = reinterpret_cast<const char *>(modInfo.lpBaseOfDll);
		const uintptr_t size = static_cast<uintptr_t>(modInfo.SizeOfImage);

		intptr_t pos = 0;
		const uintptr_t searchLen = static_cast<uintptr_t>(strlen(mask) - 1);

		for (const char *retAddress = start_offset; retAddress < start_offset + size; retAddress++)
		{
			if (*retAddress == pattern[pos] || mask[pos] == '?')
			{
				if (mask[pos + 1] == '\0')
				{
					return (reinterpret_cast<uintptr_t>(retAddress) - searchLen);
				}

				pos++;
			}
			else
			{
				pos = 0;
			}
		}

		return 0;
	}

	uintptr_t MemoryAccess::GetAddressOfItemInPool(MemoryPool *PoolAddress, int Handle)
	{
		if (PoolAddress == nullptr)
		{
			return 0;
		}

		const int index = HandleToIndex(Handle);
		const int flag = PoolAddress->BoolAdr[index]; // flag should be equal to 2 if everything is ok

		// parity check? (taken from ScriptHookDotNet for IV
		if (flag & 0x80 || flag != (Handle & 0xFF))
		{
			return 0;
		}

		return (PoolAddress->ListAddr + index * PoolAddress->ItemSize);
	}
	array<int> ^MemoryAccess::GetListOfHandlesInPool(MemoryPool *PoolAddress)
	{
		if (PoolAddress == nullptr)
		{
			return nullptr;
		}

		List<int> ^handles = gcnew List<int>();

		for (int i = 0; i < PoolAddress->MaxCount; i++)
		{
			int val = static_cast<int>(PoolAddress->BoolAdr[i]); // parity value, important to cast it to int

			if ((val & 0x80) == 0)
			{
				val = (i << 8) | val;
				handles->Add(val);
			}
		}

		return handles->ToArray();
	}
}