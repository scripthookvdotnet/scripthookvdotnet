using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
    /// <summary>
    /// Represents <c>rage::fwBasePool</c>, which all of the <c>rage::fwPool&lt;typename T&gt;</c> types has as
    /// the sole field.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <c>rage::fwBasePool</c> takes 0x30 bytes without a vtable pointer in production builds, but that in
    /// a debug build around v1.0.2699.0 takes 0x68 bytes with a vtable pointer and additional debug fields.
    /// </para>
    /// <para>
    /// All of <c>rage::fwPool&lt;typename T&gt;</c> types (at least 243 types) has the same layout but with
    /// different element type (at least the return type of <c>GetSlot(int)</c> differs by type parameter).
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    internal struct FwBasePool
    {
        [FieldOffset(0x00)]
        public ulong PoolAddress;

        [FieldOffset(0x08)]
        public IntPtr Flags;

        // The max count value should be at least 3072 as long as ScriptHookV is installed.
        // Without ScriptHookV, the default value is hardcoded and may be different between different game versions (the value is 300 in b372 and 700 in b2824).
        // The default value (when running without ScriptHookV) can be found by searching the dumped exe or the game memory with "D7 A8 11 73" (0x7311A8D7).
        [FieldOffset(0x10)]
        public uint Capacity;

        [FieldOffset(0x14)]
        public uint SlotSize;

        [FieldOffset(0x18)]
        public int FirstEmptySlot;

        [FieldOffset(0x1C)]
        public int LastEmptySlot;

        [FieldOffset(0x20)]
        private ushort _slotsUsedAndFlags;

        public int SlotsUsed => _slotsUsedAndFlags & 0x3FFFFFFF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFull()
        {
            return Capacity - SlotsUsed <= 256;
        }

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
            return ((Mask(index) & (PoolAddress + index * SlotSize)));
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
            if (address < PoolAddress || address >= PoolAddress + Capacity * SlotSize)
            {
                return 0;
            }

            ulong offset = address - PoolAddress;

            if (offset % SlotSize != 0)
            {
                return 0;
            }

            uint indexOfPool = (uint)(offset / SlotSize);
            return (int)((indexOfPool << 8) + GetCounter(indexOfPool));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte GetCounter(uint index)
        {
            unsafe
            {
                byte* flagsPtr = (byte*)Flags.ToPointer();
                return (byte)(flagsPtr[index] & 0x7F);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong Mask(uint index)
        {
            unsafe
            {
                byte* flagsPtr = (byte*)Flags.ToPointer();
                long num1 = flagsPtr[index] & 0x80;

                return (ulong)(~((num1 | -num1) >> 63));
            }
        }
    }
}
