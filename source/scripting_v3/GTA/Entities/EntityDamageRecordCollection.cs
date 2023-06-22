//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Collections;
using System.Collections.Generic;
using SHVDN;

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
				IntPtr address = _owner.MemoryAddress;
				if (address == IntPtr.Zero || !SHVDN.NativeMemory.IsIndexOfEntityDamageRecordValid(address, i))
				{
					yield break;
				}

				NativeMemory.EntityDamageRecordForReturnValue returnDamageRecord = SHVDN.NativeMemory.GetEntityDamageRecordEntryAtIndex(_owner.MemoryAddress, i);
				Entity attackerEntity = returnDamageRecord.attackerEntityHandle != 0 ? Entity.FromHandle(returnDamageRecord.attackerEntityHandle) : null;

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
			IntPtr address = _owner.MemoryAddress;
			if (address == IntPtr.Zero)
			{
				return Array.Empty<EntityDamageRecord>();
			}

			NativeMemory.EntityDamageRecordForReturnValue[] damageEntries = SHVDN.NativeMemory.GetEntityDamageRecordEntries(address);
			var returnDamageRecords = new EntityDamageRecord[damageEntries.Length];

			for (int i = 0; i < returnDamageRecords.Length; i++)
			{
				NativeMemory.EntityDamageRecordForReturnValue damageRecord = damageEntries[i];
				Entity attackerEntity = damageRecord.attackerEntityHandle != 0 ? Entity.FromHandle(damageRecord.attackerEntityHandle) : null;
				returnDamageRecords[i] = new EntityDamageRecord(_owner, attackerEntity, (WeaponHash)damageRecord.weaponHash, damageRecord.gameTime);
			}

			return returnDamageRecords;
		}
	}
}
