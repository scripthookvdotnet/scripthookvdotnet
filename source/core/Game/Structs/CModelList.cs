using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x400)]
    internal unsafe struct CModelList
    {
        [FieldOffset(0x0)]
        public fixed uint ModelMemberIndices[0x100];
    }
}
