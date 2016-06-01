using System;

namespace GTA
{
	public static class Test
	{
		public static void Func()
		{
			Native.Function.Call(Native.Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");
			Native.Function.Call(Native.Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "Test");
			Native.Function.Call<int>(Native.Hash._DRAW_NOTIFICATION, false, 1);
		}
	}
}
