#include "NativeMemory.hpp"
#include "ScriptDomain.hpp"

#include <Main.h>
#include <Psapi.h>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::Runtime::InteropServices;

const char* const _cellEmailBcon = "CELL_EMAIL_BCON";
const char* const _string = "STRING";
const char* const _nullStr = "";

namespace GTA
{
	namespace Native
	{
		namespace
		{
			[StructLayout(LayoutKind::Explicit)]
			private value class EntityPool
			{
			public:
				[FieldOffset(0x10)]UInt32 num1;
				[FieldOffset(0x20)]UInt32 num2;

				inline bool Full()
				{
					return num1 - (num2 & 0x3FFFFFFF) <= 256;
				}
			};

			[StructLayout(LayoutKind::Explicit)]
			private value class VehiclePool
			{
			public:
				[FieldOffset(0x00)]UInt64 *poolAddress;
				[FieldOffset(0x08)]UInt32 size;
				[FieldOffset(0x30)]UInt32* bitArray;
				[FieldOffset(0x60)]UInt32 itemCount;

				inline bool isValid(UInt32 i)
				{
					return (bitArray[i >> 5] >> (i & 0x1F)) & 1;
				}

				inline UInt64 getAddress(UInt32 i)
				{
					return poolAddress[i];
				}
			};

			[StructLayout(LayoutKind::Explicit)]
			private value class GenericPool{
			public:
				[FieldOffset(0x00)] UInt64 poolStartAddress;
				[FieldOffset(0x08)] Byte* byteArray;
				[FieldOffset(0x10)] UInt32 size;
				[FieldOffset(0x14)] UInt32 itemSize;


				inline bool isValid(UInt32 i)
				{
					return mask(i) != 0;
				}

				inline UInt64 getAddress(UInt32 i)
				{
					return mask(i) & (poolStartAddress + i * itemSize);
				}
			private:
				inline long long mask(UInt32 i)
				{
					long long num1 = byteArray[i] & 0x80;
					return ~((num1 | -num1) >> 63);
				}
			};


