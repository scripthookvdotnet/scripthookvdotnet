#include "NativeMemory.hpp"

#include <Main.h>
#include <Psapi.h>

namespace GTA
{
	namespace Native
	{
		using namespace System;
		using namespace System::Collections::Generic;

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

				EntityPoolTask(Type type) : _type(type), _handles(gcnew List<int>()), _posCheck(false), _modelCheck(false) { }

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
				List<int> ^_handles;
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
			// 3 bytes equal the size of the opcode and its first argument. 7 bytes are the length of opcode and all its parameters.
			address = FindPattern("\x33\xFF\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x58", "xxx????xxxxx");
			GetAddressOfEntity = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x33\xC9\x48\x85\xC0\x74\x3B", "xxx????xxxxxxx");
			GetAddressOfPlayer = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 3) + address + 7);

			address = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			AddEntityToPool = reinterpret_cast<int(*)(uintptr_t)>(address - 0x68);

			address = FindPattern("\x48\x8B\xC8\xE8\x00\x00\x00\x00\xF3\x0F\x10\x54\x24\x00\xF3\x0F\x10\x4C\x24\x00\xF3\x0F\x10", "xxxx????xxxxx?xxxxx?xxx");
			GetEntityPos = reinterpret_cast<UINT64(*)(UINT64, float *)>(*reinterpret_cast<int *>(address + 4) + address + 8);
			address = FindPattern("\x25\xFF\xFF\xFF\x3F\x89\x44\x24\x38\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x03", "xxxxxxxxxx????xxxxx");
			GetEntityModel1 = reinterpret_cast<UINT64(*)(UINT64)>(*reinterpret_cast<int *>(address - 61) + address - 57);
			GetEntityModel2 = reinterpret_cast<UINT64(*)(UINT64)>(*reinterpret_cast<int *>(address + 10) + address + 14);

			address = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			EntityPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			VehiclePoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			PedPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			ObjectPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);

			CreateNmMessageFunc = FindPattern("\x33\xDB\x48\x89\x1D\x00\x00\x00\x00\x85\xFF", "xxxxx????xx") - 0x42;
			GiveNmMessageFunc = FindPattern("\x0F\x84\x00\x00\x00\x00\x48\x8B\x01\xFF\x90\x00\x00\x00\x00\x41\x3B\xC5", "xx????xxxxx????xxx") - 0x78;
			SetNmBoolAddress = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmFloatAddress = FindPattern("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmIntAddress = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmStringAddress = FindPattern("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			SetNmVec3Address = FindPattern("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
		}

		array<int> ^MemoryAccess::GetVehicleHandles()
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius, int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles()
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius, int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles()
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Object);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius, int modelhash)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;
			pool->_modelHash = modelhash;
			pool->_modelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetEntityHandles()
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Entity);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetEntityHandles(Math::Vector3 position, float radius)
		{
			EntityPoolTask ^pool = gcnew EntityPoolTask(EntityPoolTask::Type::Entity);
			pool->_position = position;
			pool->_radiusSquared = radius * radius;
			pool->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->_handles->ToArray();
		}

		uintptr_t MemoryAccess::FindPattern(const char *pattern, const char *mask)
		{
			MODULEINFO module = { };
			GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &module, sizeof(MODULEINFO));

			const char *address = reinterpret_cast<const char *>(module.lpBaseOfDll), *address_end = address + module.SizeOfImage;
			const size_t mask_length = static_cast<size_t>(strlen(mask) - 1);

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
