using System.Runtime.InteropServices;

namespace SHVDN
{
    /// <summary>
    /// Represents a `<c>CWeaponComponentPoint</c>` but without the `<c>m_Components</c>` field followed by
    /// `<c>m_AttachBoneId</c>`, where the type is <c>atFixedArray&lt;sComponent, MAX_WEAPON_COMPONENTS&gt;</c> and
    /// `<c>MAX_WEAPON_COMPONENTS</c>` is a hardcoded `<c>i32</c>`/`<c>s32</c>` const.
    /// </summary>
    /// <remarks>
    /// This struct omits the field for `<c>m_Components</c>` because its byte size can be grown in some game
    /// updates (e.g. `<c>m_AttachBoneId</c>` takes 0x5C bytes in b2699 and takes 0x64 bytes in b3095).
    /// </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 0x8)]
    internal struct WeaponComponentPointHeader
    {
        /// <summary>
        /// The attach bone hash for the `<c>m_AttachBone</c>` field, where the type is
        /// `<c>atHashWithStringNotFinal</c>` (basically just a `<c>u32</c>` hash).
        /// </summary>
        [FieldOffset(0x0)]
        internal uint AttachBoneHash;
        /// <summary>
        /// The corresponding bone hierarchy id (index) for the attach bone for the `<c>m_AttachBoneId</c>` field,
        /// where the type is `<c>eHierarchyId</c>` (a `<c>i32</c>`/`<c>s32</c>` enum).
        /// </summary>
        [FieldOffset(0x4)]
        internal uint AttachBoneId;
    }
}
