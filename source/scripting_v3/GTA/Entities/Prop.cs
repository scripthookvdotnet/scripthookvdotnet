//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//
using GTA.Native;
using SHVDN;

namespace GTA
{
	public class Prop : Entity
	{
		internal Prop(int handle) : base(handle)
		{
		}

		#region Fragment

		/// <summary>
		/// Determines if this <see cref="Prop"/> is a broken off (detached) child fragment part of some <see cref="Entity"/>.
		/// </summary>
		public bool IsBrokenOffChildFragmentPart => NativeMemory.IsPropBrokenOffPart(Handle);

		/// <summary>
		/// Gets the <see cref="Entity"/> this <see cref="Prop"/> is detached from.
		/// Will return a <see cref="Vehicle"/> or <see cref="Prop"/> instance if found, and will return <see langword="null"/> if not found.
		/// </summary>
		public Entity ParentEntityDetachedFrom
		{
			get
			{
				var parentEntityHandle = NativeMemory.GetParentEntityHandleOfPropDetachedFrom(Handle);
				if (parentEntityHandle == 0)
					return null;

				return FromHandle(parentEntityHandle);
			}
		}

		#endregion

		/// <summary>
		/// Determines if this <see cref="Prop"/> exists.
		/// You should ensure <see cref="Prop"/>s still exist before manipulating them or getting some values for them on every tick, since some native functions may crash the game if invalid entity handles are passed.
		/// </summary>
		/// <returns><see langword="true" /> if this <see cref="Prop"/> exists; otherwise, <see langword="false" />.</returns>
		public new bool Exists()
		{
			return EntityType == EntityType.Prop;
		}
	}
}
