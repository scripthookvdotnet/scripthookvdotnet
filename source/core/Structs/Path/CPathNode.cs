using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x28)]
    internal unsafe struct CPathNode
    {
        [FieldOffset(0x0)]
        public CPathNode* Next;

        [FieldOffset(0x8)]
        public CPathNode* Previous;
        // Note: CPathNode in the game is supposed to have rage::CNodeAddress (4-byte union, which has
        // a regular uint32_t field and bit fields) at 0x10

        [FieldOffset(0x10)]
        public ushort AreaId;

        [FieldOffset(0x12)]
        public ushort NodeId;

        [FieldOffset(0x14)]
        public uint StreetNameHash;

        [FieldOffset(0x1A)]
        public ushort StartIndexOfLinks;
        // These 2 fields should be multiplied by 4 when you convert back to float

        [FieldOffset(0x1C)]
        public short PositionX;

        [FieldOffset(0x1E)]
        public short PositionY;

        [FieldOffset(0x20)]
        private ushort _flags1;
        // This field should be multiplied by 32 when you convert back to float

        [FieldOffset(0x22)]
        public short PositionZ;

        [FieldOffset(0x24)]
        private byte _flags2;

        [FieldOffset(0x25)]
        private byte _flags3AndLinkCount;

        [FieldOffset(0x26)]
        private byte _flags4;

        // 1st to 4th bits are used for density
        [FieldOffset(0x27)]
        private byte _flag5AndDensity;

        public int Density => _flag5AndDensity & 0xF;

        public int LinkCount => _flags3AndLinkCount >> 3;

        // Native functions for path nodes get area IDs and node IDs from the values subtracted by one from passed values
        // When the lower half of bits (of passed values) are equal to zero, the natives considers the null handle is passed
        public int GetHandleForNativeFunctions() => ((NodeId << 0x10) + AreaId + 1);

        public FVector3 UncompressedPosition => new FVector3((float) PositionX / 4, (float) PositionY / 4, (float) PositionZ / 32);

        public bool IsSwitchedOff
        {
            get => (_flags2 & 0x80) != 0;
            set
            {
                if (value)
                {
                    _flags2 |= 0x80;
                }
                else
                {
                    _flags2 &= 0x7F;
                }
            }
        }
        /// <summary>
        /// Get property flags in almost the same way as GET_VEHICLE_NODE_PROPERTIES returns flags as the 5th parameter (seems the flags the native returns will never contain the 1024 flag).
        /// </summary>
        public VehiclePathNodeProperties GetPropertyFlags()
        {
            // for those wondering the proper implementation in GET_VEHICLE_NODE_PROPERTIES, you can find it with "41 0F B6 40 27 83 E0 0F 89 07 41 F6 40 20 08" (tested with b372, b2699, and b2944)
            VehiclePathNodeProperties propertyFlags = VehiclePathNodeProperties.None;

            if ((_flags1 & 8) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.OffRoad;
            }

            if ((_flags1 & 0x10) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.OnPlayersRoad;
            }

            if ((_flags1 & 0x20) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.NoBigVehicles;
            }

            if ((_flags2 & 0x80) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.SwitchedOff;
            }

            if ((_flags4 & 0x1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.TunnelOrInterior;
            }

            // equivalent to "if (*(uint32_t*)(CPathNode + 36) & 0x70000000)" in C or C++
            if ((_flag5AndDensity & 0x70) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.LeadsToDeadEnd;
            }

            // The water/boat bit takes precedence over this highway flag
            if (((_flags2 & 0x40) != 0 || (_flags2 & 0x20) == 0))
            {
                propertyFlags |= VehiclePathNodeProperties.Highway;
            }


            if (((_flags2 >> 8) & 1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.Junction;
            }

            if ((_flags1 & 0xF800) == 0x7800)
            {
                propertyFlags |= VehiclePathNodeProperties.TrafficLight;
            }

            if ((_flags1 & 0xF800) == 0x8000)
            {
                propertyFlags |= VehiclePathNodeProperties.GiveWay;
            }

            if ((_flags2 & 0x20) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.Boat;
            }

            if ((_flags2 & 1) != 0)
            {
                propertyFlags |= VehiclePathNodeProperties.DontAllowGps;
            }

            return propertyFlags;
        }

        public bool IsInArea(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            FVector3 uncompressedPos = UncompressedPosition;

            if (uncompressedPos.X < x1 || uncompressedPos.X > x2)
            {
                return false;
            }

            if (uncompressedPos.Y < y1 || uncompressedPos.Y > y2)
            {
                return false;
            }

            if (uncompressedPos.Z < z1 || uncompressedPos.Z > z2)
            {
                return false;
            }

            return true;
        }

        public bool IsInCircle(float x, float y, float z, float radius)
        {
            FVector3 uncompressedPos = UncompressedPosition;

            float deltaX = (float) x - uncompressedPos.X;
            float deltaY = (float) y - uncompressedPos.Y;
            float deltaZ = (float) z - uncompressedPos.Z;

            return ((deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ)) <= radius * radius;
        }
    }
}
