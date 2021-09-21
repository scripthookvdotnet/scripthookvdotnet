//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using GTA.NaturalMotion;

namespace GTA
{
	public enum RagdollType
	{
		/// <summary>
		/// <see cref="Ped"/>s will fall with their muscle relax, just like when <see cref="Ped"/>s' healths are set to zero and get killed by setting the healths.
		/// </summary>
		Relax = 0,
		/// <summary>
		/// You can control <see cref="Ped"/>s' ragdoll behaviors by additional configrations. Consider using the <see cref="Euphoria"/> class for advanced and easier ragdoll configrations.
		/// </summary>
		ScriptControl = 1,
		/// <summary>
		/// <see cref="Ped"/>s will try to balance.
		/// </summary>
		Balance = 2,
		[Obsolete]
		Normal = 0,
		[Obsolete]
		StiffLegs,
		[Obsolete]
		NarrowLegs,
		[Obsolete]
		WideLegs,
	}
}
