//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using SHVDN;

namespace GTA
{
    public class Prop : Entity
    {
        internal Prop(int handle) : base(handle)
        {
        }

        #region Fragment

        /// <summary>
        /// Determines if this <see cref="Prop"/> has been created as a <see cref="Prop"/> detached from the parent <see cref="Entity"/>.
        /// Will return <see langword="true"/> when the <see cref="Prop"/> has been detached from parent <see cref="Ped"/> and has been created as a separate <see cref="Prop"/>
        /// or when the <see cref="Prop"/> is a fragment part detached from parent <see cref="Vehicle"/> or <see cref="Prop"/> and has been created as a separate <see cref="Prop"/>
        /// </summary>
        public bool HasBeenDetachedFromParentEntity => NativeMemory.HasPropBeenDetachedFromParentEntity(Handle);

        /// <summary>
        /// Gets the <see cref="Entity"/> this <see cref="Prop"/> is detached from.
        /// If found, will return an instance of any one of <see cref="Ped"/>, <see cref="Vehicle"/>, or <see cref="Prop"/>.
        /// If not found, will return <see langword="null"/>.
        /// </summary>
        public Entity ParentEntityDetachedFrom
        {
            get
            {
                int parentEntityHandle = NativeMemory.GetParentEntityHandleOfPropDetachedFrom(Handle);
                if (parentEntityHandle == 0)
                {
                    return null;
                }

                return FromHandle(parentEntityHandle);
            }
        }

        #endregion

        /// <summary>
        /// Determines if this <see cref="Prop"/> exists.
        /// You should ensure <see cref="Prop"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
        /// </summary>
        /// <returns><see langword="true" /> if this <see cref="Prop"/> exists; otherwise, <see langword="false" />.</returns>
        public new bool Exists()
        {
            return EntityType == EntityType.Prop;
        }

        /// <summary>
        /// Get as a <see cref="Projectile"/> instance if this <see cref="Prop"/> is a <see cref="Projectile"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Projectile"/> if the <see cref="Prop"/> exists and is a <see cref="Projectile"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public Projectile AsProjectile()
        {
            IntPtr address = MemoryAddress;
            if (address == IntPtr.Zero)
            {
                return null;
            }

            // `CObject::GetAsCProjectile` should return the same address if the vfunc is overridden by `CProjectile`'s
            // implementation and it should return false otherwise
            return NativeMemory.GetAsCProjectile(address) != address ? null : new Projectile(Handle);
        }

        /// <summary>
        /// Get as a <see cref="ProjectileRocket"/> instance if this <see cref="Prop"/> is
        /// a <see cref="ProjectileRocket"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="ProjectileRocket"/> if the <see cref="Prop"/> exists and is a <see cref="ProjectileRocket"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public ProjectileRocket AsProjectileRocket()
        {
            IntPtr address = MemoryAddress;
            if (address == IntPtr.Zero)
            {
                return null;
            }

            // `CObject::GetAsCProjectileRocket` should return the same address if the vfunc is overridden by
            // `CProjectileRocket`'s implementation and it should return false otherwise
            return NativeMemory.GetAsCProjectileRocket(address) != address ? null : new ProjectileRocket(Handle);
        }

        /// <summary>
        /// Get as a <see cref="ProjectileThrown"/> instance if this <see cref="Prop"/> is
        /// a <see cref="ProjectileThrown"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="ProjectileThrown"/> if the <see cref="Prop"/> exists and is a <see cref="ProjectileThrown"/>;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public ProjectileThrown AsProjectileThrown()
        {
            IntPtr address = MemoryAddress;
            if (address == IntPtr.Zero)
            {
                return null;
            }

            // `CObject::GetAsCProjectileThrown` should return the same address if the vfunc is overridden by
            // `CProjectileThrown`'s implementation and it should return false otherwise
            return NativeMemory.GetAsCProjectileThrown(address) != address ? null : new ProjectileThrown(Handle);
        }
    }
}
