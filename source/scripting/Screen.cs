using System;
using System.Drawing;
using GTA.Math;
using GTA.Native;

namespace GTA.UI
{
	public enum HudComponent
	{
		WantedStars = 1,
		WeaponIcon,
		Cash,
		MpCash,
		MpMessage,
		VehicleName,
		AreaName,
		Unused,
		StreetName,
		HelpText,
		FloatingHelpText1,
		FloatingHelpText2,
		CashChange,
		Reticle,
		SubtitleText,
		RadioStationsWheel,
		Saving,
		GamingStreamUnusde,
		WeaponWheel,
		WeaponWheelStats,
		DrugsPurse01,
		DrugsPurse02,
		DrugsPurse03,
		DrugsPurse04,
		MpTagCashFromBank,
		MpTagPackages,
		MpTagCuffKeys,
		MpTagDownloadData,
		MpTagIfPedFollowing,
		MpTagKeyCard,
		MpTagRandomObject,
		MpTagRemoteControl,
		MpTagCashFromSafe,
		MpTagWeaponsPackage,
		MpTagKeys,
		MpVehicle,
		MpVehicleHeli,
		MpVehiclePlane,
		PlayerSwitchAlert,
		MpRankBar,
		DirectorMode,
		ReplayController,
		ReplayMouse,
		ReplayHeader,
		ReplayOptions,
		ReplayHelpText,
		ReplayMiscText,
		ReplayTopLine,
		ReplayBottomLine,
		ReplayLeftBar,
		ReplayTimer
	}

	public sealed class Notification
	{
		#region Fields
		int _handle;
		#endregion

		internal Notification(int handle)
		{
			_handle = handle;
		}

		/// <summary>
		/// Hides this <see cref="Notification"/> instantly
		/// </summary>
		public void Hide()
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, _handle);
		}
	}

	public static class Screen
	{
		/// <summary>
		/// The base width of the screen used for all UI Calculations, unless ScaledDraw is used
		/// </summary>
		public const float Width = 1280f;
		/// <summary>
		/// The base height of the screen used for all UI Calculations
		/// </summary>
		public const float Height = 720f;
		/// <summary>
		/// Gets the current Screen Aspect Ratio
		/// </summary>		   
		public static float AspectRatio
		{
			get
			{
				return Function.Call<float>(Hash._GET_SCREEN_ASPECT_RATIO, 0);
			}
		}
		/// <summary>
		/// Gets the width of the scaled against a 720pixel height base.
		/// </summary>
		public static float ScaledWidth
		{
			get
			{
				return Height * AspectRatio;
			}
		}
		/// <summary>
		/// Gets the actual Screen resolution the game is being rendered at
		/// </summary>
		public static Size Resolution
		{
			get
			{
				var width = new OutputArgument();
				var height = new OutputArgument();
				Function.Call(Hash._GET_SCREEN_ACTIVE_RESOLUTION, width, height);

				return new Size(width.GetResult<int>(), height.GetResult<int>());
			}
		}

		/// <summary>
		/// Shows a subtitle at the bottom of the screen for a given time
		/// </summary>
		/// <param name="message">The message to display.</param>
		/// <param name="duration">The duration to display the subtitle in milliseconds.</param>
		public static void ShowSubtitle(string message, int duration = 2500)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			Function.Call(Hash._DRAW_SUBTITLE_TIMED, duration, 1);
		}

		/// <summary>
		/// Displays a help message in the top corner of the screen this frame.
		/// </summary>
		/// <param name="helpText">The text to display.</param>
		public static void DisplayHelpTextThisFrame(string helpText)
		{
			Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "CELL_EMAIL_BCON");
			const int maxStringLength = 99;

			for (int i = 0; i < helpText.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, helpText.Substring(i, System.Math.Min(maxStringLength, helpText.Length - i)));
			}
			Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, 0, 1, -1);
		}

		/// <summary>
		/// Creates a <see cref="Notification"/> above the minimap with the given message.
		/// </summary>
		/// <param name="message">The message in the notification</param>
		/// <param name="blinking">if set to <c>true</c> the notification will blink</param>
		/// <returns>The handle of the <see cref="Notification"/> which can be used to hide it using <see cref="Notification.Hide()"/></returns>
		public static Notification ShowNotification(string message, bool blinking = false)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			return new Notification(Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, true));
		}

		/// <summary>
		/// Determines whether a given <see cref="HudComponent"/> is Active.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to check</param>
		/// <returns><c>true</c> if the <see cref="HudComponent"/> is Active; otherwise, <c>false</c></returns>
		public static bool IsHudComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, component);
		}
		/// <summary>
		/// Draws the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/></param>
		///<remarks>This will only draw the <see cref="HudComponent"/> if the <see cref="HudComponent"/> can be drawn</remarks>
		public static void ShowHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, component);
		}
		/// <summary>
		/// Hides the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to hide.</param>
		public static void HideHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, component);
		}

		/// <summary>
		/// Translates a point in WorldSpace to its given Coordinates on the <see cref="Screen"/>
		/// </summary>
		/// <param name="position">The position in the World</param>
		/// <param name="scaleWidth">if set to <c>true</c> Returns the screen position scaled by <see cref="ScaledWidth"/>; otherwise, returns the screen position scaled by <see cref="Width"/></param>
		/// <returns></returns>
		public static PointF WorldToScreen(Vector3 position, bool scaleWidth = false)
		{
			return WorldToScreen(position, scaleWidth ? ScaledWidth : Width, Height);
		}
		private static PointF WorldToScreen(Vector3 position, float screenWidth, float screenHeight)
		{
			var pointX = new OutputArgument();
			var pointY = new OutputArgument();

			if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, pointX, pointY))
			{
				return PointF.Empty;
			}

			return new PointF(pointX.GetResult<float>() * screenWidth, pointY.GetResult<float>() * screenHeight);
		}
		/// <summary>
		/// Gets a value indicating whether the screen is faded in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedIn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADED_IN);
			}
		}
		/// <summary>
		/// Gets a value indicating whether the screen is faded out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is faded out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadedOut
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADED_OUT);
			}
		}
		/// <summary>
		/// Gets a value indicating whether the screen is fading in.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading in; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingIn
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADING_IN);
			}
		}
		/// <summary>
		/// Gets a value indicating whether the screen is fading out.
		/// </summary>
		/// <value>
		/// <c>true</c> if the screen is fading out; otherwise, <c>false</c>.
		/// </value>
		public static bool IsFadingOut
		{
			get
			{
				return Function.Call<bool>(Hash.IS_SCREEN_FADING_OUT);
			}
		}
		/// <summary>
		/// Fades the screen in over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade in to take</param>
		public static void FadeIn(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_IN, time);
		}
		/// <summary>
		/// Fades the screen out over a specific time, useful for transitioning
		/// </summary>
		/// <param name="time">The time for the fade out to take</param>
		public static void FadeOut(int time)
		{
			Function.Call(Hash.DO_SCREEN_FADE_OUT, time);
		}
	}
}
