//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum PhysicsDampingType
	{
		/// <summary>
		/// The linear damping constant.
		/// </summary>
		LinearC,
		/// <summary>
		/// The linear damping coefficient proportional to velocity.
		/// </summary>
		LinearV,
		/// <summary>
		/// The linear damping coefficient proportional to velocity squared.
		/// </summary>
		LinearV2,
		/// <summary>
		/// The angular damping constant.
		/// </summary>
		AngularC,
		/// <summary>
		/// The angular damping coefficient proportional to velocity.
		/// </summary>
		AngularV,
		/// <summary>
		/// The angular damping coefficient proportional to velocity squared.
		/// </summary>
		AngularV2,
	}
}
