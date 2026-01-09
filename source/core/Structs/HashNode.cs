using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct HashNode
    {
        internal int Hash;
        internal ushort Data;
        internal ushort Padding;
        internal HashNode* Next;
    }
}
