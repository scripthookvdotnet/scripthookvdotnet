#include "NativeMemory.hpp"

#include <Main.h>
#include <Psapi.h>

namespace GTA
{
	using namespace System;
	using namespace System::Collections::Generic;

	static MemoryAccess::MemoryAccess()
	{
		UINT64 patternAddress = FindPattern(EntityAddressPattern, EntityAddressMask);
		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		EntityAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;
		patternAddress = FindPattern(PlayerAddressPattern, PlayerAddressMask);
		// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
		PlayerAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

		patternAddress = FindPattern(EntityPoolPattern, EntityPoolMask);
		EntityPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

		patternAddress = FindPattern(VehiclePoolPattern, VehiclePoolMask);
		VehiclePoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

		patternAddress = FindPattern(PedPoolPattern, PedPoolMask);
		PedPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

		patternAddress = FindPattern(ObjPoolPattern, ObjPoolMask);
		ObjPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

		patternAddress = FindPattern(EntityCoordsPattern, EntityCoordsMask);
		EntityCoordsAddress = *reinterpret_cast<int *>(patternAddress + 4) + patternAddress + 8;

		patternAddress = FindPattern(EntityModelPattern, EntityModelMask);
		EntityModelAddress1 = *reinterpret_cast<int *>(patternAddress - 61) + patternAddress - 57;
		EntityModelAddress2 = *reinterpret_cast<int *>(patternAddress + 10) + patternAddress + 14;

		AddEntToPoolAddress = FindPattern(AddEntToPoolPattern, AddEntToPoolMask) - 0x68;

	}

