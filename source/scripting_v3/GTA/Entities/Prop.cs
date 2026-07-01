//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Math;
using GTA.Native;
using SHVDN;

namespace GTA
{
    public class Prop : Entity
    {
        internal Prop(int handle) : base(handle)
        {
        }

        /// <inheritdoc cref="Create(Model, Vector3, Vector3, bool, bool)"/>
        public static Prop Create(Model model, Vector3 position, bool dynamic, bool placeOnGround)
        {
            if (World.PropCount >= World.PropCapacity || !model.Request(1000))
            {
                return null;
            }

            if (placeOnGround)
            {
                World.GetGroundHeight(position, out float groundHeight);
                position.Z = groundHeight; // will be zero if the test failed since values will be initialized with zero by default in C#
            }

            return new Prop(Function.Call<int>(Hash.CREATE_OBJECT, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic));
        }
        /// <summary>
        /// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position.
        /// </summary>
        /// <param name="model">The <see cref="Model"/> of the <see cref="Prop"/>.</param>
        /// <param name="position">The position to spawn the <see cref="Prop"/> at.</param>
        /// <param name="rotation">The rotation of the <see cref="Prop"/>.</param>
        /// <param name="dynamic">
        /// <para>
        /// If <see langword="true"/>, the <see cref="Prop"/> will always be forced to be a regular prop type (<c>CObject</c>). This applies when creating a <see cref="Prop"/> that uses a door <see cref="Model"/>.
        /// If this is <see langword="false"/>, the <see cref="Prop"/> will be created as a door type (<c>CDoor</c>) and it will work as a door.
        /// </para>
        /// <para>Although "dynamic" is an incorrectly named parameter, the name is retained for scripts that use the method with named parameters.</para>
        /// </param>
        /// <param name="placeOnGround">if set to <see langword="true" /> place the prop on the ground nearest to the <paramref name="position"/>.</param>
        /// <remarks>returns <see langword="null" /> if the <see cref="Prop"/> could not be spawned or the model could not be loaded within 1 second.</remarks>
        public static Prop Create(Model model, Vector3 position, Vector3 rotation, bool dynamic, bool placeOnGround)
        {
            Prop prop = Create(model, position, dynamic, placeOnGround);

            if (prop != null)
            {
                prop.Rotation = rotation;
            }

            return prop;
        }
        /// <summary>
        /// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position without any offset.
        /// </summary>
        /// <inheritdoc cref="Create(Model, Vector3, Vector3, bool, bool)"/>
        public static Prop CreateNoOffset(Model model, Vector3 position, bool dynamic)
        {
            if (World.PropCount >= World.PropCapacity || !model.Request(1000))
            {
                return null;
            }

            return new Prop(Function.Call<int>(Hash.CREATE_OBJECT_NO_OFFSET, model.Hash, position.X, position.Y, position.Z, 1, 1, dynamic));
        }
        /// <summary>
        /// Spawns a <see cref="Prop"/> of the given <see cref="Model"/> at the specified position without any offset.
        /// </summary>
        /// <inheritdoc cref="Create(Model, Vector3, Vector3, bool, bool)"/>
        public static Prop CreateNoOffset(Model model, Vector3 position, Vector3 rotation, bool dynamic)
        {
            Prop prop = CreateNoOffset(model, position, dynamic);

            if (prop != null)
            {
                prop.Rotation = rotation;
            }

            return prop;
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
            if (!TryGetMemoryAddress(out IntPtr address))
                return null;

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
            if (!TryGetMemoryAddress(out IntPtr address))
                return null;

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
            if (!TryGetMemoryAddress(out IntPtr address))
                return null;

            // `CObject::GetAsCProjectileThrown` should return the same address if the vfunc is overridden by
            // `CProjectileThrown`'s implementation and it should return false otherwise
            return NativeMemory.GetAsCProjectileThrown(address) != address ? null : new ProjectileThrown(Handle);
        }
    }
}
