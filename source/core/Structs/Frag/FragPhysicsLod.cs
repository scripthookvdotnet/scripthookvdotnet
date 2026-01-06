using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct FragPhysicsLod
    {
        [FieldOffset(0xD0)]
        internal ulong fragTypeChildArr;
        [FieldOffset(0x11E)]
        internal byte fragmentGroupCount;

        internal FragTypeChild* GetFragTypeChild(int index)
        {
            if (index >= fragmentGroupCount)
            {
                return null;
            }

            return (FragTypeChild*)*((ulong*)fragTypeChildArr + index);
        }
    }
}
