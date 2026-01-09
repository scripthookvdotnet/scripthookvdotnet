using System.Runtime.InteropServices;
namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal unsafe struct HashEntry
    {
        internal uint Hash;
        internal int Data;
        internal HashEntry* Next;
    }
}
