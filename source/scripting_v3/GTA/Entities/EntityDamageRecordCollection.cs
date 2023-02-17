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
				var address = _owner.MemoryAddress;
				if (address == IntPtr.Zero || !SHVDN.NativeMemory.IsIndexOfEntityDamageRecordValid(address, i))
				{
					yield break;
				}

				var returnDamageRecord = SHVDN.NativeMemory.GetEntityDamageRecordEntryAtIndex(_owner.MemoryAddress, i);

				(int attackerHandle, int weaponHash, int gameTime) = returnDamageRecord;
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
			var address = _owner.MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return new EntityDamageRecord[0];
			}

			var damageEntries = SHVDN.NativeMemory.GetEntityDamageRecordEntries(address);
			var returnDamageRecords = new EntityDamageRecord[damageEntries.Length];

			for (int i = 0; i < returnDamageRecords.Length; i++)
			{
				(int attackerHandle, int weaponHash, int gameTime) = damageEntries[i];
				var attackerEntity = attackerHandle != 0 ? Entity.FromHandle(attackerHandle) : null;
				returnDamageRecords[i] = new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)weaponHash, gameTime);
			}

			return returnDamageRecords;
		}
	}
}
