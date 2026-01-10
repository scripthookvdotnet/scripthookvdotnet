using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLod
    {
        [FieldOffset(0xD0)]
        private ulong _fragTypeChildArr;

        [FieldOffset(0x11E)]
        public byte FragmentGroupCount;

        public FragTypeChild* GetFragTypeChild(int index)
        {
            if (index >= FragmentGroupCount)
            {
                return null;
            }

            return (FragTypeChild*)*((ulong*)_fragTypeChildArr + index);
        }
    }
}
