using System;
using System.ComponentModel;

namespace GTA
{
    public partial class Projectile : Prop
    {
        /// <summary>
        /// Gets the <see cref="Ped"/> this <see cref="Projectile"/> belongs to.
        /// Can be <see langword="null" /> or a <see cref="Ped"/> instance whose handle is for <see cref="Vehicle"/>, which is not valid as a <see cref="Ped"/> instance.
        /// </summary>
        [Obsolete("The Projectile.Owner is obsolete in the v3 API because the actual owner can be a Vehicle, use Projectile.OwnerEntity instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public Ped Owner
        {
            get
            {
                int ownerHandle = GetOwnerEntityInternal();
                return ownerHandle != 0 ? new Ped(ownerHandle) : null;
            }
        }
    }
}
