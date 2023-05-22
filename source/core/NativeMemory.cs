using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
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
		[SuppressUnmanagedCodeSecurity]
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
		[SuppressUnmanagedCodeSecurity]
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
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ScriptHookV.dll", ExactSpelling = true, EntryPoint = "?getGlobalPtr@@YAPEA_KH@Z")]
		public static extern IntPtr GetGlobalPtr(int index);
		#endregion

		/// <inheritdoc cref="FindPatternNaive(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternNaive(string pattern, string mask)
		{
			var module = Process.GetCurrentProcess().MainModule;
			return FindPatternNaive(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternNaive(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress)
		{
			var module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
				return null;

			var size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

			return FindPatternNaive(pattern, mask, startAddress, size);
		}

		/// <summary>
		/// Searches the specific address space of the current process for a memory pattern using the naive algorithm.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress, ulong size)
		{
			var address = (ulong)startAddress.ToInt64();
			var endAddress = address + size;

			for (; address < endAddress; address++)
			{
				for (var i = 0; i < pattern.Length; i++)
				{
					if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
						break;
					if (i + 1 == pattern.Length)
						return (byte*)address;
				}
			}

			return null;
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask)
		{
			var module = Process.GetCurrentProcess().MainModule;
			return FindPatternBmh(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress)
		{
			var module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
				return null;

			var size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

			return FindPatternBmh(pattern, mask, startAddress, size);
		}

		/// <summary>
		/// Searches the address space of the current process for a memory pattern using the Boyer–Moore–Horspool algorithm.
		/// Will perform faster than the naive algorithm when the pattern is long enough to expect the bad character skip is consistently high.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <param name="mask">The pattern mask.</param>
		/// <param name="startAddress">The address to start searching at.</param>
		/// <param name="size">The size where the pattern search will be performed from <paramref name="startAddress"/>.</param>
		/// <returns>The address of a region matching the pattern or <see langword="null" /> if none was found.</returns>
		public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress, ulong size)
		{
			// Use short array intentionally to spare heap
			// Warning: throws an exception if length of pattern and mask strings does not match
			var patternArray = new short[pattern.Length];
			for (var i = 0; i < patternArray.Length; i++)
			{
				patternArray[i] = (mask[i] != '?') ? (short)pattern[i] : (short)-1;
			}

			var lastPatternIndex = patternArray.Length - 1;
			var skipTable = CreateShiftTableForBmh(patternArray);

			var endAddressToScan = (byte*)startAddress + size - patternArray.Length;

			// Pin arrays to avoid boundary check and search will be long enough to amortize the pin cost in time wise
			fixed (short* skipTablePtr = skipTable)
			fixed (short* patternArrayPtr = patternArray)
			{
				for (var curHeadAddress = (byte*)startAddress; curHeadAddress <= endAddressToScan; curHeadAddress += Math.Max((int)skipTablePtr[(curHeadAddress)[lastPatternIndex] & 0xFF], 1))
				{
					for (var i = lastPatternIndex; patternArrayPtr[i] < 0 || ((byte*)curHeadAddress)[i] == patternArrayPtr[i]; --i)
					{
						if (i == 0)
						{
							return curHeadAddress;
						}
					}
				}
			}

			return null;
		}

		private static short[] CreateShiftTableForBmh(short[] pattern)
		{
			var skipTable = new short[256];
			var lastIndex = pattern.Length - 1;

			var diff = lastIndex - Math.Max(Array.LastIndexOf<short>(pattern, -1), 0);
			if (diff == 0)
			{
				diff = 1;
			}

			for (var i = 0; i < skipTable.Length; i++)
			{
				skipTable[i] = (short)diff;
			}

			for (var i = lastIndex - diff; i < lastIndex; i++)
			{
				var patternVal = pattern[i];
				if (patternVal >= 0)
				{
					skipTable[patternVal] = (short)(lastIndex - i);
				}
			}

			return skipTable;
		}

		/// <summary>
		/// Disposes unmanaged resources.
		/// </summary>
		internal static void DisposeUnmanagedResources()
		{
			Marshal.FreeCoTaskMem(String);
			Marshal.FreeCoTaskMem(NullString);
			Marshal.FreeCoTaskMem(CellEmailBcon);

			String = IntPtr.Zero;
			NullString = IntPtr.Zero;
			CellEmailBcon = IntPtr.Zero;
		}

		/// <summary>
		/// Initializes all known functions and offsets based on pattern searching.
		/// </summary>
		static NativeMemory()
		{
			String = StringToCoTaskMemUTF8("STRING"); // "~a~"
			NullString = StringToCoTaskMemUTF8(string.Empty); // ""
			CellEmailBcon = StringToCoTaskMemUTF8("CELL_EMAIL_BCON"); // "~a~~a~~a~~a~~a~~a~~a~~a~~a~~a~"

			byte* address;
			IntPtr startAddressToSearch;

			// Get relative address and add it to the instruction address.
			address = FindPatternBmh("\x74\x21\x48\x8B\x48\x20\x48\x85\xC9\x74\x18\x48\x8B\xD6\xE8", "xxxxxxxxxxxxxxx") - 10;
			GetPtfxAddressFunc = (delegate* unmanaged[Stdcall]<int, ulong>)(
				new IntPtr(*(int*)(address) + address + 4));

			address = FindPatternBmh("\x85\xED\x74\x0F\x8B\xCD\xE8\x00\x00\x00\x00\x48\x8B\xF8\x48\x85\xC0\x74\x2E", "xxxxxxx????xxxxxxxx");
			GetScriptEntity = (delegate* unmanaged[Stdcall]<int, ulong>)(
				new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x8B\xC2\xB2\x01\x8B\xC8\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x53\x8A\x88\x00\x00\x00\x00\xF6\xC1\x01\x75\x05\xF6\xC1\x02\x75\x43\x48", "xxxxxxx????xxxxxxx????xxxxxxxxxxx");
			GetPlayerPedAddressFunc = (delegate* unmanaged[Stdcall]<int, ulong>)(
			new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x0F\x84\xA1\x00\x00\x00\x33\xC9\x48\x89\x35", "xxxxxxxxxxx");
			if (address != null)
			{
				isGameMultiplayerAddr = (bool*)(*(int*)(address + 0x27) + address + 0x2B);
			}

			address = FindPatternBmh("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			CreateGuid = (delegate* unmanaged[Stdcall]<ulong, int>)(
				new IntPtr(address - 0x68));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x30\x48\x8B\xDA\xE8\x00\x00\x00\x00\xF3\x0F\x10\x44\x24\x2C\x33\xC9\xF3\x0F\x11\x43\x0C\x48\x89\x0B\x89\x4B\x08\x48\x85\xC0\x74\x2C", "xxxxxxxxxx????xxxxxxxxxxxxxxxxxxxxxxxx");
			EntityPosFunc = (delegate* unmanaged[Stdcall]<ulong, float*, ulong>)(address);

			// Find handling data functions
			address = FindPatternBmh("\x8B\xF7\x83\xF8\x01\x77\x08\x44\x8B\xF7\x8D\x77\xFF\xEB\x06\x41\xBE\x03\x00\x00\x00\x8B\x8B", "xxxxxxxxxxxxxxxxxxxxxxx");
			GetHandlingDataByIndex = (delegate* unmanaged[Stdcall]<int, ulong>)(new IntPtr(*(int*)(address + 28) + address + 32));
			handlingIndexOffsetInModelInfo = *(int*)(address + 23);

			address = FindPatternBmh("\x75\x5A\xB2\x01\x48\x8B\xCB\xE8\x00\x00\x00\x00\x41\x8B\xF5\x66\x44\x3B\xAB", "xxxxxxxx????xxxxxxx");
			GetHandlingDataByHash = (delegate* unmanaged[Stdcall]<IntPtr, ulong>)(
				new IntPtr(*(int*)(address - 7) + address - 3));

			// Find entity pools and interior proxy pool
			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			PedPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			ObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			FwScriptGuidPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			VehiclePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x4C\x8B\x05\x00\x00\x00\x00\x40\x8A\xF2\x8B\xE9", "xxx????xxxxx");
			PickupObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x83\x38\xFF\x74\x27\xD1\xEA\xF6\xC2\x01\x74\x20", "xxxxxxxxxxxx");
			if (address != null)
			{
				BuildingPoolAddress = (ulong*)(*(int*)(address + 47) + address + 51);
				AnimatedBuildingPoolAddress = (ulong*)(*(int*)(address + 15) + address + 19);
			}
			address = FindPatternBmh("\x83\xBB\x80\x01\x00\x00\x01\x75\x12", "xxxxxxxxx");
			if (address != null)
			{
				InteriorInstPoolAddress = (ulong*)(*(int*)(address + 23) + address + 27);
			}
			address = FindPatternBmh("\x0F\x85\xA3\x00\x00\x00\x8B\x52\x0C\x48\x8B\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x85\xC0\x0F\x84\x8B\x00\x00\x00\x45\x33\xC9\x45\x33\xC0", "xxxxxxxxxxxx????x????xxxxxxxxxxxxxxx");
			if (address != null)
			{
				InteriorProxyPoolAddress = (ulong*)(*(int*)(address + 12) + address + 16);
			}

			// Find euphoria functions
			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x20\x83\x61\x0C\x00\x44\x89\x41\x08\x49\x63\xC0", "xxxxxxxxxxxxxxxxx");
			InitMessageMemoryFunc = (delegate* unmanaged[Stdcall]<ulong, ulong, int, ulong>)(new IntPtr(address));

			address = FindPatternBmh("\x0F\x84\x8B\x00\x00\x00\x48\x8B\x47\x30\x48\x8B\x48\x10\x48\x8B\x51\x20\x80\x7A\x10\x0A", "xxxxxxxxxxxxxxxxxxxxxx");
			SendNmMessageToPedFunc = (delegate* unmanaged[Stdcall]<ulong, IntPtr, ulong, void>)((ulong*)(*(int*)(address - 0x1E) + address - 0x1A));

			address = FindPatternBmh("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmParameterInt = (delegate* unmanaged[Stdcall]<ulong, IntPtr, int, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			SetNmParameterBool = (delegate* unmanaged[Stdcall]<ulong, IntPtr, bool, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmParameterFloat = (delegate* unmanaged[Stdcall]<ulong, IntPtr, float, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			SetNmParameterString = (delegate* unmanaged[Stdcall]<ulong, IntPtr, IntPtr, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			SetNmParameterVector = (delegate* unmanaged[Stdcall]<ulong, IntPtr, float, float, float, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x4D\x8B\xF0\x48\x8B\xF2\xE8\x00\x00\x00\x00\x33\xFF\x48\x85\xC0\x75\x07\x32\xC0\xE9\xD8\x03\x00\x00", "xxxxxxx????xxxxxxxxxxxxxx");
			GetActiveTaskFunc = (delegate* unmanaged[Stdcall]<ulong, CTask*>)(new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x75\xEF\x48\x8B\x5C\x24\x30\xB8", "xxxxxxxx");
			if (address != null)
			{
				cTaskNMScriptControlTypeIndex = *(int*)(address + 8);
			}

			address = FindPatternBmh("\x4C\x8B\x03\x48\x8B\xD5\x48\x8B\xCB\x41\xFF\x50\x00\x83\xFE\x04", "xxxxxxxxxxxx?xxx");
			if (address != null)
			{
				// The instruction expects a signed value, but virtual function offsets can't be negative
				getEventTypeIndexVFuncOffset = (uint)*(byte*)(address + 12);
			}
			address = FindPatternBmh("\x48\x8D\x05\x00\x00\x00\x00\x48\x89\x01\x8B\x44\x24\x50", "xxx????xxxxxxx");
			if (address != null)
			{
				var cEventSwitch2NMVfTableArrayAddr = (ulong)(*(int*)(address + 3) + address + 7);
				var getEventTypeOfcEventSwitch2NMFuncAddr = *(ulong*)(cEventSwitch2NMVfTableArrayAddr + getEventTypeIndexVFuncOffset);
				cEventSwitch2NMTypeIndex = *(int*)(getEventTypeOfcEventSwitch2NMFuncAddr + 1);
			}

			address = FindPatternBmh("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
			GetLabelTextByHashAddress = (ulong)(*(int*)(address + 7) + address + 11);

			// Find the function that returns if the corresponding text label exist first.
			// We have to find GetLabelTextByHashFunc indirectly since Rampage Trainer hooks the function that returns the string address for corresponding text label hash by inserting jmp instruction at the beginning if that trainer is installed.
			address = FindPatternBmh("\x74\x64\x48\x8D\x15\x00\x00\x00\x00\x48\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x84\xC0\x74\x33", "xxxxx????xxx????x????xxxx");
			if (address != null)
			{
				var doesTextLabelExistFuncAddr = (byte*)(*(int*)(address + 17) + address + 21);
				var getLabelTextByHashFuncAddr = (long)(*(int*)(doesTextLabelExistFuncAddr + 28) + doesTextLabelExistFuncAddr + 32);
				GetLabelTextByHashFunc = (delegate* unmanaged[Stdcall]<ulong, int, ulong>)(new IntPtr(getLabelTextByHashFuncAddr));
			}

			address = FindPatternBmh("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
			CheckpointPoolAddress = (ulong*)(*(int*)(address + 17) + address + 21);
			GetCGameScriptHandlerAddressFunc = (delegate* unmanaged[Stdcall]<ulong>)(new IntPtr(*(int*)(address - 19) + address - 15));

			address = FindPatternBmh("\x4C\x8D\x05\x00\x00\x00\x00\x0F\xB7\xC1", "xxx????xxx");
			RadarBlipPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			address = FindPatternBmh("\xFF\xC6\x49\x83\xC6\x08\x3B\x35\x00\x00\x00\x00\x7C\x9B", "xxxxxxxx????xx");
			PossibleRadarBlipCountAddress = (int*)(*(int*)(address + 8) + address + 12);
			address = FindPatternBmh("\x3B\x35\x00\x00\x00\x00\x74\x2E\x48\x81\xFD\xDB\x05\x00\x00", "xx????xxxxxxxxx");
			UnkFirstRadarBlipIndexAddress = (int*)(*(int*)(address + 2) + address + 6);
			address = FindPatternBmh("\x41\xB8\x07\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFC", "xxxxxxxxxx????xxxx");
			NorthRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);
			address = FindPatternBmh("\x41\xB8\x06\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFD", "xxxxxxxxxx????xxxx");
			CenterRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);

			address = FindPatternBmh("\x33\xDB\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x07\x48\x8B\x40\x20\x8B\x58\x18", "xxx????xxxxxxxxxxxx");
			GetLocalPlayerPedAddressFunc = (delegate* unmanaged[Stdcall]<ulong>)(new IntPtr(*(int*)(address + 3) + address + 7));

			address = FindPatternBmh("\x4C\x8D\x05\x00\x00\x00\x00\x74\x07\xB8\x00\x00\x00\x00\xEB\x2D\x33\xC0", "xxx????xxx????xxxx");
			waypointInfoArrayStartAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			if (waypointInfoArrayStartAddress != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x8D\x15\x00\x00\x00\x00\x48\x83\xC1\x00\xFF\xC0\x48\x3B\xCA\x7C\xEA\x32\xC0", "xxx????xxx?xxxxxxxxx", startAddressToSearch);
				waypointInfoArrayEndAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}

			address = FindPatternBmh("\xF3\x0F\x10\x5C\x24\x20\xF3\x0F\x10\x54\x24\x24\xF3\x0F\x59\xD9\xF3\x0F\x59\xD1\xF3\x0F\x10\x44\x24\x28\xF3\x0F\x11\x1F", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				GetRotationFromMatrixFunc = (delegate* unmanaged[Stdcall]<float*, ulong, int, float*>)(new IntPtr(*(int*)(address - 0x14) + address - 0x10));
			}
			address = FindPatternBmh("\xF3\x0F\x11\x4D\x38\xF3\x0F\x11\x45\x3C\xE8\x00\x00\x00\x00\x0F\x28\xC6\x0F\x28\xCE\xB9\x01\x00\x00\x00\xF3\x0F\x11\x73\x10\x66\x44\x03\xE9", "xxxxxxxxxxx????xxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				GetQuaternionFromMatrixFunc = (delegate* unmanaged[Stdcall]<float*, ulong, int>)(new IntPtr(*(int*)(address + 11) + address + 15));
			}

			address = FindPatternBmh("\x48\x8B\x42\x20\x48\x85\xC0\x74\x09\xF3\x0F\x10\x80", "xxxxxxxxxxxxx");
			if (address != null)
			{
				EntityMaxHealthOffset = *(int*)(address + 0x25);
			}

			address = FindPatternBmh("\x75\x11\x48\x8B\x06\x48\x8D\x54\x24\x20\x48\x8B\xCE\xFF\x90", "xxxxxxxxxxxxxxx");
			if (address != null)
			{
				SetAngularVelocityVFuncOfEntityOffset = *(int*)(address + 15);
				GetAngularVelocityVFuncOfEntityOffset = SetAngularVelocityVFuncOfEntityOffset + 0x8;
			}

			address = FindPatternBmh("\x48\x8B\x89\x00\x00\x00\x00\x33\xC0\x44\x8B\xC2\x48\x85\xC9\x74\x20", "xxx????xxxxxxxxxx");
			cAttackerArrayOfEntityOffset = *(uint*)(address + 3); // the correct name is unknown
			if (address != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x63\x51\x00\x48\x85\xD2", "xxx?xxx", startAddressToSearch);
				elementCountOfCAttackerArrayOfEntityOffset = (uint)(*(sbyte*)(address + 3));

				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x83\xC1\x00\x48\x3B\xC2\x7C\xEF", "xxx?xxxxx", startAddressToSearch);
				// the element size might be 0x10 in older builds (the size is 0x18 at least in b1604 and b2372)
				elementSizeOfCAttackerArrayOfEntity = (uint)(*(sbyte*)(address + 3));
			}

			address = FindPatternBmh("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
			cursorSpriteAddr = (int*)(*(int*)(address - 4) + address);

			address = FindPatternBmh("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05", "xxxxxx????xxxxxxxxx");
			readWorldGravityAddress = (float*)(*(int*)(address + 19) + address + 23);
			writeWorldGravityAddress = (float*)(*(int*)(address + 6) + address + 10);

			address = FindPatternBmh("\xF3\x0F\x11\x05\x00\x00\x00\x00\xF3\x0F\x10\x08\x0F\x2F\xC8\x73\x03\x0F\x28\xC1\x48\x83\xC0\x04\x49\x2B", "xxxx????xxxxxxxxxxxxxxxxxx");
			var timeScaleArrayAddress = (float*)(*(int*)(address + 4) + address + 8);
			if (timeScaleArrayAddress != null)
				// SET_TIME_SCALE changes the 2nd element, so obtain the address of it
				timeScaleAddress = timeScaleArrayAddress + 1;

			address = FindPatternBmh("\xF3\x0F\x11\xB5\x60\x01\x00\x00\x84\xC0\x75\x4C\x85\xC9\x79\x1D\x33\xD2\xE8", "xxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				var unkClockFunc = (byte*)(*(int*)(address + 19) + address + 23);
				millisecondsPerGameMinuteAddress = (int*)(*(int*)(unkClockFunc + 0x46) + unkClockFunc + 0x4A);
			}

			address = FindPatternBmh("\x75\x2D\x44\x38\x3D\x00\x00\x00\x00\x75\x24", "xxxxx????xx");
			isClockPausedAddress = (bool*)(*(int*)(address + 5) + address + 9);

			// Find camera objects
			address = FindPatternBmh("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
			CameraPoolAddress = (ulong*)(*(int*)(address) + address + 4);
			address = FindPatternBmh("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
			address = address + *(int*)(address) + 4;
			GameplayCameraAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			// Find model hash table
			address = FindPatternBmh("\x3C\x05\x75\x16\x8B\x81", "xxxxxx");
			if (address != null)
				VehicleTypeOffsetInModelInfo = *(int*)(address + 6);

			address = FindPatternBmh("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			var vehicleClassOffset = *(uint*)(address + 0x31);

			address = address + *(int*)(address) + 4;
			modelNum1 = *(UInt32*)(*(int*)(address + 0x52) + address + 0x56);
			modelNum2 = *(UInt64*)(*(int*)(address + 0x63) + address + 0x67);
			modelNum3 = *(UInt64*)(*(int*)(address + 0x7A) + address + 0x7E);
			modelNum4 = *(UInt64*)(*(int*)(address + 0x81) + address + 0x85);
			modelHashTable = *(UInt64*)(*(int*)(address + 0x24) + address + 0x28);
			modelHashEntries = *(UInt16*)(address + *(int*)(address + 3) + 7);

			address = FindPatternBmh("\x33\xD2\x00\x8B\xD0\x00\x2B\x05\x00\x00\x00\x00\xC1\xE6\x10", "xx?xx?xx????xxx");
			modelInfoArrayPtr = (ulong*)(*(int*)(address + 8) + address + 12);

			address = FindPatternBmh("\x48\x83\xEC\x20\x48\x8B\x91\x00\x00\x00\x00\x33\xF6\x48\x8B\xD9\x48\x85\xD2\x74\x2B\x48\x8D\x0D", "xxxxxxx??xxxxxxxxxxxxxxx");
			cStreamingAddr = (ulong*)(*(int*)(address + 24) + address + 28);

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x41\x8B\x1E", "xxx????xxx");
			weaponAndAmmoInfoArrayPtr = (RageAtArrayPtr*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x84\xC0\x74\x20\x48\x8B\x47\x40\x48\x85\xC0\x74\x08\x8B\xB0\x00\x00\x00\x00\xEB\x02\x33\xF6\x48\x8D\x4D\x48\xE8", "xxxxxxxxxxxxxxx????xxxxxxxxx");
			weaponInfoHumanNameHashOffset = *(int*)(address + 15);

			address = FindPatternBmh("\x8B\x05\x00\x00\x00\x00\x44\x8B\xD3\x8D\x48\xFF", "xx????xxxxxx");
			if (address != null)
			{
				weaponComponentArrayCountAddr = (uint*)(*(int*)(address + 2) + address + 6);

				address = FindPatternNaive("\x46\x8D\x04\x11\x48\x8D\x15\x00\x00\x00\x00\x41\xD1\xF8", "xxxxxxx????xxx", new IntPtr(address));
				offsetForCWeaponComponentArrayAddr = (ulong)(address + 7);

				address = FindPatternNaive("\x74\x10\x49\x8B\xC9\xE8", "xxxxxx", new IntPtr(address));
				var findAttachPointFuncAddr = new IntPtr((long)(*(int*)(address + 6) + address + 10));

				address = FindPatternNaive("\x4C\x8D\x81", "xxx", findAttachPointFuncAddr);
				weaponAttachPointsStartOffset = *(int*)(address + 3);
				address = FindPatternNaive("\x4D\x63\x98", "xxx", new IntPtr(address));
				weaponAttachPointsArrayCountOffset = *(int*)(address + 3);
				address = FindPatternNaive("\x4C\x63\x50", "xxx", new IntPtr(address));
				weaponAttachPointElementComponentCountOffset = *(byte*)(address + 3);
				address = FindPatternNaive("\x48\x83\xC0", "xxx", new IntPtr(address));
				weaponAttachPointElementSize = *(byte*)(address + 3);
			}

			address = FindPatternBmh("\x24\x1F\x3C\x05\x0F\x85\x00\x00\x00\x00\x48\x8D\x82\x00\x00\x00\x00\x48\x8D\xB2\x00\x00\x00\x00\x48\x85\xC0\x74\x09\x80\x38\x00\x74\x04\x8A\xCB", "xxxxxx????xxx????xxx????xxxxxxxxxxxx");
			if (address != null)
			{
				vehicleMakeNameOffsetInModelInfo = *(int*)(address + 13);
			}

			address = FindPatternBmh("\x66\x89\x44\x24\x38\x8B\x44\x24\x38\x8B\xC8\x33\x4C\x24\x30\x81\xE1\x00\x00\xFF\x0F\x33\xC1\x0F\xBA\xF0\x1D\x8B\xC8\x33\x4C\x24\x30\x23\xCB\x33\xC1", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				pedPersonalityIndexOffsetInModelInfo = *(int*)(address + 0x42);
				pedPersonalitiesArrayAddr = (ulong*)(*(int*)(address + 0x49) + address + 0x4D);
			}

			address = FindPatternBmh("\x49\x8B\xF1\x48\x8B\xF9\x0F\x57\xC0\x0F\x28\xF9\x0F\x28\xF2\x74\x4C", "xxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				CVehicleEngineOffset = *(int*)(address + 0x24);

				NextGearOffset = CVehicleEngineOffset;
				GearOffset = CVehicleEngineOffset + 2;
				HighGearOffset = CVehicleEngineOffset + 6;

				address = FindPatternBmh("\x0F\x28\xC3\xF3\x0F\x5C\xC2\x0F\x2F\xC8\x76\x2E\x0F\x2F\xDA\x73\x29\xF3\x0F\x10\x44\x24\x58", "xxxxxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					CVehicleEngineTurboOffset = (int)*(byte*)(address - 1);
				}
			}

			address = FindPatternBmh("\x74\x26\x0F\x57\xC9\x0F\x2F\x8B\x34\x08\x00\x00\x73\x1A\xF3\x0F\x10\x83", "xxxxxxxx????xxxxxx");
			if (address != null)
			{
				FuelLevelOffset = *(int*)(address + 8);
			}
			address = FindPatternBmh("\x74\x2D\x0F\x57\xC0\x0F\x2F\x83", "xxxxxxxx");
			if (address != null)
			{
				OilLevelOffset = *(int*)(address + 8);
			}

			address = FindPatternBmh("\xF3\x0F\x10\x8F\x10\x0A\x00\x00\xF3\x0F\x59\x05", "xxxx????xxxx");
			if (address != null)
			{
				WheelSpeedOffset = *(int*)(address + 4);
			}

			address = FindPatternBmh("\x48\x63\x99\x00\x00\x00\x00\x45\x33\xC0\x45\x8B\xD0\x48\x85\xDB", "xxx????xxxxxxxxx");
			if (address != null)
			{
				WheelCountOffset = *(int*)(address + 3);
				WheelPtrArrayOffset = WheelCountOffset - 8;
				WheelBoneIdToPtrArrayIndexOffset = WheelCountOffset + 4;
			}

			address = FindPatternBmh("\x74\x18\x80\xA0\x00\x00\x00\x00\xBF\x84\xDB\x0F\x94\xC1\x80\xE1\x01\xC0\xE1\x06", "xxxx????xxxxxxxxxxxx");
			if (address != null)
			{
				CanWheelBreakOffset = *(int*)(address + 4);
			}

			address = FindPatternBmh("\x76\x03\x0F\x28\xF0\xF3\x44\x0F\x10\x93", "xxxxxxxxxx");
			if (address != null)
			{
				CurrentRPMOffset = *(int*)(address + 10);
				ClutchOffset = *(int*)(address + 10) + 0xC;
				AccelerationOffset = *(int*)(address + 10) + 0x10;
			}

			address = FindPatternBmh("\x74\x0A\xF3\x0F\x11\xB3\x1C\x09\x00\x00\xEB\x25", "xxxxxx????xx");
			if (address != null)
			{
				SteeringScaleOffset = *(int*)(address + 6);
				SteeringAngleOffset = *(int*)(address + 6) + 8;
				ThrottlePowerOffset = *(int*)(address + 6) + 0x10;
				BrakePowerOffset = *(int*)(address + 6) + 0x14;
			}

			address = FindPatternBmh("\xF3\x0F\x11\x9B\xDC\x09\x00\x00\x0F\x84\xB1", "xxxx????xxx");
			if (address != null)
			{
				EngineTemperatureOffset = *(int*)(address + 4);
			}

			var gameVersion = GetGameVersion();

			// Get the offset that is stored by MODIFY_VEHICLE_TOP_SPEED if the game version is b944 or later for existing script compatibility
			// MODIFY_VEHICLE_TOP_SPEED stores the 2nd argument value to CVehicle in b944 or later, but that's not the case in earlier ones
			if (gameVersion >= 28)
			{

				address = FindPatternBmh("\x48\x89\x5C\x24\x28\x44\x0F\x29\x40\xC8\x0F\x28\xF9\x44\x0F\x29\x48\xB8\xF3\x0F\x11\xB9", "xxxxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					var modifyVehicleTopSpeedOffset1 = *(int*)(address - 4);
					var modifyVehicleTopSpeedOffset2 = *(int*)(address + 22);
					EnginePowerMultiplierOffset = modifyVehicleTopSpeedOffset1 + modifyVehicleTopSpeedOffset2;
				}
			}

			address = FindPatternBmh("\x48\x8B\xF8\x48\x85\xC0\x0F\x84\xE2\x00\x00\x00\x80\x88", "xxxxxxxxxxxxxx");
			if (address != null)
			{
				DisablePretendOccupantOffset = *(int*)(address + 14);
			}

			address = FindPatternBmh("\x48\x83\xEC\x20\x49\x8B\xF8\x48\x8B\xDA\x48\x85\xD2\x74\x4A\x80\x7A\x28\x03\x75\x44\xF6\x82", "xxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				VehicleProvidesCoverOffset = *(int*)(address + 23);
			}

			address = FindPatternBmh("\x74\x03\x41\x8A\xF4\xF3\x44\x0F\x59\x93\x00\x00\x00\x00\x48\x8B\xCB\xF3\x44\x0F\x59\x97\xFC\x00\x00\x00\xF3\x44\x0F\x59\xD6", "xxxxxxxxxx????xxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				VehicleLightsMultiplierOffset = *(int*)(address + 10);
			}

			address = FindPatternBmh("\xFD\x02\xDB\x08\x98\x00\x00\x00\x00\x48\x8B\x5C\x24\x30", "xxxxx????xxxxx");
			if (address != null)
			{
				IsInteriorLightOnOffset = *(int*)(address - 4);
				IsEngineStartingOffset = IsInteriorLightOnOffset + 1;
			}

			address = FindPatternBmh("\x39\x00\x75\x0F\x48\xFF\xC1\x48\x83\xC0\x04\x48\x83\xF9\x02\x7C\xEF\xEB\x08\x48\x8B\xCF", "x?xxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{

				address = FindPatternNaive("\x48\x8D\x8F\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x8A\x87", "xxx????x????xx", new IntPtr(address - 0x50), 0x50u);
				if (address != null)
				{
					var unkVehHeadlightOffset = *(int*)(address + 3);
					var unkFuncAddr = (*(int*)(address + 8) + address + 12);
					address = FindPatternBmh("\x8B\xC3\x8B\xCB\x48\xC1\xE8\x05\x83\xE1\x1F", "xxxxxxxxxxx", new IntPtr(unkFuncAddr), 0x200u);
					if (address != null)
					{
						IsHeadlightDamagedOffset = *(int*)(address + 14) + unkVehHeadlightOffset;
					}
				}
			}

			address = FindPatternBmh("\x8B\xCB\x89\x87\x90\x00\x00\x00\x8A\x86\xD8\x00\x00\x00\x24\x07\x41\x3A\xC6\x0F\x94\xC1\xC1\xE1\x12\x33\xCA\x81\xE1\x00\x00\x04\x00\x33\xCA", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				VehicleLodMultiplierOffset = *(int*)(address - 4);
			}

			address = FindPatternBmh("\x8A\x96\x00\x00\x00\x00\x0F\xB6\xC8\x84\xD2\x41", "xx????xxxxxx");
			if (address != null)
			{
				IsWantedOffset = *(int*)(address + 40);
			}

			address = FindPatternBmh("\x45\x33\xC9\x41\xB0\x01\x40\x8A\xD7", "xxxxxxxxx");
			if (address != null)
			{
				PreviouslyOwnedByPlayerOffset = *(int*)(address - 5);
				NeedsToBeHotwiredOffset = PreviouslyOwnedByPlayerOffset;
			}

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x20\x48\x8B\x81\xD0\x00\x00\x00\x48\x8B\xD9\x48\x85\xC0\x74\x06\x80\x78\x4B\x00\x75\x65", "xxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				AlarmTimeOffset = *(int*)(address + 0x2C);
			}

			address = FindPatternBmh("\x8B\xCB\x89\x87\x90\x00\x00\x00\x8A\x86\xD8\x00\x00\x00\x24\x07\x41\x3A\xC6\x0F\x94\xC1\xC1\xE1\x12\x33\xCA\x81\xE1\x00\x00\x04\x00\x33\xCA", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				VehicleLodMultiplierOffset = *(int*)(address - 4);
			}

			address = FindPatternNaive("\x48\x85\xC0\x74\x32\x8A\x88\x00\x00\x00\x00\xF6\xC1\x00\x75\x27", "xxxxxxx????xx?xx");
			if (address != null)
			{
				HasMutedSirensOffset = *(int*)(address + 7);
				HasMutedSirensBit = *(byte*)(address + 13); // the bit is changed between b372 and b2802
				CanUseSirenOffset = *(int*)(address + 23);
			}

			address = FindPatternBmh("\x41\xBB\x07\x00\x00\x00\x8A\xC2\x41\x23\xCB\x41\x22\xC3\x3C\x03\x75\x16", "xxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				VehicleTypeOffsetInCVehicle = *(int*)(address - 0x1C);
			}

			address = FindPatternBmh("\x83\xB8\x00\x00\x00\x00\x00\x77\x12\x80\xA0\x00\x00\x00\x00\xFD\x80\xE3\x01\x02\xDB\x08\x98", "xx?????xxxx????xxxxxxxx");
			if (address != null)
			{
				VehicleDropsMoneyWhenBlownUpOffset = *(int*)(address + 11);
			}

			address = FindPatternBmh("\x73\x1E\xF3\x41\x0F\x59\x86\x00\x00\x00\x00\xF3\x0F\x59\xC2\xF3\x0F\x59\xC7", "xxxxxxx????xxxxxxxx");
			if (address != null)
			{
				HeliBladesSpeedOffset = *(int*)(address + 7);
			}

			address = FindPatternBmh("\xB3\x03\x22\xD3\x48\x8B\xCF\xE8\x00\x00\x00\x00\x48\x8B\xCF\xF3\x0F\x11\x86\x00\x00\x00\x00\x8A\x97\xD4\x00\x00\x00\xC0\xEA\x02\x22\xD3\xE8", "xxxxxxxx????xxxxxxx????xxxxxxxxxxxx");
			if (address != null)
			{
				HeliMainRotorHealthOffset = *(int*)(address + 19);
				HeliTailRotorHealthOffset = HeliMainRotorHealthOffset + 4;
				HeliTailBoomHealthOffset = HeliMainRotorHealthOffset + 8;
			}

			address = FindPatternBmh("\x3C\x03\x0F\x85\x00\x00\x00\x00\x48\x8B\x41\x20\x48\x8B\x88", "xxxx????xxxxxxx");
			if (address != null)
			{
				HandlingDataOffset = *(int*)(address + 22);
			}

			address = FindPatternBmh("\x45\x33\xF6\x8B\xEA\x48\x8B\xF1\x41\x8B\xFE", "xxxxxxxxxxx");
			if (address != null)
			{
				SubHandlingDataArrayOffset = (*(int*)(address + 15) - 8);
			}

			address = FindPatternNaive("\x48\x85\xC0\x74\x3C\x8B\x80\x00\x00\x00\x00\xC1\xE8\x0F", "xxxxxxx????xxx");
			if (address != null)
			{
				FirstVehicleFlagsOffset = *(int*)(address + 7);
			}

			address = FindPatternBmh("\xF3\x0F\x59\x05\x00\x00\x00\x00\xF3\x0F\x59\x83\x00\x00\x00\x00\xF3\x0F\x10\xC8\x0F\xC6\xC9\x00", "xxxx????xxxx????xxxxxxxx");
			if (address != null)
			{
				VehicleWheelSteeringLimitMultiplierOffset = *(int*)(address + 12);
				VehicleWheelSuspensionStrengthOffset = VehicleWheelSteeringLimitMultiplierOffset - 4;
			}

			address = FindPatternBmh("\xF3\x0F\x5C\xC8\x0F\x2F\xCB\xF3\x0F\x11\x89\x00\x00\x00\x00\x72\x10\xF3\x0F\x10\x1D", "xxxxxxxxxxx????xxxxxx");
			if (address != null)
			{
				VehicleWheelTemperatureOffset = *(int*)(address + 11);
			}

			address = FindPatternBmh("\x74\x13\x0F\x57\xC0\x0F\x2E\x80", "xxxxxxxx");
			if (address != null)
			{
				VehicleTireHealthOffset = *(int*)(address + 8);
				VehicleWheelHealthOffset = VehicleTireHealthOffset - 4;
			}

			// the tire wear multipiler value for vehicle wheels is present only in b1868 or newer versions
			if (gameVersion >= 54)
			{
				address = FindPatternNaive("\x45\x84\xF6\x74\x08\xF3\x0F\x59\x0D\x00\x00\x00\x00\xF3\x0F\x10\x83", "xxxxxxxxx????xxxx");
				if (address != null)
				{
					VehicleTireWearMultiplierOffset = *(int*)(address + 0x22);
					// Note: The values for SET_TYRE_WEAR_RATE_SCALE and SET_TYRE_MAXIMUM_GRIP_DIFFERENCE_DUE_TO_WEAR_RATE are not present in b1868
				}
			}

			address = FindPatternBmh("\x0F\xBF\x88\x00\x00\x00\x00\x3B\xCA\x74\x17", "xxx????xxxx");
			if (address != null)
			{
				VehicleWheelIdOffset = *(int*)(address + 3);
			}

			address = FindPatternNaive("\xEB\x02\x33\xC9\xF6\x81\x00\x00\x00\x00\x01\x75\x43", "xxxxxx????xxx");
			if (address != null)
			{
				VehicleWheelTouchingFlagsOffset = *(int*)(address + 6);
			}

			address = FindPatternBmh("\x74\x21\x8B\xD7\x48\x8B\xCB\xE8\x00\x00\x00\x00\x48\x8B\xC8\xE8", "xxxxxxxx????xxxx");
			if (address != null)
			{
				FixVehicleWheelFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr(*(int*)(address + 16) + address + 20));
				address = FindPatternNaive("\x80\xA1\x00\x00\x00\x00\xFD", "xx????x", new IntPtr(address + 20));
				ShouldShowOnlyVehicleTiresWithPositiveHealthOffset = *(int*)(address + 2);
			}

			// Vehicle Wheels has the owner vehicle pointer and new wheel functions are used since b1365
			if (gameVersion >= 40)
			{
				address = FindPatternBmh("\x4C\x8B\x81\x28\x01\x00\x00\x0F\x29\x70\xE8\x0F\x29\x78\xD8", "xxxxxxxxxxxxxxx");
				PunctureVehicleTireNewFunc = (delegate* unmanaged[Stdcall]<IntPtr, ulong, float, ulong, ulong, int, byte, bool, void>)(new IntPtr((long)(address - 0x10)));
				address = FindPatternBmh("\x48\x83\xEC\x50\x48\x8B\x81\x00\x00\x00\x00\x48\x8B\xF1\xF6\x80", "xxxxxxx????xxxxx");
				BurstVehicleTireOnRimNewFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr((long)(address - 0xB)));
			}
			else
			{
				address = FindPatternBmh("\x41\xF6\x81\x00\x00\x00\x00\x20\x0F\x29\x70\xE8\x0F\x29\x78\xD8\x49\x8B\xF9", "xxx????xxxxxxxxxxxx");
				PunctureVehicleTireOldFunc = (delegate* unmanaged[Stdcall]<IntPtr, ulong, float, IntPtr, ulong, ulong, int, byte, bool, void>)(new IntPtr((long)(address - 0x14)));
				address = FindPatternBmh("\x48\x83\xEC\x50\xF6\x82\x00\x00\x00\x00\x20\x48\x8B\xF2\x48\x8B\xE9", "xxxxxx????xxxxxxx");
				BurstVehicleTireOnRimOldFunc = (delegate* unmanaged[Stdcall]<IntPtr, IntPtr, void>)(new IntPtr((long)(address - 0x10)));
			}

			// The values for special flight mode (e.g. Deluxo) are present only in b1290 or later versions
			if (gameVersion >= 38)
			{
				address = FindPatternBmh("\x41\x0F\x2F\xC1\x72\x2E\xF6\x83", "xxxxxxxx");
				if (address != null)
				{
					SpecialFlightTargetRatioOffset = *(int*)(address + 0x1C);
					SpecialFlightWingRatioOffset = SpecialFlightTargetRatioOffset + 0x4;
					SpecialFlightAreWingsDisabledOffset = SpecialFlightTargetRatioOffset + 0x1C;
					SpecialFlightCurrentRatioOffset = SpecialFlightTargetRatioOffset + 0x28;
				}
			}

			address = FindPatternBmh("\x66\x0F\x6E\xF0\x0F\x5B\xF6\xE8\x00\x00\x00\x00\x0F\x28\xCE\x41\xB1\x01\x45\x33\xC0\x48\x8B\xC8\xE8", "xxxxxxxx????xxxxxxxxxxxxx");
			if (address != null)
			{
				CPlayerInfoMaxHealthOffset = *(int*)(address - 4);
			}
			// None of variables for player targeting and wanted info are not accessed with direct offsets from CPlayerInfo instances in the game code
			address = FindPatternBmh("\x48\x85\xFF\x74\x23\x48\x85\xDB\x74\x26", "xxxxxxxxxx");
			if (address != null)
			{
				CPlayerPedTargetingOfffset = *(int*)(address - 7);
			}
			address = FindPatternBmh("\x48\x85\xFF\x74\x3F\x48\x8B\xCF\xE8", "xxxxxxxxx");
			if (address != null)
			{
				CWantedOffset = *(int*)(address + 0x1B);
			}
			address = FindPatternBmh("\x45\x84\xC9\x74\x32\x8B\x41\x00\x85\xC0\x74\x2B", "xxxxxxx?xxxx");
			if (address != null)
			{
				CurrentCrimeValueOffset = (int)*(byte*)(address + 0x2A);
				TimeToApplyPendingCrimeValueOffset = (int)*(byte*)(address + 0x7);
				CurrentWantedLevelOffset = *(int*)(address + 0x17);
				PendingCrimeValueOffset = *(int*)(address + 0x1D);
			}
			address = FindPatternBmh("\xF6\x87\x00\x00\x00\x00\x02\x44\x8B\x00\x00\x00\x00\x00\x75\x0E", "xx??xxxxx?????xx");
			if (address != null)
			{
				var isWantedStarFlashingOffset = *(int*)(address + 0x2);
				// Flags for ignoring player are actually read/written as a byte in the game code, but make this value 4-byte aligned because SetBit and IsBitSet reads/writes as an int value
				CWantedIgnorePlayerFlagOffset = isWantedStarFlashingOffset - 3;

				CWantedLastTimeSearchAreaRefocusedOffset = isWantedStarFlashingOffset - 0x23;
				CWantedLastTimeSpottedByPoliceOffset = CWantedLastTimeSearchAreaRefocusedOffset + 0x4;
				CWantedStartTimeOfHiddenEvasionOffset = CWantedLastTimeSearchAreaRefocusedOffset + 0xC;
			}
			address = FindPatternBmh("\xEB\x26\x8B\x87\x00\x00\x00\x00\x25\x00\xF8\xFF\xFF\xC1\xE0\x12", "xxxx????xxxxxxxx");
			if (address != null)
			{
				ActivateSpecialAbilityFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr(*(int*)(address + 0x24) + address + 0x28));
			}
			// Two special ability slots are available in b2060 and later versions
			if (gameVersion >= 59)
			{
				address = FindPatternBmh("\x0F\x84\x49\x01\x00\x00\x33\xD2\xE8", "xxxxxxxxx");
				if (address != null)
				{
					GetSpecialAbilityAddressFunc = (delegate* unmanaged[Stdcall]<IntPtr, int, IntPtr>)(new IntPtr(*(int*)(address + 9) + address + 13));
				}
			}
			else
			{
				address = FindPatternBmh("\x0F\x84\x46\x01\x00\x00\x48\x8B\x9B", "xxxxxxxxx");
				if (address != null)
				{
					PlayerPedSpecialAbilityOffset = *(int*)(address + 9);
				}
			}

			address = FindPatternBmh("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
			if (address != null)
			{
				PedIntelligenceOffset = *(int*)(address + 0x11);

				var setDecisionMakerHashFuncAddr = *(int*)(address + 0x18) + address + 0x1C;
				PedIntelligenceDecisionMakerHashOffset = *(int*)(setDecisionMakerHashFuncAddr + 0x1C);

				PedPlayerInfoOffset = PedIntelligenceOffset + 0x8;
				CPedIntentoryOfCPedOffset = PedIntelligenceOffset + 0x10;
				UnkCPedStateOffset = PedIntelligenceOffset - 0x10;
			}

			address = FindPatternNaive("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
			if (address != null)
			{
				PedIntelligenceOffset = *(int*)(address + 0x11);

				var setDecisionMakerHashFuncAddr = *(int*)(address + 0x18) + address + 0x1C;
				PedIntelligenceDecisionMakerHashOffset = *(int*)(setDecisionMakerHashFuncAddr + 0x1C);
			}

			address = FindPatternBmh("\x48\x8B\x88\x00\x00\x00\x00\x48\x85\xC9\x74\x43\x48\x85\xD2", "xxx????xxxxxxxx");
			if (address != null)
			{
				CTaskTreePedOffset = *(int*)(address + 3);
			}

			address = FindPatternNaive("\x40\x38\x3D\x00\x00\x00\x00\x8B\xB6\x00\x00\x00\x00\x74\x0C", "xxx????xx????xx");
			if (address != null)
			{
				CEventCountOffset = *(int*)(address + 9);
				address = FindPatternNaive("\x48\x8B\xB4\xC6", "xxxx", new IntPtr(address));
				CEventStackOffset = *(int*)(address + 4);
			}

			address = FindPatternNaive("\x48\x83\xEC\x28\x48\x8B\x42\x00\x48\x85\xC0\x74\x09\x48\x3B\x82\x00\x00\x00\x00\x74\x21", "xxxxxxx?xxxxxxxx????xx");
			if (address != null)
			{
				fragInstNMGtaOffset = *(int*)(address + 16);
			}
			address = FindPatternNaive("\xB2\x01\x48\x8B\x01\xFF\x90\x00\x00\x00\x00\x80", "xxxxxxx????x");
			if (address != null)
			{
				fragInstNMGtaGetUnkValVFuncOffset = (uint)*(int*)(address + 7);
			}

			address = FindPatternBmh("\x0F\x93\xC0\x84\xC0\x74\x0F\xF3\x41\x0F\x58\xD1\x41\x0F\x2F\xD0\x72\x04\x41\x0F\x28\xD0", "xxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				SweatOffset = *(int*)(address + 26);
			}

			address = FindPatternBmh("\x8A\x41\x30\xC0\xE8\x03\xA8\x01\x74\x49\x8B\x82", "xxxxxxxxxxxx");
			if (address != null)
			{
				PedIsInVehicleOffset = *(int*)(address + 12);
				PedLastVehicleOffset = *(int*)(address + 0x1A);
			}
			address = FindPatternBmh("\x24\x3F\x0F\xB6\xC0\x66\x89\x87", "xxxxxxxx");
			if (address != null)
			{
				SeatIndexOffset = *(int*)(address + 8);
			}

			address = FindPatternBmh("\xC1\xE8\x11\xA8\x01\x75\x10\x48\x8B\xCB\xE8\x00\x00\x00\x00\x84\xC0\x0F\x84", "xxxxxxxxxxx????xxxx");
			if (address != null)
			{
				PedDropsWeaponsWhenDeadOffset = *(int*)(address - 4);
			}

			address = FindPatternBmh("\x4D\x8B\xF1\x48\x8B\xFA\xC1\xE8\x02\x48\x8B\xF1\xA8\x01\x0F\x85\xEB\x00\x00\x00", "xxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				PedSuffersCriticalHitOffset = *(int*)(address - 4);
			}

			// Implementation of armor system was different drastically in the game version between b877 and b2699 and the other versions
			if (gameVersion >= 80 || gameVersion <= 25)
			{
				address = FindPatternBmh("\x66\x0F\x6E\xC1\x0F\x5B\xC0\x41\x0F\x2F\x86\x00\x00\x00\x00\x0F\x97\xC0\xEB\x02\x32\xC0\x48\x8B\x5C\x24\x40", "xxxxxxxxxxx????xxxxxxxxxxxx");
				if (address != null)
				{
					ArmorOffset = *(int*)(address + 11);
					InjuryHealthThresholdOffset = ArmorOffset + 0xC;
					FatalInjuryHealthThresholdOffset = ArmorOffset + 0x10;
				}
			}
			else
			{
				address = FindPatternBmh("\x0F\x29\x70\xD8\x0F\x29\x78\xC8\x49\x8B\xF0\x48\x8B\xEA\x4C\x8B\xF1\x44\x0F\x29\x40\xB8", "xxxxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					ArmorOffset = *(int*)(address + 0x1F);
				}

				// Injury healths value are stored with different distance from the armor value in different game versions, search for another pattern to make sure we get correct injury health offsets
				address = FindPatternBmh("\xF3\x41\x0F\x10\x16\xF3\x0F\x10\xA7\xA0\x02\x00\x00\x0F\x28\xC3\xF3\x0F\x5C\xC2\x0F\x2F\xC6\x72\x05\x0F\x28\xCE\xEB\x12", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					InjuryHealthThresholdOffset = *(int*)(address - 4);
					FatalInjuryHealthThresholdOffset = InjuryHealthThresholdOffset + 0x4;
				}
			}

			address = FindPatternBmh("\x8B\x83\x00\x00\x00\x00\x8B\x35\x00\x00\x00\x00\x3B\xF0\x76\x04", "xx????xx????xxxx");
			if (address != null)
			{
				PedTimeOfDeathOffset = *(int*)(address + 2);
				PedCauseOfDeathOffset = PedTimeOfDeathOffset - 4;
				PedSourceOfDeathOffset = PedTimeOfDeathOffset - 12;
			}

			address = FindPatternNaive("\x74\x08\x8B\x81\x00\x00\x00\x00\xEB\x0D\x48\x8B\x87\x00\x00\x00\x00\x8B\x80", "xxxx????xxxxx????xx");
			if (address != null)
			{
				FiringPatternOffset = *(int*)(address + 19);
			}

			address = FindPatternBmh("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
			if (address != null)
			{
				var setDecisionMakerHashFuncAddr = *(int*)(address + 0x18) + address + 0x1C;
				PedIntelligenceDecisionMakerHashOffset = *(int*)(setDecisionMakerHashFuncAddr + 0x1C);
			}

			address = FindPatternBmh("\xC1\xE8\x09\xA8\x01\x74\xAE\x0F\x28\x00\x00\x00\x00\x00\x49\x8B\x47\x30\xF3\x0F\x10\x81", "xxxxxxxxx????xxxxxxxxx");
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

			address = FindPatternBmh("\x48\x8B\x87\x00\x00\x00\x00\x48\x85\xC0\x0F\x84\x8B\x00\x00\x00", "xxx????xxxxxxxxx");
			if (address != null)
			{
				objParentEntityAddressDetachedFromOffset = *(int*)(address + 3);
			}

			address = FindPatternBmh("\x48\x8D\x1D\x00\x00\x00\x00\x4C\x8B\x0B\x4D\x85\xC9\x74\x67", "xxx????xxxxxxxx");
			if (address != null)
			{
				ProjectilePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}
			// Find address of the projectile count, just in case the max number of projectile changes from 50
			address = FindPatternBmh("\x44\x8B\x0D\x00\x00\x00\x00\x33\xDB\x45\x8A\xF8", "xxx????xxxxx");
			if (address != null)
			{
				ProjectileCountAddress = (int*)(*(int*)(address + 3) + address + 7);
			}
			address = FindPatternBmh("\x48\x85\xED\x74\x09\x48\x39\xA9\x00\x00\x00\x00\x75\x2D", "xxxxxxxx????xx");
			if (address != null)
			{
				ProjectileOwnerOffset = *(int*)(address + 8);
			}
			address = FindPatternBmh("\x45\x85\xF6\x74\x0D\x48\x8B\x81\x00\x00\x00\x00\x44\x39\x70\x10", "xxxxxxxx????xxxx");
			if (address != null)
			{
				ProjectileAmmoInfoOffset = *(int*)(address + 8);
			}
			address = FindPatternBmh("\x39\x70\x10\x75\x17\x40\x84\xED\x74\x09\x33\xD2\xE8", "xxxxxxxxxxxxx");
			if (address != null)
			{
				ExplodeProjectileFunc = (delegate* unmanaged[Stdcall]<IntPtr, int, void>)(new IntPtr(*(int*)(address + 13) + address + 17));
			}

			address = FindPatternNaive("\x0F\xBE\x5E\x06\x48\x8B\xCF\xFF\x50\x00\x8B\xD3\x48\x8B\xC8\xE8\x00\x00\x00\x00\x8B\x4E", "xxxxxxxxx?xxxxxx????xx");
			if (address != null)
			{
				getFragInstVFuncOffset = *(sbyte*)(address + 9);
				detachFragmentPartByIndexFunc = (delegate* unmanaged[Stdcall]<FragInst*, int, FragInst*>)(new IntPtr(*(int*)(address + 16) + address + 20));
			}
			address = FindPatternBmh("\x74\x56\x48\x8B\x0D\x00\x00\x00\x00\x41\x0F\xB7\xD0\x45\x33\xC9\x45\x33\xC0", "xxxxx????xxxxxxxxxx");
			if (address != null)
			{
				phSimulatorInstPtr = (ulong**)(*(int*)(address + 5) + address + 9);
			}
			address = FindPatternBmh("\xC0\xE8\x07\xA8\x01\x74\x57\x0F\xB7\x4E\x18\x85\xC9\x78\x4F", "xxxxxxxxxxxxxxx");
			if (address != null)
			{
				colliderCapacityOffset = *(int*)(address - 0x41);
				colliderCountOffset = colliderCapacityOffset + 4;
			}

			address = FindPatternBmh("\x7E\x63\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20", "xxxxxxxxxxxx");
			if (address != null)
			{
				InteriorProxyPtrFromGameplayCamAddress = (ulong*)(*(int*)(address + 37) + address + 41);
				InteriorInstPtrInInteriorProxyOffset = (int)*(byte*)(address + 49);
			}

			// These 2 nopping are done by trainers such as Simple Trainer, Menyoo, and Enhanced Native Trainer, but we try to do this if they are not done yet
			#region -- Bypass model requests block for some models --
			// Nopping this enables to spawn some drawable objects without a dedicated collision (e.g. prop_fan_palm_01a)
			address = FindPatternBmh("\x40\x84\x00\x74\x13\xE8\x00\x00\x00\x00\x48\x85\xC0\x75\x09\x38\x45\x57\x0F\x84", "xx?xxx????xxxxxxxxxx");
			if (address != null)
			{
				// Find address to patch because some of the instructions are changed and offset differs between b1290 and b1180
				// Skip the region where there are no "lea rcx, [rbp+6F]"
				address = FindPatternNaive("\x33\xC1\x48\x8D\x4D\x6F", "xxxxxx", new IntPtr(address + 0x3A), 0x30);
				address = address != null ? (address + 0x16) : null;
				if (address != null && *address != 0x90)
				{
					const int bytesToWriteInstructions = 0x18;
					var nopBytes = Enumerable.Repeat((byte)0x90, bytesToWriteInstructions).ToArray();
					Marshal.Copy(nopBytes, 0, new IntPtr(address), bytesToWriteInstructions);
				}
			}
			#endregion
			#region -- Bypass is player model allowed to spawn checks --
			address = FindPatternBmh("\x74\x12\x48\x8B\x10\x48\x8B\xC8\xFF\x52\x30\x84\xC0\x74\x05\x48\x8B\xC3", "xxxxxxxxxxxxxxxxxx");
			address = address != null ? (address + 11) : null;
			if (address != null && *address != 0x90)
			{
				const int bytesToWriteInstructions = 4;
				var nopBytes = Enumerable.Repeat((byte)0x90, bytesToWriteInstructions).ToArray();
				Marshal.Copy(nopBytes, 0, new IntPtr(address), bytesToWriteInstructions);
			}
			#endregion

			// Generate vehicle model list
			var vehicleHashesGroupedByClass = new List<int>[0x20];
			for (var i = 0; i < 0x20; i++)
				vehicleHashesGroupedByClass[i] = new List<int>();

			var vehicleHashesGroupedByType = new List<int>[0x10];
			for (var i = 0; i < 0x10; i++)
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

			for (var i = 0; i < modelHashEntries; i++)
			{
				for (var cur = ((HashNode**)modelHashTable)[i]; cur != null; cur = cur->next)
				{
					var data = cur->data;
					var bitTest = ((*(int*)(modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
					if (data >= modelNum1 || !bitTest) continue;

					var addr1 = modelNum4 + modelNum3 * data;
					if (addr1 == 0) continue;

					var addr2 = *(ulong*)(addr1);
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
								var vehicleTypeInt = *(int*)((byte*)addr2 + VehicleTypeOffsetInModelInfo);
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

			var vehicleResult = new ReadOnlyCollection<int>[0x20];
			for (var i = 0; i < 0x20; i++)
				vehicleResult[i] = Array.AsReadOnly(vehicleHashesGroupedByClass[i].ToArray());
			VehicleModels = Array.AsReadOnly(vehicleResult);

			vehicleResult = new ReadOnlyCollection<int>[0x10];
			for (var i = 0; i < 0x10; i++)
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

			address = FindPatternBmh("\x48\x03\x15\x00\x00\x00\x00\x4C\x23\xC2\x49\x8B\x08", "xxx????xxxxxx");
			var yscScriptTable = (YscScriptTable*)(address + *(int*)(address + 3) + 7);

			// find the shop_controller script
			var shopControllerItem = yscScriptTable->FindScript(0x39DA738B);

			if (shopControllerItem == null || !shopControllerItem->IsLoaded())
			{
				return;
			}

			var shopControllerHeader = shopControllerItem->header;

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

			var codepageCount = shopControllerHeader->CodePageCount();
			for (var i = 0; i < codepageCount; i++)
			{
				var size = shopControllerHeader->GetCodePageSize(i);
				if (size <= 0) continue;

				address = FindPatternNaive(enableCarsGlobalPattern, enableCarsGlobalMask, shopControllerHeader->GetCodePageAddress(i), (ulong)size);
				if (address == null) continue;

				var globalIndex = *(int*)(address + enableCarsGlobalOffset) & 0xFFFFFF;
				*(int*)GetGlobalPtr(globalIndex).ToPointer() = 1;
				break;
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
		public static short ReadInt16(IntPtr address)
		{
			return *(short*)address.ToPointer();
		}
		/// <summary>
		/// Reads an unsigned single 16-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static ushort ReadUInt16(IntPtr address)
		{
			return *(ushort*)address.ToPointer();
		}
		/// <summary>
		/// Reads a single 32-bit value from the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <returns>The value at the address.</returns>
		public static int ReadInt32(IntPtr address)
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
		public static string ReadString(IntPtr address)
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
		public static FVector3 ReadVector3(IntPtr address)
		{
			return *(FVector3*)address.ToPointer();
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
		public static void WriteInt16(IntPtr address, short value)
		{
			var data = (short*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes an unsigned single 16-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteUInt16(IntPtr address, ushort value)
		{
			var data = (ushort*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 32-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt32(IntPtr address, int value)
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
			for (var i = 0; i < value.Length; i++)
				data[i] = value[i];
		}
		/// <summary>
		/// Writes a 3-component floating-point to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The vector components to write.</param>
		public static void WriteVector3(IntPtr address, FVector3 value)
		{
			*(FVector3*)address.ToPointer() = value;
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
		/// Sets or clears a single bit in the 32-bit value at the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="bit">The bit index to change.</param>
		/// <param name="value">Whether to set or clear the bit.</param>
		public static void SetBit(IntPtr address, int bit, bool value = true)
		{
			if (bit < 0 || bit > 31)
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");

			var data = (int*)address.ToPointer();
			if (value)
				*data |= (1 << bit);
			else
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

		static byte[] _strBufferForStringToCoTaskMemUTF8 = new byte[100];
		public static IntPtr String { get; private set; } // "~a~"
		public static IntPtr NullString { get; private set; } // ""
		public static IntPtr CellEmailBcon { get; private set; } // "~a~~a~~a~~a~~a~~a~~a~~a~~a~~a~"

		public static string PtrToStringUTF8(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return string.Empty;

			var data = (byte*)ptr.ToPointer();

			// Calculate length of null-terminated string
			var len = 0;
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

			var byteCountUtf8 = Encoding.UTF8.GetByteCount(s);
			if (byteCountUtf8 > _strBufferForStringToCoTaskMemUTF8.Length)
			{
				_strBufferForStringToCoTaskMemUTF8 = new byte[byteCountUtf8 * 2];
			}

			Encoding.UTF8.GetBytes(s, 0, s.Length, _strBufferForStringToCoTaskMemUTF8, 0);
			var dest = AllocCoTaskMem(byteCountUtf8 + 1);
			if (dest == IntPtr.Zero)
				throw new OutOfMemoryException();

			Copy(_strBufferForStringToCoTaskMemUTF8, 0, dest, byteCountUtf8);
			// Add null-terminator to end
			((byte*)dest.ToPointer())[byteCountUtf8] = 0;

			return dest;
		}

		// To avoid unnessesary GC pressure for creating temp managed arrays when you pass methods vector 3 values to method in NativeMemory that take ones.
		[StructLayout(LayoutKind.Explicit, Size = 0xC)]
		public struct FVector3
		{
			[FieldOffset(0x0)]
			public float X;
			[FieldOffset(0x4)]
			public float Y;
			[FieldOffset(0x8)]
			public float Z;

			public FVector3(float x, float y, float z)
			{
				X = x;
				Y = y;
				Z = z;
			}
		}

		private static float DistanceToSquared(float x1, float y1, float z1, float x2, float y2, float z2)
		{
			float deltaX = x1 - x2;
			float deltaY = y1 - y2;
			float deltaZ = z1 - z2;

			return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
		}

		#region -- Cameras --

		static ulong* CameraPoolAddress;
		static ulong* GameplayCameraAddress;

		public static IntPtr GetCameraAddress(int handle)
		{
			var index = (uint)(handle >> 8);
			var poolAddr = *CameraPoolAddress;
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

		// Performs ASCII uppercase to ASCII lowercase and backslash to slash conversion, does not perform any convertions to non-ASCII characters.
		// Use this table because character conversion with this table performs faster than calculating converted characters using branch jump instructions.
		// The former method is used in GTA5.exe and the latter one is used in GTAIV.exe.
		static readonly byte[] LookupTableForGetHashKey =
		{
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A,
			0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
			0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B,
			0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36,
			0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x61,
			0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C,
			0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77,
			0x78, 0x79, 0x7A, 0x5B, 0x2F, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62,
			0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D,
			0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
			0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83,
			0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E,
			0x8F, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99,
			0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4,
			0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF,
			0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA,
			0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5,
			0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF, 0xD0,
			0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB,
			0xDC, 0xDD, 0xDE, 0xDF, 0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6,
			0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED, 0xEE, 0xEF, 0xF0, 0xF1,
			0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC,
			0xFD, 0xFE, 0xFF
		};

		public static uint GetHashKey(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}

			uint hash = 0;
			foreach (var c in Encoding.UTF8.GetBytes(str))
			{
				hash += LookupTableForGetHashKey[c];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}
		public static uint GetHashKeyASCII(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}

			uint hash = 0;
			foreach (var c in Encoding.ASCII.GetBytes(str))
			{
				hash += LookupTableForGetHashKey[c];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}
		// You can find the equivalent function of the method below with "EB 15 0F BE C0 48 FF C1"
		public static uint GetHashKeyASCIINoPreConversion(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}

			uint hash = 0;
			foreach (var c in Encoding.ASCII.GetBytes(str))
			{
				hash += c;
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}

		static ulong GetLabelTextByHashAddress;
		static delegate* unmanaged[Stdcall]<ulong, int, ulong> GetLabelTextByHashFunc;

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
					return null; //table initialisation hasn't happened yet
				}
				for (var i = 0; i < count; i++)
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

		public static int CursorSprite => *cursorSpriteAddr;

		static float* timeScaleAddress;

		public static float TimeScale => *timeScaleAddress;

		static int* millisecondsPerGameMinuteAddress;

		public static int MillisecondsPerGameMinute
		{
			set => *millisecondsPerGameMinuteAddress = value;
		}

		static bool* isClockPausedAddress;

		public static bool IsClockPaused => *isClockPausedAddress;

		static float* readWorldGravityAddress;
		static float* writeWorldGravityAddress;

		public static float WorldGravity
		{
			get => *readWorldGravityAddress;
			set => *writeWorldGravityAddress = value;
		}

		#endregion

		#region -- Skeleton Data --

		static CrSkeleton* GetCrSkeletonFromEntityHandle(int handle)
		{
			var entityAddress = GetEntityAddress(handle);
			if (entityAddress == IntPtr.Zero)
			{
				return null;
			}

			return GetCrSkeletonOfEntity(entityAddress);
		}
		static CrSkeleton* GetCrSkeletonOfEntity(IntPtr entityAddress)
		{
			var fragInst = GetFragInstAddressOfEntity(entityAddress);
			// Return value will not be null if the entity is a CVehicle or a CPed
			if (fragInst != null)
			{
				return GetEntityCrSkeletonOfFragInst(fragInst);
			}

			var unkAddr = *(ulong*)(entityAddress + 80);
			if (unkAddr == 0)
			{
				return null;
			}

			return (CrSkeleton*)*(ulong*)(unkAddr + 40);
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

		public static int GetBoneIdForEntityBoneIndex(int entityHandle, int boneIndex)
		{
			if (boneIndex < 0)
				return -1;

			var crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
				return -1;

			return crSkeleton->skeletonData->GetBoneIdByIndex(boneIndex);
		}
		public static void GetNextSiblingBoneIndexAndIdOfEntityBoneIndex(int entityHandle, int boneIndex, out int nextSiblingBoneIndex, out int nextSiblingBoneTag)
		{
			if (boneIndex < 0)
			{
				nextSiblingBoneIndex = -1;
				nextSiblingBoneTag = -1;
				return;
			}

			var crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
			{
				nextSiblingBoneIndex = -1;
				nextSiblingBoneTag = -1;
				return;
			}

			crSkeleton->skeletonData->GetNextSiblingBoneIndexAndId(boneIndex, out nextSiblingBoneIndex, out nextSiblingBoneTag);
		}
		public static void GetParentBoneIndexAndIdOfEntityBoneIndex(int entityHandle, int boneIndex, out int parentBoneIndex, out int parentBoneTag)
		{
			if (boneIndex < 0)
			{
				parentBoneIndex = -1;
				parentBoneTag = -1;
				return;
			}

			var crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
			{
				parentBoneIndex = -1;
				parentBoneTag = -1;
				return;
			}

			crSkeleton->skeletonData->GetParentBoneIndexAndId(boneIndex, out parentBoneIndex, out parentBoneTag);
		}
		public static string GetEntityBoneName(int entityHandle, int boneIndex)
		{
			if (boneIndex < 0)
				return null;

			var crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
				return null;

			return crSkeleton->skeletonData->GetBoneName(boneIndex);
		}
		public static int GetEntityBoneCount(int handle)
		{
			var crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			return crSkeleton != null ? crSkeleton->boneCount : 0;
		}
		public static IntPtr GetEntityBoneTransformMatrixAddress(int handle)
		{
			var crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
				return IntPtr.Zero;

			return crSkeleton->GetTransformMatrixAddress();
		}
		public static IntPtr GetEntityBoneObjectMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
				return IntPtr.Zero;

			if (boneIndex < crSkeleton->boneCount)
			{
				return crSkeleton->GetBoneObjectMatrixAddress((boneIndex));
			}

			return IntPtr.Zero;
		}
		public static IntPtr GetEntityBoneGlobalMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
				return IntPtr.Zero;

			var crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
				return IntPtr.Zero;

			if (boneIndex < crSkeleton->boneCount)
			{
				return crSkeleton->GetBoneGlobalMatrixAddress(boneIndex);
			}

			return IntPtr.Zero;
		}

		#endregion

		#region -- CEntity Functions --

		static delegate* unmanaged[Stdcall]<float*, ulong, int, float*> GetRotationFromMatrixFunc;
		static delegate* unmanaged[Stdcall]<float*, ulong, int> GetQuaternionFromMatrixFunc;

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

		internal class SetEntityAngularVelocityTask : IScriptTask
		{
			#region Fields
			IntPtr entityAddress;
			// return value will be the address of the temporary 4 float storage
			delegate* unmanaged[Stdcall]<IntPtr, float*, void> setAngularVelocityDelegate;
			float x, y, z;
			#endregion

			internal SetEntityAngularVelocityTask(IntPtr entityAddress, delegate* unmanaged[Stdcall]<IntPtr, float*, void> vFuncDelegate, float x, float y, float z)
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
			var getEntityAngularVelocity = (delegate* unmanaged[Stdcall]<IntPtr, float*>)(vFuncAddr);

			return getEntityAngularVelocity(entityAddress);
		}

		public static void SetEntityAngularVelocity(IntPtr entityAddress, float x, float y, float z)
		{
			var vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)SetAngularVelocityVFuncOfEntityOffset);
			var setEntityAngularVelocityDelegate = (delegate* unmanaged[Stdcall]<IntPtr, float*, void>)(vFuncAddr);

			var task = new SetEntityAngularVelocityTask(entityAddress, setEntityAngularVelocityDelegate, x, y, z);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
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

		[StructLayout(LayoutKind.Sequential)]
		public struct EntityDamageRecordForReturnValue
		{
			public int attackerEntityHandle;
			public int weaponHash;
			public int gameTime;

			public EntityDamageRecordForReturnValue(int attackerEntityHandle, int weaponHash, int gameTime)
			{
				this.attackerEntityHandle = attackerEntityHandle;
				this.weaponHash = weaponHash;
				this.gameTime = gameTime;
			}
		}

		public static bool IsIndexOfEntityDamageRecordValid(IntPtr entityAddress, uint index)
		{
			if (cAttackerArrayOfEntityOffset == 0 ||
				elementCountOfCAttackerArrayOfEntityOffset == 0 ||
				elementSizeOfCAttackerArrayOfEntity == 0)
				return false;

			var entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return false;

			var entryCount = *(int*)(entityCAttackerArrayAddress + elementCountOfCAttackerArrayOfEntityOffset);

			return index < entryCount;
		}
		static EntityDamageRecordForReturnValue GetEntityDamageRecordEntryAtIndexInternal(ulong cAttackerArrayAddress, uint index)
		{
			var cAttacker = (CAttacker*)(cAttackerArrayAddress + index * elementSizeOfCAttackerArrayOfEntity);

			var attackerEntityAddress = cAttacker->attackerEntityAddress;
			var weaponHash = cAttacker->weaponHash;
			var gameTime = cAttacker->gameTime;
			var attackerHandle = attackerEntityAddress != 0 ? GetEntityHandleFromAddress(new IntPtr((long)attackerEntityAddress)) : 0;

			return new EntityDamageRecordForReturnValue(attackerHandle, weaponHash, gameTime);
		}
		public static EntityDamageRecordForReturnValue GetEntityDamageRecordEntryAtIndex(IntPtr entityAddress, uint index)
		{
			var entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return default(EntityDamageRecordForReturnValue);

			return GetEntityDamageRecordEntryAtIndexInternal(entityCAttackerArrayAddress, index);
		}

		public static EntityDamageRecordForReturnValue[] GetEntityDamageRecordEntries(IntPtr entityAddress)
		{
			if (cAttackerArrayOfEntityOffset == 0 ||
				elementCountOfCAttackerArrayOfEntityOffset == 0 ||
				elementSizeOfCAttackerArrayOfEntity == 0)
				return Array.Empty<EntityDamageRecordForReturnValue>();

			var entityCAttackerArrayAddress = *(ulong*)(entityAddress + (int)cAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
				return Array.Empty<EntityDamageRecordForReturnValue>();

			var returnEntrySize = *(int*)(entityCAttackerArrayAddress + elementCountOfCAttackerArrayOfEntityOffset);
			var returnEntries = returnEntrySize != 0 ? new EntityDamageRecordForReturnValue[returnEntrySize] : Array.Empty<EntityDamageRecordForReturnValue>();

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
			var isPedInVehicle = ((bitFlags & (1 << 0x1E)) != 0);
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

		public static int CVehicleEngineOffset { get; }
		public static int CVehicleEngineTurboOffset { get; }

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

		public static int CanUseSirenOffset { get; }
		public static int HasMutedSirensOffset { get; }
		public static int HasMutedSirensBit { get; }

		public static int VehicleDropsMoneyWhenBlownUpOffset { get; }

		public static int HeliBladesSpeedOffset { get; }

		public static int HeliMainRotorHealthOffset { get; }
		public static int HeliTailRotorHealthOffset { get; }
		public static int HeliTailBoomHealthOffset { get; }

		public static int HandlingDataOffset { get; }

		public static int SubHandlingDataArrayOffset { get; }

		public static int FirstVehicleFlagsOffset { get; }

		public static IntPtr GetSubHandlingData(IntPtr handlingDataAddr, int handlingType)
		{
			var subHandlingArray = (RageAtArrayPtr*)(handlingDataAddr + SubHandlingDataArrayOffset);
			var subHandlingCount = subHandlingArray->size;
			if (subHandlingCount <= 0)
			{
				return IntPtr.Zero;
			}

			for (var i = 0; i < subHandlingCount; i++)
			{
				var subHandlingDataAddr = subHandlingArray->GetElementAddress(i);
				if (subHandlingDataAddr == 0)
				{
					continue;
				}

				var vFuncAddr = *(ulong*)(*(ulong*)subHandlingDataAddr + (uint)0x10);
				var getSubHandlingDataVFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(vFuncAddr);
				var handlingTypeOfCurrentElement = getSubHandlingDataVFunc(subHandlingDataAddr);
				if (handlingTypeOfCurrentElement == handlingType)
				{
					return new IntPtr((long)subHandlingDataAddr);
				}
			}

			return IntPtr.Zero;
		}

		public static float GetVehicleTurbo(int vehicleHandle)
		{
			if (CVehicleEngineTurboOffset == 0)
			{
				return 0f;
			}

			var vehEngineStructAddr = GetCVehicleEngine(vehicleHandle);

			if (vehEngineStructAddr == null)
			{
				return 0f;
			}

			return *(float*)(vehEngineStructAddr + CVehicleEngineTurboOffset);
		}
		public static void SetVehicleTurbo(int vehicleHandle, float value)
		{
			if (CVehicleEngineTurboOffset == 0)
			{
				return;
			}

			var vehEngineStructAddr = GetCVehicleEngine(vehicleHandle);

			if (vehEngineStructAddr == null)
			{
				return;
			}

			*(float*)(vehEngineStructAddr + CVehicleEngineTurboOffset) = value;
		}

		private static byte* GetCVehicleEngine(int vehicleHandle)
		{
			var address = GetEntityAddress(vehicleHandle);

			if (address == IntPtr.Zero)
			{
				return null;
			}

			return (byte*)(address + CVehicleEngineOffset);
		}

		public static int SpecialFlightTargetRatioOffset { get; }
		public static int SpecialFlightWingRatioOffset { get; }
		public static int SpecialFlightCurrentRatioOffset { get; }
		public static int SpecialFlightAreWingsDisabledOffset { get; }

		public static bool HasMutedSirens(int vehicleHandle)
		{
			var address = GetEntityAddress(vehicleHandle);

			if (address == IntPtr.Zero)
			{
				return false;
			}

			return (*(byte*)(address + HasMutedSirensOffset) & HasMutedSirensBit) != 0;
		}

		#endregion

		#region -- Prop Data --

		static int objParentEntityAddressDetachedFromOffset;

		static IntPtr GetParentEntityOfPropDetachedFrom(int objHandle)
		{
			var entityAddress = GetEntityAddress(objHandle);
			if (objParentEntityAddressDetachedFromOffset == 0 || entityAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr(*(long*)(entityAddress + objParentEntityAddressDetachedFromOffset));
		}
		public static int GetParentEntityHandleOfPropDetachedFrom(int objHandle)
		{
			var parentEntityAddr = GetParentEntityOfPropDetachedFrom(objHandle);
			if (parentEntityAddr == IntPtr.Zero)
			{
				return 0;
			}

			return GetEntityHandleFromAddress(parentEntityAddr);
		}
		public static bool HasPropBeenDetachedFromParentEntity(int objHandle) => GetParentEntityOfPropDetachedFrom(objHandle) != IntPtr.Zero;

		#endregion -- Prop Data --

		#region -- Vehicle Wheel Data --


		static delegate* unmanaged[Stdcall]<IntPtr, void> FixVehicleWheelFunc;
		static delegate* unmanaged[Stdcall]<IntPtr, ulong, float, ulong, ulong, int, byte, bool, void> PunctureVehicleTireNewFunc;
		static delegate* unmanaged[Stdcall]<IntPtr, ulong, float, IntPtr, ulong, ulong, int, byte, bool, void> PunctureVehicleTireOldFunc;
		static delegate* unmanaged[Stdcall]<IntPtr, void> BurstVehicleTireOnRimNewFunc;
		static delegate* unmanaged[Stdcall]<IntPtr, IntPtr, void> BurstVehicleTireOnRimOldFunc;

		public static int VehicleWheelSteeringLimitMultiplierOffset { get; }

		public static int VehicleWheelSuspensionStrengthOffset { get; }

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

		static bool VehicleWheelHasVehiclePtr() => GetGameVersion() >= 40;

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

		/// <summary>
		/// The value at this offset should be 2 if the ped is a player ped.
		/// </summary>
		public static int UnkCPedStateOffset { get; }

		public static int CPedIntentoryOfCPedOffset { get; }

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

		public static int PedIntelligenceDecisionMakerHashOffset { get; }

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

		#region -- CPedInventory Data --

		public static IntPtr GetCPedInventoryAddressFromPedHandle(int pedHandle)
		{
			if (CPedIntentoryOfCPedOffset == 0)
			{
				return IntPtr.Zero;
			}

			var cPedAddress = GetEntityAddress(pedHandle);
			if (cPedAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr(*(long**)(cPedAddress + CPedIntentoryOfCPedOffset));
		}

		public static uint[] GetAllWeaponHashesOfPedInventory(int pedHandle)
		{
			var weaponInventoryArray = GetWeaponInventoryArrayOfCPedInventory(pedHandle);
			if (weaponInventoryArray == null)
			{
				return Array.Empty<uint>();
			}

			var weaponInventoryCount = weaponInventoryArray->size;
			var result = new List<uint>(weaponInventoryCount);
			for (int i = 0; i < weaponInventoryCount; i++)
			{
				var itemAddress = weaponInventoryArray->GetElementAddress(i);
				var weaponInfo = *(ItemInfo**)(itemAddress + 0x8);
				if (weaponInfo != null)
				{
					result.Add(weaponInfo->nameHash);
				}
			}

			return result.ToArray();
		}

		public static bool TryGetWeaponHashInPedInventoryBySlotHash(int pedHandle, uint slotHash, out uint weaponHash)
		{
			var weaponInventoryArray = GetWeaponInventoryArrayOfCPedInventory(pedHandle);
			if (weaponInventoryArray == null)
			{
				weaponHash = 0;
				return false;
			}

			int low = 0, high = weaponInventoryArray->size - 1;
			while (true)
			{
				unsafe
				{
					int indexToRead = (low + high) >> 1;
					var currentItem = weaponInventoryArray->GetElementAddress(indexToRead);

					var slotHashOfCurrentItem = *(uint*)(currentItem);
					var weaponInfo = *(ItemInfo**)(currentItem + 0x8);
					if (slotHashOfCurrentItem == slotHash && weaponInfo != null)
					{
						weaponHash = weaponInfo->nameHash;
						return true;
					}

					// The array is sorted in ascending order
					if (slotHashOfCurrentItem <= slotHash)
						low = indexToRead + 1;
					else
						high = indexToRead - 1;

					if (low > high)
					{
						weaponHash = 0;
						return false;
					}
				}
			}
		}

		private static RageAtArrayPtr* GetWeaponInventoryArrayOfCPedInventory(int pedHandle)
		{
			if (CPedIntentoryOfCPedOffset == 0)
			{
				return null;
			}

			var cPedAddress = GetEntityAddress(pedHandle);
			if (cPedAddress == IntPtr.Zero)
			{
				return null;
			}

			var cPedInventoryAddress = *(ulong*)(cPedAddress + CPedIntentoryOfCPedOffset);
			if (cPedInventoryAddress == 0)
			{
				return null;
			}

			return (RageAtArrayPtr*)(cPedInventoryAddress + 0x18);
		}

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
			for (var cur = ((HashNode**)modelHashTable)[(uint)(modelHash) % modelHashEntries]; cur != null; cur = cur->next)
			{
				if (cur->hash != modelHash)
					continue;

				var data = cur->data;
				var bitTest = ((*(int*)(modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
				if (data >= modelNum1 || !bitTest) continue;
				var addr1 = modelNum4 + modelNum3 * data;
				if (addr1 == 0) continue;
				var address = (long*)(*(ulong*)(addr1));
				return new IntPtr(address);
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
			if (GetModelInfoClass(modelInfoAddress) != ModelInfoClassType.Vehicle) return VehicleStructClassType.None;
			var typeInt = (*(int*)((byte*)modelInfoAddress.ToPointer() + VehicleTypeOffsetInModelInfo));

			// Normalize the value to vehicle type range for b944 or later versions if current game version is earlier than b944.
			// The values for CAmphibiousAutomobile and CAmphibiousQuadBike were inserted between those for CSubmarineCar and CHeli in b944.
			if (GetGameVersion() < 28 && typeInt >= 6)
				typeInt += 2;

			return (VehicleStructClassType)typeInt;

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
			if (entityAddress == IntPtr.Zero) return 0;
			var modelInfoAddress = GetModelInfo(entityAddress);
			if (modelInfoAddress != IntPtr.Zero)
			{
				return GetModelHashFromFwArcheType(modelInfoAddress);
			}

			return 0;
		}
		static bool IsFwArcheTypeAFragment(IntPtr fwArcheTypeAddress)
		{
			if (fwArcheTypeAddress != IntPtr.Zero)
			{
				// The game can't draw fragment entities properly if this value is not 1
				return (*(byte*)((ulong)fwArcheTypeAddress.ToInt64() + 0x60) == 1);
			}

			return false;
		}
		public static bool IsModelAFragment(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			if (modelInfo == IntPtr.Zero)
				return false;

			return IsFwArcheTypeAFragment(modelInfo);
		}

		static IntPtr GetModelInfoByIndex(uint index)
		{
			if (modelInfoArrayPtr == null || index < 0)
				return IntPtr.Zero;

			var modelInfoArrayFirstElemPtr = *modelInfoArrayPtr;

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
				var indexOfModelInfo = modelSet->modelMemberIndices[i];

				if (indexOfModelInfo == 0xFFFF)
					break;

				resultList.Add(GetModelHashFromFwArcheType(GetModelInfoByIndex(indexOfModelInfo)));
			}

			return resultList;
		}


		public static bool IsModelAPed(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetModelInfoClass(modelInfo) == ModelInfoClassType.Ped;
		}
		public static bool IsModelABlimp(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Blimp;
		}
		public static bool IsModelAMotorcycle(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Bike;
		}
		public static bool IsModelASubmarine(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Submarine;
		}
		public static bool IsModelASubmarineCar(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.SubmarineCar;
		}
		public static bool IsModelATrailer(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetVehicleStructClass(modelInfo) == VehicleStructClassType.Trailer;
		}
		public static bool IsModelAMlo(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			return GetModelInfoClass(modelInfo) == ModelInfoClassType.Mlo;
		}

		public static string GetVehicleMakeName(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);

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

			var modelInfo = FindCModelInfo(modelHash);

			if (GetModelInfoClass(modelInfo) != ModelInfoClassType.Vehicle) return false;
			var modelFlags = *(ulong*)(modelInfo + FirstVehicleFlagsOffset + flagOffset).ToPointer();
			return (modelFlags & flag) != 0;
		}

		public static ReadOnlyCollection<int> WeaponModels { get; }
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModels { get; }
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModelsGroupedByType { get; }
		public static ReadOnlyCollection<int> PedModels { get; }


		static delegate* unmanaged[Stdcall]<IntPtr, ulong> GetHandlingDataByHash;
		static delegate* unmanaged[Stdcall]<int, ulong> GetHandlingDataByIndex;

		public static IntPtr GetHandlingDataByModelHash(int modelHash)
		{
			var modelInfo = FindCModelInfo(modelHash);
			if (GetModelInfoClass(modelInfo) != ModelInfoClassType.Vehicle)
				return IntPtr.Zero;

			var handlingIndex = *(int*)(modelInfo + handlingIndexOffsetInModelInfo).ToPointer();
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
		struct FwScriptGuidPool
		{
			// The max count value should be at least 3072 as long as ScriptHookV is installed.
			// Without ScriptHookV, the default value is hardcoded and may be different between different game versions (the value is 300 in b372 and 700 in b2824).
			// The default value (when running without ScriptHookV) can be found by searching the dumped exe or the game memory with "D7 A8 11 73" (0x7311A8D7).
			[FieldOffset(0x10)]
			internal uint maxCount;
			[FieldOffset(0x14)]
			internal int itemSize;
			[FieldOffset(0x18)]
			internal int firstEmptySlot;
			[FieldOffset(0x1C)]
			internal int emptySlots;
			[FieldOffset(0x20)]
			internal uint itemCount;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal bool IsFull()
			{
				return maxCount - (itemCount & 0x3FFFFFFF) <= 256;
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
				var handleUInt = (uint)handle;
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
					var byteArrayPtr = (byte*)byteArray.ToPointer();
					return (byte)(byteArrayPtr[index] & 0x7F);
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private ulong Mask(uint index)
			{
				unsafe
				{
					var byteArrayPtr = (byte*)byteArray.ToPointer();
					long num1 = byteArrayPtr[index] & 0x80;
					return (ulong)(~((num1 | -num1) >> 63));
				}
			}
		}

		static ulong* FwScriptGuidPoolAddress;
		static ulong* PedPoolAddress;
		static ulong* ObjectPoolAddress;
		static ulong* PickupObjectPoolAddress;
		static ulong* VehiclePoolAddress;
		static ulong* BuildingPoolAddress;
		static ulong* AnimatedBuildingPoolAddress;
		static ulong* InteriorInstPoolAddress;
		static ulong* InteriorProxyPoolAddress;

		static ulong* ProjectilePoolAddress;
		static int* ProjectileCountAddress;

		// if the entity is a ped and they are in a vehicle, the vehicle position will be returned instead (just like GET_ENTITY_COORDS does)
		static delegate* unmanaged[Stdcall]<ulong, float*, ulong> EntityPosFunc;
		// should be rage::fwScriptGuid::CreateGuid
		static delegate* unmanaged[Stdcall]<ulong, int> CreateGuid;

		internal sealed class FwScriptGuidPoolTask : IScriptTask
		{
			internal enum PoolType
			{
				Generic,
				Vehicle,
				Projectile,
			}

			#region Fields
			internal PoolType _poolType;
			internal IntPtr _poolAddress;

			internal bool doPosCheck = false;
			internal bool doModelCheck = false;
			internal float radiusSquared;
			internal FVector3? position;
			internal HashSet<int> modelHashes;
			internal int[] resultHandles = Array.Empty<int>();

			#endregion

			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress)
			{
				_poolType = type;
				_poolAddress = poolAddress;
			}
			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress, int[] modelHashes) : this(type, poolAddress)
			{
				if (modelHashes == null || modelHashes.Length <= 0) return;
				doModelCheck = true;
				this.modelHashes = new HashSet<int>(modelHashes);
			}
			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress, FVector3 position, float radiusSquared, int[] modelHashes = null) : this(type, poolAddress)
			{
				doPosCheck = true;
				this.radiusSquared = radiusSquared;
				this.position = position;

				if (modelHashes == null || modelHashes.Length <= 0) return;
				doModelCheck = true;
				this.modelHashes = new HashSet<int>(modelHashes);
			}

			public void Run()
			{
				if (*NativeMemory.FwScriptGuidPoolAddress == 0)
					return;

				var fwScriptGuidPool = (FwScriptGuidPool*)(*NativeMemory.FwScriptGuidPoolAddress);

				switch (_poolType)
				{
					case PoolType.Generic:
						var genericPool = (GenericPool*)_poolAddress;
						resultHandles = GetGuidHandlesFromGenericPool(fwScriptGuidPool, genericPool);
						break;

					case PoolType.Vehicle:
						var vehiclePool = (VehiclePool*)_poolAddress;
						resultHandles = GetGuidHandlesFromVehiclePool(fwScriptGuidPool, vehiclePool);
						break;

					case PoolType.Projectile:
						var projectilesCount = NativeMemory.GetProjectileCount();
						var projectileCapacity = NativeMemory.GetProjectileCapacity();
						var projectilePoolAddress = (ulong*)_poolAddress;

						resultHandles = GetGuidHandlesFromProjectilePool(fwScriptGuidPool, projectilePoolAddress, projectilesCount, projectileCapacity);
						break;
				}
			}

			int[] GetGuidHandlesFromGenericPool(FwScriptGuidPool* fwScriptGuidPool, GenericPool* genericPool)
			{
				var resultList = new List<int>(genericPool->itemCount);

				var genericPoolSize = genericPool->size;
				for (uint i = 0; i < genericPoolSize; i++)
				{
					if (fwScriptGuidPool->IsFull())
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all entity handles.");

					if (!genericPool->IsValid(i))
					{
						continue;
					}

					var address = genericPool->GetAddress(i);

					if (doPosCheck && !CheckEntityDistance(address, position.GetValueOrDefault(), radiusSquared))
						continue;
					if (doModelCheck && !CheckEntityModel(address, modelHashes))
						continue;

					var createdHandle = NativeMemory.CreateGuid(address);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			int[] GetGuidHandlesFromVehiclePool(FwScriptGuidPool* fwScriptGuidPool, VehiclePool* vehiclePool)
			{
				var resultList = new List<int>((int)vehiclePool->itemCount);

				var poolSize = vehiclePool->size;
				for (uint i = 0; i < poolSize; i++)
				{
					if (fwScriptGuidPool->IsFull())
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all vehicle handles.");

					if (!vehiclePool->IsValid(i))
					{
						continue;
					}

					var address = vehiclePool->GetAddress(i);

					if (doPosCheck && !CheckEntityDistance(address, position.GetValueOrDefault(), radiusSquared))
						continue;
					if (doModelCheck && !CheckEntityModel(address, modelHashes))
						continue;

					var createdHandle = NativeMemory.CreateGuid(address);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			int[] GetGuidHandlesFromProjectilePool(FwScriptGuidPool* fwScriptGuidPool, ulong* projectilePool, int itemCount, int maxItemCount)
			{
				var projectilesLeft = itemCount;
				var projectileCapacity = maxItemCount;

				var resultList = new List<int>(itemCount);

				for (uint i = 0; (projectilesLeft > 0 && i < projectileCapacity); i++)
				{
					if (fwScriptGuidPool->IsFull())
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all projectile handles.");

					var entityAddress = (ulong)ReadAddress(new IntPtr(projectilePool + i)).ToInt64();
					if (entityAddress == 0)
						continue;

					projectilesLeft--;

					if (doPosCheck && !CheckEntityDistance(entityAddress, position.GetValueOrDefault(), radiusSquared))
						continue;
					if (doModelCheck && !CheckEntityModel(entityAddress, modelHashes))
						continue;

					var createdHandle = NativeMemory.CreateGuid(entityAddress);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			static bool CheckEntityDistance(ulong address, FVector3 position, float radiusSquared)
			{
				var entityPosition = stackalloc float[4];

				NativeMemory.EntityPosFunc(address, entityPosition);

				var x = position.X - entityPosition[0];
				var y = position.Y - entityPosition[1];
				var z = position.Z - entityPosition[2];

				var distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
					return false;

				return true;
			}

			static bool CheckEntityModel(ulong address, HashSet<int> modelHashes)
			{
				var modelHash = GetModelHashFromEntity(new IntPtr((long)address));
				if (!modelHashes.Contains(modelHash))
					return false;

				return true;
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
				returnEntityHandle = NativeMemory.CreateGuid(entityAddress);
			}
		}

		public static int GetVehicleCount()
		{
			if (*VehiclePoolAddress == 0) return 0;
			var pool = *(VehiclePool**)(*VehiclePoolAddress);
			return (int)pool->itemCount;
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
			var pool = (GenericPool*)(address);
			return (int)pool->itemCount;
		}

		public static int GetVehicleCapacity()
		{
			if (*VehiclePoolAddress == 0) return 0;
			var pool = *(VehiclePool**)(*VehiclePoolAddress);
			return (int)pool->size;
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
			var pool = (GenericPool*)(address);
			return (int)pool->size;
		}

		public static int[] GetPedHandles(int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.PedPoolAddress, modelHashes);
		}
		public static int[] GetPedHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.PedPoolAddress, position, radius, modelHashes);
		}

		public static int[] GetPropHandles(int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.ObjectPoolAddress, modelHashes);
		}
		public static int[] GetPropHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.ObjectPoolAddress, position, radius, modelHashes);
		}

		public static int[] GetEntityHandles()
		{
			var vehicleHandles = GetVehicleHandles();
			var pedHandles = GetPedHandles();
			var propHandles = GetPropHandles();

			return BuildOneArrayFromElementsOfEntityHandleArrays(vehicleHandles, pedHandles, propHandles);
		}
		public static int[] GetEntityHandles(FVector3 position, float radius)
		{
			var vehicleHandles = GetVehicleHandles(position, radius);
			var pedHandles = GetPedHandles(position, radius);
			var propHandles = GetPropHandles(position, radius);

			return BuildOneArrayFromElementsOfEntityHandleArrays(vehicleHandles, pedHandles, propHandles);
		}

		private static int[] BuildOneArrayFromElementsOfEntityHandleArrays(int[] vehicleHandles, int[] pedHandles, int[] propHandles)
		{
			var entityHandleCount = vehicleHandles.Length + pedHandles.Length + propHandles.Length;
			var entityHandles = new int[entityHandleCount];

			Array.Copy(vehicleHandles, 0, entityHandles, 0, vehicleHandles.Length);
			Array.Copy(pedHandles, 0, entityHandles, vehicleHandles.Length, pedHandles.Length);
			Array.Copy(propHandles, 0, entityHandles, vehicleHandles.Length + pedHandles.Length, propHandles.Length);

			return entityHandles;
		}

		public static int[] GetVehicleHandles(int[] modelHashes = null)
		{
			if (*NativeMemory.VehiclePoolAddress == 0)
				return Array.Empty<int>();

			var vehiclePool = new IntPtr(*(VehiclePool**)(*NativeMemory.VehiclePoolAddress));

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Vehicle, vehiclePool, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}
		public static int[] GetVehicleHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			if (*NativeMemory.VehiclePoolAddress == 0)
				return Array.Empty<int>();

			var vehiclePool = new IntPtr(*(VehiclePool**)(*NativeMemory.VehiclePoolAddress));

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Vehicle, vehiclePool, position, radius * radius, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}

		public static int[] GetPickupObjectHandles()
		{
			return GetGuidsInGenericPool(NativeMemory.PickupObjectPoolAddress);
		}
		public static int[] GetPickupObjectHandles(FVector3 position, float radius)
		{
			return GetGuidsInGenericPool(NativeMemory.PickupObjectPoolAddress, position, radius);
		}
		public static int[] GetProjectileHandles()
		{
			if (NativeMemory.ProjectilePoolAddress == null)
				return Array.Empty<int>();

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Projectile, new IntPtr(NativeMemory.ProjectilePoolAddress));
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}
		public static int[] GetProjectileHandles(FVector3 position, float radius)
		{
			if (NativeMemory.ProjectilePoolAddress == null)
				return Array.Empty<int>();

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Projectile, new IntPtr(NativeMemory.ProjectilePoolAddress), position, radius * radius);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}

		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
				return Array.Empty<int>();

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}
		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr, int[] modelHashes)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
				return Array.Empty<int>();

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}
		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr, FVector3 position, float radius, int[] modelHashes = null)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
				return Array.Empty<int>();

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool, position, radius * radius, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.resultHandles;
		}

		public static int[] GetBuildingHandles()
		{
			if (BuildingPoolAddress == null)
				return Array.Empty<int>();

			return GetHandlesInGenericPool(*NativeMemory.BuildingPoolAddress);
		}

		public static int[] GetBuildingHandles(FVector3 position, float radius)
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

		public static int[] GetAnimatedBuildingHandles(FVector3 position, float radius)
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

		public static int[] GetInteriorInstHandles(FVector3 position, float radius)
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

		public static int[] GetInteriorProxyHandles(FVector3 position, float radius)
		{
			if (InteriorProxyPoolAddress == null)
				return Array.Empty<int>();

			var pool = (GenericPool*)(*NativeMemory.InteriorProxyPoolAddress);

			// CInteriorProxy is not a subclass of CEntity and position data is placed at different offset
			var returnHandles = new List<int>();
			var poolSize = pool->size;
			var radiusSquared = radius * radius;
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
					continue;

				var address = pool->GetAddress(i);

				var x = *(float*)(address + 0x70) - position.X;
				var y = *(float*)(address + 0x74) - position.Y;
				var z = *(float*)(address + 0x78) - position.Z;

				var distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
					continue;

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		public static bool BuildingHandleExists(int handle) => BuildingPoolAddress != null ? ((GenericPool*)(*BuildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool AnimatedBuildingHandleExists(int handle) => AnimatedBuildingPoolAddress != null ? ((GenericPool*)(*AnimatedBuildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorInstHandleExists(int handle) => InteriorInstPoolAddress != null ? ((GenericPool*)(*InteriorInstPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorProxyHandleExists(int handle) => InteriorProxyPoolAddress != null ? ((GenericPool*)(*InteriorProxyPoolAddress))->IsHandleValid(handle) : false;

		static int[] GetHandlesInGenericPool(ulong poolAddress)
		{
			var pool = (GenericPool*)poolAddress;

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

		static int[] GetCEntityHandlesInRange(ulong poolAddress, FVector3 position, float radius)
		{
			var pool = (GenericPool*)poolAddress;

			var returnHandles = new List<int>();
			var poolSize = pool->size;
			var radiusSquared = radius * radius;
			var entityPosition = stackalloc float[4];
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
					continue;

				var address = pool->GetAddress(i);

				NativeMemory.EntityPosFunc(address, entityPosition);
				var x = entityPosition[0] - position.X;
				var y = entityPosition[1] - position.Y;
				var z = entityPosition[2] - position.Z;

				var distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
					continue;

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		static int CalculateAppropriateExtendedArrayLength(int[] array, int targetElementCount)
		{
			return (array.Length * 2 > targetElementCount) ? array.Length * 2 : targetElementCount * 2;
		}

		#endregion

		#region -- CPlayerInfo Data --

		static delegate* unmanaged[Stdcall]<int, ulong> GetPlayerPedAddressFunc;

		static bool* isGameMultiplayerAddr;

		/// <summary>
		/// The offset for max health of CPlayerInfo, which is stored as an uint16_t.
		/// </summary>
		public static int CPlayerInfoMaxHealthOffset { get; }

		public static int PedPlayerInfoOffset { get; }
		public static int CWantedOffset { get; }
		public static int CPlayerPedTargetingOfffset { get; }

		public static int CurrentWantedLevelOffset { get; }
		public static int CurrentCrimeValueOffset { get; }
		public static int PendingCrimeValueOffset { get; }
		public static int TimeToApplyPendingCrimeValueOffset { get; }
		public static int CWantedLastTimeSearchAreaRefocusedOffset { get; }
		public static int CWantedLastTimeSpottedByPoliceOffset { get; }
		public static int CWantedStartTimeOfHiddenEvasionOffset { get; }
		public static int CWantedIgnorePlayerFlagOffset { get; }

		static delegate* unmanaged[Stdcall]<IntPtr, void> ActivateSpecialAbilityFunc;

		// The function is for b2060 or later and static offset is for prior to b2060
		static delegate* unmanaged[Stdcall]<IntPtr, int, IntPtr> GetSpecialAbilityAddressFunc;
		public static int PlayerPedSpecialAbilityOffset { get; }

		public static IntPtr GetPlayerPedAddress(int playerIndex)
		{
			return new IntPtr((long)GetPlayerPedAddressFunc(playerIndex));
		}
		public static IntPtr GetLocalPlayerPedAddress()
		{
			return new IntPtr((long)GetLocalPlayerPedAddressFunc());
		}
		public static int GetPlayerPedHandle(int handle)
		{
			var playerPedAddress = GetPlayerPedAddress(handle);
			return playerPedAddress != IntPtr.Zero ? GetEntityHandleFromAddress(playerPedAddress) : 0;
		}
		public static int GetLocalPlayerPedHandle()
		{
			var localPlayerPedAddress = GetLocalPlayerPedAddress();
			return localPlayerPedAddress != IntPtr.Zero ? GetEntityHandleFromAddress(localPlayerPedAddress) : 0;
		}
		public static int GetLocalPlayerIndex()
		{
			if (isGameMultiplayerAddr == null || *isGameMultiplayerAddr)
			{
				// A fallback path if the variable could not found to make sure the same value will be returned as what PLAYER_ID returns, an extreme edge case if the variable was found
				// You even have to disable SHV to call NETWORK_GET_NUM_CONNECTED_PLAYERS (for preventing the game from going Online) before custom scripts (for enabling multiplayer) can use features for multiplayer
				return GetLocalPlayerIndexViaNativeCall();
			}

			// The same value as what PLAYER_ID returns if the game mode is singleplayer and not multiplayer
			return 0;

			static int GetLocalPlayerIndexViaNativeCall()
			{
				var resultAddr = NativeFunc.Invoke(0x4F8644AF03D0E0D6 /* PLAYER_ID */, null, 0);
				if (resultAddr == null)
				{
					throw new InvalidOperationException("Game.Player can only be called from the main thread.");
				}

				return *(int*)resultAddr;
			}
		}

		public static IntPtr GetCPlayerInfoAddress(int playerIndex)
		{
			if (PedPlayerInfoOffset == 0)
			{
				return IntPtr.Zero;
			}

			var playerPedAddr = GetPlayerPedAddress(playerIndex);
			if (playerPedAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			var playerInfoAddr = *(long**)((ulong)playerPedAddr + (uint)PedPlayerInfoOffset);
			if (playerPedAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr(playerInfoAddr);
		}
		public static IntPtr GetCPlayerPedTargetingAddress(int playerIndex)
		{
			if (CPlayerPedTargetingOfffset == 0)
			{
				return IntPtr.Zero;
			}

			var cPlayerInfoAddr = GetCPlayerInfoAddress(playerIndex);
			if (cPlayerInfoAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr((long)((ulong)cPlayerInfoAddr + (uint)CPlayerPedTargetingOfffset));
		}
		public static IntPtr GetCWantedAddress(int playerIndex)
		{
			if (CWantedOffset == 0)
			{
				return IntPtr.Zero;
			}

			var cPlayerInfoAddr = GetCPlayerInfoAddress(playerIndex);
			if (cPlayerInfoAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr((long)((ulong)cPlayerInfoAddr + (uint)CWantedOffset));
		}

		public static int GetTargetedBuildingHandleOfPlayer(int playerIndex)
		{
			var playerPedTargetingAddr = GetCPlayerPedTargetingAddress(playerIndex);
			if (playerPedTargetingAddr == IntPtr.Zero)
			{
				return 0;
			}

			var targetedCEntityAddr = *(ulong*)(playerPedTargetingAddr + 0x110);
			// Return zero if the targeted CEntity address is null or is not null but not a CBuilding instance
			// Should be a CPhysical address in that case since the value is null if the player is aiming a CAnimatedBuilding instance (e.g. the fan at Ammu-Nation in Little Seoul)
			if (targetedCEntityAddr == 0 || *(byte*)(targetedCEntityAddr + 0x28) != 1)
			{
				return 0;
			}

			return GetBuildingHandleFromAddress(new IntPtr((long)targetedCEntityAddr));
		}

		/// <summary>
		/// Activates the special ability for the player.
		/// </summary>
		/// <remarks>
		/// This function is for v1.0.617.1 and earlier versions, where the native function SPECIAL_ABILITY_ACTIVATE is not present.
		/// </remarks>
		public static void ActivateSpecialAbility(int playerIndex)
		{
			var specialAbilityAddr = GetPrimarySpecialAbilityStructAddress(playerIndex);
			if (specialAbilityAddr == IntPtr.Zero)
			{
				return;
			}

			var task = new ActivateSpecialAbilityTask(specialAbilityAddr);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}
		public static IntPtr GetPrimarySpecialAbilityStructAddress(int playerIndex)
		{
			var playerPedAddress = GetPlayerPedAddress(playerIndex);

			if (playerPedAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			// Two special ability slots are available in b2060 and later versions
			if (GetGameVersion() >= 59)
			{
				if (GetSpecialAbilityAddressFunc == null)
				{
					return IntPtr.Zero;
				}

				return GetSpecialAbilityAddressFunc(playerPedAddress, 0);
			}
			else
			{
				if (PlayerPedSpecialAbilityOffset == 0)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(*(long**)((ulong)playerPedAddress + (uint)PlayerPedSpecialAbilityOffset));
			}
		}

		internal class ActivateSpecialAbilityTask : IScriptTask
		{
			#region Fields
			internal IntPtr specialAbilityStructAddress;
			#endregion

			internal ActivateSpecialAbilityTask(IntPtr specialAbilityStructAddress)
			{
				this.specialAbilityStructAddress = specialAbilityStructAddress;
			}

			public void Run()
			{
				ActivateSpecialAbilityFunc(specialAbilityStructAddress);
			}
		}

		#endregion

		#region -- CPathFind Data --

		public static unsafe class PathFind
		{
			static ulong cPathFindInstanceAddress;

			static PathFind()
			{
				byte* address;

				address = FindPatternBmh("\x4D\x8B\xF0\x45\x8A\xE1\x48\x8B\xF9\x4C\x8D\x05", "xxxxxxxxxxxx");
				if (address != null)
				{
					cPathFindInstanceAddress = (ulong)(*(int*)(address + 12) + address + 16);
				}
			}

			// These values hasn't been changed between b372 and b2845
			const int START_PATH_NODE_OFFSET_OF_C_PATH_FIND = 0x1640;
			const int MAX_C_PATH_REGION_COUNT = 0x400;

			[StructLayout(LayoutKind.Explicit, Size = 0x70)]
			internal struct CPathRegion
			{
				[FieldOffset(0x10)]
				internal IntPtr NodeArrayPtr;
				[FieldOffset(0x18)]
				internal uint NodeCount;
				[FieldOffset(0x1C)]
				internal uint NodeCountVehicle;
				[FieldOffset(0x20)]
				internal uint NodeCountPed;
				[FieldOffset(0x28)]
				internal IntPtr NodeLinkPtr;
				[FieldOffset(0x30)]
				internal uint NodeLinkCount;
				[FieldOffset(0x38)]
				internal IntPtr JunctionPtr;
				[FieldOffset(0x40)]
				internal IntPtr JunctionHeightMapBytesPtr;
				[FieldOffset(0x50)]
				internal IntPtr JunctionRefsPtr;
				[FieldOffset(0x58)]
				internal ushort JunctionRefsCount0;
				[FieldOffset(0x5A)]
				internal ushort JunctionRefsCount1;
				[FieldOffset(0x60)]
				internal uint JunctionCount;
				[FieldOffset(0x64)]
				internal uint JunctionHeightMapBytesCount;

				internal CPathNode* GetPathNode(uint nodeId)
				{
					if (NodeArrayPtr == IntPtr.Zero && nodeId >= NodeCount)
					{
						return null;
					}

					return GetPathNodeUnsafe(nodeId);
				}
				internal CPathNode* GetPathNodeUnsafe(uint nodeId) => (CPathNode*)((ulong)NodeArrayPtr + nodeId * (uint)sizeof(CPathNode));

				internal CPathNodeLink* GetPathNodeLink(uint index)
				{
					if (NodeLinkPtr == IntPtr.Zero && index >= NodeLinkCount)
					{
						return null;
					}

					return GetPathNodeLinkUnsafe(index);
				}
				internal CPathNodeLink* GetPathNodeLinkUnsafe(uint index) => (CPathNodeLink*)((ulong)NodeLinkPtr + index * (uint)sizeof(CPathNodeLink));
			}

			static CPathRegion* GetCPathRegion(uint areaId)
			{
				if (areaId >= MAX_C_PATH_REGION_COUNT || cPathFindInstanceAddress == 0)
				{
					return null;
				}

				return *(CPathRegion**)(cPathFindInstanceAddress + START_PATH_NODE_OFFSET_OF_C_PATH_FIND + areaId * 0x8);
			}

			public enum VehiclePathNodeProperties
			{
				None = 0,
				OffRoad = 1,
				OnPlayersRoad = 2,
				NoBigVehicles = 4,
				SwitchedOff = 8,
				TunnelOrInterior = 16,
				LeadsToDeadEnd = 32,
				/// <summary>
				/// <see cref="Boat"/> takes precedence over this flag.
				/// </summary>
				Highway = 64,
				Junction = 128,
				/// <summary>
				/// Cannot be used with <see cref="GiveWay"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
				/// </summary>
				TrafficLight = 256,
				/// <summary>
				/// Cannot be used with <see cref="TrafficLight"/>, because vehicle nodes can have either traffic-light or give-way feature as a special function but cannot have both of them.
				/// </summary>
				GiveWay = 512,
				/// <summary>
				/// Cannot be used with <see cref="Highway"/>.
				/// </summary>
				Boat = 1024,

				// GET_VEHICLE_NODE_PROPERTIES will not set any of the values below set as flags
				DontAllowGps = 2048,
			}

			[StructLayout(LayoutKind.Explicit, Size = 0x28)]
			internal struct CPathNode
			{
				// 0x0 to 0xF region is unused
				[FieldOffset(0x10)]
				internal ushort AreaId;
				[FieldOffset(0x12)]
				internal ushort NodeId;
				[FieldOffset(0x14)]
				internal uint StreetHash;

				[FieldOffset(0x1A)]
				internal ushort LinkId;

				// These 2 fields should be multiplied by 4 when you convert back to float
				[FieldOffset(0x1C)]
				internal short PositionX;
				[FieldOffset(0x1E)]
				internal short PositionY;

				[FieldOffset(0x20)]
				internal ushort Flags1;

				// This field should be multiplied by 32 when you convert back to float
				[FieldOffset(0x22)]
				internal short PositionZ;

				[FieldOffset(0x24)]
				internal byte Flags2;
				[FieldOffset(0x25)]
				internal byte Flags3AndLinkCount;
				[FieldOffset(0x26)]
				internal byte Flags4;
				// 1st to 4th bits are used for density
				[FieldOffset(0x27)]
				internal byte Flag5AndDensity;

				internal int Density => Flag5AndDensity & 0xF;

				internal int LinkCount => Flags3AndLinkCount >> 3;

				// Native functions for path nodes get area IDs and node IDs from the values subtracted by one from passed values
				// When the lower half of bits (of passed values) are equal to zero, the natives considers the null handle is passed
				internal int GetHandleForNativeFunctions() => ((NodeId << 0x10) + AreaId + 1);

				internal FVector3 UncompressedPosition => new FVector3((float)PositionX / 4, (float)PositionY / 4, (float)PositionZ / 32);

				internal bool IsSwitchedOff
				{
					get => (Flags2 & 0x80) != 0;
					set
					{
						if (value)
						{
							Flags2 |= 0x80;
						}
						else
						{
							Flags2 &= 0x7F;
						}
					}
				}

				/// <summary>
				/// Get property flags in almost the same way as GET_VEHICLE_NODE_PROPERTIES returns flags as the 5th parameter (seems the flags the native returns will never contain the 1024 flag).
				/// </summary>
				internal VehiclePathNodeProperties GetPropertyFlags()
				{
					VehiclePathNodeProperties propertyFlags = VehiclePathNodeProperties.None;
					if ((Flags1 & 8) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.OffRoad;
					}
					if ((Flags1 & 0x10) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.OnPlayersRoad;
					}
					if ((Flags1 & 0x20) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.NoBigVehicles;
					}
					if ((Flags2 & 0x80) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.SwitchedOff;
					}
					if ((Flags4 & 0x1) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.TunnelOrInterior;
					}
					if ((Flag5AndDensity & 0x7) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.LeadsToDeadEnd;
					}
					// The water/boat bit takes precedence over this highway flag
					if (((Flags2 & 0x40) != 0 || (Flags2 & 0x20) == 0))
					{
						propertyFlags |= VehiclePathNodeProperties.Highway;
					}
					if ((((Flags2 & 0x40) >> 8) & 1) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.Junction;
					}
					if ((Flags1 & 0xF800) == 0x7800)
					{
						propertyFlags |= VehiclePathNodeProperties.TrafficLight;
					}
					if ((Flags1 & 0xF800) == 0x8000)
					{
						propertyFlags |= VehiclePathNodeProperties.GiveWay;
					}
					if ((Flags2 & 0x20) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.Boat;
					}

					return propertyFlags;
				}

				internal bool IsInArea(float x1, float y1, float z1, float x2, float y2, float z2)
				{
					float posXUncompressed = (float)PositionX / 4;
					float posYUncompressed = (float)PositionY / 4;
					float posZUncompressed = (float)PositionZ / 32;

					if (posXUncompressed < x1 || posXUncompressed > x2)
					{
						return false;
					}
					if (posYUncompressed < y1 || posYUncompressed > y2)
					{
						return false;
					}
					if (posZUncompressed < z1 || posYUncompressed > z2)
					{
						return false;
					}

					return true;
				}
				internal bool IsInCircle(float x, float y, float z, float radius)
				{
					float posXUncompressed = (float)PositionX / 4;
					float posYUncompressed = (float)PositionY / 4;
					float posZUncompressed = (float)PositionZ / 32;

					float deltaX = (float)x - posXUncompressed;
					float deltaY = (float)y - posYUncompressed;
					float deltaZ = (float)z - posZUncompressed;

					return ((deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ)) <= radius * radius;
				}
			}

			public static IntPtr GetPathNodeAddress(int handle)
			{
				GetCorrectedNodeAndAreaIdFromPathNodeHandle(handle, out uint areaId, out uint nodeId);

				var pathRegion = GetCPathRegion(areaId);
				if (pathRegion == null)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(pathRegion->GetPathNode(nodeId));
			}

			public static IntPtr GetPathNodeLinkAddress(int areaId, int nodeLinkIndex)
			{
				var pathRegion = GetCPathRegion((uint)areaId);
				if (pathRegion == null)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(pathRegion->GetPathNodeLink((uint)nodeLinkIndex));
			}

			static void GetCorrectedNodeAndAreaIdFromPathNodeHandle(int handleForNatives, out uint areaId, out uint nodeId)
			{
				uint handleCorrected = (uint)handleForNatives - 1;
				areaId = (ushort)(handleCorrected & 0xFFFF);
				nodeId = (ushort)(handleCorrected >> 0x10);
			}

			public static FVector3 GetPathNodePosition(int handle)
			{
				var pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return default;
				}

				return ((CPathNode*)pathNode)->UncompressedPosition;
			}

			public static int GetVehiclePathNodeDensity(int handle)
			{
				var pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return 0;
				}

				return ((CPathNode*)pathNode)->Density;
			}

			public static int GetVehiclePathNodePropertyFlags(int handle)
			{
				var pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return 0;
				}

				return (int)((CPathNode*)pathNode)->GetPropertyFlags();
			}

			public static bool GetPathNodeSwitchedOffFlag(int handle)
			{
				var pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return false;
				}

				return ((CPathNode*)pathNode)->IsSwitchedOff;
			}

			public static void SetPathNodeSwitchedOffFlag(int handle, bool toggle)
			{
				var pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return;
				}

				((CPathNode*)pathNode)->IsSwitchedOff = toggle;
			}

			[StructLayout(LayoutKind.Explicit, Size = 0x8)]
			internal struct CPathNodeLink
			{
				[FieldOffset(0x0)]
				internal ushort AreaId;
				[FieldOffset(0x2)]
				internal ushort NodeId;

				[FieldOffset(0x4)]
				internal byte Flags0;
				[FieldOffset(0x5)]
				internal byte Flags1;
				[FieldOffset(0x6)]
				internal byte Flags2;
				[FieldOffset(0x7)]
				internal byte LinkLength;

				internal int ForwardLaneCount => (Flags2 >> 5) & 7;
				internal int BackwardLaneCount => (Flags2 >> 2) & 7;

				internal void GetForwardAndBackwardCount(out int forwardCount, out int backwardCount)
				{
					forwardCount = (Flags2 >> 5) & 7;
					backwardCount = (Flags2 >> 2) & 7;
				}

				internal void GetTargetAreaAndNodeId(out int areaId, out int nodeId)
				{
					areaId = AreaId;
					nodeId = NodeId;
				}
			}

			// Use this buffer when we get all loaded path nodes to avoid allocating new large objects for buffer space and costing a significant time, since the number of path node handles can even exceed more than 21250
			// On the other hand, each vanilla ynd file (each ynd file has nodes in an area of 512 meters x 512 meters) contains likely 100 to 1500 nodes, and getting nodes nearby 512 meters will likely get less than 1500 nodes.
			// Therefore, using this buffer won't make much difference in how many CPU cycles will be used when we get nodes in certain area
			static List<int> _pathNodeBuffer = new List<int>();

			public static int[] GetAllLoadedVehicleNodes(Func<int, bool> predicateForFlags)
			{
				_pathNodeBuffer.Clear();

				for (uint i = 0; i < MAX_C_PATH_REGION_COUNT; i++)
				{
					var pathRegion = GetCPathRegion(i);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					var vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						var pathNode = pathRegion->GetPathNodeUnsafe(j);
						if (predicateForFlags == null || predicateForFlags((int)pathNode->GetPropertyFlags()))
						{
							_pathNodeBuffer.Add(pathNode->GetHandleForNativeFunctions());
						}
					}
				}

				return _pathNodeBuffer.ToArray();
			}

			public static int[] GetLoadedVehicleNodesInRange(float x, float y, float z, float radius, Func<int, bool> predicateForFlags)
			{
				var result = new List<int>();

				foreach (var areaId in GetAreaIdsInRange(x, y, radius))
				{
					var pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					var vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						var vehPathNode = pathRegion->GetPathNodeUnsafe(j);
						if (!CheckVehPathNodePropertyPredicateAndPosition(vehPathNode, predicateForFlags, x, y, z, radius))
						{
							continue;
						}

						result.Add(vehPathNode->GetHandleForNativeFunctions());
					}
				}

				return result.ToArray();
			}

			public static int GetClosestLoadedVehiclePathNode(float x, float y, float z, float radius, Func<int, bool> predicateForFlags)
			{
				int result = 0;
				float closestDistance = 3e38f;

				foreach (var areaId in GetAreaIdsInRange(x, y, radius))
				{
					var pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					var vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						var vehPathNode = pathRegion->GetPathNodeUnsafe(j);
						if (!CheckVehPathNodePropertyPredicateAndPosition(vehPathNode, predicateForFlags, x, y, z, radius))
						{
							continue;
						}

						var nodePos = vehPathNode->UncompressedPosition;
						var nodeDist = DistanceToSquared(x, y, z, nodePos.X, nodePos.Y, nodePos.Z);
						if (nodeDist < closestDistance)
						{
							result = vehPathNode->GetHandleForNativeFunctions();
							closestDistance = nodeDist;
						}
					}
				}

				return result;
			}

			public static int[] GetLoadedVehicleNodesInArea(float x1, float y1, float z1, float x2, float y2, float z2, Func<int, bool> predicateForFlags)
			{
				var minX = Math.Min(x1, x2);
				var minY = Math.Min(y1, y2);
				var minZ = Math.Min(z1, z2);
				var maxX = Math.Max(x1, x2);
				var maxY = Math.Max(y1, y2);
				var maxZ = Math.Max(z1, z2);

				var result = new List<int>();

				foreach (var areaId in GetAreaIdsInArea(minX, minY, maxX, maxY))
				{
					var pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					var vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						var vehPathNode = pathRegion->GetPathNodeUnsafe(j);

						if (!CheckVehPathNodePropertyPredicateAndPosition(vehPathNode, predicateForFlags, minX, minY, minZ, maxX, maxY, maxZ))
						{
							continue;
						}

						result.Add(vehPathNode->GetHandleForNativeFunctions());
					}
				}

				return result.ToArray();
			}

			public static int[] GetPathNodeLinkIndicesOfPathNode(int handleOfPathNode)
			{
				GetCorrectedNodeAndAreaIdFromPathNodeHandle(handleOfPathNode, out uint areaId, out uint nodeId);

				var pathRegion = GetCPathRegion(areaId);
				if (pathRegion == null)
				{
					return Array.Empty<int>();
				}

				var pathNode = pathRegion->GetPathNode(nodeId);
				if (pathNode == null)
				{
					return Array.Empty<int>();
				}

				var pathNodeLinkStartId = pathNode->LinkId;
				var pathNodeLinkCount = pathNode->LinkCount;

				var result = new int[pathNodeLinkCount];
				for (int i = 0; i < pathNodeLinkCount; i++)
				{
					result[i] = pathNodeLinkStartId + i;
				}
				return result;
			}

			public static bool GetPathNodeLinkLanes(int areaId, int nodeLinkIndex, out int forwardLaneCount, out int backwardLaneCount)
			{
				var pathRegion = GetCPathRegion((uint)areaId);
				if (pathRegion == null)
				{
					forwardLaneCount = 0;
					backwardLaneCount = 0;

					return false;
				}

				var pathNodeLink = pathRegion->GetPathNodeLink((uint)nodeLinkIndex);
				if (pathNodeLink == null)
				{
					forwardLaneCount = 0;
					backwardLaneCount = 0;

					return false;
				}

				pathNodeLink->GetForwardAndBackwardCount(out forwardLaneCount, out backwardLaneCount);
				return true;
			}

			public static bool GetTargetAreaAndNodeIdToTargetNode(int areaIdOfNodeLink, int nodeLinkIndex, out int targetAreaId, out int targetNodeId)
			{
				var pathRegion = GetCPathRegion((uint)areaIdOfNodeLink);
				if (pathRegion == null)
				{
					targetAreaId = 0;
					targetNodeId = 0;

					return false;
				}

				var pathNodeLink = pathRegion->GetPathNodeLink((uint)nodeLinkIndex);
				if (pathNodeLink == null)
				{
					targetAreaId = 0;
					targetNodeId = 0;

					return false;
				}

				pathNodeLink->GetTargetAreaAndNodeId(out targetAreaId, out targetNodeId);
				return true;
			}

			public static int GetTargetNodeHandleFromNodeLink(int areaIdOfNodeLink, int nodeLinkIndex)
			{
				var pathRegionOfNodeLink = GetCPathRegion((uint)areaIdOfNodeLink);
				if (pathRegionOfNodeLink == null)
				{
					return 0;
				}
				var pathNodeLink = pathRegionOfNodeLink->GetPathNodeLink((uint)nodeLinkIndex);
				if (pathNodeLink == null)
				{
					return 0;
				}

				pathNodeLink->GetTargetAreaAndNodeId(out var targetAreaId, out var targetNodeId);
				var pathRegionOfTargetNode = GetCPathRegion((uint)targetAreaId);
				if (pathRegionOfTargetNode == null)
				{
					return 0;
				}
				var targetPathNode = pathRegionOfTargetNode->GetPathNode((uint)targetNodeId);
				if (targetPathNode == null)
				{
					return 0;
				}

				return targetPathNode->GetHandleForNativeFunctions();
			}

			static bool CheckVehPathNodePropertyPredicateAndPosition(CPathNode* vehPathNode, Func<int, bool> predicateForFlags, float x, float y, float z, float maxDistRadius)
			{
				if (predicateForFlags != null && !predicateForFlags((int)vehPathNode->GetPropertyFlags()))
				{
					return false;
				}
				if (!vehPathNode->IsInCircle(x, y, z, maxDistRadius))
				{
					return false;
				}

				return true;
			}

			static bool CheckVehPathNodePropertyPredicateAndPosition(CPathNode* vehPathNode, Func<int, bool> predicateForFlags, float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
			{
				if (predicateForFlags != null && !predicateForFlags((int)vehPathNode->GetPropertyFlags()))
				{
					return false;
				}
				if (!vehPathNode->IsInArea(minX, minY, minZ, maxX, maxY, maxZ))
				{
					return false;
				}

				return true;
			}

			static IEnumerable<uint> GetAreaIdsInArea(float x1, float y1, float x2, float y2)
			{
				var minX = Math.Min(x1, x2);
				var minY = Math.Min(y1, y2);
				var maxX = Math.Max(x1, x2);
				var maxY = Math.Max(y1, y2);

				var minAreaRegionX = Math.Min(Math.Max((int)((minX + 8192f) / 512), 0), 32);
				var minAreaRegionY = Math.Min(Math.Max((int)((minY + 8192f) / 512), 0), 32);
				var maxAreaRegionX = Math.Min(Math.Max((int)((maxX + 8192f) / 512), 0), 32);
				var maxAreaRegionY = Math.Min(Math.Max((int)((maxY + 8192f) / 512), 0), 32);

				var areaIdCount = (maxAreaRegionX - minAreaRegionX + 1) * (maxAreaRegionY - minAreaRegionY + 1);

				for (int regionY = minAreaRegionY; regionY <= maxAreaRegionY; regionY++)
				{
					for (int regionX = minAreaRegionX; regionX <= maxAreaRegionX; regionX++)
					{
						yield return ComposeAreaIdByIndex(regionX, regionY);
					}
				}
			}

			static IEnumerable<uint> GetAreaIdsInRange(float x, float y, float radius)
			{
				var rectMinX = x - radius;
				var rectMinY = y - radius;
				var rectMaxX = x + radius;
				var rectMaxY = y + radius;

				var minAreaRegionXIndex = Math.Min(Math.Max((int)((rectMinX + 8192f) / 512), 0), 32);
				var minAreaRegionYIndex = Math.Min(Math.Max((int)((rectMinY + 8192f) / 512), 0), 32);
				var maxAreaRegionXIndex = Math.Min(Math.Max((int)((rectMaxX + 8192f) / 512), 0), 32);
				var maxAreaRegionYIndex = Math.Min(Math.Max((int)((rectMaxY + 8192f) / 512), 0), 32);

				for (int regionYIndex = minAreaRegionYIndex; regionYIndex <= maxAreaRegionYIndex; regionYIndex++)
				{
					for (int regionXIndex = minAreaRegionXIndex; regionXIndex <= maxAreaRegionXIndex; regionXIndex++)
					{
						float currectRegionMinXBound = (float)regionXIndex * 512 - 8192f;
						float currectRegionMinYBound = (float)regionYIndex * 512 - 8192f;
						float currectRegionMaxXBound = currectRegionMinXBound + 512f;
						float currectRegionMaxYBound = currectRegionMinYBound + 512f;

						if (DoCircleAndRectIntersectOrTouch(radius, x, y, currectRegionMinXBound, currectRegionMinYBound, currectRegionMaxXBound, currectRegionMaxYBound))
						{
							yield return ComposeAreaIdByIndex(regionXIndex, regionYIndex);
						}
					}
				}
			}

			static uint ComposeAreaIdByIndex(int x, int y) => (uint)(x + y * 0x20);

			// Nodes at bound can be included in either area (e.g. a vehicle node at (0, 263, 10) can be included in either of the ynd files for the area IDs 527 (0x20F) or 528 (0x210))
			static bool DoCircleAndRectIntersectOrTouch(float radius, float xCenter, float yCenter, float x1, float y1, float x2, float y2)
			{
				// Nearest position will be calculated wrong if x1 and y2 parameters are passed as x2 and y2 and vice versa
				var nearestX = Math.Max(x1, Math.Min(xCenter, x2));
				var nearestY = Math.Max(y1, Math.Min(yCenter, y2));
				var deltaX = xCenter - nearestX;
				var deltaY = yCenter - nearestY;

				return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
			}
		}

		#endregion

		#region -- Radar Blip Pool --

		static ulong* RadarBlipPoolAddress;
		static int* PossibleRadarBlipCountAddress;
		static int* UnkFirstRadarBlipIndexAddress;
		static int* NorthRadarBlipHandleAddress;
		static int* CenterRadarBlipHandleAddress;

		static bool CheckBlip(ulong blipAddress, FVector3? position, float radius, params int[] spriteTypes)
		{
			if (spriteTypes.Length > 0)
			{
				var spriteIndex = *(int*)(blipAddress + 0x40);
				if (!Array.Exists(spriteTypes, x => x == spriteIndex))
					return false;
			}

			if (position == null || !(radius > 0f)) return true;

			var positionNonNullable = position.GetValueOrDefault();
			var blipPosition = stackalloc float[3];

			blipPosition[0] = *(float*)(blipAddress + 0x10);
			blipPosition[1] = *(float*)(blipAddress + 0x14);
			blipPosition[2] = *(float*)(blipAddress + 0x18);

			var x = blipPosition[0] - positionNonNullable.X;
			var y = blipPosition[1] - positionNonNullable.Y;
			var z = blipPosition[2] - positionNonNullable.Z;
			var distanceSquared = (x * x) + (y * y) + (z * z);
			var radiusSquared = radius * radius;

			return distanceSquared <= radiusSquared;
		}

		// The equivalent function is called in 2 functions (which is for the north and player blip) used in GET_NUMBER_OF_ACTIVE_BLIPS
		private static short GetBlipIndexIfHandleIsValid(int handle)
		{
			if (handle == 0)
			{
				return -1;
			}
			var blipIndex = (ushort)handle;
			var blipAddress = *(RadarBlipPoolAddress + blipIndex);
			if (blipAddress == 0)
			{
				return -1;
			}

			var blipCreationIncrement = (handle >> 0x10);
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
		public static int[] GetNonCriticalRadarBlipHandles(FVector3? position = default, float radius = 0f, params int[] spriteTypes)
		{
			if (RadarBlipPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			var possibleBlipCount = *PossibleRadarBlipCountAddress;
			var unkFirstBlipIndex = *UnkFirstRadarBlipIndexAddress;
			int northBlipIndex = GetBlipIndexIfHandleIsValid(*NorthRadarBlipHandleAddress);
			int centerBlipIndex = GetBlipIndexIfHandleIsValid(*CenterRadarBlipHandleAddress);

			var handles = new List<int>(possibleBlipCount);

			// Skip the 3 critical blips, just like GET_FIRST_BLIP_INFO_ID does
			// The 3 critical blips is the north blip, the center blip, and the unknown simple blip (placeholder?).
			for (var i = 0; i < possibleBlipCount; i++)
			{
				var address = *(RadarBlipPoolAddress + i);

				if (address == 0 || i == unkFirstBlipIndex || i == northBlipIndex || i == centerBlipIndex)
					continue;

				if (!CheckBlip(address, position, radius, spriteTypes)) continue;
				var blipCreationIncrement = *(ushort*)(address + 8);
				handles.Add((int)((blipCreationIncrement << 0x10) + (uint)i));
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

			var poolIndexOfHandle = handle & 0xFFFF;
			var possibleBlipCount = *PossibleRadarBlipCountAddress;

			if (poolIndexOfHandle >= possibleBlipCount)
			{
				return IntPtr.Zero;
			}

			var address = *(RadarBlipPoolAddress + poolIndexOfHandle);

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

				var elementCount = 0;
				var firstRegisteredScriptResourceItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (var item = firstRegisteredScriptResourceItem; item != null; item = item->next)
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
				for (var item = firstRegisteredScriptResourceItem; item != null; item = item->next)
				{
					if (item->counterOfPool != targetHandle) continue;
					returnAddress = new IntPtr((long)((byte*)(poolAddress) + item->indexOfPool * elementSize));
					break;
				}
			}
		}

		#endregion

		#region -- Checkpoint Pool --

		static ulong* CheckpointPoolAddress;

		static delegate* unmanaged[Stdcall]<ulong> GetCGameScriptHandlerAddressFunc;

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

		static ulong* waypointInfoArrayStartAddress;
		static ulong* waypointInfoArrayEndAddress;
		static delegate* unmanaged[Stdcall]<ulong> GetLocalPlayerPedAddressFunc;

		public static int GetWaypointBlip()
		{
			if (waypointInfoArrayStartAddress == null || waypointInfoArrayEndAddress == null)
				return 0;

			var playerPedModelHash = 0;
			var playerPedAddress = GetLocalPlayerPedAddressFunc();

			if (playerPedAddress != 0)
			{
				playerPedModelHash = GetModelHashFromEntity(new IntPtr((long)playerPedAddress));
			}

			var waypointInfoAddress = (ulong)waypointInfoArrayStartAddress;
			for (; waypointInfoAddress < (ulong)waypointInfoArrayEndAddress; waypointInfoAddress += 0x18)
			{
				var modelHash = *(int*)waypointInfoAddress;

				if (modelHash == playerPedModelHash)
				{
					return *(int*)(waypointInfoAddress + 4);
				}
			}

			return 0;
		}

		#endregion

		#region -- Pool Addresses --

		static delegate* unmanaged[Stdcall]<int, ulong> GetPtfxAddressFunc;
		// should be CGameScriptHandler::GetScriptEntity
		static delegate* unmanaged[Stdcall]<int, ulong> GetScriptEntity;

		public static IntPtr GetPtfxAddress(int handle)
		{
			return new IntPtr((long)GetPtfxAddressFunc(handle));
		}
		public static IntPtr GetEntityAddress(int handle)
		{
			return new IntPtr((long)GetScriptEntity(handle));
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

		static delegate* unmanaged[Stdcall]<IntPtr, int, void> ExplodeProjectileFunc;

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

		public static int GetEntityHandleFromAddress(IntPtr address)
		{
			var task = new GetEntityHandleTask(address);

			ScriptDomain.CurrentDomain.ExecuteTask(task);

			return task.returnEntityHandle;
		}

		static int GetBuildingHandleFromAddress(IntPtr address)
		{
			if (BuildingPoolAddress == null)
			{
				return 0;
			}

			return GetHandleForGenericPoolFromAddress(*BuildingPoolAddress, address);
		}

		static int GetHandleForGenericPoolFromAddress(ulong poolAddress, IntPtr instanceAddress) => ((GenericPool*)poolAddress)->GetGuidHandleFromAddress((ulong)instanceAddress);

		#endregion

		#region -- Weapon Info And Ammo Info --

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

		static RageAtArrayPtr* weaponAndAmmoInfoArrayPtr;
		static HashSet<uint> disallowWeaponHashSetForHumanPedsOnFoot = new HashSet<uint>()
		{
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

		[StructLayout(LayoutKind.Explicit, Size = 0x20)]
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

			internal uint GetClassNameHash()
			{
				// In the b2802 or a later exe, the function returns a hash value (not a pointer value)
				if (GetGameVersion() >= 80)
				{
					// The function is for the game version b2802 or later ones.
					// This one directly returns a hash value (not a pointer value) unlike the previous function.
					var GetClassNameHashFunc = (delegate* unmanaged[Stdcall]<uint>)(vTable[2]);
					return GetClassNameHashFunc();
				}

				// The function is for game versions prior to b2802.
				// The function uses rax and rdx registers in newer versions prior to b2802 (probably since b2189), and it uses only rax register in older versions.
				// The function returns the address where the class name hash is in all versions prior to (the address will be the outVal address in newer versions).
				var GetClassNameAddressHashFunc = (delegate* unmanaged[Stdcall]<ulong, uint*, uint*>)(vTable[2]);

				uint outVal = 0;
				var returnValueAddress = GetClassNameAddressHashFunc(0, &outVal);
				return *returnValueAddress;
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
				var indexToRead = (low + high) >> 1;
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
				var indexToRead = (low + high) >> 1;
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

			for (var i = 0; i < weaponAttachPointsCount; i++)
			{
				var weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + (i * weaponAttachPointElementSize) + 0x8;
				var componentItemsCount = *(int*)(weaponAttachPointElementAddr + weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
					continue;

				for (var j = 0; j < componentItemsCount; j++)
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

			for (var i = 0; i < weaponAndAmmoInfoElementCount; i++)
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
			for (var i = 0; i < weaponAttachPointsCount; i++)
			{
				var weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + i * weaponAttachPointElementSize;
				var componentItemsCount = *(int*)(weaponAttachPointElementAddr + weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
					continue;

				for (var j = 0; j < componentItemsCount; j++)
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

		static int getFragInstVFuncOffset;
		static delegate* unmanaged[Stdcall]<FragInst*, int, FragInst*> detachFragmentPartByIndexFunc;
		static ulong** phSimulatorInstPtr;
		static int colliderCapacityOffset;
		static int colliderCountOffset;

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
			// this field has a pointer to one matrix, not a pointer to an array of matrices for all bones
			[FieldOffset(0x8)] internal ulong boneTransformMatrixPtr;
			// object matrices (entity-local space)
			[FieldOffset(0x10)] internal ulong boneObjectMatrixArrayPtr;
			// global matrices (world space)
			[FieldOffset(0x18)] internal ulong boneGlobalMatrixArrayPtr;
			[FieldOffset(0x20)] internal int boneCount;

			public IntPtr GetTransformMatrixAddress()
			{
				return new IntPtr((long)(boneTransformMatrixPtr));
			}

			public IntPtr GetBoneObjectMatrixAddress(int boneIndex)
			{
				return new IntPtr((long)(boneObjectMatrixArrayPtr + ((uint)boneIndex * 0x40)));
			}

			public IntPtr GetBoneGlobalMatrixAddress(int boneIndex)
			{
				return new IntPtr((long)(boneGlobalMatrixArrayPtr + ((uint)boneIndex * 0x40)));
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 0x50)]
		internal struct CrBoneData
		{
			// Rotation (quaternion) is between 0x0 - 0x10
			// Translation (vector3) is between 0x10 - 0x1C
			// Scale (vector3?) is between 0x20 - 0x2C
			[FieldOffset(0x30)]
			internal ushort nextSiblingBoneIndex;
			[FieldOffset(0x32)]
			internal ushort parentBoneIndex;
			[FieldOffset(0x38)]
			internal IntPtr namePtr;
			[FieldOffset(0x42)]
			internal ushort boneIndex;
			[FieldOffset(0x44)]
			internal ushort boneId;

			internal string Name => namePtr == default ? null : Marshal.PtrToStringAnsi(namePtr);
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct CrSkeletonData
		{
			[FieldOffset(0x10)] internal PgHashMap boneHashMap;
			[FieldOffset(0x20)] internal CrBoneData* boneData;
			[FieldOffset(0x5E)] internal ushort boneCount;

			/// <summary>
			/// Gets the bone index from specified bone id. Note that bone indexes are sequential values and bone ids are not sequential ones.
			/// </summary>
			public int GetBoneIndexByBoneId(int boneId)
			{
				if (boneHashMap.elementCount == 0)
				{
					if (boneId < boneCount)
						return boneId;

					return -1;
				}

				if (boneHashMap.bucketCount == 0)
					return -1;

				if (boneHashMap.Get((uint)boneId, out var returnBoneId))
				{
					return returnBoneId;
				}

				return -1;
			}

			/// <summary>
			/// Gets the bone id from specified bone index. Note that bone indexes are sequential values and bone ids are not sequential ones.
			/// </summary>
			internal int GetBoneIdByIndex(int boneIndex)
			{
				if (boneIndex < 0 || boneIndex >= boneCount)
					return -1;

				return ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->boneId;
			}

			/// <summary>
			/// Gets the next sibling bone index of specified bone index.
			/// </summary>
			internal void GetNextSiblingBoneIndexAndId(int boneIndex, out int nextSiblingBoneIndex, out int nextSiblingBoneId)
			{
				if (boneIndex < 0 || boneIndex >= boneCount)
				{
					nextSiblingBoneIndex = -1;
					nextSiblingBoneId = -1;
					return;
				}

				var crBoneData = ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
				var nextSiblingBoneIndexFetched = crBoneData->nextSiblingBoneIndex;
				if (nextSiblingBoneIndexFetched == 0xFFFF)
				{
					nextSiblingBoneIndex = -1;
					nextSiblingBoneId = -1;
					return;
				}

				var nextSiblingBoneIdFetched = GetBoneIdByIndex(nextSiblingBoneIndexFetched);
				if (nextSiblingBoneIndexFetched == 0xFFFF)
				{
					nextSiblingBoneIndex = -1;
					nextSiblingBoneId = -1;
					return;
				}

				nextSiblingBoneIndex = nextSiblingBoneIndexFetched;
				nextSiblingBoneId = nextSiblingBoneIdFetched;
			}

			/// <summary>
			/// Gets the next parent bone index of specified bone index.
			/// </summary>
			internal void GetParentBoneIndexAndId(int boneIndex, out int parentBoneIndex, out int parentBoneId)
			{
				if (boneIndex < 0 || boneIndex >= boneCount)
				{
					parentBoneIndex = -1;
					parentBoneId = -1;
					return;
				}

				var crBoneData = ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex));
				var nextParentBoneIndexFetched = crBoneData->parentBoneIndex;
				if (nextParentBoneIndexFetched == 0xFFFF)
				{
					parentBoneIndex = -1;
					parentBoneId = -1;
					return;
				}

				var nextParentBoneIdFetched = GetBoneIdByIndex(nextParentBoneIndexFetched);
				if (nextParentBoneIdFetched == 0xFFFF)
				{
					parentBoneIndex = -1;
					parentBoneId = -1;
					return;
				}

				parentBoneIndex = nextParentBoneIndexFetched;
				parentBoneId = nextParentBoneIdFetched;
			}

			/// <summary>
			/// Gets the bone name string from specified bone index.
			/// </summary>
			internal string GetBoneName(int boneIndex)
			{
				if (boneIndex < 0 || boneIndex >= boneCount)
					return null;

				return ((CrBoneData*)((ulong)boneData + (uint)sizeof(CrBoneData) * (uint)boneIndex))->Name;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct HashEntry
		{
			internal uint hash;
			internal int data;
			internal HashEntry* next;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal unsafe struct PgHashMap
		{
			[FieldOffset(0x0)]
			internal ulong* buckets;
			[FieldOffset(0x8)]
			internal ushort bucketCount;
			[FieldOffset(0xA)]
			internal ushort elementCount;

			internal ulong GetBucketAddress(int index)
			{
				return buckets[index];
			}

			internal bool Get(uint hash, out int value)
			{
				var firstEntryAddr = (ulong*)GetBucketAddress((int)(hash % bucketCount));
				for (var hashEntry = (HashEntry*)firstEntryAddr; hashEntry != null; hashEntry = hashEntry->next)
				{
					if (hash != hashEntry->hash) continue;
					value = hashEntry->data;
					return true;
				}

				value = default;
				return false;
			}
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

			for (var i = 0; i < fragmentGroupCount; i++)
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
			var getFragInstFunc = (delegate* unmanaged[Stdcall]<IntPtr, FragInst*>)(vFuncAddr);

			return getFragInstFunc(entityAddress);
		}

		private static int GetFragmentGroupCountOfFragInst(FragInst* fragInst)
		{
			var fragPhysicsLOD = fragInst->GetAppropriateFragPhysicsLOD();
			return fragPhysicsLOD != null ? fragPhysicsLOD->fragmentGroupCount : 0;
		}


		#endregion

		#region -- NaturalMotion Euphoria --

		// These CNmParameter functions can also be called as virtual functions for your information
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, int, byte> SetNmParameterInt;
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, bool, byte> SetNmParameterBool;
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, float, byte> SetNmParameterFloat;
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, IntPtr, byte> SetNmParameterString;
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, float, float, float, byte> SetNmParameterVector;

		static delegate* unmanaged[Stdcall]<ulong, ulong, int, ulong> InitMessageMemoryFunc;
		static delegate* unmanaged[Stdcall]<ulong, IntPtr, ulong, void> SendNmMessageToPedFunc;
		static delegate* unmanaged[Stdcall]<ulong, CTask*> GetActiveTaskFunc;

		static int fragInstNMGtaOffset;
		static int cTaskNMScriptControlTypeIndex;
		static int cEventSwitch2NMTypeIndex;
		static uint getEventTypeIndexVFuncOffset;
		static uint fragInstNMGtaGetUnkValVFuncOffset;

		[StructLayout(LayoutKind.Explicit, Size = 0x38)]
		struct CTask
		{
			[FieldOffset(0x34)]
			internal ushort taskTypeIndex;
		}

		public static bool IsTaskNMScriptControlOrEventSwitch2NMActive(IntPtr pedAddress)
		{
			var phInstGtaAddress = *(ulong*)(pedAddress + 0x30);

			if (phInstGtaAddress == 0)
				return false;

			var fragInstNMGtaAddress = *(ulong*)(pedAddress + fragInstNMGtaOffset);

			if (phInstGtaAddress != fragInstNMGtaAddress || IsPedInjured((byte*)pedAddress)) return false;
			// This virtual function will return -1 if phInstGta is not a NM one
			var fragInstNMGtaGetUnkValVFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(new IntPtr((long)*(ulong*)(*(ulong*)fragInstNMGtaAddress + fragInstNMGtaGetUnkValVFuncOffset)));
			if (fragInstNMGtaGetUnkValVFunc(fragInstNMGtaAddress) == -1) return false;

			var pedIntelligenceAddr = *(ulong*)(pedAddress + PedIntelligenceOffset);

			var activeTask = GetActiveTaskFunc(*(ulong*)((byte*)pedIntelligenceAddr + CTaskTreePedOffset));
			if (activeTask != null && activeTask->taskTypeIndex == cTaskNMScriptControlTypeIndex)
			{
				return true;
			}

			var eventCount = *(int*)((byte*)pedIntelligenceAddr + CEventCountOffset);
			for (var i = 0; i < eventCount; i++)
			{
				var eventAddress = *(ulong*)((byte*)pedIntelligenceAddr + CEventStackOffset + 8 * ((i + *(int*)((byte*)pedIntelligenceAddr + (CEventCountOffset - 4)) + 1) % 16));
				if (eventAddress == 0) continue;

				var getEventTypeIndexVirtualFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(*(ulong*)(*(ulong*)eventAddress + getEventTypeIndexVFuncOffset));
				if (getEventTypeIndexVirtualFunc(eventAddress) != cEventSwitch2NMTypeIndex) continue;

				var taskInEvent = *(CTask**)(eventAddress + 0x28);
				if (taskInEvent == null) continue;

				if (taskInEvent->taskTypeIndex == cTaskNMScriptControlTypeIndex)
				{
					return true;
				}
			}

			return false;
		}

		static bool IsPedInjured(byte* pedAddress) => *(float*)(pedAddress + 0x280) < *(float*)(pedAddress + InjuryHealthThresholdOffset);

		private static void SetNMParameters(ulong messageMemory, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
		{
			if (boolIntFloatParameters != null)
			{
				foreach (var arg in boolIntFloatParameters)
				{
					var name = ScriptDomain.CurrentDomain.PinString(arg.Key);

					var (argValue, argType) = arg.Value;

					if (argType == typeof(float))
					{
						var argValueConverted = *(float*)(&argValue);
						NativeMemory.SetNmParameterFloat(messageMemory, name, argValueConverted);
					}
					else if (argType == typeof(bool))
					{
						var argValueConverted = argValue != 0 ? true : false;
						NativeMemory.SetNmParameterBool(messageMemory, name, argValueConverted);
					}
					else if (argType == typeof(int))
					{
						NativeMemory.SetNmParameterInt(messageMemory, name, argValue);
					}
				}
			}

			if ((stringVector3ArrayParameters != null))
			{
				foreach (var arg in stringVector3ArrayParameters)
				{
					var name = ScriptDomain.CurrentDomain.PinString(arg.Key);

					var argValue = arg.Value;
					switch (argValue)
					{
						case float[] vector3ArgValue:
							NativeMemory.SetNmParameterVector(messageMemory, name, vector3ArgValue[0], vector3ArgValue[1], vector3ArgValue[2]);
							break;
						case string stringArgValue:
							NativeMemory.SetNmParameterString(messageMemory, name, ScriptDomain.CurrentDomain.PinString(stringArgValue));
							break;
					}
				}
			}
		}

		internal class NmMessageTask : IScriptTask
		{
			#region Fields
			int targetHandle;
			string messageName;
			Dictionary<string, (int value, Type type)> boolIntFloatParameters;
			Dictionary<string, object> stringVector3ArrayParameters;
			#endregion

			internal NmMessageTask(int target, string messageName, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
			{
				targetHandle = target;
				this.messageName = messageName;
				this.boolIntFloatParameters = boolIntFloatParameters;
				this.stringVector3ArrayParameters = stringVector3ArrayParameters;
			}

			public void Run()
			{
				var pedAddress = (byte*)NativeMemory.GetEntityAddress(targetHandle).ToPointer();

				if (pedAddress == null)
					return;

				if (!IsTaskNMScriptControlOrEventSwitch2NMActive(new IntPtr(pedAddress)))
					return;

				var messageMemory = (ulong)AllocCoTaskMem(0x1218).ToInt64();
				if (messageMemory == 0)
					return;
				InitMessageMemoryFunc(messageMemory, messageMemory + 0x18, 0x40);

				SetNMParameters(messageMemory, boolIntFloatParameters, stringVector3ArrayParameters);

				var fragInstNMGtaAddress = *(ulong*)(pedAddress + fragInstNMGtaOffset);
				var messageStringPtr = ScriptDomain.CurrentDomain.PinString(messageName);
				SendNmMessageToPedFunc((ulong)fragInstNMGtaAddress, messageStringPtr, messageMemory);

				FreeCoTaskMem(new IntPtr((long)messageMemory));
			}
		}

		public static void SendNmMessage(int targetHandle, string messageName, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
		{
			var task = new NmMessageTask(targetHandle, messageName, boolIntFloatParameters, stringVector3ArrayParameters);
			ScriptDomain.CurrentDomain.ExecuteTask(task);
		}

		#endregion
	}
}
