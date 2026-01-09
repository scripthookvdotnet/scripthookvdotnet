using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x48)]
    internal unsafe struct WeaponComponentInfo
    {
        [FieldOffset(0x0)]
        internal ulong* vTable;
        [FieldOffset(0x10)]
        internal uint NameHash;
        [FieldOffset(0x14)]
        internal uint ModelHash;
        [FieldOffset(0x18)]
        internal uint LocNameHash;
        [FieldOffset(0x1C)]
        internal uint LocDescHash;
        [FieldOffset(0x40)]
        internal bool ShownOnWheel;
        [FieldOffset(0x41)]
        internal bool CreateObject;
        [FieldOffset(0x42)]
        internal bool ApplyWeaponTint;
    }
}
