using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public unsafe struct CItemInfo
    {
        [FieldOffset(0x0)]
        public ulong* vTable;

        [FieldOffset(0x10)]
        public uint NameHash;

        [FieldOffset(0x14)]
        public uint ModelHash;

        [FieldOffset(0x18)]
        public uint AudioHash;

        [FieldOffset(0x1C)]
        public uint Slot;

        public uint GetClassNameHash()
        {
            // In the b2802 or a later exe, the function returns a hash value (not a pointer value)
            if (NativeMemory.GetGameVersion() >= 80)
            {
                // The function is for the game version b2802 or later ones.
                // This one directly returns a hash value (not a pointer value) unlike the previous function.
                var getClassNameHashFunc = (delegate * unmanaged[Stdcall] < uint > )(vTable[2]);
                return getClassNameHashFunc();
            }

            // The function is for game versions prior to b2802.
            // The function uses rax and rdx registers in newer versions prior to b2802 (probably since b2189), and it uses only rax register in older versions.
            // The function returns the address where the class name hash is in all versions prior to (the address will be the outVal address in newer versions).
            var getClassNameAddressHashFunc = (delegate * unmanaged[Stdcall] < ulong, uint * , uint * > )(vTable[2]);
            uint outVal = 0;
            uint * returnValueAddress = getClassNameAddressHashFunc(0, & outVal);
            return *returnValueAddress;
        }
    }
}
