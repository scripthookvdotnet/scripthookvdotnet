//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public class EntityDamageLogCollection
	{
		#region Fields
		protected readonly Entity _owner;
		#endregion

		internal EntityDamageLogCollection(Entity owner)
		{
			_owner = owner;
		}

		/// <summary>
		/// Gets all the <see cref="EntityDamageLog" /> at the moment.
		/// The return array can contain up to 3 <see cref="EntityDamageLog" />s.
		/// </summary>
		public EntityDamageLog[] GetAllDamageLogs()
		{
			var memoryAddress = _owner.MemoryAddress;
			if (memoryAddress == IntPtr.Zero)
			{
				return new EntityDamageLog[0];
			}

			var damageEntries = SHVDN.NativeMemory.GetEntityDamageLogEntries(memoryAddress);
			var returnDamageLogs = new EntityDamageLog[damageEntries.Length];

			for (int i = 0; i < returnDamageLogs.Length; i++)
			{
				(int attackerHandle, int weaponHash, int gameTime) = damageEntries[i];
				var attackerEntity = attackerHandle != -1 ? Entity.FromHandle(attackerHandle) : null;
				returnDamageLogs[i] = new EntityDamageLog(_owner, attackerEntity, (WeaponHash)weaponHash, gameTime);
			}

			return returnDamageLogs;
		}
	}
}
