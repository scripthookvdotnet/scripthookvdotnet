//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum EntityPopulationType
	{
		/// <summary>
		/// The game does not automatically delete entities when this value is set.
		/// </summary>
		Unknown,
		/// <summary>
		/// The game does not automatically delete entities when this value is set.
		/// </summary>
		RandomPermanent,
		/// <summary>
		/// This value is set when parked vehicles are created.
		/// </summary>
		RandomParked,
		RandomPatrol,
		/// <summary>
		/// This value is set when scenario peds are created.
		/// </summary>
		RandomScenario,
		/// <summary>
		/// This value is set when ambient entities are created or when SET_ENTITY_AS_NO_LONGER_NEEDED is called.
		/// </summary>
		RandomAmbient,
		/// <summary>
		/// The game does not automatically delete entities when this value is set.
		/// </summary>
		Permanent,
		/// <summary>
		/// This value is set when entities are created via native functions or when SET_ENTITY_AS_MISSION_ENTITY is called.
		/// The game does not automatically delete entities when this value is set.
		/// </summary>
		Mission,
		/// <summary>
		/// The game does not automatically delete entities when this value is set.
		/// </summary>
		Replay,
		Cache,
		Tool,
	}
}
