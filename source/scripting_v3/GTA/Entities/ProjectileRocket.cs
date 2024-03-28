//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// Represents a rocket projectile, which is for `<c>CProjectileRocket</c>`.
    /// </summary>
    public sealed class ProjectileRocket : Projectile
    {
        internal ProjectileRocket(int handle) : base(handle)
        {
        }

        public Entity Target
        {
            get
            {
                IntPtr address = SHVDN.NativeMemory.GetEntityAddress(Handle);
                if (address == IntPtr.Zero)
                {
                    return null;
                }

                return Entity.FromHandle(SHVDN.NativeMemory.GetTargetEntityOfCProjectileRocket(address));
            }
        }

        /// <summary>
        /// Get a <see cref="ProjectileRocket"/> instance by its handle.
        /// </summary>
        /// <param name="handle">The handle to test.</param>
        /// <returns>
        /// A <see cref="ProjectileRocket"/> if the handle is assigned for a <see cref="ProjectileRocket"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public static new ProjectileRocket FromHandle(int handle)
        {
            IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
            if (address == IntPtr.Zero
                || (EntityTypeInternal)SHVDN.NativeMemory.ReadByte(address + 0x28) != EntityTypeInternal.Object)
            {
                return null;
            }

            // `CObject::GetAsCProjectileRocket` should return the same address if the vfunc is overridden by
            // `CProjectileRocket`'s implementation and it should return false otherwise
            return SHVDN.NativeMemory.GetAsCProjectileRocket(address) != address ? null : new ProjectileRocket(handle);
        }
    }
}
