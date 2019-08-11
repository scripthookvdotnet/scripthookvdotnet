using GTA.Native;

namespace GTA
{
	public class Notification
	{
		readonly int handle;

		internal Notification(int handle)
		{
			this.handle = handle;
		}

		public void Hide()
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, handle);
		}
	}
}
