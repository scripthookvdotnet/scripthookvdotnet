using GTA.Math;
using GTA.Native;

namespace GTA
{
	public interface ISpatial
	{
		Vector3 Position { get; set; }
		Vector3 Rotation { get; set; }
	}

	public abstract class PoolObject : INativeValue
	{
		public PoolObject(int handle)
		{
			Handle = handle;
		}

		public int Handle { get; protected set; }
		public ulong NativeValue
		{
			get { return (ulong)Handle; }
			set { Handle = unchecked((int)value); }
		}

		public abstract bool Exists();
	}
}
