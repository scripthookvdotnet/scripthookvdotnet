#include "NativeMemory.hpp"

#include <Main.h>
#include <Psapi.h>

using namespace System;
using namespace System::Collections::Generic;

namespace GTA
{
	namespace Native
	{
		namespace
		{
			private ref struct EntityPoolTask : IScriptTask
			{
				enum class Type
				{
					Ped,
					Object,
					Vehicle,
					Entity
				};

				EntityPoolTask(Type type) : _type(type) { }

				bool CheckEntity(uintptr_t address)
				{
					if (_posCheck)
					{
						float position[3];
						MemoryAccess::GetEntityPos(address, position);

						if (Math::Vector3::Subtract(_position, Math::Vector3(position[0], position[1], position[2])).LengthSquared() > _radiusSquared)
						{
							return false;
						}
					}

					if (_modelCheck)
					{
						UINT32 v0 = *reinterpret_cast<UINT32 *>(MemoryAccess::GetEntityModel1(*reinterpret_cast<UINT64 *>(address + 32)));
						UINT32 v1 = v0 & 0xFFFF;
						UINT32 v2 = ((v1 ^ v0) & 0x0FFF0000 ^ v1) & 0xDFFFFFFF;
						UINT32 v3 = ((v2 ^ v0) & 0x10000000 ^ v2) & 0x3FFFFFFF;
						const uintptr_t v5 = MemoryAccess::GetEntityModel2(reinterpret_cast<uintptr_t>(&v3));

						if (v5 && *reinterpret_cast<int *>(v5 + 24) != _modelHash)
						{
							return false;
						}
					}

					return true;
				}
				virtual void Run()
				{
					const uintptr_t EntityPool = *MemoryAccess::EntityPoolAddress;
					const uintptr_t VehiclePool = *MemoryAccess::VehiclePoolAddress;
					const uintptr_t PedPool = *MemoryAccess::PedPoolAddress;
					const uintptr_t ObjectPool = *MemoryAccess::ObjectPoolAddress;

					if (EntityPool == 0 || VehiclePool == 0 || PedPool == 0 || ObjectPool == 0)
					{
						return;
					}

					switch (_type)
					{
						case Type::Entity:
						case Type::Vehicle:
						{
							const uintptr_t VehiclePoolInfo = *reinterpret_cast<UINT64 *>(VehiclePool);
							for (unsigned int i = 0; i < *reinterpret_cast<UINT32 *>(VehiclePoolInfo + 8); i++)
							{
								if (*reinterpret_cast<UINT32 *>(EntityPool + 16) - (*reinterpret_cast<UINT32 *>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if ((*reinterpret_cast<UINT32 *>(*reinterpret_cast<UINT64 *>(VehiclePoolInfo + 48) + 4 * (static_cast<UINT64>(i) >> 5)) >> (i & 0x1F)) & 1)
								{
									const uintptr_t address = *reinterpret_cast<UINT64 *>(i * 8 + *reinterpret_cast<UINT64 *>(VehiclePoolInfo));

									if (address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::AddEntityToPool(address));
									}
								}
							}
							if (_type != Type::Entity)
							{
								break;
							}
						}
						case Type::Ped:
						{
							for (unsigned int i = 0; i < *reinterpret_cast<UINT32 *>(PedPool + 16); i++)
							{
								if (*reinterpret_cast<UINT32 *>(EntityPool + 16) - (*reinterpret_cast<UINT32 *>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if (~(*reinterpret_cast<UINT8 *>(*reinterpret_cast<UINT64 *>(PedPool + 8) + i) >> 7) & 1)
								{
									const uintptr_t address = *reinterpret_cast<UINT64 *>(PedPool) + i * *reinterpret_cast<UINT32 *>(PedPool + 20);

									if (address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::AddEntityToPool(address));
									}
								}
							}
							if (_type != Type::Entity)
							{
								break;
							}
						}
						case Type::Object:
						{
							for (unsigned int i = 0; i < *reinterpret_cast<UINT32 *>(ObjectPool + 16); i++)
							{
								if (*reinterpret_cast<UINT32 *>(EntityPool + 16) - (*reinterpret_cast<UINT32 *>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if (~(*reinterpret_cast<UINT8 *>(*reinterpret_cast<UINT64 *>(ObjectPool + 8) + i) >> 7) & 1)
								{
									const uintptr_t address = *reinterpret_cast<UINT64 *>(ObjectPool) + i * *reinterpret_cast<UINT32 *>(ObjectPool + 20);

									if (address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::AddEntityToPool(address));
									}
								}
							}
						}
					}
				}

				Type _type;
				List<int> ^_handles = gcnew List<int>();
				bool _posCheck, _modelCheck;
				Math::Vector3 _position;
				float _radiusSquared;
				int _modelHash;
			};
		}

		static MemoryAccess::MemoryAccess()
		{
			uintptr_t address;

			// Get relative address and add it to the instruction address.
			address = FindPattern("\xE8\x00\x00\x00\x00\x48\x8B\xD8\x48\x85\xC0\x74\x2E\x48\x83\x3D", "x????xxxxxxxxxxx");
			GetAddressOfEntity = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 1) + address + 5);
			address = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x1C\x8A\x88", "xxx????xxxxxxx");
			GetAddressOfPlayer = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 3) + address + 7);

