using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class WeaponComponentCollection
    {
        /// <summary>
        /// Gets the first component of all the components for <see cref="WeaponAttachmentPoint.GunRoot"/>.
        /// Despite the method name, return value is not guaranteed to a <see cref="WeaponComponent"/> instance that represents the luxury finish component.
        /// </summary>
        /// <returns>
        /// The <see cref="WeaponComponent"/> instance if the first component of all the components for <see cref="WeaponAttachmentPoint.GunRoot"/> is found;
        /// otherwise, the <see cref="WeaponComponent"/> instance representing the invalid component.
        /// </returns>
        [Obsolete("WeaponComponentCollection.GetLuxuryFinishComponent is wrongly named and cannot necessarily get all of the components for gun_root (e.g. camo components)," +
                  "use WeaponComponentCollection.GetGunRootComponent instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public WeaponComponent GetLuxuryFinishComponent()
        {
            foreach (WeaponComponent component in this)
            {
                if (component.AttachmentPoint == WeaponAttachmentPoint.GunRoot)
                {
                    return component;
                }
            }
            return _invalidComponent;
        }
    }
}
