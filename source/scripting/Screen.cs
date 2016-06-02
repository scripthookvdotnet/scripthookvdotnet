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
		public static float Width = 1280f, Height = 720f;

		public static Notification Notify(string message)
		{
			return Notify(message, false);
		}
		public static Notification Notify(string message, bool blinking)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			return new Notification(Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, 1));
		}

		public static void ShowSubtitle(string message)
		{
			ShowSubtitle(message, 2500);
		}
		public static void ShowSubtitle(string message, int duration)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < message.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, message.Substring(i, System.Math.Min(maxStringLength, message.Length - i)));
			}

			Function.Call(Hash._DRAW_SUBTITLE_TIMED, duration, 1);
		}

		public static bool IsHudComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, (int)component);
		}
		public static void ShowHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, (int)component);
		}
		public static void HideHudComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, (int)component);
		}

		public static PointF WorldToScreen(Vector3 position)
		{
			var pointX = new OutputArgument();
			var pointY = new OutputArgument();

			if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, pointX, pointY))
			{
				return PointF.Empty;
			}

			return new PointF(pointX.GetResult<float>() * Width, pointY.GetResult<float>() * Height);
		}
	}
}
