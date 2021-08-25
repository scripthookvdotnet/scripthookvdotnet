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

		/// <summary>
		/// Determines if an <see cref="object"/> is an <see cref="EntityDamageLog"/> and has the same properties as this <see cref="EntityDamageLog"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><c>true</c> if the <paramref name="obj"/> is an <see cref="EntityDamageLog"/> and has the same properties as this <see cref="EntityDamageLog"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityDamageLog entityDamageLog)
			{
				return Victim == entityDamageLog.Victim &&
					Attacker == entityDamageLog.Attacker &&
					WeaponHash == entityDamageLog.WeaponHash &&
					GameTime == entityDamageLog.GameTime;
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="EntityDamageLog"/>s have the same properties.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same properties as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(EntityDamageLog left, EntityDamageLog right)
		{
			return left is null ? right is null : left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Entity"/>s do not have the same properties.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> does not have the same properties as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(EntityDamageLog left, EntityDamageLog right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			int attackerHandle = Attacker != null ? Attacker.Handle : 0;

			uint joaatHash = 0;
			var propertyValuesToHash = new int[4] { Victim.Handle, attackerHandle, (int)WeaponHash, GameTime };
			foreach (int propertyValue in propertyValuesToHash)
			{
				joaatHash += (uint)propertyValue;
				joaatHash += (joaatHash << 10);
				joaatHash ^= (joaatHash >> 6);
			}

			joaatHash += (joaatHash << 3);
			joaatHash ^= (joaatHash >> 11);
			joaatHash += (joaatHash << 15);

			return (int)joaatHash;
		}
	}
}
