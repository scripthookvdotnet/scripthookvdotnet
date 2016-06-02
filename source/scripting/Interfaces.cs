using GTA.Math;

namespace GTA
{
	public interface IHandleable
	{
		int Handle { get; }

		bool Exists();
	}

	public interface ISpatial
	{
		Vector3 Position { get; set; }
		Vector3 Rotation { get; set; }
	}
}
