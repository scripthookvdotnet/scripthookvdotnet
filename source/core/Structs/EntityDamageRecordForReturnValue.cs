using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EntityDamageRecordForReturnValue
    {
        public int AttackerEntityHandle;
        public int WeaponHash;
        public int GameTime;

        public EntityDamageRecordForReturnValue(int attackerEntityHandle, int weaponHash, int gameTime)
        {
            AttackerEntityHandle = attackerEntityHandle;
            WeaponHash = weaponHash;
            GameTime = gameTime;
        }
    }
}
