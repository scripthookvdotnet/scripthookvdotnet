using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct YscScriptTable
    {
        [FieldOffset(0x0)]
        internal YscScriptTableItem* TablePtr;
        [FieldOffset(0x18)]
        internal uint count;
        internal YscScriptTableItem* FindScript(int hash)
        {
            if (TablePtr == null)
            {
                return null; //table initialisation hasn't happened yet
            }
            for (int i = 0; i < count; i++)
            {
                if (TablePtr[i].hash == hash)
                {
                    return &TablePtr[i];
                }
            }
            return null;
        }
    }
}
