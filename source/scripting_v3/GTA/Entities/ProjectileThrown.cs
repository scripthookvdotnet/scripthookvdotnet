//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// Represents a thrown projectile, which is for `<c>CProjectileThrown</c>`.
    /// </summary>
    /// <remarks>
    /// Unlike `<c>CProjectileRocket</c>`, `<c>CProjectileThrown</c>` only overrides
    /// `<c>CObject::GetAsProjectileThrown</c>` (both const and non-const variants) and `<c>CProjectile::GetInfo</c>`
    /// (which returns `<c>CAmmoProjectileInfo</c>`).
    /// `<c>CProjectileThrown</c>` does not have additional members or functions, either.
    /// </remarks>
    public sealed class ProjectileThrown : Projectile
    {
        internal ProjectileThrown(int handle) : base(handle)
        {
        }

        /// <summary>
        /// Get a <see cref="ProjectileThrown"/> instance by its handle.
        /// </summary>
        /// <param name="handle">The handle to test.</param>
        /// <returns>
        /// A <see cref="ProjectileThrown"/> if the handle is assigned for a <see cref="ProjectileThrown"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public static new ProjectileThrown FromHandle(int handle)
        {
            IntPtr address = SHVDN.NativeMemory.GetEntityAddress(handle);
            if (address == IntPtr.Zero
                || (EntityTypeInternal)SHVDN.MemDataMarshal.ReadByte(address + 0x28) != EntityTypeInternal.Object)
            {
                return null;
            }

            // `CObject::GetAsCProjectileThrown` should return the same address if the vfunc is overridden by
            // `CProjectileThrown`'s implementation and it should return false otherwise
            return SHVDN.NativeMemory.GetAsCProjectileThrown(address) != address ? null : new ProjectileThrown(handle);
        }
    }
}
