using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x8)]
    internal struct CPathNodeLink
    {
        // Same as CPathNode, this field is supposed to be a rage::CNodeAddress...
        [FieldOffset(0x0)]
        public ushort AreaId;

        [FieldOffset(0x2)]
        public ushort NodeId;

        [FieldOffset(0x4)]
        public byte Flags0;

        [FieldOffset(0x5)]
        public byte Flags1;

        [FieldOffset(0x6)]
        public byte Flags2;

        [FieldOffset(0x7)]
        public byte LinkLength;

        public int ForwardLaneCount => (Flags2 >> 5) & 7;

        public int BackwardLaneCount => (Flags2 >> 2) & 7;

        public void GetForwardAndBackwardCount(out int forwardCount, out int backwardCount)
        {
            forwardCount = (Flags2 >> 5) & 7;
            backwardCount = (Flags2 >> 2) & 7;
        }

        public void GetTargetAreaAndNodeId(out int areaId, out int nodeId)
        {
            areaId = AreaId;
            nodeId = NodeId;
        }
    }
}
