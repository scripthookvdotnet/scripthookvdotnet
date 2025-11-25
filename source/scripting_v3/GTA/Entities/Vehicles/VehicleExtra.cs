using GTA.Native;

namespace GTA
{
    public class VehicleExtra
    {
        private string _boneName { get; set; }

        /// <summary>
        /// Gets the owner <see cref="Vehicle"/> of this <see cref="VehicleExtra"/>.
        /// </summary>
        public Vehicle Owner { get; private set; }

        /// <summary>
        /// Gets the <see cref="VehicleExtraIndex"/> of this <see cref="VehicleExtra"/>.
        /// </summary>
        public VehicleExtraIndex Index { get; private set; }

        internal VehicleExtra(Vehicle owner, VehicleExtraIndex index)
        {
            Owner = owner;
            Index = index;

            _boneName = Index.GetBoneName();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VehicleExtra"/> is enabled.
        /// </summary>
        /// <remarks>
        /// If this <see cref="VehicleExtra"/> was detached, setting this property to <see langword="true"/> will not affect its state and create a new one instead.
        /// </remarks>
        public bool Enabled
        {
            get => Function.Call<bool>(Hash.IS_VEHICLE_EXTRA_TURNED_ON, Owner, (int)Index);
            set => Function.Call(Hash.SET_VEHICLE_EXTRA, Owner, (int)Index, !value);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="VehicleExtra"/> was detached.
        /// </summary>
        /// <remarks>
        /// To break off this <see cref="VehicleExtra"/>, use <see cref="BreakOff"/>.
        /// </remarks>
        public bool IsBrokenOff
        {
            get => Function.Call<bool>(Hash.IS_EXTRA_BROKEN_OFF, Owner, (int)Index);
        }

        /// <summary>
        /// Breaks off this <see cref="VehicleExtra"/>.
        /// </summary>
        /// <returns><see langword="true"/> if this <see cref="VehicleExtra"/> was detached; otherwise <see langword="false"/>.</returns>
        public bool BreakOff()
        {
            int fragmentGroup = Owner.Bones[BoneName].FragmentGroupIndex;
            return Owner.DetachFragmentPart(fragmentGroup);
        }

        /// <summary>
        /// Gets whether this <see cref="VehicleExtra"/> exists for the <see cref="Owner"/> vehicle.
        /// </summary>
        /// <remarks>
        /// If the extra is disabled, this will still return <see langword="true"/>.
        /// </remarks>
        /// <returns><see langword="true"/> if this <see cref="VehicleExtra"/> exists; otherwise <see langword="false"/>.</returns>
        public bool Exists() => Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Owner, (int)Index);
    }
}
