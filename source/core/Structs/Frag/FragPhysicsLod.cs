using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLod
    {
        [FieldOffset(0xD0)]
        internal ulong FragTypeChildArr;
        [FieldOffset(0x11E)]
        internal byte FragmentGroupCount;

        internal FragTypeChild* GetFragTypeChild(int index)
        {
            if (index >= FragmentGroupCount)
            {
                return null;
            }

            return (FragTypeChild*)*((ulong*)FragTypeChildArr + index);
        }
    }
}
