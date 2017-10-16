using System;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// An enumeration of possible loading spinner styles.
	/// </summary>
	public enum LoadingSpinnerType
	{
		Clockwise1 = 1,
		Clockwise2,
		Clockwise3,
		SocialClubSaving,
		RegularClockwise
	}

	/// <summary>
	/// Methods to manage the display of a loading spinner prompt.
	/// </summary>
	public static class LoadingPrompt
	{
		/// <summary>
		/// Gets a value indicating whether the Loading Prompt is currently being displayed
		/// </summary>
		public static bool IsActive => Function.Call<bool>(Hash._IS_LOADING_PROMPT_BEING_DISPLAYED);

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

			if (loadingText == null)
			{
				Function.Call(Hash._SET_LOADING_PROMPT_TEXT_ENTRY, MemoryAccess.NullString);
			}
			else
			{
				Function.Call(Hash._SET_LOADING_PROMPT_TEXT_ENTRY, MemoryAccess.StringPtr);
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, loadingText);
			}

			Function.Call(Hash._SHOW_LOADING_PROMPT, spinnerType);
		}

		/// <summary>
		/// Remove the loading prompt at the bottom right of the screen
		/// </summary>
		public static void Hide()
		{
			if (IsActive)
			{
				Function.Call(Hash._REMOVE_LOADING_PROMPT);
			}
		}
	}
}
