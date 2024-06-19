//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Diagnostics;
using System.Linq;
using System.Text;
using System;

namespace SHVDN
{
    public static class MemScanner
    {
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

            LogMemPatternNotFound(pattern, mask, startAddress, size);

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

            LogMemPatternNotFound(pattern, mask, startAddress, size);

            return null;
        }

        [Conditional("DEBUG")]
        private static void LogMemPatternNotFound(string pattern, string mask, IntPtr startAddr, ulong searchSize)
        {
            string patternFormatted = pattern.ToCharArray().Aggregate(new StringBuilder("\"", pattern.Length * 3 + 2),
                    (a, b) => a.Append(((byte)b).ToString("X2")).Append(" "),
                    (a) => a.Remove(a.Length - 1, 1).Append("\"").ToString());

            Log.Message(Log.Level.Warning, $"Memory pattern not found. " +
                $"Pattern: {patternFormatted}, Mask: {mask}, Start Address: 0x{startAddr.ToString("X")}, " +
                $"Search Size: 0x{searchSize.ToString("X")}"
                );
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
    }
}
