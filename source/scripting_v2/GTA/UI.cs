//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GTA
{
	public static class UI
	{
		// These two definitions need to have 'modopt(System.Runtime.CompilerServices.IsConst)'
		public static int WIDTH = 1280;
		public static int HEIGHT = 720;

		public static Notification Notify(string message)
		{
			return Notify(message, false);
		}
		public static Notification Notify(string message, bool blinking)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");
			SHVDN.NativeFunc.PushLongString(message);

			return new Notification(Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, true));
		}

		public static void ShowSubtitle(string message)
		{
			ShowSubtitle(message, 2500);
		}
		public static void ShowSubtitle(string message, int duration)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");
			SHVDN.NativeFunc.PushLongString(message);
			Function.Call(Hash._DRAW_SUBTITLE_TIMED, duration, true);
		}

		public static void ShowHelpMessage(string message)
		{
			ShowHelpMessage(message, 5000, true);
		}
		public static void ShowHelpMessage(string message, bool sound)
		{
			ShowHelpMessage(message, 5000, sound);
		}
		public static void ShowHelpMessage(string message, int duration)
		{
			ShowHelpMessage(message, duration, true);
		}
		public static void ShowHelpMessage(string message, int duration, bool sound)
		{
			Function.Call(Hash._SET_TEXT_COMPONENT_FORMAT, "CELL_EMAIL_BCON");
			SHVDN.NativeFunc.PushLongString(message);
			Function.Call(Hash._DISPLAY_HELP_TEXT_FROM_STRING_LABEL, 0, false, sound, duration);
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

		public static Point WorldToScreen(Vector3 position)
		{
			float pointX, pointY;
			unsafe
			{
				if (!Function.Call<bool>(Hash._WORLD3D_TO_SCREEN2D, position.X, position.Y, position.Z, &pointX, &pointY))
				{
					return default;
				}
			}

			return new Point((int)(pointX * WIDTH), (int)(pointY * HEIGHT));
		}

		public static void DrawTexture(string filename, int index, int level, int time, Point pos, Size size)
		{
			DrawTexture(filename, index, level, time, pos, new PointF(0.0f, 0.0f), size, 0.0f, Color.White, 1.0f);
		}
		public static void DrawTexture(string filename, int index, int level, int time, Point pos, Size size, float rotation, Color color)
		{
			DrawTexture(filename, index, level, time, pos, new PointF(0.0f, 0.0f), size, rotation, color, 1.0f);
		}
		public static void DrawTexture(string filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color)
		{
			DrawTexture(filename, index, level, time, pos, center, size, rotation, color, 1.0f);
		}
		public static void DrawTexture(string filename, int index, int level, int time, Point pos, PointF center, Size size, float rotation, Color color, float aspectRatio)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(filename);
			}

			int id;

			if (_textures.TryGetValue(filename, out int texture))
			{
				id = texture;
			}
			else
			{
				id = SHVDN.NativeMemory.CreateTexture(filename);

				_textures.Add(filename, id);
			}

			float x = (float)pos.X / WIDTH;
			float y = (float)pos.Y / HEIGHT;
			float w = (float)size.Width / WIDTH;
			float h = (float)size.Height / HEIGHT;

			SHVDN.NativeMemory.DrawTexture(id, index, level, time, w, h, center.X, center.Y, x, y, rotation, aspectRatio, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		private static readonly Dictionary<string, int> _textures = new();
	}
}
