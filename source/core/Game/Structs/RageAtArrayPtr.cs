using System.Runtime.InteropServices;

namespace SHVDN
{
    // TODO: support size and capacity type other than ushort (uint16_t).
    // Actually defined like rage::atArray<element_type, some_size, size_and_capacity_type> in the exe, but we
    // assumed the template was defined with 1 type parameter before the big incident happened.
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public unsafe struct RageAtArrayPtr
    {
        [FieldOffset(0x0)]
        public ulong * Data;

        [FieldOffset(0x8)]
        public ushort Size;

        [FieldOffset(0xA)]
        public ushort Capacity;

        // rage::atArray always has 4-byte padding at the end
        [FieldOffset(0xC)]
        private fixed char padding[4];

        public ulong GetElementAddress(int i)
        {
            return Data[i];
        }
    }
}
