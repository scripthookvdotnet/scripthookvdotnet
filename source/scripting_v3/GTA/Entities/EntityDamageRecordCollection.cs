//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace GTA
{
	public sealed class EntityDamageRecordCollection : IEnumerable<EntityDamageRecord>, IEnumerable
	{
		#region Fields
		private readonly Entity _owner;
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
				var attackerEntity = returnDamageRecord.attackerEntityHandle != 0 ? Entity.FromHandle(returnDamageRecord.attackerEntityHandle) : null;

				yield return new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)returnDamageRecord.weaponHash, returnDamageRecord.gameTime);
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
				return Array.Empty<EntityDamageRecord>();
			}

			var damageEntries = SHVDN.NativeMemory.GetEntityDamageRecordEntries(address);
			var returnDamageRecords = new EntityDamageRecord[damageEntries.Length];

			for (var i = 0; i < returnDamageRecords.Length; i++)
			{
				var damageRecord = damageEntries[i];
				var attackerEntity = damageRecord.attackerEntityHandle != 0 ? Entity.FromHandle(damageRecord.attackerEntityHandle) : null;
				returnDamageRecords[i] = new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)damageRecord.weaponHash, damageRecord.gameTime);
			}

			return returnDamageRecords;
		}
	}
}
