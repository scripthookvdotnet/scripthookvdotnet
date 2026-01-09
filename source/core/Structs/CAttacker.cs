using System.Runtime.InteropServices;

namespace SHVDN
{
    // the size is at least 0x10 in all game versions
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    internal struct CAttacker
    {
        [FieldOffset(0x0)]
        internal ulong AttackerEntityAddress;
        [FieldOffset(0x8)]
        internal int WeaponHash;
        [FieldOffset(0xC)]
        internal int GameTime;
    }
}
