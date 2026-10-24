using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct HashNode
    {
        public int Hash;
        public ushort Data;
        public ushort Padding;
        public HashNode* Next;
    }
}
