using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTable
    {
        [FieldOffset(0x0)]
        public YscScriptTableItem* TablePtr;

        [FieldOffset(0x18)]
        public uint Count;

        public YscScriptTableItem* FindScript(int hash)
        {
            if (TablePtr == null)
            {
                return null; //table initialisation hasn't happened yet
            }
            for (int i = 0; i < Count; i++)
            {
                if (TablePtr[i].Hash == hash)
                {
                    return &TablePtr[i];
                }
            }
            return null;
        }
    }
}
