using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x48)]
    internal unsafe struct WeaponComponentInfo
    {
        [FieldOffset(0x0)]
        public ulong* vTable;

        [FieldOffset(0x10)]
        public uint NameHash;

        [FieldOffset(0x14)]
        public uint ModelHash;

        [FieldOffset(0x18)]
        public uint LocNameHash;

        [FieldOffset(0x1C)]
        public uint LocDescHash;

        [FieldOffset(0x40)]
        public bool ShownOnWheel;

        [FieldOffset(0x41)]
        public bool CreateObject;

        [FieldOffset(0x42)]
        public bool ApplyWeaponTint;
    }
}
