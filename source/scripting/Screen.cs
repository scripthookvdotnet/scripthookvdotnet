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

		public void Hide()
		{
			Function.Call(Hash._REMOVE_NOTIFICATION, _handle);
		}
	}

	public static class Screen
	{
		public const float Width = 1280f, Height = 720f;
		public static float AspectRatio
		{
			get
			{
				return Function.Call<float>(Hash._GET_SCREEN_ASPECT_RATIO, 0);
			}
		}
		public static float ScaledWidth
		{
			get
			{
				return Height * AspectRatio;
			}
		}
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

		public static bool IsHudComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, component);
		}
		public static void ShowHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, component);
		}
		public static void HideHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, component);
		}

		public static PointF WorldToScreen(Vector3 position, bool scaleWidth = false)
		{
			return WorldToScreen(position, scaleWidth ? ScaledWidth : Width, Height);
		}
		public static PointF WorldToScreen(Vector3 position, float screenWidth, float screenHeight)
		{
			var pointX = new OutputArgument();
			var pointY = new OutputArgument();

			if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, pointX, pointY))
			{
				return PointF.Empty;
			}

			return new PointF(pointX.GetResult<float>() * screenWidth, pointY.GetResult<float>() * screenHeight);
		}
	}
}
