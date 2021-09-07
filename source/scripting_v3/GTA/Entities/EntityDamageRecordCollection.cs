//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public class EntityDamageRecordCollection : IEnumerable<EntityDamageRecord>, IEnumerable
	{
		#region Fields
		protected readonly Entity _owner;
		#endregion

		internal EntityDamageRecordCollection(Entity owner)
		{
			_owner = owner;
		}

		public IEnumerator<EntityDamageRecord> GetEnumerator()
		{
			// No more than 3 damage records
			for (uint i = 0; i < 3; i++)
			{
				var memoryAddress = _owner.MemoryAddress;

				if (memoryAddress == IntPtr.Zero || SHVDN.NativeMemory.IsIndexOfEntityDamageLogValid(memoryAddress, i))
					yield break;

				var returnDamageLog = SHVDN.NativeMemory.GetEntityDamageLogEntryAtIndex(_owner.MemoryAddress, i);

				(int attackerHandle, int weaponHash, int gameTime) = returnDamageLog;
				var attackerEntity = attackerHandle != 0 ? Entity.FromHandle(attackerHandle) : null;
				yield return new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)weaponHash, gameTime);
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Gets all the <see cref="EntityDamageRecord" /> at the moment.
		/// The return array can contain up to 3 <see cref="EntityDamageRecord" />s.
		/// </summary>
		public EntityDamageRecord[] GetAllDamageRecords()
		{
			var memoryAddress = _owner.MemoryAddress;

			if (memoryAddress == IntPtr.Zero)
				return new EntityDamageRecord[0];

			var damageEntries = SHVDN.NativeMemory.GetEntityDamageLogEntries(memoryAddress);
			var returnDamageLogs = new EntityDamageRecord[damageEntries.Length];

			for (int i = 0; i < returnDamageLogs.Length; i++)
			{
				(int attackerHandle, int weaponHash, int gameTime) = damageEntries[i];
				var attackerEntity = attackerHandle != 0 ? Entity.FromHandle(attackerHandle) : null;
				returnDamageLogs[i] = new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)weaponHash, gameTime);
			}

			return returnDamageLogs;
		}
	}
}
