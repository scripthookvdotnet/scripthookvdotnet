#include "MemoryAccess.hpp"
#include "Log.hpp"
#include "Main.h"

namespace GTA
{
	using namespace System::Collections::Generic;

	static MemoryAccess::MemoryAccess()
	{
		intptr_t baseAddr = reinterpret_cast<intptr_t>(GetModuleHandle(L"GTA5.exe"));
		GameVersion gameVersion = GetGameVersion();

		switch (gameVersion)
		{
		case GameVersion::v1_0_350_1_STEAM:
			ADDRESS_ENTITYPOOL = baseAddr + 0x2994C00;
			break;
		case GameVersion::v1_0_350_2_NOSTEAM:
			ADDRESS_ENTITYPOOL = baseAddr + 0x29A1920;		// need to add addresses for older versions versions
			break;
		default:
			ADDRESS_ENTITYPOOL = 0;
			break;
		}
	}

	GameVersion MemoryAccess::GetGameVersion()
	{
		return (GameVersion)((int)getGameVersion() + 1); // converting eGameVersion to GameVersion.
	}

	int MemoryAccess::HandleToIndex(int Handle)
	{
		return Handle >> 8; // == Handle / 256
	}

	intptr_t MemoryAccess::GetAddressOfItemInPool(intptr_t PoolAddress, int Handle)
	{
		if (PoolAddress == 0) return 0;

		intptr_t listadr = *reinterpret_cast<intptr_t*>(PoolAddress);
		intptr_t booladr = *reinterpret_cast<intptr_t*>(PoolAddress + 8);
		int maxcount = *reinterpret_cast<int*>(PoolAddress + 16); // don't forget what these are 4 bytes
		int itemsize = *reinterpret_cast<int*>(PoolAddress + 20);

		int index = HandleToIndex(Handle);

		char flag = *reinterpret_cast<char*>(booladr + index); // flag should be equal to 2 if everything is ok
		if (flag & 0x80) return 0; // parity check? (taken from ScriptHookDotNet for IV
		if (int(flag) != (Handle & 0xFF)) return 0;

		return (listadr + index * itemsize);
	}
	array<int>^ MemoryAccess::GetListOfHandlesInPool(intptr_t PoolAddress)
	{
		if (PoolAddress == 0) return nullptr;

		List<int> ^handles = gcnew List<int>();

		intptr_t listadr = *reinterpret_cast<intptr_t*>(PoolAddress);
		intptr_t booladr = *reinterpret_cast<intptr_t*>(PoolAddress + 8);
		int maxcount = *reinterpret_cast<int*>(PoolAddress + 16);
		int itemsize = *reinterpret_cast<int*>(PoolAddress + 20);

		for (int i = 0; i < maxcount; i++)
		{
			int val = static_cast<int>(*reinterpret_cast<char*>(booladr + i)); // parity value, important to cast it to int

			if ((val & 0x80) == 0)
			{
				val = (i << 8) | val;
				handles->Add(val);
			}
		}

		return handles->ToArray();
	}
	intptr_t MemoryAccess::GetAddressOfEntity(int Handle)
	{
		return *reinterpret_cast<intptr_t*>(GetAddressOfItemInPool(*reinterpret_cast<intptr_t*>(ADDRESS_ENTITYPOOL), Handle) + 8);
	}

	array<int> ^MemoryAccess::GetEtityHandleList()
	{
		return GetListOfHandlesInPool(*reinterpret_cast<intptr_t*>(ADDRESS_ENTITYPOOL));
	}
}

