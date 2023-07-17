//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	/// <summary>
	/// An enumeration of most of the possible values for <see cref="Ped.SetToRagdollWithFall"/>.
	/// </summary>
	/// <remarks>
	/// Most of the values listed are named after the values of the enum <c>eRagdollType</c>,
	/// which is used in <c>RagdollType</c> in <c>pedbounds.xml</c>.
	/// </remarks>
	public enum RagdollFallType
	{
		Male = 0,
		Female = 1,
		MaleLarge = 2,
		/// <summary>
		/// Behaves similar to <see cref="Male"/>, but the <see cref="Ped"/> will be killed
		/// after a ragdoll fall task ends (by a <c>CEventDeath</c>).
		/// </summary>
		FallToDeath = 4,
	}
}
