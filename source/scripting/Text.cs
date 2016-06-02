using System;
using System.Drawing;
using GTA.Native;

namespace GTA.UI
{
	public class Text : IElement
	{
		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public float Scale { get; set; }
		public Font Font { get; set; }
		public string Caption { get; set; }
		public bool Centered { get; set; }
		public bool Shadow { get; set; }
		public bool Outline { get; set; }

		public Text(string caption, PointF position, float scale) : this(caption, position, scale, Color.WhiteSmoke, Font.ChaletLondon, false, false, false)
		{
		}
		public Text(string caption, PointF position, float scale, Color color) : this(caption, position, scale, color, Font.ChaletLondon, false, false, false)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font, bool centered) : this(caption, position, scale, color, font, centered, false, false)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font, bool centered, bool shadow, bool outline)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Centered = centered;
			Shadow = shadow;
			Outline = outline;
		}

		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		public virtual void Draw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			float x = (Position.X + offset.Width) / Screen.Width;
			float y = (Position.Y + offset.Height) / Screen.Height;

			if (Shadow)
			{
				Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			}
			if (Outline)
			{
				Function.Call(Hash.SET_TEXT_OUTLINE);
			}

			Function.Call(Hash.SET_TEXT_FONT, (int)Font);
			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);
			Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			Function.Call(Hash.SET_TEXT_CENTRE, Centered);
			Function.Call(Hash._SET_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < Caption.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Caption.Substring(i, System.Math.Min(maxStringLength, Caption.Length - i)));
			}

			Function.Call(Hash._DRAW_TEXT, x, y);
		}
	}
}