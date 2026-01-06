using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct HashNode
    {
        internal int hash;
        internal ushort data;
        internal ushort padding;
        internal HashNode* next;
    }
}