			private ref struct EntityPoolTask : IScriptTask
			{
				enum class Type
				{
					Ped=1,
					Object=2,
					Vehicle=4,
					PickupObject=8
				};

				EntityPoolTask(Type type) : _type(type) { }

				inline bool CheckEntity(uintptr_t address)
				{
					if (_posCheck)
					{
						float position[3];
						MemoryAccess::_entityPositionFunc(address, position);

						if(_position.DistanceToSquared(Math::Vector3(position[0], position[1], position[2])) > _radiusSquared)
						{
							return false;
						}
					}

					if (_modelCheck)
					{
						UINT32 v0 = *reinterpret_cast<UINT32 *>(MemoryAccess::_entityModel1Func(*reinterpret_cast<UINT64 *>(address + 32)));
						UINT32 v1 = v0 & 0xFFFF;
						UINT32 v2 = ((v1 ^ v0) & 0x0FFF0000 ^ v1) & 0xDFFFFFFF;
						UINT32 v3 = ((v2 ^ v0) & 0x10000000 ^ v2) & 0x3FFFFFFF;
						const uintptr_t v5 = MemoryAccess::_entityModel2Func(reinterpret_cast<uintptr_t>(&v3));

						
						if (!v5)
						{
							return false;
						}
						for each(int hash in _modelHashes)
						{
							if (*reinterpret_cast<int *>(v5 + 24) == hash)
							{
								return true;
							}
						}
						return false;
					}

					return true;
				}

				virtual void Run()
				{
					if(*MemoryAccess::_entityPoolAddress == 0)
					{
						return;
					}
					EntityPool* entityPool = reinterpret_cast<EntityPool*>(*MemoryAccess::_entityPoolAddress);
	
					if(_type.HasFlag(Type::Vehicle))
					{
						if(*MemoryAccess::_vehiclePoolAddress)
						{
							VehiclePool* vehiclePool = *reinterpret_cast<VehiclePool**>(*MemoryAccess::_vehiclePoolAddress);

							for(UInt32 i = 0; i < vehiclePool->size; i++)
							{
								if(entityPool->Full())
								{
									break;
								}
								if(vehiclePool->isValid(i))
								{
									UInt64 address = vehiclePool->getAddress(i);
									if(address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::_addEntityToPoolFunc(address));
									}
								}
							}
						}
					}
					if(_type.HasFlag(Type::Ped))
					{
						if(*MemoryAccess::_pedPoolAddress)
						{
							GenericPool* pedPool = reinterpret_cast<GenericPool*>(*MemoryAccess::_pedPoolAddress);

							for(UInt32 i = 0; i < pedPool->size; i++)
							{
								if(entityPool->Full())
								{
									break;
								}
								if(pedPool->isValid(i))
								{
									UInt64 address = pedPool->getAddress(i);
									if(address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::_addEntityToPoolFunc(address));
									}
								}
							}
						}
					}
					if(_type.HasFlag(Type::Object))
					{
						if(*MemoryAccess::_objectPoolAddress)
						{
							GenericPool* propPool = reinterpret_cast<GenericPool*>(*MemoryAccess::_objectPoolAddress);
							
							for(UInt32 i = 0; i < propPool->size; i++)
							{
								if(entityPool->Full())
								{
									break;
								}
								if(propPool->isValid(i))
								{
									UInt64 address = propPool->getAddress(i);
									if(address && CheckEntity(address))
									{
										_handles->Add(MemoryAccess::_addEntityToPoolFunc(address));
									}
								}
							}
						}
					}
					if(_type.HasFlag(Type::PickupObject))
					{
						if(*MemoryAccess::_pickupObjectPoolAddress)
						{
							GenericPool* pickupPool = reinterpret_cast<GenericPool*>(*MemoryAccess::_pickupObjectPoolAddress);

							for(UInt32 i = 0; i < pickupPool->size; i++)
							{
								if(entityPool->Full())
								{
									break;
								}
								if(pickupPool->isValid(i))
								{
									UInt64 address = pickupPool->getAddress(i);
									if(address)
									{
										if (_posCheck)
										{
											float* position = (float*)(address + 0x90);
											if(_position.DistanceToSquared(Math::Vector3(position[0], position[1], position[2])) > _radiusSquared)
											{
												continue;
											}
										}
										_handles->Add(MemoryAccess::_addEntityToPoolFunc(address));
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
				array<int> ^_modelHashes;
			};
			private ref struct EuphoriaMessageTask : IScriptTask
			{
				EuphoriaMessageTask(int target, String ^message, Dictionary<String ^, Object ^> ^arguments) : _targetHandle(target), _message(message), _arguments(arguments) { }

				virtual void Run()
				{
					__int64 NativeFunc = Native::MemoryAccess::CreateNmMessageFunc;
					__int64 MessageAddress = reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(NativeFunc + 0x22) + NativeFunc + 0x26)(4632);

					if (MessageAddress == 0)
					{
						return;
					}

					reinterpret_cast<__int64(*)(__int64, __int64, int)>(*reinterpret_cast<int*>(NativeFunc + 0x3C) + NativeFunc + 0x40)(MessageAddress, MessageAddress + 24, 64);

					for each (auto argument in _arguments)
					{
						IntPtr name = ScriptDomain::CurrentDomain->PinString(argument.Key);

						if (argument.Value->GetType() == Boolean::typeid)
						{
							MemoryAccess::SetNmBoolAddress(MessageAddress, name.ToInt64(), (bool)argument.Value ? 1 : 0);
						}
						if (argument.Value->GetType() == Int32::typeid)
						{
							MemoryAccess::SetNmIntAddress(MessageAddress, name.ToInt64(), (int)argument.Value);
						}
						if (argument.Value->GetType() == Single::typeid)
						{
							MemoryAccess::SetNmFloatAddress(MessageAddress, name.ToInt64(), (float)argument.Value);
						}
						if (argument.Value->GetType() == Math::Vector3::typeid)
						{
							auto value = (Math::Vector3)argument.Value;

							MemoryAccess::SetNmVec3Address(MessageAddress, name.ToInt64(), value.X, value.Y, value.Z);
						}
						if (argument.Value->GetType() == String::typeid)
						{
							MemoryAccess::SetNmStringAddress(MessageAddress, name.ToInt64(), ScriptDomain::CurrentDomain->PinString((String ^)argument.Value).ToInt64());
						}
					}

					__int64 BaseFunc = Native::MemoryAccess::GiveNmMessageFunc;
					__int64 ByteAddr = *reinterpret_cast<int*>(BaseFunc + 0xBC) + BaseFunc + 0xC0;
					__int64 UnkStrAddr = *reinterpret_cast<int*>(BaseFunc + 0xCE) + BaseFunc + 0xD2;
					__int64 _PedAddress = Native::MemoryAccess::GetEntityAddress(_targetHandle).ToInt64();
					__int64 PedNmAddress;
					bool v5 = false;
					__int8 v7;
					__int64 v11;
					__int64 v12;
					if (_PedAddress == 0)
						return;
					if (*reinterpret_cast<__int64*>(_PedAddress + 48) == 0)
						return;
					int AddrOff = getGameVersion() < VER_1_0_573_1_NOSTEAM ? 0 : 16;
					PedNmAddress = *reinterpret_cast<__int64*>(_PedAddress + 5016 + AddrOff); //
					if (*reinterpret_cast<__int64*>(_PedAddress + 48) == PedNmAddress && *reinterpret_cast<float*>(_PedAddress + 5232 + AddrOff) <= *reinterpret_cast<float*>(_PedAddress + 640))
					{
						if ((*reinterpret_cast<int(**)(__int64)> (*reinterpret_cast<__int64*>(PedNmAddress) + 152))(PedNmAddress) != -1)
						{
							if (*(short *)(reinterpret_cast<__int64(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xA2) + BaseFunc + 0xA6)(*(__int64 *)(*(__int64 *)(_PedAddress + 4208 + AddrOff) + 864)) + 52) == 401)
							{
								v5 = true;
							}
							else
							{
								v7 = *(__int8*)ByteAddr;
								if (v7)
								{
									reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xD3) + BaseFunc + 0xD7)(UnkStrAddr);
									v7 = *(__int8*)ByteAddr;
								}
								int count = *reinterpret_cast<int*>(*reinterpret_cast<__int64*>(_PedAddress + 4208 + AddrOff) + 1064);
								if (v7)
								{
									reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0xF0) + BaseFunc + 0xF4)(UnkStrAddr);
								}
								for (int i = 0; i < count; i++)
								{
									v11 = *reinterpret_cast<__int64*>(*reinterpret_cast<__int64*>(_PedAddress + 4208 + AddrOff) + 8 * ((i + *reinterpret_cast<int*>(*reinterpret_cast<__int64*>(_PedAddress + 4208 + AddrOff) + 1060) + 1) % 16) + 928);
									if (v11)
									{
										if ((*(int(__fastcall **)(__int64))(*reinterpret_cast<__int64*>(v11) + 24))(v11) == 132)
										{
											v12 = *reinterpret_cast<__int64*>(v11 + 40);
											if (v12)
											{
												if (*reinterpret_cast<short*>(v12 + 52) == 401)
													v5 = true;
											}
										}
									}
								}
							}
							if (v5 && (*reinterpret_cast<int(**)(__int64)>(*reinterpret_cast<__int64*>(PedNmAddress) + 152))(PedNmAddress) != -1)
							{
								reinterpret_cast<void(*)(__int64, __int64, __int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1AA) + BaseFunc + 0x1AE)(PedNmAddress, ScriptDomain::CurrentDomain->PinString(_message).ToInt64(), MessageAddress);//Send Message To Ped
							}
							reinterpret_cast<void(*)(__int64)>(*reinterpret_cast<int*>(BaseFunc + 0x1BB) + BaseFunc + 0x1BF)(MessageAddress);//Free Message Memory
						}
					}
				}

