#include "NativeMemory.hpp"
#include "ScriptDomain.hpp"

#include <Main.h>
#include <Psapi.h>

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	private ref struct Pools : public IScriptTask
	{
	public:
		enum class Type
		{
			Ped,
			Object,
			Vehicle,
			Entity
		};
		static array<int> ^GetHandles(Type poolType)
		{
			Pools ^task = gcnew Pools(poolType);
			ScriptDomain::CurrentDomain->ExecuteTask(task);
			array<int> ^ManagedArray = gcnew array<int>(task->_handleCount);
			if (task->_handleCount > 0)
			{
				pin_ptr<int> ManagedStart = &ManagedArray[0];
				memcpy(ManagedStart, task->_arr, task->_handleCount * 4);
			}
			delete task->_arr;
			return ManagedArray;
		}
		virtual void Run()
		{
			switch (_poolType)
			{
			case Type::Object:
				_handleCount = worldGetAllObjects(_arr, objSize);
				break;
			case Type::Ped:
				_handleCount = worldGetAllPeds(_arr, pedSize);
				break;
			case Type::Vehicle:
				_handleCount = worldGetAllVehicles(_arr, vehSize);
				break;
			case Type::Entity:
				_handleCount = worldGetAllPeds(_arr, pedSize);
				_handleCount += worldGetAllVehicles((_arr + _handleCount), vehSize);
				_handleCount += worldGetAllObjects((_arr + _handleCount), objSize);
				break;
			}
		}
	private:

		const int pedSize = 256, vehSize = 300, objSize = 2000;
		Pools(Type type) : _poolType(type)
		{
			switch (type)
			{
			case Type::Object: _max = objSize; break;
			case Type::Ped: _max = pedSize; break;
			case Type::Vehicle: _max = vehSize; break;
			case Type::Entity: _max = objSize + pedSize + vehSize;
			}
			_arr = new int[_max];
		}
		Type _poolType;
		int* _arr;
		int _max;
		int _handleCount;
	};

	static MemoryAccess::MemoryAccess()
	{
		uintptr_t patternAddress = FindPattern(EntityAddressPattern, EntityAddressMask);
		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		EntityAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;
		patternAddress = FindPattern(PlayerAddressPattern, PlayerAddressMask);
		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		PlayerAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

	}

	uintptr_t MemoryAccess::GetAddressOfEntity(int handle)
	{
		return ((uintptr_t(*)(int))EntityAddress)(handle);
	}
	uintptr_t MemoryAccess::GetAddressOfPlayer(int handle)
	{
		return ((uintptr_t(*)(int))PlayerAddress)(handle);
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
	array<int> ^MemoryAccess::GetAllVehicleHandles()
	{
		return Pools::GetHandles(Pools::Type::Vehicle);
	}

	array<int> ^MemoryAccess::GetAllPedHandles()
	{
		return Pools::GetHandles(Pools::Type::Ped);
	}

	array<int> ^MemoryAccess::GetAllObjectHandles()
	{
		return Pools::GetHandles(Pools::Type::Object);
	}
	array<int> ^MemoryAccess::GetAllEntityHandles()
	{
		return Pools::GetHandles(Pools::Type::Entity);
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
}
