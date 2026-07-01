using System;
using System.ComponentModel;
using GTA.Math;
using GTA.Native;

namespace GTA
{
    public sealed partial class Vehicle : Entity
    {
        /// <summary>
        /// Determines whether the specified <c>extra</c> is currently enabled on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to check.</param>
        /// <returns>
        /// <see langword="true"/> if the extra is enabled; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Enabled instead!")]
        public bool IsExtraOn(int extra)
        {
            return Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Handle, extra);
        }

        /// <summary>
        /// Determines whether the specified <c>extra</c> exists on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to check.</param>
        /// <returns>
        /// <see langword="true"/> if the extra exists; otherwise, <see langword="false"/>.
        /// </returns>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Exists() instead!")]
        public bool ExtraExists(int extra)
        {
            return Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Handle, extra);
        }

        /// <summary>
        /// Enables or disables the specified <c>extra</c> on this <see cref="Vehicle"/>.
        /// </summary>
        /// <param name="extra">The extra to enable or disable.</param>
        /// <param name="toggle"><see langword="true"/> to enable the extra; <see langword="false"/> to disable it.</param>
        [Obsolete("Use Vehicle.Extras[VehicleExtraIndex].Enabled instead!")]
        public void ToggleExtra(int extra, bool toggle)
        {
            Function.Call(Hash.SET_VEHICLE_EXTRA, Handle, extra, !toggle);
        }

        /// <summary>
        /// Gets or sets the gears value of this <see cref="Vehicle"/>.
        /// </summary>
        [Obsolete("Use Vehicle.HighGear for the high gear value and Vehicle.CurrentGear for the current gear value instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public int Gears
        {
            get => HighGear;
            set => HighGear = value;
        }

        /// <inheritdoc cref="Vehicle.HasDamageDecals"/>
        [Obsolete("Use Vehicle.HasDamageDecals instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsDamaged => Function.Call<bool>(Hash.GET_DOES_VEHICLE_HAVE_DAMAGE_DECALS, Handle);

        [Obsolete("Use ApplyDamageDeformation instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void ApplyDamage(Vector3 position, float damageAmount, float radius)
        {
            Function.Call(Hash.SET_VEHICLE_DAMAGE, Handle, position.X, position.Y, position.Z, damageAmount, radius);
        }

        [Obsolete("Vehicle.TowVehicle(Vehicle, bool) is obsolete because the bone index parameter is incorrectly used " +
    "as a bool parameter. Use one of the other overload instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public void TowVehicle(Vehicle vehicle, bool rear)
        {
            Function.Call(Hash.ATTACH_VEHICLE_TO_TOW_TRUCK, Handle, vehicle.Handle, rear, 0f, 0f, 0f);
        }
    }
}