				int _targetHandle;
				String ^_message;
				Dictionary<String ^, Object ^> ^_arguments;
			};
			private ref struct GenericTask : IScriptTask
			{
			public:
				typedef UInt64(*func)(UInt64);
				GenericTask(func pFunc, UInt64 Arg) : _toRun(pFunc), _arg(Arg)
				{
				}
				virtual void Run()
				{
					_res = _toRun(_arg);
				}
			
				UInt64 GetResult()
				{
					return _res;
				}

			private:
				func _toRun;
				UInt64 _arg;
				UInt64 _res;
			};
		}

		static MemoryAccess::MemoryAccess()
		{
			uintptr_t address;

			// Get relative address and add it to the instruction address.
			// 3 bytes equal the size of the opcode and its first argument. 7 bytes are the length of opcode and all its parameters.
			address = FindPattern("\x33\xFF\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x58", "xxx????xxxxx");
			_entityAddressFunc = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x33\xC9\x48\x85\xC0\x74\x3B", "xxx????xxxxxxx");
			_playerAddressFunc = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int *>(address + 3) + address + 7);

			address = FindPattern("\x74\x21\x48\x8B\x48\x20\x48\x85\xC9\x74\x18\x48\x8B\xD6\xE8", "xxxxxxxxxxxxxxx") - 10;
			_ptfxAddressFunc = reinterpret_cast<uintptr_t(*)(int)>(*reinterpret_cast<int*>(address) + address + 4);

			address = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			_addEntityToPoolFunc = reinterpret_cast<int(*)(uintptr_t)>(address - 0x68);

			address = FindPattern("\x48\x8B\xC8\xE8\x00\x00\x00\x00\xF3\x0F\x10\x54\x24\x00\xF3\x0F\x10\x4C\x24\x00\xF3\x0F\x10", "xxxx????xxxxx?xxxxx?xxx");
			_entityPositionFunc = reinterpret_cast<UINT64(*)(UINT64, float *)>(*reinterpret_cast<int *>(address + 4) + address + 8);
			address = FindPattern("\x25\xFF\xFF\xFF\x3F\x89\x44\x24\x38\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x03", "xxxxxxxxxx????xxxxx");
			_entityModel1Func = reinterpret_cast<UINT64(*)(UINT64)>(*reinterpret_cast<int *>(address - 61) + address - 57);
			_entityModel2Func = reinterpret_cast<UINT64(*)(UINT64)>(*reinterpret_cast<int *>(address + 10) + address + 14);

			address = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			_entityPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			_vehiclePoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			_pedPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			_objectPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 3) + address + 7);
			address = FindPattern("\x8B\xF0\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6", "xxxxx????xxxx");
			_pickupObjectPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 5) + address + 9);

			CreateNmMessageFunc = FindPattern("\x33\xDB\x48\x89\x1D\x00\x00\x00\x00\x85\xFF", "xxxxx????xx") - 0x42;
			GiveNmMessageFunc = FindPattern("\x48\x8b\xc4\x48\x89\x58\x08\x48\x89\x68\x10\x48\x89\x70\x18\x48\x89\x78\x20\x41\x55\x41\x56\x41\x57\x48\x83\xec\x20\xe8\x00\x00\x00\x00\x48\x8b\xd8\x48\x85\xc0\x0f", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxx");
			address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmBoolAddress = reinterpret_cast<unsigned char(*)(__int64, __int64, unsigned char)>(address);
			address = FindPattern("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmFloatAddress = reinterpret_cast<unsigned char(*)(__int64, __int64, float)>(address);
			address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmIntAddress = reinterpret_cast<unsigned char(*)(__int64, __int64, int)>(address);
			address = FindPattern("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			SetNmStringAddress = reinterpret_cast<unsigned char(*)(__int64, __int64, __int64)>(address);
			address = FindPattern("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmVec3Address = reinterpret_cast<unsigned char(*)(__int64, __int64, float, float, float)>(address);

			address = FindPattern("\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x18\x89\x54\x24\x10\x56\x57\x41\x56\x48\x83\xEC\x20", "xxxxxxxxxxxxxxxxxxxxxx");
			GetLabelTextByHashFunc = reinterpret_cast<UINT64(*)(UINT64, int)>(reinterpret_cast<int*>(address));
			address = FindPattern("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
			GetLabelTextByHashAddr2 = (*reinterpret_cast<int*>(address + 7) + address + 11);

			address = FindPattern("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
			CheckpointBaseAddr = reinterpret_cast<UINT64(*)()>(*reinterpret_cast<int*>(address - 19) + address - 15);
			CheckpointHandleAddr = reinterpret_cast<UINT64(*)(UINT64, int)>(*reinterpret_cast<int*>(address - 9) + address - 5);
			checkpointPoolAddress = reinterpret_cast<uintptr_t *>(*reinterpret_cast<int *>(address + 17) + address + 21);

			address = FindPattern("\x48\x8B\x0B\x33\xD2\xE8\x00\x00\x00\x00\x89\x03", "xxxxxx????xx");
			_getHashKey = reinterpret_cast<unsigned int(*)(char*, unsigned int)>(*reinterpret_cast<int*>(address + 6) + address + 10);

			address = FindPattern("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05\x00\x00\x00\x00", "xxxxxx????xxxxxxxxx????");
			_writeWorldGravityAddr = reinterpret_cast<float *>(*reinterpret_cast<int *>(address + 6) + address + 10);
			_readWorldGravityAddr = reinterpret_cast<float *>(*reinterpret_cast<int *>(address + 19) + address + 23);

			address = FindPattern("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
			_cursorSpriteAddr = reinterpret_cast<int *>(*reinterpret_cast<int*>(address - 4) + address);

			address = FindPattern("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
			address = address + *reinterpret_cast<int*>(address) + 4;
			_gamePlayCameraAddr = reinterpret_cast<UInt64*>(*reinterpret_cast<int*>(address + 3) + address + 7);
			address = FindPattern("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
			_cameraPoolAddress = reinterpret_cast<UInt64*>(*reinterpret_cast<int*>(address) + address + 4);

			GenerateVehicleModelList();

			_cellEmailBconPtr = IntPtr((void*)_cellEmailBcon);
			_stringPtr = IntPtr((void*)_string);
			_nullString = IntPtr((void*)_nullStr);
		}
		IntPtr MemoryAccess::CellEmailBcon::get()
		{
		return _cellEmailBconPtr;
		}
		IntPtr MemoryAccess::StringPtr::get()
		 {
		return _stringPtr;
		}
		IntPtr MemoryAccess::NullString::get()
		{
			return _nullString;
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

		void MemoryAccess::GenerateVehicleModelList()
		{
			uintptr_t address = FindPattern("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			UINT64 baseFuncAddr = address + *reinterpret_cast<int*>(address)+4;
			modelHashEntries = *reinterpret_cast<PUINT16>(baseFuncAddr + *reinterpret_cast<int*>(baseFuncAddr + 3) + 7);
			modelNum1 = *reinterpret_cast<int*>(*reinterpret_cast<int*>(baseFuncAddr + 0x52) + baseFuncAddr + 0x56);
			modelNum2 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x63) + baseFuncAddr + 0x67);
			modelNum3 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x7A) + baseFuncAddr + 0x7E);
			modelNum4 = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x81) + baseFuncAddr + 0x85);
			modelHashTable = *reinterpret_cast<PUINT64>(*reinterpret_cast<int*>(baseFuncAddr + 0x24) + baseFuncAddr + 0x28);
			int vehClassOff = *reinterpret_cast<int*>(address + 0x31);

	
			HashNode** HashMap = reinterpret_cast<HashNode**>(modelHashTable);
			array<List<int>^> ^hashes = gcnew array<List<int>^>(0x20);
			for (int i = 0; i<0x20; i++)
			{
				hashes[i] = gcnew List<int>();
			}
			for (int i = 0; i < modelHashEntries; i++)
			{
				for (HashNode* cur = HashMap[i]; cur; cur = cur->next)
				{
					UINT16 data = cur->data;
					if ((int)data < modelNum1 && bittest(*reinterpret_cast<int*>(modelNum2 + (4 * data >> 5)), data & 0x1F))
					{
						UINT64 addr1 = modelNum4 + modelNum3 * data;
						if (addr1)
						{
							UINT64 addr2 = *reinterpret_cast<PUINT64>(addr1);
							if (addr2)
							{
								if ((*reinterpret_cast<PBYTE>(addr2 + 157) & 0x1F) == 5)
								{
									hashes[*reinterpret_cast<PBYTE>(addr2 + vehClassOff) & 0x1F]->Add(cur->hash);
								}
							}
						}
					}
				}
			}
			array<ReadOnlyCollection<int> ^> ^result = gcnew array<ReadOnlyCollection<int> ^>(0x20);
			for (int i = 0; i<0x20; i++)
			{
				result[i] = Array::AsReadOnly(hashes[i]->ToArray());
			}
			vehicleModels = Array::AsReadOnly(result);
		}

		bool MemoryAccess::IsModelAPed(int modelHash)
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
						UINT64 addr2 = *reinterpret_cast<PUINT64>(addr1);
						if (addr2)
						{
							return (*reinterpret_cast<PBYTE>(addr2 + 157) & 0x1F) == 6;
						}
					}
				}
			}
			return false;
		}
		int MemoryAccess::GetGameVersion()
		{
			return getGameVersion();
		}

		char MemoryAccess::ReadSByte(IntPtr address)
		{
			const auto data = static_cast<const char *>(address.ToPointer());

			return *data;
		}
		unsigned char MemoryAccess::ReadByte(IntPtr address)
		{
			const auto data = static_cast<const unsigned char *>(address.ToPointer());

			return *data;
		}
		short MemoryAccess::ReadShort(IntPtr address)
		{
			const auto data = static_cast<const short *>(address.ToPointer());

			return *data;
		}
		unsigned short MemoryAccess::ReadUShort(IntPtr address)
		{
			const auto data = static_cast<const unsigned short *>(address.ToPointer());

			return *data;
		}
		int MemoryAccess::ReadInt(IntPtr address)
		{
			const auto data = static_cast<const int *>(address.ToPointer());

			return *data;
		}
		unsigned int MemoryAccess::ReadUInt(IntPtr address)
		{
			const auto data = static_cast<const unsigned int *>(address.ToPointer());

			return *data;
		}
		float MemoryAccess::ReadFloat(IntPtr address)
		{
			const auto data = static_cast<const float *>(address.ToPointer());

			return *data;
		}
		Math::Vector3 MemoryAccess::ReadVector3(IntPtr address)
		{
			const auto data = static_cast<const float *>(address.ToPointer());

			return Math::Vector3(data[0], data[1], data[2]);
		}
		String ^MemoryAccess::ReadString(IntPtr address)
		{
			const auto data = static_cast<const char *>(address.ToPointer());

			return gcnew System::String(data);
		}
		IntPtr MemoryAccess::ReadPtr(IntPtr address)
		{
			const auto data = static_cast<void **>(address.ToPointer());

			return IntPtr(*data);
		}
		Math::Matrix MemoryAccess::ReadMatrix(System::IntPtr address)
		{
			const auto data = static_cast<const Math::Matrix*>(address.ToPointer());

			return Math::Matrix(*data);
		}
		long long MemoryAccess::ReadLong(IntPtr address)
		{
			const auto data = static_cast<const long long *>(address.ToPointer());

			return *data;
		}
		unsigned long long MemoryAccess::ReadULong(IntPtr address)
		{
			const auto data = static_cast<const unsigned long long *>(address.ToPointer());

			return *data;
		}
		void MemoryAccess::WriteSByte(System::IntPtr address, char value)
		{
			const auto data = static_cast<char *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteByte(System::IntPtr address, unsigned char value)
		{
			const auto data = static_cast<unsigned char *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteShort(System::IntPtr address, short value)
		{
			const auto data = static_cast<short *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteUShort(System::IntPtr address, unsigned short value)
		{
			const auto data = static_cast<unsigned short *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteInt(IntPtr address, int value)
		{
			const auto data = static_cast<int *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteUInt(IntPtr address, unsigned int value)
		{
			const auto data = static_cast<unsigned int *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteFloat(IntPtr address, float value)
		{
			const auto data = static_cast<float *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteVector3(IntPtr address, Math::Vector3 value)
		{
			const auto data = static_cast<float *>(address.ToPointer());

			data[0] = value.X;
			data[1] = value.Y;
			data[2] = value.Z;
		}
		void MemoryAccess::WriteMatrix(IntPtr address, Math::Matrix value)
		{
			const auto data = static_cast<float *>(address.ToPointer());

			auto arr = value.ToArray();
			for (int i = 0; i < arr->Length; i++)
			{
				data[i] = arr[i];
			}	
		}
		void MemoryAccess::WriteLong(IntPtr address, long long value)
		{
			const auto data = static_cast<long long *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::WriteULong(IntPtr address, unsigned long long value)
		{
			const auto data = static_cast<unsigned long long *>(address.ToPointer());

			*data = value;
		}
		void MemoryAccess::SetBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit >31)
			{
				throw gcnew ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
			}
			const int mask = 1 << bit;
			
			const auto data = static_cast<int *>(address.ToPointer());
			*data |= mask;
		}
		void MemoryAccess::ClearBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit >31)
			{
				throw gcnew ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
			}
			const int mask = 1 << bit;

			const auto data = static_cast<int *>(address.ToPointer());
			*data &= ~mask;
		}
		bool MemoryAccess::IsBitSet(IntPtr address, int bit)
		{
			if (bit < 0 || bit >31)
			{
				throw gcnew ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
			}
			const int mask = 1 << bit;

			const auto data = static_cast<int *>(address.ToPointer());
			return (*data & mask) != 0;
		}
		unsigned int MemoryAccess::GetHashKey(String^ toHash)
		{
			IntPtr handle = ScriptDomain::CurrentDomain->PinString(toHash);
			return _getHashKey((char*)handle.ToPointer(), 0);
		}
		String ^MemoryAccess::GetGXTEntryByHash(int entryLabelHash)
		{
			const char* entryText = reinterpret_cast<const char*>(GetLabelTextByHashFunc(GetLabelTextByHashAddr2, entryLabelHash));
			if (entryText != nullptr)
			{
				int textLen = static_cast<int>(strlen(entryText));
				array<Byte> ^textBytes = gcnew array<Byte>(textLen);
				Marshal::Copy(IntPtr(const_cast<char *>(entryText)), textBytes, 0, textLen);
				return Text::Encoding::UTF8->GetString(textBytes);
			}
			return String::Empty;
		}

		IntPtr MemoryAccess::GetEntityAddress(int handle)
		{
			return IntPtr((long long)_entityAddressFunc(handle));
		}
		IntPtr MemoryAccess::GetPlayerAddress(int handle)
		{
			return IntPtr((long long)_playerAddressFunc(handle));
		}
		UInt64 _getCheckpointAddress(UInt64 Data)
		{
			int handle = *(int*)(&Data);
			UInt64 addr = MemoryAccess::CheckpointHandleAddr(MemoryAccess::CheckpointBaseAddr(), handle);
			if (addr != 0)
			{
				return (UInt64)((UInt64)(MemoryAccess::checkpointPoolAddress) + 96 * *reinterpret_cast<int *>(addr + 16));
			}
			return 0;
		}
		IntPtr MemoryAccess::GetCheckpointAddress(int handle)
		{
			GenericTask ^task = gcnew GenericTask(_getCheckpointAddress, handle);
			ScriptDomain::CurrentDomain->ExecuteTask(task);
			return IntPtr((long long)task->GetResult());
		}

		IntPtr MemoryAccess::GetPtfxAddress(int handle)
		{
			return IntPtr((long long)_ptfxAddressFunc(handle));
		}

		int MemoryAccess::GetEntityBoneCount(int handle)
		{
			auto fragSkeletonData = GetEntitySkeletonData(handle);
			return fragSkeletonData ? *reinterpret_cast<int*>(fragSkeletonData + 32) : 0;
		}
		
		IntPtr MemoryAccess::GetEntityBoneMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0)//boneIndex cant be negative
				return IntPtr::Zero;

			auto fragSkeletonData = GetEntitySkeletonData(handle);

			if (!fragSkeletonData) return IntPtr::Zero;

			if (boneIndex < *reinterpret_cast<int*>(fragSkeletonData + 32))// boneIndex < max bones?
			{
				return IntPtr((long long)(*(UInt64*)(fragSkeletonData + 24) + (boneIndex * 0x40)));
			}

			return IntPtr::Zero;
		}

		IntPtr MemoryAccess::GetEntityBonePoseAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0)//boneIndex cant be negative
				return IntPtr::Zero;

			auto fragSkeletonData = GetEntitySkeletonData(handle);

			if (!fragSkeletonData) return IntPtr::Zero;

			if (boneIndex < *reinterpret_cast<int*>(fragSkeletonData + 32))// boneIndex < max bones?
			{
				return IntPtr((long long)(*(UInt64*)(fragSkeletonData + 16) + (boneIndex * 0x40)));
			}

			return IntPtr::Zero;
		}

		unsigned long long MemoryAccess::GetEntitySkeletonData(int handle)
		{
			UInt64 MemAddress = _entityAddressFunc(handle);
			UInt64 Addr2 = (*(UInt64(__thiscall **)(__int64))(*(UInt64 *)MemAddress + 88i64))(MemAddress);
			UInt64 Addr3;
			if (!Addr2)
			{
				Addr3 = *(UInt64*)(MemAddress + 80);
				if (!Addr3)
				{
					return 0;
				}
				else
				{
					Addr3 = *(UInt64*)(Addr3 + 40);
				}
			}
			else
			{
				Addr3 = *(UInt64*)(Addr2 + 104);
				if (!Addr3 || !*(UInt64*)(Addr2 + 120))
				{
					return 0;
				}
				else
				{
					Addr3 = *(UInt64*)(Addr3 + 376);
				}
			}
			if (!Addr3)
			{
				return 0;
			}

			return Addr3;
		}

		float MemoryAccess::ReadWorldGravity()
		{
			return *_readWorldGravityAddr;
		}
		void MemoryAccess::WriteWorldGravity(float value)
		{
			*_writeWorldGravityAddr = value;
		}

		int MemoryAccess::ReadCursorSprite()
		{
			return *_cursorSpriteAddr;
		}

		IntPtr MemoryAccess::GetGameplayCameraAddress()
		{
			return IntPtr((long long)*_gamePlayCameraAddr);
		}
		
		IntPtr MemoryAccess::GetCameraAddress(int handle)
		{
			unsigned int index = (unsigned int)(handle >> 8);
			UInt64 poolAddr = *_cameraPoolAddress;
			if(*(Byte *)(index + *(Int64*)(poolAddr + 8)) == (Byte)(handle & 0xFF))
			{
				return IntPtr(*(Int64*)poolAddr + (unsigned int)(index * *(UInt32 *)(poolAddr + 20)));
			}
			return IntPtr::Zero;
			
		}

		array<int> ^MemoryAccess::GetEntityHandles()
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Ped | EntityPoolTask::Type::Object | EntityPoolTask::Type::Vehicle);

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetEntityHandles(Math::Vector3 position, float radius)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Ped | EntityPoolTask::Type::Object | EntityPoolTask::Type::Vehicle);
			task->_position = position;
			task->_radiusSquared = radius * radius;
			task->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetVehicleHandles(Math::Vector3 position, float radius, array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Vehicle);
			task->_position = position;
			task->_radiusSquared = radius * radius;
			task->_posCheck = true;
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPedHandles(Math::Vector3 position, float radius, array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Ped);
			task->_position = position;
			task->_radiusSquared = radius * radius;
			task->_posCheck = true;
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPropHandles(Math::Vector3 position, float radius, array<int> ^modelhashes)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::Object);
			task->_position = position;
			task->_radiusSquared = radius * radius;
			task->_posCheck = true;
			task->_modelHashes = modelhashes;
			task->_modelCheck = modelhashes != nullptr && modelhashes->Length > 0;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		struct checkpoint
		{
			char padding[0xC];
			int handle;
			char padding1[0x08];
			checkpoint* next;
		};
		UInt64 _getCheckpoinHandles(UInt64 ArrayPtr)
		{
			int* handles = (int*)ArrayPtr;
			UInt64 count = 0;
			for (checkpoint* item = *reinterpret_cast<checkpoint**>(MemoryAccess::CheckpointBaseAddr() + 48); item  && count < 64; item = item->next)
			{
				handles[count++] = item->handle;
			}
			return count;
		}
		array<int> ^MemoryAccess::GetCheckpointHandles()
		{
			int Handles[64];
			GenericTask ^task = gcnew GenericTask(_getCheckpoinHandles, (UInt64)Handles);
			ScriptDomain::CurrentDomain->ExecuteTask(task);
			int count = (int)task->GetResult();
			array<int>^ data_array = gcnew array<int>(count);
			pin_ptr<int> ptrBuffer = &data_array[0];
			memcpy(ptrBuffer, Handles, count * 4);
			return data_array;
		}
		array<int> ^MemoryAccess::GetPickupObjectHandles()
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::PickupObject);

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}
		array<int> ^MemoryAccess::GetPickupObjectHandles(Math::Vector3 position, float radius)
		{
			auto task = gcnew EntityPoolTask(EntityPoolTask::Type::PickupObject);
			task->_position = position;
			task->_radiusSquared = radius * radius;
			task->_posCheck = true;

			ScriptDomain::CurrentDomain->ExecuteTask(task);

			return task->_handles->ToArray();
		}

		int MemoryAccess::GetNumberOfVehicles()
		{
			if (*_vehiclePoolAddress)
			{
				VehiclePool* pool = *reinterpret_cast<VehiclePool**>(*_vehiclePoolAddress);
				return pool->itemCount;
			}
			return 0;
		}

		void MemoryAccess::SendEuphoriaMessage(int targetHandle, String ^message, Dictionary<String ^, Object ^> ^arguments)
		{
			auto task = gcnew EuphoriaMessageTask(targetHandle, message, arguments);

			ScriptDomain::CurrentDomain->ExecuteTask(task);
		}

		int MemoryAccess::CreateTexture(System::String ^filename)
		{
			return createTexture(static_cast<const char *>(ScriptDomain::CurrentDomain->PinString(filename).ToPointer()));
		}
		void MemoryAccess::DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, Drawing::Color color)
		{
			drawTexture(id, index, level, time, sizeX, sizeY, centerX, centerY, posX, posY, rotation, scaleFactor, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
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
