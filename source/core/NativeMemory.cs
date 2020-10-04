//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static System.Runtime.InteropServices.Marshal;

namespace SHVDN
{
	/// <summary>
	/// Class responsible for managing all access to game memory.
	/// </summary>
	public static unsafe class NativeMemory
	{
		#region ScriptHookV Imports
		/// <summary>
		/// Creates a texture. Texture deletion is performed automatically when game reloads scripts.
		/// Can be called only in the same thread as natives.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>Internal texture ID.</returns>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?createTexture@@YAHPEBD@Z")]
		public static extern int CreateTexture([MarshalAs(UnmanagedType.LPStr)] string filename);

		/// <summary>
		/// Draws a texture on screen. Can be called only in the same thread as natives.
		/// </summary>
		/// <param name="id">Texture ID returned by <see cref="CreateTexture(string)"/>.</param>
		/// <param name="instance">The instance index. Each texture can have up to 64 different instances on screen at a time.</param>
		/// <param name="level">Texture instance with low levels draw first.</param>
		/// <param name="time">How long in milliseconds the texture instance should stay on screen.</param>
		/// <param name="sizeX">Width in screen space [0,1].</param>
		/// <param name="sizeY">Height in screen space [0,1].</param>
		/// <param name="centerX">Center position in texture space [0,1].</param>
		/// <param name="centerY">Center position in texture space [0,1].</param>
		/// <param name="posX">Position in screen space [0,1].</param>
		/// <param name="posY">Position in screen space [0,1].</param>
		/// <param name="rotation">Normalized rotation [0,1].</param>
		/// <param name="scaleFactor">Screen aspect ratio, used for size correction.</param>
		/// <param name="colorR">Red tint.</param>
		/// <param name="colorG">Green tint.</param>
		/// <param name="colorB">Blue tint.</param>
		/// <param name="colorA">Alpha value.</param>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?drawTexture@@YAXHHHHMMMMMMMMMMMM@Z")]
		public static extern void DrawTexture(int id, int instance, int level, int time, float sizeX, float sizeY, float centerX, float centerY, float posX, float posY, float rotation, float scaleFactor, float colorR, float colorG, float colorB, float colorA);

		/// <summary>
		/// Gets the game version enumeration value as specified by ScriptHookV.
		/// </summary>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?getGameVersion@@YA?AW4eGameVersion@@XZ")]
		public static extern int GetGameVersion();

		/// <summary>
		/// Returns pointer to a global variable. IDs may differ between game versions.
		/// </summary>
		/// <param name="index">The variable ID to query.</param>
		/// <returns>Pointer to the variable, or <see cref="IntPtr.Zero"/> if it does not exist.</returns>
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?getGlobalPtr@@YAPEA_KH@Z")]
		public static extern IntPtr GetGlobalPtr(int index);
		#endregion

		/// <summary>
		/// Searches the address space of the current process for a memory pattern.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <returns>The address of a region matching the pattern or <c>null</c> if none was found.</returns>
		static unsafe byte* FindPattern(string pattern, string mask)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;
			return FindPattern(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <summary>
		/// Searches the specific address space of the current process for a memory pattern.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <c>null</c> if none was found.</returns>
		static unsafe byte* FindPattern(string pattern, string mask, IntPtr startAddress, ulong size)
		{
			ulong address = (ulong)startAddress.ToInt64();
			ulong endAddress = address + size;

			for (; address < endAddress; address++)
			{
				for (int i = 0; i < pattern.Length; i++)
				{
					if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
						break;
					else if (i + 1 == pattern.Length)
						return (byte*)address;
				}
			}

			return null;
		}

		/// <summary>
		/// Initializes all known functions and offsets based on pattern searching.
		/// </summary>
		static NativeMemory()
		{
			byte* address;

			// Get relative address and add it to the instruction address.
			address = FindPattern("\x74\x21\x48\x8B\x48\x20\x48\x85\xC9\x74\x18\x48\x8B\xD6\xE8", "xxxxxxxxxxxxxxx") - 10;
			GetPtfxAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new IntPtr(*(int*)(address) + address + 4));

			address = FindPattern("\xE8\x00\x00\x00\x00\x48\x8B\xD8\x48\x85\xC0\x74\x2E\x48\x83\x3D", "x????xxxxxxxxxxx");
			GetEntityAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new IntPtr(*(int*)(address + 1) + address + 5));

			address = FindPattern("\xB2\x01\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x1C\x8A\x88", "xxx????xxxxxxx");
			GetPlayerAddressFunc = GetDelegateForFunctionPointer<GetHandleAddressFuncDelegate>(
				new IntPtr(*(int*)(address + 3) + address + 7));

			address = FindPattern("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			AddEntityToPoolFunc = GetDelegateForFunctionPointer<AddEntityToPoolFuncDelegate>(
				new IntPtr(address - 0x68));

			address = FindPattern("\x48\x8B\xDA\xE8\x00\x00\x00\x00\xF3\x0F\x10\x44\x24", "xxxx????xxxxx");
			EntityPosFunc = GetDelegateForFunctionPointer<EntityPosFuncDelegate>(
				new IntPtr((address - 6)));

			address = FindPattern("\x0F\x85\x00\x00\x00\x00\x48\x8B\x4B\x20\xE8\x00\x00\x00\x00\x48\x8B\xC8", "xx????xxxxx????xxx");
			EntityModel1Func = GetDelegateForFunctionPointer<EntityModel1FuncDelegate>(
				new IntPtr((*(int*)address + 11) + address + 15));

			address = FindPattern("\x45\x33\xC9\x3B\x05", "xxxxx");
			EntityModel2Func = GetDelegateForFunctionPointer<EntityModel2FuncDelegate>(
				new IntPtr(address - 0x46));

			// Find handling data functions
			address = FindPattern("\x0F\x84\x00\x00\x00\x00\x8B\x8B\x00\x00\x00\x00\xE8\x00\x00\x00\x00\xBA\x09\x00\x00\x00", "xx????xx????x????xxxxx");
			GetHandlingDataByIndex = GetDelegateForFunctionPointer<GetHandlingDataByIndexDelegate>(
				new IntPtr(*(int*)(address + 13) + address + 17));
			handlingIndexOffsetInModelInfo = *(int*)(address + 8);

			address = FindPattern("\xE8\x00\x00\x00\x00\x48\x85\xC0\x75\x5A\xB2\x01", "x????xxxxxxx");
			GetHandlingDataByHash = GetDelegateForFunctionPointer<GetHandlingDataByHashDelegate>(
				new IntPtr(*(int*)(address + 1) + address + 5));

