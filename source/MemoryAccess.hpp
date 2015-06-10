#pragma once

#include "Game.hpp"
#include <inttypes.h>


namespace GTA
{
	private ref class MemoryAccess sealed
	{
	private:
		static intptr_t ADDRESS_ENTITYPOOL;
	public:

		static MemoryAccess();

		static GameVersion GetGameVersion();

		static int HandleToIndex(int Handle);

		static intptr_t GetAddressOfItemInPool(intptr_t PoolAddress, int Handle);
		static array<int>^ GetListOfHandlesInPool(intptr_t PoolAddress);

		static intptr_t GetAddressOfEntity(int Handle);

		static array<int>^ GetEtityHandleList();
	};
}


