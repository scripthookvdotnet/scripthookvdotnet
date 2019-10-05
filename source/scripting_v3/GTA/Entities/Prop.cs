//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public sealed class Prop : Entity
	{
		internal Prop(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Determines if this <see cref="Prop"/> exists.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="Prop"/> exists; otherwise, <c>false</c>.</returns>
		public new bool Exists()
		{
			return EntityType == EntityType.Prop;
		}
	}
}
