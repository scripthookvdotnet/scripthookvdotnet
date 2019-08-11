using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.IO;

namespace GTA
{
	public static class UI
	{
		// TODO: These need the System.Runtime.CompilerServices.IsConst modopt
		public static int WIDTH = 1280;
		public static int HEIGHT = 720;

		public static Notification Notify(string message)
		{
			return Notify(message, false);
		}
		public static Notification Notify(string message, bool blinking)
		{
			Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "CELL_EMAIL_BCON");
			Function.PushLongString(message);

			return new Notification(Function.Call<int>(Hash._DRAW_NOTIFICATION, blinking, 1));
		}

		public static void ShowSubtitle(string message)
		{
			ShowSubtitle(message, 2500);
		}
		public static void ShowSubtitle(string message, int duration)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_2, "CELL_EMAIL_BCON");
			Function.PushLongString(message);
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
			DrawTexture(filename, index, level, time, pos, new PointF(0.0f, 0.0f), size, rotation, Color.White, 1.0f);
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

			if (_textures.ContainsKey(filename))
			{
				id = _textures[filename];
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

		internal static Dictionary<string, int> _textures = new Dictionary<string, int>();
	}
}