			address = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			AddEntityToPool = reinterpret_cast<int(*)(uintptr_t)>(address - 0x68);

			address = FindPattern("\x48\x8B\xDA\xE8\x00\x00\x00\x00\xF3\x0F\x10\x44\x24", "xxxx????xxxxx");
			GetEntityPos = reinterpret_cast<UINT64(*)(UINT64, float *)>(address - 6);
			address = FindPattern("\x0F\x85\x00\x00\x00\x00\x48\x8B\x4B\x20\xE8\x00\x00\x00\x00\x48\x8B\xC8", "xx????xxxxx????xxx");
			GetEntityModel1 = reinterpret_cast<UINT64(*)(UINT64)>(*reinterpret_cast<int *>(address + 11) + address + 15);
			address = FindPattern("\x45\x33\xC9\x3B\x05", "xxxxx");
			GetEntityModel2 = reinterpret_cast<UINT64(*)(UINT64)>(address - 0x46);

			address = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			EntityPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			VehiclePoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			PedPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			ObjectPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);

			CreateNmMessageFunc = FindPattern("\x33\xDB\x48\x89\x1D\x00\x00\x00\x00\x85\xFF", "xxxxx????xx") - 0x42;
			GiveNmMessageFunc = FindPattern("\x48\x8b\xc4\x48\x89\x58\x08\x48\x89\x68\x10\x48\x89\x70\x18\x48\x89\x78\x20\x41\x55\x41\x56\x41\x57\x48\x83\xec\x20\xe8\x00\x00\x00\x00\x48\x8b\xd8\x48\x85\xc0\x0f", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxx");
			SetNmBoolAddress = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmFloatAddress = FindPattern("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmIntAddress = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmStringAddress = FindPattern("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			SetNmVec3Address = FindPattern("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");

			address = FindPattern("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			UINT64 baseFuncAddr = address + *reinterpret_cast<int*>(address) + 4;
			modelHashEntries = *reinterpret_cast<PUINT16>(baseFuncAddr + *reinterpret_cast<int*>(baseFuncAddr + 3) + 7);
			modelNum1 = *reinterpret_cast<int*>(*reinterpret_cast<int*>(baseFuncAddr + 0x52) + baseFuncAddr + 0x56);
			modelNum2 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x63) + baseFuncAddr + 0x67);
			modelNum3 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x7A) + baseFuncAddr + 0x7E);
			modelNum4 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x81) + baseFuncAddr + 0x85);
			modelHashTable = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x24) + baseFuncAddr + 0x28);

			address = FindPattern("\x48\x8D\x8F\x00\x00\x00\x00\x4C\x8B\xC3\xF3\x0F\x11\x7C\x24", "xxx????xxxxxxxx");
			currentGearOffset = address == 0 ? 0 : *(int*)(address + 3) + 2;
			highGearOffset = address == 0 ? 0 : *(int*)(address + 3) + 6;

			address = FindPattern("\x74\x26\x0F\x57\xC9\x0F\x2F\x8B\x34\x08\x00\x00\x73\x1A\xF3\x0F\x10\x83\x24\x08\x00\x00", "x?xxxxxx????x?xxxx????");
			fuelLevelOffset = address == 0 ? 0 : *(int*)(address + 8);

			address = FindPattern("\x76\x03\x0F\x28\xF0\xF3\x44\x0F\x10\x93", "xxxxxxxxxx");
			currentRPMOffset = address == 0 ? 0 : *(int*)(address + 10);
			accelerationOffset = address == 0 ? 0 : *(int*)(address + 10) + 0x10;

			address = FindPattern("\x74\x0A\xF3\x0F\x11\xB3\x1C\x09\x00\x00\xEB\x25", "xxxxxx????xx");
			steeringScaleOffset = address == 0 ? 0 : *(int*)(address + 6);
			steeringAngleOffset = address == 0 ? 0 : *(int*)(address + 6) + 8;

			address = FindPattern("\xF3\x0F\x10\x8F\x10\x0A\x00\x00\xF3\x0F\x59\x05\x5E\x30\x8D\x00", "xxxx????xxxx????");
			wheelSpeedOffset = address == 0 ? 0 : *(int*)(address + 4);
		}

		array<int> ^MemoryAccess::GetVehicleHandles()
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius, int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles()
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius, int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles()
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Object);

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius, int modelhash)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;
			poolTask->_modelHash = modelhash;
			poolTask->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetEntityHandles()
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Entity);

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetEntityHandles(Math::Vector3 position, float radius)
		{
			auto poolTask = gcnew EntityPoolTask(EntityPoolTask::Type::Entity);
			poolTask->_position = position;
			poolTask->_radiusSquared = radius * radius;
			poolTask->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(poolTask);

			return poolTask->_handles->ToArray();
		}

		bool MemoryAccess::IsModelAPed(int modelHash)
		{
			UINT64 modelInfo = FindCModelInfo(modelHash);

			if (modelInfo)
			{
				return GetModelInfoClassType(modelInfo) == 6;
			}

			return false;
		}

		struct HashNode
		{
			int hash;
			UINT16 data;
			UINT16 padding;
			HashNode* next;
		};

		inline bool bittest(int data, unsigned char index)
		{
			return (data & (1 << index)) != 0;
		}

		uintptr_t MemoryAccess::FindCModelInfo(int modelHash)
		{
			HashNode** HashMap = reinterpret_cast<HashNode**>(modelHashTable);
			for (HashNode* cur = HashMap[static_cast<unsigned int>(modelHash) % modelHashEntries]; cur; cur = cur->next)
			{
				if (cur->hash != modelHash)
				{
					continue;
				}

				UINT16 data = cur->data;
				if ((int)data < modelNum1 && bittest(*reinterpret_cast<int*>(modelNum2 + (4 * data >> 5)), data & 0x1F))
				{
					UINT64 addr1 = modelNum4 + modelNum3 * data;
					if (addr1)
					{
						return *reinterpret_cast<PUINT64>(addr1);
					}
				}
			}

			return 0;
		}
		int MemoryAccess::GetModelInfoClassType(System::UInt64 address)
		{
			if (address)
			{
				return (*reinterpret_cast<PBYTE>(address + 157) & 0x1F);
			}

			return 0;
		}

		uintptr_t MemoryAccess::FindPattern(const char *pattern, const char *mask)
		{
			MODULEINFO module = { };
			GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &module, sizeof(MODULEINFO));

			auto *address = reinterpret_cast<const char *>(module.lpBaseOfDll), *address_end = address + module.SizeOfImage;
			const auto mask_length = static_cast<size_t>(strlen(mask) - 1);

			for (size_t i = 0; address < address_end; address++)
			{
				if (*address == pattern[i] || mask[i] == '?')
				{
					if (mask[i + 1] == '\0')
					{
						return reinterpret_cast<uintptr_t>(address) - mask_length;
					}

					i++;
				}
				else
				{
					i = 0;
				}
			}

			return 0;
		}
	}
}
