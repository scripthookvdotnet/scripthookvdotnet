using System;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// An enumeration of all possible component of the HUD.
	/// </summary>
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

	/// <summary>
	/// An enumeration of all possible cursor sprites.
	/// </summary>
	public enum CursorSprite
	{
		Normal = 1,
		LightArrow,
		OpenHand,
		GrabHand,
		MiddleFinger,
		LeftArrow,
		RightArrow,
		UpArrow,
		DownArrow,
		HorizontalDoubleArrow,
		NormalWithPlus,
		NormalWithMinus
	}

	/// <summary>
	/// Methods to manipulate the HUD (heads-up-display) of the game.
	/// </summary>
	public static class Hud
	{
		/// <summary>
		/// Determines whether a given <see cref="HudComponent"/> is active.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to check</param>
		/// <returns><c>true</c> if the <see cref="HudComponent"/> is active; otherwise, <c>false</c></returns>
		public static bool IsComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, component);
		}

		/// <summary>
		/// Draws the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/></param>
		///<remarks>This will only draw the <see cref="HudComponent"/> if the <see cref="HudComponent"/> can be drawn</remarks>
		public static void ShowComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, component);
		}
		/// <summary>
		/// Hides the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to hide.</param>
		public static void HideComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, component);
		}

		/// <summary>
		/// Shows the mouse cursor this frame.
		/// </summary>
		public static void ShowCursorThisFrame()
		{
			Function.Call(Hash._SHOW_CURSOR_THIS_FRAME);
		}

		/// <summary>
		/// Gets or sets the sprite the cursor should used when drawn
		/// </summary>
		public static CursorSprite CursorSprite
		{
			get { return (CursorSprite)MemoryAccess.ReadCursorSprite(); }
			set { Function.Call(Hash._SET_CURSOR_SPRITE, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether any HUD components should be rendered.
		/// </summary>
		public static bool IsVisible
		{
			get { return !Function.Call<bool>(Hash.IS_HUD_HIDDEN); }
			set { Function.Call(Hash.DISPLAY_HUD, value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the radar is visible.
		/// </summary>
		public static bool IsRadarVisible
		{
			get { return !Function.Call<bool>(Hash.IS_RADAR_HIDDEN); }
			set { Function.Call(Hash.DISPLAY_RADAR, value); }
		}

		/// <summary>
		/// Sets how far the minimap should be zoomed in.
		/// </summary>
		/// <value>
		/// The radar zoom; accepts values from 0 to 200.
		/// </value>
		public static int RadarZoom
		{
			set { Function.Call(Hash.SET_RADAR_ZOOM, value); }
		}
	}
}
