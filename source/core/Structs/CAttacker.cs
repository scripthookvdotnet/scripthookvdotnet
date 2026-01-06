using System.Runtime.InteropServices;

namespace SHVDN
{
    // the size is at least 0x10 in all game versions
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    internal struct CAttacker
    {
        [FieldOffset(0x0)]
        internal ulong attackerEntityAddress;
        [FieldOffset(0x8)]
        internal int weaponHash;
        [FieldOffset(0xC)]
        internal int gameTime;
    }
}
