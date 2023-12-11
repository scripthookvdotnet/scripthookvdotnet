//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA
{
    /// <summary>
    /// The apply force type.
    /// </summary>
    public enum ForceType
    {
        /// <summary>
        /// Add a continuous internal force to the entity.
        /// Force itself cannot detach any fragment parts of props like <see cref="ExternalForce"/> can.
        /// </summary>
        InternalForce,
        /// <summary>
        /// Add an instant internal impulse to the entity.
        /// Impulse itself cannot detach any fragment parts of props like <see cref="ExternalImpulse"/> can.
        /// </summary>
        InternalImpulse,
        /// <summary>
        /// Add a continuous external force to the entity.
        /// Unlike <see cref="InternalForce"/>, force itself can detach any fragment parts of props.
        /// </summary>
        ExternalForce,
        /// <summary>
        /// Add an instant external impulse to the entity.
        /// Unlike <see cref="InternalImpulse"/>, impulse itself can detach any fragment parts of props.
        /// </summary>
        ExternalImpulse,
        /// <summary>
        /// Add a torque to the entity.
        /// </summary>
        Torque,
        /// <summary>
        /// Add an angular impulse to the entity.
        /// Basically works just like <see cref="Torque"/>, but the force will be multiplied by 102.931, which is
        /// calculated by <c>1.75f / (0.0340035 / 2.0f)</c>.
        /// </summary>
        AngularImpulse,

        [Obsolete("ForceType.MinForce is obsolete because it is incorrect, use ForceType.InternalForce instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        MinForce = InternalForce,
        [Obsolete("ForceType.MaxForceRot is obsolete because it is incorrect, use ForceType.InternalImpulse instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        MaxForceRot = InternalImpulse,
        [Obsolete("ForceType.MinForce2 is obsolete because it is incorrect, use ForceType.ExternalForce instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        MinForce2 = ExternalForce,
        [Obsolete("ForceType.MaxForceRot2 is obsolete because it is incorrect, use ForceType.ExternalImpulse instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        MaxForceRot2 = ExternalImpulse,
        [Obsolete("ForceType.ForceNoRot is obsolete because it is incorrect, use ForceType.Torque instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        ForceNoRot = Torque,
        [Obsolete("ForceType.ForceRotPlusForce is obsolete because it is incorrect, use ForceType.AngularImpulse instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        ForceRotPlusForce = AngularImpulse,
    }
}
