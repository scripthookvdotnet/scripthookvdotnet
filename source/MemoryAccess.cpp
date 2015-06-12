#include "MemoryAccess.hpp"
#include "Log.hpp"
#include "Main.h"
#include <Psapi.h>

namespace GTA
{
	using namespace System::Collections::Generic;

	uintptr_t MemoryAccess::FindPattern(char pattern[], char mask[])
	{
		MODULEINFO modInfo = { 0 };

		GetModuleInformation(GetCurrentProcess(), GetModuleHandle(L"GTA5.exe"), &modInfo, sizeof(MODULEINFO));

		char* start_offset = reinterpret_cast<char*>(modInfo.lpBaseOfDll);
		uintptr_t size = static_cast<uintptr_t>(modInfo.SizeOfImage);

		intptr_t pos = 0;
		uintptr_t searchLen = static_cast<uintptr_t>(strlen(mask) - 1);

		for (char* retAddress = start_offset; retAddress < start_offset + size; retAddress++)
		{
			if (*retAddress == pattern[pos] || mask[pos] == '?'){
				if (mask[pos + 1] == '\0')
					return (reinterpret_cast<uintptr_t>(retAddress)-searchLen);
				pos++;
			}
			else
				pos = 0;
		}

		return NULL;
	}

	static MemoryAccess::MemoryAccess()
	{
		uintptr_t patternAddress = FindPattern(EntityPoolOpcodePattern, EntityPoolOpcodeMask);

		uintptr_t adr = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7; // 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		ADDRESS_ENTITYPOOL = reinterpret_cast<Pool **>(adr);
	}

	GameVersion MemoryAccess::GetGameVersion()
	{
		return (GameVersion)((int)getGameVersion() + 1); // converting eGameVersion to GameVersion.
	}

	int MemoryAccess::HandleToIndex(int Handle)
	{
		return Handle >> 8; // == Handle / 256
	}

	uintptr_t MemoryAccess::GetAddressOfItemInPool(Pool* PoolAddress, int Handle)
	{
		if (PoolAddress == NULL) return 0;

		int index = HandleToIndex(Handle);

		char flag = PoolAddress->BoolAdr[index]; // flag should be equal to 2 if everything is ok
		if (flag & 0x80) return 0; // parity check? (taken from ScriptHookDotNet for IV
		if (int(flag) != (Handle & 0xFF)) return 0;

		return (PoolAddress->ListAddr + index * PoolAddress->ItemSize);
	}
	array<int>^ MemoryAccess::GetListOfHandlesInPool(Pool* PoolAddress)
	{
		if (PoolAddress == NULL) return nullptr;

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
	uintptr_t MemoryAccess::GetAddressOfEntity(int Handle)
	{
		return *reinterpret_cast<uintptr_t*>(GetAddressOfItemInPool(*ADDRESS_ENTITYPOOL, Handle) + 8);
	}

	array<int> ^MemoryAccess::GetEntityHandleList()
	{
		return GetListOfHandlesInPool(*ADDRESS_ENTITYPOOL);
	}
}

