using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EntityDamageRecordForReturnValue
    {
        public int attackerEntityHandle;
        public int weaponHash;
        public int gameTime;
        public EntityDamageRecordForReturnValue(int attackerEntityHandle, int weaponHash, int gameTime)
        {
            this.attackerEntityHandle = attackerEntityHandle;
            this.weaponHash = weaponHash;
            this.gameTime = gameTime;
        }
    }
}
