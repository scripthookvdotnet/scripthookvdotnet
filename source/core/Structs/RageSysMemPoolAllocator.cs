using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace SHVDN
{
        /// <summary>
        /// Represents <c>rage::sysMemPoolAllocator</c>, which all of the
        /// <c>rage::sysMemPoolAllocator::PoolWrapper&lt;typename T&gt;</c> have as the sole field via an pointer.
        /// </summary>
        /// <remarks>
        /// Possible (without limitation) <c>typename T</c>s of
        /// <c>rage::sysMemPoolAllocator::PoolWrapper&lt;typename T&gt;</c> are <c>CTask</c>, <c>CTaskInfo</c>,
        /// <c>CVehicle</c>, <c>audVehicleAudioEntity</c>, and <c>void *</c>.
        /// </remarks>
        [StructLayout(LayoutKind.Explicit)]
        internal unsafe struct RageSysMemPoolAllocator
        {
            // m_pool at offset 0x0 takes 0x60 byte
            // (type: "rage::atIteratablePool<rage::sysMemPoolAllocator::PoolNode>").
            [FieldOffset(0x00)]
            internal ulong* PoolAddress;
            [FieldOffset(0x08)]
            internal uint Size;
            [FieldOffset(0x30)]
            internal uint* BitArray;

            // m_freeList at 0x60 takes 0x18 bytes (type: "rage::inlist<rage::sysMemPoolAllocator::FreeNode,8>").
            // The struct contains m_head, m_tail and m_size fields.
            [FieldOffset(0x60)]
            internal uint ItemCount;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal bool IsValid(uint i)
            {
                return ((BitArray[i >> 5] >> ((int)i & 0x1F)) & 1) != 0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal ulong GetAddress(uint i)
            {
                return PoolAddress[i];
            }
        }
}
