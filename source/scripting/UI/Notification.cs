using System;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manage the display of notifications above the minimap.
	/// </summary>
	public static class Notification
	{
		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification.</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink.</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide(int)"/>.</returns>
		public static int Show(string message, bool blinking = false)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			return Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, true);
		}

		/// <summary>
		/// Hides a <see cref="Notification"/> instantly.
		/// </summary>
		/// <param name="handle">The handle of the <see cref="Notification"/> to hide.</param>
		public static void Hide(int handle)
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, handle);
		}
	}
}