			// Find entity pools
			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			PedPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			ObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			EntityPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			VehiclePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x4C\x8B\x05\x00\x00\x00\x00\x40\x8A\xF2\x8B\xE9", "xxx????xxxxx");
			PickupObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			// Find euphoria functions
			address = FindPattern("\x48\x8b\xc4\x48\x89\x58\x08\x48\x89\x68\x10\x48\x89\x70\x18\x48\x89\x78\x20\x41\x55\x41\x56\x41\x57\x48\x83\xec\x20\xe8\x00\x00\x00\x00\x48\x8b\xd8\x48\x85\xc0\x0f", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx????xxxxxxx");
			GiveNmMessageFuncAddress = (ulong)address;

			address = FindPattern("\x33\xDB\x48\x89\x1D\x00\x00\x00\x00\x85\xFF", "xxxxx????xx");
			CreateNmMessageFuncAddress = (ulong)address - 0x42;

			address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmIntAddress = GetDelegateForFunctionPointer<SetNmIntAddressDelegate>(new IntPtr(address));

			address = FindPattern("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmBoolAddress = GetDelegateForFunctionPointer<SetNmBoolAddressDelegate>(new IntPtr(address));

			address = FindPattern("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmFloatAddress = GetDelegateForFunctionPointer<SetNmFloatAddressDelegate>(new IntPtr(address));

			address = FindPattern("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			SetNmStringAddress = GetDelegateForFunctionPointer<SetNmStringAddressDelegate>(new IntPtr(address));

			address = FindPattern("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmVector3Address = GetDelegateForFunctionPointer<SetNmVector3AddressDelegate>(new IntPtr(address));

			address = FindPattern("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
			GetLabelTextByHashAddress = (ulong)(*(int*)(address + 7) + address + 11);

			address = FindPattern("\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x18\x89\x54\x24\x10\x56\x57\x41\x56\x48\x83\xEC\x20", "xxxxxxxxxxxxxxxxxxxxxx");
			GetLabelTextByHashFunc = GetDelegateForFunctionPointer<GetLabelTextByHashFuncDelegate>(new IntPtr(address));

			address = FindPattern("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
			CheckpointPoolAddress = (ulong*)(*(int*)(address + 17) + address + 21);
			GetCheckpointBaseAddress = GetDelegateForFunctionPointer<GetCheckpointBaseAddressDelegate>(new IntPtr(*(int*)(address - 19) + address - 15));
			GetCheckpointHandleAddress = GetDelegateForFunctionPointer<GetCheckpointHandleAddressDelegate>(new IntPtr(*(int*)(address - 9) + address - 5));

			address = FindPattern("\x4C\x8D\x05\x00\x00\x00\x00\x0F\xB7\xC1", "xxx????xxx");
			RadarBlipPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x48\x8B\x0B\x33\xD2\xE8\x00\x00\x00\x00\x89\x03", "xxxxxx????xx");
			GetHashKeyFunc = GetDelegateForFunctionPointer<GetHashKeyDelegate>(new IntPtr(*(int*)(address + 6) + address + 10));

			address = FindPattern("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
			cursorSpriteAddr = (int*)(*(int*)(address - 4) + address);

			address = FindPattern("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05\x00\x00\x00\x00", "xxxxxx????xxxxxxxxx????");
			readWorldGravityAddress = (float*)(*(int*)(address + 19) + address + 23);
			writeWorldGravityAddress = (float*)(*(int*)(address + 6) + address + 10);

			address = FindPattern("\xF3\x0F\x10\x1D\x00\x00\x00\x00\x41\xB8\x00\x00\x00\x00\x41\x8B\xD0", "xxxx????xx????xxx");
			var timeScaleArrayAddress = (float*)(*(int*)(address + 4) + address + 8);
			if (timeScaleArrayAddress != null)
				// SET_TIME_SCALE changes the 2nd element, so obtain the address of it
				timeScaleAddress = timeScaleArrayAddress + 1;

			// Find camera objects
			address = FindPattern("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
			CameraPoolAddress = (ulong*)(*(int*)(address) + address + 4);
			address = FindPattern("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
			address = address + *(int*)(address) + 4;
			GameplayCameraAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			// Find model hash table
			address = FindPattern("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			uint vehicleClassOffset = *(uint*)(address + 0x31);
			address = address + *(int*)(address) + 4;
			modelNum1 = *(UInt32*)(*(int*)(address + 0x52) + address + 0x56);
			modelNum2 = *(UInt64*)(*(int*)(address + 0x63) + address + 0x67);
			modelNum3 = *(UInt64*)(*(int*)(address + 0x7A) + address + 0x7E);
			modelNum4 = *(UInt64*)(*(int*)(address + 0x81) + address + 0x85);
			modelHashTable = *(UInt64*)(*(int*)(address + 0x24) + address + 0x28);
			modelHashEntries = *(UInt16*)(address + *(int*)(address + 3) + 7);

			// Find vehicle data offsets
			address = FindPattern("\x48\x8D\x8F\x00\x00\x00\x00\x4C\x8B\xC3\xF3\x0F\x11\x7C\x24", "xxx????xxxxxxxx");
			if (address != null)
			{
				NextGearOffset = *(int*)(address + 3);
				GearOffset = *(int*)(address + 3) + 2;
				HighGearOffset = *(int*)(address + 3) + 6;
			}

			address = FindPattern("\x74\x26\x0F\x57\xC9\x0F\x2F\x8B\x34\x08\x00\x00\x73\x1A\xF3\x0F\x10\x83\x24\x08\x00\x00", "x?xxxxxx????x?xxxx????");
			if (address != null)
			{
				FuelLevelOffset = *(int*)(address + 8);
			}

			address = FindPattern("\xF3\x0F\x10\x8F\x10\x0A\x00\x00\xF3\x0F\x59\x05\x5E\x30\x8D\x00", "xxxx????xxxx????");
			if (address != null)
			{
				WheelSpeedOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x76\x03\x0F\x28\xF0\xF3\x44\x0F\x10\x93", "xxxxxxxxxx");
			if (address != null)
			{
				CurrentRPMOffset = *(int*)(address + 10);
				ClutchOffset = *(int*)(address + 10) + 0xC;
				AccelerationOffset = *(int*)(address + 10) + 0x10;
			}

			// use the former pattern if the version is 1.0.1604.0 or newer
			var gameVersion = GetGameVersion();
			address = gameVersion >= 46 ?
						FindPattern("\xF3\x0F\x10\x9F\xD4\x08\x00\x00\x0F\x2F\xDF\x73\x0A", "xxxx????xxxxx") :
						FindPattern("\xF3\x0F\x10\x8F\x68\x08\x00\x00\x88\x4D\x8C\x0F\x2F\xCF", "xxxx????xxx???");
			if (address != null)
			{
				TurboOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x74\x0A\xF3\x0F\x11\xB3\x1C\x09\x00\x00\xEB\x25", "xxxxxx????xx");
			if (address != null)
			{
				SteeringScaleOffset = *(int*)(address + 6);
				SteeringAngleOffset = *(int*)(address + 6) + 8;
				ThrottlePowerOffset = *(int*)(address + 6) + 0x10;
				BrakePowerOffset = *(int*)(address + 6) + 0x14;
			}

			address = FindPattern("\xF3\x0F\x11\x9B\xDC\x09\x00\x00\x0F\x84\xB1\x00\x00\x00", "xxxx????xxx???");
			if (address != null)
			{
				EngineTemperatureOffset = *(int*)(address + 4);
			}

			address = FindPattern("\xFD\x02\xDB\x08\x98\x00\x00\x00\x00\x48\x8B\x5C\x24\x30", "xxxxx????xxxxx");
			if (address != null)
			{
				IsInteriorLightOnOffset = *(int*)(address - 4);
				IsEngineStartingOffset = IsInteriorLightOnOffset + 1;
			}

			address = FindPattern("\x8A\x96\x00\x00\x00\x00\x0F\xB6\xC8\x84\xD2\x41", "xx????xxxxxx");
			if (address != null)
			{
				IsWantedOffset = *(int*)(address + 40);
			}


			address = FindPattern("\x45\x33\xC9\x41\xB0\x01\x40\x8A\xD7", "xxxxxxxxx");
			if (address != null)
			{
				PreviouslyOwnedByPlayerOffset = *(int*)(address - 5);
				NeedsToBeHotwiredOffset = PreviouslyOwnedByPlayerOffset;
			}

			address = FindPattern("\x24\x07\x3C\x03\x74\x00\xE8", "xxxxx?x");
			if (address != null)
			{
				AlarmTimeOffset = *(int*)(address + 52);
			}

			address = FindPattern("\x44\x0F\x2F\x43\x00\x45\x8D\x74\x24\x01", "xxxx?xxxxx");
			if (address != null)
			{
				HandlingDataOffset = *(int*)(address - 35);
			}

			address = FindPattern("\x48\x85\xC0\x74\x3C\x8B\x80\x00\x00\x00\x00\xC1\xE8\x0F", "xxxxxxx????xxx");
			if (address != null)
			{
				FirstVehicleFlagsOffset = *(int*)(address + 7);
			}

			// Generate vehicle model list
			var vehicleHashes = new List<int>[0x20];
			for (int i = 0; i < 0x20; i++)
				vehicleHashes[i] = new List<int>();

			var weaponObjectHashes = new List<int>();
			var pedHashes = new List<int>();

			for (int i = 0; i < modelHashEntries; i++)
			{
				for (HashNode* cur = ((HashNode**)modelHashTable)[i]; cur != null; cur = cur->next)
				{
					ushort data = cur->data;
					bool bitTest = ((*(int*)(modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
					if (data < modelNum1 && bitTest)
					{
						ulong addr1 = modelNum4 + modelNum3 * data;
						if (addr1 != 0)
						{
							ulong addr2 = *(ulong*)(addr1);
							if (addr2 != 0)
							{
								switch ((ModelInfoClassType)(*(byte*)(addr2 + 157) & 0x1F))
								{
									case ModelInfoClassType.Weapon:
										weaponObjectHashes.Add(cur->hash);
										break;
									case ModelInfoClassType.Vehicle:
										vehicleHashes[*(byte*)(addr2 + vehicleClassOffset) & 0x1F].Add(cur->hash);
										break;
									case ModelInfoClassType.Ped:
										pedHashes.Add(cur->hash);
										break;
								}
							}
						}
					}
				}
			}

			var vehicleResult = new ReadOnlyCollection<int>[0x20];
			for (int i = 0; i < 0x20; i++)
				vehicleResult[i] = Array.AsReadOnly(vehicleHashes[i].ToArray());
			VehicleModels = Array.AsReadOnly(vehicleResult);

			WeaponModels = Array.AsReadOnly(weaponObjectHashes.ToArray());
			PedModels = Array.AsReadOnly(pedHashes.ToArray());

			#region -- Enable All DLC Vehicles --
			// no need to patch the global variable in v1.0.573.1 or older builds
			if (gameVersion <= 15)
			{
				return;
			}

			address = FindPattern("\x48\x03\x15\x00\x00\x00\x00\x4C\x23\xC2\x49\x8B\x08", "xxx????xxxxxx");
			var yscScriptTable = (YscScriptTable*)(address + *(int*)(address + 3) + 7);

			// find the shop_controller script
			YscScriptTableItem* shopControllerItem = yscScriptTable->FindScript(0x39DA738B);

			if (shopControllerItem == null || !shopControllerItem->IsLoaded())
			{
				return;
			}

			YscScriptHeader* shopControllerHeader = shopControllerItem->header;

			for (int i = 0; i < shopControllerHeader->CodePageCount(); i++)
			{
				int size = shopControllerHeader->GetCodePageSize(i);
				if (size > 0)
				{
					var enableCarsGlobalPattern = gameVersion >= 46 ?
							"\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01" :
							"\x2C\x01\x00\x00\x20\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01";
					var enableCarsGlobalMask = gameVersion >= 46 ? "x??xxxx??xxxxx?xx????xxxx?x" : "xx??xxxxxx?xx????xxxx?x";
					var enableCarsGlobalOffset = gameVersion >= 46 ? 17 : 13;
					address = FindPattern(enableCarsGlobalPattern, enableCarsGlobalMask, shopControllerHeader->GetCodePageAddress(i), (ulong)size);

					if (address != null)
					{
						int globalindex = *(int*)(address + enableCarsGlobalOffset) & 0xFFFFFF;
						*(int*)GetGlobalPtr(globalindex).ToPointer() = 1;
					}
				}
			}
			#endregion
		}

		/// <summary>
		/// Reads a single 8-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static byte ReadByte(IntPtr address)
		{
			return *(byte*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single 16-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static Int16 ReadInt16(IntPtr address)
		{
			return *(short*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single 32-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static Int32 ReadInt32(IntPtr address)
		{
			return *(int*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single floating-point value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static float ReadFloat(IntPtr address)
		{
			return *(float*)address.ToPointer();
		}
		/// <summary>
		/// Reads a null-terminated UTF-8 string from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The string at the address.</returns>
		public static String ReadString(IntPtr address)
		{
			return PtrToStringUTF8(address);
		}
		/// <summary>
		/// Reads a single 64-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static IntPtr ReadAddress(IntPtr address)
		{
			return new IntPtr(*(void**)(address.ToPointer()));
		}
		/// <summary>
		/// Reads a 4x4 floating-point matrix from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>All elements of the matrix in row major arrangement.</returns>
		public static float[] ReadMatrix(IntPtr address)
		{
			var data = (float*)address.ToPointer();
			return new float[16] { data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15] };
		}
		/// <summary>
		/// Reads a 3-component floating-point vector from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>All elements of the vector.</returns>
		public static float[] ReadVector3(IntPtr address)
		{
			var data = (float*)address.ToPointer();
			return new float[3] { data[0], data[1], data[2] };
		}

		/// <summary>
		/// Writes a single 8-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteByte(IntPtr address, byte value)
		{
			var data = (byte*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 16-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt16(IntPtr address, Int16 value)
		{
			var data = (short*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 32-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt32(IntPtr address, Int32 value)
		{
			var data = (int*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single floating-point value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteFloat(IntPtr address, float value)
		{
			var data = (float*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a 4x4 floating-point matrix to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The elements of the matrix in row major arrangement to write.</param>
		public static void WriteMatrix(IntPtr address, float[] value)
		{
			var data = (float*)(address.ToPointer());
			for (int i = 0; i < value.Length; i++)
				data[i] = value[i];
		}
		/// <summary>
		/// Writes a 3-component floating-point to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The vector components to write.</param>
		public static void WriteVector3(IntPtr address, float[] value)
		{
			var data = (float*)address.ToPointer();
			data[0] = value[0];
			data[1] = value[1];
			data[2] = value[2];
		}

		/// <summary>
		/// Sets a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to change.</param>
		public static void SetBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			*data |= (1 << bit);
		}
		/// <summary>
		/// Clears a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to change.</param>
		public static void ClearBit(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			*data &= ~(1 << bit);
		}
		/// <summary>
		/// Checks a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to check.</param>
		/// <returns><c>true</c> if the bit is set, <c>false</c> if it is unset.</returns>
		public static bool IsBitSet(IntPtr address, int bit)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			return (*data & (1 << bit)) != 0;
		}

		public static IntPtr String => StringToCoTaskMemUTF8("STRING");
		public static IntPtr NullString => StringToCoTaskMemUTF8(string.Empty);
		public static IntPtr CellEmailBcon => StringToCoTaskMemUTF8("CELL_EMAIL_BCON");

		public static string PtrToStringUTF8(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return string.Empty;

			var data = (byte*)ptr.ToPointer();

			// Calculate length of null-terminated string
			int len = 0;
			while (data[len] != 0)
				++len;

			return PtrToStringUTF8(ptr, len);
		}
		public static string PtrToStringUTF8(IntPtr ptr, int len)
		{
			if (len < 0)
				throw new ArgumentException(null, nameof(len));

			if (ptr == IntPtr.Zero)
				return null;
			if (len == 0)
				return string.Empty;

			return Encoding.UTF8.GetString((byte*)ptr.ToPointer(), len);
		}
		public static IntPtr StringToCoTaskMemUTF8(string s)
		{
			if (s == null)
				return IntPtr.Zero;

			byte[] utf8Bytes = Encoding.UTF8.GetBytes(s);
			IntPtr dest = AllocCoTaskMem(utf8Bytes.Length + 1);
			if (dest == IntPtr.Zero)
				throw new OutOfMemoryException();

			Copy(utf8Bytes, 0, dest, utf8Bytes.Length);
			// Add null-terminator to end
			((byte*)dest.ToPointer())[utf8Bytes.Length] = 0;

			return dest;
		}

		#region -- Cameras --

		static ulong* CameraPoolAddress;
		static ulong* GameplayCameraAddress;

		public static IntPtr GetCameraAddress(int handle)
		{
			uint index = (uint)(handle >> 8);
			ulong poolAddr = *CameraPoolAddress;
			if (*(byte*)(index + *(long*)(poolAddr + 8)) == (byte)(handle & 0xFF))
			{
				return new IntPtr(*(long*)poolAddr + (index * *(uint*)(poolAddr + 20)));
			}
			return IntPtr.Zero;

		}
		public static IntPtr GetGameplayCameraAddress()
		{
			return new IntPtr((long)*GameplayCameraAddress);
		}

		#endregion

		#region -- Game Data --

		delegate uint GetHashKeyDelegate(IntPtr stringPtr, uint initialHash);
		static GetHashKeyDelegate GetHashKeyFunc;

		public static uint GetHashKey(string key)
		{
			IntPtr keyPtr = ScriptDomain.CurrentDomain.PinString(key);
			return GetHashKeyFunc(keyPtr, 0);
		}

		static ulong GetLabelTextByHashAddress;
		delegate ulong GetLabelTextByHashFuncDelegate(ulong address, int labelHash);
		static GetLabelTextByHashFuncDelegate GetLabelTextByHashFunc;

		public static string GetGXTEntryByHash(int entryLabelHash)
		{
			var entryText = (char*)GetLabelTextByHashFunc(GetLabelTextByHashAddress, entryLabelHash);
			return entryText != null ? PtrToStringUTF8(new IntPtr(entryText)) : string.Empty;
		}

		#endregion

		#region -- YSC Script Data --

		[StructLayout(LayoutKind.Explicit)]
		struct YscScriptHeader
		{
			[FieldOffset(0x10)]
			internal byte** codeBlocksOffset;
			[FieldOffset(0x1C)]
			internal int codeLength;
			[FieldOffset(0x24)]
			internal int localCount;
			[FieldOffset(0x2C)]
			internal int nativeCount;
			[FieldOffset(0x30)]
			internal long* localOffset;
			[FieldOffset(0x40)]
			internal long* nativeOffset;
			[FieldOffset(0x58)]
			internal int nameHash;

			internal int CodePageCount()
			{
				return (codeLength + 0x3FFF) >> 14;
			}
			internal int GetCodePageSize(int page)
			{
				return (page < 0 || page >= CodePageCount() ? 0 : (page == CodePageCount() - 1) ? codeLength & 0x3FFF : 0x4000);
			}
			internal IntPtr GetCodePageAddress(int page)
			{
				return new IntPtr(codeBlocksOffset[page]);
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		struct YscScriptTableItem
		{
			[FieldOffset(0x0)]
			internal YscScriptHeader* header;
			[FieldOffset(0xC)]
			internal int hash;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal bool IsLoaded()
			{
				return header != null;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		struct YscScriptTable
		{
			[FieldOffset(0x0)]
			internal YscScriptTableItem* TablePtr;
			[FieldOffset(0x18)]
			internal uint count;

			internal YscScriptTableItem* FindScript(int hash)
			{
				if (TablePtr == null)
				{
					return null; //table initialisation hasnt happened yet
				}
				for (int i = 0; i < count; i++)
				{
					if (TablePtr[i].hash == hash)
					{
						return &TablePtr[i];
					}
				}
				return null;
			}
		}


		#endregion

		#region -- World Data --

		static int* cursorSpriteAddr;

		public static int CursorSprite
		{
			get { return *cursorSpriteAddr; }
		}

		static float* timeScaleAddress;

		public static float TimeScale
		{
			get { return *timeScaleAddress; }
		}

		static float* readWorldGravityAddress;
		static float* writeWorldGravityAddress;

		public static float WorldGravity
		{
			get { return *readWorldGravityAddress; }
			set { *writeWorldGravityAddress = value; }
		}

		#endregion

		#region -- Skeleton Data --

		static ulong GetEntitySkeletonData(int handle)
		{
			ulong MemAddress = GetEntityAddressFunc(handle);

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

		public static int GetEntityBoneCount(int handle)
		{
			var fragSkeletonData = GetEntitySkeletonData(handle);
			return fragSkeletonData != 0 ? *(int*)(fragSkeletonData + 32) : 0;
		}
		public static IntPtr GetEntityBonePoseAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var fragSkeletonData = GetEntitySkeletonData(handle);
			if (fragSkeletonData == 0)
				return IntPtr.Zero;

			if (boneIndex < *(int*)(fragSkeletonData + 32)) // boneIndex < max bones?
			{
				return new IntPtr((long)(*(ulong*)(fragSkeletonData + 16) + ((uint)boneIndex * 0x40)));
			}

			return IntPtr.Zero;
		}
		public static IntPtr GetEntityBoneMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var fragSkeletonData = GetEntitySkeletonData(handle);
			if (fragSkeletonData == 0)
				return IntPtr.Zero;

			if (boneIndex < *(int*)(fragSkeletonData + 32)) // boneIndex < max bones?
			{
				return new IntPtr((long)(*(ulong*)(fragSkeletonData + 24) + ((uint)boneIndex * 0x40)));
			}

			return IntPtr.Zero;
		}

		#endregion

		#region -- Vehicle Offsets --

		public static int NextGearOffset { get; }
		public static int GearOffset { get; }
		public static int HighGearOffset { get; }

		public static int CurrentRPMOffset { get; }
		public static int ClutchOffset { get; }
		public static int AccelerationOffset { get; }

		public static int TurboOffset { get; }

		public static int FuelLevelOffset { get; }
		public static int WheelSpeedOffset { get; }

		public static int SteeringAngleOffset { get; }
		public static int SteeringScaleOffset { get; }
		public static int ThrottlePowerOffset { get; }
		public static int BrakePowerOffset { get; }

		public static int EngineTemperatureOffset { get; }

		public static int IsInteriorLightOnOffset { get; }
		public static int IsEngineStartingOffset { get; }

		public static int IsWantedOffset { get; }

		public static int PreviouslyOwnedByPlayerOffset { get; }
		public static int NeedsToBeHotwiredOffset { get; }

		public static int AlarmTimeOffset { get; }

		public static int HandlingDataOffset { get; }

		public static int FirstVehicleFlagsOffset { get; }

		#endregion

		#region -- Model Info --

		[StructLayout(LayoutKind.Sequential)]
		struct HashNode
		{
			internal int hash;
			internal ushort data;
			internal ushort padding;
			internal HashNode* next;
		}

		enum ModelInfoClassType
		{
			Invalid = 0,
			Object = 1,
			Mlo = 2,
			Time = 3,
			Weapon = 4,
			Vehicle = 5,
			Ped = 6
		}
		enum VehicleStructClassType
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
		[Flags]
		public enum VehicleFlag1 : ulong
		{
			Big = 0x2,
			IsVan = 0x20,
			CanStandOnTop = 0x10000000,
			LawEnforcement = 0x80000000,
			EmergencyService = 0x100000000,
			AllowsRappel = 0x8000000000,
			IsElectric = 0x80000000000,
			IsOffroadVehicle = 0x1000000000000,
			IsBus = 0x400000000000000,
		}
		[Flags]
		public enum VehicleFlag2 : ulong
		{
			IsTank = 0x200,
			HasBulletProofGlass = 0x1000,
			HasLowriderHydraulics = 0x80000000000000,
			HasLowriderDonkHydraulics = 0x800000000000000,
		}

		static int handlingIndexOffsetInModelInfo;
		static UInt32 modelNum1;
		static UInt64 modelNum2;
		static UInt64 modelNum3;
		static UInt64 modelNum4;
		static UInt64 modelHashTable;
		static UInt16 modelHashEntries;

		static IntPtr FindCModelInfo(int modelHash)
		{
			for (HashNode* cur = ((HashNode**)modelHashTable)[(uint)(modelHash) % modelHashEntries]; cur != null; cur = cur->next)
			{
				if (cur->hash != modelHash)
					continue;

				ushort data = cur->data;
				bool bitTest = ((*(int*)(modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
				if (data < modelNum1 && bitTest)
				{
					ulong addr1 = modelNum4 + modelNum3 * data;
					if (addr1 != 0)
					{
						long* address = (long*)(*(ulong*)(addr1));
						return new IntPtr(address);
					}
				}
			}

			return IntPtr.Zero;
		}
		static ModelInfoClassType GetModelInfoClass(IntPtr address)
		{
			if (address != IntPtr.Zero)
			{
				return ((ModelInfoClassType)((*(byte*)((ulong)address.ToInt64() + 157) & 0x1F)));
			}

			return ModelInfoClassType.Invalid;
		}
		static VehicleStructClassType GetVehicleStructClass(IntPtr modelInfoAddress)
		{
			if (GetModelInfoClass(modelInfoAddress) == ModelInfoClassType.Vehicle)
			{
				return (VehicleStructClassType)(*(int*)((ulong)modelInfoAddress.ToInt64() + 792));
			}

			return VehicleStructClassType.Invalid;
		}
		static IntPtr GetModelInfo(IntPtr entityAddress)
		{
			if (entityAddress != IntPtr.Zero)
			{
				return new IntPtr(*(long*)((ulong)entityAddress.ToInt64() + 0x20));
			}

			return IntPtr.Zero;
		}
		static int GetModelHashFromFwArcheType(IntPtr fwArcheTypeAddress)
		{
			if (fwArcheTypeAddress != IntPtr.Zero)
			{
				return (*(int*)((ulong)fwArcheTypeAddress.ToInt64() + 0x18));
			}

			return 0;
		}
		static int GetModelHashFromEntity(IntPtr entityAddress)
		{
			if (entityAddress != IntPtr.Zero)
			{
				var modelInfoAddress = GetModelInfo(entityAddress);
				if (modelInfoAddress != IntPtr.Zero)
				{
					return GetModelHashFromFwArcheType(modelInfoAddress);
				}
			}

			return 0;
		}

		public static bool IsModelAPed(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetModelInfoClass(modelInfo) == ModelInfoClassType.Ped;
		}
		public static bool IsModelABlimp(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Blimp;
		}
		public static bool IsModelASubmarineCar(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.SubmarineCar;
		}
		public static bool IsModelATrailer(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Trailer;
		}


		public static bool HasVehicleFlag(int modelHash, VehicleFlag1 flag) => HasVehicleFlagInternal(modelHash, (ulong)flag, 0x0);
		public static bool HasVehicleFlag(int modelHash, VehicleFlag2 flag) => HasVehicleFlagInternal(modelHash, (ulong)flag, 0x8);
		private static bool HasVehicleFlagInternal(int modelHash, ulong flag, int flagOffset)
		{
			if (FirstVehicleFlagsOffset == 0)
				return false;

			IntPtr modelInfo = FindCModelInfo(modelHash);

			if (GetModelInfoClass(modelInfo) == ModelInfoClassType.Vehicle)
			{
				var modelFlags = *(ulong*)(modelInfo + FirstVehicleFlagsOffset + flagOffset).ToPointer();
				return (modelFlags & flag) != 0;
			}

			return false;
		}

		public static ReadOnlyCollection<int> WeaponModels { get; }
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModels { get; }
		public static ReadOnlyCollection<int> PedModels { get; }

		delegate ulong GetHandlingDataByHashDelegate(IntPtr hashAddress);
		delegate ulong GetHandlingDataByIndexDelegate(int index);

		static GetHandlingDataByHashDelegate GetHandlingDataByHash;
		static GetHandlingDataByIndexDelegate GetHandlingDataByIndex;

		public static IntPtr GetHandlingDataByModelHash(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			if (GetModelInfoClass(modelInfo) != ModelInfoClassType.Vehicle)
				return IntPtr.Zero;

			int handlingIndex = *(int*)(modelInfo + handlingIndexOffsetInModelInfo).ToPointer();
			return new IntPtr((long)GetHandlingDataByIndex(handlingIndex));
		}
		public static IntPtr GetHandlingDataByHandlingNameHash(int handlingNameHash)
		{
			return new IntPtr((long)GetHandlingDataByHash(new IntPtr(&handlingNameHash)));
		}

		#endregion

		#region -- Entity Pools --

		[StructLayout(LayoutKind.Sequential)]
		struct Checkpoint
		{
			internal long padding;
			internal int padding1;
			internal int handle;
			internal long padding2;
			internal Checkpoint* next;
		}

		[StructLayout(LayoutKind.Explicit)]
		struct EntityPool
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
		struct VehiclePool
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
		struct GenericPool
		{
			[FieldOffset(0x00)]
			public ulong poolStartAddress;
			[FieldOffset(0x08)]
			public IntPtr byteArray;
			[FieldOffset(0x10)]
			public uint size;
			[FieldOffset(0x14)]
			public uint itemSize;
			[FieldOffset(0x20)]
			public ushort itemCount;

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

		static ulong* PedPoolAddress;
		static ulong* EntityPoolAddress;
		static ulong* ObjectPoolAddress;
		static ulong* PickupObjectPoolAddress;
		static ulong* VehiclePoolAddress;
		static ulong* CheckpointPoolAddress;
		static ulong* RadarBlipPoolAddress;

		delegate ulong EntityPosFuncDelegate(ulong address, float* position);
		delegate ulong EntityModel1FuncDelegate(ulong address);
		delegate ulong EntityModel2FuncDelegate(ulong address);
		delegate int AddEntityToPoolFuncDelegate(ulong address);

		static EntityPosFuncDelegate EntityPosFunc;
		static EntityModel1FuncDelegate EntityModel1Func;
		static EntityModel2FuncDelegate EntityModel2Func;
		static AddEntityToPoolFuncDelegate AddEntityToPoolFunc;

		internal class EntityPoolTask : IScriptTask
		{
			#region Fields
			internal Type poolType;
			internal List<int> handles = new List<int>();
			internal bool doPosCheck;
			internal bool doModelCheck;
			internal int[] modelHashes;
			internal float radiusSquared;
			internal float[] position;
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
				poolType = type;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			bool CheckEntity(ulong address)
			{
				if (address == 0)
					return false;

				if (doPosCheck)
				{
					float* position = stackalloc float[3];

					NativeMemory.EntityPosFunc(address, position);

					float x = this.position[0] - position[0];
					float y = this.position[1] - position[1];
					float z = this.position[2] - position[2];
					float distanceSquared = (x * x) + (y * y) + (z * z);
					if (distanceSquared > radiusSquared)
						return false;
				}

				if (doModelCheck)
				{
					int modelHash = GetModelHashFromEntity(new IntPtr((long)address));
					if (!Array.Exists(modelHashes, x => x == modelHash))
						return false;

					//uint v0 = *(uint*)(NativeMemory.EntityModel1Func(*(ulong*)(address + 32)));
					//uint v1 = v0 & 0xFFFF;
					//uint v2 = ((v1 ^ v0) & 0x0FFF0000 ^ v1) & 0xDFFFFFFF;
					//uint v3 = ((v2 ^ v0) & 0x10000000 ^ v2) & 0x3FFFFFFF;
					//ulong v5 = NativeMemory.EntityModel2Func((ulong)(&v3));
					//
					//if (v5 == 0)
					//	return false;
					//
					//foreach (int hash in modelHashes)
					//	if (*(int*)(v5 + 24) == hash)
					//		return true;
					//return false;
				}

				return true;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			bool CheckCheckpoint(ulong address)
			{
				if (address == 0)
					return false;

				if (doPosCheck)
				{
					float* position = (float*)(address + 0x90);

					float x = this.position[0] - position[0];
					float y = this.position[1] - position[1];
					float z = this.position[2] - position[2];
					float distanceSquared = (x * x) + (y * y) + (z * z);
					if (distanceSquared > radiusSquared)
						return false;
				}

				return true;
			}

			public void Run()
			{
				if (*NativeMemory.EntityPoolAddress == 0)
					return;

				EntityPool* entityPool = (EntityPool*)(*NativeMemory.EntityPoolAddress);

				if (poolType.HasFlag(Type.Vehicle) && *NativeMemory.VehiclePoolAddress != 0)
				{
					VehiclePool* vehiclePool = *(VehiclePool**)(*NativeMemory.VehiclePoolAddress);

					for (uint i = 0; i < vehiclePool->size; i++)
					{
						if (entityPool->IsFull())
							break;

						if (vehiclePool->IsValid(i))
						{
							ulong address = vehiclePool->GetAddress(i);
							if (CheckEntity(address))
								handles.Add(NativeMemory.AddEntityToPoolFunc(address));
						}
					}
				}

				if (poolType.HasFlag(Type.Ped) && *NativeMemory.PedPoolAddress != 0)
				{
					GenericPool* pedPool = (GenericPool*)(*NativeMemory.PedPoolAddress);

					for (uint i = 0; i < pedPool->size; i++)
					{
						if (entityPool->IsFull())
							break;

						if (pedPool->IsValid(i))
						{
							ulong address = pedPool->GetAddress(i);
							if (CheckEntity(address))
								handles.Add(NativeMemory.AddEntityToPoolFunc(address));
						}
					}
				}

				if (poolType.HasFlag(Type.Object) && *NativeMemory.ObjectPoolAddress != 0)
				{
					GenericPool* propPool = (GenericPool*)(*NativeMemory.ObjectPoolAddress);

					for (uint i = 0; i < propPool->size; i++)
					{
						if (entityPool->IsFull())
							break;

						if (propPool->IsValid(i))
						{
							ulong address = propPool->GetAddress(i);
							if (CheckEntity(address))
								handles.Add(NativeMemory.AddEntityToPoolFunc(address));
						}
					}
				}

				if (poolType.HasFlag(Type.PickupObject) && *NativeMemory.PickupObjectPoolAddress != 0)
				{
					GenericPool* pickupPool = (GenericPool*)(*NativeMemory.PickupObjectPoolAddress);

					for (uint i = 0; i < pickupPool->size; i++)
					{
						if (entityPool->IsFull())
							break;

						if (pickupPool->IsValid(i))
						{
							ulong address = pickupPool->GetAddress(i);
							if (CheckCheckpoint(address))
								handles.Add(NativeMemory.AddEntityToPoolFunc(address));
						}
					}
				}
			}
		}

		public static int GetPedCount()
		{
			if (*PedPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*PedPoolAddress);
				return (int)pool->itemCount;
			}
			return 0;
		}
		public static int GetVehicleCount()
		{
			if (*VehiclePoolAddress != 0)
			{
				VehiclePool* pool = *(VehiclePool**)(*VehiclePoolAddress);
				return (int)pool->itemCount;
			}
			return 0;
		}
		public static int GetObjectCount()
		{
			if (*ObjectPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*ObjectPoolAddress);
				return (int)pool->itemCount;
			}
			return 0;
		}
		public static int GetPickupObjectCount()
		{
			if (*PickupObjectPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*PickupObjectPoolAddress);
				return (int)pool->itemCount;
			}
			return 0;
		}

		public static int GetPedCapacity()
		{
			if (*PedPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*PedPoolAddress);
				return (int)pool->size;
			}
			return 0;
		}
		public static int GetVehicleCapacity()
		{
			if (*VehiclePoolAddress != 0)
			{
				VehiclePool* pool = *(VehiclePool**)(*VehiclePoolAddress);
				return (int)pool->size;
			}
			return 0;
		}
		public static int GetObjectCapacity()
		{
			if (*ObjectPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*ObjectPoolAddress);
				return (int)pool->size;
			}
			return 0;
		}
		public static int GetPickupObjectCapacity()
		{
			if (*PickupObjectPoolAddress != 0)
			{
				GenericPool* pool = (GenericPool*)(*PickupObjectPoolAddress);
				return (int)pool->size;
			}
			return 0;
		}

		public static int[] GetPedHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}
		public static int[] GetPedHandles(float[] position, float radius, int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}

		public static int[] GetPropHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Object);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}
		public static int[] GetPropHandles(float[] position, float radius, int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Object);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}

		public static int[] GetEntityHandles()
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}
		public static int[] GetEntityHandles(float[] position, float radius)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}

		public static int[] GetVehicleHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Vehicle);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}
		public static int[] GetVehicleHandles(float[] position, float radius, int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Vehicle);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}

		public static int[] GetCheckpointHandles()
		{
			int[] handles = new int[64];

			ulong count = 0;
			for (Checkpoint* item = *(Checkpoint**)(GetCheckpointBaseAddress() + 48); item != null && count < 64; item = item->next)
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
		public static int[] GetPickupObjectHandles()
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}
		public static int[] GetPickupObjectHandles(float[] position, float radius)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles.ToArray();
		}

		#endregion

		#region -- Radar Blip Pool --

		public static int[] GetNonCriticalRadarBlipHandles(params int[] spriteTypes)
		{
			return GetNonCriticalRadarBlipHandles(null, 0f, spriteTypes);
		}
		public static int[] GetNonCriticalRadarBlipHandles(float[] position = null, float radius = 0f, params int[] spriteTypes)
		{
			if (RadarBlipPoolAddress == null)
			{
				return new int[0];
			}

			int possibleBlipCount = *(int*)(RadarBlipPoolAddress - 1);

			var handles = new List<int>(possibleBlipCount);

			// Skip the first 3 critical blips, just like GET_FIRST_BLIP_INFO_ID does
			// The second critical blip is the player blip and the third is the north blip (The first is unknown)
			for (int i = 3; i < possibleBlipCount; i++)
			{
				ulong address = *(RadarBlipPoolAddress + i);

				if (address == 0)
					continue;

				if (CheckBlip(address, position, radius, spriteTypes))
					handles.Add(*(int*)(address + 4));
			}

			return handles.ToArray();

			bool CheckBlip(ulong blipAddress, float[] position, float radius, params int[] spriteTypes)
			{
				if (spriteTypes.Length > 0)
				{
					int spriteIndex = *(int*)(blipAddress + 0x40);
					if (!Array.Exists(spriteTypes, x => x == spriteIndex))
						return false;
				}

				if (position != null && radius > 0f)
				{
					float* blipPosition = stackalloc float[3];

					blipPosition[0] = *(float*)(blipAddress + 0x10);
					blipPosition[1] = *(float*)(blipAddress + 0x14);
					blipPosition[2] = *(float*)(blipAddress + 0x18);

					float x = blipPosition[0] - position[0];
					float y = blipPosition[1] - position[1];
					float z = blipPosition[2] - position[2];
					float distanceSquared = (x * x) + (y * y) + (z * z);
					float radiusSquared = radius * radius;

					if (distanceSquared > radiusSquared)
						return false;
				}

				return true;
			}
		}

		public static int GetNorthBlip()
		{
			if (RadarBlipPoolAddress != null)
			{
				ulong northBlipAddress = *(RadarBlipPoolAddress + 2);

				if (northBlipAddress != 0)
					return *(int*)(northBlipAddress + 4);
			}

			return -1;
		}

		public static IntPtr GetBlipAddress(int handle)
		{
			if (RadarBlipPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			int poolIndexOfHandle = handle & 0xFFFF;
			int possibleBlipCount = *(int*)(RadarBlipPoolAddress - 1);

			if (poolIndexOfHandle >= possibleBlipCount)
			{
				return IntPtr.Zero;
			}

			ulong address = *(RadarBlipPoolAddress + poolIndexOfHandle);

			if (address != 0 && *(int*)(address + 4) == handle)
				return new IntPtr((long)address);

			return IntPtr.Zero;
		}

		#endregion

		#region -- Entity Addresses --

		delegate ulong GetHandleAddressFuncDelegate(int handle);
		static GetHandleAddressFuncDelegate GetPtfxAddressFunc;
		static GetHandleAddressFuncDelegate GetEntityAddressFunc;
		static GetHandleAddressFuncDelegate GetPlayerAddressFunc;

		public static IntPtr GetPtfxAddress(int handle)
		{
			return new IntPtr((long)GetPtfxAddressFunc(handle));
		}
		public static IntPtr GetEntityAddress(int handle)
		{
			return new IntPtr((long)GetEntityAddressFunc(handle));
		}
		public static IntPtr GetPlayerAddress(int handle)
		{
			return new IntPtr((long)GetPlayerAddressFunc(handle));
		}

		delegate ulong GetCheckpointBaseAddressDelegate();
		static GetCheckpointBaseAddressDelegate GetCheckpointBaseAddress;
		delegate ulong GetCheckpointHandleAddressDelegate(ulong baseAddr, int handle);
		static GetCheckpointHandleAddressDelegate GetCheckpointHandleAddress;

		public static IntPtr GetCheckpointAddress(int handle)
		{
			var addr = GetCheckpointHandleAddress(GetCheckpointBaseAddress(), handle);
			if (addr == 0) return IntPtr.Zero;
			return new IntPtr((long)((ulong)(CheckpointPoolAddress) + 96 * ((ulong)*(int*)(addr + 16))));
		}

		#endregion

		#region -- NaturalMotion Euphoria --

		delegate byte SetNmBoolAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, [MarshalAs(UnmanagedType.I1)] bool value);
		delegate byte SetNmIntAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, int value);
		delegate byte SetNmFloatAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float value);
		delegate byte SetNmVector3AddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float x, float y, float z);
		delegate byte SetNmStringAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, IntPtr stringPtr);

		delegate void SendMessageToPedDelegate(ulong pedNmAddress, IntPtr messagePtr, ulong messageAddress);
		delegate void FreeMessageMemoryDelegate(ulong messageAddress);

		delegate void ActionUlongDelegate(ulong T);
		delegate Int32 FuncUlongIntDelegate(ulong T);
		delegate ulong FuncUlongUlongDelegate(ulong T);
		delegate ulong FuncUlongUlongIntUlongDelegate(ulong T1, ulong T2, int T3);

		static ulong GiveNmMessageFuncAddress;
		static ulong CreateNmMessageFuncAddress;
		static SetNmIntAddressDelegate SetNmIntAddress;
		static SetNmBoolAddressDelegate SetNmBoolAddress;
		static SetNmFloatAddressDelegate SetNmFloatAddress;
		static SetNmStringAddressDelegate SetNmStringAddress;
		static SetNmVector3AddressDelegate SetNmVector3Address;

		internal class EuphoriaMessageTask : IScriptTask
		{
			#region Fields
			int targetHandle;
			string message;
			Dictionary<string, object> arguments;
			#endregion

			internal EuphoriaMessageTask(int target, string message, Dictionary<string, object> arguments)
			{
				targetHandle = target;
				this.message = message;
				this.arguments = arguments;
			}

			public void Run()
			{
				throw new NotImplementedException("Euphoria is not currently supported on latest game versions.");

				byte* NativeFunc = (byte*)NativeMemory.CreateNmMessageFuncAddress;
				ulong MessageAddress = GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(new IntPtr((long)(*(int*)(NativeFunc + 0x22) + NativeFunc + 0x26)))(4632);

				if (MessageAddress == 0)
					return;

				GetDelegateForFunctionPointer<FuncUlongUlongIntUlongDelegate>(new IntPtr((long)((*(int*)(NativeFunc + 0x3C)) + NativeFunc + 0x40)))(MessageAddress, MessageAddress + 24, 64);

				foreach (var arg in arguments)
				{
					IntPtr name = ScriptDomain.CurrentDomain.PinString(arg.Key);

					if (arg.Value is int)
						NativeMemory.SetNmIntAddress(MessageAddress, name, (int)arg.Value);
					if (arg.Value is bool)
						NativeMemory.SetNmBoolAddress(MessageAddress, name, (bool)arg.Value);
					if (arg.Value is float)
						NativeMemory.SetNmFloatAddress(MessageAddress, name, (float)arg.Value);
					if (arg.Value is string)
						NativeMemory.SetNmStringAddress(MessageAddress, name, ScriptDomain.CurrentDomain.PinString((string)arg.Value));
					if (arg.Value is float[])
						NativeMemory.SetNmVector3Address(MessageAddress, name, ((float[])arg.Value)[0], ((float[])arg.Value)[1], ((float[])arg.Value)[2]);
				}

				byte* BaseFunc = (byte*)NativeMemory.GiveNmMessageFuncAddress;
				byte* ByteAddr = (*(int*)(BaseFunc + 0xBC) + BaseFunc + 0xC0);
				byte* UnkStrAddr = (*(int*)(BaseFunc + 0xCE) + BaseFunc + 0xD2);
				byte* _PedAddress = (byte*)NativeMemory.GetEntityAddress(targetHandle).ToPointer();
				byte* PedNmAddress;
				bool v5 = false;
				byte v7;
				ulong v11;
				ulong v12;

				if (_PedAddress == null || *(ulong*)(_PedAddress + 48) == 0)
					return;

				PedNmAddress = (byte*)GetDelegateForFunctionPointer<FuncUlongUlongDelegate>(new IntPtr((long)(*(ulong*)(*(ulong*)(_PedAddress) + 88))))((ulong)_PedAddress);

				int MinHealthOffset = NativeMemory.GetGameVersion() < 26 /*v1_0_877_1_Steam*/ ? *(int*)(BaseFunc + 78) : *(int*)(BaseFunc + 157 + *(int*)(BaseFunc + 76));

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
							IntPtr messagePtr = ScriptDomain.CurrentDomain.PinString(message);
							GetDelegateForFunctionPointer<SendMessageToPedDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0x1AA) + BaseFunc + 0x1AE)))((ulong)PedNmAddress, messagePtr, MessageAddress);
						}
						GetDelegateForFunctionPointer<FreeMessageMemoryDelegate>(new IntPtr((long)(*(int*)(BaseFunc + 0x1BB) + BaseFunc + 0x1BF)))(MessageAddress);
					}
				}
			}
		}

		public static void SendEuphoriaMessage(int targetHandle, string message, Dictionary<string, object> arguments)
		{
			var task = new EuphoriaMessageTask(targetHandle, message, arguments);

			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		#endregion
	}
}
