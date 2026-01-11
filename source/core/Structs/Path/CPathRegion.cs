using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    [StructLayout(LayoutKind.Explicit, Size = 0x70)]
    internal unsafe struct CPathRegion
    {
        [FieldOffset(0x10)]
        private IntPtr _nodeArrayPtr;

        [FieldOffset(0x18)]
        public uint NodeCount;

        [FieldOffset(0x1C)]
        public uint NodeCountVehicle;

        [FieldOffset(0x20)]
        public uint NodeCountPed;

        [FieldOffset(0x28)]
        private IntPtr _nodeLinkArrayPtr;

        [FieldOffset(0x30)]
        public uint NodeLinkCount;

        [FieldOffset(0x38)]
        private IntPtr _virtualJunctionArrayPtr;

        [FieldOffset(0x40)]
        private IntPtr _heightSampleArrayPtr;

        // `CPathRegion.JunctionMap` is at 0x50, which has a `rage::CPathRegion::JunctionMapContainer`.
        // `rage::CPathRegion::JunctionMapContainer` is practically an alias of
        // `rage::atBinaryMap<int,unsigned int>`. `rage::atBinaryMap` publicly has a bool (at the 0x0 offset)
        // that represents whether the content is sorted before the `rage::atArray` field.
        [FieldOffset(0x60)]
        public uint JunctionCount;

        [FieldOffset(0x64)]
        public uint HeightSampleCount;

        public bool IsNodeArrValid => _nodeArrayPtr == IntPtr.Zero;

        public CPathNode* GetPathNode(uint nodeId)
        {
            if (_nodeArrayPtr == IntPtr.Zero || nodeId >= NodeCount)
            {
                return null;
            }
            return GetPathNodeUnsafe(nodeId);
        }

        public CPathNode* GetPathNodeUnsafe(uint nodeId) => (CPathNode*)((ulong)_nodeArrayPtr + nodeId * (uint) sizeof(CPathNode));

        public CPathNodeLink* GetPathNodeLink(uint index)
        {
            if (_nodeLinkArrayPtr == IntPtr.Zero || index >= NodeLinkCount)
            {
                return null;
            }
            return GetPathNodeLinkUnsafe(index);
        }

        public CPathNodeLink* GetPathNodeLinkUnsafe(uint index) => (CPathNodeLink*)((ulong)_nodeLinkArrayPtr + index * (uint) sizeof(CPathNodeLink));
    }
}
