using GTA.Math;
using GTA.Native;

namespace GTA
{
	/// <summary>
	/// An object with position and rotation information.
	/// </summary>
	public interface ISpatial
	{
		Vector3 Position { get; set; }
		Vector3 Rotation { get; set; }
	}

	/// <summary>
	/// An object that can exist in the world.
	/// </summary>
	public interface IExistable
	{
		bool Exists();
	}

	/// <summary>
	/// An object that can be deleted from the world.
	/// </summary>
	public interface IDeletable : IExistable
	{
		void Delete();
	}

	/// <summary>
	/// An object that resides in one of the available object pools.
	/// </summary>
	public abstract class PoolObject : INativeValue, IDeletable
	{
		protected PoolObject(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// The handle of the object.
		/// </summary>
		public int Handle { get; protected set; }

		/// <summary>
		/// The handle of the object translated to a native value.
		/// </summary>
		public ulong NativeValue
		{
			get { return (ulong)Handle; }
			set { Handle = unchecked((int)value); }
		}

		public abstract bool Exists();
		public abstract void Delete();
	}
}
