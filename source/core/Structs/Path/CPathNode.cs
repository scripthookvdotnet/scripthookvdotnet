using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x28)]
    internal unsafe struct CPathNode
    {
        [FieldOffset(0x0)]
        internal CPathNode* Next;
        [FieldOffset(0x8)]
        internal CPathNode* Previous;
        // Note: CPathNode in the game is supposed to have rage::CNodeAddress (4-byte union, which has
        // a regular uint32_t field and bit fields) at 0x10
        [FieldOffset(0x10)]
        internal ushort AreaId;
        [FieldOffset(0x12)]
        internal ushort NodeId;
        [FieldOffset(0x14)]
        internal uint StreetNameHash;
        [FieldOffset(0x1A)]
        internal ushort startIndexOfLinks;
        // These 2 fields should be multiplied by 4 when you convert back to float
        [FieldOffset(0x1C)]
        internal short PositionX;
        [FieldOffset(0x1E)]
        internal short PositionY;
        [FieldOffset(0x20)]
        internal ushort Flags1;
        // This field should be multiplied by 32 when you convert back to float
        [FieldOffset(0x22)]
        internal short PositionZ;
        [FieldOffset(0x24)]
        internal byte Flags2;
        [FieldOffset(0x25)]
        internal byte Flags3AndLinkCount;
        [FieldOffset(0x26)]
        internal byte Flags4;
        // 1st to 4th bits are used for density
        [FieldOffset(0x27)]
        internal byte Flag5AndDensity;

        internal int Density => Flag5AndDensity & 0xF;

        internal int LinkCount => Flags3AndLinkCount >> 3;

        // Native functions for path nodes get area IDs and node IDs from the values subtracted by one from passed values
        // When the lower half of bits (of passed values) are equal to zero, the natives considers the null handle is passed
        internal int GetHandleForNativeFunctions() => ((NodeId << 0x10) + AreaId + 1);

        internal FVector3 UncompressedPosition => new FVector3((float) PositionX / 4, (float) PositionY / 4, (float) PositionZ / 32);

        internal bool IsSwitchedOff
        {
            get => (Flags2 & 0x80) != 0;
            set
            {
                if (value)
                {
                    Flags2 |= 0x80;
                }
                else
                {
                    Flags2 &= 0x7F;
                }
            }
        }
        /// <summary>
        /// Get property flags in almost the same way as GET_VEHICLE_NODE_PROPERTIES returns flags as the 5th parameter (seems the flags the native returns will never contain the 1024 flag).
        /// </summary>
        internal VehiclePathNodeProperties GetPropertyFlags()
        {
            // for those wondering the proper implementation in GET_VEHICLE_NODE_PROPERTIES, you can find it with "41 0F B6 40 27 83 E0 0F 89 07 41 F6 40 20 08" (tested with b372, b2699, and b2944)
            VehiclePathNodeProperties propertyFlags = VehiclePathNodeProperties.None;
            if ((Flags1 & 8) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.OffRoad;
            }
            if ((Flags1 & 0x10) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.OnPlayersRoad;
            }
            if ((Flags1 & 0x20) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.NoBigVehicles;
            }
            if ((Flags2 & 0x80) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.SwitchedOff;
            }
            if ((Flags4 & 0x1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.TunnelOrInterior;
            }
            // equivalent to "if (*(uint32_t*)(CPathNode + 36) & 0x70000000)" in C or C++
            if ((Flag5AndDensity & 0x70) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.LeadsToDeadEnd;
            }
            // The water/boat bit takes precedence over this highway flag
            if (((Flags2 & 0x40) != 0 || (Flags2 & 0x20) == 0))
            {
                propertyFlags |= VehiclePathNodeProperties.Highway;
            }
            if (((Flags2 >> 8) & 1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.Junction;
            }
            if ((Flags1 & 0xF800) == 0x7800)
            {
                propertyFlags |= VehiclePathNodeProperties.TrafficLight;
            }
            if ((Flags1 & 0xF800) == 0x8000)
            {
                propertyFlags |= VehiclePathNodeProperties.GiveWay;
            }
            if ((Flags2 & 0x20) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.Boat;
            }
            if ((Flags2 & 1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.DontAllowGps;
            }
            return propertyFlags;
        }
        internal bool IsInArea(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float posXUncompressed = (float) PositionX / 4;
            float posYUncompressed = (float) PositionY / 4;
            float posZUncompressed = (float) PositionZ / 32;
            if (posXUncompressed < x1 || posXUncompressed > x2)
            {
                return false;
            }
            if (posYUncompressed < y1 || posYUncompressed > y2)
            {
                return false;
            }
            if (posZUncompressed < z1 || posYUncompressed > z2)
            {
                return false;
            }
            return true;
        }
        internal bool IsInCircle(float x, float y, float z, float radius)
        {
            float posXUncompressed = (float) PositionX / 4;
            float posYUncompressed = (float) PositionY / 4;
            float posZUncompressed = (float) PositionZ / 32;
            float deltaX = (float) x - posXUncompressed;
            float deltaY = (float) y - posYUncompressed;
            float deltaZ = (float) z - posZUncompressed;
            return ((deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ)) <= radius * radius;
        }
    }
}
