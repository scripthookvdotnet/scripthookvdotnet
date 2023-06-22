//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manage the display of a loading spinner prompt.
	/// </summary>
	public static class LoadingPrompt
	{
		/// <summary>
		/// Gets a value indicating whether the Loading Prompt is currently being displayed
		/// </summary>
		public static bool IsActive => Function.Call<bool>(Hash.BUSYSPINNER_IS_ON);

		/// <summary>
		/// Creates a loading prompt at the bottom right of the screen with the given text and spinner type
		/// </summary>
		/// <param name="loadingText">The text to display next to the spinner</param>
		/// <param name="spinnerType">The style of spinner to draw</param>
		/// <remarks>
		/// <see cref="LoadingSpinnerType.Clockwise1"/>, <see cref="LoadingSpinnerType.Clockwise2"/>, <see cref="LoadingSpinnerType.Clockwise3"/> and <see cref="LoadingSpinnerType.RegularClockwise"/> all see to be the same.
		/// But Rockstar apparently always uses <see cref="LoadingSpinnerType.RegularClockwise"/> in their scripts.
		/// </remarks>
		public static void Show(string loadingText = null, LoadingSpinnerType spinnerType = LoadingSpinnerType.RegularClockwise)
		{
			Hide();

			if (string.IsNullOrEmpty(loadingText))
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, SHVDN.NativeMemory.NullString);
			}
			else
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_BUSYSPINNER_ON, SHVDN.NativeMemory.String);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, loadingText);
			}

			Function.Call(Hash.END_TEXT_COMMAND_BUSYSPINNER_ON, (int)spinnerType);
		}

		/// <summary>
		/// Remove the loading prompt at the bottom right of the screen
		/// </summary>
		public static void Hide()
		{
			if (IsActive)
			{
				Function.Call(Hash.BUSYSPINNER_OFF);
			}
		}
	}
}
