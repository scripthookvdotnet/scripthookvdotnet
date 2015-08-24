#include "NativeMemory.hpp"

#include <Main.h>
#include <Psapi.h>

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	static MemoryAccess::MemoryAccess()
	{
		const uintptr_t patternAddress = FindPattern(EntityPoolOpcodePattern, EntityPoolOpcodeMask);

		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		sAddressEntityPool = reinterpret_cast<MemoryPool **>(*reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7);
	}

	int MemoryAccess::HandleToIndex(int handle)
	{
		return handle >> 8; // == handle / 256
	}

	uintptr_t MemoryAccess::GetAddressOfEntity(int handle)
	{
		return *reinterpret_cast<const uintptr_t *>(GetAddressOfItemInPool(*sAddressEntityPool, handle) + 8);
	}
	array<int> ^MemoryAccess::GetEntityHandleList()
	{
		return GetListOfHandlesInPool(*sAddressEntityPool);
	}
	float MemoryAccess::GetVehicleRPM(int handle)
	{
		const uintptr_t address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2004);
	}
	float MemoryAccess::GetVehicleAcceleration(int handle)
	{
		const uintptr_t address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2020);
	}
	float MemoryAccess::GetVehicleSteering(int handle)
	{
		const uintptr_t address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2212);
	}

	uintptr_t MemoryAccess::FindPattern(const char *pattern, const char *mask)
	{
		MODULEINFO modInfo = { 0 };
		GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &modInfo, sizeof(MODULEINFO));

		const char *start_offset = reinterpret_cast<const char *>(modInfo.lpBaseOfDll);
		const size_t size = static_cast<size_t>(modInfo.SizeOfImage);

		intptr_t pos = 0;
		const size_t searchLen = static_cast<size_t>(strlen(mask) - 1);

		for (const char *retAddress = start_offset; retAddress < start_offset + size; retAddress++)
		{
			if (*retAddress == pattern[pos] || mask[pos] == '?')
			{
				if (mask[pos + 1] == '\0')
				{
					return reinterpret_cast<uintptr_t>(retAddress) - searchLen;
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

		const int idx = HandleToIndex(Handle);
		// Flag should be equal to 2 if everything is ok
		const int flag = static_cast<int>(PoolAddress->BoolAddress[idx]);

		// Parity check (taken from ScriptHookDotNet for GTAIV)
		if (flag & 0x80 || flag != (Handle & 0xFF))
		{
			return 0;
		}

		return PoolAddress->ListAddress + idx * PoolAddress->ItemSize;
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
			int val = static_cast<int>(PoolAddress->BoolAddress[i]);

			if ((val & 0x80) == 0)
			{
				val = (i << 8) | val;

				handles->Add(val);
			}
		}

		return handles->ToArray();
	}
}