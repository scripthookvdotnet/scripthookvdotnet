//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public class Prop : Entity
	{
		internal Prop(int handle) : base(handle)
		{
		}

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
