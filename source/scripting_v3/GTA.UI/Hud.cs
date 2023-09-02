//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;

namespace GTA.UI
{
	/// <summary>
	/// Methods to manipulate the HUD (heads-up-display) of the game.
	/// </summary>
	public static class Hud
	{
		/// <summary>
		/// Determines whether a given <see cref="HudComponent"/> is active.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to check</param>
		/// <returns><see langword="true" /> if the <see cref="HudComponent"/> is active; otherwise, <see langword="false" /></returns>
		public static bool IsComponentActive(HudComponent component)
		{
			return Function.Call<bool>(Hash.IS_HUD_COMPONENT_ACTIVE, (int)component);
		}

		/// <summary>
		/// Draws the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/></param>
		///<remarks>This will only draw the <see cref="HudComponent"/> if the <see cref="HudComponent"/> can be drawn</remarks>
		public static void ShowComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.SHOW_HUD_COMPONENT_THIS_FRAME, (int)component);
		}
		/// <summary>
		/// Hides the specified <see cref="HudComponent"/> this frame.
		/// </summary>
		/// <param name="component">The <see cref="HudComponent"/> to hide.</param>
		public static void HideComponentThisFrame(HudComponent component)
		{
			Function.Call(Hash.HIDE_HUD_COMPONENT_THIS_FRAME, (int)component);
		}

		/// <summary>
		/// Shows the mouse cursor this frame.
		/// </summary>
		public static void ShowCursorThisFrame()
		{
			Function.Call(Hash.SET_MOUSE_CURSOR_THIS_FRAME);
		}

		/// <summary>
		/// Gets or sets the sprite the cursor should used when drawn
		/// </summary>
		public static CursorSprite CursorSprite
		{
			get => (CursorSprite)SHVDN.NativeMemory.CursorSprite;
			set => Function.Call(Hash.SET_MOUSE_CURSOR_STYLE, (int)value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether any HUD components should be rendered.
		/// </summary>
		public static bool IsVisible
		{
			get => !Function.Call<bool>(Hash.IS_HUD_HIDDEN);
			set => Function.Call(Hash.DISPLAY_HUD, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the radar is visible.
		/// </summary>
		public static bool IsRadarVisible
		{
			get => !Function.Call<bool>(Hash.IS_RADAR_HIDDEN);
			set => Function.Call(Hash.DISPLAY_RADAR, value);
		}

		/// <summary>
		/// Locks the minimap at the specified angle relative to the main map layout.
		/// If a value less than 0 or greater than 360 is used, the minimap angle will become unlocked.
		/// </summary>
		/// <param name="angle">The angle in degrees from 0-360 to lock the minimap to.</param>
		public static void LockRadarDirection(int angle) => Function.Call(Hash.LOCK_MINIMAP_ANGLE, angle);

		/// <summary>
		/// Unlocks the minimap direction if locked.
		/// </summary>
		public static void UnlockRadarDirection() => Function.Call(Hash.UNLOCK_MINIMAP_ANGLE);

		/// <summary>
		/// Locks the minimap at the specified position in the game world.
		/// </summary>
		/// <param name="position">The world coordinates to lock the minimap to.</param>
		public static void LockRadarPosition(Vector2 position) => Function.Call(Hash.LOCK_MINIMAP_POSITION, position.X, position.Y);

		/// <summary>
		/// Unlocks the minimap position if it has been locked.
		/// </summary>
		public static void UnlockRadarPosition() => Function.Call(Hash.UNLOCK_MINIMAP_POSITION);

		/// <summary>
		/// Sets how far the minimap should be zoomed in.
		/// </summary>
		/// <value>
		/// The radar zoom; accepts values from 0 to 200.
		/// </value>
		public static int RadarZoom
		{
			set => Function.Call(Hash.SET_RADAR_ZOOM, value);
		}
	}
}
