using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x48)]
    internal unsafe struct WeaponComponentInfo
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
}
