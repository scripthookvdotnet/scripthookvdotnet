using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x8)]
    internal struct CPathNodeLink
    {
        // Same as CPathNode, this field is supposed to be a rage::CNodeAddress...
        [FieldOffset(0x0)]
        internal ushort AreaId;
        [FieldOffset(0x2)]
        internal ushort NodeId;
        [FieldOffset(0x4)]
        internal byte Flags0;
        [FieldOffset(0x5)]
        internal byte Flags1;
        [FieldOffset(0x6)]
        internal byte Flags2;
        [FieldOffset(0x7)]
        internal byte LinkLength;
        internal int ForwardLaneCount => (Flags2 >> 5) & 7;
        internal int BackwardLaneCount => (Flags2 >> 2) & 7;
        internal void GetForwardAndBackwardCount(out int forwardCount, out int backwardCount)
        {
            forwardCount = (Flags2 >> 5) & 7;
            backwardCount = (Flags2 >> 2) & 7;
        }
        internal void GetTargetAreaAndNodeId(out int areaId, out int nodeId)
        {
            areaId = AreaId;
            nodeId = NodeId;
        }
    }
}
