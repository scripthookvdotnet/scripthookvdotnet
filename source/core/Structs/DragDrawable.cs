using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragDrawable
    {
        [FieldOffset(0x18)]
        public CrSkeletonData* SkeletonData;
    }
}
