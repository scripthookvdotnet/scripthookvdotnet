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
using System.Linq;
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
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPattern(string pattern, string mask)
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
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPattern(string pattern, string mask, IntPtr startAddress)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
				return null;

			ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

			return FindPattern(pattern, mask, startAddress, size);
		}

		/// <summary>
		/// Searches the specific address space of the current process for a memory pattern.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
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
			IntPtr startAddressToSearch;

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

			// Find entity pools and interior proxy pool
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

			address = FindPattern("\x83\x38\xFF\x74\x27\xD1\xEA\xF6\xC2\x01\x74\x20", "xxxxxxxxxxxx");
			if (address != null)
			{
				BuildingPoolAddress = (ulong*)(*(int*)(address + 47) + address + 51);
				AnimatedBuildingPoolAddress = (ulong*)(*(int*)(address + 15) + address + 19);
			}
			address = FindPattern("\x83\xBB\x80\x01\x00\x00\x01\x75\x12", "xxxxxxxxx");
			if (address != null)
			{
				InteriorInstPoolAddress = (ulong*)(*(int*)(address + 23) + address + 27);
			}
			address = FindPattern("\x0F\x85\xA3\x00\x00\x00\x8B\x52\x0C\x48\x8B\x0D\x00\x00\x00\x00", "xxxxxxxxxxxx????");
			if (address != null)
			{
				InteriorProxyPoolAddress = (ulong*)(*(int*)(address + 12) + address + 16);
			}

			// Find euphoria functions
			address = FindPattern("\x40\x53\x48\x83\xEC\x20\x83\x61\x0C\x00\x44\x89\x41\x08\x49\x63\xC0", "xxxxxxxxxxxxxxxxx");
			InitMessageMemoryFunc = GetDelegateForFunctionPointer<InitMessageMemoryDelegate>(new IntPtr(address));

			address = FindPattern("\x41\x83\xFA\xFF\x74\x4A\x48\x85\xD2\x74\x19", "xxxxxxxxxxx") - 0xE;
			SendMessageToPedFunc = GetDelegateForFunctionPointer<SendMessageToPedDelegate>(new IntPtr(address));

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

			address = FindPattern("\x83\x79\x10\xFF\x7E\x1D\x48\x63\x41\x10", "xxxxxxxxxx");
			GetActiveTaskFunc = GetDelegateForFunctionPointer<GetActiveTaskFuncDelegate>(new IntPtr(address));

			address = FindPattern("\x75\xEF\x48\x8B\x5C\x24\x30\xB8\x00\x00\x00\x00", "xxxxxxxx????");
			if (address != null)
			{
				cTaskNMScriptControlTypeIndex = *(int*)(address + 8);
			}

			address = FindPattern("\x48\x8D\x05\x00\x00\x00\x00\x48\x89\x01\x8B\x44\x24\x50", "xxx????xxxxxxx");
			if (address != null)
			{
				var cEventSwitch2NMVfTableArrayAddr = (ulong)(*(int*)(address + 3) + address + 7);
				var getEventTypeOfcEventSwitch2NMFuncAddr = *(ulong*)(cEventSwitch2NMVfTableArrayAddr + 0x18);
				cEventSwitch2NMTypeIndex = *(int*)(getEventTypeOfcEventSwitch2NMFuncAddr + 1);
			}

			address = FindPattern("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
			GetLabelTextByHashAddress = (ulong)(*(int*)(address + 7) + address + 11);

			address = FindPattern("\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x18\x89\x54\x24\x10\x56\x57\x41\x56\x48\x83\xEC\x20", "xxxxxxxxxxxxxxxxxxxxxx");
			GetLabelTextByHashFunc = GetDelegateForFunctionPointer<GetLabelTextByHashFuncDelegate>(new IntPtr(address));

			address = FindPattern("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
			CheckpointPoolAddress = (ulong*)(*(int*)(address + 17) + address + 21);
			GetCGameScriptHandlerAddressFunc = GetDelegateForFunctionPointer<GetCGameScriptHandlerAddressDelegate>(new IntPtr(*(int*)(address - 19) + address - 15));

			address = FindPattern("\x4C\x8D\x05\x00\x00\x00\x00\x0F\xB7\xC1", "xxx????xxx");
			RadarBlipPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			address = FindPattern("\xFF\xC6\x49\x83\xC6\x08\x3B\x35\x00\x00\x00\x00\x7C\x9B", "xxxxxxxx????xx");
			PossibleRadarBlipCountAddress = (int*)(*(int*)(address + 8) + address + 12);
			address = FindPattern("\x3B\x35\x00\x00\x00\x00\x74\x2E\x48\x81\xFD\xDB\x05\x00\x00", "xx????xxxxxxxxx");
			UnkFirstRadarBlipIndexAddress = (int*)(*(int*)(address + 2) + address + 6);
			address = FindPattern("\x41\xB8\x07\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFC", "xxxxxxxxxx????xxxx");
			NorthRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);
			address = FindPattern("\x41\xB8\x06\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFD", "xxxxxxxxxx????xxxx");
			CenterRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);

			address = FindPattern("\x33\xDB\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x07\x48\x8B\x40\x20\x8B\x58\x18", "xxx????xxxxxxxxxxxx");
			GetLocalPlayerPedAddressFunc = GetDelegateForFunctionPointer<GetLocalPlayerPedAddressFuncDelegate>(new IntPtr(*(int*)(address + 3) + address + 7));

			address = FindPattern("\x4C\x8D\x05\x00\x00\x00\x00\x74\x07\xB8\x00\x00\x00\x00\xEB\x2D\x33\xC0", "xxx????xxx????xxxx");
			waypointInfoArrayStartAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			if (waypointInfoArrayStartAddress != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPattern("\x48\x8D\x15\x00\x00\x00\x00\x48\x83\xC1\x18\xFF\xC0\x48\x3B\xCA\x7C\xEA\x32\xC0", "xxx????xxx????xxxxxxxxxxxxx", startAddressToSearch);
				waypointInfoArrayEndAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}

			address = FindPattern("\x48\x8D\x4C\x24\x20\x41\xB8\x02\x00\x00\x00\xE8\x00\x00\x00\x00\xF3", "xxxxxxxxxxxx????x");
			if (address != null)
			{
				GetRotationFromMatrixFunc = GetDelegateForFunctionPointer<GetRotationFromMatrixDelegate>(new IntPtr(*(int*)(address + 12) + address + 16));
			}
			address = FindPattern("\xF3\x0F\x11\x4D\x38\xF3\x0F\x11\x45\x3C\xE8\x00\x00\x00\x00", "xxxxxxxxxxx????");
			if (address != null)
			{
				GetQuaternionFromMatrixFunc = GetDelegateForFunctionPointer<GetQuaternionFromMatrixDelegate>(new IntPtr(*(int*)(address + 11) + address + 15));
			}

			address = FindPattern("\x48\x8B\x42\x20\x48\x85\xC0\x74\x09\xF3\x0F\x10\x80", "xxxxxxxxxxxxx");
			if (address != null)
			{
				EntityMaxHealthOffset = *(int*)(address + 0x25);
			}

			address = FindPattern("\x75\x11\x48\x8B\x06\x48\x8D\x54\x24\x20\x48\x8B\xCE\xFF\x90", "xxxxxxxxxxxxxxx");
			if (address != null)
			{
				SetAngularVelocityVFuncOfEntityOffset = *(int*)(address + 15);
				GetAngularVelocityVFuncOfEntityOffset = SetAngularVelocityVFuncOfEntityOffset + 0x8;
			}

			address = FindPattern("\x48\x8B\x89\x00\x00\x00\x00\x33\xC0\x44\x8B\xC2\x48\x85\xC9\x74\x20", "xxx????xxxxxxxxxx");
			cAttackerArrayOfEntityOffset = *(uint*)(address + 3); // the correct name is unknown
			if (address != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPattern("\x48\x63\x51\x00\x48\x85\xD2", "xxx?xxx", startAddressToSearch);
				elementCountOfCAttackerArrayOfEntityOffset = (uint)(*(sbyte*)(address + 3));

				startAddressToSearch = new IntPtr(address);
				address = FindPattern("\x48\x83\xC1\x00\x48\x3B\xC2\x7C\xEF", "xxx?xxxxx", startAddressToSearch);
				// the element size might be 0x10 in older builds (the size is 0x18 at least in b1604 and b2372)
				elementSizeOfCAttackerArrayOfEntity = (uint)(*(sbyte*)(address + 3));
			}

			address = FindPattern("\x48\x8B\x0B\x33\xD2\xE8\x00\x00\x00\x00\x89\x03", "xxxxxx????xx");
			GetHashKeyFunc = GetDelegateForFunctionPointer<GetHashKeyDelegate>(new IntPtr(*(int*)(address + 6) + address + 10));

			address = FindPattern("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
			cursorSpriteAddr = (int*)(*(int*)(address - 4) + address);

			address = FindPattern("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05\x00\x00\x00\x00", "xxxxxx????xxxxxxxxx????");
			readWorldGravityAddress = (float*)(*(int*)(address + 19) + address + 23);
			writeWorldGravityAddress = (float*)(*(int*)(address + 6) + address + 10);

			address = FindPattern("\xF3\x0F\x11\x05\x00\x00\x00\x00\xF3\x0F\x10\x08\x0F\x2F\xC8\x73\x03\x0F\x28\xC1\x48\x83\xC0\x04\x49\x2B", "xxxx????xxxxxxxxxxxxxxxxxx");
			var timeScaleArrayAddress = (float*)(*(int*)(address + 4) + address + 8);
			if (timeScaleArrayAddress != null)
				// SET_TIME_SCALE changes the 2nd element, so obtain the address of it
				timeScaleAddress = timeScaleArrayAddress + 1;

			address = FindPattern("\x66\x0F\x6E\x05\x00\x00\x00\x00\x0F\x57\xF6", "xxxx????xxx");
			millisecondsPerGameMinuteAddress = (int*)(*(int*)(address + 4) + address + 8);

			address = FindPattern("\x75\x2D\x44\x38\x3D\x00\x00\x00\x00\x75\x24", "xxxxx????xx");
			isClockPausedAddress = (bool*)(*(int*)(address + 5) + address + 9);

			// Find camera objects
			address = FindPattern("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
			CameraPoolAddress = (ulong*)(*(int*)(address) + address + 4);
			address = FindPattern("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
			address = address + *(int*)(address) + 4;
			GameplayCameraAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			// Find model hash table
			address = FindPattern("\x3C\x05\x75\x16\x8B\x81\x00\x00\x00\x00", "xxxxxx????");
			if (address != null)
				VehicleTypeOffsetInModelInfo = *(int*)(address + 6);

			address = FindPattern("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			uint vehicleClassOffset = *(uint*)(address + 0x31);

			address = address + *(int*)(address) + 4;
			modelNum1 = *(UInt32*)(*(int*)(address + 0x52) + address + 0x56);
			modelNum2 = *(UInt64*)(*(int*)(address + 0x63) + address + 0x67);
			modelNum3 = *(UInt64*)(*(int*)(address + 0x7A) + address + 0x7E);
			modelNum4 = *(UInt64*)(*(int*)(address + 0x81) + address + 0x85);
			modelHashTable = *(UInt64*)(*(int*)(address + 0x24) + address + 0x28);
			modelHashEntries = *(UInt16*)(address + *(int*)(address + 3) + 7);

			address = FindPattern("\x33\xD2\x00\x8B\xD0\x00\x2B\x05\x00\x00\x00\x00\xC1\xE6\x10", "xx?xx?xx????xxx");
			modelInfoArrayPtr = (ulong*)(*(int*)(address + 8) + address + 12);

			address = FindPattern("\x8B\x54\x00\x00\x00\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x8A\xC3", "xx???xx????x????xx");
			cStreamingAddr = (ulong*)(*(int*)(address + 7) + address + 11);

			address = FindPattern("\x48\x8B\x05\x00\x00\x00\x00\x41\x8B\x1E", "xxx????xxx");
			weaponAndAmmoInfoArrayPtr = (RageAtArrayPtr*)(*(int*)(address + 3) + address + 7);

			address = FindPattern("\x48\x85\xC0\x74\x08\x8B\x90\x00\x00\x00\x00\xEB\x02", "xxxxxxx????xx");
			weaponInfoHumanNameHashOffset = *(int*)(address + 7);

			address = FindPattern("\x8B\x05\x00\x00\x00\x00\x44\x8B\xD3\x8D\x48\xFF", "xx????xxxxxx");
			if (address != null)
			{
				weaponComponentArrayCountAddr = (uint*)(*(int*)(address + 2) + address + 6);

				address = FindPattern("\x46\x8D\x04\x11\x48\x8D\x15\x00\x00\x00\x00\x41\xD1\xF8", "xxxxxxx????xxx", new IntPtr(address));
				offsetForCWeaponComponentArrayAddr = (ulong)(address + 7);

				address = FindPattern("\x74\x10\x49\x8B\xC9\xE8\x00\x00\x00\x00", "xxxxxx????", new IntPtr(address));
				var findAttachPointFuncAddr = new IntPtr((long)(*(int*)(address + 6) + address + 10));

				address = FindPattern("\x4C\x8D\x81\x00\x00\x00\x00", "xxx????", findAttachPointFuncAddr);
				weaponAttachPointsStartOffset = *(int*)(address + 3);
				address = FindPattern("\x4D\x63\x98\x00\x00\x00\x00", "xxx????", new IntPtr(address));
				weaponAttachPointsArrayCountOffset = *(int*)(address + 3);
				address = FindPattern("\x4C\x63\x50\x00", "xxx?", new IntPtr(address));
				weaponAttachPointElementComponentCountOffset = *(byte*)(address + 3);
				address = FindPattern("\x48\x83\xC0\x00", "xxx?", new IntPtr(address));
				weaponAttachPointElementSize = *(byte*)(address + 3);
			}

			address = FindPattern("\x24\x1F\x3C\x05\x0F\x85\x00\x00\x00\x00\x48\x8D\x82\x00\x00\x00\x00", "xxxxxx????xxx????");
			if (address != null)
			{
				vehicleMakeNameOffsetInModelInfo = *(int*)(address + 13);
			}

			address = FindPattern("\x33\xD2\x48\x85\xC0\x74\x1E\x0F\xBF\x88\x00\x00\x00\x00\x48\x8B\x05\x00\x00\x00\x00", "xxxxxxxxxx????xxx????");
			if (address != null)
			{
				pedPersonalityIndexOffsetInModelInfo = *(int*)(address + 10);
				pedPersonalitiesArrayAddr = (ulong*)(*(int*)(address + 17) + address + 21);
			}

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
			address = FindPattern("\x74\x2D\x0F\x57\xC0\x0F\x2F\x83\x00\x00\x00\x00", "xxxxxxxx????");
			if (address != null)
			{
				OilLevelOffset = *(int*)(address + 8);
			}

			address = FindPattern("\xF3\x0F\x10\x8F\x10\x0A\x00\x00\xF3\x0F\x59\x05\x5E\x30\x8D\x00", "xxxx????xxxx????");
			if (address != null)
			{
				WheelSpeedOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x48\x63\x99\x00\x00\x00\x00\x45\x33\xC0\x45\x8B\xD0\x48\x85\xDB", "xxx????xxxxxxxxx");
			if (address != null)
			{
				WheelCountOffset = *(int*)(address + 3);
				WheelPtrArrayOffset = WheelCountOffset - 8;
				WheelBoneIdToPtrArrayIndexOffset = WheelCountOffset + 4;
			}

			address = FindPattern("\x74\x18\x80\xA0\x00\x00\x00\x00\xBF\x84\xDB\x0F\x94\xC1\x80\xE1\x01\xC0\xE1\x06", "xxxx????xxxxxxxxxxxx");
			if (address != null)
			{
				CanWheelBreakOffset = *(int*)(address + 4);
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

			address = FindPattern("\x48\x89\x5C\x24\x28\x44\x0F\x29\x40\xC8\x0F\x28\xF9\x44\x0F\x29\x48\xB8\xF3\x0F\x11\xB9", "xxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				var modifyVehicleTopSpeedOffset1 = *(int*)(address - 4);
				var modifyVehicleTopSpeedOffset2 = *(int*)(address + 22);
				EnginePowerMultiplierOffset = modifyVehicleTopSpeedOffset1 + modifyVehicleTopSpeedOffset2;
			}

			address = FindPattern("\x74\x00\xF6\x00\x00\x00\x00\x00\x80\x75\x00\x33\x00\x48\x8D", "x?x?????xx?x?xx");
			if (address != null)
			{
				DisablePretendOccupantOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x74\x4A\x80\x7A\x28\x03\x75\x44\xF6\x82\x00\x00\x00\x00\x04", "xxxxxxxxxx????x");
			if (address != null)
			{
				VehicleProvidesCoverOffset = *(int*)(address + 10);
			}

			address = FindPattern("\xF3\x44\x0F\x59\x93\x00\x00\x00\x00\x48\x8B\xCB\xF3\x44\x0F\x59\x97\x00\x00\x00\x00", "xxxxx????xxxxxxxx????");
			if (address != null)
			{
				VehicleLightsMultiplierOffset = *(int*)(address + 5);
			}

			address = FindPattern("\xFD\x02\xDB\x08\x98\x00\x00\x00\x00\x48\x8B\x5C\x24\x30", "xxxxx????xxxxx");
			if (address != null)
			{
				IsInteriorLightOnOffset = *(int*)(address - 4);
				IsEngineStartingOffset = IsInteriorLightOnOffset + 1;
			}

			address = FindPattern("\x84\xC0\x75\x09\x8A\x9F\x00\x00\x00\x00\x80\xE3\x01\x8A\xC3\x48\x8B\x5C\x24\x30", "xxxxxx????xxxxxxxxxx");
			if (address != null)
			{
				IsHeadlightDamagedOffset = *(int*)(address + 6);
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

			address = FindPattern("\x0F\x84\xE0\x02\x00\x00\xF3\x0F\x10\x05\x00\x00\x00\x00\x41\x0F\x2F\x86\x00\x00\x00\x00", "xxxxxxxxxx????xxxx????");
			if (address != null)
			{
				VehicleLodMultiplierOffset = *(int*)(address + 18);
			}

			address = FindPattern("\x83\xB8\x00\x00\x00\x00\x0A\x77\x12\x80\xA0\x00\x00\x00\x00\xFD", "xx????xxxxx????x");
			if (address != null)
			{
				VehicleTypeOffsetInCVehicle = *(int*)(address + 2);
				VehicleDropsMoneyWhenBlownUpOffset = *(int*)(address + 11);
			}

			address = FindPattern("\x73\x1E\xF3\x41\x0F\x59\x86\x00\x00\x00\x00\xF3\x0F\x59\xC2\xF3\x0F\x59\xC7", "xxxxxxx????xxxxxxxx");
			if (address != null)
			{
				HeliBladesSpeedOffset = *(int*)(address + 7);
			}

			{
				string patternForHeliHealthOffsets = "\x48\x85\xC0\x74\x18\x8B\x88\x00\x00\x00\x00\x83\xE9\x08\x83\xF9\x01\x77\x0A\xF3\x0F\x10\x80\x00\x00\x00\x00";
				string maskForHeliHealthOffsets = "xxxxxxx????xxxxxxxxxxxx????";
				startAddressToSearch = Process.GetCurrentProcess().MainModule.BaseAddress;

				int[] heliHealthOffsets = new int[3];

				// the pattern will match 3 times
				for (int i = 0; i < 3; i++)
				{
					address = FindPattern(patternForHeliHealthOffsets, maskForHeliHealthOffsets, startAddressToSearch);

					if (address != null)
					{
						heliHealthOffsets[i] = *(int*)(address + 23);
						startAddressToSearch = new IntPtr((long)(address + patternForHeliHealthOffsets.Length));
					}
				}

				if (!Array.Exists(heliHealthOffsets, (x => x == 0)))
				{
					Array.Sort<int>(heliHealthOffsets);
					HeliMainRotorHealthOffset = heliHealthOffsets[0];
					HeliTailRotorHealthOffset = heliHealthOffsets[1];
					HeliTailBoomHealthOffset = heliHealthOffsets[2];
				}
			}

			address = FindPattern("\x3C\x03\x0F\x85\x00\x00\x00\x00\x48\x8B\x41\x20\x48\x8B\x88", "xxxx????xxxxxxx");
			if (address != null)
			{
				HandlingDataOffset = *(int*)(address + 22);
			}

			address = FindPattern("\x48\x85\xC0\x74\x3C\x8B\x80\x00\x00\x00\x00\xC1\xE8\x0F", "xxxxxxx????xxx");
			if (address != null)
			{
				FirstVehicleFlagsOffset = *(int*)(address + 7);
			}

			address = FindPattern("\xF3\x0F\x59\x05\x00\x00\x00\x00\xF3\x0F\x59\x83\x00\x00\x00\x00\xF3\x0F\x10\xC8\x0F\xC6\xC9\x00", "xxxx????xxxx????xxxxxxxx");
			if (address != null)
			{
				VehicleWheelSteeringLimitMultiplierOffset = *(int*)(address + 12);
			}

			address = FindPattern("\xF3\x0F\x5C\xC8\x0F\x2F\xCB\xF3\x0F\x11\x89\x00\x00\x00\x00\x72\x10\xF3\x0F\x10\x1D", "xxxxxxxxxxx????xxxxxx");
			if (address != null)
			{
				VehicleWheelTemperatureOffset = *(int*)(address + 11);
			}

			address = FindPattern("\x74\x13\x0F\x57\xC0\x0F\x2E\x80\x00\x00\x00\x00", "xxxxxxxx????");
			if (address != null)
			{
				VehicleTireHealthOffset = *(int*)(address + 8);
				VehicleWheelHealthOffset = VehicleTireHealthOffset - 4;
			}

			// the tire wear multipiler value for vehicle wheels is present only in v1.0.1868.0 or newer versions
			if (gameVersion >= 54)
			{
				address = FindPattern("\x76\x31\x0F\x2E\xB3\x00\x00\x00\x00\x75\x28\xC7\x83\x00\x00\x00\x00\x00\x00\x00\x00\x0F\xBA\xB3\x00\x00\x00\x00\x1E", "xxxxx????xxxx????????xxx????x");
				if (address != null)
				{
					VehicleTireWearMultiplierOffset = *(int*)(address + 5);
				}
			}

			address = FindPattern("\x0F\xBF\x88\x00\x00\x00\x00\x3B\xCA\x74\x17", "xxx????xxxx");
			if (address != null)
			{
				VehicleWheelIdOffset = *(int*)(address + 3);
			}

			address = FindPattern("\xEB\x02\x33\xC9\xF6\x81\x00\x00\x00\x00\x01\x75\x43", "xxxxxx????xxx");
			if (address != null)
			{
				VehicleWheelTouchingFlagsOffset = *(int*)(address + 6);
			}

			address = FindPattern("\x74\x21\x8B\xD7\x48\x8B\xCB\xE8\x00\x00\x00\x00\x48\x8B\xC8\xE8\x00\x00\x00\x00", "xxxxxxxx????xxxx????");
			if (address != null)
			{
				FixVehicleWheelFunc = GetDelegateForFunctionPointer<FixVehicleWheelDelegate>(new IntPtr(*(int*)(address + 16) + address + 20));
				address = FindPattern("\x80\xA1\x00\x00\x00\x00\xFD", "xx????x", new IntPtr(address + 20));
				ShouldShowOnlyVehicleTiresWithPositiveHealthOffset = *(int*)(address + 2);
			}

			address = FindPattern("\x4C\x8B\x81\x28\x01\x00\x00\x0F\x29\x70\xE8\x0F\x29\x78\xD8", "xxxxxxxxxxxxxxx");
			if (address != null)
			{
				PunctureVehicleTireNewFunc = GetDelegateForFunctionPointer<PunctureVehicleTireNewDelegate>(new IntPtr((long)(address - 0x10)));
				address = FindPattern("\x48\x83\xEC\x50\x48\x8B\x81\x00\x00\x00\x00\x48\x8B\xF1\xF6\x80", "xxxxxxx????xxxxx");
				BurstVehicleTireOnRimNewFunc = GetDelegateForFunctionPointer<BurstVehicleTireOnRimNewDelegate>(new IntPtr((long)(address - 0xB)));
			}
			else
			{
				address = FindPattern("\x41\xF6\x81\x00\x00\x00\x00\x20\x0F\x29\x70\xE8\x0F\x29\x78\xD8\x49\x8B\xF9", "xxx????xxxxxxxxxxxx");
				if (address != null)
				{
					PunctureVehicleTireOldFunc = GetDelegateForFunctionPointer<PunctureVehicleTireOldDelegate>(new IntPtr((long)(address - 0x14)));
					address = FindPattern("\x48\x83\xEC\x50\xF6\x82\x00\x00\x00\x00\x20\x48\x8B\xF2\x48\x8B\xE9", "xxxxxx????xxxxxxx");
					BurstVehicleTireOnRimOldFunc = GetDelegateForFunctionPointer<BurstVehicleTireOnRimOldDelegate>(new IntPtr((long)(address - 0x10)));
				}
			}

			address = FindPattern("\x8B\x00\x00\x00\x00\x00\x0F\xBA\x00\x00\x00\x00\x00\x09\x8B\x05", "x?????xx?????xxx");
			if (address != null)
			{
				PedIntelligenceOffset = *(int*)(address + 2);
			}

			address = FindPattern("\x48\x8B\x88\x00\x00\x00\x00\x48\x85\xC9\x74\x43\x48\x85\xD2", "xxx????xxxxxxxx");
			if (address != null)
			{
				CTaskTreePedOffset = *(int*)(address + 3);
			}

			address = FindPattern("\x40\x38\x3D\x00\x00\x00\x00\x8B\xB6\x00\x00\x00\x00\x74\x0C", "xxx????xx????xx");
			if (address != null)
			{
				CEventCountOffset = *(int*)(address + 9);
				address = FindPattern("\x48\x8B\xB4\xC6\x00\x00\x00\x00", "xxxx????", new IntPtr(address));
				CEventStackOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x0F\x29\x4D\xF0\x48\x8B\x92\x00\x00\x00\x00", "xxxxxxx????");
			if (address != null)
			{
				fragInstNMGtaOffset = *(int*)(address + 7);
			}

			address = FindPattern("\xF3\x44\x0F\x10\xAB\x00\x00\x00\x00\x0F\x5B\xC9\xF3\x45\x0F\x5C\xD4", "xxxxx????xxxxxxxx");
			if (address != null)
			{
				SweatOffset = *(int*)(address + 5);
			}

			address = FindPattern("\x8A\x41\x30\xC0\xE8\x03\xA8\x01\x74\x49\x8B\x82\x00\x00\x00\x00", "xxxxxxxxxxxx????");
			if (address != null)
			{
				PedIsInVehicleOffset = *(int*)(address + 12);
				PedLastVehicleOffset = *(int*)(address + 0x1A);
			}
			address = FindPattern("\x24\x3F\x0F\xB6\xC0\x66\x89\x87\x00\x00\x00\x00", "xxxxxxxx????");
			if (address != null)
			{
				SeatIndexOffset = *(int*)(address + 8);
			}

			address = FindPattern("\x74\x14\x8B\x88\x00\x00\x00\x00\x81\xE1\x00\x40\x00\x00\x31\x88", "xxxx????xxxxxxxx");
			if (address != null)
			{
				PedDropsWeaponsWhenDeadOffset = *(int*)(address + 4);
			}

			address = FindPattern("\x8B\x88\x00\x00\x00\x00\x83\xE1\x04\x31\x88\x00\x00\x00\x00\x55\x48\x8D\x2D", "xx????xxxxx????xxxx");
			if (address != null)
			{
				PedSuffersCriticalHitOffset = *(int*)(address + 2);
			}

			address = FindPattern("\x48\x8D\x99\x00\x00\x00\x00\x0F\x29\x74\x24\x20\x48\x8B\xF1", "xxx????xxxxxxxx");
			if (address != null)
			{
				ArmorOffset = *(int*)(address + 3);
			}

			address = FindPattern("\x49\x3B\xF6\x75\xD3\xF3\x0F\x10\x9F\x00\x00\x00\x00", "xxxxxxxxx????");
			if (address != null)
			{
				InjuryHealthThresholdOffset = *(int*)(address + 9);
			}

			address = FindPattern("\x75\xD0\xF3\x0F\x10\x83\x00\x00\x00\x00\x41\x0F\x2F\x06", "xxxxxx????xxxx");
			if (address != null)
			{
				FatalInjuryHealthThresholdOffset = *(int*)(address + 6);
			}

			address = FindPattern("\x8B\x83\x00\x00\x00\x00\x8B\x35\x00\x00\x00\x00\x3B\xF0\x76\x04", "xx????xx????xxxx");
			if (address != null)
			{
				PedTimeOfDeathOffset = *(int*)(address + 2);
				PedCauseOfDeathOffset = PedTimeOfDeathOffset - 4;
				PedSourceOfDeathOffset = PedTimeOfDeathOffset - 12;
			}

			address = FindPattern("\x74\x08\x8B\x81\x00\x00\x00\x00\xEB\x0D\x48\x8B\x87\x00\x00\x00\x00\x8B\x80", "xxxx????xxxxx????xx");
			if (address != null)
			{
				FiringPatternOffset = *(int*)(address + 19);
			}

			address = FindPattern("\xC1\xE8\x09\xA8\x01\x74\xAE\x0F\x28\x00\x00\x00\x00\x00\x49\x8B\x47\x30\xF3\x0F\x10\x81", "xxxxxxxxx????xxxxxxxxx");
			if (address != null)
			{
				SeeingRangeOffset = *(int*)(address + 9);
				HearingRangeOffset = SeeingRangeOffset - 4;
				VisualFieldMinAngleOffset = SeeingRangeOffset + 8;
				VisualFieldMaxAngleOffset = SeeingRangeOffset + 0xC;
				VisualFieldMinElevationAngleOffset = SeeingRangeOffset + 0x10;
				VisualFieldMaxElevationAngleOffset = SeeingRangeOffset + 0x14;
				VisualFieldPeripheralRangeOffset = SeeingRangeOffset + 0x18;
				VisualFieldCenterAngleOffset = SeeingRangeOffset + 0x1C;
			}

			address = FindPattern("\x48\x8D\x1D\x00\x00\x00\x00\x4C\x8B\x0B\x4D\x85\xC9\x74\x67", "xxx????xxxxxxxx");
			if (address != null)
			{
				ProjectilePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}
			// Find address of the projectile count, just in case the max number of projectile changes from 50
			address = FindPattern("\x44\x8B\x0D\x00\x00\x00\x00\x33\xDB\x45\x8A\xF8", "xxx????xxxxx");
			if (address != null)
			{
				ProjectileCountAddress = (int*)(*(int*)(address + 3) + address + 7);
			}
			address = FindPattern("\x48\x85\xED\x74\x09\x48\x39\xA9\x00\x00\x00\x00\x75\x2D", "xxxxxxxx????xx");
			if (address != null)
			{
				ProjectileOwnerOffset = *(int*)(address + 8);
			}
			address = FindPattern("\x45\x85\xF6\x74\x0D\x48\x8B\x81\x00\x00\x00\x00\x44\x39\x70\x10", "xxxxxxxx????xxxx");
			if (address != null)
			{
				ProjectileAmmoInfoOffset = *(int*)(address + 8);
			}
			address = FindPattern("\x39\x70\x10\x75\x17\x40\x84\xED\x74\x09\x33\xD2\xE8", "xxxxxxxxxxxxx");
			if (address != null)
			{
				ExplodeProjectileFunc = GetDelegateForFunctionPointer<ExplodeProjectileDelegate>(new IntPtr(*(int*)(address + 13) + address + 17));
			}

			address = FindPattern("\x0F\xBE\x5E\x06\x48\x8B\xCF\xFF\x50\x00\x8B\xD3\x48\x8B\xC8\xE8\x00\x00\x00\x00\x8B\x4E\x00", "xxxxxxxxx?xxxxxx????xx?");
			if (address != null)
			{
				getFragInstVFuncOffset = *(sbyte*)(address + 9);
				detachFragmentPartByIndexFunc = GetDelegateForFunctionPointer<DetachFragmentPartByIndexDelegate>(new IntPtr(*(int*)(address + 16) + address + 20));
			}
			address = FindPattern("\x00\x8B\x0D\x00\x00\x00\x00\x00\x83\x64\x00\x00\x00\x00\x0F\xB7\xD1\x00\x33\xC9\xE8", "?xx?????xx????xxx?xxx");
			if (address != null)
			{
				phSimulatorInstPtr = (ulong**)(*(int*)(address + 3) + address + 7);
			}
			address = FindPattern("\x00\x63\x00\x00\x00\x00\x00\x3B\x00\x00\x00\x00\x00\x0F\x8D\x00\x00\x00\x00\x00\x8B\xC8", "?x?????x?????xx?????xx");
			if (address != null)
			{
				colliderCountOffset = *(int*)(address + 3);
				colliderCapacityOffset = *(int*)(address + 9);
			}

			address = FindPattern("\x7E\x63\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20", "xxxxxxxxxxxx");
			if (address != null)
			{
				InteriorProxyPtrFromGameplayCamAddress = (ulong*)(*(int*)(address + 37) + address + 41);
				InteriorInstPtrInInteriorProxyOffset = (int)*(byte*)(address + 49);
			}


			// Generate vehicle model list
			var vehicleHashesGroupedByClass = new List<int>[0x20];
			for (int i = 0; i < 0x20; i++)
				vehicleHashesGroupedByClass[i] = new List<int>();

			var vehicleHashesGroupedByType = new List<int>[0x10];
			for (int i = 0; i < 0x10; i++)
				vehicleHashesGroupedByType[i] = new List<int>();

			var weaponObjectHashes = new List<int>();
			var pedHashes = new List<int>();

			// The game will crash when it load these vehicles because of the stub vehicle models
			var stubVehicles = new HashSet<uint> {
				0xA71D0D4F, /* astron2 */
				0x170341C2, /* cyclone2 */
				0x5C54030C, /* arbitergt */
				0x39085F47, /* ignus2 */
				0x438F6593, /* s95 */
			};

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
										// Avoid loading stub vehicles since it will crash the game
										if (stubVehicles.Contains((uint)cur->hash))
											continue;
										vehicleHashesGroupedByClass[*(byte*)(addr2 + vehicleClassOffset) & 0x1F].Add(cur->hash);

										// Normalize the value to vehicle type range for b944 or later versions if current game version is earlier than b944.
										// The values for CAmphibiousAutomobile and CAmphibiousQuadBike were inserted between those for CSubmarineCar and CHeli in b944.
										int vehicleTypeInt = *(int*)((byte*)addr2 + VehicleTypeOffsetInModelInfo);
										if (gameVersion < 28 && vehicleTypeInt >= 6)
											vehicleTypeInt += 2;
										vehicleHashesGroupedByType[vehicleTypeInt].Add(cur->hash);

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
				vehicleResult[i] = Array.AsReadOnly(vehicleHashesGroupedByClass[i].ToArray());
			VehicleModels = Array.AsReadOnly(vehicleResult);

			vehicleResult = new ReadOnlyCollection<int>[0x10];
			for (int i = 0; i < 0x10; i++)
				vehicleResult[i] = Array.AsReadOnly(vehicleHashesGroupedByType[i].ToArray());
			VehicleModelsGroupedByType = Array.AsReadOnly(vehicleResult);

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

			string enableCarsGlobalPattern;
			if (gameVersion >= 80)
			{
				// b2802 has 3 additional opcodes between CALL opcode (0x5D) and GLOBAL_U24 opcode (0x61 in b2802)
				enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x71\x2E\x00\x01\x62\x00\x00\x00\x00\x04\x00\x71\x2E\x00\x01";
			}
			else if (gameVersion >= 46)
			{
				enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01";
			}
			else
			{
				enableCarsGlobalPattern = "\x2D\x00\x00\x00\x00\x2C\x01\x00\x00\x56\x04\x00\x6E\x2E\x00\x01\x5F\x00\x00\x00\x00\x04\x00\x6E\x2E\x00\x01";
			}
			var enableCarsGlobalMask = gameVersion >= 46 ? "x??xxxx??xxxxx?xx????xxxx?x" : "xx??xxxxxx?xx????xxxx?x";
			var enableCarsGlobalOffset = gameVersion >= 46 ? 17 : 13;

			for (int i = 0; i < shopControllerHeader->CodePageCount(); i++)
			{
				int size = shopControllerHeader->GetCodePageSize(i);
				if (size > 0)
				{
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
		/// Writes a single 64-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteAddress(IntPtr address, IntPtr value)
		{
			var data = (long*)address.ToPointer();
			*data = value.ToInt64();
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
		/// <returns><see langword="true" /> if the bit is set, <see langword="false" /> if it is unset.</returns>
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
		static byte[] _strBufferForStringToCoTaskMemUTF8 = new byte[100];

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

			int byteCountUtf8 = Encoding.UTF8.GetByteCount(s);
			if (byteCountUtf8 > _strBufferForStringToCoTaskMemUTF8.Length)
			{
				_strBufferForStringToCoTaskMemUTF8 = new byte[byteCountUtf8 * 2];
			}

			Encoding.UTF8.GetBytes(s, 0, s.Length, _strBufferForStringToCoTaskMemUTF8, 0);
			IntPtr dest = AllocCoTaskMem(byteCountUtf8 + 1);
			if (dest == IntPtr.Zero)
				throw new OutOfMemoryException();

			Copy(_strBufferForStringToCoTaskMemUTF8, 0, dest, byteCountUtf8);
			// Add null-terminator to end
			((byte*)dest.ToPointer())[byteCountUtf8] = 0;

			return dest;
		}

		#region -- RAGE classes --

		[StructLayout(LayoutKind.Explicit, Size = 0xC)]
		struct RageAtArrayPtr
		{
			[FieldOffset(0x0)]
			internal ulong* data;
			[FieldOffset(0x8)]
			internal ushort size;
			[FieldOffset(0xA)]
			internal ushort capacity;

			internal ulong GetElementAddress(int i)
			{
				return data[i];
			}
		}

		#endregion

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

		static int* millisecondsPerGameMinuteAddress;

		public static int MillisecondsPerGameMinute
		{
			set { *millisecondsPerGameMinuteAddress = value; }
		}

		static bool* isClockPausedAddress;

		public static bool IsClockPaused
		{
			get { return *isClockPausedAddress; }
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

		static CrSkeleton* GetCrSkeletonOfEntityHandle(int handle) => GetCrSkeletonOfEntity(new IntPtr((long)GetEntityAddressFunc(handle)));
		static CrSkeleton* GetCrSkeletonOfEntity(IntPtr entityAddress)
		{
			var fragInst = GetFragInstAddressOfEntity(entityAddress);
			// Return value will not be null if the entity is a CVehicle or a CPed
			if (fragInst != null)
			{
				return GetEntityCrSkeletonOfFragInst(fragInst);
			}
			else
			{
				ulong unkAddr = *(ulong*)(entityAddress + 80);
				if (unkAddr == 0)
				{
					return null;
				}
				else
				{
					return (CrSkeleton*)*(ulong*)(unkAddr + 40);
				}
			}
		}
		static CrSkeleton* GetEntityCrSkeletonOfFragInst(FragInst* fragInst)
		{
			var fragCacheEntry = fragInst->fragCacheEntry;
			var gtaFragType = fragInst->gtaFragType;

			// Check if either pointer is null just like native functions that take a bone index argument
			if (fragCacheEntry == null || gtaFragType == null)
				return null;

			return fragCacheEntry->crSkeleton;
		}

		public static int GetEntityBoneCount(int handle)
		{
			var crSkeleton = GetCrSkeletonOfEntityHandle(handle);
			return crSkeleton != null ? crSkeleton->boneCount : 0;
		}
		public static IntPtr GetEntityBonePoseAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var crSkeletonData = GetCrSkeletonOfEntityHandle(handle);
			if (crSkeletonData == null)
				return IntPtr.Zero;

			if (boneIndex < crSkeletonData->boneCount)
			{
				return crSkeletonData->GetBonePoseMatrixAddress(boneIndex);
			}

			return IntPtr.Zero;
		}
		public static IntPtr GetEntityBoneMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var crSkeletonData = GetCrSkeletonOfEntityHandle(handle);
			if (crSkeletonData == null)
				return IntPtr.Zero;

			if (boneIndex < crSkeletonData->boneCount)
			{
				return crSkeletonData->GetBoneMatrixAddress(boneIndex);
			}

			return IntPtr.Zero;
		}

		#endregion

		#region -- CEntity Functions --

		public delegate float* GetRotationFromMatrixDelegate(float* returnRotationArrayAddress, ulong matrixAddress, int rotationOrder);
		public delegate int GetQuaternionFromMatrixDelegate(float* returnQuaternionArrayAddress, ulong matrixAddress);
		static GetRotationFromMatrixDelegate GetRotationFromMatrixFunc;
		static GetQuaternionFromMatrixDelegate GetQuaternionFromMatrixFunc;

		public static void GetRotationFromMatrix(float* returnRotationArray, IntPtr matrixAddress, int rotationOrder = 2)
		{
			GetRotationFromMatrixFunc(returnRotationArray, (ulong)matrixAddress.ToInt64(), rotationOrder);

			const float RAD_2_DEG = 57.2957763671875f; // 0x42652EE0 in hex. Exactly the same value as the GET_ENTITY_ROTATION multiplies the rotation values in radian by.
			returnRotationArray[0] *= RAD_2_DEG;
			returnRotationArray[1] *= RAD_2_DEG;
			returnRotationArray[2] *= RAD_2_DEG;
		}
		public static void GetQuaternionFromMatrix(float* returnRotationArray, IntPtr matrixAddress)
		{
			GetQuaternionFromMatrixFunc(returnRotationArray, (ulong)matrixAddress.ToInt64());
		}

		#endregion

		#region -- CPhysical Offsets --

		public static int EntityMaxHealthOffset { get; }
		public static int SetAngularVelocityVFuncOfEntityOffset { get; }
		public static int GetAngularVelocityVFuncOfEntityOffset { get; }

		public static uint cAttackerArrayOfEntityOffset { get; }
		public static uint elementCountOfCAttackerArrayOfEntityOffset { get; }
		public static uint elementSizeOfCAttackerArrayOfEntity { get; }

		#endregion

		#region -- CPhysical Functions --

		public delegate void SetEntityAngularVelocityDelegate(IntPtr entityAddress, float* angularVelocity);
		// return value will be the address of the temporary 4 float storage
		public delegate float* GetEntityAngularVelocityDelegate(IntPtr entityAddress);

		// Only 2 virtural functions are present (one for peds and objects and one for vehicles)
		static Dictionary<ulong, SetEntityAngularVelocityDelegate> setEntityAngularVelocityVFuncCache = new Dictionary<ulong, SetEntityAngularVelocityDelegate>(2);
		static Dictionary<ulong, GetEntityAngularVelocityDelegate> getEntityAngularVelocityVFuncCache = new Dictionary<ulong, GetEntityAngularVelocityDelegate>(2);

		internal class SetEntityAngularVelocityTask : IScriptTask
		{
			#region Fields
			IntPtr entityAddress;
			SetEntityAngularVelocityDelegate setAngularVelocityDelegate;
			float x, y, z;
			#endregion

			internal SetEntityAngularVelocityTask(IntPtr entityAddress, SetEntityAngularVelocityDelegate vFuncDelegate, float x, float y, float z)
			{
				this.entityAddress = entityAddress;
				this.setAngularVelocityDelegate = vFuncDelegate;
				this.x = x;
				this.y = y;
				this.z = z;
			}

			public void Run()
			{
				var angularVelocity = stackalloc float[4];
				angularVelocity[0] = x;
				angularVelocity[1] = y;
				angularVelocity[2] = z;

				setAngularVelocityDelegate(entityAddress, angularVelocity);
			}
		}

		public static float* GetEntityAngularVelocity(IntPtr entityAddress)
		{
			var vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)GetAngularVelocityVFuncOfEntityOffset);
			var getEntityAngularVelocity = CreateGetEntityAngularVelocityDelegateIfNotCreated(vFuncAddr);

			return getEntityAngularVelocity(entityAddress);
		}

		public static void SetEntityAngularVelocity(IntPtr entityAddress, float x, float y, float z)
		{
			var vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)SetAngularVelocityVFuncOfEntityOffset);
			var setEntityAngularVelocityDelegate = CreateSetEntityAngularVelocityDelegateIfNotCreated(vFuncAddr);

			var task = new SetEntityAngularVelocityTask(entityAddress, setEntityAngularVelocityDelegate, x, y, z);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		static SetEntityAngularVelocityDelegate CreateSetEntityAngularVelocityDelegateIfNotCreated(ulong virtualFuncAddr)
		{
			if (setEntityAngularVelocityVFuncCache.TryGetValue(virtualFuncAddr, out var outDelegate))
			{
				return outDelegate;
			}
			else
			{
				var newDelegate = GetDelegateForFunctionPointer<SetEntityAngularVelocityDelegate>(new IntPtr((long)virtualFuncAddr));
				setEntityAngularVelocityVFuncCache.Add(virtualFuncAddr, newDelegate);

				return newDelegate;
			}
		}

		static GetEntityAngularVelocityDelegate CreateGetEntityAngularVelocityDelegateIfNotCreated(ulong virtualFuncAddr)
		{
			if (getEntityAngularVelocityVFuncCache.TryGetValue(virtualFuncAddr, out var outDelegate))
			{
				return outDelegate;
			}
			else
			{
				var newDelegate = GetDelegateForFunctionPointer<GetEntityAngularVelocityDelegate>(new IntPtr((long)virtualFuncAddr));
				getEntityAngularVelocityVFuncCache.Add(virtualFuncAddr, newDelegate);

				return newDelegate;
			}
		}

		#endregion

		#region -- CPhysical Data --

		// the size is at least 0x10 in all game versions
		[StructLayout(LayoutKind.Explicit, Size = 0x10)]
		struct CAttacker
		{
			[FieldOffset(0x0)]
			internal ulong attackerEntityAddress;
			[FieldOffset(0x8)]
			internal int weaponHash;
			[FieldOffset(0xC)]
			internal int gameTime;
		}

		public static bool IsIndexOfEntityDamageRecordValid(IntPtr entityAddress, uint index)
		{
			if (index < 0 ||
				cAttackerArrayOfEntityOffset == 0 ||
				elementCountOfCAttackerArrayOfEntityOffset == 0 ||
				elementSizeOfCAttackerArrayOfEntity == 0)
				return false;

			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return false;

			var entryCount = *(int*)(entityCAttackerArrayAddress + elementCountOfCAttackerArrayOfEntityOffset);

			return index < entryCount;
		}
		static (int attackerHandle, int weaponHash, int gameTime) GetEntityDamageRecordEntryAtIndexInternal(ulong cAttackerArrayAddress, uint index)
		{
			var cAttacker = (CAttacker*)(cAttackerArrayAddress + index * elementSizeOfCAttackerArrayOfEntity);

			var attackerEntityAddress = cAttacker->attackerEntityAddress;
			var weaponHash = cAttacker->weaponHash;
			var gameTime = cAttacker->gameTime;
			var attackerHandle = attackerEntityAddress != 0 ? GetEntityHandleFromAddress(new IntPtr((long)attackerEntityAddress)) : 0;

			return (attackerHandle, weaponHash, gameTime);
		}
		public static (int attackerHandle, int weaponHash, int gameTime) GetEntityDamageRecordEntryAtIndex(IntPtr entityAddress, uint index)
		{
			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return default((int attackerHandle, int weaponHash, int gameTime));

			return GetEntityDamageRecordEntryAtIndexInternal(entityCAttackerArrayAddress, index);
		}

		public static (int attackerHandle, int weaponHash, int gameTime)[] GetEntityDamageRecordEntries(IntPtr entityAddress)
		{
			if (cAttackerArrayOfEntityOffset == 0 ||
				elementCountOfCAttackerArrayOfEntityOffset == 0 ||
				elementSizeOfCAttackerArrayOfEntity == 0)
				return Array.Empty<(int handle, int weaponHash, int gameTime)>();

			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return Array.Empty<(int attackerHandle, int weaponHash, int gameTime)>();

			var returnEntrySize = *(int*)(entityCAttackerArrayAddress + elementCountOfCAttackerArrayOfEntityOffset);
			var returnEntries = returnEntrySize != 0 ? new (int attackerHandle, int weaponHash, int gameTime)[returnEntrySize] : Array.Empty<(int attackerHandle, int weaponHash, int gameTime)>();

			for (uint i = 0; i < returnEntries.Length; i++)
			{
				returnEntries[i] = GetEntityDamageRecordEntryAtIndexInternal(entityCAttackerArrayAddress, i);
			}

			return returnEntries;
		}

		#endregion

		#region -- CPed Data --

		/// <summary>
		/// <para>Gets the last vehicle the ped used or is using.</para>
		/// <para>
		/// This method exists because there are no reliable ways with native functions.
		/// The native <c>GET_VEHICLE_PED_IS_IN</c> returns the last vehicle the ped used when not in vehicle (though the 2nd parameter name is supposed to be <c>ConsiderEnteringAsInVehicle</c> as a leaked header suggests),
        /// but returns <c>0</c> when the ped is going to a door of some vehicle or opening one.
		/// Also, the native returns the vehicle's handle the ped is getting in when ped is getting in it, which is different from the last vehicle this method returns and the native <c>RESET_PED_LAST_VEHICLE</c> changes.
		/// </para>
		/// </summary>
		public static int GetLastVehicleHandleOfPed(IntPtr pedAddress)
		{
			if (PedLastVehicleOffset == 0)
				return 0;

			var lastVehicleAddress = new IntPtr(*(long*)(pedAddress + PedLastVehicleOffset));
			return lastVehicleAddress != IntPtr.Zero ? GetEntityHandleFromAddress(lastVehicleAddress) : 0;
		}

		/// <summary>
		/// <para>Gets the current vehicle the ped is using.</para>
		/// <para>
		/// This method exists because <c>GET_VEHICLE_PED_IS_IN</c> always returns the last vehicle without checking the driving flag in b2699 even when the 2nd argument is set to <c>false</c> unlike previous versions.
		/// </para>
		/// </summary>
		public static int GetVehicleHandlePedIsIn(IntPtr pedAddress)
		{
			if (PedIsInVehicleOffset == 0 || PedLastVehicleOffset == 0)
				return 0;

			var bitFlags = *(uint*)(pedAddress + PedIsInVehicleOffset);
			bool isPedInVehicle = ((bitFlags & (1 << 0x1E)) != 0);
			if (!isPedInVehicle)
				return 0;

			var lastVehicleAddress = new IntPtr(*(long*)(pedAddress + PedLastVehicleOffset));
			return lastVehicleAddress != IntPtr.Zero ? GetEntityHandleFromAddress(lastVehicleAddress) : 0;
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
		public static int OilLevelOffset { get; }

		public static int VehicleTypeOffsetInCVehicle { get; }

		public static int WheelPtrArrayOffset { get; }
		public static int WheelCountOffset { get; }
		public static int WheelBoneIdToPtrArrayIndexOffset { get; }
		public static int WheelSpeedOffset { get; }
		public static int CanWheelBreakOffset { get; }

		public static int SteeringAngleOffset { get; }
		public static int SteeringScaleOffset { get; }
		public static int ThrottlePowerOffset { get; }
		public static int BrakePowerOffset { get; }

		public static int EngineTemperatureOffset { get; }
		public static int EnginePowerMultiplierOffset { get; }

		public static int DisablePretendOccupantOffset { get; }

		public static int VehicleProvidesCoverOffset { get; }

		public static int VehicleLightsMultiplierOffset { get; }

		public static int IsInteriorLightOnOffset { get; }
		public static int IsEngineStartingOffset { get; }

		public static int IsWantedOffset { get; }

		public static int IsHeadlightDamagedOffset { get; }

		public static int PreviouslyOwnedByPlayerOffset { get; }
		public static int NeedsToBeHotwiredOffset { get; }

		public static int AlarmTimeOffset { get; }

		public static int VehicleLodMultiplierOffset { get; }

		public static int VehicleDropsMoneyWhenBlownUpOffset { get; }

		public static int HeliBladesSpeedOffset { get; }

		public static int HeliMainRotorHealthOffset { get; }
		public static int HeliTailRotorHealthOffset { get; }
		public static int HeliTailBoomHealthOffset { get; }

		public static int HandlingDataOffset { get; }

		public static int FirstVehicleFlagsOffset { get; }

		#endregion

		#region -- Vehicle Wheel Data --

		delegate void FixVehicleWheelDelegate(IntPtr wheelAddress);
		delegate void PunctureVehicleTireNewDelegate(IntPtr wheelPtr, ulong unkPtr, float damage, ulong unkIntReturnPtrParam, ulong unkFloatReturnPtrParam, int unkIntParam, byte unkByteParam, [MarshalAs(UnmanagedType.I1)] bool someBoolFlagForMultiplayer);
		delegate void PunctureVehicleTireOldDelegate(IntPtr wheelPtr, ulong unkPtr, float damage, IntPtr vehiclePtr, ulong unkIntReturnPtrParam, ulong unkFloatReturnPtrParam, int unkIntParam, byte unkByteParam, [MarshalAs(UnmanagedType.I1)] bool someBoolFlagForMultiplayer);
		delegate void BurstVehicleTireOnRimNewDelegate(IntPtr wheelPtr);
		delegate void BurstVehicleTireOnRimOldDelegate(IntPtr wheelPtr, IntPtr vehiclePtr);

		static FixVehicleWheelDelegate FixVehicleWheelFunc;
		static PunctureVehicleTireNewDelegate PunctureVehicleTireNewFunc;
		static PunctureVehicleTireOldDelegate PunctureVehicleTireOldFunc;
		static BurstVehicleTireOnRimNewDelegate BurstVehicleTireOnRimNewFunc;
		static BurstVehicleTireOnRimOldDelegate BurstVehicleTireOnRimOldFunc;

		public static int VehicleWheelSteeringLimitMultiplierOffset { get; }

		public static int VehicleWheelTemperatureOffset { get; }

		public static int VehicleWheelHealthOffset { get; }

		public static int VehicleTireHealthOffset { get; }

		public static int VehicleTireWearMultiplierOffset { get; }

		// the on fire offset is the same as this offset
		public static int VehicleWheelTouchingFlagsOffset { get; }

		public static int VehicleWheelIdOffset { get; }

		public static int ShouldShowOnlyVehicleTiresWithPositiveHealthOffset { get; }

		public static void FixVehicleWheel(IntPtr wheelAddress) => FixVehicleWheelFunc(wheelAddress);

		public static IntPtr GetVehicleWheelAddressByIndexOfWheelArray(IntPtr vehicleAddress, int index)
		{
			var vehicleWheelArrayAddr = *(ulong**)(vehicleAddress + SHVDN.NativeMemory.WheelPtrArrayOffset);

			if (vehicleWheelArrayAddr == null)
				return IntPtr.Zero;

			return new IntPtr((long)*(vehicleWheelArrayAddr + index));
		}

		public static bool IsWheelTouchingSurface(IntPtr wheelAddress, IntPtr vehicleAddress)
		{
			if (VehicleWheelTouchingFlagsOffset == 0)
				return false;

			var wheelTouchingFlag = *(uint*)(wheelAddress + VehicleWheelTouchingFlagsOffset).ToPointer();
			if ((wheelTouchingFlag & 1) != 0)
				return true;

			#region Slower Check
			if (((wheelTouchingFlag >> 1) & 1) == 0)
				return false;

			var phCollider = *(ulong*)(*(ulong*)(vehicleAddress + 0x50).ToPointer() + 0x50);
			if (phCollider == 0)
				return true;
			var unkStructAddr = *(ulong*)(phCollider + 0x18);
			if (unkStructAddr == 0)
				return false;

			return (*(uint*)(unkStructAddr + 0x14) & 0xFFFFFFFD) == 0;
			#endregion
		}

		static bool VehicleWheelHasVehiclePtr() => PunctureVehicleTireNewFunc != null;

		public static void PunctureTire(IntPtr wheelAddress, float damage, IntPtr vehicleAddress)
		{
			var task = new VehicleWheelPunctureTask(wheelAddress, vehicleAddress, false, damage);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		public static void BurstTireOnRim(IntPtr wheelAddress, IntPtr vehicleAddress)
		{
			var task = new VehicleWheelPunctureTask(wheelAddress, vehicleAddress, true);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		// the function BurstVehicleTireOnRimNew(Old)Func calls must be called in the main thread or the game will crash
		// the function PunctureVehicleTireNew(Old)Func calls should be called in the main thread or the game might crash in some cases
		internal class VehicleWheelPunctureTask : IScriptTask
		{
			#region Fields
			IntPtr wheelAddress;
			IntPtr vehicleAddress;
			bool burstWheelCompletely;
			float damage;
			#endregion

			internal VehicleWheelPunctureTask(IntPtr wheelAddress, IntPtr vehicleAddress, bool burstWheelCompletely, float damage = 1000f)
			{
				this.wheelAddress = wheelAddress;
				this.vehicleAddress = vehicleAddress;
				this.burstWheelCompletely = burstWheelCompletely;
				this.damage = damage;
			}

			public void Run()
			{
				int outValInt;
				float outValFloat;

				if (VehicleWheelHasVehiclePtr())
				{
					PunctureVehicleTireNewFunc(wheelAddress, 0, damage, (ulong)&outValInt, (ulong)&outValFloat, 3, 0, true);
					if (burstWheelCompletely)
						BurstVehicleTireOnRimNewFunc(wheelAddress);
				}
				else
				{
					PunctureVehicleTireOldFunc(wheelAddress, 0, damage, vehicleAddress, (ulong)&outValInt, (ulong)&outValFloat, 3, 0, true);
					if (burstWheelCompletely)
						BurstVehicleTireOnRimOldFunc(wheelAddress, vehicleAddress);
				}
			}
		}

		#endregion

		#region -- Ped Offsets --

		public static int SweatOffset { get; }

		// the same offset as the offset for SET_PED_CAN_BE_TARGETTED
		public static int PedDropsWeaponsWhenDeadOffset { get; }

		public static int PedSuffersCriticalHitOffset { get; }

		public static int ArmorOffset { get; }

		public static int InjuryHealthThresholdOffset { get; }
		public static int FatalInjuryHealthThresholdOffset { get; }

		public static int PedIsInVehicleOffset { get; }
		public static int PedLastVehicleOffset { get; }
		public static int SeatIndexOffset { get; }

		public static int PedSourceOfDeathOffset { get; }
		public static int PedCauseOfDeathOffset { get; }
		public static int PedTimeOfDeathOffset { get; }

		#region -- Ped Intelligence Offsets --

		public static int PedIntelligenceOffset { get; }

		public static int FiringPatternOffset { get; }

		public static int SeeingRangeOffset { get; }
		public static int HearingRangeOffset { get; }
		public static int VisualFieldMinAngleOffset { get; }
		public static int VisualFieldMaxAngleOffset { get; }
		public static int VisualFieldMinElevationAngleOffset { get; }
		public static int VisualFieldMaxElevationAngleOffset { get; }
		public static int VisualFieldPeripheralRangeOffset { get; }
		public static int VisualFieldCenterAngleOffset { get; }

		static int CTaskTreePedOffset { get; }

		static int CEventCountOffset { get; }

		static int CEventStackOffset { get; }

		#endregion

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
			None = -1,
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
			Bicycle = 0xC,
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

		[StructLayout(LayoutKind.Explicit, Size = 0x400)]
		struct CModelList
		{
			[FieldOffset(0x0)]
			internal fixed uint modelMemberIndices[0x100];
		}

		[StructLayout(LayoutKind.Explicit, Size = 0xB8)]
		struct PedPersonality
		{
			[FieldOffset(0x7C)]
			internal bool isMale;
			[FieldOffset(0x7D)]
			internal bool isHuman;
			[FieldOffset(0x7F)]
			internal bool isGang;
		}

		static int vehicleMakeNameOffsetInModelInfo;
		static int VehicleTypeOffsetInModelInfo;
		static int handlingIndexOffsetInModelInfo;
		static int pedPersonalityIndexOffsetInModelInfo;
		static UInt32 modelNum1;
		static UInt64 modelNum2;
		static UInt64 modelNum3;
		static UInt64 modelNum4;
		static UInt64 modelHashTable;
		static UInt16 modelHashEntries;
		static ulong* modelInfoArrayPtr;
		static ulong* cStreamingAddr;
		static ulong* pedPersonalitiesArrayAddr;

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
				int typeInt = (*(int*)((byte*)modelInfoAddress.ToPointer() + VehicleTypeOffsetInModelInfo));

				// Normalize the value to vehicle type range for b944 or later versions if current game version is earlier than b944.
				// The values for CAmphibiousAutomobile and CAmphibiousQuadBike were inserted between those for CSubmarineCar and CHeli in b944.
				if (GetGameVersion() < 28 && typeInt >= 6)
					typeInt += 2;

				return (VehicleStructClassType)typeInt;
			}

			return VehicleStructClassType.None;
		}
		public static int GetVehicleType(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);

			if (modelInfo == IntPtr.Zero)
				return -1;

			return (int)GetVehicleStructClass(modelInfo);
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
		public static int GetModelHashFromEntity(IntPtr entityAddress)
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

		static IntPtr GetModelInfoByIndex(uint index)
		{
			if (modelInfoArrayPtr == null || index < 0)
				return IntPtr.Zero;

			ulong modelInfoArrayFirstElemPtr = *modelInfoArrayPtr;

			return new IntPtr(*(long*)(modelInfoArrayFirstElemPtr + index * 0x8));
		}
		public static List<int> GetLoadedAppropriateVehicleHashes()
		{
			return GetLoadedHashesOfModelList(0x2D00);
		}
		public static List<int> GetLoadedAppropriatePedHashes()
		{
			return GetLoadedHashesOfModelList(0x4504);
		}
		internal static List<int> GetLoadedHashesOfModelList(int startOffsetOfCStreaming)
		{
			if (modelInfoArrayPtr == null || cStreamingAddr == null)
				return new List<int>();

			var resultList = new List<int>();

			const int MAX_MODEL_LIST_ELEMENT_COUNT = 256;
			var modelSet = (CModelList*)((ulong)cStreamingAddr + (uint)startOffsetOfCStreaming);
			for (uint i = 0; i < MAX_MODEL_LIST_ELEMENT_COUNT; i++)
			{
				uint indexOfModelInfo = modelSet->modelMemberIndices[i];

				if (indexOfModelInfo == 0xFFFF)
					break;

				resultList.Add(GetModelHashFromFwArcheType(GetModelInfoByIndex(indexOfModelInfo)));
			}

			return resultList;
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
		public static bool IsModelAMotorcycle(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Bike;
		}
		public static bool IsModelASubmarine(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Submarine;
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
		public static bool IsModelAMlo(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			return GetModelInfoClass(modelInfo) == ModelInfoClassType.Mlo;
		}

		public static string GetVehicleMakeName(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);

			if (GetModelInfoClass(modelInfo) == ModelInfoClassType.Vehicle)
			{
				return PtrToStringUTF8(modelInfo + vehicleMakeNameOffsetInModelInfo);
			}

			return "CARNOTFOUND";
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
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModelsGroupedByType { get; }
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

		private static PedPersonality* GetPedPersonalityElementAddress(IntPtr modelInfoAddress)
		{
			if (modelInfoAddress == IntPtr.Zero ||
				pedPersonalitiesArrayAddr == null ||
				pedPersonalityIndexOffsetInModelInfo == 0 ||
				*(ulong*)pedPersonalitiesArrayAddr == 0)
				return null;

			if (GetModelInfoClass(modelInfoAddress) != ModelInfoClassType.Ped)
				return null;

			// This values is not likely to be changed in further updates
			const int PED_PERSONALITY_ELEMENT_SIZE = 0xB8;

			var indexOfPedPersonality = *(ushort*)(modelInfoAddress + pedPersonalityIndexOffsetInModelInfo).ToPointer();
			return (PedPersonality*)(*(ulong*)pedPersonalitiesArrayAddr + (uint)(indexOfPedPersonality * PED_PERSONALITY_ELEMENT_SIZE));
		}
		public static bool IsModelAMalePed(int modelHash)
		{
			var pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
				return false;

			return pedPersonalityAddress->isMale;
		}
		public static bool IsModelAFemalePed(int modelHash)
		{
			var pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
				return false;

			return !pedPersonalityAddress->isMale;
		}
		public static bool IsModelHumanPed(int modelHash)
		{
			var pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
				return false;

			return pedPersonalityAddress->isHuman;
		}
		public static bool IsModelAnAnimalPed(int modelHash)
		{
			var pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
				return false;

			return !pedPersonalityAddress->isHuman;
		}
		public static bool IsModelAGangPed(int modelHash)
		{
			var pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
				return false;

			return pedPersonalityAddress->isGang;
		}

		#endregion

		#region -- Entity Pools --

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
			public bool IsHandleValid(int handle)
			{
				uint handleUInt = (uint)handle;
				var index = handleUInt >> 8;
				return GetCounter(index) == (handleUInt & 0xFFu);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ulong GetAddress(uint index)
			{
				return ((Mask(index) & (poolStartAddress + index * itemSize)));
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public IntPtr GetAddressFromHandle(int handle)
			{
				return IsHandleValid(handle) ? new IntPtr((long)GetAddress((uint)handle >> 8)) : IntPtr.Zero;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetGuidHandleByIndex(uint index)
			{
				return IsValid(index) ? (int)((index << 8) + GetCounter(index)) : 0;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetGuidHandleFromAddress(ulong address)
			{
				if (address < poolStartAddress || address >= poolStartAddress + size * itemSize)
					return 0;

				var offset = address - poolStartAddress;
				if (offset % itemSize != 0)
					return 0;

				var indexOfPool = (uint)(offset / itemSize);
				return (int)((indexOfPool << 8) + GetCounter(indexOfPool));
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private byte GetCounter(uint index)
			{
				unsafe
				{
					byte* byteArrayPtr = (byte*)byteArray.ToPointer();
					return (byte)(byteArrayPtr[index] & 0x7F);
				}
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
		static ulong* BuildingPoolAddress;
		static ulong* AnimatedBuildingPoolAddress;
		static ulong* InteriorInstPoolAddress;
		static ulong* InteriorProxyPoolAddress;

		static ulong* ProjectilePoolAddress;
		static int* ProjectileCountAddress;

		delegate ulong EntityPosFuncDelegate(ulong address, float* position);
		delegate ulong EntityModel1FuncDelegate(ulong address);
		delegate ulong EntityModel2FuncDelegate(ulong address);
		delegate int AddEntityToPoolFuncDelegate(ulong address);

		// if the entity is a ped and they are in a vehicle, the vehicle position will be returned instead (just like GET_ENTITY_COORDS does)
		static EntityPosFuncDelegate EntityPosFunc;
		static EntityModel1FuncDelegate EntityModel1Func;
		static EntityModel2FuncDelegate EntityModel2Func;
		static AddEntityToPoolFuncDelegate AddEntityToPoolFunc;

		internal class EntityPoolTask : IScriptTask
		{
			#region Fields
			internal Type poolType;
			internal int[] handles = Array.Empty<int>(); // Assign the reserved empty int array to avoid NullReferenceException in edge cases (e.g. at the very beginning of game session launching)
			internal bool doPosCheck;
			internal bool doModelCheck;
			internal int[] modelHashes;
			internal float radiusSquared;
			internal float[] position;

			// We should avoid wasting (temp) arrays many times by casually using List, but ArrayPool is not available in .NET Framework. So prepare resource pools manually
			static int[] _vehicleHandleBuffer;
			static int[] _pedHandleBuffer;
			static int[] _objectHandleBuffer;
			static int[] _pickupObjectHandleBuffer;
			static int[] _projectileHandleBuffer = new int[50];
			#endregion

			internal enum Type
			{
				Ped = 1,
				Object = 2,
				Vehicle = 4,
				PickupObject = 8,
				Projectile = 16,
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
					float* position = stackalloc float[4];

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
				}

				return true;
			}

			int CopyCPhysicalHandlesFromArrayGenericPool(GenericPool* pool, ref int[] handleBuffer)
			{
				int returnEntityCount = 0;

				uint entityCountInPool = pool->itemCount;
				if (handleBuffer == null)
					handleBuffer = new int[(int)entityCountInPool * 2];
				else if (entityCountInPool > handleBuffer.Length)
					handleBuffer = new int[CalculateAppropriateExtendedArrayLength(handleBuffer, (int)entityCountInPool)];

				uint poolSize = pool->size;
				for (uint i = 0; i < poolSize; i++)
				{
					if (pool->IsValid(i))
					{
						ulong address = pool->GetAddress(i);
						if (CheckEntity(address))
							AddElementAndReallocateIfLengthIsNotLongEnough(ref handleBuffer, returnEntityCount++, NativeMemory.AddEntityToPoolFunc(address));
					}
				}

				return returnEntityCount;
			}

			public void Run()
			{
				if (*NativeMemory.EntityPoolAddress == 0)
					return;

				EntityPool* entityPool = (EntityPool*)(*NativeMemory.EntityPoolAddress);

				#region Store Entity Handles to Buffer Arrays
				int vehicleCountStored = 0;
				if (HasFlagFast(poolType, Type.Vehicle) && *NativeMemory.VehiclePoolAddress != 0)
				{
					VehiclePool* vehiclePool = *(VehiclePool**)(*NativeMemory.VehiclePoolAddress);

					uint vehicleCountInPool = vehiclePool->itemCount;
					if (_vehicleHandleBuffer == null)
						_vehicleHandleBuffer = new int[(int)vehicleCountInPool * 2];
					else if (_vehicleHandleBuffer == null || vehicleCountInPool > _vehicleHandleBuffer.Length)
						_vehicleHandleBuffer = new int[CalculateAppropriateExtendedArrayLength(_vehicleHandleBuffer, (int)vehicleCountInPool)];

					uint poolSize = vehiclePool->size;
					for (uint i = 0; i < poolSize; i++)
					{
						if (entityPool->IsFull())
							break;

						if (vehiclePool->IsValid(i))
						{
							ulong address = vehiclePool->GetAddress(i);
							if (CheckEntity(address))
								AddElementAndReallocateIfLengthIsNotLongEnough(ref _vehicleHandleBuffer, vehicleCountStored++, NativeMemory.AddEntityToPoolFunc(address));
						}
					}
				}

				int pedCountStored = 0;
				if (HasFlagFast(poolType, Type.Ped) && *NativeMemory.PedPoolAddress != 0)
				{
					GenericPool* pedPool = (GenericPool*)(*NativeMemory.PedPoolAddress);
					pedCountStored = CopyCPhysicalHandlesFromArrayGenericPool(pedPool, ref _pedHandleBuffer);
				}

				int objectCountStored = 0;
				if (HasFlagFast(poolType, Type.Object) && *NativeMemory.ObjectPoolAddress != 0)
				{
					GenericPool* objectPool = (GenericPool*)(*NativeMemory.ObjectPoolAddress);
					objectCountStored = CopyCPhysicalHandlesFromArrayGenericPool(objectPool, ref _objectHandleBuffer);
				}

				int pickupCountStored = 0;
				if (HasFlagFast(poolType, Type.PickupObject) && *NativeMemory.PickupObjectPoolAddress != 0)
				{
					GenericPool* pickupPool = (GenericPool*)(*NativeMemory.PickupObjectPoolAddress);
					pickupCountStored = CopyCPhysicalHandlesFromArrayGenericPool(pickupPool, ref _pickupObjectHandleBuffer);
				}

				int projectileCountStored = 0;
				if (HasFlagFast(poolType, Type.Projectile) && NativeMemory.ProjectilePoolAddress != null)
				{
					int projectilesLeft = NativeMemory.GetProjectileCount();
					int projectileCapacity = NativeMemory.GetProjectileCapacity();
					ulong* projectilePoolAddress = NativeMemory.ProjectilePoolAddress;

					int projectileCountInPool = projectilesLeft;
					if (_projectileHandleBuffer == null)
						_projectileHandleBuffer = new int[(int)projectileCountInPool * 2];
					else if (projectileCountInPool > _projectileHandleBuffer.Length)
						_projectileHandleBuffer = new int[CalculateAppropriateExtendedArrayLength(_projectileHandleBuffer, (int)projectileCountInPool)];

					for (uint i = 0; (projectilesLeft > 0 && i < projectileCapacity); i++)
					{
						ulong entityAddress = (ulong)ReadAddress(new IntPtr(projectilePoolAddress + i)).ToInt64();

						if (entityAddress == 0)
							continue;

						projectilesLeft--;

						if (CheckEntity(entityAddress))
							AddElementAndReallocateIfLengthIsNotLongEnough(ref _projectileHandleBuffer, projectileCountStored++, NativeMemory.AddEntityToPoolFunc(entityAddress));
					}
				}
				#endregion

				#region Copy Entity Handles to a New Result Array
				int totalEntityCount = vehicleCountStored + pedCountStored + objectCountStored + pickupCountStored + projectileCountStored;
				if (totalEntityCount == 0)
					return;

				handles = new int[totalEntityCount];
				int currentStartIndexToCopy = 0;

				if (vehicleCountStored != 0)
				{
					Array.Copy(_vehicleHandleBuffer, 0, handles, currentStartIndexToCopy, vehicleCountStored);
					currentStartIndexToCopy += vehicleCountStored;
				}
				if (pedCountStored != 0)
				{
					Array.Copy(_pedHandleBuffer, 0, handles, currentStartIndexToCopy, pedCountStored);
					currentStartIndexToCopy += pedCountStored;
				}
				if (objectCountStored != 0)
				{
					Array.Copy(_objectHandleBuffer, 0, handles, currentStartIndexToCopy, objectCountStored);
					currentStartIndexToCopy += objectCountStored;
				}
				if (pickupCountStored != 0)
				{
					Array.Copy(_pickupObjectHandleBuffer, 0, handles, currentStartIndexToCopy, pickupCountStored);
					currentStartIndexToCopy += pickupCountStored;
				}
				if (projectileCountStored != 0)
				{
					Array.Copy(_projectileHandleBuffer, 0, handles, currentStartIndexToCopy, projectileCountStored);
					currentStartIndexToCopy += projectileCountStored;
				}
				#endregion

				// Enum.HasFlag causes the boxing in .NET Framework and much slower than manually comparing enum flags with bitwise AND
				bool HasFlagFast(Type poolTypeValue, Type flag) => (poolTypeValue & flag) == flag;
			}
		}

		internal class GetEntityHandleTask : IScriptTask
		{
			#region Fields
			internal ulong entityAddress;
			internal int returnEntityHandle;
			#endregion

			internal GetEntityHandleTask(IntPtr entityAddress)
			{
				this.entityAddress = (ulong)entityAddress.ToInt64();
			}

			public void Run()
			{
				returnEntityHandle = NativeMemory.AddEntityToPoolFunc(entityAddress);
			}
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
		public static int GetPedCount() => PedPoolAddress != null ? GetGenericPoolCount(*PedPoolAddress) : 0;
		public static int GetObjectCount() => ObjectPoolAddress != null ? GetGenericPoolCount(*ObjectPoolAddress) : 0;
		public static int GetPickupObjectCount() => PickupObjectPoolAddress != null ? GetGenericPoolCount(*PickupObjectPoolAddress) : 0;
		public static int GetBuildingCount() => BuildingPoolAddress != null ? GetGenericPoolCount(*BuildingPoolAddress) : 0;
		public static int GetAnimatedBuildingCount() => AnimatedBuildingPoolAddress != null ? GetGenericPoolCount(*AnimatedBuildingPoolAddress) : 0;
		public static int GetInteriorInstCount() => InteriorInstPoolAddress != null ? GetGenericPoolCount(*InteriorInstPoolAddress) : 0;
		public static int GetInteriorProxyCount() => InteriorProxyPoolAddress != null ? GetGenericPoolCount(*InteriorProxyPoolAddress) : 0;
		public static int GetProjectileCount() => ProjectileCountAddress != null ? *ProjectileCountAddress : 0;
		static int GetGenericPoolCount(ulong address)
		{
			GenericPool* pool = (GenericPool*)(address);
			return (int)pool->itemCount;
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
		public static int GetPedCapacity() => PedPoolAddress != null ? GetGenericPoolCapacity(*PedPoolAddress) : 0;
		public static int GetObjectCapacity() => ObjectPoolAddress != null ? GetGenericPoolCapacity(*ObjectPoolAddress) : 0;
		public static int GetPickupObjectCapacity() => PickupObjectPoolAddress != null ? GetGenericPoolCapacity(*PickupObjectPoolAddress) : 0;
		public static int GetBuildingCapacity() => BuildingPoolAddress != null ? GetGenericPoolCapacity(*BuildingPoolAddress) : 0;
		public static int GetAnimatedBuildingCapacity() => AnimatedBuildingPoolAddress != null ? GetGenericPoolCapacity(*AnimatedBuildingPoolAddress) : 0;
		public static int GetInteriorInstCapacity() => InteriorInstPoolAddress != null ? GetGenericPoolCapacity(*InteriorInstPoolAddress) : 0;
		public static int GetInteriorProxyCapacity() => InteriorProxyPoolAddress != null ? GetGenericPoolCapacity(*InteriorProxyPoolAddress) : 0;
		//the max number of projectile has not been changed from 50
		public static int GetProjectileCapacity() => 50;
		static int GetGenericPoolCapacity(ulong address)
		{
			GenericPool* pool = (GenericPool*)(address);
			return (int)pool->size;
		}

		public static int[] GetPedHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
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

			return task.handles;
		}

		public static int[] GetPropHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Object);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
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

			return task.handles;
		}

		public static int[] GetEntityHandles()
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}
		public static int[] GetEntityHandles(float[] position, float radius)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Ped | EntityPoolTask.Type.Object | EntityPoolTask.Type.Vehicle);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}

		public static int[] GetVehicleHandles(int[] modelHashes = null)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Vehicle);
			task.modelHashes = modelHashes;
			task.doModelCheck = modelHashes != null && modelHashes.Length > 0;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
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

			return task.handles;
		}

		public static int[] GetPickupObjectHandles()
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}
		public static int[] GetPickupObjectHandles(float[] position, float radius)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.PickupObject);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}
		public static int[] GetProjectileHandles()
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Projectile);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}
		public static int[] GetProjectileHandles(float[] position, float radius)
		{
			var task = new EntityPoolTask(EntityPoolTask.Type.Projectile);
			task.position = position;
			task.radiusSquared = radius * radius;
			task.doPosCheck = true;

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.handles;
		}

		public static int[] GetBuildingHandles()
		{
			if (BuildingPoolAddress == null)
				return Array.Empty<int>();

			return GetHandlesInGenericPool(*NativeMemory.BuildingPoolAddress);
		}

		public static int[] GetBuildingHandles(float[] position, float radius)
		{
			if (BuildingPoolAddress == null)
				return Array.Empty<int>();

			return GetCEntityHandlesInRange(*NativeMemory.BuildingPoolAddress, position, radius);
		}

		public static int[] GetAnimatedBuildingHandles()
		{
			if (AnimatedBuildingPoolAddress == null)
				return Array.Empty<int>();

			return GetHandlesInGenericPool(*NativeMemory.AnimatedBuildingPoolAddress);
		}

		public static int[] GetAnimatedBuildingHandles(float[] position, float radius)
		{
			if (AnimatedBuildingPoolAddress == null)
				return Array.Empty<int>();

			return GetCEntityHandlesInRange(*NativeMemory.AnimatedBuildingPoolAddress, position, radius);
		}

		public static int[] GetInteriorInstHandles()
		{
			if (InteriorInstPoolAddress == null)
				return Array.Empty<int>();

			return GetHandlesInGenericPool(*NativeMemory.InteriorInstPoolAddress);
		}

		public static int[] GetInteriorInstHandles(float[] position, float radius)
		{
			if (InteriorInstPoolAddress == null)
				return Array.Empty<int>();

			return GetCEntityHandlesInRange(*NativeMemory.InteriorInstPoolAddress, position, radius);
		}

		public static int[] GetInteriorProxyHandles()
		{
			if (InteriorProxyPoolAddress == null)
				return Array.Empty<int>();

			return GetHandlesInGenericPool(*NativeMemory.InteriorProxyPoolAddress);
		}

		public static int[] GetInteriorProxyHandles(float[] position, float radius)
		{
			if (InteriorProxyPoolAddress == null)
				return Array.Empty<int>();

			GenericPool* pool = (GenericPool*)(*NativeMemory.InteriorProxyPoolAddress);

			// CInteriorProxy is not a subclass of CEntity and position data is placed at different offset
			var returnHandles = new List<int>();
			var poolSize = pool->size;
			float radiusSquared = radius * radius;
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
					continue;

				var address = pool->GetAddress(i);

				float x = *(float*)(address + 0x70) - position[0];
				float y = *(float*)(address + 0x74) - position[1];
				float z = *(float*)(address + 0x78) - position[2];

				float distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
					continue;

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		public static int GetEntityHandleFromAddress(IntPtr address)
		{
			var task = new GetEntityHandleTask(address);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.returnEntityHandle;
		}

		public static bool BuildingHandleExists(int handle) => BuildingPoolAddress != null ? ((GenericPool*)(*BuildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool AnimatedBuildingHandleExists(int handle) => AnimatedBuildingPoolAddress != null ? ((GenericPool*)(*AnimatedBuildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorInstHandleExists(int handle) => InteriorInstPoolAddress != null ? ((GenericPool*)(*InteriorInstPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorProxyHandleExists(int handle) => InteriorProxyPoolAddress != null ? ((GenericPool*)(*InteriorProxyPoolAddress))->IsHandleValid(handle) : false;

		static int[] GetHandlesInGenericPool(ulong poolAddress)
		{
			GenericPool* pool = (GenericPool*)poolAddress;

			var returnHandles = new List<int>(pool->itemCount);
			var poolSize = pool->size;
			for (uint i = 0; i < poolSize; i++)
			{
				if (pool->IsValid(i))
				{
					returnHandles.Add(pool->GetGuidHandleByIndex(i));
				}
			}

			return returnHandles.ToArray();
		}

		static int[] GetCEntityHandlesInRange(ulong poolAddress, float[] position, float radius)
		{
			GenericPool* pool = (GenericPool*)poolAddress;

			var returnHandles = new List<int>();
			var poolSize = pool->size;
			float radiusSquared = radius * radius;
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
					continue;

				var address = pool->GetAddress(i);

				float* entityPosition = stackalloc float[4];

				NativeMemory.EntityPosFunc(address, entityPosition);
				float x = entityPosition[0] - position[0];
				float y = entityPosition[1] - position[1];
				float z = entityPosition[2] - position[2];

				float distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
					continue;

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		static void AddElementAndReallocateIfLengthIsNotLongEnough(ref int[] array, int index, int elementToAdd)
		{
			if (index >= array.Length)
			{
				// This path is an edge case!
				var newArray = new int[array.Length * 2];
				Array.Copy(array, newArray, array.Length);
				newArray[index] = elementToAdd;

				array = newArray;
			}
			else
			{
				array[index] = elementToAdd;
			}
		}

		static int CalculateAppropriateExtendedArrayLength(int[] array, int targetElementCount)
		{
			return (array.Length * 2 > targetElementCount) ? array.Length * 2 : targetElementCount * 2;
		}

		#endregion

		#region -- Radar Blip Pool --

		static ulong* RadarBlipPoolAddress;
		static int* PossibleRadarBlipCountAddress;
		static int* UnkFirstRadarBlipIndexAddress;
		static int* NorthRadarBlipHandleAddress;
		static int* CenterRadarBlipHandleAddress;

		static bool CheckBlip(ulong blipAddress, float[] position, float radius, params int[] spriteTypes)
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

		// The equivalent function is called in 2 functions (which is for the north and player blip) used in GET_NUMBER_OF_ACTIVE_BLIPS
		private static short GetBlipIndexIfHandleIsValid(int handle)
		{
			if (handle == 0)
			{
				return -1;
			}
			ushort blipIndex = (ushort)handle;
			ulong blipAddress = *(RadarBlipPoolAddress + blipIndex);
			if (blipAddress == 0)
			{
				return -1;
			}

			int blipCreationIncrement = (handle >> 0x10);
			if (blipCreationIncrement != *(int*)(blipAddress + 0x8))
			{
				return -1;
			}

			return (short)blipIndex;
		}
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

			int possibleBlipCount = *PossibleRadarBlipCountAddress;
			int unkFirstBlipIndex = *UnkFirstRadarBlipIndexAddress;
			int northBlipIndex = GetBlipIndexIfHandleIsValid(*NorthRadarBlipHandleAddress);
			int centerBlipIndex = GetBlipIndexIfHandleIsValid(*CenterRadarBlipHandleAddress);

			var handles = new List<int>(possibleBlipCount);

			// Skip the 3 critical blips, just like GET_FIRST_BLIP_INFO_ID does
			// The 3 critical blips is the north blip, the center blip, and the unknown simple blip (placeholder?).
			for (int i = 0; i < possibleBlipCount; i++)
			{
				ulong address = *(RadarBlipPoolAddress + i);

				if (address == 0 || i == unkFirstBlipIndex || i == northBlipIndex || i == centerBlipIndex)
					continue;

				if (CheckBlip(address, position, radius, spriteTypes))
				{
					ushort blipCreationIncrement = *(ushort*)(address + 8);
					handles.Add((int)((blipCreationIncrement << 0x10) + (uint)i));
				}
			}

			return handles.ToArray();
		}

		public static int GetNorthBlip() => NorthRadarBlipHandleAddress != null ? *NorthRadarBlipHandleAddress : 0;

		public static IntPtr GetBlipAddress(int handle)
		{
			if (RadarBlipPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			int poolIndexOfHandle = handle & 0xFFFF;
			int possibleBlipCount = *PossibleRadarBlipCountAddress;

			if (poolIndexOfHandle >= possibleBlipCount)
			{
				return IntPtr.Zero;
			}

			ulong address = *(RadarBlipPoolAddress + poolIndexOfHandle);

			if (address != 0 && IsBlipCreationIncrementValid(address, handle))
				return new IntPtr((long)address);

			return IntPtr.Zero;

			bool IsBlipCreationIncrementValid(ulong blipAddress, int blipHandle) => *(ushort*)(blipAddress + 8) == (((uint)blipHandle >> 0x10));
		}

		#endregion

		#region -- CScriptResource Data --

		internal enum CScriptResourceTypeNameIndex
		{
			Checkpoint = 6
		}

		[StructLayout(LayoutKind.Explicit)]
		struct CGameScriptResource
		{
			[FieldOffset(0x0)]
			internal ulong* vTable;
			[FieldOffset(0x8)]
			internal CScriptResourceTypeNameIndex resourceTypeNameIndex;
			[FieldOffset(0xC)]
			internal int counterOfPool;
			[FieldOffset(0x10)]
			internal int indexOfPool;
			[FieldOffset(0x18)]
			internal CGameScriptResource* next;
			[FieldOffset(0x20)]
			internal CGameScriptResource* prev;
		}

		internal class GetAllCScriptResourceHandlesTask : IScriptTask
		{
			#region Fields
			internal CScriptResourceTypeNameIndex typeNameIndex;
			internal int[] returnHandles = Array.Empty<int>();

			const int MAX_CHECKPOINT_COUNT = 64; // hard coded in the exe
			static readonly int[] _cScriptResourceHandleBuffer = new int[MAX_CHECKPOINT_COUNT];
			#endregion

			internal GetAllCScriptResourceHandlesTask(CScriptResourceTypeNameIndex typeNameIndex)
			{
				this.typeNameIndex = typeNameIndex;
			}

			public void Run()
			{
				var cGameScriptHandlerAddress = GetCGameScriptHandlerAddressFunc();

				if (cGameScriptHandlerAddress == 0)
					return;

				int elementCount = 0;
				var firstRegisteredScriptResourceItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (CGameScriptResource* item = firstRegisteredScriptResourceItem; item != null; item = item->next)
				{
					if (item->resourceTypeNameIndex != typeNameIndex)
						continue;

					_cScriptResourceHandleBuffer[elementCount++] = item->counterOfPool;
				}

				if (elementCount == 0)
					return;

				returnHandles = new int[elementCount];
				Array.Copy(_cScriptResourceHandleBuffer, returnHandles, elementCount);
			}
		}

		internal class GetCScriptResourceAddressTask : IScriptTask
		{
			#region Fields
			internal int targetHandle;
			internal ulong* poolAddress;
			internal int elementSize;
			internal IntPtr returnAddress;
			#endregion

			internal GetCScriptResourceAddressTask(int handle, ulong* poolAddress, int elementSize)
			{
				this.targetHandle = handle;
				this.poolAddress = poolAddress;
				this.elementSize = elementSize;
			}

			public void Run()
			{
				var cGameScriptHandlerAddress = GetCGameScriptHandlerAddressFunc();

				if (cGameScriptHandlerAddress == 0)
					return;

				var firstRegisteredScriptResourceItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (CGameScriptResource* item = firstRegisteredScriptResourceItem; item != null; item = item->next)
				{
					if (item->counterOfPool == targetHandle)
					{
						returnAddress = new IntPtr((long)((byte*)(poolAddress) + item->indexOfPool * elementSize));
						break;
					}
				}
			}
		}

		#endregion

		#region -- Checkpoint Pool --

		static ulong* CheckpointPoolAddress;

		delegate ulong GetCGameScriptHandlerAddressDelegate();
		static GetCGameScriptHandlerAddressDelegate GetCGameScriptHandlerAddressFunc;

		public static int[] GetCheckpointHandles()
		{
			var task = new GetAllCScriptResourceHandlesTask(CScriptResourceTypeNameIndex.Checkpoint);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.returnHandles;
		}

		public static IntPtr GetCheckpointAddress(int handle)
		{
			var task = new GetCScriptResourceAddressTask(handle, CheckpointPoolAddress, 0x60);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.returnAddress;
		}

		#endregion

		#region -- Waypoint Info Array --

		delegate ulong GetLocalPlayerPedAddressFuncDelegate();
		static ulong* waypointInfoArrayStartAddress;
		static ulong* waypointInfoArrayEndAddress;
		static GetLocalPlayerPedAddressFuncDelegate GetLocalPlayerPedAddressFunc;

		public static int GetWaypointBlip()
		{
			if (waypointInfoArrayStartAddress == null || waypointInfoArrayEndAddress == null)
				return 0;

			int playerPedModelHash = 0;
			ulong playerPedAddress = GetLocalPlayerPedAddressFunc();

			if (playerPedAddress != 0)
			{
				playerPedModelHash = GetModelHashFromEntity(new IntPtr((long)playerPedAddress));
			}

			ulong waypointInfoAddress = (ulong)waypointInfoArrayStartAddress;
			for (; waypointInfoAddress < (ulong)waypointInfoArrayEndAddress; waypointInfoAddress += 0x18)
			{
				int modelHash = *(int*)waypointInfoAddress;

				if (modelHash == playerPedModelHash)
				{				
					return *(int*)(waypointInfoAddress + 4);
				}
			}

			return 0;
		}
		#endregion

		#region -- Pool Addresses --

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

		public static IntPtr GetBuildingAddress(int handle)
		{
			if (BuildingPoolAddress == null)
				return IntPtr.Zero;

			return ((GenericPool*)(*NativeMemory.BuildingPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetAnimatedBuildingAddress(int handle)
		{
			if (AnimatedBuildingPoolAddress == null)
				return IntPtr.Zero;

			return ((GenericPool*)(*NativeMemory.AnimatedBuildingPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetInteriorInstAddress(int handle)
		{
			if (InteriorInstPoolAddress == null)
				return IntPtr.Zero;

			return ((GenericPool*)(*NativeMemory.InteriorInstPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetInteriorProxyAddress(int handle)
		{
			if (InteriorProxyPoolAddress == null)
				return IntPtr.Zero;

			return ((GenericPool*)(*NativeMemory.InteriorProxyPoolAddress))->GetAddressFromHandle(handle);
		}

		#endregion

		#region -- Projectile Offsets --
		public static int ProjectileAmmoInfoOffset { get; }
		public static int ProjectileOwnerOffset { get; }
		#endregion

		#region -- Projectile Functions --

		delegate void ExplodeProjectileDelegate(IntPtr projectileAddress, int unkParam);
		static ExplodeProjectileDelegate ExplodeProjectileFunc;

		public static void ExplodeProjectile(IntPtr projectileAddress)
		{
			var task = new ExplodeProjectileTask(projectileAddress);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		internal class ExplodeProjectileTask : IScriptTask
		{
			#region Fields
			internal IntPtr projectileAddress;
			#endregion

			internal ExplodeProjectileTask(IntPtr projectileAddress)
			{
				this.projectileAddress = projectileAddress;
			}

			public void Run()
			{
				ExplodeProjectileFunc(projectileAddress, 0);
			}
		}

		#endregion

		#region -- Interior Offsets --

		public static ulong* InteriorProxyPtrFromGameplayCamAddress { get; }
		public static int InteriorInstPtrInInteriorProxyOffset { get; }

		public static int GetAssociatedInteriorInstHandleFromInteriorProxy(int interiorProxyHandle)
		{
			if (InteriorInstPtrInInteriorProxyOffset == 0 || InteriorInstPoolAddress == null)
				return 0;

			var interiorProxyAddress = GetInteriorProxyAddress(interiorProxyHandle);
			if (interiorProxyAddress == IntPtr.Zero)
				return 0;

			var interiorInstAddress = *(ulong*)(interiorProxyAddress + InteriorInstPtrInInteriorProxyOffset).ToPointer();
			if (interiorInstAddress == 0)
				return 0;

			return ((GenericPool*)(*NativeMemory.InteriorInstPoolAddress))->GetGuidHandleFromAddress(interiorInstAddress);
		}
		public static int GetInteriorProxyHandleFromInteriorInst(int interiorInstHandle)
		{
			if (InteriorProxyPoolAddress == null)
				return 0;

			var interiorInstAddress = GetInteriorInstAddress(interiorInstHandle);
			if (interiorInstAddress == IntPtr.Zero)
				return 0;

			var interiorProxyAddress = *(ulong*)(interiorInstAddress + 0x188).ToPointer();
			if (interiorProxyAddress == 0)
				return 0;

			return ((GenericPool*)(*NativeMemory.InteriorProxyPoolAddress))->GetGuidHandleFromAddress(interiorProxyAddress);
		}
		public static int GetInteriorProxyHandleFromGameplayCam()
		{
			if (InteriorProxyPtrFromGameplayCamAddress == null || InteriorInstPoolAddress == null)
				return 0;

			var interiorProxyAddress = *InteriorProxyPtrFromGameplayCamAddress;
			if (interiorProxyAddress == 0)
				return 0;

			return ((GenericPool*)(*NativeMemory.InteriorProxyPoolAddress))->GetGuidHandleFromAddress(interiorProxyAddress);
		}

		#endregion

		#region -- Weapon Info And Ammo Info --

		static RageAtArrayPtr* weaponAndAmmoInfoArrayPtr;
		static HashSet<uint> disallowWeaponHashSetForHumanPedsOnFoot = new HashSet<uint>() {
				0x1B79F17,  /* weapon_briefcase_02 */
				0x166218FF, /* weapon_passenger_rocket */
				0x32A888BD, /* weapon_tranquilizer */
				0x687652CE, /* weapon_stinger */
				0x6D5E2801, /* weapon_bird_crap */
				0x88C78EB7, /* weapon_briefcase */
				0xFDBADCED, /* weapon_digiscanner */
			};

		static uint* weaponComponentArrayCountAddr;
		// Store the offset instead of the calculated address for compatibility with mods like Weapon Limits Adjuster by alexguirre (although Weapon Limits Adjuster allocates a new array in the very beginning).
		static ulong offsetForCWeaponComponentArrayAddr;
		static int weaponAttachPointsStartOffset;
		static int weaponAttachPointsArrayCountOffset;
		static int weaponAttachPointElementComponentCountOffset;
		static int weaponAttachPointElementSize;

		static int weaponInfoHumanNameHashOffset;

		[StructLayout(LayoutKind.Explicit, Size=0x20)]
		struct ItemInfo
		{
			[FieldOffset(0x0)]
			internal ulong* vTable;
			[FieldOffset(0x10)]
			internal uint nameHash;
			[FieldOffset(0x14)]
			internal uint modelHash;
			[FieldOffset(0x18)]
			internal uint audioHash;
			[FieldOffset(0x1C)]
			internal uint slot;

			// The function is for the game version b2802 or later ones.
			// This one directly returns a hash value (not a pointer value) unlike the previous function.
			delegate uint GetClassNameHashOfCItemInfoDelegate();
			static Dictionary<ulong, GetClassNameHashOfCItemInfoDelegate> getClassNameHashOfCItemInfoCacheDict = new Dictionary<ulong, GetClassNameHashOfCItemInfoDelegate>();
			// The function is for game versions prior to b2802.
			// The function uses rax and rdx registers in newer versions prior to b2802 (probably since b2189), and it uses only rax register in older versions.
			// The function returns the address where the class name hash is in all versions prior to (the address will be the outVal address in newer versions).
			delegate uint* GetClassNameHashAddressOfCItemInfoDelegate(ulong unused, uint* outVal);
			static Dictionary<ulong, GetClassNameHashAddressOfCItemInfoDelegate> getClassNameHashAddressOfCItemInfoCacheDict = new Dictionary<ulong, GetClassNameHashAddressOfCItemInfoDelegate>();

			internal uint GetClassNameHash()
			{
				// In the b2802 or a later exe, the function returns a hash value (not a pointer value)
				if (GetGameVersion() >= 80)
				{
					var GetClassNameHashFunc = CreateGetClassNameHashDelegateIfNotCreated(vTable[2]);
					return GetClassNameHashFunc();
				}
				else
				{
					var GetClassNameAddressHashFunc = CreateGetClassNameHashAddressDelegateIfNotCreated(vTable[2]);
					uint outVal = 0;
					var returnValueAddress = GetClassNameAddressHashFunc(0, &outVal);
					return *returnValueAddress;
				}
			}

			private static GetClassNameHashOfCItemInfoDelegate CreateGetClassNameHashDelegateIfNotCreated(ulong virtualFuncAddr)
			{
				if (getClassNameHashOfCItemInfoCacheDict.TryGetValue(virtualFuncAddr, out var outDelegate))
				{
					return outDelegate;
				}
				else
				{
					var newDelegate = GetDelegateForFunctionPointer<GetClassNameHashOfCItemInfoDelegate>(new IntPtr((long)virtualFuncAddr));
					getClassNameHashOfCItemInfoCacheDict.Add(virtualFuncAddr, newDelegate);

					return newDelegate;
				}
			}

			private static GetClassNameHashAddressOfCItemInfoDelegate CreateGetClassNameHashAddressDelegateIfNotCreated(ulong virtualFuncAddr)
			{
				if (getClassNameHashAddressOfCItemInfoCacheDict.TryGetValue(virtualFuncAddr, out var outDelegate))
				{
					return outDelegate;
				}
				else
				{
					var newDelegate = GetDelegateForFunctionPointer<GetClassNameHashAddressOfCItemInfoDelegate>(new IntPtr((long)virtualFuncAddr));
					getClassNameHashAddressOfCItemInfoCacheDict.Add(virtualFuncAddr, newDelegate);

					return newDelegate;
				}
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 0x48)]
		struct WeaponComponentInfo
		{
			[FieldOffset(0x0)]
			internal ulong* vTable;
			[FieldOffset(0x10)]
			internal uint nameHash;
			[FieldOffset(0x14)]
			internal uint modelHash;
			[FieldOffset(0x18)]
			internal uint locNameHash;
			[FieldOffset(0x1C)]
			internal uint locDescHash;
			[FieldOffset(0x40)]
			internal bool shownOnWheel;
			[FieldOffset(0x41)]
			internal bool createObject;
			[FieldOffset(0x42)]
			internal bool applyWeaponTint;
		}

		static ItemInfo* FindItemInfoFromWeaponAndAmmoInfoArray(uint nameHash)
		{
			if (weaponAndAmmoInfoArrayPtr == null)
			{
				return null;
			}

			var weaponAndAmmoInfoElementCount = weaponAndAmmoInfoArrayPtr->size;

			if (weaponAndAmmoInfoElementCount == 0)
				return null;

			int low = 0, high = weaponAndAmmoInfoElementCount - 1;
			while (true)
			{
				int indexToRead = (low + high) >> 1;
				var weaponOrAmmoInfo = (ItemInfo*)weaponAndAmmoInfoArrayPtr->GetElementAddress(indexToRead);

				if (weaponOrAmmoInfo->nameHash == nameHash)
					return weaponOrAmmoInfo;

				// The array is sorted in ascending order
				if (weaponOrAmmoInfo->nameHash <= nameHash)
					low = indexToRead + 1;
				else
					high = indexToRead - 1;

				if (low > high)
					return null;
			}
		}

		static ItemInfo* FindWeaponInfo(uint nameHash)
		{
			var itemInfoPtr = FindItemInfoFromWeaponAndAmmoInfoArray(nameHash);

			if (itemInfoPtr == null)
				return null;

			var classNameHash = itemInfoPtr->GetClassNameHash();

			const uint CWEAPONINFO_NAME_HASH = 0x861905B4;
			if (classNameHash == CWEAPONINFO_NAME_HASH)
				return itemInfoPtr;

			return null;
		}

		static WeaponComponentInfo* FindWeaponComponentInfo(uint nameHash)
		{
			var cWeaponComponentArrayFirstPtr = (ulong*)((byte*)offsetForCWeaponComponentArrayAddr + 4 + *(int*)offsetForCWeaponComponentArrayAddr);
			var arrayCount = weaponComponentArrayCountAddr != null ? *(uint*)weaponComponentArrayCountAddr : 0;
			if (cWeaponComponentArrayFirstPtr == null || arrayCount == 0)
			{
				return null;
			}

			int low = 0, high = (int)arrayCount - 1;
			while (true)
			{
				int indexToRead = (low + high) >> 1;
				var weaponComponentInfo = (WeaponComponentInfo*)cWeaponComponentArrayFirstPtr[indexToRead];

				if (weaponComponentInfo->nameHash == nameHash)
					return weaponComponentInfo;

				// The array is sorted in ascending order
				if (weaponComponentInfo->nameHash <= nameHash)
					low = indexToRead + 1;
				else
					high = indexToRead - 1;

				if (low > high)
					return null;
			}
		}

		public static bool IsHashValidAsWeaponHash(uint weaponHash) => FindWeaponInfo(weaponHash) != null;

		public static uint GetAttachmentPointHash(uint weaponHash, uint componentHash)
		{
			var weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
				return 0xFFFFFFFF;

			var weaponAttachPointsAddr = (byte*)weaponInfo + weaponAttachPointsStartOffset;
			var weaponAttachPointsCount = *(int*)(weaponAttachPointsAddr + weaponAttachPointsArrayCountOffset);
			var weaponAttachPointElementStartAddr = (byte*)(weaponAttachPointsAddr);

			for (int i = 0; i < weaponAttachPointsCount; i++)
			{
				var weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + (i * weaponAttachPointElementSize) + 0x8;
				int componentItemsCount = *(int*)(weaponAttachPointElementAddr + weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
					continue;

				for (int j = 0; j < componentItemsCount; j++)
				{
					var componentHashInItemArray = *(uint*)(weaponAttachPointElementAddr + j * 0x8);
					if (componentHashInItemArray == componentHash)
						return *(uint*)(weaponAttachPointElementStartAddr + i * weaponAttachPointElementSize);
				}
			}

			return 0xFFFFFFFF;
		}

		public static List<uint> GetAllWeaponHashesForHumanPeds()
		{
			if (weaponAndAmmoInfoArrayPtr == null)
			{
				return new List<uint>();
			}

			var weaponAndAmmoInfoElementCount = weaponAndAmmoInfoArrayPtr->size;
			var resultList = new List<uint>();

			for (int i = 0; i < weaponAndAmmoInfoElementCount; i++)
			{
				var weaponOrAmmoInfo = (ItemInfo*)weaponAndAmmoInfoArrayPtr->GetElementAddress(i);

				if (!CanPedEquip(weaponOrAmmoInfo) && !disallowWeaponHashSetForHumanPedsOnFoot.Contains(weaponOrAmmoInfo->nameHash))
					continue;

				var classNameHash = weaponOrAmmoInfo->GetClassNameHash();

				const uint CWEAPONINFO_NAME_HASH = 0x861905B4;
				if (classNameHash == CWEAPONINFO_NAME_HASH)
					resultList.Add(weaponOrAmmoInfo->nameHash);
			}

			return resultList;

			bool CanPedEquip(ItemInfo* weaponInfoAddress)
			{
				return weaponInfoAddress->modelHash != 0 && weaponInfoAddress->slot != 0;
			}
		}

		public static List<uint> GetAllWeaponComponentHashes()
		{
			var cWeaponComponentArrayFirstPtr = (ulong*)((byte*)offsetForCWeaponComponentArrayAddr + 4 + *(int*)offsetForCWeaponComponentArrayAddr);
			var arrayCount = weaponComponentArrayCountAddr != null ? *(uint*)weaponComponentArrayCountAddr : 0;
			var resultList = new List<uint>();

			for (uint i = 0; i < arrayCount; i++)
			{
				var cWeaponComponentInfo = cWeaponComponentArrayFirstPtr[i];
				var weaponComponentNameHash = *(uint*)(cWeaponComponentInfo + 0x10);
				resultList.Add(weaponComponentNameHash);
			}

			return resultList;
		}

		public static List<uint> GetAllCompatibleWeaponComponentHashes(uint weaponHash)
		{
			var weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
				return new List<uint>();

			var returnList = new List<uint>();

			var weaponAttachPointsAddr = (byte*)weaponInfo + weaponAttachPointsStartOffset;
			var weaponAttachPointsCount = *(int*)(weaponAttachPointsAddr + weaponAttachPointsArrayCountOffset);
			var weaponAttachPointElementStartAddr = (byte*)(weaponAttachPointsAddr + 0x8);
			for (int i = 0; i < weaponAttachPointsCount; i++)
			{
				var weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + i * weaponAttachPointElementSize;
				int componentItemsCount = *(int*)(weaponAttachPointElementAddr + weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
					continue;

				for (int j = 0; j < componentItemsCount; j++)
				{
					returnList.Add(*(uint*)(weaponAttachPointElementAddr + j * 0x8));
				}
			}

			return returnList;
		}

		public static uint GetHumanNameHashOfWeaponInfo(uint weaponHash)
		{
			var weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
				// hashed value of WT_INVALID
				return 0xBFED8500;

			return *(uint*)((byte*)weaponInfo + weaponInfoHumanNameHashOffset);
		}

		public static uint GetHumanNameHashOfWeaponComponentInfo(uint weaponComponentHash)
		{
			var weaponComponentInfo = FindWeaponComponentInfo(weaponComponentHash);

			if (weaponComponentInfo == null)
				// hashed value of WCT_INVALID
				return 0xDE4BE9F8;

			return weaponComponentInfo->locNameHash;
		}

		#endregion

		#region -- Fragment Object for Entity --

		internal delegate FragInst* GetFragInstDelegate(IntPtr entityAddress);
		internal delegate FragInst* DetachFragmentPartByIndexDelegate(FragInst* fragInst, int partIndex);

		static int getFragInstVFuncOffset;
		static DetachFragmentPartByIndexDelegate detachFragmentPartByIndexFunc;
		static ulong** phSimulatorInstPtr;
		static int colliderCapacityOffset;
		static int colliderCountOffset;

		// Only 3 virtural functions are present (one for peds and objects and one for vehicles)
		static Dictionary<ulong, GetFragInstDelegate> getFragInstVFuncCache = new Dictionary<ulong, GetFragInstDelegate>(3);

		[StructLayout(LayoutKind.Explicit, Size = 0xC0)]
		internal unsafe struct FragInst
		{
			[FieldOffset(0x68)]
			internal FragCacheEntry* fragCacheEntry;
			[FieldOffset(0x78)]
			internal GtaFragType* gtaFragType;
			[FieldOffset(0xB8)]
			internal uint unkType;

			internal FragPhysicsLOD* GetAppropriateFragPhysicsLOD()
			{
				var fragPhysicsLODGroup = gtaFragType->fragPhysicsLODGroup;
				if (fragPhysicsLODGroup == null)
					return null;

				switch (unkType)
				{
					case 0:
					case 1:
					case 2:
						return fragPhysicsLODGroup->GetFragPhysicsLODByIndex((int)unkType);
					default:
						return fragPhysicsLODGroup->GetFragPhysicsLODByIndex(0);
				}
			}
		}
		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct FragCacheEntry
		{
			[FieldOffset(0x178)] internal CrSkeleton* crSkeleton;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct GtaFragType
		{
			[FieldOffset(0x30)]
			internal FragDrawable* fragDrawable;
			[FieldOffset(0xF0)]
			internal FragPhysicsLODGroup* fragPhysicsLODGroup;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragDrawable
		{
			[FieldOffset(0x18)]
			internal CrSkeletonData* crSkeletonData;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragPhysicsLODGroup
		{
			[FieldOffset(0x10)]
			internal fixed ulong fragPhysicsLODAddresses[3];

			internal FragPhysicsLOD* GetFragPhysicsLODByIndex(int index) => (FragPhysicsLOD*)((ulong*)fragPhysicsLODAddresses[index]);
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragPhysicsLOD
		{
			[FieldOffset(0xD0)]
			internal ulong fragTypeChildArr;
			[FieldOffset(0x11E)]
			internal byte fragmentGroupCount;

			internal FragTypeChild* GetFragTypeChild(int index)
			{
				if (index >= fragmentGroupCount)
					return null;

				return (FragTypeChild*)*((ulong*)fragTypeChildArr + index);
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct FragTypeChild
		{
			[FieldOffset(0x10)]
			internal ushort boneIndex;
			[FieldOffset(0x12)]
			internal ushort boneId;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct CrSkeleton
		{
			[FieldOffset(0x00)] internal CrSkeletonData* skeletonData;
			[FieldOffset(0x10)] internal ulong bonePoseMatrixArrayPtr;
			[FieldOffset(0x18)] internal ulong boneMatrixArrayPtr;
			[FieldOffset(0x20)] internal int boneCount;

			public IntPtr GetBonePoseMatrixAddress(int boneIndex)
			{
				return new IntPtr((long)(bonePoseMatrixArrayPtr + ((uint)boneIndex * 0x40)));
			}

			public IntPtr GetBoneMatrixAddress(int boneIndex)
			{
				return new IntPtr((long)(boneMatrixArrayPtr + ((uint)boneIndex * 0x40)));
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct CrSkeletonData
		{
			[FieldOffset(0x10)] internal ulong boneIdAndIndexTupleArrayPtr;
			[FieldOffset(0x18)] internal ushort divisorForBoneIdAndIndexTuple;
			[FieldOffset(0x1A)] internal ushort unkValue;
			[FieldOffset(0x5E)] internal ushort boneCount;

			/// <summary>
			/// Gets the bone id from specified bone index. Note that bone indexes are sequential values and bone ids are not sequential ones.
			/// </summary>
			public int GetBoneIndexByBoneId(int boneId)
			{
				if (unkValue == 0)
				{
					if (boneId < boneCount)
						return boneId;

					return -1;
				}

				if (divisorForBoneIdAndIndexTuple == 0)
					return -1;

				var firstTuplePtr = ((ulong*)boneIdAndIndexTupleArrayPtr + (boneId % divisorForBoneIdAndIndexTuple));

				for (var boneIdAndIndexTuple = (BoneIdAndIndexTuple*)*firstTuplePtr; boneIdAndIndexTuple != null; boneIdAndIndexTuple = (BoneIdAndIndexTuple*)boneIdAndIndexTuple->nextTupleAddr)
				{
					if (boneId == boneIdAndIndexTuple->boneId)
						return boneIdAndIndexTuple->boneIndex;
				}

				return -1;
			}
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct BoneIdAndIndexTuple
		{
			[FieldOffset(0x0)] internal int boneId;
			[FieldOffset(0x4)] internal int boneIndex;
			[FieldOffset(0x8)] internal ulong nextTupleAddr;
		}

		internal class DetachFragmentPartByIndexTask : IScriptTask
		{
			#region Fields
			internal FragInst* fragInst;
			internal int fragmentGroupIndex;
			internal bool wasNewFragInstCreated;
			#endregion

			internal DetachFragmentPartByIndexTask(FragInst* fragInst, int fragmentGroupIndex)
			{
				this.fragInst = fragInst;
				this.fragmentGroupIndex = fragmentGroupIndex;
			}

			public void Run()
			{
				wasNewFragInstCreated = detachFragmentPartByIndexFunc(fragInst, fragmentGroupIndex) != null;
			}
		}

		public static int GetFragmentGroupCountFromEntity(IntPtr entityAddress)
		{
			var fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
				return 0;

			return GetFragmentGroupCountOfFragInst(fragInst);
		}

		public static bool DetachFragmentPartByIndex(IntPtr entityAddress, int fragmentGroupIndex)
		{
			if (fragmentGroupIndex < 0)
				return false;

			// If the entity collider count is at the capacity, the game can crash for trying to create the new entity while no free collider slots are available
			if (GetEntityColliderCount() >= GetEntityColliderCapacity())
				return false;

			var fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
				return false;

			var fragmentGroupCount = GetFragmentGroupCountOfFragInst(fragInst);
			if (fragmentGroupIndex >= fragmentGroupCount)
				return false;

			var task = new DetachFragmentPartByIndexTask(fragInst, fragmentGroupIndex);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.wasNewFragInstCreated;
		}

		public static int GetFragmentGroupIndexByEntityBoneIndex(IntPtr entityAddress, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return -1;

			var fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
				return -1;


			var crSkeletonData = fragInst->gtaFragType->fragDrawable->crSkeletonData;
			if (crSkeletonData == null)
				return -1;

			var boneCount = crSkeletonData->boneCount;
			if (boneIndex >= boneCount)
				return -1;

			var fragPhysicsLOD = fragInst->GetAppropriateFragPhysicsLOD();
			if (fragPhysicsLOD == null)
				return -1;

			var fragmentGroupCount = fragPhysicsLOD->fragmentGroupCount;

			for (int i = 0; i < fragmentGroupCount; i++)
			{
				var fragTypeChild = fragPhysicsLOD->GetFragTypeChild(i);

				if (fragTypeChild == null)
					continue;

				if (boneIndex == crSkeletonData->GetBoneIndexByBoneId(fragTypeChild->boneId))
					return i;
			}

			return -1;
		}

		public static int GetEntityColliderCapacity()
		{
			if (*phSimulatorInstPtr == null)
				return 0;

			return *(int*)((byte*)*phSimulatorInstPtr + colliderCapacityOffset);
		}

		public static int GetEntityColliderCount()
		{
			if (*phSimulatorInstPtr == null)
				return 0;

			return *(int*)((byte*)*phSimulatorInstPtr + colliderCountOffset);
		}

		public static bool IsEntityFragmentObject(IntPtr entityAddress)
		{
			// For CObject, a valid address will be returned only when a certain flag is set. For CPed and CVehicle, a valid address will always be returned.
			return GetFragInstAddressOfEntity(entityAddress) != null;
		}

		private static FragInst* GetFragInstAddressOfEntity(IntPtr entityAddress)
		{
			var vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)getFragInstVFuncOffset);
			var getFragInstFunc = CreateGetFragInstDelegateIfNotCreated(vFuncAddr);

			return getFragInstFunc(entityAddress);
		}

		private static int GetFragmentGroupCountOfFragInst(FragInst* fragInst)
		{
			var fragPhysicsLOD = fragInst->GetAppropriateFragPhysicsLOD();
			return fragPhysicsLOD != null ? fragPhysicsLOD->fragmentGroupCount : 0;
		}

		private static GetFragInstDelegate CreateGetFragInstDelegateIfNotCreated(ulong virtualFuncAddr)
		{
			if (getFragInstVFuncCache.TryGetValue(virtualFuncAddr, out var outDelegate))
			{
				return outDelegate;
			}
			else
			{
				var newDelegate = GetDelegateForFunctionPointer<GetFragInstDelegate>(new IntPtr((long)virtualFuncAddr));
				getFragInstVFuncCache.Add(virtualFuncAddr, newDelegate);

				return newDelegate;
			}
		}

		#endregion

		#region -- NaturalMotion Euphoria --

		delegate byte SetNmBoolAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, [MarshalAs(UnmanagedType.I1)] bool value);
		delegate byte SetNmIntAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, int value);
		delegate byte SetNmFloatAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float value);
		delegate byte SetNmVector3AddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, float x, float y, float z);
		delegate byte SetNmStringAddressDelegate(ulong messageAddress, IntPtr argumentNamePtr, IntPtr stringPtr);

		delegate ulong InitMessageMemoryDelegate(ulong T1, ulong T2, int T3);
		delegate void SendMessageToPedDelegate(ulong pedNmAddress, IntPtr messagePtr, ulong messageAddress);
		delegate void FreeMessageMemoryDelegate(ulong messageAddress);

		delegate CTask* GetActiveTaskFuncDelegate(ulong cTaskTreePedAddress);
		delegate int GetEventTypeIndexDelegate(ulong cEventAddress);

		delegate void ActionUlongDelegate(ulong T);
		delegate int FuncUlongIntDelegate(ulong T);
		delegate ulong FuncUlongUlongDelegate(ulong T);

		static SetNmIntAddressDelegate SetNmIntAddress;
		static SetNmBoolAddressDelegate SetNmBoolAddress;
		static SetNmFloatAddressDelegate SetNmFloatAddress;
		static SetNmStringAddressDelegate SetNmStringAddress;
		static SetNmVector3AddressDelegate SetNmVector3Address;
		static GetActiveTaskFuncDelegate GetActiveTaskFunc;
		static InitMessageMemoryDelegate InitMessageMemoryFunc;
		static SendMessageToPedDelegate SendMessageToPedFunc;

		static int fragInstNMGtaOffset;
		static int cTaskNMScriptControlTypeIndex;
		static int cEventSwitch2NMTypeIndex;

		static Dictionary<ulong, GetEventTypeIndexDelegate> getEventTypeIndexDelegateCacheDict = new Dictionary<ulong, GetEventTypeIndexDelegate>();

		[StructLayout(LayoutKind.Explicit, Size=0x38)]
		struct CTask
		{
			[FieldOffset(0x34)]
			internal ushort taskTypeIndex;
		}

		public static bool IsTaskNMScriptControlOrEventSwitch2NMActive(IntPtr pedAddress)
		{
			ulong phInstGtaAddress = *(ulong*)(pedAddress + 0x30);

			if (phInstGtaAddress == 0)
				return false;

			ulong fragInstNMGtaAddress = *(ulong*)(pedAddress + fragInstNMGtaOffset);

			if (phInstGtaAddress == fragInstNMGtaAddress && !IsPedInjured((byte*)pedAddress))
			{
				var funcUlongIntDelegate = GetDelegateForFunctionPointer<FuncUlongIntDelegate>(new IntPtr((long)*(ulong*)(*(ulong*)fragInstNMGtaAddress + 0x98)));
				if (funcUlongIntDelegate(fragInstNMGtaAddress) != -1)
				{
					var PedIntelligenceAddr = *(ulong*)(pedAddress + PedIntelligenceOffset);

					var activeTask = GetActiveTaskFunc(*(ulong*)((byte*)PedIntelligenceAddr + CTaskTreePedOffset));
					if (activeTask != null && activeTask->taskTypeIndex == cTaskNMScriptControlTypeIndex)
					{
						return true;
					}
					else
					{
						int eventCount = *(int*)((byte*)PedIntelligenceAddr + CEventCountOffset);
						for (int i = 0; i < eventCount; i++)
						{
							var eventAddress = *(ulong*)((byte*)PedIntelligenceAddr + CEventStackOffset + 8 * ((i + *(int*)((byte*)PedIntelligenceAddr + (CEventCountOffset - 4)) + 1) % 16));
							if (eventAddress != 0)
							{
								var getEventTypeIndexFunc = CreateGetEventTypeIndexDelegateIfNotCreated(eventAddress);
								if (getEventTypeIndexFunc(eventAddress) == cEventSwitch2NMTypeIndex)
								{
									var taskInEvent = *(CTask**)(eventAddress + 0x28);
									if (taskInEvent != null)
									{
										if (taskInEvent->taskTypeIndex == cTaskNMScriptControlTypeIndex)
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
			}

			return false;
		}

		static bool IsPedInjured(byte* pedAddress) => *(float*)(pedAddress + 0x280) < *(float*)(pedAddress + InjuryHealthThresholdOffset);

		static GetEventTypeIndexDelegate CreateGetEventTypeIndexDelegateIfNotCreated(ulong eventAddress)
		{
			var getEventTypeIndexVirtualFuncAddr = *(ulong*)(*(ulong*)eventAddress + 0x18);

			if (getEventTypeIndexDelegateCacheDict.TryGetValue(getEventTypeIndexVirtualFuncAddr, out var cachedDelegate))
				return cachedDelegate;

			var createdDelegate = GetDelegateForFunctionPointer<GetEventTypeIndexDelegate>(new IntPtr((long)getEventTypeIndexVirtualFuncAddr));
			getEventTypeIndexDelegateCacheDict[getEventTypeIndexVirtualFuncAddr] = createdDelegate;

			return createdDelegate;
		}

		internal class EuphoriaMessageTask : IScriptTask
		{
			#region Fields
			int targetHandle;
			string message;
			Dictionary<string, (int value, Type type)> _boolIntFloatArguments;
			Dictionary<string, object> _stringVector3ArrayArguments;
			#endregion

			internal EuphoriaMessageTask(int target, string message, Dictionary<string, (int, Type)> boolIntFloatArguments, Dictionary<string, object> stringVector3ArrayArguments)
			{
				targetHandle = target;
				this.message = message;
				_boolIntFloatArguments = boolIntFloatArguments;
				_stringVector3ArrayArguments = stringVector3ArrayArguments;
			}

			public void Run()
			{
				byte* _PedAddress = (byte*)NativeMemory.GetEntityAddress(targetHandle).ToPointer();

				if (_PedAddress == null)
					return;

				ulong messageMemory = (ulong)AllocCoTaskMem(0x1218).ToInt64();

				if (messageMemory == 0)
					return;

				InitMessageMemoryFunc(messageMemory, messageMemory + 0x18, 0x40);

				if (_boolIntFloatArguments != null)
				{
					foreach (var arg in _boolIntFloatArguments)
					{
						IntPtr name = ScriptDomain.CurrentDomain.PinString(arg.Key);

						(var argValue, var argType) = arg.Value;

						if (argType == typeof(float))
						{
							var argValueConverted = *(float*)(&argValue);
							NativeMemory.SetNmFloatAddress(messageMemory, name, argValueConverted);
						}
						else if (argType == typeof(bool))
						{
							var argValueConverted = argValue != 0 ? true : false;
							NativeMemory.SetNmBoolAddress(messageMemory, name, argValueConverted);
						}
						else if (argType == typeof(int))
						{
							NativeMemory.SetNmIntAddress(messageMemory, name, argValue);
						}
					}
				}

				if (_stringVector3ArrayArguments != null)
				{
					foreach (var arg in _stringVector3ArrayArguments)
					{
						IntPtr name = ScriptDomain.CurrentDomain.PinString(arg.Key);

						var argValue = arg.Value;
						if (argValue is float[] vector3ArgValue)
							NativeMemory.SetNmVector3Address(messageMemory, name, vector3ArgValue[0], vector3ArgValue[1], vector3ArgValue[2]);
						else if (argValue is string stringArgValue)
							NativeMemory.SetNmStringAddress(messageMemory, name, ScriptDomain.CurrentDomain.PinString(stringArgValue));
					}
				}

				if (IsTaskNMScriptControlOrEventSwitch2NMActive(new IntPtr(_PedAddress)))
				{
					ulong fragInstNMGtaAddress = *(ulong*)(_PedAddress + fragInstNMGtaOffset);
					IntPtr messageStringPtr = ScriptDomain.CurrentDomain.PinString(message);
					SendMessageToPedFunc((ulong)fragInstNMGtaAddress, messageStringPtr, messageMemory);
				}

				FreeCoTaskMem(new IntPtr((long)messageMemory));
			}
		}

		public static void SendEuphoriaMessage(int targetHandle, string message, Dictionary<string, (int, Type)> boolIntFloatArguments, Dictionary<string, object> stringVector3ArrayArguments)
		{
			var task = new EuphoriaMessageTask(targetHandle, message, boolIntFloatArguments, stringVector3ArrayArguments);

			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		#endregion
	}
}
