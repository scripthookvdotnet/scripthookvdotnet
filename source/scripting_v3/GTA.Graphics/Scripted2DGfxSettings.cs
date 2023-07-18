//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Drawing;

namespace GTA.Graphics
{
	/// <summary>
	/// Represents a static class to access global scripted 2D graphics settings.
	/// </summary>
	/// <remarks>
	/// Changing values that can be accessed by this class will affect all scripts including game ysc scripts
	/// and external scripts not for SHVDN.
	/// </remarks>
	public static class Scripted2DGfxSettings
	{
		/// <summary>
		/// Sets whether to display this scripted gfx when the game pauses where the pause menu is drawn.
		/// The setting is defaulted to off (<see langword="false"/>).
		/// </summary>
		public static bool DrawsBehindPauseMenu
		{
			set => Function.Call(Hash.SET_SCRIPT_GFX_DRAW_BEHIND_PAUSEMENU, value);
		}

		/// <summary>
		/// Sets scripted graphics draw order.
		/// The default setting is <see cref="ScriptedGfxDrawOrder.AfterHud"/>.
		/// </summary>
		public static ScriptedGfxDrawOrder DrawOrder
		{
			set => Function.Call(Hash.SET_SCRIPT_GFX_DRAW_ORDER, (int)value);
		}

		/// <summary>
		/// Sets the alignment type to the safezone.
		/// </summary>
		/// <param name="alignX">
		/// The x alignment type.
		/// The following is the list of acceptable values that make the game aligns 2D graphic elements differently:
		/// <see cref="UIAlignment.Left"/> or <see cref="UIAlignment.Right"/>.
		/// </param>
		/// <param name="alignY">
		///	The y alignment type.
		/// The following is the list of acceptable values that make the game aligns 2D graphic elements differently:
		/// <see cref="UIAlignment.Top"/> or <see cref="UIAlignment.Bottom"/>.
		/// </param>
		public static void SetAlignmentType(UIAlignment alignX, UIAlignment alignY)
			=> Function.Call(Hash.SET_SCRIPT_GFX_ALIGN, (byte)alignX, (byte)alignY);

		/// <summary>
		/// Sets the alignment offset and size.
		/// </summary>
		/// <param name="offset">
		/// <para>
		/// The <see cref="PointF"/> value to offset all x, y coords passed to 2d renderer,
		/// where 0 is at the top left corner of the screen and 1 is at the bottom right corner of the screen.
		/// </para>
		/// <para>
		/// Set <c>new PointF(0, 0)</c> to revert to the default value.
		/// The method will not assert that both offset values are within the range of 0 to 1 inclusive.
		/// </para>
		/// </param>
		/// <param name="size">
		/// <para>
		/// If you are aligned to the right or bottom of the screen, it assumes the x or y size of everything is this.
		/// This makes the calculations for positioning multiple UI elements of different sizes easier.
		/// Set this to the size of the largest element.
		/// </para>
		/// <para>
		/// Set <c>new SizeF(0, 0)</c> to revert to the default value.
		/// The method will not assert that both size values are within the range of 0 to 1 inclusive.
		/// </para>
		/// </param>
		public static void SetAlignmentOffsetAndSize(PointF offset, SizeF size)
			=> Function.Call(Hash.SET_SCRIPT_GFX_ALIGN_PARAMS, offset.X, offset.Y, size.Width, size.Height);

		/// <summary>
		/// Resets all the alignment parameters to unaligned with no offsets.
		/// </summary>
		/// <remarks>
		/// Calling this method has the same effect as calling <see cref="SetAlignmentType"/> with both parameters
		/// assigned to <see cref="UIAlignment.Ignore"/> and calling <see cref="SetAlignmentOffsetAndSize(PointF, SizeF)"/>
		/// with the zero offset and the zero size.
		/// </remarks>
		public static void ResetAlignment() => Function.Call(Hash.RESET_SCRIPT_GFX_ALIGN);

		/// <summary>
		/// Get a position on screen given the current alignment setup.
		/// </summary>
		/// <param name="offset">
		/// The input <see cref="PointF"/> value in screen space (not 1280x720 screen pixel space),
		/// where 0 is at the top left corner of the screen and 1 is at the bottom right corner of the screen.
		/// </param>
		public static PointF GetAlignPosition(PointF offset)
		{
			unsafe
			{
				float newX, newY;
				Function.Call(Hash.GET_SCRIPT_GFX_ALIGN_POSITION, offset.X, offset.Y, &newX, &newY);
				return new PointF(newX, newY);
			}
		}
	}
}
