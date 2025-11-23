using GTA.Native;

namespace GTA
{
    public class VehicleExtra
    {
        public Vehicle Owner { get; private set; }
        public VehicleExtraIndex Index { get; private set; }

        public string BoneName { get; private set; }

        internal VehicleExtra(Vehicle owner, VehicleExtraIndex index)
        {
            Owner = owner;
            Index = index;

            UpdateBoneName();
        }

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

        public bool Exists() => Function.Call<bool>(Hash.DOES_EXTRA_EXIST, Owner, (int)Index);

        private void UpdateBoneName()
        {
            BoneName = $"extra_{(Index == VehicleExtraIndex.Extra10 ? "ten" : (int)Index + 1)}";
        }
    }
}
