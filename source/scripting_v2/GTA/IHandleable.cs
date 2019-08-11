namespace GTA
{
	public interface IHandleable
	{
		int Handle { get; }

		bool Exists();
	}
}
