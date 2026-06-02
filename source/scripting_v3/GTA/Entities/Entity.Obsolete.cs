using System;
using System.ComponentModel;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public abstract partial class Entity : PoolObject, ISpatial
    {
        /// <summary>
        /// Gets or sets the rotation velocity of this <see cref="Entity"/> in local space.
        /// </summary>
        [Obsolete("Entity.RotationVelocity is obsolete because GET_ENTITY_ROTATION_VELOCITY returns the world angular velocity with local to world conversion applied. Use Entity.LocalRotationVelocity instead.")]
        public Vector3 RotationVelocity
        {
            get => Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION_VELOCITY, Handle);
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return;

                Vector3 angularVelocityInLocalAxes = Quaternion * value;
                SHVDN.NativeMemory.SetEntityAngularVelocity(address, angularVelocityInLocalAxes.X, angularVelocityInLocalAxes.Y, angularVelocityInLocalAxes.Z);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Entity"/> is water cannon proof.
        /// <see cref="Ped"/>s does not get ragdolled by the water jet from fire hydrants when this property is set to <see langword="true" />.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="Entity"/> is water cannon proof; otherwise, <see langword="false" />.
        /// </value>
        [Obsolete("Entity.IsWaterCannonProof is obsolete because CPhysical has no flags that makes it " +
            "water cannon proof.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsWaterCannonProof
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return false;

                return SHVDN.MemDataMarshal.IsBitSet(address + 392, 12);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return;

                SHVDN.MemDataMarshal.SetBit(address + 392, 12, value);
            }
        }
    }
}
