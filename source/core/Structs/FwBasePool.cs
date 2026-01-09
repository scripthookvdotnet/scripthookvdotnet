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
        public ulong PoolStartAddress;
        [FieldOffset(0x08)]
        public IntPtr ByteArray;
        [FieldOffset(0x10)]
        public uint Size;
        [FieldOffset(0x14)]
        public uint ItemSize;
        // The "first" index should be at 0x18 and The "last" index should be at 0x1C in production builds
        // according to the layout in a debug build around v1.0.2699.0, but the "first" and the "last" aren't
        // related to about the order.
        // WARNING: according to `rage::fwBasePoolTracker::GetNoOfUsedSpaces`, this field is supposed to be read
        // by reading as a 4-byte value, applying left shift by 2 and SIGNED right shift (`SAR` in assembly code)
        // by 2, and then return the calculated value.
        [FieldOffset(0x20)]
        public ushort ItemCount;

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
            return ((Mask(index) & (PoolStartAddress + index * ItemSize)));
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
            if (address < PoolStartAddress || address >= PoolStartAddress + Size * ItemSize)
            {
                return 0;
            }
            ulong offset = address - PoolStartAddress;
            if (offset % ItemSize != 0)
            {
                return 0;
            }
            uint indexOfPool = (uint)(offset / ItemSize);
            return (int)((indexOfPool << 8) + GetCounter(indexOfPool));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte GetCounter(uint index)
        {
            unsafe
            {
                byte* byteArrayPtr = (byte*)ByteArray.ToPointer();
                return (byte)(byteArrayPtr[index] & 0x7F);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong Mask(uint index)
        {
            unsafe
            {
                byte* byteArrayPtr = (byte*)ByteArray.ToPointer();
                long num1 = byteArrayPtr[index] & 0x80;
                return (ulong)(~((num1 | -num1) >> 63));
            }
        }
    }
}
