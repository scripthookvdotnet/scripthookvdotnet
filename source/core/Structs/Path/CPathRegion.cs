using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x70)]
    internal unsafe struct CPathRegion
    {
        [FieldOffset(0x10)]
        internal IntPtr NodeArrayPtr;
        [FieldOffset(0x18)]
        internal uint NodeCount;
        [FieldOffset(0x1C)]
        internal uint NodeCountVehicle;
        [FieldOffset(0x20)]
        internal uint NodeCountPed;
        [FieldOffset(0x28)]
        internal IntPtr NodeLinkArrayPtr;
        [FieldOffset(0x30)]
        internal uint NodeLinkCount;
        [FieldOffset(0x38)]
        internal IntPtr VirtualJunctionArrayPtr;
        [FieldOffset(0x40)]
        internal IntPtr HeightSampleArrayPtr;
        // `CPathRegion.JunctionMap` is at 0x50, which has a `rage::CPathRegion::JunctionMapContainer`.
        // `rage::CPathRegion::JunctionMapContainer` is practically an alias of
        // `rage::atBinaryMap<int,unsigned int>`. `rage::atBinaryMap` internally has a bool (at the 0x0 offset)
        // that represents whether the content is sorted before the `rage::atArray` field.
        [FieldOffset(0x60)]
        internal uint JunctionCount;
        [FieldOffset(0x64)]
        internal uint HeightSampleCount;
        internal CPathNode * GetPathNode(uint nodeId)
        {
            if (NodeArrayPtr == IntPtr.Zero && nodeId >= NodeCount)
            {
                return null;
            }
            return GetPathNodeUnsafe(nodeId);
        }
        internal CPathNode * GetPathNodeUnsafe(uint nodeId) => (CPathNode * )((ulong) NodeArrayPtr + nodeId * (uint) sizeof(CPathNode));
        internal CPathNodeLink * GetPathNodeLink(uint index)
        {
            if (NodeLinkArrayPtr == IntPtr.Zero && index >= NodeLinkCount)
            {
                return null;
            }
            return GetPathNodeLinkUnsafe(index);
        }
        internal CPathNodeLink * GetPathNodeLinkUnsafe(uint index) => (CPathNodeLink * )((ulong) NodeLinkArrayPtr + index * (uint) sizeof(CPathNodeLink));
    }
}
