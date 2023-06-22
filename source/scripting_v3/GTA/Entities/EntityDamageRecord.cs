//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public readonly struct EntityDamageRecord
	{
		internal EntityDamageRecord(Entity victim, Entity attacker, WeaponHash weaponHash, int gameTime)
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

		public void Deconstruct(out Entity attacker, out WeaponHash weaponHash, out int gameTime)
		{
			attacker = Attacker;
			weaponHash = WeaponHash;
			gameTime = GameTime;
		}

		public void Deconstruct(out Entity victim, out Entity attacker, out WeaponHash weaponHash, out int gameTime)
		{
			victim = Victim;
			attacker = Attacker;
			weaponHash = WeaponHash;
			gameTime = GameTime;
		}

		/// <summary>
		/// Determines if <paramref name="entityDamageRecord"/> has the same properties as this <see cref="EntityDamageRecord"/>.
		/// </summary>
		/// <param name="entityDamageRecord">The <see cref="object"/> to check.</param>
		/// <returns><c>true</c> if the <paramref name="entityDamageRecord"/> has the same properties as this <see cref="EntityDamageRecord"/>; otherwise, <c>false</c>.</returns>
		public bool Equals(EntityDamageRecord entityDamageRecord)
		{
			return Victim == entityDamageRecord.Victim &&
				Attacker == entityDamageRecord.Attacker &&
				WeaponHash == entityDamageRecord.WeaponHash &&
				GameTime == entityDamageRecord.GameTime;
		}
		/// <summary>
		/// Determines if an <see cref="object"/> is an <see cref="EntityDamageRecord"/> and has the same properties as this <see cref="EntityDamageRecord"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to check.</param>
		/// <returns><c>true</c> if the <paramref name="obj"/> is an <see cref="EntityDamageRecord"/> and has the same properties as this <see cref="EntityDamageRecord"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityDamageRecord entityDamageRecord)
			{
				Equals(entityDamageRecord);
			}

			return false;
		}

		/// <summary>
		/// Determines if two <see cref="EntityDamageRecord"/>s have the same properties.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> has the same properties as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator ==(EntityDamageRecord left, EntityDamageRecord right)
		{
			return left.Equals(right);
		}
		/// <summary>
		/// Determines if two <see cref="Entity"/>s do not have the same properties.
		/// </summary>
		/// <param name="left">The left <see cref="Entity"/>.</param>
		/// <param name="right">The right <see cref="Entity"/>.</param>
		/// <returns><c>true</c> if <paramref name="left"/> does not have the same properties as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
		public static bool operator !=(EntityDamageRecord left, EntityDamageRecord right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			int hash = 17;
			hash = Victim != null ? hash * 5039 + Victim.Handle.GetHashCode() : hash;
			hash = Attacker != null ? hash * 883 + Attacker.Handle.GetHashCode() : hash;
			hash = hash * 9719 + ((int)WeaponHash).GetHashCode();
			hash = hash * 317 + GameTime.GetHashCode();
			return hash;
		}
	}
}
