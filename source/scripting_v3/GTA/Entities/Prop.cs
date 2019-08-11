namespace GTA
{
	public sealed class Prop : Entity
	{
		public Prop(int handle) : base(handle)
		{
		}

		/// <summary>
		/// Determines whether this <see cref="Prop"/> exists.
		/// </summary>
		/// <returns><c>true</c> if this <see cref="Prop"/> exists; otherwise, <c>false</c></returns>
		public new bool Exists()
		{
			return EntityType == EntityType.Prop;
		}
	}
}
