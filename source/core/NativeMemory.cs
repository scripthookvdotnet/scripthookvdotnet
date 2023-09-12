using System;
using System.Drawing;
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
			ProcessModule module = Process.GetCurrentProcess().MainModule;
			return FindPatternNaive(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternNaive(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternNaive(string pattern, string mask, IntPtr startAddress)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
			{
				return null;
			}

			ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

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
			ulong address = (ulong)startAddress.ToInt64();
			ulong endAddress = address + size;

			for (; address < endAddress; address++)
			{
				for (int i = 0; i < pattern.Length; i++)
				{
					if (mask[i] != '?' && ((byte*)address)[i] != pattern[i])
					{
						break;
					}

					if (i + 1 == pattern.Length)
					{
						return (byte*)address;
					}
				}
			}

			return null;
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;
			return FindPatternBmh(pattern, mask, module.BaseAddress, (ulong)module.ModuleMemorySize);
		}

		/// <inheritdoc cref="FindPatternBmh(string, string, IntPtr, ulong)"/>
		public static unsafe byte* FindPatternBmh(string pattern, string mask, IntPtr startAddress)
		{
			ProcessModule module = Process.GetCurrentProcess().MainModule;

			if ((ulong)startAddress.ToInt64() < (ulong)module.BaseAddress.ToInt64())
			{
				return null;
			}

			ulong size = (ulong)module.ModuleMemorySize - ((ulong)startAddress - (ulong)module.BaseAddress);

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
			short[] patternArray = new short[pattern.Length];
			for (int i = 0; i < patternArray.Length; i++)
			{
				patternArray[i] = (mask[i] != '?') ? (short)pattern[i] : (short)-1;
			}

			int lastPatternIndex = patternArray.Length - 1;
			short[] skipTable = CreateShiftTableForBmh(patternArray);

			byte* endAddressToScan = (byte*)startAddress + size - patternArray.Length;

			// Pin arrays to avoid boundary check and search will be long enough to amortize the pin cost in time wise
			fixed (short* skipTablePtr = skipTable)
			fixed (short* patternArrayPtr = patternArray)
			{
				for (byte* curHeadAddress = (byte*)startAddress; curHeadAddress <= endAddressToScan; curHeadAddress += Math.Max((int)skipTablePtr[(curHeadAddress)[lastPatternIndex] & 0xFF], 1))
				{
					for (int i = lastPatternIndex; patternArrayPtr[i] < 0 || ((byte*)curHeadAddress)[i] == patternArrayPtr[i]; --i)
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
			short[] skipTable = new short[256];
			int lastIndex = pattern.Length - 1;

			int diff = lastIndex - Math.Max(Array.LastIndexOf<short>(pattern, -1), 0);
			if (diff == 0)
			{
				diff = 1;
			}

			for (int i = 0; i < skipTable.Length; i++)
			{
				skipTable[i] = (short)diff;
			}

			for (int i = lastIndex - diff; i < lastIndex; i++)
			{
				short patternVal = pattern[i];
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
			String = StringMarshal.StringToCoTaskMemUtf8("STRING"); // "~a~"
			NullString = StringMarshal.StringToCoTaskMemUtf8(string.Empty); // ""
			CellEmailBcon = StringMarshal.StringToCoTaskMemUtf8("CELL_EMAIL_BCON"); // "~a~~a~~a~~a~~a~~a~~a~~a~~a~~a~"

			byte* address;
			IntPtr startAddressToSearch;

			// Get relative address and add it to the instruction address.
			address = FindPatternBmh("\x74\x21\x48\x8B\x48\x20\x48\x85\xC9\x74\x18\x48\x8B\xD6\xE8", "xxxxxxxxxxxxxxx") - 10;
			s_getPtfxAddressFunc = (delegate* unmanaged[Stdcall]<int, ulong>)(
				new IntPtr(*(int*)(address) + address + 4));

			address = FindPatternBmh("\x85\xED\x74\x0F\x8B\xCD\xE8\x00\x00\x00\x00\x48\x8B\xF8\x48\x85\xC0\x74\x2E", "xxxxxxx????xxxxxxxx");
			s_getScriptEntity = (delegate* unmanaged[Stdcall]<int, ulong>)(
				new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x8B\xC2\xB2\x01\x8B\xC8\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x53\x8A\x88\x00\x00\x00\x00\xF6\xC1\x01\x75\x05\xF6\xC1\x02\x75\x43\x48", "xxxxxxx????xxxxxxx????xxxxxxxxxxx");
			s_getPlayerPedAddressFunc = (delegate* unmanaged[Stdcall]<int, ulong>)(
			new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x0F\x84\xA1\x00\x00\x00\x33\xC9\x48\x89\x35", "xxxxxxxxxxx");
			if (address != null)
			{
				s_isGameMultiplayerAddr = (bool*)(*(int*)(address + 0x27) + address + 0x2B);
			}

			address = FindPatternBmh("\x48\xF7\xF9\x49\x8B\x48\x08\x48\x63\xD0\xC1\xE0\x08\x0F\xB6\x1C\x11\x03\xD8", "xxxxxxxxxxxxxxxxxxx");
			s_createGuid = (delegate* unmanaged[Stdcall]<ulong, int>)(
				new IntPtr(address - 0x68));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x30\x48\x8B\xDA\xE8\x00\x00\x00\x00\xF3\x0F\x10\x44\x24\x2C\x33\xC9\xF3\x0F\x11\x43\x0C\x48\x89\x0B\x89\x4B\x08\x48\x85\xC0\x74\x2C", "xxxxxxxxxx????xxxxxxxxxxxxxxxxxxxxxxxx");
			s_entityPosFunc = (delegate* unmanaged[Stdcall]<ulong, float*, ulong>)(address);

			// Find handling data functions
			address = FindPatternBmh("\x8B\xF7\x83\xF8\x01\x77\x08\x44\x8B\xF7\x8D\x77\xFF\xEB\x06\x41\xBE\x03\x00\x00\x00\x8B\x8B", "xxxxxxxxxxxxxxxxxxxxxxx");
			s_getHandlingDataByIndex = (delegate* unmanaged[Stdcall]<int, ulong>)(new IntPtr(*(int*)(address + 28) + address + 32));
			s_handlingIndexOffsetInModelInfo = *(int*)(address + 23);

			address = FindPatternBmh("\x75\x5A\xB2\x01\x48\x8B\xCB\xE8\x00\x00\x00\x00\x41\x8B\xF5\x66\x44\x3B\xAB", "xxxxxxxx????xxxxxxx");
			s_getHandlingDataByHash = (delegate* unmanaged[Stdcall]<IntPtr, ulong>)(
				new IntPtr(*(int*)(address - 7) + address - 3));

			// Find entity pools and interior proxy pool
			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x41\x0F\xBF\xC8\x0F\xBF\x40\x10", "xxx????xxxxxxxx");
			s_pedPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x8B\x78\x10\x85\xFF", "xxx????xxxxx");
			s_objectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x4C\x8B\x0D\x00\x00\x00\x00\x44\x8B\xC1\x49\x8B\x41\x08", "xxx????xxxxxxx");
			s_fwScriptGuidPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\xF3\x0F\x59\xF6\x48\x8B\x08", "xxx????xxxxxxx");
			s_vehiclePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x4C\x8B\x05\x00\x00\x00\x00\x40\x8A\xF2\x8B\xE9", "xxx????xxxxx");
			s_pickupObjectPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x83\x38\xFF\x74\x27\xD1\xEA\xF6\xC2\x01\x74\x20", "xxxxxxxxxxxx");
			if (address != null)
			{
				s_buildingPoolAddress = (ulong*)(*(int*)(address + 47) + address + 51);
				s_animatedBuildingPoolAddress = (ulong*)(*(int*)(address + 15) + address + 19);
			}
			address = FindPatternBmh("\x83\xBB\x80\x01\x00\x00\x01\x75\x12", "xxxxxxxxx");
			if (address != null)
			{
				s_interiorInstPoolAddress = (ulong*)(*(int*)(address + 23) + address + 27);
			}
			address = FindPatternBmh("\x0F\x85\xA3\x00\x00\x00\x8B\x52\x0C\x48\x8B\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x48\x85\xC0\x0F\x84\x8B\x00\x00\x00\x45\x33\xC9\x45\x33\xC0", "xxxxxxxxxxxx????x????xxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_interiorProxyPoolAddress = (ulong*)(*(int*)(address + 12) + address + 16);
			}

			address = FindPatternBmh("\x0F\x84\x87\x00\x00\x00\xFF\xC9\x74\x79\xFF\xC9\x74\x6B\x66\x0F\x6E\x35", "xxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_physicalScrenWidthAddr = (int*)(*(int*)(address + 0x12) + address + 0x16);
				s_physicalScrenHeightAddr = (int*)(*(int*)(address + 0x1A) + address + 0x1E);
				s_screenInfoAddr = new IntPtr((long*)(*(int*)(address + 0x2B) + address + 0x2F));

				s_unkScreenFunc = (delegate* unmanaged[Stdcall]<IntPtr, IntPtr>)((long*)(*(int*)(address + 0x30) + address + 0x34));
				s_isUsingMultiScreenFunc = (delegate* unmanaged[Stdcall]<IntPtr, bool>)((long*)(*(int*)(address + 0x38) + address + 0x3C));
				s_getMainScreenInfoFunc = (delegate* unmanaged[Stdcall]<IntPtr, ScreenInfo*>)((long*)(*(int*)(address + 0x50) + address + 0x54));
			}

			// Find euphoria functions
			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x20\x83\x61\x0C\x00\x44\x89\x41\x08\x49\x63\xC0", "xxxxxxxxxxxxxxxxx");
			s_initMessageMemoryFunc = (delegate* unmanaged[Stdcall]<ulong, ulong, int, ulong>)(new IntPtr(address));

			address = FindPatternBmh("\x0F\x84\x8B\x00\x00\x00\x48\x8B\x47\x30\x48\x8B\x48\x10\x48\x8B\x51\x20\x80\x7A\x10\x0A", "xxxxxxxxxxxxxxxxxxxxxx");
			s_sendNmMessageToPedFunc = (delegate* unmanaged[Stdcall]<ulong, IntPtr, ulong, void>)((ulong*)(*(int*)(address - 0x1E) + address - 0x1A));

			address = FindPatternBmh("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8B\xF8", "xxxx?xxxxxxxxxxxxxxx");
			s_setNmParameterInt = (delegate* unmanaged[Stdcall]<ulong, IntPtr, int, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x48\x89\x5C\x24\x00\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x41\x8A\xF8", "xxxx?xxxxxxxxxxxxxxx");
			s_setNmParameterBool = (delegate* unmanaged[Stdcall]<ulong, IntPtr, bool, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x30\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			s_setNmParameterFloat = (delegate* unmanaged[Stdcall]<ulong, IntPtr, float, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x57\x48\x83\xEC\x20\x48\x8B\xD9\x48\x63\x49\x0C\x49\x8B\xE8", "xxxxxxxxxxxxxxx") - 15;
			s_setNmParameterString = (delegate* unmanaged[Stdcall]<ulong, IntPtr, IntPtr, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x40\x53\x48\x83\xEC\x40\x48\x8B\xD9\x48\x63\x49\x0C", "xxxxxxxxxxxxx");
			s_setNmParameterVector = (delegate* unmanaged[Stdcall]<ulong, IntPtr, float, float, float, byte>)(new IntPtr(address));

			address = FindPatternBmh("\x4D\x8B\xF0\x48\x8B\xF2\xE8\x00\x00\x00\x00\x33\xFF\x48\x85\xC0\x75\x07\x32\xC0\xE9\xD8\x03\x00\x00", "xxxxxxx????xxxxxxxxxxxxxx");
			s_getActiveTaskFunc = (delegate* unmanaged[Stdcall]<ulong, CTask*>)(new IntPtr(*(int*)(address + 7) + address + 11));

			address = FindPatternBmh("\x75\xEF\x48\x8B\x5C\x24\x30\xB8", "xxxxxxxx");
			if (address != null)
			{
				s_cTaskNmScriptControlTypeIndex = *(int*)(address + 8);
			}

			address = FindPatternBmh("\x4C\x8B\x03\x48\x8B\xD5\x48\x8B\xCB\x41\xFF\x50\x00\x83\xFE\x04", "xxxxxxxxxxxx?xxx");
			if (address != null)
			{
				// The instruction expects a signed value, but virtual function offsets can't be negative
				s_getEventTypeIndexVFuncOffset = (uint)*(byte*)(address + 12);
			}
			address = FindPatternBmh("\x48\x8D\x05\x00\x00\x00\x00\x48\x89\x01\x8B\x44\x24\x50", "xxx????xxxxxxx");
			if (address != null)
			{
				ulong cEventSwitch2NmVfTableArrayAddr = (ulong)(*(int*)(address + 3) + address + 7);
				ulong getEventTypeOfcEventSwitch2NmFuncAddr = *(ulong*)(cEventSwitch2NmVfTableArrayAddr + s_getEventTypeIndexVFuncOffset);
				s_cEventSwitch2NmTypeIndex = *(int*)(getEventTypeOfcEventSwitch2NmFuncAddr + 1);
			}

			address = FindPatternNaive("\x48\x83\xEC\x28\x48\x8B\x42\x00\x48\x85\xC0\x74\x09\x48\x3B\x82\x00\x00\x00\x00\x74\x21", "xxxxxxx?xxxxxxxx????xx");
			if (address != null)
			{
				s_fragInstNmGtaOffset = *(int*)(address + 16);
			}
			address = FindPatternNaive("\xB2\x01\x48\x8B\x01\xFF\x90\x00\x00\x00\x00\x80", "xxxxxxx????x");
			if (address != null)
			{
				s_fragInstNmGtaGetUnkValVFuncOffset = (uint)*(int*)(address + 7);
			}

			address = FindPatternBmh("\x84\xC0\x74\x34\x48\x8D\x0D\x00\x00\x00\x00\x48\x8B\xD3", "xxxxxxx????xxx");
			s_getLabelTextByHashAddress = (ulong)(*(int*)(address + 7) + address + 11);

			// Find the function that returns if the corresponding text label exist first.
			// We have to find GetLabelTextByHashFunc indirectly since Rampage Trainer hooks the function that returns the string address for corresponding text label hash by inserting jmp instruction at the beginning if that trainer is installed.
			address = FindPatternBmh("\x74\x64\x48\x8D\x15\x00\x00\x00\x00\x48\x8D\x0D\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x84\xC0\x74\x33", "xxxxx????xxx????x????xxxx");
			if (address != null)
			{
				byte* doesTextLabelExistFuncAddr = (byte*)(*(int*)(address + 17) + address + 21);
				long getLabelTextByHashFuncAddr = (long)(*(int*)(doesTextLabelExistFuncAddr + 28) + doesTextLabelExistFuncAddr + 32);
				s_getLabelTextByHashFunc = (delegate* unmanaged[Stdcall]<ulong, int, ulong>)(new IntPtr(getLabelTextByHashFuncAddr));
			}

			address = FindPatternBmh("\x8A\x4C\x24\x60\x8B\x50\x10\x44\x8A\xCE", "xxxxxxxxxx");
			s_checkpointPoolAddress = (ulong*)(*(int*)(address + 17) + address + 21);
			s_getCGameScriptHandlerAddressFunc = (delegate* unmanaged[Stdcall]<ulong>)(new IntPtr(*(int*)(address - 19) + address - 15));

			address = FindPatternBmh("\x4C\x8D\x05\x00\x00\x00\x00\x0F\xB7\xC1", "xxx????xxx");
			s_radarBlipPoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			address = FindPatternBmh("\xFF\xC6\x49\x83\xC6\x08\x3B\x35\x00\x00\x00\x00\x7C\x9B", "xxxxxxxx????xx");
			s_possibleRadarBlipCountAddress = (int*)(*(int*)(address + 8) + address + 12);
			address = FindPatternBmh("\x3B\x35\x00\x00\x00\x00\x74\x2E\x48\x81\xFD\xDB\x05\x00\x00", "xx????xxxxxxxxx");
			s_unkFirstRadarBlipIndexAddress = (int*)(*(int*)(address + 2) + address + 6);
			address = FindPatternBmh("\x41\xB8\x07\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFC", "xxxxxxxxxx????xxxx");
			s_northRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);
			address = FindPatternBmh("\x41\xB8\x06\x00\x00\x00\x8B\xD0\x89\x05\x00\x00\x00\x00\x41\x8D\x48\xFD", "xxxxxxxxxx????xxxx");
			s_centerRadarBlipHandleAddress = (int*)(*(int*)(address + 10) + address + 14);

			address = FindPatternBmh("\x33\xDB\xE8\x00\x00\x00\x00\x48\x85\xC0\x74\x07\x48\x8B\x40\x20\x8B\x58\x18", "xxx????xxxxxxxxxxxx");
			s_getLocalPlayerPedAddressFunc = (delegate* unmanaged[Stdcall]<ulong>)(new IntPtr(*(int*)(address + 3) + address + 7));

			address = FindPatternBmh("\x4C\x8D\x05\x00\x00\x00\x00\x74\x07\xB8\x00\x00\x00\x00\xEB\x2D\x33\xC0", "xxx????xxx????xxxx");
			s_waypointInfoArrayStartAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			if (s_waypointInfoArrayStartAddress != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x8D\x15\x00\x00\x00\x00\x48\x83\xC1\x00\xFF\xC0\x48\x3B\xCA\x7C\xEA\x32\xC0", "xxx????xxx?xxxxxxxxx", startAddressToSearch);
				s_waypointInfoArrayEndAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}

			address = FindPatternBmh("\x80\x3D\x00\x00\x00\x00\x00\x8B\xDA\x75\x29\x48\x8B\xD1\x33\xC9\xE8", "xx????xxxxxxxxxxx");
			if (address != null)
			{
				s_isDecoratorLocked = (byte*)(*(int*)(address + 2) + address + 7);
			}

			address = FindPatternBmh("\xF3\x0F\x10\x5C\x24\x20\xF3\x0F\x10\x54\x24\x24\xF3\x0F\x59\xD9\xF3\x0F\x59\xD1\xF3\x0F\x10\x44\x24\x28\xF3\x0F\x11\x1F", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_getRotationFromMatrixFunc = (delegate* unmanaged[Stdcall]<float*, ulong, int, float*>)(new IntPtr(*(int*)(address - 0x14) + address - 0x10));
			}
			address = FindPatternBmh("\xF3\x0F\x11\x4D\x38\xF3\x0F\x11\x45\x3C\xE8\x00\x00\x00\x00\x0F\x28\xC6\x0F\x28\xCE\xB9\x01\x00\x00\x00\xF3\x0F\x11\x73\x10\x66\x44\x03\xE9", "xxxxxxxxxxx????xxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_getQuaternionFromMatrixFunc = (delegate* unmanaged[Stdcall]<float*, ulong, int>)(new IntPtr(*(int*)(address + 11) + address + 15));
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
			NativeMemory.CAttackerArrayOfEntityOffset = *(int*)(address + 3); // the correct name is unknown
			if (address != null)
			{
				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x63\x51\x00\x48\x85\xD2", "xxx?xxx", startAddressToSearch);
				NativeMemory.ElementCountOfCAttackerArrayOfEntityOffset = (*(sbyte*)(address + 3));

				startAddressToSearch = new IntPtr(address);
				address = FindPatternBmh("\x48\x83\xC1\x00\x48\x3B\xC2\x7C\xEF", "xxx?xxxxx", startAddressToSearch);
				// the element size might be 0x10 in older builds (the size is 0x18 at least in b1604 and b2372)
				NativeMemory.ElementSizeOfCAttackerArrayOfEntity = (*(sbyte*)(address + 3));
			}

			address = FindPatternBmh("\x74\x11\x8B\xD1\x48\x8D\x0D\x00\x00\x00\x00\x45\x33\xC0", "xxxxxxx????xxx");
			s_cursorSpriteAddr = (int*)(*(int*)(address - 4) + address);

			address = FindPatternBmh("\x48\x63\xC1\x48\x8D\x0D\x00\x00\x00\x00\xF3\x0F\x10\x04\x81\xF3\x0F\x11\x05", "xxxxxx????xxxxxxxxx");
			s_readWorldGravityAddress = (float*)(*(int*)(address + 19) + address + 23);
			s_writeWorldGravityAddress = (float*)(*(int*)(address + 6) + address + 10);

			address = FindPatternBmh("\xF3\x0F\x11\x05\x00\x00\x00\x00\xF3\x0F\x10\x08\x0F\x2F\xC8\x73\x03\x0F\x28\xC1\x48\x83\xC0\x04\x49\x2B", "xxxx????xxxxxxxxxxxxxxxxxx");
			float* timeScaleArrayAddress = (float*)(*(int*)(address + 4) + address + 8);
			if (timeScaleArrayAddress != null)
			// SET_TIME_SCALE changes the 2nd element, so obtain the address of it
			{
				s_timeScaleAddress = timeScaleArrayAddress + 1;
			}

			address = FindPatternBmh("\xF3\x0F\x11\xB5\x60\x01\x00\x00\x84\xC0\x75\x4C\x85\xC9\x79\x1D\x33\xD2\xE8", "xxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				byte* unkClockFunc = (byte*)(*(int*)(address + 19) + address + 23);
				s_millisecondsPerGameMinuteAddress = (int*)(*(int*)(unkClockFunc + 0x46) + unkClockFunc + 0x4A);
				s_lastClockTickAddress = (int*)(s_millisecondsPerGameMinuteAddress + 2);
			}

			address = FindPatternBmh("\x75\x2D\x44\x38\x3D\x00\x00\x00\x00\x75\x24", "xxxxx????xx");
			s_isClockPausedAddress = (byte*)(*(int*)(address + 5) + address + 9);

			// Find camera objects
			address = FindPatternBmh("\x48\x8B\xC8\xEB\x02\x33\xC9\x48\x85\xC9\x74\x26", "xxxxxxxxxxxx") - 9;
			s_cameraPoolAddress = (ulong*)(*(int*)(address) + address + 4);
			address = FindPatternBmh("\x48\x8B\xC7\xF3\x0F\x10\x0D", "xxxxxxx") - 0x1D;
			address = address + *(int*)(address) + 4;
			s_gameplayCameraAddress = (ulong*)(*(int*)(address + 3) + address + 7);

			// Find model hash table
			address = FindPatternBmh("\x3C\x05\x75\x16\x8B\x81", "xxxxxx");
			if (address != null)
			{
				s_vehicleTypeOffsetInModelInfo = *(int*)(address + 6);
			}

			address = FindPatternBmh("\x66\x81\xF9\x00\x00\x74\x10\x4D\x85\xC0", "xxx??xxxxx") - 0x21;
			uint vehicleClassOffset = *(uint*)(address + 0x31);

			address = address + *(int*)(address) + 4;
			s_modelNum1 = *(UInt32*)(*(int*)(address + 0x52) + address + 0x56);
			s_modelNum2 = *(UInt64*)(*(int*)(address + 0x63) + address + 0x67);
			s_modelNum3 = *(UInt64*)(*(int*)(address + 0x7A) + address + 0x7E);
			s_modelNum4 = *(UInt64*)(*(int*)(address + 0x81) + address + 0x85);
			s_modelHashTable = *(UInt64*)(*(int*)(address + 0x24) + address + 0x28);
			s_modelHashEntries = *(UInt16*)(address + *(int*)(address + 3) + 7);

			address = FindPatternBmh("\x33\xD2\x00\x8B\xD0\x00\x2B\x05\x00\x00\x00\x00\xC1\xE6\x10", "xx?xx?xx????xxx");
			s_modelInfoArrayPtr = (ulong*)(*(int*)(address + 8) + address + 12);

			address = FindPatternBmh("\x48\x83\xEC\x20\x48\x8B\x91\x00\x00\x00\x00\x33\xF6\x48\x8B\xD9\x48\x85\xD2\x74\x2B\x48\x8D\x0D", "xxxxxxx??xxxxxxxxxxxxxxx");
			s_cStreamingAddr = (ulong*)(*(int*)(address + 24) + address + 28);

			address = FindPatternBmh("\x44\x39\x38\x74\x17\x48\xFF\xC1\x48\x83\xC0\x04\x48\x3B\xCB\x7C\xEF\x41\x8B\xD7\x49\x8B\xCE\xE8", "xxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				var unkFuncForVehicleModelIndices = (byte*)(*(int*)(address + 0x18) + address + 0x1C);
				s_cStreamingAppropriateVehicleIndicesOffset = *(int*)(unkFuncForVehicleModelIndices + 0x1E);
			}

			address = FindPatternBmh("\x75\x0D\x8B\xD7\x49\x8B\xCE\xE8\x00\x00\x00\x00\x41\x2B\xDD\x45\x03\xFD\x41\x03\xDD\x41\x3B\xDC\x0F\x8C\x9A\xFE\xFF\xFF", "xxxxxxxx????xxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				var unkFuncForPedModelIndices = (byte*)(*(int*)(address + 8) + address + 12);
				s_cStreamingAppropriatePedIndicesOffset = *(int*)(unkFuncForPedModelIndices + 0x1E);
			}

			address = FindPatternBmh("\x48\x8B\x05\x00\x00\x00\x00\x41\x8B\x1E", "xxx????xxx");
			s_weaponAndAmmoInfoArrayPtr = (RageAtArrayPtr*)(*(int*)(address + 3) + address + 7);

			address = FindPatternBmh("\x84\xC0\x74\x20\x48\x8B\x47\x40\x48\x85\xC0\x74\x08\x8B\xB0\x00\x00\x00\x00\xEB\x02\x33\xF6\x48\x8D\x4D\x48\xE8", "xxxxxxxxxxxxxxx????xxxxxxxxx");
			s_weaponInfoHumanNameHashOffset = *(int*)(address + 15);

			address = FindPatternBmh("\x8B\x05\x00\x00\x00\x00\x44\x8B\xD3\x8D\x48\xFF", "xx????xxxxxx");
			if (address != null)
			{
				s_weaponComponentArrayCountAddr = (uint*)(*(int*)(address + 2) + address + 6);

				address = FindPatternNaive("\x46\x8D\x04\x11\x48\x8D\x15\x00\x00\x00\x00\x41\xD1\xF8", "xxxxxxx????xxx", new IntPtr(address));
				s_offsetForCWeaponComponentArrayAddr = (ulong)(address + 7);

				address = FindPatternNaive("\x74\x10\x49\x8B\xC9\xE8", "xxxxxx", new IntPtr(address));
				var findAttachPointFuncAddr = new IntPtr((long)(*(int*)(address + 6) + address + 10));

				address = FindPatternNaive("\x4C\x8D\x81", "xxx", findAttachPointFuncAddr);
				s_weaponAttachPointsStartOffset = *(int*)(address + 3);
				address = FindPatternNaive("\x4D\x63\x98", "xxx", new IntPtr(address));
				s_weaponAttachPointsArrayCountOffset = *(int*)(address + 3);
				address = FindPatternNaive("\x4C\x63\x50", "xxx", new IntPtr(address));
				s_weaponAttachPointElementComponentCountOffset = *(byte*)(address + 3);
				address = FindPatternNaive("\x48\x83\xC0", "xxx", new IntPtr(address));
				s_weaponAttachPointElementSize = *(byte*)(address + 3);
			}

			address = FindPatternBmh("\x24\x1F\x3C\x05\x0F\x85\x00\x00\x00\x00\x48\x8D\x82\x00\x00\x00\x00\x48\x8D\xB2\x00\x00\x00\x00\x48\x85\xC0\x74\x09\x80\x38\x00\x74\x04\x8A\xCB", "xxxxxx????xxx????xxx????xxxxxxxxxxxx");
			if (address != null)
			{
				s_vehicleMakeNameOffsetInModelInfo = *(int*)(address + 13);
			}

			address = FindPatternBmh("\x66\x89\x44\x24\x38\x8B\x44\x24\x38\x8B\xC8\x33\x4C\x24\x30\x81\xE1\x00\x00\xFF\x0F\x33\xC1\x0F\xBA\xF0\x1D\x8B\xC8\x33\x4C\x24\x30\x23\xCB\x33\xC1", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_pedPersonalityIndexOffsetInModelInfo = *(int*)(address + 0x42);
				s_pedPersonalitiesArrayAddr = (ulong*)(*(int*)(address + 0x49) + address + 0x4D);
			}

			address = FindPatternBmh("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
			if (address != null)
			{
				int pedIntelligenceOffset = *(int*)(address + 0x11);
				PedPlayerInfoOffset = pedIntelligenceOffset + 0x8;
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
				int isWantedStarFlashingOffset = *(int*)(address + 0x2);
				// Flags for ignoring player are actually read/written as a byte in the game code, but make this value 4-byte aligned because SetBit and IsBitSet reads/writes as an int value
				CWantedIgnorePlayerFlagOffset = isWantedStarFlashingOffset - 3;

				CWantedLastTimeSearchAreaRefocusedOffset = isWantedStarFlashingOffset - 0x23;
				CWantedLastTimeSpottedByPoliceOffset = CWantedLastTimeSearchAreaRefocusedOffset + 0x4;
				CWantedStartTimeOfHiddenEvasionOffset = CWantedLastTimeSearchAreaRefocusedOffset + 0xC;
			}
			address = FindPatternBmh("\xEB\x26\x8B\x87\x00\x00\x00\x00\x25\x00\xF8\xFF\xFF\xC1\xE0\x12", "xxxx????xxxxxxxx");
			if (address != null)
			{
				s_activateSpecialAbilityFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr(*(int*)(address + 0x24) + address + 0x28));
			}

			int gameVersion = GetGameVersion();
			// Two special ability slots are available in b2060 and later versions
			if (gameVersion >= 59)
			{
				address = FindPatternBmh("\x0F\x84\x49\x01\x00\x00\x33\xD2\xE8", "xxxxxxxxx");
				if (address != null)
				{
					s_getSpecialAbilityAddressFunc = (delegate* unmanaged[Stdcall]<IntPtr, int, IntPtr>)(new IntPtr(*(int*)(address + 9) + address + 13));
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

			address = FindPatternBmh("\x48\x8B\x87\x00\x00\x00\x00\x48\x85\xC0\x0F\x84\x8B\x00\x00\x00", "xxx????xxxxxxxxx");
			if (address != null)
			{
				s_objParentEntityAddressDetachedFromOffset = *(int*)(address + 3);
			}

			address = FindPatternBmh("\x48\x8D\x1D\x00\x00\x00\x00\x4C\x8B\x0B\x4D\x85\xC9\x74\x67", "xxx????xxxxxxxx");
			if (address != null)
			{
				s_projectilePoolAddress = (ulong*)(*(int*)(address + 3) + address + 7);
			}
			// Find address of the projectile count, just in case the max number of projectile changes from 50
			address = FindPatternBmh("\x44\x8B\x0D\x00\x00\x00\x00\x33\xDB\x45\x8A\xF8", "xxx????xxxxx");
			if (address != null)
			{
				s_projectileCountAddress = (int*)(*(int*)(address + 3) + address + 7);
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
				s_explodeProjectileFunc = (delegate* unmanaged[Stdcall]<IntPtr, int, void>)(new IntPtr(*(int*)(address + 13) + address + 17));
			}

			address = FindPatternBmh("\x0F\x84\x8F\x00\x00\x00\x8A\x48\x28\x80\xE9\x02\x80\xF9\x03\x0F\x87\x80\x00\x00\x00\x48\x8B\x10\x48\x8B\xC8\xFF\x52", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
			if (address != null)
			{
				// The offset is 0x78 in b2944, and one more v func addition before this func makes us have to create another memory pattern/signature
				s_getFragInstVFuncOffset = *(sbyte*)(address + 0x1D);
			}
			address = FindPatternNaive("\x0F\xBE\x5E\x06\x48\x8B\xCF\xFF\x50\x00\x8B\xD3\x48\x8B\xC8\xE8\x00\x00\x00\x00\x8B\x4E", "xxxxxxxxx?xxxxxx????xx");
			if (address != null)
			{
				s_detachFragmentPartByIndexFunc = (delegate* unmanaged[Stdcall]<FragInst*, int, FragInst*>)(new IntPtr(*(int*)(address + 16) + address + 20));
			}
			address = FindPatternBmh("\x74\x56\x48\x8B\x0D\x00\x00\x00\x00\x41\x0F\xB7\xD0\x45\x33\xC9\x45\x33\xC0", "xxxxx????xxxxxxxxxx");
			if (address != null)
			{
				s_phSimulatorInstPtr = (ulong**)(*(int*)(address + 5) + address + 9);
			}
			address = FindPatternBmh("\xC0\xE8\x07\xA8\x01\x74\x57\x0F\xB7\x4E\x18\x85\xC9\x78\x4F", "xxxxxxxxxxxxxxx");
			if (address != null)
			{
				s_colliderCapacityOffset = *(int*)(address - 0x41);
				s_colliderCountOffset = s_colliderCapacityOffset + 4;
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
					byte[] nopBytes = Enumerable.Repeat((byte)0x90, bytesToWriteInstructions).ToArray();
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
				byte[] nopBytes = Enumerable.Repeat((byte)0x90, bytesToWriteInstructions).ToArray();
				Marshal.Copy(nopBytes, 0, new IntPtr(address), bytesToWriteInstructions);
			}
			#endregion

			// Generate vehicle model list
			var vehicleHashesGroupedByClass = new List<int>[0x20];
			for (int i = 0; i < 0x20; i++)
			{
				vehicleHashesGroupedByClass[i] = new List<int>();
			}

			var vehicleHashesGroupedByType = new List<int>[0x10];
			for (int i = 0; i < 0x10; i++)
			{
				vehicleHashesGroupedByType[i] = new List<int>();
			}

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

			for (int i = 0; i < s_modelHashEntries; i++)
			{
				for (HashNode* cur = ((HashNode**)s_modelHashTable)[i]; cur != null; cur = cur->next)
				{
					ushort data = cur->data;
					bool bitTest = ((*(int*)(s_modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
					if (data >= s_modelNum1 || !bitTest)
					{
						continue;
					}

					ulong addr1 = s_modelNum4 + s_modelNum3 * data;
					if (addr1 == 0)
					{
						continue;
					}

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
								{
									continue;
								}

								vehicleHashesGroupedByClass[*(byte*)(addr2 + vehicleClassOffset) & 0x1F].Add(cur->hash);

								// Normalize the value to vehicle type range for b944 or later versions if current game version is earlier than b944.
								// The values for CAmphibiousAutomobile and CAmphibiousQuadBike were inserted between those for CSubmarineCar and CHeli in b944.
								int vehicleTypeInt = *(int*)((byte*)addr2 + s_vehicleTypeOffsetInModelInfo);
								if (gameVersion < 28 && vehicleTypeInt >= 6)
								{
									vehicleTypeInt += 2;
								}

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
			for (int i = 0; i < 0x20; i++)
			{
				vehicleResult[i] = Array.AsReadOnly(vehicleHashesGroupedByClass[i].ToArray());
			}

			VehicleModels = Array.AsReadOnly(vehicleResult);

			vehicleResult = new ReadOnlyCollection<int>[0x10];
			for (int i = 0; i < 0x10; i++)
			{
				vehicleResult[i] = Array.AsReadOnly(vehicleHashesGroupedByType[i].ToArray());
			}

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
			string enableCarsGlobalMask = gameVersion >= 46 ? "x??xxxx??xxxxx?xx????xxxx?x" : "xx??xxxxxx?xx????xxxx?x";
			int enableCarsGlobalOffset = gameVersion >= 46 ? 17 : 13;

			int codepageCount = shopControllerHeader->CodePageCount();
			for (int i = 0; i < codepageCount; i++)
			{
				int size = shopControllerHeader->GetCodePageSize(i);
				if (size <= 0)
				{
					continue;
				}

				address = FindPatternNaive(enableCarsGlobalPattern, enableCarsGlobalMask, shopControllerHeader->GetCodePageAddress(i), (ulong)size);
				if (address == null)
				{
					continue;
				}

				int globalIndex = *(int*)(address + enableCarsGlobalOffset) & 0xFFFFFF;
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
			return StringMarshal.PtrToStringUtf8(address);
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
			float* data = (float*)address.ToPointer();
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
			byte* data = (byte*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 16-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt16(IntPtr address, short value)
		{
			short* data = (short*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes an unsigned single 16-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteUInt16(IntPtr address, ushort value)
		{
			ushort* data = (ushort*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single 32-bit value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteInt32(IntPtr address, int value)
		{
			int* data = (int*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a single floating-point value to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The value to write.</param>
		public static void WriteFloat(IntPtr address, float value)
		{
			float* data = (float*)address.ToPointer();
			*data = value;
		}
		/// <summary>
		/// Writes a 4x4 floating-point matrix to the specified <paramref name="address"/>.
		/// </summary>
		/// <param name="address">The memory address to access.</param>
		/// <param name="value">The elements of the matrix in row major arrangement to write.</param>
		public static void WriteMatrix(IntPtr address, float[] value)
		{
			float* data = (float*)(address.ToPointer());
			for (int i = 0; i < value.Length; i++)
			{
				data[i] = value[i];
			}
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
			long* data = (long*)address.ToPointer();
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
			{
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");
			}

			int* data = (int*)address.ToPointer();
			if (value)
			{
				*data |= (1 << bit);
			}
			else
			{
				*data &= ~(1 << bit);
			}
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
			{
				throw new ArgumentOutOfRangeException(nameof(bit), "The bit index has to be between 0 and 31");
			}

			int* data = (int*)address.ToPointer();
			return (*data & (1 << bit)) != 0;
		}

		public static IntPtr String { get; private set; } // "~a~"
		public static IntPtr NullString { get; private set; } // ""
		public static IntPtr CellEmailBcon { get; private set; } // "~a~~a~~a~~a~~a~~a~~a~~a~~a~~a~"

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

		#region -- fwExtensibleBase RTTI Sytem --

		/// <summary>
		/// Calls rage::fwExtensibleBase::GetClassId on a GTA class that is a subclass of rage::fwExtensibleBase.
		/// </summary>
		public static uint GetRageClassId(IntPtr addr)
		{
			ulong* vTable = *(ulong**)addr;

			// In the b2802 or a later exe, the function returns a hash value (not a pointer value)
			if (GetGameVersion() >= 80)
			{
				// The function is for the game version b2802 or later ones.
				// This one directly returns a hash value (not a pointer value) unlike the previous function.
				var getClassNameHashFunc = (delegate* unmanaged[Stdcall]<uint>)(vTable[2]);
				return getClassNameHashFunc();
			}

			// The function is for game versions prior to b2802.
			// The function uses rax and rdx registers in newer versions prior to b2802 (probably since b2189), and it uses only rax register in older versions.
			// The function returns the address where the class name hash is in all versions prior to (the address will be the outVal address in newer versions).
			var getClassNameAddressHashFunc = (delegate* unmanaged[Stdcall]<ulong, uint*, uint*>)(vTable[2]);

			uint outVal = 0;
			uint* returnValueAddress = getClassNameAddressHashFunc(0, &outVal);
			return *returnValueAddress;
		}

		#endregion

		#region -- Cameras --

		private static ulong* s_cameraPoolAddress;
		private static ulong* s_gameplayCameraAddress;

		public static IntPtr GetCameraAddress(int handle)
		{
			uint index = (uint)(handle >> 8);
			ulong poolAddr = *s_cameraPoolAddress;
			if (*(byte*)(index + *(long*)(poolAddr + 8)) == (byte)(handle & 0xFF))
			{
				return new IntPtr(*(long*)poolAddr + (index * *(uint*)(poolAddr + 20)));
			}
			return IntPtr.Zero;

		}
		public static IntPtr GetGameplayCameraAddress()
		{
			return new IntPtr((long)*s_gameplayCameraAddress);
		}

		#endregion

		#region -- Game Data --

		// Performs ASCII uppercase to ASCII lowercase and backslash to slash conversion, does not perform any convertions to non-ASCII characters.
		// Use this table because character conversion with this table performs faster than calculating converted characters using branch jump instructions.
		// The former method is used in GTA5.exe and the latter one is used in GTAIV.exe.
		private static readonly byte[] s_lookupTableForGetHashKey =
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
			foreach (byte c in Encoding.UTF8.GetBytes(str))
			{
				hash += s_lookupTableForGetHashKey[c];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}
		public static uint GetHashKeyAscii(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}

			uint hash = 0;
			foreach (byte c in Encoding.ASCII.GetBytes(str))
			{
				hash += s_lookupTableForGetHashKey[c];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);

			return hash;
		}
		// You can find the equivalent function of the method below with "EB 15 0F BE C0 48 FF C1"
		public static uint GetHashKeyAsciiNoPreConversion(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}

			uint hash = 0;
			foreach (byte c in Encoding.ASCII.GetBytes(str))
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

		private static ulong s_getLabelTextByHashAddress;
		private static delegate* unmanaged[Stdcall]<ulong, int, ulong> s_getLabelTextByHashFunc;

		public static string GetGxtEntryByHash(int entryLabelHash)
		{
			char* entryText = (char*)s_getLabelTextByHashFunc(s_getLabelTextByHashAddress, entryLabelHash);
			return entryText != null ? StringMarshal.PtrToStringUtf8(new IntPtr(entryText)) : string.Empty;
		}

		#endregion

		#region -- YSC Script Data --

		[StructLayout(LayoutKind.Explicit)]
		private struct YscScriptHeader
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
		private struct YscScriptTableItem
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
		private struct YscScriptTable
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

		#region -- Decorator Data --

		private static byte* s_isDecoratorLocked;

		public static bool IsDecoratorLocked
		{
			get => *s_isDecoratorLocked != 0;
			set => *s_isDecoratorLocked = (byte)(value ? 1 : 0);
		}

		#endregion

		#region -- World Data --

		private static int* s_cursorSpriteAddr;

		public static int CursorSprite => *s_cursorSpriteAddr;

		private static float* s_timeScaleAddress;

		public static float TimeScale => *s_timeScaleAddress;

		private static int* s_millisecondsPerGameMinuteAddress;

		public static int MillisecondsPerGameMinute
		{
			set => *s_millisecondsPerGameMinuteAddress = value;
		}

		private static byte* s_isClockPausedAddress;

		public static bool IsClockPaused => *s_isClockPausedAddress != 0;

		private static int* s_lastClockTickAddress;

		public static int LastTimeClockTicked
		{
			get => *s_lastClockTickAddress;
			set => *s_lastClockTickAddress = value;
		}

		private static float* s_readWorldGravityAddress;
		private static float* s_writeWorldGravityAddress;

		public static float WorldGravity
		{
			get => *s_readWorldGravityAddress;
			set => *s_writeWorldGravityAddress = value;
		}

		#endregion

		#region -- Skeleton Data --

		private static CrSkeleton* GetCrSkeletonFromEntityHandle(int handle)
		{
			IntPtr entityAddress = GetEntityAddress(handle);
			if (entityAddress == IntPtr.Zero)
			{
				return null;
			}

			return GetCrSkeletonOfEntity(entityAddress);
		}

		private static CrSkeleton* GetCrSkeletonOfEntity(IntPtr entityAddress)
		{
			FragInst* fragInst = GetFragInstAddressOfEntity(entityAddress);
			// Return value will not be null if the entity is a CVehicle or a CPed
			if (fragInst != null)
			{
				return GetEntityCrSkeletonOfFragInst(fragInst);
			}

			ulong unkAddr = *(ulong*)(entityAddress + 80);
			if (unkAddr == 0)
			{
				return null;
			}

			return (CrSkeleton*)*(ulong*)(unkAddr + 40);
		}

		private static CrSkeleton* GetEntityCrSkeletonOfFragInst(FragInst* fragInst)
		{
			FragCacheEntry* fragCacheEntry = fragInst->fragCacheEntry;
			GtaFragType* gtaFragType = fragInst->gtaFragType;

			// Check if either pointer is null just like native functions that take a bone index argument
			if (fragCacheEntry == null || gtaFragType == null)
			{
				return null;
			}

			return fragCacheEntry->crSkeleton;
		}

		public static int GetBoneIdForEntityBoneIndex(int entityHandle, int boneIndex)
		{
			if (boneIndex < 0)
			{
				return -1;
			}

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
			{
				return -1;
			}

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

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
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

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
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
			{
				return null;
			}

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(entityHandle);
			if (crSkeleton == null)
			{
				return null;
			}

			return crSkeleton->skeletonData->GetBoneName(boneIndex);
		}
		public static int GetEntityBoneCount(int handle)
		{
			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			return crSkeleton != null ? crSkeleton->boneCount : 0;
		}
		public static IntPtr GetEntityBoneTransformMatrixAddress(int handle)
		{
			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
			{
				return IntPtr.Zero;
			}

			return crSkeleton->GetTransformMatrixAddress();
		}
		public static IntPtr GetEntityBoneObjectMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
			{
				return IntPtr.Zero;
			}

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
			{
				return IntPtr.Zero;
			}

			if (boneIndex < crSkeleton->boneCount)
			{
				return crSkeleton->GetBoneObjectMatrixAddress((boneIndex));
			}

			return IntPtr.Zero;
		}
		public static IntPtr GetEntityBoneGlobalMatrixAddress(int handle, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
			{
				return IntPtr.Zero;
			}

			CrSkeleton* crSkeleton = GetCrSkeletonFromEntityHandle(handle);
			if (crSkeleton == null)
			{
				return IntPtr.Zero;
			}

			if (boneIndex < crSkeleton->boneCount)
			{
				return crSkeleton->GetBoneGlobalMatrixAddress(boneIndex);
			}

			return IntPtr.Zero;
		}

		#endregion

		#region -- CEntity Functions --

		private static delegate* unmanaged[Stdcall]<float*, ulong, int, float*> s_getRotationFromMatrixFunc;
		private static delegate* unmanaged[Stdcall]<float*, ulong, int> s_getQuaternionFromMatrixFunc;

		public static void GetRotationFromMatrix(float* returnRotationArray, IntPtr matrixAddress, int rotationOrder = 2)
		{
			s_getRotationFromMatrixFunc(returnRotationArray, (ulong)matrixAddress.ToInt64(), rotationOrder);

			const float rad2Deg = 57.2957763671875f; // 0x42652EE0 in hex. Exactly the same value as the GET_ENTITY_ROTATION multiplies the rotation values in radian by.
			returnRotationArray[0] *= rad2Deg;
			returnRotationArray[1] *= rad2Deg;
			returnRotationArray[2] *= rad2Deg;
		}
		public static void GetQuaternionFromMatrix(float* returnRotationArray, IntPtr matrixAddress)
		{
			s_getQuaternionFromMatrixFunc(returnRotationArray, (ulong)matrixAddress.ToInt64());
		}

		#endregion

		#region -- CPhysical Offsets --

		public static int EntityMaxHealthOffset { get; }
		public static int SetAngularVelocityVFuncOfEntityOffset { get; }
		public static int GetAngularVelocityVFuncOfEntityOffset { get; }

		public static int CAttackerArrayOfEntityOffset { get; }
		public static int ElementCountOfCAttackerArrayOfEntityOffset { get; }
		public static int ElementSizeOfCAttackerArrayOfEntity { get; }

		#endregion

		#region -- CPhysical Functions --

		internal sealed class SetEntityAngularVelocityTask : IScriptTask
		{
			#region Fields

			private IntPtr _entityAddress;
			// return value will be the address of the temporary 4 float storage
			private delegate* unmanaged[Stdcall]<IntPtr, float*, void> _setAngularVelocityDelegate;
			private float _x, _y, _z;
			#endregion

			internal SetEntityAngularVelocityTask(IntPtr entityAddress, delegate* unmanaged[Stdcall]<IntPtr, float*, void> vFuncDelegate, float x, float y, float z)
			{
				this._entityAddress = entityAddress;
				this._setAngularVelocityDelegate = vFuncDelegate;
				this._x = x;
				this._y = y;
				this._z = z;
			}

			public void Run()
			{
				float* angularVelocity = stackalloc float[4];
				angularVelocity[0] = _x;
				angularVelocity[1] = _y;
				angularVelocity[2] = _z;

				_setAngularVelocityDelegate(_entityAddress, angularVelocity);
			}
		}

		public static float* GetEntityAngularVelocity(IntPtr entityAddress)
		{
			ulong vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)GetAngularVelocityVFuncOfEntityOffset);
			var getEntityAngularVelocity = (delegate* unmanaged[Stdcall]<IntPtr, float*>)(vFuncAddr);

			return getEntityAngularVelocity(entityAddress);
		}

		public static void SetEntityAngularVelocity(IntPtr entityAddress, float x, float y, float z)
		{
			ulong vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)SetAngularVelocityVFuncOfEntityOffset);
			var setEntityAngularVelocityDelegate = (delegate* unmanaged[Stdcall]<IntPtr, float*, void>)(vFuncAddr);

			var task = new SetEntityAngularVelocityTask(entityAddress, setEntityAngularVelocityDelegate, x, y, z);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
		}



		#endregion

		#region -- CPhysical Data --

		// the size is at least 0x10 in all game versions
		[StructLayout(LayoutKind.Explicit, Size = 0x10)]
		private struct CAttacker
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
			if (NativeMemory.CAttackerArrayOfEntityOffset == 0 ||
				NativeMemory.ElementCountOfCAttackerArrayOfEntityOffset == 0 ||
				NativeMemory.ElementSizeOfCAttackerArrayOfEntity == 0)
			{
				return false;
			}

			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + NativeMemory.CAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
			{
				return false;
			}

			int entryCount = *(int*)((byte*)entityCAttackerArrayAddress + NativeMemory.ElementCountOfCAttackerArrayOfEntityOffset);

			return index < entryCount;
		}

		private static EntityDamageRecordForReturnValue GetEntityDamageRecordEntryAtIndexInternal(ulong cAttackerArrayAddress, uint index)
		{
			var cAttacker = (CAttacker*)((byte*)cAttackerArrayAddress + index * NativeMemory.ElementSizeOfCAttackerArrayOfEntity);

			ulong attackerEntityAddress = cAttacker->attackerEntityAddress;
			int weaponHash = cAttacker->weaponHash;
			int gameTime = cAttacker->gameTime;
			int attackerHandle = attackerEntityAddress != 0 ? GetEntityHandleFromAddress(new IntPtr((long)attackerEntityAddress)) : 0;

			return new EntityDamageRecordForReturnValue(attackerHandle, weaponHash, gameTime);
		}
		public static EntityDamageRecordForReturnValue GetEntityDamageRecordEntryAtIndex(IntPtr entityAddress, uint index)
		{
			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + NativeMemory.CAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
			{
				return default(EntityDamageRecordForReturnValue);
			}

			return GetEntityDamageRecordEntryAtIndexInternal(entityCAttackerArrayAddress, index);
		}

		public static EntityDamageRecordForReturnValue[] GetEntityDamageRecordEntries(IntPtr entityAddress)
		{
			if (NativeMemory.CAttackerArrayOfEntityOffset == 0 ||
				NativeMemory.ElementCountOfCAttackerArrayOfEntityOffset == 0 ||
				NativeMemory.ElementSizeOfCAttackerArrayOfEntity == 0)
			{
				return Array.Empty<EntityDamageRecordForReturnValue>();
			}

			ulong entityCAttackerArrayAddress = *(ulong*)(entityAddress + NativeMemory.CAttackerArrayOfEntityOffset).ToPointer();

			if (entityCAttackerArrayAddress == 0)
			{
				return Array.Empty<EntityDamageRecordForReturnValue>();
			}

			int returnEntrySize = *(int*)((byte*)entityCAttackerArrayAddress + NativeMemory.ElementCountOfCAttackerArrayOfEntityOffset);
			EntityDamageRecordForReturnValue[] returnEntries = returnEntrySize != 0 ? new EntityDamageRecordForReturnValue[returnEntrySize] : Array.Empty<EntityDamageRecordForReturnValue>();

			for (uint i = 0; i < returnEntries.Length; i++)
			{
				returnEntries[i] = GetEntityDamageRecordEntryAtIndexInternal(entityCAttackerArrayAddress, i);
			}

			return returnEntries;
		}

		public static bool EntityRecordsCollision(int entityHandle)
		{
			IntPtr entityAddress = GetEntityAddress(entityHandle);
			if (entityAddress == IntPtr.Zero)
			{
				return false;
			}

			return CPhysicalRecordsCollision(entityAddress);
		}

		public static bool CPhysicalRecordsCollision(IntPtr cPhysicalAddress)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return false;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x47;
			return *(byte*)(cPhysicalAddress + offsetToRead) != 0;
		}

		public static bool HasEntityCollidedWithBuildingOrAnimatedBuilding(int entityHandle)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return false;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x8;
			return GetTargetCEntityAddressCollidingWith(entityHandle, offsetToRead) != IntPtr.Zero;
		}

		public static int GetVehicleHandleEntityIsCollidingWith(int entityHandle)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return 0;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x10;
			return GetPhysicalEntityHandleEntityIsCollidingWith(entityHandle, offsetToRead);
		}

		public static int GetPedHandleEntityIsCollidingWith(int entityHandle)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return 0;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x18;
			return GetPhysicalEntityHandleEntityIsCollidingWith(entityHandle, offsetToRead);
		}

		// maybe there's another collision record entry for CObject, but we're not sure about this
		public static int GetPropHandleEntityIsCollidingWith(int entityHandle)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return 0;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x20;
			return GetPhysicalEntityHandleEntityIsCollidingWith(entityHandle, offsetToRead);
		}

		public static int GetPhysicalEntityHandleFromLastCollisionEntryOfEntity(int entityHandle)
		{
			if (CAttackerArrayOfEntityOffset == 0)
			{
				return 0;
			}

			int offsetToRead = CAttackerArrayOfEntityOffset + 0x30;
			IntPtr targetCEntityAddress = GetTargetCEntityAddressCollidingWith(entityHandle, offsetToRead);

			if (targetCEntityAddress == IntPtr.Zero)
			{
				return 0;
			}

			var targetCEntityType = (EntityTypeInternal)(*(byte*)(targetCEntityAddress + 0x28));
			switch (targetCEntityType)
			{
				case EntityTypeInternal.Vehicle:
				case EntityTypeInternal.Ped:
				case EntityTypeInternal.Object:
					return GetEntityHandleFromAddress(targetCEntityAddress);
				default:
					return 0;
			}
		}

		private static int GetPhysicalEntityHandleEntityIsCollidingWith(int entityHandle, int offsetOfCollisionRecord)
		{
			IntPtr targetCEntityAddress = GetTargetCEntityAddressCollidingWith(entityHandle, offsetOfCollisionRecord);
			if (targetCEntityAddress == IntPtr.Zero)
			{
				return 0;
			}

			return GetEntityHandleFromAddress(targetCEntityAddress);
		}

		private static IntPtr GetTargetCEntityAddressCollidingWith(int entityHandle, int offsetOfCollisionRecord)
		{
			IntPtr entityAddress = GetEntityAddress(entityHandle);
			if (entityAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			if (!CPhysicalRecordsCollision(entityAddress))
			{
				return IntPtr.Zero;
			}

			long** collisionRecord = *(long***)(entityAddress + offsetOfCollisionRecord);
			if (collisionRecord == null)
			{
				return IntPtr.Zero;
			}

			long* targetCEntityAddress = *collisionRecord;
			if (targetCEntityAddress == null)
			{
				return IntPtr.Zero;
			}

			return new IntPtr(targetCEntityAddress);
		}

		private enum EntityTypeInternal
		{
			Invalid = 0,
			Building = 1,
			AnimatedBuilding = 2,
			Vehicle = 3,
			Ped = 4,
			Object = 5
		}

		#endregion

		public static unsafe class Vehicle
		{
			#region -- Vehicle Offsets --

			public static int NextGearOffset { get; }
			public static int GearOffset { get; }
			public static int HighGearOffset { get; }

			public static int CurrentRpmOffset { get; }
			public static int ClutchOffset { get; }
			public static int AccelerationOffset { get; }

			public static int CVehicleEngineOffset { get; }
			public static int CVehicleEngineTurboOffset { get; }

			public static int FuelLevelOffset { get; }
			public static int OilLevelOffset { get; }

			public static int VehicleTypeOffset { get; }

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

			public static int ProvidesCoverOffset { get; }

			public static int LightsMultiplierOffset { get; }

			public static int IsInteriorLightOnOffset { get; }
			public static int IsEngineStartingOffset { get; }

			public static int IsWantedOffset { get; }

			public static int IsHeadlightDamagedOffset { get; }

			public static int PreviouslyOwnedByPlayerOffset { get; }
			public static int NeedsToBeHotwiredOffset { get; }

			public static int AlarmTimeOffset { get; }

			public static int LodMultiplierOffset { get; }

			public static int CanUseSirenOffset { get; }
			public static int HasMutedSirensOffset { get; }
			public static int HasMutedSirensBit { get; }

			public static int SirenBufferOffset { get; }

			public static int DropsMoneyWhenBlownUpOffset { get; }

			public static int HeliBladesSpeedOffset { get; }

			public static int HeliMainRotorHealthOffset { get; }
			public static int HeliTailRotorHealthOffset { get; }
			public static int HeliTailBoomHealthOffset { get; }

			public static int HandlingDataOffset { get; }

			public static int SubHandlingDataArrayOffset { get; }

			public static int ModelSirenIdOffset { get; }

			public static int FirstVehicleFlagsOffset { get; }

			static Vehicle()
			{
				byte* address;

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
					CurrentRpmOffset = *(int*)(address + 10);
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

				int gameVersion = GetGameVersion();

				// Get the offset that is stored by MODIFY_VEHICLE_TOP_SPEED if the game version is b944 or later for existing script compatibility
				// MODIFY_VEHICLE_TOP_SPEED stores the 2nd argument value to CVehicle in b944 or later, but that's not the case in earlier ones
				if (gameVersion >= 28)
				{

					address = FindPatternBmh("\x48\x89\x5C\x24\x28\x44\x0F\x29\x40\xC8\x0F\x28\xF9\x44\x0F\x29\x48\xB8\xF3\x0F\x11\xB9", "xxxxxxxxxxxxxxxxxxxxxx");
					if (address != null)
					{
						int modifyVehicleTopSpeedOffset1 = *(int*)(address - 4);
						int modifyVehicleTopSpeedOffset2 = *(int*)(address + 22);
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
					ProvidesCoverOffset = *(int*)(address + 23);
				}

				address = FindPatternBmh("\x74\x03\x41\x8A\xF4\xF3\x44\x0F\x59\x93\x00\x00\x00\x00\x48\x8B\xCB\xF3\x44\x0F\x59\x97\xFC\x00\x00\x00\xF3\x44\x0F\x59\xD6", "xxxxxxxxxx????xxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					LightsMultiplierOffset = *(int*)(address + 10);
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
						int unkVehHeadlightOffset = *(int*)(address + 3);
						byte* unkFuncAddr = (*(int*)(address + 8) + address + 12);
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
					LodMultiplierOffset = *(int*)(address - 4);
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
					LodMultiplierOffset = *(int*)(address - 4);
				}

				address = FindPatternNaive("\x48\x85\xC0\x74\x32\x8A\x88\x00\x00\x00\x00\xF6\xC1\x00\x75\x27", "xxxxxxx????xx?xx");
				if (address != null)
				{
					HasMutedSirensOffset = *(int*)(address + 7);
					HasMutedSirensBit = *(byte*)(address + 13); // the bit is changed between b372 and b2802
					CanUseSirenOffset = *(int*)(address + 23);
				}
				address = FindPatternBmh("\xC1\xE9\x1C\xC0\xE1\x06\x32\xC8\x80\xE1\x40\x32\xC8\x88\x8E", "xxxxxxxxxxxxxxx");
				if (address != null)
				{
					ModelSirenIdOffset = *(int*)(address + 0x1F);
					SirenBufferOffset = *(int*)(address + 0x26);
				}

				address = FindPatternBmh("\x41\xBB\x07\x00\x00\x00\x8A\xC2\x41\x23\xCB\x41\x22\xC3\x3C\x03\x75\x16", "xxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					VehicleTypeOffset = *(int*)(address - 0x1C);
				}

				address = FindPatternBmh("\x83\xB8\x00\x00\x00\x00\x00\x77\x12\x80\xA0\x00\x00\x00\x00\xFD\x80\xE3\x01\x02\xDB\x08\x98", "xx?????xxxx????xxxxxxxx");
				if (address != null)
				{
					DropsMoneyWhenBlownUpOffset = *(int*)(address + 11);
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
					WheelSteeringLimitMultiplierOffset = *(int*)(address + 12);
					WheelSuspensionStrengthOffset = WheelSteeringLimitMultiplierOffset - 4;
				}

				address = FindPatternBmh("\xF3\x0F\x5C\xC8\x0F\x2F\xCB\xF3\x0F\x11\x89\x00\x00\x00\x00\x72\x10\xF3\x0F\x10\x1D", "xxxxxxxxxxx????xxxxxx");
				if (address != null)
				{
					WheelTemperatureOffset = *(int*)(address + 11);
				}

				address = FindPatternBmh("\x74\x13\x0F\x57\xC0\x0F\x2E\x80", "xxxxxxxx");
				if (address != null)
				{
					TireHealthOffset = *(int*)(address + 8);
					WheelHealthOffset = TireHealthOffset - 4;
				}

				// the tire wear multipiler value for vehicle wheels is present only in b1868 or newer versions
				if (gameVersion >= 54)
				{
					address = FindPatternNaive("\x45\x84\xF6\x74\x08\xF3\x0F\x59\x0D\x00\x00\x00\x00\xF3\x0F\x10\x83", "xxxxxxxxx????xxxx");
					if (address != null)
					{
						TireWearRateOffset = *(int*)(address + 0x22);

						// The values for SET_TYRE_WEAR_RATE_SCALE and SET_TYRE_MAXIMUM_GRIP_DIFFERENCE_DUE_TO_WEAR_RATE are not present in b1868
						if (gameVersion >= 59)
						{
							WheelMaxGripDiffDueToWearRateOffset = TireWearRateOffset + 4;
							TireWearRateScaleOffset = TireWearRateOffset + 8;
						}
					}
				}

				address = FindPatternBmh("\x0F\xBF\x88\x00\x00\x00\x00\x3B\xCA\x74\x17", "xxx????xxxx");
				if (address != null)
				{
					WheelIdOffset = *(int*)(address + 3);
				}

				address = FindPatternNaive("\xEB\x02\x33\xC9\xF6\x81\x00\x00\x00\x00\x01\x75\x43", "xxxxxx????xxx");
				if (address != null)
				{
					WheelTouchingFlagsOffset = *(int*)(address + 6);
				}

				address = FindPatternBmh("\x74\x21\x8B\xD7\x48\x8B\xCB\xE8\x00\x00\x00\x00\x48\x8B\xC8\xE8", "xxxxxxxx????xxxx");
				if (address != null)
				{
					s_fixVehicleWheelFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr(*(int*)(address + 16) + address + 20));
					address = FindPatternNaive("\x80\xA1\x00\x00\x00\x00\xFD", "xx????x", new IntPtr(address + 20));
					ShouldShowOnlyVehicleTiresWithPositiveHealthOffset = *(int*)(address + 2);
				}

				// Vehicle Wheels has the owner vehicle pointer and new wheel functions are used since b1365
				if (gameVersion >= 40)
				{
					address = FindPatternBmh("\x4C\x8B\x81\x28\x01\x00\x00\x0F\x29\x70\xE8\x0F\x29\x78\xD8", "xxxxxxxxxxxxxxx");
					s_punctureVehicleTireNewFunc = (delegate* unmanaged[Stdcall]<IntPtr, ulong, float, ulong, ulong, int, byte, bool, void>)(new IntPtr((long)(address - 0x10)));
					address = FindPatternBmh("\x48\x83\xEC\x50\x48\x8B\x81\x00\x00\x00\x00\x48\x8B\xF1\xF6\x80", "xxxxxxx????xxxxx");
					s_burstVehicleTireOnRimNewFunc = (delegate* unmanaged[Stdcall]<IntPtr, void>)(new IntPtr((long)(address - 0xB)));
				}
				else
				{
					address = FindPatternBmh("\x41\xF6\x81\x00\x00\x00\x00\x20\x0F\x29\x70\xE8\x0F\x29\x78\xD8\x49\x8B\xF9", "xxx????xxxxxxxxxxxx");
					s_punctureVehicleTireOldFunc = (delegate* unmanaged[Stdcall]<IntPtr, ulong, float, IntPtr, ulong, ulong, int, byte, bool, void>)(new IntPtr((long)(address - 0x14)));
					address = FindPatternBmh("\x48\x83\xEC\x50\xF6\x82\x00\x00\x00\x00\x20\x48\x8B\xF2\x48\x8B\xE9", "xxxxxx????xxxxxxx");
					s_burstVehicleTireOnRimOldFunc = (delegate* unmanaged[Stdcall]<IntPtr, IntPtr, void>)(new IntPtr((long)(address - 0x10)));
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
			}

			public static IntPtr GetSubHandlingData(IntPtr handlingDataAddr, int handlingType)
			{
				var subHandlingArray = (RageAtArrayPtr*)(handlingDataAddr + SubHandlingDataArrayOffset);
				ushort subHandlingCount = subHandlingArray->size;
				if (subHandlingCount <= 0)
				{
					return IntPtr.Zero;
				}

				for (int i = 0; i < subHandlingCount; i++)
				{
					ulong subHandlingDataAddr = subHandlingArray->GetElementAddress(i);
					if (subHandlingDataAddr == 0)
					{
						continue;
					}

					ulong vFuncAddr = *(ulong*)(*(ulong*)subHandlingDataAddr + (uint)0x10);
					var getSubHandlingDataVFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(vFuncAddr);
					int handlingTypeOfCurrentElement = getSubHandlingDataVFunc(subHandlingDataAddr);
					if (handlingTypeOfCurrentElement == handlingType)
					{
						return new IntPtr((long)subHandlingDataAddr);
					}
				}

				return IntPtr.Zero;
			}

			public static float GetTurbo(int handle)
			{
				if (CVehicleEngineTurboOffset == 0)
				{
					return 0f;
				}

				byte* vehEngineStructAddr = GetCVehicleEngine(handle);

				if (vehEngineStructAddr == null)
				{
					return 0f;
				}

				return *(float*)(vehEngineStructAddr + CVehicleEngineTurboOffset);
			}
			public static void SetTurbo(int handle, float value)
			{
				if (CVehicleEngineTurboOffset == 0)
				{
					return;
				}

				byte* vehEngineStructAddr = GetCVehicleEngine(handle);

				if (vehEngineStructAddr == null)
				{
					return;
				}

				*(float*)(vehEngineStructAddr + CVehicleEngineTurboOffset) = value;
			}

			private static byte* GetCVehicleEngine(int handle)
			{
				IntPtr address = GetEntityAddress(handle);

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
				IntPtr address = GetEntityAddress(vehicleHandle);

				if (address == IntPtr.Zero)
				{
					return false;
				}

				return (*(byte*)(address + HasMutedSirensOffset) & HasMutedSirensBit) != 0;
			}

			public static bool HasSiren(int vehicleHandle)
			{
				IntPtr address = GetEntityAddress(vehicleHandle);

				if (address == IntPtr.Zero)
				{
					return false;
				}

				IntPtr modelAddress = GetModelInfo(address);

				if (modelAddress == IntPtr.Zero)
				{
					return false;
				}

				// The siren id must not be zero to use the siren
				return (*(ulong**)(address + SirenBufferOffset) != null) && GetByteSirenIdOfVehicleModel(modelAddress) != 0;
			}

			public static int GetByteSirenIdOfVehicleModel(IntPtr vehicleModelAddress)
			{
				// This implementation doesn't consider real siren ID values generated by SirenSetting Limit Adjuster by cp702, but no need to consider as valid siren IDs will not be zero even with the adjuster installed
				// Raw carcols.meta and carvariations.meta files must be used for siren ID that exceeds 0xFF since carcols.ymt and carvariations.ymt can contain siren ID as uint8_t value
				// (SirenSetting Limit Adjuster modifies the siren ID type to uint16_t type during runtime)
				// Do not complain CodeWalker for carcols.ymt and carvariations.ymt not supporting siren ID that exceeds 0xFF, that is an expected behavior
				return *(byte*)(vehicleModelAddress + ModelSirenIdOffset);
			}

			#endregion

			#region -- Vehicle Wheel Data --

			private static delegate* unmanaged[Stdcall]<IntPtr, void> s_fixVehicleWheelFunc;
			private static delegate* unmanaged[Stdcall]<IntPtr, ulong, float, ulong, ulong, int, byte, bool, void> s_punctureVehicleTireNewFunc;
			private static delegate* unmanaged[Stdcall]<IntPtr, ulong, float, IntPtr, ulong, ulong, int, byte, bool, void> s_punctureVehicleTireOldFunc;
			private static delegate* unmanaged[Stdcall]<IntPtr, void> s_burstVehicleTireOnRimNewFunc;
			private static delegate* unmanaged[Stdcall]<IntPtr, IntPtr, void> s_burstVehicleTireOnRimOldFunc;

			public static int WheelSteeringLimitMultiplierOffset { get; }

			public static int WheelSuspensionStrengthOffset { get; }

			public static int WheelTemperatureOffset { get; }

			public static int WheelHealthOffset { get; }

			public static int TireHealthOffset { get; }

			public static int TireWearRateOffset { get; }
			/// <summary>
			/// This value only affects how fast a vehicle tire health decreases, which is different from
			/// <see cref="TireWearRateOffset"/>.
			/// </summary>
			public static int WheelMaxGripDiffDueToWearRateOffset { get; }
			public static int TireWearRateScaleOffset { get; }

			// the on fire offset is the same as this offset
			public static int WheelTouchingFlagsOffset { get; }

			public static int WheelIdOffset { get; }

			public static int ShouldShowOnlyVehicleTiresWithPositiveHealthOffset { get; }

			public static void FixVehicleWheel(IntPtr wheelAddress) => s_fixVehicleWheelFunc(wheelAddress);

			public static IntPtr GetVehicleWheelAddressByIndexOfWheelArray(IntPtr vehicleAddress, int index)
			{
				ulong* vehicleWheelArrayAddr = *(ulong**)(vehicleAddress + WheelPtrArrayOffset);

				if (vehicleWheelArrayAddr == null)
				{
					return IntPtr.Zero;
				}

				return new IntPtr((long)*(vehicleWheelArrayAddr + index));
			}

			public static bool IsWheelTouchingSurface(IntPtr wheelAddress, IntPtr vehicleAddress)
			{
				if (WheelTouchingFlagsOffset == 0)
				{
					return false;
				}

				uint wheelTouchingFlag = *(uint*)(wheelAddress + WheelTouchingFlagsOffset).ToPointer();
				if ((wheelTouchingFlag & 1) != 0)
				{
					return true;
				}

				#region Slower Check
				if (((wheelTouchingFlag >> 1) & 1) == 0)
				{
					return false;
				}

				ulong phCollider = *(ulong*)(*(ulong*)(vehicleAddress + 0x50).ToPointer() + 0x50);
				if (phCollider == 0)
				{
					return true;
				}

				ulong unkStructAddr = *(ulong*)(phCollider + 0x18);
				if (unkStructAddr == 0)
				{
					return false;
				}

				return (*(uint*)(unkStructAddr + 0x14) & 0xFFFFFFFD) == 0;
				#endregion
			}

			private static bool VehicleWheelHasVehiclePtr() => GetGameVersion() >= 40;

			public static void PunctureTire(IntPtr wheelAddress, float damage, IntPtr vehicleAddress)
			{
				var task = new VehicleWheelPunctureTask(wheelAddress, vehicleAddress, false, damage);
				ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
			}

			public static void BurstTireOnRim(IntPtr wheelAddress, IntPtr vehicleAddress)
			{
				var task = new VehicleWheelPunctureTask(wheelAddress, vehicleAddress, true);
				ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
			}

			// the function BurstVehicleTireOnRimNew(Old)Func calls must be called in the main thread or the game will crash
			// the function PunctureVehicleTireNew(Old)Func calls should be called in the main thread or the game might crash in some cases
			internal sealed class VehicleWheelPunctureTask : IScriptTask
			{
				#region Fields

				private IntPtr _wheelAddress;
				private IntPtr _vehicleAddress;
				private bool _burstWheelCompletely;
				private float _damage;
				#endregion

				internal VehicleWheelPunctureTask(IntPtr wheelAddress, IntPtr vehicleAddress, bool burstWheelCompletely, float damage = 1000f)
				{
					this._wheelAddress = wheelAddress;
					this._vehicleAddress = vehicleAddress;
					this._burstWheelCompletely = burstWheelCompletely;
					this._damage = damage;
				}

				public void Run()
				{
					int outValInt;
					float outValFloat;

					if (VehicleWheelHasVehiclePtr())
					{
						s_punctureVehicleTireNewFunc(_wheelAddress, 0, _damage, (ulong)&outValInt, (ulong)&outValFloat, 3, 0, true);
						if (_burstWheelCompletely)
						{
							s_burstVehicleTireOnRimNewFunc(_wheelAddress);
						}
					}
					else
					{
						s_punctureVehicleTireOldFunc(_wheelAddress, 0, _damage, _vehicleAddress, (ulong)&outValInt, (ulong)&outValFloat, 3, 0, true);
						if (_burstWheelCompletely)
						{
							s_burstVehicleTireOnRimOldFunc(_wheelAddress, _vehicleAddress);
						}
					}
				}
			}

			#endregion
		}


		#region -- Prop Data --

		private static int s_objParentEntityAddressDetachedFromOffset;

		private static IntPtr GetParentEntityOfPropDetachedFrom(int objHandle)
		{
			IntPtr entityAddress = GetEntityAddress(objHandle);
			if (s_objParentEntityAddressDetachedFromOffset == 0 || entityAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr(*(long*)(entityAddress + s_objParentEntityAddressDetachedFromOffset));
		}
		public static int GetParentEntityHandleOfPropDetachedFrom(int objHandle)
		{
			IntPtr parentEntityAddr = GetParentEntityOfPropDetachedFrom(objHandle);
			if (parentEntityAddr == IntPtr.Zero)
			{
				return 0;
			}

			return GetEntityHandleFromAddress(parentEntityAddr);
		}
		public static bool HasPropBeenDetachedFromParentEntity(int objHandle) => GetParentEntityOfPropDetachedFrom(objHandle) != IntPtr.Zero;

		#endregion -- Prop Data --

		public static unsafe class Ped
		{
			static Ped()
			{
				byte* address;

				address = FindPatternBmh("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
				if (address != null)
				{
					PedIntelligenceOffset = *(int*)(address + 0x11);

					byte* setDecisionMakerHashFuncAddr = *(int*)(address + 0x18) + address + 0x1C;
					PedIntelligenceDecisionMakerHashOffset = *(int*)(setDecisionMakerHashFuncAddr + 0x1C);

					IntentoryOfCPedOffset = PedIntelligenceOffset + 0x10;
					UnkStateOffset = PedIntelligenceOffset - 0x10;
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

				address = FindPatternBmh("\x74\x51\x48\x8D\x81\x00\x00\x00\x00\x48\x85\xC0\x74\x43\x48\x8B\x00\x48\x85\xC0\x74\x0A", "xxxxx????xxxxxxxxxxxxx");
				if (address != null)
				{
					PedIntelligenceCTaskInfoOffset = *(int*)(address + 0x5);
					PedIntelligenceCombatTargetPedAddressOffset = PedIntelligenceCTaskInfoOffset + 0x18;
					PedIntelligenceCurrentScriptTaskHashOffset = PedIntelligenceCTaskInfoOffset + 0x20;
					PedIntelligenceCurrentScriptTaskStatusOffset = PedIntelligenceCTaskInfoOffset + 0x24;
				}

				address = FindPatternNaive("\x48\x83\xEC\x28\x48\x8B\x42\x00\x48\x85\xC0\x74\x09\x48\x3B\x82\x00\x00\x00\x00\x74\x21", "xxxxxxx?xxxxxxxx????xx");
				if (address != null)
				{
					int fragInstNmGtaOffset = *(int*)(address + 16);
					KnockOffVehicleTypeOffset = s_fragInstNmGtaOffset + 0xC;
				}

				address = FindPatternBmh("\x76\x20\xEB\x17\x76\x1C\xF3\x0F\x59\xE1\xF3\x0F\x5C\xC4\x0F\x2F\xC2", "xxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					LowerWetnessLevelOffset = *(int*)(address - 4);
					UpperWetnessLevelOffset = LowerWetnessLevelOffset + 4;
					LowerWetnessHeightOffset = LowerWetnessLevelOffset - 8;
					UpperWetnessHeightOffset = LowerWetnessLevelOffset - 4;

					// this may look too risky, but this offset fetching do work in b372, b2699, and b2845
					IsUsingWetEffectOffset = *(int*)(address + 0x85);
				}

				address = FindPatternBmh("\x0F\x93\xC0\x84\xC0\x74\x0F\xF3\x41\x0F\x58\xD1\x41\x0F\x2F\xD0\x72\x04\x41\x0F\x28\xD0", "xxxxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					SweatOffset = *(int*)(address + 26);
				}

				address = FindPatternBmh("\x8A\x41\x30\xC0\xE8\x03\xA8\x01\x74\x49\x8B\x82", "xxxxxxxxxxxx");
				if (address != null)
				{
					IsInVehicleOffset = *(int*)(address + 12);
					LastVehicleOffset = *(int*)(address + 0x1A);
				}
				address = FindPatternBmh("\x24\x3F\x0F\xB6\xC0\x66\x89\x87", "xxxxxxxx");
				if (address != null)
				{
					SeatIndexOffset = *(int*)(address + 8);
				}

				address = FindPatternBmh("\x84\xC0\x0F\x84\x2C\x01\x00\x00\x48\x8D\x9F\x00\x00\x00\x00\x48\x8B\x0B\x48\x3B\xCE\x74\x1B\x48\x85\xC9\x74\x08\x48\x8B\xD3\xE8", "xxxxxxxxxxx????xxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					VehicleStandingOnOffset = *(int*)(address + 11);
				}

				address = FindPatternBmh("\xC1\xE8\x11\xA8\x01\x75\x10\x48\x8B\xCB\xE8\x00\x00\x00\x00\x84\xC0\x0F\x84", "xxxxxxxxxxx????xxxx");
				if (address != null)
				{
					DropsWeaponsWhenDeadOffset = *(int*)(address - 4);
				}

				address = FindPatternBmh("\x4D\x8B\xF1\x48\x8B\xFA\xC1\xE8\x02\x48\x8B\xF1\xA8\x01\x0F\x85\xEB\x00\x00\x00", "xxxxxxxxxxxxxxxxxxxx");
				if (address != null)
				{
					SuffersCriticalHitOffset = *(int*)(address - 4);
				}

				int gameVersion = GetGameVersion();

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
					TimeOfDeathOffset = *(int*)(address + 2);
					CauseOfDeathOffset = TimeOfDeathOffset - 4;
					SourceOfDeathOffset = TimeOfDeathOffset - 12;
				}

				address = FindPatternNaive("\x74\x08\x8B\x81\x00\x00\x00\x00\xEB\x0D\x48\x8B\x87\x00\x00\x00\x00\x8B\x80", "xxxx????xxxxx????xx");
				if (address != null)
				{
					FiringPatternOffset = *(int*)(address + 19);
				}

				address = FindPatternBmh("\x48\x85\xC0\x74\x7F\xF6\x80\x00\x00\x00\x00\x02\x75\x76", "xxxxxxx????xxx");
				if (address != null)
				{
					byte* setDecisionMakerHashFuncAddr = *(int*)(address + 0x18) + address + 0x1C;
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
			}

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
			public static int GetLastVehicleHandle(IntPtr pedAddress)
			{
				if (LastVehicleOffset == 0)
				{
					return 0;
				}

				var lastVehicleAddress = new IntPtr(*(long*)(pedAddress + LastVehicleOffset));
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
				if (IsInVehicleOffset == 0 || LastVehicleOffset == 0)
				{
					return 0;
				}

				uint bitFlags = *(uint*)(pedAddress + IsInVehicleOffset);
				bool isPedInVehicle = ((bitFlags & (1 << 0x1E)) != 0);
				if (!isPedInVehicle)
				{
					return 0;
				}

				var lastVehicleAddress = new IntPtr(*(long*)(pedAddress + LastVehicleOffset));
				return lastVehicleAddress != IntPtr.Zero ? GetEntityHandleFromAddress(lastVehicleAddress) : 0;
			}

			/// <summary>
			/// Gets the vehicle the ped is standing on.
			/// </summary>
			public static int GetVehicleHandlePedIsStandingOn(IntPtr pedAddress)
			{
				if (VehicleStandingOnOffset == 0)
				{
					return 0;
				}

				var vehicleStandingOnAddress = new IntPtr(*(long*)(pedAddress + VehicleStandingOnOffset));
				return vehicleStandingOnAddress != IntPtr.Zero ? GetEntityHandleFromAddress(vehicleStandingOnAddress) : 0;
			}

			#endregion

			#region -- Ped Offsets --

			public static int LowerWetnessHeightOffset { get; }
			public static int UpperWetnessHeightOffset { get; }
			public static int LowerWetnessLevelOffset { get; }
			public static int UpperWetnessLevelOffset { get; }

			public static int IsUsingWetEffectOffset { get; }

			public static int SweatOffset { get; }

			/// <summary>
			/// The value at this offset should be 2 if the ped is a player ped.
			/// </summary>
			public static int UnkStateOffset { get; }

			public static int IntentoryOfCPedOffset { get; }

			// the same offset as the offset for SET_PED_CAN_BE_TARGETTED
			public static int DropsWeaponsWhenDeadOffset { get; }

			public static int SuffersCriticalHitOffset { get; }

			public static int ArmorOffset { get; }

			public static int InjuryHealthThresholdOffset { get; }
			public static int FatalInjuryHealthThresholdOffset { get; }

			public static int IsInVehicleOffset { get; }
			public static int LastVehicleOffset { get; }
			public static int SeatIndexOffset { get; }

			public static int VehicleStandingOnOffset { get; }

			public static int SourceOfDeathOffset { get; }
			public static int CauseOfDeathOffset { get; }
			public static int TimeOfDeathOffset { get; }

			public static int KnockOffVehicleTypeOffset { get; }

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

			public static int CTaskTreePedOffset { get; }

			public static int CEventCountOffset { get; }

			public static int CEventStackOffset { get; }

			public static int PedIntelligenceCTaskInfoOffset { get; }

			public static int PedIntelligenceCombatTargetPedAddressOffset { get; }

			public static int PedIntelligenceCurrentScriptTaskHashOffset { get; }
			public static int PedIntelligenceCurrentScriptTaskStatusOffset { get; }

			#endregion

			#endregion

			#region -- CPedIntelligence Data --

			public static IntPtr GetCPedIntelligence(IntPtr pedAddress)
				=> PedIntelligenceOffset != 0 ? new IntPtr(*(long*)(pedAddress + PedIntelligenceOffset)) : IntPtr.Zero;

			public static int GetCombatTargetPedHandleFromCombatPed(IntPtr pedAddress)
			{
				if (PedIntelligenceCombatTargetPedAddressOffset == 0)
				{
					return 0;
				}

				IntPtr pedIntelligence = GetCPedIntelligence(pedAddress);
				if (pedIntelligence == IntPtr.Zero)
				{
					return 0;
				}

				// Actually, the game tests the value at [CTaskInfo + 0x8] using the AND and shr bitwise operations
				// and tests if the value at [CTaskInfo + 0xC] is the task index for CTaskCombat before accessing the member of the target ped pointer
				// In practice, however, it looks like we can use the target address without testing the 2 checks

				var targetPedAddress = new IntPtr(*(long*)(pedIntelligence + PedIntelligenceCombatTargetPedAddressOffset));
				if (targetPedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return GetEntityHandleFromAddress(targetPedAddress);
			}

			public static int GetCombatTargetPedHandleFromCombatPed(int pedHandle)
			{
				if (PedIntelligenceCombatTargetPedAddressOffset == 0)
				{
					return 0;
				}

				IntPtr pedAddress = GetEntityAddress(pedHandle);
				if (pedAddress == IntPtr.Zero)
				{
					return 0;
				}

				IntPtr pedIntelligence = GetCPedIntelligence(pedAddress);
				if (pedIntelligence == IntPtr.Zero)
				{
					return 0;
				}

				// Actually, the game tests the value at [CTaskInfo + 0x8] using the AND and shr bitwise operations
				// and tests if the value at [CTaskInfo + 0xC] is the task index for CTaskCombat before accessing the member of the target ped pointer
				// In practice, however, it looks like we can use the target address without testing the 2 checks

				var targetPedAddress = new IntPtr(*(long*)(pedIntelligence + PedIntelligenceCombatTargetPedAddressOffset));
				if (targetPedAddress == IntPtr.Zero)
				{
					return 0;
				}

				return GetEntityHandleFromAddress(targetPedAddress);
			}

			public static void GetScriptTaskHashAndStatus(int pedHandle, out uint taskHash, out uint taskStatus)
			{
				taskHash = 0x811E343C; // the hashed value of SCRIPT_TASK_INVALID, hardcoded in a lot of places
				taskStatus = 3; // the vacant status, hardcoded nearby most of the places where the hashed value of SCRIPT_TASK_INVALID is hardcoded
				if (PedIntelligenceCurrentScriptTaskHashOffset == 0 || PedIntelligenceCurrentScriptTaskStatusOffset == 0)
				{
					return;
				}

				IntPtr pedAddress = GetEntityAddress(pedHandle);
				if (pedAddress == IntPtr.Zero)
				{
					return;
				}

				IntPtr pedIntelligence = GetCPedIntelligence(pedAddress);
				if (pedIntelligence == IntPtr.Zero)
				{
					return;
				}

				taskHash = *(uint*)(pedIntelligence + PedIntelligenceCurrentScriptTaskHashOffset);
				taskStatus = *(uint*)(pedIntelligence + PedIntelligenceCurrentScriptTaskStatusOffset);
			}

			#endregion

			#region -- CPedInventory Data --

			public static IntPtr GetCPedInventoryAddressFromPedHandle(int pedHandle)
			{
				if (IntentoryOfCPedOffset == 0)
				{
					return IntPtr.Zero;
				}

				IntPtr cPedAddress = GetEntityAddress(pedHandle);
				if (cPedAddress == IntPtr.Zero)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(*(long**)(cPedAddress + IntentoryOfCPedOffset));
			}

			public static uint[] GetAllWeaponHashesOfPedInventory(int pedHandle)
			{
				RageAtArrayPtr* weaponInventoryArray = GetWeaponInventoryArrayOfCPedInventory(pedHandle);
				if (weaponInventoryArray == null)
				{
					return Array.Empty<uint>();
				}

				ushort weaponInventoryCount = weaponInventoryArray->size;
				var result = new List<uint>(weaponInventoryCount);
				for (int i = 0; i < weaponInventoryCount; i++)
				{
					ulong itemAddress = weaponInventoryArray->GetElementAddress(i);
					ItemInfo* weaponInfo = *(ItemInfo**)(itemAddress + 0x8);
					if (weaponInfo != null)
					{
						result.Add(weaponInfo->nameHash);
					}
				}

				return result.ToArray();
			}

			public static bool TryGetWeaponHashInPedInventoryBySlotHash(int pedHandle, uint slotHash, out uint weaponHash)
			{
				RageAtArrayPtr* weaponInventoryArray = GetWeaponInventoryArrayOfCPedInventory(pedHandle);
				if (weaponInventoryArray == null)
				{
					weaponHash = 0;
					return false;
				}

				int arraySize = weaponInventoryArray->size;
				if (arraySize == 0)
				{
					weaponHash = 0;
					return false;
				}

				int low = 0, high = arraySize - 1;
				while (true)
				{
					unsafe
					{
						int indexToRead = (low + high) >> 1;
						ulong currentItem = weaponInventoryArray->GetElementAddress(indexToRead);

						uint slotHashOfCurrentItem = *(uint*)(currentItem);
						ItemInfo* weaponInfo = *(ItemInfo**)(currentItem + 0x8);
						if (slotHashOfCurrentItem == slotHash && weaponInfo != null)
						{
							weaponHash = weaponInfo->nameHash;
							return true;
						}

						// The array is sorted in ascending order
						if (slotHashOfCurrentItem <= slotHash)
						{
							low = indexToRead + 1;
						}
						else
						{
							high = indexToRead - 1;
						}

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
				if (IntentoryOfCPedOffset == 0)
				{
					return null;
				}

				IntPtr cPedAddress = GetEntityAddress(pedHandle);
				if (cPedAddress == IntPtr.Zero)
				{
					return null;
				}

				ulong cPedInventoryAddress = *(ulong*)(cPedAddress + IntentoryOfCPedOffset);
				if (cPedInventoryAddress == 0)
				{
					return null;
				}

				return (RageAtArrayPtr*)(cPedInventoryAddress + 0x18);
			}

			#endregion
		}

		#region -- Screen Data --

		[StructLayout(LayoutKind.Explicit, Size = 0x30)]
		internal struct ScreenInfo
		{
			// these fields should be in pixel coordinates
			[FieldOffset(0x14)]
			internal uint left;
			[FieldOffset(0x18)]
			internal uint right;
			[FieldOffset(0x1C)]
			internal uint top;
			[FieldOffset(0x20)]
			internal uint bottom;
		}

		private static int* s_physicalScrenWidthAddr;
		private static int* s_physicalScrenHeightAddr;
		private static IntPtr s_screenInfoAddr;
		/// <remarks>
		/// May need to be called in the main thread if the game is using multiple screens.
		/// </remarks>
		private static delegate* unmanaged[Stdcall]<IntPtr, IntPtr> s_unkScreenFunc;
		/// <remarks>
		/// Returns only either 0 or 1.
		/// </remarks>
		private static delegate* unmanaged[Stdcall]<IntPtr, bool> s_isUsingMultiScreenFunc;
		private static delegate* unmanaged[Stdcall]<IntPtr, ScreenInfo*> s_getMainScreenInfoFunc;

		internal sealed class GetMainWindowResoltionTask : IScriptTask
		{
			#region Fields
			internal Size resolutionResult;
			#endregion

			public void Run()
			{
				resolutionResult = new Size(*s_physicalScrenWidthAddr, *s_physicalScrenHeightAddr);

				IntPtr generalScreenInfoAddr = s_unkScreenFunc(s_screenInfoAddr);
				if (s_isUsingMultiScreenFunc(generalScreenInfoAddr))
				{
					// A lot of functions call this function twice for some reason, so we call it twice for safely
					generalScreenInfoAddr = s_unkScreenFunc(s_screenInfoAddr);
					ScreenInfo* screenInfoAddr = s_getMainScreenInfoFunc(generalScreenInfoAddr);

					resolutionResult = new Size(
						(int)(screenInfoAddr->right - screenInfoAddr->left),
						(int)(screenInfoAddr->bottom - screenInfoAddr->top)
						);
				}
			}
		}

		public static Size GetMainWindowResolution()
		{
			var task = new GetMainWindowResoltionTask();
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
			return task.resolutionResult;
		}

		#endregion

		#region -- Model Info --

		[StructLayout(LayoutKind.Sequential)]
		private struct HashNode
		{
			internal int hash;
			internal ushort data;
			internal ushort padding;
			internal HashNode* next;
		}

		private enum ModelInfoClassType
		{
			Invalid = 0,
			Object = 1,
			Mlo = 2,
			Time = 3,
			Weapon = 4,
			Vehicle = 5,
			Ped = 6
		}

		private enum VehicleStructClassType
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
		private struct CModelList
		{
			[FieldOffset(0x0)]
			internal fixed uint modelMemberIndices[0x100];
		}

		[StructLayout(LayoutKind.Explicit, Size = 0xB8)]
		private struct PedPersonality
		{
			[FieldOffset(0x7C)]
			internal bool isMale;
			[FieldOffset(0x7D)]
			internal bool isHuman;
			[FieldOffset(0x7F)]
			internal bool isGang;
		}

		private static int s_vehicleMakeNameOffsetInModelInfo;
		private static int s_vehicleTypeOffsetInModelInfo;
		private static int s_handlingIndexOffsetInModelInfo;
		private static int s_pedPersonalityIndexOffsetInModelInfo;
		private static UInt32 s_modelNum1;
		private static UInt64 s_modelNum2;
		private static UInt64 s_modelNum3;
		private static UInt64 s_modelNum4;
		private static UInt64 s_modelHashTable;
		private static UInt16 s_modelHashEntries;
		private static ulong* s_modelInfoArrayPtr;
		private static ulong* s_pedPersonalitiesArrayAddr;

		private static ulong* s_cStreamingAddr;
		private static int s_cStreamingAppropriateVehicleIndicesOffset;
		private static int s_cStreamingAppropriatePedIndicesOffset;

		private static IntPtr FindCModelInfo(int modelHash)
		{
			for (HashNode* cur = ((HashNode**)s_modelHashTable)[(uint)(modelHash) % s_modelHashEntries]; cur != null; cur = cur->next)
			{
				if (cur->hash != modelHash)
				{
					continue;
				}

				ushort data = cur->data;
				bool bitTest = ((*(int*)(s_modelNum2 + (ulong)(4 * data >> 5))) & (1 << (data & 0x1F))) != 0;
				if (data >= s_modelNum1 || !bitTest)
				{
					continue;
				}

				ulong addr1 = s_modelNum4 + s_modelNum3 * data;
				if (addr1 == 0)
				{
					continue;
				}

				long* address = (long*)(*(ulong*)(addr1));
				return new IntPtr(address);
			}

			return IntPtr.Zero;
		}

		private static ModelInfoClassType GetModelInfoClass(IntPtr address)
		{
			if (address != IntPtr.Zero)
			{
				return ((ModelInfoClassType)((*(byte*)((ulong)address.ToInt64() + 157) & 0x1F)));
			}

			return ModelInfoClassType.Invalid;
		}

		private static VehicleStructClassType GetVehicleStructClass(IntPtr modelInfoAddress)
		{
			if (GetModelInfoClass(modelInfoAddress) != ModelInfoClassType.Vehicle)
			{
				return VehicleStructClassType.None;
			}

			int typeInt = (*(int*)((byte*)modelInfoAddress.ToPointer() + s_vehicleTypeOffsetInModelInfo));

			// Normalize the value to vehicle type range for b944 or later versions if current game version is earlier than b944.
			// The values for CAmphibiousAutomobile and CAmphibiousQuadBike were inserted between those for CSubmarineCar and CHeli in b944.
			if (GetGameVersion() < 28 && typeInt >= 6)
			{
				typeInt += 2;
			}

			return (VehicleStructClassType)typeInt;

		}
		public static int GetVehicleType(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);

			if (modelInfo == IntPtr.Zero)
			{
				return -1;
			}

			return (int)GetVehicleStructClass(modelInfo);
		}

		private static IntPtr GetModelInfo(IntPtr entityAddress)
		{
			if (entityAddress != IntPtr.Zero)
			{
				return new IntPtr(*(long*)((ulong)entityAddress.ToInt64() + 0x20));
			}

			return IntPtr.Zero;
		}

		private static int GetModelHashFromFwArcheType(IntPtr fwArcheTypeAddress)
		{
			if (fwArcheTypeAddress != IntPtr.Zero)
			{
				return (*(int*)((ulong)fwArcheTypeAddress.ToInt64() + 0x18));
			}

			return 0;
		}
		public static int GetModelHashFromEntity(IntPtr entityAddress)
		{
			if (entityAddress == IntPtr.Zero)
			{
				return 0;
			}

			IntPtr modelInfoAddress = GetModelInfo(entityAddress);
			if (modelInfoAddress != IntPtr.Zero)
			{
				return GetModelHashFromFwArcheType(modelInfoAddress);
			}

			return 0;
		}

		private static bool IsFwArcheTypeAFragment(IntPtr fwArcheTypeAddress)
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
			IntPtr modelInfo = FindCModelInfo(modelHash);
			if (modelInfo == IntPtr.Zero)
			{
				return false;
			}

			return IsFwArcheTypeAFragment(modelInfo);
		}

		private static IntPtr GetModelInfoByIndex(uint index)
		{
			if (s_modelInfoArrayPtr == null || index < 0)
			{
				return IntPtr.Zero;
			}

			ulong modelInfoArrayFirstElemPtr = *s_modelInfoArrayPtr;

			return new IntPtr(*(long*)(modelInfoArrayFirstElemPtr + index * 0x8));
		}
		public static List<int> GetLoadedAppropriateVehicleHashes()
		{
			return GetLoadedHashesOfModelList(s_cStreamingAppropriateVehicleIndicesOffset);
		}
		public static List<int> GetLoadedAppropriatePedHashes()
		{
			return GetLoadedHashesOfModelList(s_cStreamingAppropriatePedIndicesOffset);
		}
		internal static List<int> GetLoadedHashesOfModelList(int startOffsetOfCStreaming)
		{
			if (s_modelInfoArrayPtr == null || s_cStreamingAddr == null)
			{
				return new List<int>();
			}

			var resultList = new List<int>();

			const int maxModelListElementCount = 256;
			var modelSet = (CModelList*)((ulong)s_cStreamingAddr + (uint)startOffsetOfCStreaming);
			for (uint i = 0; i < maxModelListElementCount; i++)
			{
				uint indexOfModelInfo = modelSet->modelMemberIndices[i];

				if (indexOfModelInfo == 0xFFFF)
				{
					break;
				}

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
				return StringMarshal.PtrToStringUtf8(modelInfo + s_vehicleMakeNameOffsetInModelInfo);
			}

			return "CARNOTFOUND";
		}

		public static bool HasVehicleFlag(int modelHash, VehicleFlag1 flag) => HasVehicleFlagInternal(modelHash, (ulong)flag, 0x0);
		public static bool HasVehicleFlag(int modelHash, VehicleFlag2 flag) => HasVehicleFlagInternal(modelHash, (ulong)flag, 0x8);
		private static bool HasVehicleFlagInternal(int modelHash, ulong flag, int flagOffset)
		{
			if (Vehicle.FirstVehicleFlagsOffset == 0)
			{
				return false;
			}

			IntPtr modelInfo = FindCModelInfo(modelHash);

			if (GetModelInfoClass(modelInfo) != ModelInfoClassType.Vehicle)
			{
				return false;
			}

			ulong modelFlags = *(ulong*)(modelInfo + Vehicle.FirstVehicleFlagsOffset + flagOffset).ToPointer();
			return (modelFlags & flag) != 0;
		}

		public static ReadOnlyCollection<int> WeaponModels { get; }
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModels { get; }
		public static ReadOnlyCollection<ReadOnlyCollection<int>> VehicleModelsGroupedByType { get; }
		public static ReadOnlyCollection<int> PedModels { get; }


		private static delegate* unmanaged[Stdcall]<IntPtr, ulong> s_getHandlingDataByHash;
		private static delegate* unmanaged[Stdcall]<int, ulong> s_getHandlingDataByIndex;

		public static IntPtr GetHandlingDataByModelHash(int modelHash)
		{
			IntPtr modelInfo = FindCModelInfo(modelHash);
			if (GetModelInfoClass(modelInfo) != ModelInfoClassType.Vehicle)
			{
				return IntPtr.Zero;
			}

			int handlingIndex = *(int*)(modelInfo + s_handlingIndexOffsetInModelInfo).ToPointer();
			return new IntPtr((long)s_getHandlingDataByIndex(handlingIndex));
		}
		public static IntPtr GetHandlingDataByHandlingNameHash(int handlingNameHash)
		{
			return new IntPtr((long)s_getHandlingDataByHash(new IntPtr(&handlingNameHash)));
		}

		private static PedPersonality* GetPedPersonalityElementAddress(IntPtr modelInfoAddress)
		{
			if (modelInfoAddress == IntPtr.Zero ||
				s_pedPersonalitiesArrayAddr == null ||
				s_pedPersonalityIndexOffsetInModelInfo == 0 ||
				*(ulong*)s_pedPersonalitiesArrayAddr == 0)
			{
				return null;
			}

			if (GetModelInfoClass(modelInfoAddress) != ModelInfoClassType.Ped)
			{
				return null;
			}

			// This values is not likely to be changed in further updates
			const int pedPersonalityElementSize = 0xB8;

			ushort indexOfPedPersonality = *(ushort*)(modelInfoAddress + s_pedPersonalityIndexOffsetInModelInfo).ToPointer();
			return (PedPersonality*)(*(ulong*)s_pedPersonalitiesArrayAddr + (uint)(indexOfPedPersonality * pedPersonalityElementSize));
		}
		public static bool IsModelAMalePed(int modelHash)
		{
			PedPersonality* pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
			{
				return false;
			}

			return pedPersonalityAddress->isMale;
		}
		public static bool IsModelAFemalePed(int modelHash)
		{
			PedPersonality* pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
			{
				return false;
			}

			return !pedPersonalityAddress->isMale;
		}
		public static bool IsModelHumanPed(int modelHash)
		{
			PedPersonality* pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
			{
				return false;
			}

			return pedPersonalityAddress->isHuman;
		}
		public static bool IsModelAnAnimalPed(int modelHash)
		{
			PedPersonality* pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
			{
				return false;
			}

			return !pedPersonalityAddress->isHuman;
		}
		public static bool IsModelAGangPed(int modelHash)
		{
			PedPersonality* pedPersonalityAddress = GetPedPersonalityElementAddress(FindCModelInfo(modelHash));

			if (pedPersonalityAddress == null)
			{
				return false;
			}

			return pedPersonalityAddress->isGang;
		}

		#endregion

		#region -- Entity Pools --

		[StructLayout(LayoutKind.Explicit)]
		private struct FwScriptGuidPool
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
		private struct VehiclePool
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
		private struct GenericPool
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
				uint index = handleUInt >> 8;
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
				{
					return 0;
				}

				ulong offset = address - poolStartAddress;
				if (offset % itemSize != 0)
				{
					return 0;
				}

				uint indexOfPool = (uint)(offset / itemSize);
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

		private static ulong* s_fwScriptGuidPoolAddress;
		private static ulong* s_pedPoolAddress;
		private static ulong* s_objectPoolAddress;
		private static ulong* s_pickupObjectPoolAddress;
		private static ulong* s_vehiclePoolAddress;
		private static ulong* s_buildingPoolAddress;
		private static ulong* s_animatedBuildingPoolAddress;
		private static ulong* s_interiorInstPoolAddress;
		private static ulong* s_interiorProxyPoolAddress;

		private static ulong* s_projectilePoolAddress;
		private static int* s_projectileCountAddress;

		// if the entity is a ped and they are in a vehicle, the vehicle position will be returned instead (just like GET_ENTITY_COORDS does)
		private static delegate* unmanaged[Stdcall]<ulong, float*, ulong> s_entityPosFunc;
		// should be rage::fwScriptGuid::CreateGuid
		private static delegate* unmanaged[Stdcall]<ulong, int> s_createGuid;

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

			internal bool _doPosCheck = false;
			internal bool _doModelCheck = false;
			internal float _radiusSquared;
			internal FVector3? _position;
			internal HashSet<int> _modelHashes;
			internal int[] _resultHandles = Array.Empty<int>();

			#endregion

			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress)
			{
				_poolType = type;
				_poolAddress = poolAddress;
			}
			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress, int[] modelHashes) : this(type, poolAddress)
			{
				if (modelHashes == null || modelHashes.Length <= 0)
				{
					return;
				}

				_doModelCheck = true;
				this._modelHashes = new HashSet<int>(modelHashes);
			}
			internal FwScriptGuidPoolTask(PoolType type, IntPtr poolAddress, FVector3 position, float radiusSquared, int[] modelHashes = null) : this(type, poolAddress)
			{
				_doPosCheck = true;
				this._radiusSquared = radiusSquared;
				this._position = position;

				if (modelHashes == null || modelHashes.Length <= 0)
				{
					return;
				}

				_doModelCheck = true;
				this._modelHashes = new HashSet<int>(modelHashes);
			}

			public void Run()
			{
				if (*NativeMemory.s_fwScriptGuidPoolAddress == 0)
				{
					return;
				}

				var fwScriptGuidPool = (FwScriptGuidPool*)(*NativeMemory.s_fwScriptGuidPoolAddress);

				switch (_poolType)
				{
					case PoolType.Generic:
						var genericPool = (GenericPool*)_poolAddress;
						_resultHandles = GetGuidHandlesFromGenericPool(fwScriptGuidPool, genericPool);
						break;

					case PoolType.Vehicle:
						var vehiclePool = (VehiclePool*)_poolAddress;
						_resultHandles = GetGuidHandlesFromVehiclePool(fwScriptGuidPool, vehiclePool);
						break;

					case PoolType.Projectile:
						int projectilesCount = NativeMemory.GetProjectileCount();
						int projectileCapacity = NativeMemory.GetProjectileCapacity();
						ulong* projectilePoolAddress = (ulong*)_poolAddress;

						_resultHandles = GetGuidHandlesFromProjectilePool(fwScriptGuidPool, projectilePoolAddress, projectilesCount, projectileCapacity);
						break;
				}
			}

			private int[] GetGuidHandlesFromGenericPool(FwScriptGuidPool* fwScriptGuidPool, GenericPool* genericPool)
			{
				var resultList = new List<int>(genericPool->itemCount);

				uint genericPoolSize = genericPool->size;
				for (uint i = 0; i < genericPoolSize; i++)
				{
					if (fwScriptGuidPool->IsFull())
					{
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all entity handles.");
					}

					if (!genericPool->IsValid(i))
					{
						continue;
					}

					ulong address = genericPool->GetAddress(i);

					if (_doPosCheck && !CheckEntityDistance(address, _position.GetValueOrDefault(), _radiusSquared))
					{
						continue;
					}

					if (_doModelCheck && !CheckEntityModel(address, _modelHashes))
					{
						continue;
					}

					int createdHandle = NativeMemory.s_createGuid(address);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			private int[] GetGuidHandlesFromVehiclePool(FwScriptGuidPool* fwScriptGuidPool, VehiclePool* vehiclePool)
			{
				var resultList = new List<int>((int)vehiclePool->itemCount);

				uint poolSize = vehiclePool->size;
				for (uint i = 0; i < poolSize; i++)
				{
					if (fwScriptGuidPool->IsFull())
					{
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all vehicle handles.");
					}

					if (!vehiclePool->IsValid(i))
					{
						continue;
					}

					ulong address = vehiclePool->GetAddress(i);

					if (_doPosCheck && !CheckEntityDistance(address, _position.GetValueOrDefault(), _radiusSquared))
					{
						continue;
					}

					if (_doModelCheck && !CheckEntityModel(address, _modelHashes))
					{
						continue;
					}

					int createdHandle = NativeMemory.s_createGuid(address);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			private int[] GetGuidHandlesFromProjectilePool(FwScriptGuidPool* fwScriptGuidPool, ulong* projectilePool, int itemCount, int maxItemCount)
			{
				int projectilesLeft = itemCount;
				int projectileCapacity = maxItemCount;

				var resultList = new List<int>(itemCount);

				for (uint i = 0; (projectilesLeft > 0 && i < projectileCapacity); i++)
				{
					if (fwScriptGuidPool->IsFull())
					{
						throw new InvalidOperationException("The fwScriptGuid pool is full. The pool must be extended to retrieve all projectile handles.");
					}

					ulong entityAddress = (ulong)ReadAddress(new IntPtr(projectilePool + i)).ToInt64();
					if (entityAddress == 0)
					{
						continue;
					}

					projectilesLeft--;

					if (_doPosCheck && !CheckEntityDistance(entityAddress, _position.GetValueOrDefault(), _radiusSquared))
					{
						continue;
					}

					if (_doModelCheck && !CheckEntityModel(entityAddress, _modelHashes))
					{
						continue;
					}

					int createdHandle = NativeMemory.s_createGuid(entityAddress);
					resultList.Add(createdHandle);
				}

				return resultList.ToArray();
			}

			private static bool CheckEntityDistance(ulong address, FVector3 position, float radiusSquared)
			{
				float* entityPosition = stackalloc float[4];

				NativeMemory.s_entityPosFunc(address, entityPosition);

				float x = position.X - entityPosition[0];
				float y = position.Y - entityPosition[1];
				float z = position.Z - entityPosition[2];

				float distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
				{
					return false;
				}

				return true;
			}

			private static bool CheckEntityModel(ulong address, HashSet<int> modelHashes)
			{
				int modelHash = GetModelHashFromEntity(new IntPtr((long)address));
				if (!modelHashes.Contains(modelHash))
				{
					return false;
				}

				return true;
			}
		}

		internal sealed class GetEntityHandleTask : IScriptTask
		{
			#region Fields
			internal ulong _entityAddress;
			internal int _returnEntityHandle;
			#endregion

			internal GetEntityHandleTask(IntPtr entityAddress)
			{
				this._entityAddress = (ulong)entityAddress.ToInt64();
			}

			public void Run()
			{
				_returnEntityHandle = NativeMemory.s_createGuid(_entityAddress);
			}
		}

		public static int GetVehicleCount()
		{
			if (*s_vehiclePoolAddress == 0)
			{
				return 0;
			}

			VehiclePool* pool = *(VehiclePool**)(*s_vehiclePoolAddress);
			return (int)pool->itemCount;
		}

		public static int GetPedCount() => s_pedPoolAddress != null ? GetGenericPoolCount(*s_pedPoolAddress) : 0;
		public static int GetObjectCount() => s_objectPoolAddress != null ? GetGenericPoolCount(*s_objectPoolAddress) : 0;
		public static int GetPickupObjectCount() => s_pickupObjectPoolAddress != null ? GetGenericPoolCount(*s_pickupObjectPoolAddress) : 0;
		public static int GetBuildingCount() => s_buildingPoolAddress != null ? GetGenericPoolCount(*s_buildingPoolAddress) : 0;
		public static int GetAnimatedBuildingCount() => s_animatedBuildingPoolAddress != null ? GetGenericPoolCount(*s_animatedBuildingPoolAddress) : 0;
		public static int GetInteriorInstCount() => s_interiorInstPoolAddress != null ? GetGenericPoolCount(*s_interiorInstPoolAddress) : 0;
		public static int GetInteriorProxyCount() => s_interiorProxyPoolAddress != null ? GetGenericPoolCount(*s_interiorProxyPoolAddress) : 0;
		public static int GetProjectileCount() => s_projectileCountAddress != null ? *s_projectileCountAddress : 0;

		private static int GetGenericPoolCount(ulong address)
		{
			var pool = (GenericPool*)(address);
			return (int)pool->itemCount;
		}

		public static int GetVehicleCapacity()
		{
			if (*s_vehiclePoolAddress == 0)
			{
				return 0;
			}

			VehiclePool* pool = *(VehiclePool**)(*s_vehiclePoolAddress);
			return (int)pool->size;
		}
		public static int GetPedCapacity() => s_pedPoolAddress != null ? GetGenericPoolCapacity(*s_pedPoolAddress) : 0;
		public static int GetObjectCapacity() => s_objectPoolAddress != null ? GetGenericPoolCapacity(*s_objectPoolAddress) : 0;
		public static int GetPickupObjectCapacity() => s_pickupObjectPoolAddress != null ? GetGenericPoolCapacity(*s_pickupObjectPoolAddress) : 0;
		public static int GetBuildingCapacity() => s_buildingPoolAddress != null ? GetGenericPoolCapacity(*s_buildingPoolAddress) : 0;
		public static int GetAnimatedBuildingCapacity() => s_animatedBuildingPoolAddress != null ? GetGenericPoolCapacity(*s_animatedBuildingPoolAddress) : 0;
		public static int GetInteriorInstCapacity() => s_interiorInstPoolAddress != null ? GetGenericPoolCapacity(*s_interiorInstPoolAddress) : 0;
		public static int GetInteriorProxyCapacity() => s_interiorProxyPoolAddress != null ? GetGenericPoolCapacity(*s_interiorProxyPoolAddress) : 0;
		//the max number of projectile has not been changed from 50
		public static int GetProjectileCapacity() => 50;

		private static int GetGenericPoolCapacity(ulong address)
		{
			var pool = (GenericPool*)(address);
			return (int)pool->size;
		}

		public static int[] GetPedHandles(int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.s_pedPoolAddress, modelHashes);
		}
		public static int[] GetPedHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.s_pedPoolAddress, position, radius, modelHashes);
		}

		public static int[] GetPropHandles(int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.s_objectPoolAddress, modelHashes);
		}
		public static int[] GetPropHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			return GetGuidsInGenericPool(NativeMemory.s_objectPoolAddress, position, radius, modelHashes);
		}

		public static int[] GetEntityHandles()
		{
			int[] vehicleHandles = GetVehicleHandles();
			int[] pedHandles = GetPedHandles();
			int[] propHandles = GetPropHandles();

			return BuildOneArrayFromElementsOfEntityHandleArrays(vehicleHandles, pedHandles, propHandles);
		}
		public static int[] GetEntityHandles(FVector3 position, float radius)
		{
			int[] vehicleHandles = GetVehicleHandles(position, radius);
			int[] pedHandles = GetPedHandles(position, radius);
			int[] propHandles = GetPropHandles(position, radius);

			return BuildOneArrayFromElementsOfEntityHandleArrays(vehicleHandles, pedHandles, propHandles);
		}

		private static int[] BuildOneArrayFromElementsOfEntityHandleArrays(int[] vehicleHandles, int[] pedHandles, int[] propHandles)
		{
			int entityHandleCount = vehicleHandles.Length + pedHandles.Length + propHandles.Length;
			int[] entityHandles = new int[entityHandleCount];

			Array.Copy(vehicleHandles, 0, entityHandles, 0, vehicleHandles.Length);
			Array.Copy(pedHandles, 0, entityHandles, vehicleHandles.Length, pedHandles.Length);
			Array.Copy(propHandles, 0, entityHandles, vehicleHandles.Length + pedHandles.Length, propHandles.Length);

			return entityHandles;
		}

		public static int[] GetVehicleHandles(int[] modelHashes = null)
		{
			if (*NativeMemory.s_vehiclePoolAddress == 0)
			{
				return Array.Empty<int>();
			}

			var vehiclePool = new IntPtr(*(VehiclePool**)(*NativeMemory.s_vehiclePoolAddress));

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Vehicle, vehiclePool, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}
		public static int[] GetVehicleHandles(FVector3 position, float radius, int[] modelHashes = null)
		{
			if (*NativeMemory.s_vehiclePoolAddress == 0)
			{
				return Array.Empty<int>();
			}

			var vehiclePool = new IntPtr(*(VehiclePool**)(*NativeMemory.s_vehiclePoolAddress));

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Vehicle, vehiclePool, position, radius * radius, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}

		public static int[] GetPickupObjectHandles()
		{
			return GetGuidsInGenericPool(NativeMemory.s_pickupObjectPoolAddress);
		}
		public static int[] GetPickupObjectHandles(FVector3 position, float radius)
		{
			return GetGuidsInGenericPool(NativeMemory.s_pickupObjectPoolAddress, position, radius);
		}
		public static int[] GetProjectileHandles()
		{
			if (NativeMemory.s_projectilePoolAddress == null)
			{
				return Array.Empty<int>();
			}

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Projectile, new IntPtr(NativeMemory.s_projectilePoolAddress));
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}
		public static int[] GetProjectileHandles(FVector3 position, float radius)
		{
			if (NativeMemory.s_projectilePoolAddress == null)
			{
				return Array.Empty<int>();
			}

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Projectile, new IntPtr(NativeMemory.s_projectilePoolAddress), position, radius * radius);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}

		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
			{
				return Array.Empty<int>();
			}

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}
		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr, int[] modelHashes)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
			{
				return Array.Empty<int>();
			}

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}
		private static int[] GetGuidsInGenericPool(ulong* ptrOfPoolPtr, FVector3 position, float radius, int[] modelHashes = null)
		{
			var genericPool = new IntPtr((GenericPool*)(*ptrOfPoolPtr));

			if (genericPool == IntPtr.Zero)
			{
				return Array.Empty<int>();
			}

			var task = new FwScriptGuidPoolTask(FwScriptGuidPoolTask.PoolType.Generic, genericPool, position, radius * radius, modelHashes);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._resultHandles;
		}

		public static int[] GetBuildingHandles()
		{
			if (s_buildingPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetHandlesInGenericPool(*NativeMemory.s_buildingPoolAddress);
		}

		public static int[] GetBuildingHandles(FVector3 position, float radius)
		{
			if (s_buildingPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetCEntityHandlesInRange(*NativeMemory.s_buildingPoolAddress, position, radius);
		}

		public static int[] GetAnimatedBuildingHandles()
		{
			if (s_animatedBuildingPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetHandlesInGenericPool(*NativeMemory.s_animatedBuildingPoolAddress);
		}

		public static int[] GetAnimatedBuildingHandles(FVector3 position, float radius)
		{
			if (s_animatedBuildingPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetCEntityHandlesInRange(*NativeMemory.s_animatedBuildingPoolAddress, position, radius);
		}

		public static int[] GetInteriorInstHandles()
		{
			if (s_interiorInstPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetHandlesInGenericPool(*NativeMemory.s_interiorInstPoolAddress);
		}

		public static int[] GetInteriorInstHandles(FVector3 position, float radius)
		{
			if (s_interiorInstPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetCEntityHandlesInRange(*NativeMemory.s_interiorInstPoolAddress, position, radius);
		}

		public static int[] GetInteriorProxyHandles()
		{
			if (s_interiorProxyPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			return GetHandlesInGenericPool(*NativeMemory.s_interiorProxyPoolAddress);
		}

		public static int[] GetInteriorProxyHandles(FVector3 position, float radius)
		{
			if (s_interiorProxyPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			var pool = (GenericPool*)(*NativeMemory.s_interiorProxyPoolAddress);

			// CInteriorProxy is not a subclass of CEntity and position data is placed at different offset
			var returnHandles = new List<int>();
			uint poolSize = pool->size;
			float radiusSquared = radius * radius;
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
				{
					continue;
				}

				ulong address = pool->GetAddress(i);

				float x = *(float*)(address + 0x70) - position.X;
				float y = *(float*)(address + 0x74) - position.Y;
				float z = *(float*)(address + 0x78) - position.Z;

				float distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
				{
					continue;
				}

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		public static bool BuildingHandleExists(int handle) => s_buildingPoolAddress != null ? ((GenericPool*)(*s_buildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool AnimatedBuildingHandleExists(int handle) => s_animatedBuildingPoolAddress != null ? ((GenericPool*)(*s_animatedBuildingPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorInstHandleExists(int handle) => s_interiorInstPoolAddress != null ? ((GenericPool*)(*s_interiorInstPoolAddress))->IsHandleValid(handle) : false;
		public static bool InteriorProxyHandleExists(int handle) => s_interiorProxyPoolAddress != null ? ((GenericPool*)(*s_interiorProxyPoolAddress))->IsHandleValid(handle) : false;

		private static int[] GetHandlesInGenericPool(ulong poolAddress)
		{
			var pool = (GenericPool*)poolAddress;

			var returnHandles = new List<int>(pool->itemCount);
			uint poolSize = pool->size;
			for (uint i = 0; i < poolSize; i++)
			{
				if (pool->IsValid(i))
				{
					returnHandles.Add(pool->GetGuidHandleByIndex(i));
				}
			}

			return returnHandles.ToArray();
		}

		private static int[] GetCEntityHandlesInRange(ulong poolAddress, FVector3 position, float radius)
		{
			var pool = (GenericPool*)poolAddress;

			var returnHandles = new List<int>();
			uint poolSize = pool->size;
			float radiusSquared = radius * radius;
			float* entityPosition = stackalloc float[4];
			for (uint i = 0; i < poolSize; i++)
			{
				if (!pool->IsValid(i))
				{
					continue;
				}

				ulong address = pool->GetAddress(i);

				NativeMemory.s_entityPosFunc(address, entityPosition);
				float x = entityPosition[0] - position.X;
				float y = entityPosition[1] - position.Y;
				float z = entityPosition[2] - position.Z;

				float distanceSquared = (x * x) + (y * y) + (z * z);
				if (distanceSquared > radiusSquared)
				{
					continue;
				}

				returnHandles.Add(pool->GetGuidHandleByIndex(i));
			}

			return returnHandles.ToArray();
		}

		private static int CalculateAppropriateExtendedArrayLength(int[] array, int targetElementCount)
		{
			return (array.Length * 2 > targetElementCount) ? array.Length * 2 : targetElementCount * 2;
		}

		#endregion

		#region -- CPlayerInfo Data --

		private static delegate* unmanaged[Stdcall]<int, ulong> s_getPlayerPedAddressFunc;

		private static bool* s_isGameMultiplayerAddr;

		/// <summary>
		/// The offset for max health of CPlayerInfo, which is stored as an uint16_t.
		/// </summary>
		public static int CPlayerInfoMaxHealthOffset { get; }

		public static int PedPlayerInfoOffset { set; get; }
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

		private static delegate* unmanaged[Stdcall]<IntPtr, void> s_activateSpecialAbilityFunc;

		// The function is for b2060 or later and static offset is for prior to b2060
		private static delegate* unmanaged[Stdcall]<IntPtr, int, IntPtr> s_getSpecialAbilityAddressFunc;
		public static int PlayerPedSpecialAbilityOffset { get; }

		public static IntPtr GetPlayerPedAddress(int playerIndex)
		{
			return new IntPtr((long)s_getPlayerPedAddressFunc(playerIndex));
		}
		public static IntPtr GetLocalPlayerPedAddress()
		{
			return new IntPtr((long)s_getLocalPlayerPedAddressFunc());
		}
		public static int GetPlayerPedHandle(int handle)
		{
			IntPtr playerPedAddress = GetPlayerPedAddress(handle);
			return playerPedAddress != IntPtr.Zero ? GetEntityHandleFromAddress(playerPedAddress) : 0;
		}
		public static int GetLocalPlayerPedHandle()
		{
			IntPtr localPlayerPedAddress = GetLocalPlayerPedAddress();
			return localPlayerPedAddress != IntPtr.Zero ? GetEntityHandleFromAddress(localPlayerPedAddress) : 0;
		}
		public static int GetLocalPlayerIndex()
		{
			if (s_isGameMultiplayerAddr == null || *s_isGameMultiplayerAddr)
			{
				// A fallback path if the variable could not found to make sure the same value will be returned as what PLAYER_ID returns, an extreme edge case if the variable was found
				// You even have to disable SHV to call NETWORK_GET_NUM_CONNECTED_PLAYERS (for preventing the game from going Online) before custom scripts (for enabling multiplayer) can use features for multiplayer
				return GetLocalPlayerIndexViaNativeCall();
			}

			// The same value as what PLAYER_ID returns if the game mode is singleplayer and not multiplayer
			return 0;

			static int GetLocalPlayerIndexViaNativeCall()
			{
				ulong* resultAddr = NativeFunc.Invoke(0x4F8644AF03D0E0D6 /* PLAYER_ID */, null, 0);
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

			IntPtr playerPedAddr = GetPlayerPedAddress(playerIndex);
			if (playerPedAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			long* playerInfoAddr = *(long**)((ulong)playerPedAddr + (uint)PedPlayerInfoOffset);
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

			IntPtr cPlayerInfoAddr = GetCPlayerInfoAddress(playerIndex);
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

			IntPtr cPlayerInfoAddr = GetCPlayerInfoAddress(playerIndex);
			if (cPlayerInfoAddr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			return new IntPtr((long)((ulong)cPlayerInfoAddr + (uint)CWantedOffset));
		}

		public static int GetTargetedBuildingHandleOfPlayer(int playerIndex)
		{
			IntPtr playerPedTargetingAddr = GetCPlayerPedTargetingAddress(playerIndex);
			if (playerPedTargetingAddr == IntPtr.Zero)
			{
				return 0;
			}

			ulong targetedCEntityAddr = *(ulong*)(playerPedTargetingAddr + 0x110);
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
			IntPtr specialAbilityAddr = GetPrimarySpecialAbilityStructAddress(playerIndex);
			if (specialAbilityAddr == IntPtr.Zero)
			{
				return;
			}

			var task = new ActivateSpecialAbilityTask(specialAbilityAddr);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
		}
		public static IntPtr GetPrimarySpecialAbilityStructAddress(int playerIndex)
		{
			IntPtr playerPedAddress = GetPlayerPedAddress(playerIndex);

			if (playerPedAddress == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}

			// Two special ability slots are available in b2060 and later versions
			if (GetGameVersion() >= 59)
			{
				if (s_getSpecialAbilityAddressFunc == null)
				{
					return IntPtr.Zero;
				}

				return s_getSpecialAbilityAddressFunc(playerPedAddress, 0);
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

		internal sealed class ActivateSpecialAbilityTask : IScriptTask
		{
			#region Fields
			internal IntPtr _specialAbilityStructAddress;
			#endregion

			internal ActivateSpecialAbilityTask(IntPtr specialAbilityStructAddress)
			{
				this._specialAbilityStructAddress = specialAbilityStructAddress;
			}

			public void Run()
			{
				s_activateSpecialAbilityFunc(_specialAbilityStructAddress);
			}
		}

		#endregion

		#region -- CPathFind Data --

		public static unsafe class PathFind
		{
			private static ulong s_cPathFindInstanceAddress;

			static PathFind()
			{
				byte* address;

				address = FindPatternBmh("\x4D\x8B\xF0\x45\x8A\xE1\x48\x8B\xF9\x4C\x8D\x05", "xxxxxxxxxxxx");
				if (address != null)
				{
					s_cPathFindInstanceAddress = (ulong)(*(int*)(address + 12) + address + 16);
				}
			}

			// These values hasn't been changed between b372 and b2845
			private const int StartPathNodeOffsetOfCPathFind = 0x1640;
			private const int MaxCPathRegionCount = 0x400;

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

			private static CPathRegion* GetCPathRegion(uint areaId)
			{
				if (areaId >= MaxCPathRegionCount || s_cPathFindInstanceAddress == 0)
				{
					return null;
				}

				return *(CPathRegion**)(s_cPathFindInstanceAddress + StartPathNodeOffsetOfCPathFind + areaId * 0x8);
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
					// for those wondering the proper implementation in GET_VEHICLE_NODE_PROPERTIES, you can find it with "41 0F B6 40 27 83 E0 0F 89 07 41 F6 40 20 08" (tested with b372, b2699, and b2944)

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
					// equivalent to "if (*(uint32_t*)(CPathNode + 36) & 0x70000000)" in C or C++
					if ((Flag5AndDensity & 0x70) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.LeadsToDeadEnd;
					}
					// The water/boat bit takes precedence over this highway flag
					if (((Flags2 & 0x40) != 0 || (Flags2 & 0x20) == 0))
					{
						propertyFlags |= VehiclePathNodeProperties.Highway;
					}
					if (((Flags2 >> 8) & 1) != 0)
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
					if ((Flags2 & 1) != 0)
					{
						propertyFlags |= VehiclePathNodeProperties.DontAllowGps;
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

				CPathRegion* pathRegion = GetCPathRegion(areaId);
				if (pathRegion == null)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(pathRegion->GetPathNode(nodeId));
			}

			public static IntPtr GetPathNodeLinkAddress(int areaId, int nodeLinkIndex)
			{
				CPathRegion* pathRegion = GetCPathRegion((uint)areaId);
				if (pathRegion == null)
				{
					return IntPtr.Zero;
				}

				return new IntPtr(pathRegion->GetPathNodeLink((uint)nodeLinkIndex));
			}

			private static void GetCorrectedNodeAndAreaIdFromPathNodeHandle(int handleForNatives, out uint areaId, out uint nodeId)
			{
				uint handleCorrected = (uint)handleForNatives - 1;
				areaId = (ushort)(handleCorrected & 0xFFFF);
				nodeId = (ushort)(handleCorrected >> 0x10);
			}

			public static FVector3 GetPathNodePosition(int handle)
			{
				IntPtr pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return default;
				}

				return ((CPathNode*)pathNode)->UncompressedPosition;
			}

			public static int GetVehiclePathNodeDensity(int handle)
			{
				IntPtr pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return 0;
				}

				return ((CPathNode*)pathNode)->Density;
			}

			public static int GetVehiclePathNodePropertyFlags(int handle)
			{
				IntPtr pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return 0;
				}

				return (int)((CPathNode*)pathNode)->GetPropertyFlags();
			}

			public static bool GetPathNodeSwitchedOffFlag(int handle)
			{
				IntPtr pathNode = GetPathNodeAddress(handle);
				if (pathNode == null)
				{
					return false;
				}

				return ((CPathNode*)pathNode)->IsSwitchedOff;
			}

			public static void SetPathNodeSwitchedOffFlag(int handle, bool toggle)
			{
				IntPtr pathNode = GetPathNodeAddress(handle);
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
			private static List<int> s_pathNodeBuffer = new List<int>();

			public static int[] GetAllLoadedVehicleNodes(Func<int, bool> predicateForFlags)
			{
				s_pathNodeBuffer.Clear();

				for (uint i = 0; i < MaxCPathRegionCount; i++)
				{
					CPathRegion* pathRegion = GetCPathRegion(i);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					uint vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						CPathNode* pathNode = pathRegion->GetPathNodeUnsafe(j);
						if (predicateForFlags == null || predicateForFlags((int)pathNode->GetPropertyFlags()))
						{
							s_pathNodeBuffer.Add(pathNode->GetHandleForNativeFunctions());
						}
					}
				}

				return s_pathNodeBuffer.ToArray();
			}

			public static int[] GetLoadedVehicleNodesInRange(float x, float y, float z, float radius, Func<int, bool> predicateForFlags)
			{
				var result = new List<int>();

				foreach (uint areaId in GetAreaIdsInRange(x, y, radius))
				{
					CPathRegion* pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					uint vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						CPathNode* vehPathNode = pathRegion->GetPathNodeUnsafe(j);
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

				foreach (uint areaId in GetAreaIdsInRange(x, y, radius))
				{
					CPathRegion* pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					uint vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						CPathNode* vehPathNode = pathRegion->GetPathNodeUnsafe(j);
						if (!CheckVehPathNodePropertyPredicateAndPosition(vehPathNode, predicateForFlags, x, y, z, radius))
						{
							continue;
						}

						FVector3 nodePos = vehPathNode->UncompressedPosition;
						float nodeDist = DistanceToSquared(x, y, z, nodePos.X, nodePos.Y, nodePos.Z);
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
				float minX = Math.Min(x1, x2);
				float minY = Math.Min(y1, y2);
				float minZ = Math.Min(z1, z2);
				float maxX = Math.Max(x1, x2);
				float maxY = Math.Max(y1, y2);
				float maxZ = Math.Max(z1, z2);

				var result = new List<int>();

				foreach (uint areaId in GetAreaIdsInArea(minX, minY, maxX, maxY))
				{
					CPathRegion* pathRegion = GetCPathRegion(areaId);
					if (pathRegion == null || pathRegion->NodeArrayPtr == IntPtr.Zero)
					{
						continue;
					}

					uint vehicleNodeCountInRegion = pathRegion->NodeCountVehicle;
					for (uint j = 0; j < vehicleNodeCountInRegion; j++)
					{
						CPathNode* vehPathNode = pathRegion->GetPathNodeUnsafe(j);

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

				CPathRegion* pathRegion = GetCPathRegion(areaId);
				if (pathRegion == null)
				{
					return Array.Empty<int>();
				}

				CPathNode* pathNode = pathRegion->GetPathNode(nodeId);
				if (pathNode == null)
				{
					return Array.Empty<int>();
				}

				ushort pathNodeLinkStartId = pathNode->LinkId;
				int pathNodeLinkCount = pathNode->LinkCount;

				int[] result = new int[pathNodeLinkCount];
				for (int i = 0; i < pathNodeLinkCount; i++)
				{
					result[i] = pathNodeLinkStartId + i;
				}
				return result;
			}

			public static bool GetPathNodeLinkLanes(int areaId, int nodeLinkIndex, out int forwardLaneCount, out int backwardLaneCount)
			{
				CPathRegion* pathRegion = GetCPathRegion((uint)areaId);
				if (pathRegion == null)
				{
					forwardLaneCount = 0;
					backwardLaneCount = 0;

					return false;
				}

				CPathNodeLink* pathNodeLink = pathRegion->GetPathNodeLink((uint)nodeLinkIndex);
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
				CPathRegion* pathRegion = GetCPathRegion((uint)areaIdOfNodeLink);
				if (pathRegion == null)
				{
					targetAreaId = 0;
					targetNodeId = 0;

					return false;
				}

				CPathNodeLink* pathNodeLink = pathRegion->GetPathNodeLink((uint)nodeLinkIndex);
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
				CPathRegion* pathRegionOfNodeLink = GetCPathRegion((uint)areaIdOfNodeLink);
				if (pathRegionOfNodeLink == null)
				{
					return 0;
				}
				CPathNodeLink* pathNodeLink = pathRegionOfNodeLink->GetPathNodeLink((uint)nodeLinkIndex);
				if (pathNodeLink == null)
				{
					return 0;
				}

				pathNodeLink->GetTargetAreaAndNodeId(out int targetAreaId, out int targetNodeId);
				CPathRegion* pathRegionOfTargetNode = GetCPathRegion((uint)targetAreaId);
				if (pathRegionOfTargetNode == null)
				{
					return 0;
				}
				CPathNode* targetPathNode = pathRegionOfTargetNode->GetPathNode((uint)targetNodeId);
				if (targetPathNode == null)
				{
					return 0;
				}

				return targetPathNode->GetHandleForNativeFunctions();
			}

			private static bool CheckVehPathNodePropertyPredicateAndPosition(CPathNode* vehPathNode, Func<int, bool> predicateForFlags, float x, float y, float z, float maxDistRadius)
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

			private static bool CheckVehPathNodePropertyPredicateAndPosition(CPathNode* vehPathNode, Func<int, bool> predicateForFlags, float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
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

			private static IEnumerable<uint> GetAreaIdsInArea(float x1, float y1, float x2, float y2)
			{
				float minX = Math.Min(x1, x2);
				float minY = Math.Min(y1, y2);
				float maxX = Math.Max(x1, x2);
				float maxY = Math.Max(y1, y2);

				int minAreaRegionX = CalcIndexComponentOfAreaId(minX);
				int minAreaRegionY = CalcIndexComponentOfAreaId(minY);
				int maxAreaRegionX = CalcIndexComponentOfAreaId(maxX);
				int maxAreaRegionY = CalcIndexComponentOfAreaId(maxY);

				int areaIdCount = (maxAreaRegionX - minAreaRegionX + 1) * (maxAreaRegionY - minAreaRegionY + 1);

				for (int regionY = minAreaRegionY; regionY <= maxAreaRegionY; regionY++)
				{
					for (int regionX = minAreaRegionX; regionX <= maxAreaRegionX; regionX++)
					{
						yield return ComposeAreaIdByIndex(regionX, regionY);
					}
				}
			}

			private static IEnumerable<uint> GetAreaIdsInRange(float x, float y, float radius)
			{
				float rectMinX = x - radius;
				float rectMinY = y - radius;
				float rectMaxX = x + radius;
				float rectMaxY = y + radius;

				int minAreaRegionXIndex = CalcIndexComponentOfAreaId(rectMinX);
				int minAreaRegionYIndex = CalcIndexComponentOfAreaId(rectMinY);
				int maxAreaRegionXIndex = CalcIndexComponentOfAreaId(rectMaxX);
				int maxAreaRegionYIndex = CalcIndexComponentOfAreaId(rectMaxY);

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

			private static int CalcIndexComponentOfAreaId(float val)
			{
				int indexUnclamped = (int)((val + 8192f) / 512);
				return Math.Min(Math.Max(indexUnclamped, 0), 31);
			}
			private static uint ComposeAreaIdByIndex(int x, int y) => (uint)(x + y * 0x20);

			// Nodes at bound can be included in either area (e.g. a vehicle node at (0, 263, 10) can be included in either of the ynd files for the area IDs 527 (0x20F) or 528 (0x210))
			private static bool DoCircleAndRectIntersectOrTouch(float radius, float xCenter, float yCenter, float x1, float y1, float x2, float y2)
			{
				// Nearest position will be calculated wrong if x1 and y2 parameters are passed as x2 and y2 and vice versa
				float nearestX = Math.Max(x1, Math.Min(xCenter, x2));
				float nearestY = Math.Max(y1, Math.Min(yCenter, y2));
				float deltaX = xCenter - nearestX;
				float deltaY = yCenter - nearestY;

				return (deltaX * deltaX + deltaY * deltaY) <= (radius * radius);
			}
		}

		#endregion

		#region -- Radar Blip Pool --

		private static ulong* s_radarBlipPoolAddress;
		private static int* s_possibleRadarBlipCountAddress;
		private static int* s_unkFirstRadarBlipIndexAddress;
		private static int* s_northRadarBlipHandleAddress;
		private static int* s_centerRadarBlipHandleAddress;

		private static bool CheckBlip(ulong blipAddress, FVector3? position, float radius, params int[] spriteTypes)
		{
			if (spriteTypes.Length > 0)
			{
				int spriteIndex = *(int*)(blipAddress + 0x40);
				if (!Array.Exists(spriteTypes, x => x == spriteIndex))
				{
					return false;
				}
			}

			if (position == null || !(radius > 0f))
			{
				return true;
			}

			FVector3 positionNonNullable = position.GetValueOrDefault();
			float* blipPosition = stackalloc float[3];

			blipPosition[0] = *(float*)(blipAddress + 0x10);
			blipPosition[1] = *(float*)(blipAddress + 0x14);
			blipPosition[2] = *(float*)(blipAddress + 0x18);

			float x = blipPosition[0] - positionNonNullable.X;
			float y = blipPosition[1] - positionNonNullable.Y;
			float z = blipPosition[2] - positionNonNullable.Z;
			float distanceSquared = (x * x) + (y * y) + (z * z);
			float radiusSquared = radius * radius;

			return distanceSquared <= radiusSquared;
		}

		// The equivalent function is called in 2 functions (which is for the north and player blip) used in GET_NUMBER_OF_ACTIVE_BLIPS
		private static short GetBlipIndexIfHandleIsValid(int handle)
		{
			if (handle == 0)
			{
				return -1;
			}
			ushort blipIndex = (ushort)handle;
			ulong blipAddress = *(s_radarBlipPoolAddress + blipIndex);
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
		public static int[] GetNonCriticalRadarBlipHandles(FVector3? position = default, float radius = 0f, params int[] spriteTypes)
		{
			if (s_radarBlipPoolAddress == null)
			{
				return Array.Empty<int>();
			}

			int possibleBlipCount = *s_possibleRadarBlipCountAddress;
			int unkFirstBlipIndex = *s_unkFirstRadarBlipIndexAddress;
			int northBlipIndex = GetBlipIndexIfHandleIsValid(*s_northRadarBlipHandleAddress);
			int centerBlipIndex = GetBlipIndexIfHandleIsValid(*s_centerRadarBlipHandleAddress);

			var handles = new List<int>(possibleBlipCount);

			// Skip the 3 critical blips, just like GET_FIRST_BLIP_INFO_ID does
			// The 3 critical blips is the north blip, the center blip, and the unknown simple blip (placeholder?).
			for (int i = 0; i < possibleBlipCount; i++)
			{
				ulong address = *(s_radarBlipPoolAddress + i);

				if (address == 0 || i == unkFirstBlipIndex || i == northBlipIndex || i == centerBlipIndex)
				{
					continue;
				}

				if (!CheckBlip(address, position, radius, spriteTypes))
				{
					continue;
				}

				ushort blipCreationIncrement = *(ushort*)(address + 8);
				handles.Add((int)((blipCreationIncrement << 0x10) + (uint)i));
			}

			return handles.ToArray();
		}

		public static int GetNorthBlip() => s_northRadarBlipHandleAddress != null ? *s_northRadarBlipHandleAddress : 0;

		public static IntPtr GetBlipAddress(int handle)
		{
			if (s_radarBlipPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			int poolIndexOfHandle = handle & 0xFFFF;
			int possibleBlipCount = *s_possibleRadarBlipCountAddress;

			if (poolIndexOfHandle >= possibleBlipCount)
			{
				return IntPtr.Zero;
			}

			ulong address = *(s_radarBlipPoolAddress + poolIndexOfHandle);

			if (address != 0 && IsBlipCreationIncrementValid(address, handle))
			{
				return new IntPtr((long)address);
			}

			return IntPtr.Zero;

			bool IsBlipCreationIncrementValid(ulong blipAddress, int blipHandle) => *(ushort*)(blipAddress + 8) == (((uint)blipHandle >> 0x10));
		}

		#endregion

		#region -- CScriptResource Data --

		internal enum CScriptResourceTypeNameIndex : ushort
		{
			Checkpoint = 6,
			RelGroup = 20,
			ScaleformMovie = 21,
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct CGameScriptResource
		{
			[FieldOffset(0x0)]
			internal ulong* vTable;
			[FieldOffset(0x8)]
			internal CScriptResourceTypeNameIndex resourceTypeNameIndex;
			[FieldOffset(0xC)]
			internal uint counterOfPool;
			[FieldOffset(0x10)]
			internal uint indexOfPool;
			[FieldOffset(0x18)]
			internal CGameScriptResource* next;
			[FieldOffset(0x20)]
			internal CGameScriptResource* prev;
		}

		internal sealed class GetAllCScriptResourceHandlesTask : IScriptTask
		{
			#region Fields
			internal CScriptResourceTypeNameIndex _typeNameIndex;
			internal int[] _returnHandles = Array.Empty<int>();

			private const int MaxCheckpointCount = 64; // hard coded in the exe
			private static readonly int[] s_cScriptResourceHandleBuffer = new int[MaxCheckpointCount];
			#endregion

			internal GetAllCScriptResourceHandlesTask(CScriptResourceTypeNameIndex typeNameIndex)
			{
				this._typeNameIndex = typeNameIndex;
			}

			public void Run()
			{
				ulong cGameScriptHandlerAddress = s_getCGameScriptHandlerAddressFunc();

				if (cGameScriptHandlerAddress == 0)
				{
					return;
				}

				int elementCount = 0;
				CGameScriptResource* firstRegisteredScriptResourceItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (CGameScriptResource* item = firstRegisteredScriptResourceItem; item != null; item = item->next)
				{
					if (item->resourceTypeNameIndex != _typeNameIndex)
					{
						continue;
					}

					s_cScriptResourceHandleBuffer[elementCount++] = (int)item->counterOfPool;
				}

				if (elementCount == 0)
				{
					return;
				}

				_returnHandles = new int[elementCount];
				Array.Copy(s_cScriptResourceHandleBuffer, _returnHandles, elementCount);
			}
		}

		internal sealed class GetCScriptResourceAddressTask : IScriptTask
		{
			#region Fields
			internal int _targetHandle;
			internal ulong* _poolAddress;
			internal int _elementSize;
			internal IntPtr _returnAddress;
			#endregion

			internal GetCScriptResourceAddressTask(int handle, ulong* poolAddress, int elementSize)
			{
				this._targetHandle = handle;
				this._poolAddress = poolAddress;
				this._elementSize = elementSize;
			}

			public void Run()
			{
				ulong cGameScriptHandlerAddress = s_getCGameScriptHandlerAddressFunc();

				if (cGameScriptHandlerAddress == 0)
				{
					return;
				}

				CGameScriptResource* firstRegisteredScriptResourceItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (CGameScriptResource* item = firstRegisteredScriptResourceItem; item != null; item = item->next)
				{
					if (item->counterOfPool != _targetHandle)
					{
						continue;
					}

					_returnAddress = new IntPtr((long)((byte*)(_poolAddress) + item->indexOfPool * _elementSize));
					break;
				}
			}
		}

		internal sealed class GetCScriptResourceByIndexTask : IScriptTask
		{
			#region Fields
			internal CScriptResourceTypeNameIndex _resourceType;
			internal uint _targetIndex;
			internal CGameScriptResource* _result;
			#endregion

			internal GetCScriptResourceByIndexTask(CScriptResourceTypeNameIndex resourceType, uint index)
			{
				this._resourceType = resourceType;
				this._targetIndex = index;
			}

			public void Run()
			{
				ulong cGameScriptHandlerAddress = s_getCGameScriptHandlerAddressFunc();

				if (cGameScriptHandlerAddress == 0)
				{
					return;
				}

				CGameScriptResource* firstItem = *(CGameScriptResource**)(cGameScriptHandlerAddress + 48);
				for (_result = firstItem;
					_result != null && (_result->resourceTypeNameIndex != _resourceType || _result->indexOfPool != _targetIndex);
					_result = _result->next
					)
				{
					;
				}

				// _result should have the result address or null if not found
			}
		}

		#endregion

		#region -- Checkpoint Pool --

		private static ulong* s_checkpointPoolAddress;

		private static delegate* unmanaged[Stdcall]<ulong> s_getCGameScriptHandlerAddressFunc;

		public static int[] GetCheckpointHandles()
		{
			var task = new GetAllCScriptResourceHandlesTask(CScriptResourceTypeNameIndex.Checkpoint);

			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._returnHandles;
		}

		public static IntPtr GetCheckpointAddress(int handle)
		{
			var task = new GetCScriptResourceAddressTask(handle, s_checkpointPoolAddress, 0x60);

			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._returnAddress;
		}

		#endregion

		#region -- Scaleform Movie Data --

		public static bool IsScaleformMovieHandleValid(uint handle)
		{
			// handle cannot be zero
			if (handle == 0)
			{
				return false;
			}

			var task = new GetCScriptResourceByIndexTask(CScriptResourceTypeNameIndex.ScaleformMovie, handle);

			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._result != null;
		}

		#endregion

		#region -- Waypoint Info Array --

		private static ulong* s_waypointInfoArrayStartAddress;
		private static ulong* s_waypointInfoArrayEndAddress;
		private static delegate* unmanaged[Stdcall]<ulong> s_getLocalPlayerPedAddressFunc;

		public static int GetWaypointBlip()
		{
			if (s_waypointInfoArrayStartAddress == null || s_waypointInfoArrayEndAddress == null)
			{
				return 0;
			}

			int playerPedModelHash = 0;
			ulong playerPedAddress = s_getLocalPlayerPedAddressFunc();

			if (playerPedAddress != 0)
			{
				playerPedModelHash = GetModelHashFromEntity(new IntPtr((long)playerPedAddress));
			}

			ulong waypointInfoAddress = (ulong)s_waypointInfoArrayStartAddress;
			for (; waypointInfoAddress < (ulong)s_waypointInfoArrayEndAddress; waypointInfoAddress += 0x18)
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

		private static delegate* unmanaged[Stdcall]<int, ulong> s_getPtfxAddressFunc;
		// should be CGameScriptHandler::GetScriptEntity
		private static delegate* unmanaged[Stdcall]<int, ulong> s_getScriptEntity;

		public static IntPtr GetPtfxAddress(int handle)
		{
			return new IntPtr((long)s_getPtfxAddressFunc(handle));
		}
		public static IntPtr GetEntityAddress(int handle)
		{
			return new IntPtr((long)s_getScriptEntity(handle));
		}

		public static IntPtr GetBuildingAddress(int handle)
		{
			if (s_buildingPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			return ((GenericPool*)(*NativeMemory.s_buildingPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetAnimatedBuildingAddress(int handle)
		{
			if (s_animatedBuildingPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			return ((GenericPool*)(*NativeMemory.s_animatedBuildingPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetInteriorInstAddress(int handle)
		{
			if (s_interiorInstPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			return ((GenericPool*)(*NativeMemory.s_interiorInstPoolAddress))->GetAddressFromHandle(handle);
		}
		public static IntPtr GetInteriorProxyAddress(int handle)
		{
			if (s_interiorProxyPoolAddress == null)
			{
				return IntPtr.Zero;
			}

			return ((GenericPool*)(*NativeMemory.s_interiorProxyPoolAddress))->GetAddressFromHandle(handle);
		}

		#endregion

		#region -- Projectile Offsets --
		public static int ProjectileAmmoInfoOffset { get; }
		public static int ProjectileOwnerOffset { get; }
		#endregion

		#region -- Projectile Functions --

		private static delegate* unmanaged[Stdcall]<IntPtr, int, void> s_explodeProjectileFunc;

		public static void ExplodeProjectile(IntPtr projectileAddress)
		{
			var task = new ExplodeProjectileTask(projectileAddress);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
		}

		internal sealed class ExplodeProjectileTask : IScriptTask
		{
			#region Fields
			internal IntPtr _projectileAddress;
			#endregion

			internal ExplodeProjectileTask(IntPtr projectileAddress)
			{
				this._projectileAddress = projectileAddress;
			}

			public void Run()
			{
				s_explodeProjectileFunc(_projectileAddress, 0);
			}
		}

		#endregion

		#region -- Interior Offsets --

		public static ulong* InteriorProxyPtrFromGameplayCamAddress { get; }
		public static int InteriorInstPtrInInteriorProxyOffset { get; }

		public static int GetAssociatedInteriorInstHandleFromInteriorProxy(int interiorProxyHandle)
		{
			if (InteriorInstPtrInInteriorProxyOffset == 0 || s_interiorInstPoolAddress == null)
			{
				return 0;
			}

			IntPtr interiorProxyAddress = GetInteriorProxyAddress(interiorProxyHandle);
			if (interiorProxyAddress == IntPtr.Zero)
			{
				return 0;
			}

			ulong interiorInstAddress = *(ulong*)(interiorProxyAddress + InteriorInstPtrInInteriorProxyOffset).ToPointer();
			if (interiorInstAddress == 0)
			{
				return 0;
			}

			return ((GenericPool*)(*NativeMemory.s_interiorInstPoolAddress))->GetGuidHandleFromAddress(interiorInstAddress);
		}
		public static int GetInteriorProxyHandleFromInteriorInst(int interiorInstHandle)
		{
			if (s_interiorProxyPoolAddress == null)
			{
				return 0;
			}

			IntPtr interiorInstAddress = GetInteriorInstAddress(interiorInstHandle);
			if (interiorInstAddress == IntPtr.Zero)
			{
				return 0;
			}

			ulong interiorProxyAddress = *(ulong*)(interiorInstAddress + 0x188).ToPointer();
			if (interiorProxyAddress == 0)
			{
				return 0;
			}

			return ((GenericPool*)(*NativeMemory.s_interiorProxyPoolAddress))->GetGuidHandleFromAddress(interiorProxyAddress);
		}
		public static int GetInteriorProxyHandleFromGameplayCam()
		{
			if (InteriorProxyPtrFromGameplayCamAddress == null || s_interiorInstPoolAddress == null)
			{
				return 0;
			}

			ulong interiorProxyAddress = *InteriorProxyPtrFromGameplayCamAddress;
			if (interiorProxyAddress == 0)
			{
				return 0;
			}

			return ((GenericPool*)(*NativeMemory.s_interiorProxyPoolAddress))->GetGuidHandleFromAddress(interiorProxyAddress);
		}

		public static int GetEntityHandleFromAddress(IntPtr address)
		{
			var task = new GetEntityHandleTask(address);

			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._returnEntityHandle;
		}

		private static int GetBuildingHandleFromAddress(IntPtr address)
		{
			if (s_buildingPoolAddress == null)
			{
				return 0;
			}

			return GetHandleForGenericPoolFromAddress(*s_buildingPoolAddress, address);
		}

		private static int GetHandleForGenericPoolFromAddress(ulong poolAddress, IntPtr instanceAddress) => ((GenericPool*)poolAddress)->GetGuidHandleFromAddress((ulong)instanceAddress);

		#endregion

		#region -- Weapon Info And Ammo Info --

		[StructLayout(LayoutKind.Explicit, Size = 0xC)]
		public struct RageAtArrayPtr
		{
			[FieldOffset(0x0)]
			public ulong* data;
			[FieldOffset(0x8)]
			public ushort size;
			[FieldOffset(0xA)]
			public ushort capacity;

			public ulong GetElementAddress(int i)
			{
				return data[i];
			}
		}

		private static RageAtArrayPtr* s_weaponAndAmmoInfoArrayPtr;

		private static HashSet<uint> s_disallowWeaponHashSetForHumanPedsOnFoot = new HashSet<uint>()
		{
			0x1B79F17,  /* weapon_briefcase_02 */
			0x166218FF, /* weapon_passenger_rocket */
			0x32A888BD, /* weapon_tranquilizer */
			0x687652CE, /* weapon_stinger */
			0x6D5E2801, /* weapon_bird_crap */
			0x88C78EB7, /* weapon_briefcase */
			0xFDBADCED, /* weapon_digiscanner */
		};

		private static uint* s_weaponComponentArrayCountAddr;
		// Store the offset instead of the calculated address for compatibility with mods like Weapon Limits Adjuster by alexguirre (although Weapon Limits Adjuster allocates a new array in the very beginning).
		private static ulong s_offsetForCWeaponComponentArrayAddr;
		private static int s_weaponAttachPointsStartOffset;
		private static int s_weaponAttachPointsArrayCountOffset;
		private static int s_weaponAttachPointElementComponentCountOffset;
		private static int s_weaponAttachPointElementSize;

		private static int s_weaponInfoHumanNameHashOffset;

		[StructLayout(LayoutKind.Explicit, Size = 0x20)]
		public struct ItemInfo
		{
			[FieldOffset(0x0)]
			public ulong* vTable;
			[FieldOffset(0x10)]
			public uint nameHash;
			[FieldOffset(0x14)]
			public uint modelHash;
			[FieldOffset(0x18)]
			public uint audioHash;
			[FieldOffset(0x1C)]
			public uint slot;

			public uint GetClassNameHash()
			{
				// In the b2802 or a later exe, the function returns a hash value (not a pointer value)
				if (GetGameVersion() >= 80)
				{
					// The function is for the game version b2802 or later ones.
					// This one directly returns a hash value (not a pointer value) unlike the previous function.
					var getClassNameHashFunc = (delegate* unmanaged[Stdcall]<uint>)(vTable[2]);
					return getClassNameHashFunc();
				}

				// The function is for game versions prior to b2802.
				// The function uses rax and rdx registers in newer versions prior to b2802 (probably since b2189), and it uses only rax register in older versions.
				// The function returns the address where the class name hash is in all versions prior to (the address will be the outVal address in newer versions).
				var getClassNameAddressHashFunc = (delegate* unmanaged[Stdcall]<ulong, uint*, uint*>)(vTable[2]);

				uint outVal = 0;
				uint* returnValueAddress = getClassNameAddressHashFunc(0, &outVal);
				return *returnValueAddress;
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 0x48)]
		private struct WeaponComponentInfo
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

		private static ItemInfo* FindItemInfoFromWeaponAndAmmoInfoArray(uint nameHash)
		{
			if (s_weaponAndAmmoInfoArrayPtr == null)
			{
				return null;
			}

			ushort weaponAndAmmoInfoElementCount = s_weaponAndAmmoInfoArrayPtr->size;

			if (weaponAndAmmoInfoElementCount == 0)
			{
				return null;
			}

			int low = 0, high = weaponAndAmmoInfoElementCount - 1;
			while (true)
			{
				int indexToRead = (low + high) >> 1;
				var weaponOrAmmoInfo = (ItemInfo*)s_weaponAndAmmoInfoArrayPtr->GetElementAddress(indexToRead);

				if (weaponOrAmmoInfo->nameHash == nameHash)
				{
					return weaponOrAmmoInfo;
				}

				// The array is sorted in ascending order
				if (weaponOrAmmoInfo->nameHash <= nameHash)
				{
					low = indexToRead + 1;
				}
				else
				{
					high = indexToRead - 1;
				}

				if (low > high)
				{
					return null;
				}
			}
		}

		private static ItemInfo* FindWeaponInfo(uint nameHash)
		{
			ItemInfo* itemInfoPtr = FindItemInfoFromWeaponAndAmmoInfoArray(nameHash);

			if (itemInfoPtr == null)
			{
				return null;
			}

			uint classNameHash = itemInfoPtr->GetClassNameHash();

			const uint cWeaponInfoNameHash = 0x861905B4;
			if (classNameHash == cWeaponInfoNameHash)
			{
				return itemInfoPtr;
			}

			return null;
		}

		private static WeaponComponentInfo* FindWeaponComponentInfo(uint nameHash)
		{
			ulong* cWeaponComponentArrayFirstPtr = (ulong*)((byte*)s_offsetForCWeaponComponentArrayAddr + 4 + *(int*)s_offsetForCWeaponComponentArrayAddr);
			uint arrayCount = s_weaponComponentArrayCountAddr != null ? *(uint*)s_weaponComponentArrayCountAddr : 0;
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
				{
					return weaponComponentInfo;
				}

				// The array is sorted in ascending order
				if (weaponComponentInfo->nameHash <= nameHash)
				{
					low = indexToRead + 1;
				}
				else
				{
					high = indexToRead - 1;
				}

				if (low > high)
				{
					return null;
				}
			}
		}

		public static bool IsHashValidAsWeaponHash(uint weaponHash) => FindWeaponInfo(weaponHash) != null;

		public static uint GetAttachmentPointHash(uint weaponHash, uint componentHash)
		{
			ItemInfo* weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
			{
				return 0xFFFFFFFF;
			}

			byte* weaponAttachPointsAddr = (byte*)weaponInfo + s_weaponAttachPointsStartOffset;
			int weaponAttachPointsCount = *(int*)(weaponAttachPointsAddr + s_weaponAttachPointsArrayCountOffset);
			byte* weaponAttachPointElementStartAddr = (byte*)(weaponAttachPointsAddr);

			for (int i = 0; i < weaponAttachPointsCount; i++)
			{
				byte* weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + (i * s_weaponAttachPointElementSize) + 0x8;
				int componentItemsCount = *(int*)(weaponAttachPointElementAddr + s_weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
				{
					continue;
				}

				for (int j = 0; j < componentItemsCount; j++)
				{
					uint componentHashInItemArray = *(uint*)(weaponAttachPointElementAddr + j * 0x8);
					if (componentHashInItemArray == componentHash)
					{
						return *(uint*)(weaponAttachPointElementStartAddr + i * s_weaponAttachPointElementSize);
					}
				}
			}

			return 0xFFFFFFFF;
		}

		public static List<uint> GetAllWeaponHashesForHumanPeds()
		{
			if (s_weaponAndAmmoInfoArrayPtr == null)
			{
				return new List<uint>();
			}

			ushort weaponAndAmmoInfoElementCount = s_weaponAndAmmoInfoArrayPtr->size;
			var resultList = new List<uint>();

			for (int i = 0; i < weaponAndAmmoInfoElementCount; i++)
			{
				var weaponOrAmmoInfo = (ItemInfo*)s_weaponAndAmmoInfoArrayPtr->GetElementAddress(i);

				if (!CanPedEquip(weaponOrAmmoInfo) && !s_disallowWeaponHashSetForHumanPedsOnFoot.Contains(weaponOrAmmoInfo->nameHash))
				{
					continue;
				}

				uint classNameHash = weaponOrAmmoInfo->GetClassNameHash();

				const uint cWeaponInfoNameHash = 0x861905B4;
				if (classNameHash == cWeaponInfoNameHash)
				{
					resultList.Add(weaponOrAmmoInfo->nameHash);
				}
			}

			return resultList;

			bool CanPedEquip(ItemInfo* weaponInfoAddress)
			{
				return weaponInfoAddress->modelHash != 0 && weaponInfoAddress->slot != 0;
			}
		}

		public static List<uint> GetAllWeaponComponentHashes()
		{
			ulong* cWeaponComponentArrayFirstPtr = (ulong*)((byte*)s_offsetForCWeaponComponentArrayAddr + 4 + *(int*)s_offsetForCWeaponComponentArrayAddr);
			uint arrayCount = s_weaponComponentArrayCountAddr != null ? *(uint*)s_weaponComponentArrayCountAddr : 0;
			var resultList = new List<uint>();

			for (uint i = 0; i < arrayCount; i++)
			{
				ulong cWeaponComponentInfo = cWeaponComponentArrayFirstPtr[i];
				uint weaponComponentNameHash = *(uint*)(cWeaponComponentInfo + 0x10);
				resultList.Add(weaponComponentNameHash);
			}

			return resultList;
		}

		public static List<uint> GetAllCompatibleWeaponComponentHashes(uint weaponHash)
		{
			ItemInfo* weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
			{
				return new List<uint>();
			}

			var returnList = new List<uint>();

			byte* weaponAttachPointsAddr = (byte*)weaponInfo + s_weaponAttachPointsStartOffset;
			int weaponAttachPointsCount = *(int*)(weaponAttachPointsAddr + s_weaponAttachPointsArrayCountOffset);
			byte* weaponAttachPointElementStartAddr = (byte*)(weaponAttachPointsAddr + 0x8);
			for (int i = 0; i < weaponAttachPointsCount; i++)
			{
				byte* weaponAttachPointElementAddr = weaponAttachPointElementStartAddr + i * s_weaponAttachPointElementSize;
				int componentItemsCount = *(int*)(weaponAttachPointElementAddr + s_weaponAttachPointElementComponentCountOffset);

				if (componentItemsCount <= 0)
				{
					continue;
				}

				for (int j = 0; j < componentItemsCount; j++)
				{
					returnList.Add(*(uint*)(weaponAttachPointElementAddr + j * 0x8));
				}
			}

			return returnList;
		}

		public static uint GetHumanNameHashOfWeaponInfo(uint weaponHash)
		{
			ItemInfo* weaponInfo = FindWeaponInfo(weaponHash);

			if (weaponInfo == null)
				// hashed value of WT_INVALID
			{
				return 0xBFED8500;
			}

			return *(uint*)((byte*)weaponInfo + s_weaponInfoHumanNameHashOffset);
		}

		public static uint GetHumanNameHashOfWeaponComponentInfo(uint weaponComponentHash)
		{
			WeaponComponentInfo* weaponComponentInfo = FindWeaponComponentInfo(weaponComponentHash);

			if (weaponComponentInfo == null)
				// hashed value of WCT_INVALID
			{
				return 0xDE4BE9F8;
			}

			return weaponComponentInfo->locNameHash;
		}

		#endregion

		#region -- Fragment Object for Entity --

		private static int s_getFragInstVFuncOffset;
		private static delegate* unmanaged[Stdcall]<FragInst*, int, FragInst*> s_detachFragmentPartByIndexFunc;
		private static ulong** s_phSimulatorInstPtr;
		private static int s_colliderCapacityOffset;
		private static int s_colliderCountOffset;

		[StructLayout(LayoutKind.Explicit, Size = 0xC0)]
		internal unsafe struct FragInst
		{
			[FieldOffset(0x68)]
			internal FragCacheEntry* fragCacheEntry;
			[FieldOffset(0x78)]
			internal GtaFragType* gtaFragType;
			[FieldOffset(0xB8)]
			internal uint unkType;

			internal FragPhysicsLod* GetAppropriateFragPhysicsLod()
			{
				FragPhysicsLodGroup* fragPhysicsLodGroup = gtaFragType->fragPhysicsLODGroup;
				if (fragPhysicsLodGroup == null)
				{
					return null;
				}

				switch (unkType)
				{
					case 0:
					case 1:
					case 2:
						return fragPhysicsLodGroup->GetFragPhysicsLodByIndex((int)unkType);
					default:
						return fragPhysicsLodGroup->GetFragPhysicsLodByIndex(0);
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
			internal FragPhysicsLodGroup* fragPhysicsLODGroup;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragDrawable
		{
			[FieldOffset(0x18)]
			internal CrSkeletonData* crSkeletonData;
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragPhysicsLodGroup
		{
			[FieldOffset(0x10)]
			internal fixed ulong fragPhysicsLODAddresses[3];

			internal FragPhysicsLod* GetFragPhysicsLodByIndex(int index) => (FragPhysicsLod*)((ulong*)fragPhysicsLODAddresses[index]);
		}
		[StructLayout(LayoutKind.Explicit)]
		internal struct FragPhysicsLod
		{
			[FieldOffset(0xD0)]
			internal ulong fragTypeChildArr;
			[FieldOffset(0x11E)]
			internal byte fragmentGroupCount;

			internal FragTypeChild* GetFragTypeChild(int index)
			{
				if (index >= fragmentGroupCount)
				{
					return null;
				}

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
					{
						return boneId;
					}

					return -1;
				}

				if (boneHashMap.bucketCount == 0)
				{
					return -1;
				}

				if (boneHashMap.Get((uint)boneId, out int returnBoneId))
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
				{
					return -1;
				}

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
				ushort nextSiblingBoneIndexFetched = crBoneData->nextSiblingBoneIndex;
				if (nextSiblingBoneIndexFetched == 0xFFFF)
				{
					nextSiblingBoneIndex = -1;
					nextSiblingBoneId = -1;
					return;
				}

				int nextSiblingBoneIdFetched = GetBoneIdByIndex(nextSiblingBoneIndexFetched);
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
				ushort nextParentBoneIndexFetched = crBoneData->parentBoneIndex;
				if (nextParentBoneIndexFetched == 0xFFFF)
				{
					parentBoneIndex = -1;
					parentBoneId = -1;
					return;
				}

				int nextParentBoneIdFetched = GetBoneIdByIndex(nextParentBoneIndexFetched);
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
				{
					return null;
				}

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
				ulong* firstEntryAddr = (ulong*)GetBucketAddress((int)(hash % bucketCount));
				for (var hashEntry = (HashEntry*)firstEntryAddr; hashEntry != null; hashEntry = hashEntry->next)
				{
					if (hash != hashEntry->hash)
					{
						continue;
					}

					value = hashEntry->data;
					return true;
				}

				value = default;
				return false;
			}
		}

		internal sealed class DetachFragmentPartByIndexTask : IScriptTask
		{
			#region Fields
			internal FragInst* _fragInst;
			internal int _fragmentGroupIndex;
			internal bool _wasNewFragInstCreated;
			#endregion

			internal DetachFragmentPartByIndexTask(FragInst* fragInst, int fragmentGroupIndex)
			{
				this._fragInst = fragInst;
				this._fragmentGroupIndex = fragmentGroupIndex;
			}

			public void Run()
			{
				_wasNewFragInstCreated = s_detachFragmentPartByIndexFunc(_fragInst, _fragmentGroupIndex) != null;
			}
		}

		public static int GetFragmentGroupCountFromEntity(IntPtr entityAddress)
		{
			FragInst* fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
			{
				return 0;
			}

			return GetFragmentGroupCountOfFragInst(fragInst);
		}

		public static bool DetachFragmentPartByIndex(IntPtr entityAddress, int fragmentGroupIndex)
		{
			if (fragmentGroupIndex < 0)
			{
				return false;
			}

			// If the entity collider count is at the capacity, the game can crash for trying to create the new entity while no free collider slots are available
			if (GetEntityColliderCount() >= GetEntityColliderCapacity())
			{
				return false;
			}

			FragInst* fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
			{
				return false;
			}

			int fragmentGroupCount = GetFragmentGroupCountOfFragInst(fragInst);
			if (fragmentGroupIndex >= fragmentGroupCount)
			{
				return false;
			}

			var task = new DetachFragmentPartByIndexTask(fragInst, fragmentGroupIndex);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);

			return task._wasNewFragInstCreated;
		}

		public static int GetFragmentGroupIndexByEntityBoneIndex(IntPtr entityAddress, int boneIndex)
		{
			if ((boneIndex & 0x80000000) != 0) // boneIndex cant be negative
			{
				return -1;
			}

			FragInst* fragInst = GetFragInstAddressOfEntity(entityAddress);
			if (fragInst == null)
			{
				return -1;
			}


			CrSkeletonData* crSkeletonData = fragInst->gtaFragType->fragDrawable->crSkeletonData;
			if (crSkeletonData == null)
			{
				return -1;
			}

			ushort boneCount = crSkeletonData->boneCount;
			if (boneIndex >= boneCount)
			{
				return -1;
			}

			FragPhysicsLod* fragPhysicsLod = fragInst->GetAppropriateFragPhysicsLod();
			if (fragPhysicsLod == null)
			{
				return -1;
			}

			byte fragmentGroupCount = fragPhysicsLod->fragmentGroupCount;

			for (int i = 0; i < fragmentGroupCount; i++)
			{
				FragTypeChild* fragTypeChild = fragPhysicsLod->GetFragTypeChild(i);

				if (fragTypeChild == null)
				{
					continue;
				}

				if (boneIndex == crSkeletonData->GetBoneIndexByBoneId(fragTypeChild->boneId))
				{
					return i;
				}
			}

			return -1;
		}

		public static int GetEntityColliderCapacity()
		{
			if (*s_phSimulatorInstPtr == null)
			{
				return 0;
			}

			return *(int*)((byte*)*s_phSimulatorInstPtr + s_colliderCapacityOffset);
		}

		public static int GetEntityColliderCount()
		{
			if (*s_phSimulatorInstPtr == null)
			{
				return 0;
			}

			return *(int*)((byte*)*s_phSimulatorInstPtr + s_colliderCountOffset);
		}

		public static bool IsEntityFragmentObject(IntPtr entityAddress)
		{
			// For CObject, a valid address will be returned only when a certain flag is set. For CPed and CVehicle, a valid address will always be returned.
			return GetFragInstAddressOfEntity(entityAddress) != null;
		}

		private static FragInst* GetFragInstAddressOfEntity(IntPtr entityAddress)
		{
			ulong vFuncAddr = *(ulong*)(*(ulong*)entityAddress.ToPointer() + (uint)s_getFragInstVFuncOffset);
			var getFragInstFunc = (delegate* unmanaged[Stdcall]<IntPtr, FragInst*>)(vFuncAddr);

			return getFragInstFunc(entityAddress);
		}

		/// <summary>
		/// Gets whether the passed entity has a skeleton.
		/// This method is provided so it makes possible for SHVDN to avoid the game crashing for trying to use an absent
		/// CrSkeleton address when calling entity attachment native functions with bone indices but one of them does not
		/// have a CrSkeleton, even in the game versions earlier than v1.0.2699.0.
		/// </summary>
		public static bool EntityHasSkeleton(int handle)
		{
			IntPtr addr = GetEntityAddress(handle);
			if (addr == null)
			{
				return false;
			}

			return CEntityHasCrSkeleton(addr);
		}
		/// <summary>
		/// Gets whether the CEntity has a CrSkeleton.
		/// Basically does the same thing as what the native DOES_ENTITY_HAVE_SKELETON does but this method takes
		/// an CEntity address.
		/// </summary>
		/// <param name="cEntityAddress">The CEntity address (does not has to be CPhysical).</param>
		public static bool CEntityHasCrSkeleton(IntPtr cEntityAddress)
		{
			FragInst* fragInst = GetFragInstAddressOfEntity(cEntityAddress);
			if (fragInst == null)
			{
				return false;
			}

			FragCacheEntry* fragCache = fragInst->fragCacheEntry;
			if (fragCache == null)
			{
				return false;
			}

			CrSkeleton* crSkel = fragCache->crSkeleton;
			if (crSkel == null)
			{
				return false;
			}

			return true;
		}

		private static int GetFragmentGroupCountOfFragInst(FragInst* fragInst)
		{
			FragPhysicsLod* fragPhysicsLod = fragInst->GetAppropriateFragPhysicsLod();
			return fragPhysicsLod != null ? fragPhysicsLod->fragmentGroupCount : 0;
		}


		#endregion

		#region -- NaturalMotion Euphoria --

		// These CNmParameter functions can also be called as virtual functions for your information
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, int, byte> s_setNmParameterInt;
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, bool, byte> s_setNmParameterBool;
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, float, byte> s_setNmParameterFloat;
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, IntPtr, byte> s_setNmParameterString;
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, float, float, float, byte> s_setNmParameterVector;

		private static delegate* unmanaged[Stdcall]<ulong, ulong, int, ulong> s_initMessageMemoryFunc;
		private static delegate* unmanaged[Stdcall]<ulong, IntPtr, ulong, void> s_sendNmMessageToPedFunc;
		private static delegate* unmanaged[Stdcall]<ulong, CTask*> s_getActiveTaskFunc;

		private static int s_fragInstNmGtaOffset;
		private static int s_cTaskNmScriptControlTypeIndex;
		private static int s_cEventSwitch2NmTypeIndex;
		private static uint s_getEventTypeIndexVFuncOffset;
		private static uint s_fragInstNmGtaGetUnkValVFuncOffset;

		[StructLayout(LayoutKind.Explicit, Size = 0x38)]
		private struct CTask
		{
			[FieldOffset(0x34)]
			internal ushort taskTypeIndex;
		}

		public static bool IsTaskNmScriptControlOrEventSwitch2NmActive(IntPtr pedAddress)
		{
			ulong phInstGtaAddress = *(ulong*)(pedAddress + 0x30);

			if (phInstGtaAddress == 0)
			{
				return false;
			}

			ulong fragInstNmGtaAddress = *(ulong*)(pedAddress + s_fragInstNmGtaOffset);

			if (phInstGtaAddress != fragInstNmGtaAddress || IsPedInjured((byte*)pedAddress))
			{
				return false;
			}

			// This virtual function will return -1 if phInstGta is not a NM one
			var fragInstNmGtaGetUnkValVFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(new IntPtr((long)*(ulong*)(*(ulong*)fragInstNmGtaAddress + s_fragInstNmGtaGetUnkValVFuncOffset)));
			if (fragInstNmGtaGetUnkValVFunc(fragInstNmGtaAddress) == -1)
			{
				return false;
			}

			ulong pedIntelligenceAddr = *(ulong*)(pedAddress + Ped.PedIntelligenceOffset);

			CTask* activeTask = s_getActiveTaskFunc(*(ulong*)((byte*)pedIntelligenceAddr + Ped.CTaskTreePedOffset));
			if (activeTask != null && activeTask->taskTypeIndex == s_cTaskNmScriptControlTypeIndex)
			{
				return true;
			}

			int eventCount = *(int*)((byte*)pedIntelligenceAddr + Ped.CEventCountOffset);
			for (int i = 0; i < eventCount; i++)
			{
				ulong eventAddress = *(ulong*)((byte*)pedIntelligenceAddr + Ped.CEventStackOffset + 8 * ((i + *(int*)((byte*)pedIntelligenceAddr + (Ped.CEventCountOffset - 4)) + 1) % 16));
				if (eventAddress == 0)
				{
					continue;
				}

				var getEventTypeIndexVirtualFunc = (delegate* unmanaged[Stdcall]<ulong, int>)(*(ulong*)(*(ulong*)eventAddress + s_getEventTypeIndexVFuncOffset));
				if (getEventTypeIndexVirtualFunc(eventAddress) != s_cEventSwitch2NmTypeIndex)
				{
					continue;
				}

				CTask* taskInEvent = *(CTask**)(eventAddress + 0x28);
				if (taskInEvent == null)
				{
					continue;
				}

				if (taskInEvent->taskTypeIndex == s_cTaskNmScriptControlTypeIndex)
				{
					return true;
				}
			}

			return false;
		}

		private static bool IsPedInjured(byte* pedAddress) => *(float*)(pedAddress + 0x280) < *(float*)(pedAddress + Ped.InjuryHealthThresholdOffset);

		private static void SetNmParameters(ulong messageMemory, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
		{
			if (boolIntFloatParameters != null)
			{
				foreach (KeyValuePair<string, (int value, Type type)> arg in boolIntFloatParameters)
				{
					IntPtr name = ScriptDomain.CurrentDomain.PinString(arg.Key);

					(int argValue, Type argType) = arg.Value;

					if (argType == typeof(float))
					{
						float argValueConverted = *(float*)(&argValue);
						NativeMemory.s_setNmParameterFloat(messageMemory, name, argValueConverted);
					}
					else if (argType == typeof(bool))
					{
						bool argValueConverted = argValue != 0 ? true : false;
						NativeMemory.s_setNmParameterBool(messageMemory, name, argValueConverted);
					}
					else if (argType == typeof(int))
					{
						NativeMemory.s_setNmParameterInt(messageMemory, name, argValue);
					}
				}
			}

			if ((stringVector3ArrayParameters != null))
			{
				foreach (KeyValuePair<string, object> arg in stringVector3ArrayParameters)
				{
					IntPtr name = ScriptDomain.CurrentDomain.PinString(arg.Key);

					object argValue = arg.Value;
					switch (argValue)
					{
						case float[] vector3ArgValue:
							NativeMemory.s_setNmParameterVector(messageMemory, name, vector3ArgValue[0], vector3ArgValue[1], vector3ArgValue[2]);
							break;
						case string stringArgValue:
							NativeMemory.s_setNmParameterString(messageMemory, name, ScriptDomain.CurrentDomain.PinString(stringArgValue));
							break;
					}
				}
			}
		}

		internal sealed class NmMessageTask : IScriptTask
		{
			#region Fields

			private int _targetHandle;
			private string _messageName;
			private Dictionary<string, (int value, Type type)> _boolIntFloatParameters;
			private Dictionary<string, object> _stringVector3ArrayParameters;
			#endregion

			internal NmMessageTask(int target, string messageName, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
			{
				_targetHandle = target;
				this._messageName = messageName;
				this._boolIntFloatParameters = boolIntFloatParameters;
				this._stringVector3ArrayParameters = stringVector3ArrayParameters;
			}

			public void Run()
			{
				byte* pedAddress = (byte*)NativeMemory.GetEntityAddress(_targetHandle).ToPointer();

				if (pedAddress == null)
				{
					return;
				}

				if (!IsTaskNmScriptControlOrEventSwitch2NmActive(new IntPtr(pedAddress)))
				{
					return;
				}

				ulong messageMemory = (ulong)AllocCoTaskMem(0x1218).ToInt64();
				if (messageMemory == 0)
				{
					return;
				}

				s_initMessageMemoryFunc(messageMemory, messageMemory + 0x18, 0x40);

				SetNmParameters(messageMemory, _boolIntFloatParameters, _stringVector3ArrayParameters);

				ulong fragInstNmGtaAddress = *(ulong*)(pedAddress + s_fragInstNmGtaOffset);
				IntPtr messageStringPtr = ScriptDomain.CurrentDomain.PinString(_messageName);
				s_sendNmMessageToPedFunc((ulong)fragInstNmGtaAddress, messageStringPtr, messageMemory);

				FreeCoTaskMem(new IntPtr((long)messageMemory));
			}
		}

		public static void SendNmMessage(int targetHandle, string messageName, Dictionary<string, (int value, Type type)> boolIntFloatParameters, Dictionary<string, object> stringVector3ArrayParameters)
		{
			var task = new NmMessageTask(targetHandle, messageName, boolIntFloatParameters, stringVector3ArrayParameters);
			ScriptDomain.CurrentDomain.ExecuteTaskWithGameThreadTlsContext(task);
		}

		#endregion
	}
}
