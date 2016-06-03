using System;
using System.Drawing;
using GTA.Native;

namespace GTA.UI
{
	public enum TextAlignment
	{
		Center = 0,
		Left = 1,
		Right = 2,
	}
	public class Text : IElement
	{
		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public float Scale { get; set; }
		public Font Font { get; set; }
		public string Caption { get; set; }
		public TextAlignment Alignment { get; set; }
		public bool Shadow { get; set; }
		public bool Outline { get; set; }
		public float WrapWidth { get; set; }
		public bool Centered
		{
			get
			{
				return Alignment == TextAlignment.Center;
			}
			set
			{
				if (value)
				{
					Alignment = TextAlignment.Center;
				}
			}
		}
		public float Width
		{
			get
			{
				return GetStringWidth(Caption, Font, Scale);
			}
		}
		public float ScaledWidth
		{
			get
			{
				return GetScaledStringWidth(Caption, Font, Scale);
			}
		}


		public Text(string caption, PointF position, float scale) : this(caption, position, scale, Color.WhiteSmoke, Font.ChaletLondon, TextAlignment.Left, false, false, 0.0f)
		{
		}
		public Text(string caption, PointF position, float scale, Color color) : this(caption, position, scale, color, Font.ChaletLondon, TextAlignment.Left, false, false, 0.0f)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font) : this(caption, position, scale, color, font, TextAlignment.Left, false, false, 0.0f)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment) : this(caption, position, scale, color, font, alignment, false, false, 0.0f)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment, bool shadow, bool outline) : this(caption, position, scale, color, font, alignment, shadow, outline, 0.0f)
		{
		}
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment, bool shadow, bool outline, float wrapWidth)
		{
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Alignment = alignment;
			Shadow = shadow;
			Outline = outline;
			WrapWidth = wrapWidth;
		}
		public static float GetStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_FOR_WIDTH, "CELL_EMAIL_BCON");
			const int maxStringLength = 99;
			for (int i = 0; i < text.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, System.Math.Min(maxStringLength, text.Length - i)));
			}
			Function.Call(Hash.SET_TEXT_FONT, font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
			return Screen.Width * Function.Call<float>(Hash._GET_TEXT_SCREEN_WIDTH, 1);
		}
		public static float GetScaledStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_FOR_WIDTH, "CELL_EMAIL_BCON");
			const int maxStringLength = 99;
			for (int i = 0; i < text.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, System.Math.Min(maxStringLength, text.Length - i)));
			}
			Function.Call(Hash.SET_TEXT_FONT, font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);
			return Screen.ScaledWidth * Function.Call<float>(Hash._GET_TEXT_SCREEN_WIDTH, 1);
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
			float w = WrapWidth / Screen.Width;

			if (Shadow)
			{
				Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			}
			if (Outline)
			{
				Function.Call(Hash.SET_TEXT_OUTLINE);
			}

			Function.Call(Hash.SET_TEXT_FONT, Font);
			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);
			Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			Function.Call(Hash.SET_TEXT_JUSTIFICATION, Alignment);
			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case TextAlignment.Center:
						Function.Call(Hash.SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case TextAlignment.Left:
						Function.Call(Hash.SET_TEXT_WRAP, x, x + w);
						break;
					case TextAlignment.Right:
						Function.Call(Hash.SET_TEXT_WRAP, x - w, x);
						break;
				}
			}
			else if (Alignment == TextAlignment.Right)
			{
				Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
			}
			Function.Call(Hash._SET_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < Caption.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Caption.Substring(i, System.Math.Min(maxStringLength, Caption.Length - i)));
			}

			Function.Call(Hash._DRAW_TEXT, x, y);
		}
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		public virtual void ScaledDraw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			float x = (Position.X + offset.Width) / Screen.ScaledWidth;
			float y = (Position.Y + offset.Height) / Screen.Height;
			float w = WrapWidth / Screen.ScaledWidth;

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
			Function.Call(Hash.SET_TEXT_JUSTIFICATION, (int)Alignment);
			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case TextAlignment.Center:
						Function.Call(Hash.SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case TextAlignment.Left:
						Function.Call(Hash.SET_TEXT_WRAP, x, x + w);
						break;
					case TextAlignment.Right:
						Function.Call(Hash.SET_TEXT_WRAP, x - w, x);
						break;
				}
			}
			else if (Alignment == TextAlignment.Right)
			{
				Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
			}
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