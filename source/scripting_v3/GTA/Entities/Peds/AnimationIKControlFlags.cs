//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
	[Flags]
	public enum AnimationIKControlFlags
	{
		/// <summary>
		/// No Ik control during the task.
		/// </summary>
		None = 0,
		/// <summary>
		/// Disable leg ik during the task.
		/// </summary>
		DisableLegIK = 1,
		/// <summary>
		/// Disable arm ik during the task.
		/// </summary>
		DisableArmIK = 2,
		/// <summary>
		/// Disable head ik during the task.
		/// </summary>
		DisableHeadIK = 4,
		/// <summary>
		/// Disable torso ik during the task.
		/// </summary>
		DisableTorsoIK = 8,
		/// <summary>
		/// Disable torso react ik during the task.
		/// </summary>
		DisableTorsoReactIK = 16,
		/// <summary>
		/// Use anim leg allow tags to determine when leg ik is enabled.
		/// </summary>
		UseLegAllowTags = 32,
		/// <summary>
		/// Use anim leg block tags to determine when leg ik is disabled.
		/// </summary>
		UseLegBlockTags = 64,
		/// <summary>
		/// Use anim arm allow tags to determine when ik is enabled.
		/// </summary>
		UseArmAllowTags = 128,
		/// <summary>
		/// Use anim arm block tags to determine when ik is disabled.
		/// </summary>
		UseArmBlockTags = 256,
		/// <summary>
		/// Process the left hand weapon grip ik during the task.
		/// </summary>
		ProcessWeaponHandGrip = 512,
		/// <summary>
		/// Use first person ik setup for left arm (cannot be used with AIK_DISABLE_ARM_IK).
		/// </summary>
		UseFirstPersonArmLeft = 1024,
		/// <summary>
		/// Use first person ik setup for right arm (cannot be used with AIK_DISABLE_ARM_IK).
		/// </summary>
		UseFirstPersonArmRight = 2048,
		/// <summary>
		/// Disable torso vehicle ik during the task.
		/// </summary>
		DisableTorsoVehicleIK = 4096,
		/// <summary>
		/// Searches the dictionary of the clip being played for another clip with the _facial suffix to be played as a facial animation.
		/// </summary>
		LinkedFacial = 8192
	}
}