	UINT64 MemoryAccess::GetAddressOfEntity(int handle)
	{
		return ((UINT64(*)(int))EntityAddress)(handle);
	}
	UINT64 MemoryAccess::GetAddressOfPlayer(int handle)
	{
		return ((UINT64(*)(int))PlayerAddress)(handle);
	}
	float MemoryAccess::GetVehicleRPM(int handle)
	{
		const UINT64 address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2004);
	}
	float MemoryAccess::GetVehicleAcceleration(int handle)
	{
		const UINT64 address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2020);
	}
	float MemoryAccess::GetVehicleSteering(int handle)
	{
		const UINT64 address = GetAddressOfEntity(handle);

		return address == 0 ? 0.0f : *reinterpret_cast<const float *>(address + 2212);
	}
	
	UINT64 MemoryAccess::FindPattern(const char *pattern, const char *mask)
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
					return reinterpret_cast<UINT64>(retAddress) - searchLen;
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
	array<int> ^MemoryPools::GetVehicleHandles()
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Vehicle);
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetVehicleHandles(int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Vehicle);
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetVehicleHandles(Math::Vector3 position, float radius)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Vehicle);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetVehicleHandles(Math::Vector3 position, float radius, int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Vehicle);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPedHandles()
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Ped);
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPedHandles(int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Ped);
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPedHandles(Math::Vector3 position, float radius)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Ped);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPedHandles(Math::Vector3 position, float radius, int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Ped);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPropHandles()
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Object);
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPropHandles(int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Object);
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPropHandles(Math::Vector3 position, float radius)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Object);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetPropHandles(Math::Vector3 position, float radius, int modelHash)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Object);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		pool->_model = modelHash;
		pool->_modelcheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetEntityHandles()
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Entity);
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	array<int> ^MemoryPools::GetEntityHandles(Math::Vector3 position, float radius)
	{
		MemoryPools ^pool = gcnew MemoryPools(Type::Entity);
		pool->_position = position;
		pool->_radius2 = radius * radius;
		pool->_poscheck = true;
		ScriptDomain::CurrentDomain->ExecuteTask(pool);
		return pool->_handles;
	}
	bool MemoryPools::CheckEntity(signed long long Address)
	{
		if (_modelcheck)
		{
			INT64 v3 = ((INT64(*)(INT64))MemoryAccess::EntityModelAddress1)(*(PINT64)(Address + 32));
			int v7 = *(PINT16)(v3);
			INT64 v4 = (v7 ^ *(PINT)v3) & 0xFFF0000 ^ v7;
			*(PINT)&v4 &= ~(1 << 0x1D);
			v7 = ((v4 ^ *(PINT)v3) & 0x10000000 ^ v4) & 0x3FFFFFFF;
			INT64 v5 = ((INT64(*)(INT64))MemoryAccess::EntityModelAddress2)((INT64)&v7);
			if (v5)
			{
				if (*(PINT)(v5 + 24) != _model)
					return false;
			}
		}
		if (_poscheck)
		{
			float position[3];
			((INT64(*)(INT64, float*))MemoryAccess::EntityCoordsAddress)(Address, position);
			if (Math::Vector3::Subtract(_position, Math::Vector3(position[0], position[1], position[2])).LengthSquared() > _radius2)
				return false;
		}
		return true;
	}
	void MemoryPools::Run()
	{
		List<int> ^Handles = gcnew List<int>();
		INT64 EntityPool = *(PINT64)MemoryAccess::EntityPoolAddress;
		int(*AddEntToPool)(INT64) = (int(*)(INT64))*(&MemoryAccess::AddEntToPoolAddress);
		INT64 FuncAddr = *(&MemoryAccess::AddEntToPoolAddress);
		INT64 VehiclePool = *(PINT64)MemoryAccess::VehiclePoolAddress;
		INT64 PedPool = *(PINT64)MemoryAccess::PedPoolAddress;
		Int64 ObjectPool = *(PINT64)MemoryAccess::ObjPoolAddress;
		INT64 VehPoolInfo;
		switch (_type)
		{
		case Type::Entity:
		case Type::Vehicle:
			if (EntityPool && VehiclePool && (VehPoolInfo = *(PINT64)VehiclePool) != 0)
			{
				for (int i = 0; i < *(PINT)(VehPoolInfo + 8); i++)
				{
					if (*(PINT)(EntityPool + 16) - (*(PINT)(EntityPool + 32) & 0x3FFFFFFF) <= 256)
						break;
					if ((*(PINT)(*(PINT64)(VehPoolInfo + 48) + 4 * ((signed __int64)i >> 5)) >> (i & 0x1F)) & 1)
					{
						INT64 Address = *(PINT64)((i * 8) + *(PINT64)VehPoolInfo);
						if (Address)
						{
							if (CheckEntity(Address))
							{
								Handles->Add(AddEntToPool(Address));
								AddEntToPool = (int(*)(INT64))FuncAddr;
							}
						}
					}
				}
			}
			if (_type != Type::Entity)
				break;
		case Type::Ped:
			if (EntityPool && PedPool)
			{
				for (int i = 0; i < *(PINT)(PedPool + 16); i++)
				{
					if (*(PINT)(EntityPool + 16) - (*(PINT)(EntityPool + 32) & 0x3FFFFFFF) <= 256)
						break;
					if (~(*(PUINT8)(*(PINT64)(PedPool + 8) + i) >> 7) & 1)
					{
						INT64 Address = *(PINT64)PedPool + i * *(PINT)(PedPool + 20);
						if (Address)
						{
							if (CheckEntity(Address))
							{
								Handles->Add(AddEntToPool(Address));
								AddEntToPool = (int(*)(INT64))FuncAddr;
							}
						}
					}
				}
			}
			if (_type != Type::Entity)
				break;
		case Type::Object:
			if (EntityPool && ObjectPool)
			{
				for (int i = 0; i < *(PINT)(ObjectPool + 16); i++)
				{
					if (*(PINT)(EntityPool + 16) - (*(PINT)(EntityPool + 32) & 0x3FFFFFFF) <= 256)
						break;
					if (~(*(PUINT8)(*(PINT64)(ObjectPool + 8) + i) >> 7) & 1)
					{
						INT64 Address = *(PINT64)ObjectPool + i * *(PINT)(ObjectPool + 20);
						if (Address)
						{
							if (CheckEntity(Address))
							{
								Handles->Add(AddEntToPool(Address));
								AddEntToPool = (int(*)(INT64))FuncAddr;
							}
						}
					}
				}

			}
			break;
		}
		_handles = Handles->ToArray();
	}

}
