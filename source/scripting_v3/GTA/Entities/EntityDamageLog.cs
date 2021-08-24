//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA
{
	public class EntityDamageLog
	{
		internal EntityDamageLog(Entity victim, Entity attacker, WeaponHash weaponHash, int gameTime)
		{
			Victim = victim;
			Attacker = attacker;
			WeaponHash = weaponHash;
			GameTime = gameTime;
		}

		/// <summary>
		/// Gets the victim <see cref="Entity" />.
		/// </summary>
		public Entity Victim
		{
			get;
		}

		/// <summary>
		/// Gets the attacker <see cref="Entity" />. Can be <c>null</c>.
		/// </summary>
		public Entity Attacker
		{
			get;
		}

		/// <summary>
		/// Gets the game time when the <see cref="Victim" /> took damage.
		/// </summary>
		public int GameTime
		{
			get;
		}

		/// <summary>
		/// Gets the weapon hash what the <see cref="Victim" /> took damage with.
		/// </summary>
		public WeaponHash WeaponHash
		{
			get;
		}
	}
}
