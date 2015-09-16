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
			private ref struct PoolTask : IScriptTask
			{
				enum class Type
				{
					Ped,
					Object,
					Vehicle,
					Entity
				};

				PoolTask(Type type) : mType(type), mPosCheck(false), mModelCheck(false) {}

				bool CheckEntity(INT64 address)
				{
					if (this->mModelCheck)
					{
						INT64 v3 = reinterpret_cast<INT64(*)(INT64)>(MemoryAccess::EntityModelAddress1)(*reinterpret_cast<PINT64>(address + 32));
						int v7 = *reinterpret_cast<PINT16>(v3);
						INT64 v4 = (v7 ^ *reinterpret_cast<PINT>(v3)) & 0xFFF0000 ^ v7;
						*reinterpret_cast<PINT>(&v4) &= ~(1 << 0x1D);
						v7 = ((v4 ^ *reinterpret_cast<PINT>(v3)) & 0x10000000 ^ v4) & 0x3FFFFFFF;
						INT64 v5 = reinterpret_cast<INT64(*)(INT64)>(MemoryAccess::EntityModelAddress2)(reinterpret_cast<INT64>(&v7));

						if (v5)
						{
							if (*reinterpret_cast<PINT>(v5 + 24) != this->mModel)
							{
								return false;
							}
						}
					}

					if (this->mPosCheck)
					{
						float position[3];
						reinterpret_cast<INT64(*)(INT64, float *)>(MemoryAccess::EntityCoordsAddress)(address, position);

						if (Math::Vector3::Subtract(this->mPosition, Math::Vector3(position[0], position[1], position[2])).LengthSquared() > this->mRadius2)
						{
							return false;
						}
					}

					return true;
				}
				virtual void Run()
				{
					List<int> ^Handles = gcnew List<int>();
					INT64 EntityPool = *reinterpret_cast<PINT64>(MemoryAccess::EntityPoolAddress);
					int(*AddEntToPool)(INT64) = reinterpret_cast<int(*)(INT64)>(*(&MemoryAccess::AddEntToPoolAddress));
					INT64 FuncAddr = *(&MemoryAccess::AddEntToPoolAddress);
					INT64 VehiclePool = *reinterpret_cast<PINT64>(MemoryAccess::VehiclePoolAddress);
					INT64 PedPool = *reinterpret_cast<PINT64>(MemoryAccess::PedPoolAddress);
					Int64 ObjectPool = *reinterpret_cast<PINT64>(MemoryAccess::ObjPoolAddress);
					INT64 VehPoolInfo;

					switch (this->mType)
					{
					case Type::Entity:
					case Type::Vehicle:
						if (EntityPool && VehiclePool && (VehPoolInfo = *reinterpret_cast<PINT64>(VehiclePool)) != 0)
						{
							for (int i = 0; i < *reinterpret_cast<PINT>(VehPoolInfo + 8); i++)
							{
								if (*reinterpret_cast<PINT>(EntityPool + 16) - (*reinterpret_cast<PINT>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if ((*reinterpret_cast<PINT>(*reinterpret_cast<PINT64>(VehPoolInfo + 48) + 4 * (static_cast<INT64>(i) >> 5)) >> (i & 0x1F)) & 1)
								{
									INT64 Address = *reinterpret_cast<PINT64>((i * 8) + *reinterpret_cast<PINT64>(VehPoolInfo));

									if (Address)
									{
										if (CheckEntity(Address))
										{
											Handles->Add(AddEntToPool(Address));
											AddEntToPool = reinterpret_cast<int(*)(INT64)>(FuncAddr);
										}
									}
								}
							}
						}
						if (this->mType != Type::Entity)
						{
							break;
						}
					case Type::Ped:
						if (EntityPool && PedPool)
						{
							for (int i = 0; i < *reinterpret_cast<PINT>(PedPool + 16); i++)
							{
								if (*reinterpret_cast<PINT>(EntityPool + 16) - (*reinterpret_cast<PINT>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if (~(*reinterpret_cast<PUINT8>(*reinterpret_cast<PINT64>(PedPool + 8) + i) >> 7) & 1)
								{
									INT64 Address = *reinterpret_cast<PINT64>(PedPool) + i * *reinterpret_cast<PINT>(PedPool + 20);

									if (Address)
									{
										if (CheckEntity(Address))
										{
											Handles->Add(AddEntToPool(Address));
											AddEntToPool = reinterpret_cast<int(*)(INT64)>(FuncAddr);
										}
									}
								}
							}
						}
						if (this->mType != Type::Entity)
						{
							break;
						}
					case Type::Object:
						if (EntityPool && ObjectPool)
						{
							for (int i = 0; i < *reinterpret_cast<PINT>(ObjectPool + 16); i++)
							{
								if (*(PINT)(EntityPool + 16) - (*reinterpret_cast<PINT>(EntityPool + 32) & 0x3FFFFFFF) <= 256)
								{
									break;
								}

								if (~(*reinterpret_cast<PUINT8>(*reinterpret_cast<PINT64>(ObjectPool + 8) + i) >> 7) & 1)
								{
									INT64 Address = *reinterpret_cast<PINT64>(ObjectPool) + i * *reinterpret_cast<PINT>(ObjectPool + 20);

									if (Address)
									{
										if (CheckEntity(Address))
										{
											Handles->Add(AddEntToPool(Address));
											AddEntToPool = reinterpret_cast<int(*)(INT64)>(FuncAddr);
										}
									}
								}
							}

						}
						break;
					}

					this->mHandles = Handles->ToArray();
				}

				Type mType;
				Math::Vector3 mPosition;
				float mRadius2;
				array<int> ^mHandles;
				bool mPosCheck;
				int mModel;
				bool mModelCheck;
			};
		}

		static MemoryAccess::MemoryAccess()
		{
			UINT64 patternAddress;

			patternAddress = FindPattern("\x33\xFF\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x58", "xxx????xxxxx");
			// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
			EntityAddressFunc = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x33\xC9\x48\x85\xC0\x74\x3B", "xxx????xxxxxxx");
			// 3 bytes are opcode and its first argument, so we add it to get relative address to patternAddress. 7 bytes are length of opcode and its parameters.
			PlayerAddressFunc = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			EntityPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			VehiclePoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			PedPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			ObjPoolAddress = *reinterpret_cast<int *>(patternAddress + 3) + patternAddress + 7;

			patternAddress = FindPattern("\x48\x8B\xC8\xE8\x00\x00\x00\x00\xF3\x0F\x10\x54\x24\x00\xF3\x0F\x10\x4C\x24\x00\xF3\x0F\x10", "xxxx????xxxxx?xxxxx?xxx");
			EntityCoordsAddress = *reinterpret_cast<int *>(patternAddress + 4) + patternAddress + 8;

			patternAddress = FindPattern("\x25\xFF\xFF\xFF\x3F\x89\x44\x24\x38\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x03", "xxxxxxxxxx????xxxxx");
			EntityModelAddress1 = *reinterpret_cast<int *>(patternAddress - 61) + patternAddress - 57;
			EntityModelAddress2 = *reinterpret_cast<int *>(patternAddress + 10) + patternAddress + 14;

			AddEntToPoolAddress = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx") - 0x68;
		}

		UINT64 MemoryAccess::GetAddressOfEntity(int handle)
		{
			return reinterpret_cast<UINT64(*)(int)>(EntityAddressFunc)(handle);
		}
		UINT64 MemoryAccess::GetAddressOfPlayer(int handle)
		{
			return reinterpret_cast<UINT64(*)(int)>(PlayerAddressFunc)(handle);
		}

		array<int> ^MemoryAccess::GetVehicleHandles()
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Vehicle);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetVehicleHandles(int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Vehicle);
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Vehicle);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius, int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Vehicle);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPedHandles()
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Ped);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPedHandles(int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Ped);
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Ped);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius, int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Ped);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPropHandles()
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Object);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPropHandles(int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Object);
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Object);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius, int modelHash)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Object);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;
			pool->mModel = modelHash;
			pool->mModelCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetEntityHandles()
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Entity);

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
		}
		array<int> ^MemoryAccess::GetEntityHandles(Math::Vector3 position, float radius)
		{
			PoolTask ^pool = gcnew PoolTask(PoolTask::Type::Entity);
			pool->mPosition = position;
			pool->mRadius2 = radius * radius;
			pool->mPosCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(pool);

			return pool->mHandles;
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
	}
}