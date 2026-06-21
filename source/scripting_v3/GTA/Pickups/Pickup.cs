//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
    /// <summary>
    /// Represents a pickup placement, not pickup object.
    /// </summary>
    public sealed class Pickup : PoolObject
    {
        public Pickup(int handle) : base(handle)
        {
        }

        /// <inheritdoc cref="Create(PickupType, Vector3, Vector3, PickupPlacementFlags, int, EulerRotationOrder, Model)"/>
        public static Pickup Create(
            PickupType type,
            Vector3 position,
            PickupPlacementFlags placementFlags = PickupPlacementFlags.None,
            int amount = -1,
            Model customModel = default)
        {
            if (customModel.Hash != 0 && !customModel.Request(1000))
            {
                return null;
            }

            // The 2nd last argument is named ScriptHostObject, so just set to true as most SP scripts do
            int handle = Function.Call<int>(Hash.CREATE_PICKUP,
                (int)type,
                position.X,
                position.Y,
                position.Z,
                (int)placementFlags,
                amount,
                true,
                customModel.Hash);

            return handle == 0 ? null : new Pickup(handle);
        }

        /// <summary>
        /// Creates a pickup spawner (a <see cref="Pickup"/> instance) which can be referenced by the script and will spawn a pickup whenever the player gets near.
        /// This spawner can also regenerate the pickup after it is collected.
        /// The spawner is removed when the script terminates.
        /// </summary>
        /// <param name="type">The pickup type hash.</param>
        /// <param name="position">The pickup position to place in world space.</param>
        /// <param name="rotation">The pickup orientation.</param>
        /// <param name="placementFlags">The pickup placement flags.</param>
        /// <param name="amount">
        /// A variable amount that can be specified for some pickups, such as money or ammo.
        /// Leave this parameter as <c>-1</c> to apply the default amount.
        /// </param>
        /// <param name="rotOrder">The rotation order in world space.</param>
        /// <param name="customModel">
        /// If set to non-zero value, this model will be used for the pickup instead of the default one.
        /// </param>
        public static Pickup Create(
            PickupType type,
            Vector3 position,
            Vector3 rotation,
            PickupPlacementFlags placementFlags = PickupPlacementFlags.None,
            int amount = -1,
            EulerRotationOrder rotOrder = EulerRotationOrder.YXZ,
            Model customModel = default
        )
        {
            if (customModel.Hash != 0 && !customModel.Request(1000))
            {
                return null;
            }

            // The 2nd last argument is named ScriptHostObject, so just set to true as most SP scripts do
            int handle = Function.Call<int>(Hash.CREATE_PICKUP_ROTATE,
                (int)type,
                position.X,
                position.Y,
                position.Z,
                rotation.X,
                rotation.Y,
                rotation.Z,
                (int)placementFlags,
                amount,
                (int)rotOrder,
                true,
                customModel.Hash);

            return handle == 0 ? null : new Pickup(handle);
        }

        /// <summary>
        /// The position of this <see cref="Pickup"/> placement.
        /// </summary>
        public Vector3 Position => Function.Call<Vector3>(Hash.GET_PICKUP_COORDS, Handle);

        /// <summary>
        /// Gets if this <see cref="Pickup"/> placement has been collected.
        /// </summary>
        public bool IsCollected => Function.Call<bool>(Hash.HAS_PICKUP_BEEN_COLLECTED, Handle);

        /// <summary>
        /// Gets the <see cref="PickupObject"/> of this <see cref="Pickup"/> placement.
        /// </summary>
        /// <returns></returns>
        public PickupObject Object
        {
            get
            {
                // GET_PICKUP_OBJECT returns -1 (not 0) if the pickup placement has no object or is invalid
                int objHandle = Function.Call<int>(Hash.GET_PICKUP_OBJECT, Handle);
                return objHandle != -1 ? new PickupObject(objHandle) : null;
            }
        }

        /// <summary>
        /// Determines if the object of this <see cref="Pickup"/> placement exists.
        /// </summary>
        /// <returns></returns>
        public bool ObjectExists()
        {
            return Function.Call<bool>(Hash.DOES_PICKUP_OBJECT_EXIST, Handle);
        }

        /// <summary>
        /// Destroys this <see cref="Pickup"/> placement.
        /// </summary>
        public override void Delete()
        {
            Function.Call(Hash.REMOVE_PICKUP, Handle);
        }

        /// <summary>
        /// Determines if this <see cref="Pickup"/> placement exists.
        /// </summary>
        /// <returns><see langword="true" /> if this <see cref="Pickup"/> exists; otherwise, <see langword="false" />.</returns>
        public override bool Exists()
        {
            return Function.Call<bool>(Hash.DOES_PICKUP_EXIST, Handle);
        }

        /// <summary>
        /// Determines if an <see cref="object"/> refers to the same pickup placement as this <see cref="Pickup"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to check.</param>
        /// <returns><see langword="true" /> if the <paramref name="obj"/> is the same pickup as this <see cref="Pickup"/>; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Pickup pickup)
            {
                return Handle == pickup.Handle;
            }

            return false;
        }

        /// <summary>
        /// Determines if two <see cref="Pickup"/>s refer to the same pickup placement.
        /// </summary>
        /// <param name="left">The left <see cref="Pickup"/>.</param>
        /// <param name="right">The right <see cref="Pickup"/>.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> is the same pickup placement as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Pickup left, Pickup right)
        {
            return left?.Equals(right) ?? right is null;
        }
        /// <summary>
        /// Determines if two <see cref="Pickup"/>s don't refer to the same pickup placement.
        /// </summary>
        /// <param name="left">The left <see cref="Pickup"/>.</param>
        /// <param name="right">The right <see cref="Pickup"/>.</param>
        /// <returns><see langword="true" /> if <paramref name="left"/> is not the same pickup as <paramref name="right"/>; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Pickup left, Pickup right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Converts a <see cref="Pickup"/> to a native input argument.
        /// </summary>
        public static implicit operator InputArgument(Pickup value)
        {
            return new InputArgument((ulong)value.Handle);
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
