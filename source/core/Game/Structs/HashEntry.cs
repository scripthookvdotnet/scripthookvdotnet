using System.Runtime.InteropServices;
namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal unsafe struct HashEntry
    {
        public uint Hash;
        public int Data;
        public HashEntry* Next;
    }
}
