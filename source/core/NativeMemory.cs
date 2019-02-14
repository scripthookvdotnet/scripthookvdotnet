using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.Marshal;

namespace GTA
{
	namespace Native
	{
		internal enum ModelInfoClassType
		{
			Invalid = 0,
			Object = 1,
			Mlo = 2,
			Time = 3,
			Weapon = 4,
			Vehicle = 5,
			Ped = 6
		}
		internal enum VehicleStructClassType
		{
			Invalid = -1,
			Automobile = 0x0,
			Plane = 0x1,
			Trailer = 0x2,
			QuadBike = 0x3,
			SubmarineCar = 0x5,
			AmphibiousAutomobile = 0x6,
			AmphibiousQuadBike = 0x7,
			Heli = 0x8,
			Blimp = 0x9,
			Autogyro = 0xA,
			Bike = 0xB,
			Bycicle = 0xC,
			Boat = 0xD,
			Train = 0xE,
			Submarine = 0xF
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct EntityPool
		{
			[FieldOffset(0x10)]
			internal uint num1;
			[FieldOffset(0x20)]
			internal uint num2;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal bool IsFull()
			{
				return num1 - (num2 & 0x3FFFFFFF) <= 256;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct VehiclePool
		{
			[FieldOffset(0x00)]
			internal ulong* poolAddress;
			[FieldOffset(0x08)]
			internal uint size;
			[FieldOffset(0x30)]
			internal uint* bitArray;
			[FieldOffset(0x60)]
			internal uint itemCount;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal bool IsValid(uint i)
			{
				return ((bitArray[i >> 5] >> ((int)i & 0x1F)) & 1) != 0;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal ulong GetAddress(uint i)
			{
				return poolAddress[i];
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct GenericPool
		{
			[FieldOffset(0x00)]
			public ulong poolStartAddress;
			[FieldOffset(0x08)]
			public IntPtr byteArray;
			[FieldOffset(0x10)]
			public uint size;
			[FieldOffset(0x14)]
			public uint itemSize;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsValid(uint index)
			{
				return Mask(index) != 0;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ulong GetAddress(uint index)
			{
				return ((Mask(index) & (poolStartAddress + index * itemSize)));
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private ulong Mask(uint index)
			{
				unsafe
				{
					byte* byteArrayPtr = (byte*)byteArray.ToPointer();
					long num1 = byteArrayPtr[index] & 0x80;
					return (ulong)(~((num1 | -num1) >> 63));
				}
			}
		}

		internal unsafe class EntityPoolTask : IScriptTask
		{
			#region Fields
			internal Type _type;
			internal List<int> _handles = new List<int>();
			internal bool _posCheck, _modelCheck;
			internal Math.Vector3 _position;
			internal float _radiusSquared;
			internal int[] _modelHashes;
			#endregion

			internal enum Type
			{
				Ped = 1,
				Object = 2,
				Vehicle = 4,
				PickupObject = 8
			}

			internal EntityPoolTask(Type type)
			{
				_type = type;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private bool CheckEntity(ulong address)
			{
				unsafe
				{
					byte* unsafePtr = (byte*)address;

					if (_posCheck)
					{
						float[] position = new float[3];

						fixed (float* posPtr = &position[0])
						{
							MemoryAccess.EntityPositionFunc(address, posPtr);
						}

						if (_position.DistanceToSquared(new Math.Vector3(position[0], position[1], position[2])) > _radiusSquared)
						{
							return false;
						}
					}

					if (_modelCheck)
					{
						uint v0 = *(uint*)(MemoryAccess.EntityModel1Func(*(ulong*)(address + 32)));
						uint v1 = v0 & 0xFFFF;
						uint v2 = ((v1 ^ v0) & 0x0FFF0000 ^ v1) & 0xDFFFFFFF;
						uint v3 = ((v2 ^ v0) & 0x10000000 ^ v2) & 0x3FFFFFFF;
						ulong v5 = MemoryAccess.EntityModel2Func((ulong)(&v3));


						if (v5 == 0)
						{
							return false;
						}
						foreach (int hash in _modelHashes)

						{
							if (*(int*)(v5 + 24) == hash)
							{
								return true;
							}
						}
						return false;
					}
					return true;
				}
			}

			public void Run()
			{
				if (*MemoryAccess._entityPoolAddress == 0)
				{
					return;
				}
				EntityPool* entityPool = (EntityPool*)(*MemoryAccess._entityPoolAddress);

				if (_type.HasFlag(Type.Vehicle))
				{
					if (*MemoryAccess._vehiclePoolAddress != 0)
					{
						VehiclePool* vehiclePool = *(VehiclePool**)(*MemoryAccess._vehiclePoolAddress);

						for (uint i = 0; i < vehiclePool->size; i++)
						{
							if (entityPool->IsFull())
							{
								break;
							}
							if (vehiclePool->IsValid(i))
							{
								ulong address = vehiclePool->GetAddress(i);
								if (address != 0 && CheckEntity(address))
								{
									_handles.Add(MemoryAccess.AddEntityToPoolFunc(address));
								}
							}
						}
					}
				}

				if (_type.HasFlag(Type.Ped))
				{
					if (*MemoryAccess._pedPoolAddress != 0)
					{
						GenericPool* pedPool = (GenericPool*)(*MemoryAccess._pedPoolAddress);

						for (uint i = 0; i < pedPool->size; i++)
						{
							if (entityPool->IsFull())
							{
								break;
							}
							if (pedPool->IsValid(i))
							{
								ulong address = pedPool->GetAddress(i);
								if (address != 0 && CheckEntity(address))
								{
									_handles.Add(MemoryAccess.AddEntityToPoolFunc(address));
								}
							}
						}
					}
				}

				if (_type.HasFlag(Type.Object))
				{
					if (*MemoryAccess._objectPoolAddress != 0)
					{
						GenericPool* propPool = (GenericPool*)(*MemoryAccess._objectPoolAddress);

						for (uint i = 0; i < propPool->size; i++)
						{
							if (entityPool->IsFull())
							{
								break;
							}
							if (propPool->IsValid(i))
							{
								ulong address = propPool->GetAddress(i);
								if (address != 0 && CheckEntity(address))
								{
									_handles.Add(MemoryAccess.AddEntityToPoolFunc(address));
								}
							}
						}
					}
				}

				if (_type.HasFlag(Type.PickupObject))
				{
					if (*MemoryAccess._pickupObjectPoolAddress != 0)
					{
						GenericPool* pickupPool = (GenericPool*)(*MemoryAccess._pickupObjectPoolAddress);

						for (uint i = 0; i < pickupPool->size; i++)
						{
							if (entityPool->IsFull())
							{
								break;
							}
							if (pickupPool->IsValid(i))
							{
								ulong address = pickupPool->GetAddress(i);
								if (address != 0)
								{
									if (_posCheck)
									{
										float* position = (float*)(address + 0x90);
										if (_position.DistanceToSquared(new Math.Vector3(position[0], position[1], position[2])) > _radiusSquared)
										{
											continue;
										}
									}
									_handles.Add(MemoryAccess.AddEntityToPoolFunc(address));
								}
							}
						}
					}
				}

			}


		}

		internal unsafe class EuphoriaMessageTask : IScriptTask
		{
			int _targetHandle;
			string _message;
			Dictionary<string, object> _arguments;

			#region Delegate Fields For Function Ponters
			//GetDelegateForFunctionPointer doesn't allow generic types such as Func and Action
			internal delegate void ActionUlongDelegate(ulong T);
			internal delegate int FuncUlongIntDelegate(ulong T);
			internal delegate ulong FuncUlongUlongDelegate(ulong T);
			internal delegate ulong FuncUlongUlongIntUlongDelegate(ulong T1, ulong T2, int T3);
			internal delegate void SendMessageToPedDelegate(ulong PedNmAddress, IntPtr messagePtr, ulong MessageAddress);
			internal delegate void FreeMessageMemoryDelegate(ulong MessageAddress);
			#endregion

			internal EuphoriaMessageTask(int target, string message, Dictionary<string, object> arguments)
			{
				_targetHandle = target;
				_message = message;
				_arguments = arguments;
			}

			public void Run()
			{
				byte* NativeFunc = (byte*)MemoryAccess.CreateNmMessageFuncAddress;
				ulong MessageAddress = GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(new IntPtr((long)(*(int*)(NativeFunc + 0x22) + NativeFunc + 0x26)))(4632);

				if (MessageAddress == 0)
				{
					return;
				}

				GetDelegateForFunctionPointer<FuncUlongUlongIntUlongDelegate>(new IntPtr((long)((*(int*)(NativeFunc + 0x3C)) + NativeFunc + 0x40)))(MessageAddress, MessageAddress + 24, 64);

				foreach (var argument in _arguments)
				{
					IntPtr name = ScriptDomain.CurrentDomain.PinString(argument.Key);

					if (argument.Value.GetType() == typeof(bool))
					{
						MemoryAccess.SetNmBoolAddress(MessageAddress, name, (bool)argument.Value);
					}
					if (argument.Value.GetType() == typeof(int))
					{
						MemoryAccess.SetNmIntAddress(MessageAddress, name, (int)argument.Value);
					}
					if (argument.Value.GetType() == typeof(float))
					{
						MemoryAccess.SetNmFloatAddress(MessageAddress, name, (float)argument.Value);
					}
					if (argument.Value.GetType() == typeof(Math.Vector3))
					{
						var value = (Math.Vector3)argument.Value;

						MemoryAccess.SetNmVec3Address(MessageAddress, name, value.X, value.Y, value.Z);
					}
					if (argument.Value.GetType() == typeof(string))
					{
						MemoryAccess.SetNmStringAddress(MessageAddress, name, ScriptDomain.CurrentDomain.PinString((string)argument.Value));
					}
				}

				byte* BaseFunc = (byte*)MemoryAccess.GiveNmMessageFuncAddress;
				byte* ByteAddr = (*(int*)(BaseFunc + 0xBC) + BaseFunc + 0xC0);
				byte* UnkStrAddr = (*(int*)(BaseFunc + 0xCE) + BaseFunc + 0xD2);
				byte* _PedAddress = (byte*)MemoryAccess.GetEntityAddress(_targetHandle).ToPointer();
				byte* PedNmAddress;
				bool v5 = false;
				byte v7;
				ulong v11;
				ulong v12;

				if (_PedAddress == null)
					return;
				if (*(ulong*)(_PedAddress + 48) == 0)
					return;

				PedNmAddress = (byte*)GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(new IntPtr((long)(*(ulong*)(*(ulong*)(_PedAddress) + 88))))((ulong)_PedAddress);

				int MinHealthOffset = Game.Version < GameVersion.v1_0_877_1_Steam ? *(int*)(BaseFunc + 78) : *(int*)(BaseFunc + 157 + *(int*)(BaseFunc + 76));

				if (*(ulong*)(_PedAddress + 48) == (ulong)PedNmAddress && *(float*)(_PedAddress + MinHealthOffset) <= *(float*)(_PedAddress + 640))
				{

					if (GetDelegateForFunctionPointer<FuncUlongIntDelegate>(new IntPtr((long)*(ulong*)(*(ulong*)PedNmAddress + 152)))((ulong)PedNmAddress) != -1)
					{
						ulong PedIntelligenceAddr = *(ulong*)(_PedAddress + *(int*)(BaseFunc + 147));

						// check whether the ped is currently performing the 'CTaskNMScriptControl' task
						if (*(short*)(GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0xA2) + BaseFunc + 0xA6)))(*(ulong*)(PedIntelligenceAddr + 864)) + 52) == 401)
						{
							v5 = true;
						}
						else
						{
							v7 = *ByteAddr;
							if (v7 != 0)
							{
								GetDelegateForFunctionPointer<ActionUlongDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0xD3) + BaseFunc + 0xD7)))((ulong)UnkStrAddr);
								v7 = *ByteAddr;
							}
							int count = *(int*)(PedIntelligenceAddr + 1064);
							if (v7 != 0)
							{
								GetDelegateForFunctionPointer<ActionUlongDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0xF0) + BaseFunc + 0xF4)))((ulong)UnkStrAddr);
							}
							for (int i = 0; i < count; i++)
							{
								v11 = *(ulong*)((byte*)PedIntelligenceAddr + 8 * ((i + *(int*)(PedIntelligenceAddr + 1060) + 1) % 16) + 928);
								if (v11 != 0)
								{
									if (GetDelegateForFunctionPointer<FuncUlongIntDelegate>(new IntPtr((long)*(ulong*)(*(ulong*)v11 + 24)))(v11) == 132)
									{
										v12 = *(ulong*)(v11 + 40);
										if (v12 != 0)
										{
											if (*(short*)(v12 + 52) == 401)
												v5 = true;
										}
									}
								}
							}
						}
						if (v5 && GetDelegateForFunctionPointer<FuncUlongIntDelegate>(new IntPtr((long)*(ulong*)(*(ulong*)PedNmAddress + 152)))((ulong)PedNmAddress) != -1)
						{
							IntPtr messagePtr = ScriptDomain.CurrentDomain.PinString(_message);
							GetDelegateForFunctionPointer<SendMessageToPedDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0x1AA) + BaseFunc + 0x1AE)))((ulong)PedNmAddress, messagePtr, MessageAddress);
						}
						GetDelegateForFunctionPointer<FreeMessageMemoryDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0x1BB) + BaseFunc + 0x1BF)))(MessageAddress);
					}
				}
			}
		}


		internal class GenericTask : IScriptTask
		{
			#region Fields
			private delegate ulong func(ulong address);
			private func _funcDelegate;
			private ulong _arg;
			private ulong _result;
			#endregion

			internal GenericTask(ulong funcAddress, ulong arg) : this(new IntPtr((long)funcAddress), arg)
			{
			}
			internal GenericTask(IntPtr funcAddress, ulong arg)
			{
				_funcDelegate = GetDelegateForFunctionPointer<func>(funcAddress);
				_arg = arg;
			}
			public void Run()
			{
				_result = _funcDelegate(_arg);
			}

			ulong GetResult()
			{
				return _result;
			}

		}

		internal unsafe static class MemoryAccess
		{
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?getGameVersion@@YA?AW4eGameVersion@@XZ")]
			static extern int _GetGameVersion();
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?createTexture@@YAHPEBD@Z")]
			static extern int CreateTexture(IntPtr fileNamePtr);
			[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?drawTexture@@YAXHHHHMMMMMMMMMMMM@Z")]
			static extern int DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation,
											float scaleFactor, float colorR, float colorG, float colorB, float colorA);

			#region Fields
			internal static ulong* checkpointPoolAddress;
			internal static float* _readWorldGravityAddr;
			internal static float* _writeWorldGravityAddr;
			internal static ulong* _gamePlayCameraAddr;
			internal static int* _cursorSpriteAddr;
			internal static float* _timeScaleAddress;
			internal static ulong* _entityPoolAddress;
			internal static ulong* _vehiclePoolAddress;
			internal static ulong* _pedPoolAddress;
			internal static ulong* _objectPoolAddress;
			internal static ulong* _cameraPoolAddress;
			internal static ulong* _pickupObjectPoolAddress;
			internal static ulong GetLabelTextByHashAddr;
			internal static ulong CreateNmMessageFuncAddress;
			internal static ulong GiveNmMessageFuncAddress;
			internal static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModels => vehicleModels;

			private static ulong modelHashTable, modelNum2, modelNum3, modelNum4;
			private static int modelNum1;
			private static int handlingIndexOffsetInModelInfo;
			private static uint vehClassOff;
			private static ushort modelHashEntries;
			private static ReadOnlyCollection<ReadOnlyCollection<int>> vehicleModels;
			#endregion

			#region Delegate Fields For Function Ponters
			internal delegate uint GetHashKeyDelegate(IntPtr stringPtr, uint initialHash);
			internal delegate ulong EntityAddressFuncDelegate(int handle);
			internal delegate ulong PlayerAddressFuncDelegate(int handle);
			internal delegate ulong PtfxAddressFuncDelegate(int handle);
			internal delegate int AddEntityToPoolFuncDelegate(ulong address);
			internal delegate ulong EntityPositionFuncDelegate(ulong address, float* position);
			internal delegate ulong EntityModel1FuncDelegate(ulong address);
			internal delegate ulong EntityModel2FuncDelegate(ulong address);
			internal delegate ulong GetHandlingDataByIndexDelegate(int index);
			internal delegate ulong GetHandlingDataByHashDelegate(IntPtr hashAddress);
			internal delegate byte SetNmBoolAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, [MarshalAs(UnmanagedType.I1)] bool value);
			internal delegate byte SetNmIntAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, int value);
			internal delegate byte SetNmFloatAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float value);
			internal delegate byte SetNmVec3AddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float x, float y, float z);
			internal delegate byte SetNmStringAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, IntPtr stringPtr);
			internal delegate ulong CheckpointHandleAddrDelegate(ulong baseAddr, int handle);
			internal delegate ulong GetCheckpointBaseAddrDelegate();
			internal delegate ulong GetLabelTextByHashFuncDelegate(ulong address, int labelHash);
			internal delegate ulong FuncUlongUlongDelegate(ulong T);

			internal static GetHashKeyDelegate _getHashKey;
			internal static EntityAddressFuncDelegate EntityAddressFunc;
			internal static PlayerAddressFuncDelegate PlayerAddressFunc;
			internal static PtfxAddressFuncDelegate PtfxAddressFunc;
			internal static AddEntityToPoolFuncDelegate AddEntityToPoolFunc;
			internal static EntityPositionFuncDelegate EntityPositionFunc;
			internal static EntityModel1FuncDelegate EntityModel1Func;
			internal static EntityModel2FuncDelegate EntityModel2Func;
			internal static GetHandlingDataByIndexDelegate GetHandlingDataByIndex;
			internal static GetHandlingDataByHashDelegate GetHandlingDataByHash;
			internal static SetNmBoolAddressDelegate SetNmBoolAddress;
			internal static SetNmIntAddressDelegate SetNmIntAddress;
			internal static SetNmFloatAddressDelegate SetNmFloatAddress;
			internal static SetNmVec3AddressDelegate SetNmVec3Address;
			internal static SetNmStringAddressDelegate SetNmStringAddress;
			internal static CheckpointHandleAddrDelegate CheckpointHandleAddr;
			internal static GetCheckpointBaseAddrDelegate CheckpointBaseAddr;
			internal static GetLabelTextByHashFuncDelegate GetLabelTextByHashFunc;
			#endregion

			static MemoryAccess()
			{
				byte* address;

				// Get relative address and add it to the instruction address.
				address = FindPattern("\xE8\x00\x00\x00\x00\x48\x8B\xD8\x48\x85\xC0\x74\x2E\x48\x83\x3D", "x????xxxxxxxxxxx");
				EntityAddressFunc = GetDelegateForFunctionPointer<EntityAddressFuncDelegate>(new IntPtr(*(int*)(address + 1) + address + 5));

				address = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x1C\x8A\x88", "xxx????xxxxxxx");
				PlayerAddressFunc = GetDelegateForFunctionPointer<PlayerAddressFuncDelegate>(new IntPtr(*(int*)(address + 3) + address + 7));

				address = FindPattern("\x74\x21\x48\x8B\x48\x20\x48\x85\xC9\x74\x18\x48\x8B\xD6\xE8", "xxxxxxxxxxxxxxx") - 10;
				PtfxAddressFunc = GetDelegateForFunctionPointer<PtfxAddressFuncDelegate>(new IntPtr(*(int*)(address) + address + 4));

				address = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
				AddEntityToPoolFunc = GetDelegateForFunctionPointer<AddEntityToPoolFuncDelegate>(new IntPtr(address - 0x68));

				address = FindPattern("\x48\x8B\xDA\xE8\x00\x00\x00\x00\xF3\x0F\x10\x44\x24", "xxxx????xxxxx");
				EntityPositionFunc = GetDelegateForFunctionPointer<EntityPositionFuncDelegate>(new IntPtr((address - 6)));
				address = FindPattern("\x0F\x85\x00\x00\x00\x00\x48\x8B\x4B\x20\xE8\x00\x00\x00\x00\x48\x8B\xC8", "xx????xxxxx????xxx");
				EntityModel1Func = GetDelegateForFunctionPointer<EntityModel1FuncDelegate>(new IntPtr((*(int*)address + 11) + address + 15));
				address = FindPattern("\x45\x33\xC9\x3B\x05", "xxxxx");
				EntityModel2Func = GetDelegateForFunctionPointer<EntityModel2FuncDelegate>(new IntPtr(address - 0x46));

				address = FindPattern("\x0F\x84\x00\x00\x00\x00\x8B\x8B\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xBA\x09\x00\x00\x00", "xx????xx????x????xxxxx");
				GetHandlingDataByIndex = GetDelegateForFunctionPointer<GetHandlingDataByIndexDelegate>(new IntPtr(*(int*)(address + 13) + address + 17));
				handlingIndexOffsetInModelInfo = *(int*)(address + 8);
				address = FindPattern("\xE8\x00\x00\x00\x00\x48\x85\xC0\x75\x5A\xB2\x01", "x????xxxxxxx");
				GetHandlingDataByHash = GetDelegateForFunctionPointer<GetHandlingDataByHashDelegate>(new IntPtr(*(int*)(address + 1) + address + 5));

				address = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
				_entityPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
				address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
				_vehiclePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
				address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
				_pedPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
				address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
				_objectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
				address = FindPattern("\x4C\x8B\x05\x00\x00\x00\x00\x40\x8A\xF2\x8B\xE9", "xxx????xxxxx");
				_pickupObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

				CreateNmMessageFuncAddress = (ulong)FindPattern("\x33\xDB\x48\x89\x1D\x00\x00\x00\x00\x85\xFF", "xxxxx????xx") - 0x42;
				GiveNmMessageFuncAddress = (ulong)FindPattern("\x48\x8b\xc4\x48\x89\x58\x08\x48\x89\x68\x10\x48\x89\x70\x18\x48\x89\x78\x20\x41\x55\x41\x56\x41\x57\x48\x83\xec\x20\xe8\x00\x00\x00\x00\x48\x8b\xd8\x48\x85\xc0\x0f", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxx");
				address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
				SetNmBoolAddress = GetDelegateForFunctionPointer<SetNmBoolAddressDelegate>(new IntPtr(address));
				address = FindPattern("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
				SetNmFloatAddress = GetDelegateForFunctionPointer<SetNmFloatAddressDelegate>(new IntPtr(address));
				address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
				SetNmIntAddress = GetDelegateForFunctionPointer<SetNmIntAddressDelegate>(new IntPtr(address));
				address = FindPattern("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
				SetNmStringAddress = GetDelegateForFunctionPointer<SetNmStringAddressDelegate>(new IntPtr(address));
				address = FindPattern("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
				SetNmVec3Address = GetDelegateForFunctionPointer<SetNmVec3AddressDelegate>(new IntPtr(address));

				address = FindPattern("\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x18\x89\x54\x24\x10\x56\x57\x41\x56\x48\x83\xEC\x20", "xxxxxxxxxxxxxxxxxxxxxx");
				GetLabelTextByHashFunc = GetDelegateForFunctionPointer<GetLabelTextByHashFuncDelegate>(new IntPtr(address));
				address = FindPattern("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
				GetLabelTextByHashAddr = (ulong)(*(int*)(address + 7) + address + 11);

				address = FindPattern("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
				CheckpointBaseAddr = GetDelegateForFunctionPointer<GetCheckpointBaseAddrDelegate>(new IntPtr(*(int*)(address - 19) + address - 15));
				CheckpointHandleAddr = GetDelegateForFunctionPointer<CheckpointHandleAddrDelegate>(new IntPtr(*(int*)(address - 9) + address - 5));
				checkpointPoolAddress = (ulong*)(*(int*)(address + 17) + address + 21);

				address = FindPattern("\x48\x8B\x0B\x33\xD2\xE8\x00\x00\x00\x00\x89\x03", "xxxxxx????xx");
				_getHashKey = GetDelegateForFunctionPointer<GetHashKeyDelegate>(new IntPtr(*(int*)(address + 6) + address + 10));

				address = FindPattern("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05\x00\x00\x00\x00", "xxxxxx????xxxxxxxxx????");
				_writeWorldGravityAddr = (float*)(*(int*)(address + 6) + address + 10);
				_readWorldGravityAddr = (float*)(*(int*)(address + 19) + address + 23);

				address = FindPattern("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
				_cursorSpriteAddr = (int*)(*(int*)(address - 4) + address);

				address = FindPattern("\xF3\x0F\x10\x0D\x00\x00\x00\x00\x41\x0F\x2F\xCB\x0F\x83", "xxxx????xxxxxx");
				var timeScaleArrayAddress = (float*)(*(int*)(address + 4) + address + 8);
				if (timeScaleArrayAddress != null)
				{
					// SET_TIME_SCALE changes the 3rd element, so obtain the address of it
					_timeScaleAddress = timeScaleArrayAddress + 2;
				}

				address = FindPattern("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
				address = address + *(int*)(address) + 4;
				_gamePlayCameraAddr = (ulong*)(*(int*)(address + 3) + address + 7);
				address = FindPattern("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
				_cameraPoolAddress = (ulong*)(*(int*)(address) + address + 4);

				address = FindPattern("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
				byte* baseFuncAddr = address + *(int*)(address) + 4;
				modelHashEntries = *(ushort*)(baseFuncAddr + *(int*)(baseFuncAddr + 3) + 7);
				modelNum1 = *(int*)(*(int*)(baseFuncAddr + 0x52) + baseFuncAddr + 0x56);
				modelNum2 = *(ulong*)(*(int*)(baseFuncAddr + 0x63) + baseFuncAddr + 0x67);
				modelNum3 = *(ulong*)(*(int*)(baseFuncAddr + 0x7A) + baseFuncAddr + 0x7E);
				modelNum4 = *(ulong*)(*(int*)(baseFuncAddr + 0x81) + baseFuncAddr + 0x85);
				modelHashTable = *(ulong*)(*(int*)(baseFuncAddr + 0x24) + baseFuncAddr + 0x28);
				vehClassOff = *(uint*)(address + 0x31);

				GenerateVehicleModelList();

			}
			internal static IntPtr CellEmailBcon => StringToCoTaskMemUTF8("CELL_EMAIL_BCON");
			internal static IntPtr StringPtr => StringToCoTaskMemUTF8("STRING");
			internal static IntPtr NullString => StringToCoTaskMemUTF8(String.Empty);

			[StructLayout(LayoutKind.Sequential)]
			internal unsafe struct HashNode
			{
				internal int hash;
				internal ushort data;
				internal ushort padding;
				internal HashNode* next;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static bool BitTest(int data, byte index)
			{
				return (data & (1 << index)) != 0;
			}

			private static IntPtr FindCModelInfo(int modelHash)
			{
				unsafe
				{
					HashNode** HashMap = (HashNode**)(modelHashTable);
					for (HashNode* cur = HashMap[(uint)(modelHash) % modelHashEntries]; cur != null; cur = cur->next)
					{
						if (cur->hash != modelHash)
						{
							continue;
						}

						ushort data = cur->data;
						if (data < modelNum1 && BitTest(*(int*)(modelNum2 + (ulong)(4 * data >> 5)), (byte)(data & 0x1F)))

						{
							ulong addr1 = modelNum4 + modelNum3 * data;
							if (addr1 != 0)
							{
								long* address = (long*)(*(ulong*)(addr1));
								return new IntPtr(address);
							}
						}
					}
				}

				return IntPtr.Zero;
			}
			private static ModelInfoClassType GetModelInfoClass(IntPtr address)
			{
				unsafe
				{
					if (address != IntPtr.Zero)
					{
						return ((ModelInfoClassType)((*(byte*)((ulong)address.ToInt64() + 157) & 0x1F)));
					}

					return ModelInfoClassType.Invalid;
				}
			}
			private static VehicleStructClassType GetVehicleStructClass(IntPtr modelInfoAddress)
			{
				unsafe
				{
					if (GetModelInfoClass(modelInfoAddress) == ModelInfoClassType.Vehicle)
					{
						return (VehicleStructClassType)(*(int*)((ulong)modelInfoAddress.ToInt64() + 792));
					}

					return VehicleStructClassType.Invalid;
				}
			}

			private static unsafe void GenerateVehicleModelList()
			{
				HashNode** HashMap = (HashNode**)(modelHashTable);
				List<int>[] hashes = new List<int>[0x20];
				for (int i = 0; i < 0x20; i++)
				{
					hashes[i] = new List<int>();
				}
				for (int i = 0; i < modelHashEntries; i++)
				{
					for (HashNode* cur = HashMap[i]; cur != null; cur = cur->next)
					{
						ushort data = cur->data;
						bool bitTest = BitTest(*(int*)(modelNum2 + (uint)(4 * data >> 5)), (byte)(data & 0x1F));
						if (data < modelNum1 && bitTest)
						{
							ulong addr1 = modelNum4 + modelNum3 * data;
							if (addr1 != 0)
							{
								ulong addr2 = *(ulong*)(addr1);
								if (addr2 != 0)
								{
									if ((*(byte*)(addr2 + 157) & 0x1F) == 5)
									{
										hashes[*(byte*)(addr2 + vehClassOff) & 0x1F].Add(cur->hash);
									}
								}
							}
						}
					}
				}
				ReadOnlyCollection<int>[] result = new ReadOnlyCollection<int>[0x20];
				for (int i = 0; i < 0x20; i++)
				{
					result[i] = Array.AsReadOnly(hashes[i].ToArray());
				}

				vehicleModels = Array.AsReadOnly(result);
			}

			internal static bool IsModelAPed(int modelHash)
			{
				unsafe
				{
					IntPtr modelInfo = FindCModelInfo(modelHash);

					if (modelInfo != IntPtr.Zero)
					{
						return GetModelInfoClass(modelInfo) == ModelInfoClassType.Ped;
					}
					return false;
				}
			}
			internal static bool IsModelAnAmphibiousQuadBike(int modelHash)
			{
				unsafe
				{
					IntPtr modelInfo = FindCModelInfo(modelHash);

					if (modelInfo != IntPtr.Zero)
					{
						return GetVehicleStructClass(modelInfo) == VehicleStructClassType.AmphibiousQuadBike;
					}

					return false;
				}
			}
			internal static bool IsModelABlimp(int modelHash)
			{
				unsafe
				{
					IntPtr modelInfo = FindCModelInfo(modelHash);

					if (modelInfo != IntPtr.Zero)
					{
						return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Blimp;
					}

					return false;
				}
			}
			internal static bool IsModelATrailer(int modelHash)
			{
				unsafe
				{
					IntPtr modelInfo = FindCModelInfo(modelHash);

					if (modelInfo != IntPtr.Zero)
					{
						return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Trailer;
					}

					return false;
				}
			}
			internal static IntPtr GetHandlingDataByModelHash(int modelHash)
			{
				unsafe
				{
					IntPtr modelInfo = FindCModelInfo(modelHash);

					if (modelInfo != IntPtr.Zero && GetModelInfoClass(modelInfo) == ModelInfoClassType.Vehicle)
					{
						int handlingIndex = *(int*)(modelInfo + handlingIndexOffsetInModelInfo).ToPointer();
						return new IntPtr((long)GetHandlingDataByIndex(handlingIndex));
					}

					return IntPtr.Zero;
				}
			}
			internal static IntPtr GetHandlingDataByHandlingNameHash(int handlingNameHash)
			{
				unsafe
				{
					return new IntPtr((long)GetHandlingDataByHash(new IntPtr(&handlingNameHash)));
				}
			}
			internal static int GetGameVersion()
			{
				return _GetGameVersion();
			}

			internal static sbyte ReadSByte(IntPtr address)
			{
				unsafe
				{
					var data = (sbyte*)address.ToPointer();

					return *data;
				}
			}
			internal static byte ReadByte(IntPtr address)
			{
				unsafe
				{
					var data = (byte*)(address.ToPointer());

					return *data;
				}
			}
			internal static short ReadShort(IntPtr address)
			{
				unsafe
				{
					var data = (short*)(address.ToPointer());

					return *data;
				}
			}
			internal static ushort ReadUShort(IntPtr address)
			{
				unsafe
				{
					var data = (ushort*)(address.ToPointer());

					return *data;
				}
			}
			internal static int ReadInt(IntPtr address)
			{
				unsafe
				{
					var data = (int*)(address.ToPointer());

					return *data;
				}
			}
			internal static uint ReadUInt(IntPtr address)
			{
				unsafe
				{
					var data = (uint*)(address.ToPointer());

					return *data;
				}
			}
			internal static float ReadFloat(IntPtr address)
			{
				unsafe
				{
					var data = (float*)(address.ToPointer());

					return *data;
				}
			}
			internal static Math.Vector3 ReadVector3(IntPtr address)
			{
				unsafe
				{
					var data = (float*)(address.ToPointer());

					return new Math.Vector3(data[0], data[1], data[2]);
				}
			}
			internal static String ReadString(IntPtr address)
			{
				return PtrToStringUTF8(address);
			}
			internal static IntPtr ReadPtr(IntPtr address)
			{
				unsafe
				{
					var data = (void**)(address.ToPointer());

					return new IntPtr(*data);
				}
			}
			internal static Math.Matrix ReadMatrix(IntPtr address)
			{
				unsafe
				{
					var data = (Math.Matrix*)(address.ToPointer());

					return *data;
				}

			}
			internal static long ReadLong(IntPtr address)
			{
				unsafe
				{
					var data = (long*)(address.ToPointer());

					return *data;
				}

			}
			internal static ulong ReadULong(IntPtr address)
			{
				unsafe
				{
					var data = (ulong*)(address.ToPointer());

					return *data;
				}
			}
			internal static void WriteSByte(IntPtr address, sbyte value)
			{
				unsafe
				{
					var data = (sbyte*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteByte(IntPtr address, byte value)
			{
				unsafe
				{
					var data = (byte*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteShort(IntPtr address, short value)
			{
				unsafe
				{
					var data = (short*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteUShort(IntPtr address, ushort value)
			{
				unsafe
				{
					var data = (ushort*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteInt(IntPtr address, int value)
			{
				unsafe
				{
					var data = (int*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteUInt(IntPtr address, uint value)
			{
				unsafe
				{
					var data = (uint*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteFloat(IntPtr address, float value)
			{
				unsafe
				{
					var data = (float*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteVector3(IntPtr address, Math.Vector3 value)
			{
				unsafe
				{
					var data = (float*)(address.ToPointer());

					data[0] = value.X;
					data[1] = value.Y;
					data[2] = value.Z;
				}
			}
			internal static void WriteMatrix(IntPtr address, Math.Matrix value)
			{
				unsafe
				{
					var data = (float*)(address.ToPointer());

					var arr = value.ToArray();
					for (int i = 0; i < arr.Length; i++)
					{
						data[i] = arr[i];
					}
				}
			}
			internal static void WriteLong(IntPtr address, long value)
			{
				unsafe
				{
					var data = (long*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void WriteULong(IntPtr address, ulong value)
			{
				unsafe
				{
					var data = (ulong*)(address.ToPointer());

					*data = value;
				}
			}
			internal static void SetBit(IntPtr address, int bit)
			{
				if (bit < 0 || bit > 31)
				{
					throw new ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
				}
				int mask = 1 << bit;

				unsafe
				{
					var data = (int*)(address.ToPointer());
					*data |= mask;
				}
			}
			internal static void ClearBit(IntPtr address, int bit)
			{
				if (bit < 0 || bit > 31)
				{
					throw new ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
				}
				int mask = 1 << bit;

				unsafe
				{
					var data = (int*)(address.ToPointer());
					*data &= ~mask;
				}
			}
			internal static bool IsBitSet(IntPtr address, int bit)
			{
				if (bit < 0 || bit > 31)
				{
					throw new ArgumentOutOfRangeException("bit", "The bit index has to be between 0 and 31");
				}
				int mask = 1 << bit;

				unsafe
				{
					var data = (int*)(address.ToPointer());
					return (*data & mask) != 0;
				}
			}
			internal static uint GetHashKey(String toHash)
			{
				IntPtr handle = ScriptDomain.CurrentDomain.PinString(toHash);
				return _getHashKey(handle, 0);
			}
			internal static string GetGXTEntryByHash(int entryLabelHash)
			{
				char* entryText = (char*)(GetLabelTextByHashFunc(GetLabelTextByHashAddr, entryLabelHash));
				if (entryText != null)
				{
					return PtrToStringUTF8(new IntPtr(entryText));
				}
				return String.Empty;
			}
			internal static string PtrToStringUTF8(IntPtr ptr)
			{
				if (IntPtr.Zero == ptr)
				{
					return null;
				}
				unsafe
				{
					byte* address = (byte*)ptr.ToPointer();
					int len = 0;

					while (address[len] != 0)
						++len;

					return PtrToStringUTF8(ptr, len);
				}
			}
			internal static String PtrToStringUTF8(IntPtr ptr, int byteLen)
			{
				if (byteLen < 0)
				{
					throw new ArgumentException(null, nameof(byteLen));
				}
				else if (IntPtr.Zero == ptr)
				{
					return null;
				}
				else if (byteLen == 0)
				{
					return string.Empty;
				}
				else
				{
					byte* pByte = (byte*)ptr.ToPointer();
					return System.Text.Encoding.UTF8.GetString(pByte, byteLen);
				}
			}
			internal static IntPtr StringToCoTaskMemUTF8(string s)
			{
				if (s == null)
				{
					return IntPtr.Zero;
				}
				else
				{
					unsafe
					{
						byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(s);
						IntPtr dest = AllocCoTaskMem(utf8Bytes.Length + 1);
						if (dest == IntPtr.Zero)
						{
							throw new OutOfMemoryException();
						}

						Copy(utf8Bytes, 0, dest, utf8Bytes.Length);
						((byte*)dest.ToPointer())[utf8Bytes.Length] = 0;

						return dest;
					}
				}
			}

			internal static IntPtr GetEntityAddress(int handle)
			{
				return new IntPtr((long)EntityAddressFunc(handle));
			}
			internal static IntPtr GetPlayerAddress(int handle)
			{
				return new IntPtr((long)PlayerAddressFunc(handle));
			}
			internal static IntPtr GetCheckpointAddress(int handle)
			{
				ulong addr = CheckpointHandleAddr(CheckpointBaseAddr(), handle);
				if (addr != 0)
				{
					return new IntPtr((long)((ulong)(checkpointPoolAddress) + 96 * ((ulong)*(int*)(addr + 16))));
				}
				return IntPtr.Zero;
			}

			internal static IntPtr GetPtfxAddress(int handle)
			{
				return new IntPtr((long)PtfxAddressFunc(handle));
			}

			internal static int GetEntityBoneCount(int handle)
			{
				unsafe
				{
					var fragSkeletonData = GetEntitySkeletonData(handle);
					return fragSkeletonData != 0 ? *(int*)(fragSkeletonData + 32) : 0;
				}
			}

			internal static IntPtr GetEntityBoneMatrixAddress(int handle, int boneIndex)
			{
				if ((boneIndex & 0x80000000) != 0)//boneIndex cant be negative
					return IntPtr.Zero;

				var fragSkeletonData = GetEntitySkeletonData(handle);

				if (fragSkeletonData == 0) return IntPtr.Zero;

				unsafe
				{
					if (boneIndex < *(int*)(fragSkeletonData + 32))// boneIndex < max bones?
					{
						return new IntPtr((long)(*(ulong*)(fragSkeletonData + 24) + ((uint)boneIndex * 0x40)));
					}
				}

				return IntPtr.Zero;
			}

			internal static IntPtr GetEntityBonePoseAddress(int handle, int boneIndex)
			{
				if ((boneIndex & 0x80000000) != 0)//boneIndex cant be negative
					return IntPtr.Zero;

				var fragSkeletonData = GetEntitySkeletonData(handle);

				if (fragSkeletonData == 0) return IntPtr.Zero;

				unsafe
				{
					if (boneIndex < *(int*)(fragSkeletonData + 32))// boneIndex < max bones?
					{
						return new IntPtr((long)(*(ulong*)(fragSkeletonData + 16) + ((uint)boneIndex * 0x40)));
					}
				}

				return IntPtr.Zero;
			}

			private unsafe static ulong GetEntitySkeletonData(int handle)
			{				
				ulong MemAddress = EntityAddressFunc(handle);

				var func2 = GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(ReadIntPtr(ReadIntPtr(new IntPtr((long)MemAddress)) + 88));
				ulong Addr2 = func2(MemAddress);
				ulong Addr3;
				if (Addr2 == 0)
				{
					Addr3 = *(ulong*)(MemAddress + 80);
					if (Addr3 == 0)
					{
						return 0;
					}
					else
					{
						Addr3 = *(ulong*)(Addr3 + 40);
					}
				}
				else
				{
					Addr3 = *(ulong*)(Addr2 + 104);
					if (Addr3 == 0 || *(ulong*)(Addr2 + 120) == 0)
					{
						return 0;
					}
					else
					{
						Addr3 = *(ulong*)(Addr3 + 376);
					}
				}
				if (Addr3 == 0)
				{
					return 0;
				}

				return Addr3;
			}

			internal static float ReadWorldGravity()
			{
				unsafe
				{
					return *_readWorldGravityAddr;
				}
			}
			internal static void WriteWorldGravity(float value)
			{
				unsafe
				{
					*_writeWorldGravityAddr = value;
				}
			}

			internal static int ReadCursorSprite()
			{
				unsafe
				{
					return *_cursorSpriteAddr;
				}
			}

			internal static float ReadTimeScale()
			{
				unsafe
				{
					return *_timeScaleAddress;
				}
			}

			internal static IntPtr GetGameplayCameraAddress()
			{
				return new IntPtr((long)*_gamePlayCameraAddr);
			}

			internal static IntPtr GetCameraAddress(int handle)
			{
				uint index = (uint)(handle >> 8);
				ulong poolAddr = *_cameraPoolAddress;
				if (*(byte*)(index + *(long*)(poolAddr + 8)) == (byte)(handle & 0xFF))
				{
					return new IntPtr(*(long*)poolAddr + (index * *(uint*)(poolAddr + 20)));
				}
				return IntPtr.Zero;

			}

			internal static int[] GetEntityHandles()
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetEntityHandles(Math.Vector3 position, float radius)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);
				task._position = position;
				task._radiusSquared = radius * radius;
				task._posCheck = true;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetVehicleHandles(int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Vehicle);
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetVehicleHandles(Math.Vector3 position, float radius, int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Vehicle);
				task._position = position;
				task._radiusSquared = radius * radius;
				task._posCheck = true;
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetPedHandles(int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Ped);
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetPedHandles(Math.Vector3 position, float radius, int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Ped);
				task._position = position;
				task._radiusSquared = radius * radius;
				task._posCheck = true;
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetPropHandles(int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Object);
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetPropHandles(Math.Vector3 position, float radius, int[] modelhashes)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.Object);
				task._position = position;
				task._radiusSquared = radius * radius;
				task._posCheck = true;
				task._modelHashes = modelhashes;
				task._modelCheck = modelhashes != null && modelhashes.Length > 0;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			[StructLayout(LayoutKind.Sequential)]
			internal unsafe struct Checkpoint
			{
				internal long padding;
				internal int padding1;
				internal int handle;
				internal long padding2;
				internal Checkpoint* next;
			}
			internal static ulong _getCheckpointHandles(ulong ArrayPtr)
			{
				unsafe
				{
					int* handles = (int*)ArrayPtr;
					ulong count = 0;
					for (Checkpoint* item = *(Checkpoint**)(CheckpointBaseAddr() + 48); item != null && count < 64; item = item->next)
					{
						handles[count++] = item->handle;
					}
					return count;
				}
			}
			internal static int[] GetCheckpointHandles()
			{
				int[] handles = new int[64];

				ulong count = 0;
				for (Checkpoint* item = *(Checkpoint**)(CheckpointBaseAddr() + 48); item != null && count < 64; item = item->next)
				{
					handles[count++] = item->handle;
				}

				int[] dataArray = new int[count];
				unsafe
				{
					fixed (int* ptrBuffer = &dataArray[0])
					{
						Copy(handles, 0, new IntPtr(ptrBuffer), (int)count);
					}
				}
				return dataArray;
			}
			internal static int[] GetPickupObjectHandles()
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}
			internal static int[] GetPickupObjectHandles(Math.Vector3 position, float radius)
			{
				var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);
				task._position = position;
				task._radiusSquared = radius * radius;
				task._posCheck = true;

				ScriptDomain.CurrentDomain.ExecuteTask(task);

				return task._handles.ToArray();
			}

			internal static int GetNumberOfVehicles()
			{
				if (*_vehiclePoolAddress != 0)
				{
					VehiclePool* pool = *(VehiclePool**)(*_vehiclePoolAddress);
					return (int)pool->itemCount;
				}
				return 0;
			}

			internal static void SendEuphoriaMessage(int targetHandle, string message, Dictionary<string, object> arguments)
			{
				var task = new EuphoriaMessageTask(targetHandle, message, arguments);

				ScriptDomain.CurrentDomain.ExecuteTask(task);
			}

			internal static int CreateTexture(string filename)
			{
				return CreateTexture(ScriptDomain.CurrentDomain.PinString(filename));
			}
			internal static void DrawTexture(int id, int index, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, System.Drawing.Color color)
			{
				DrawTexture(id, index, level, time, sizeX, sizeY, centerX, centerY, posX, posY, rotation, scaleFactor, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
			}

			public unsafe static byte* FindPattern(string pattern, string mask)
			{
				ProcessModule module = Process.GetCurrentProcess().MainModule;

				ulong address = (ulong)module.BaseAddress.ToInt64();
				ulong endAddress = address + (ulong)module.ModuleMemorySize;

				for (; address < endAddress; address++)
				{
					for (int i = 0; i < pattern.Length; i++)
					{
						if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
						{
							break;
						}
						else if (i + 1 == pattern.Length)
						{
							return (byte*)address;
						}
					}
				}

				return null;
			}
		}
	}
}
