//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Drawing;

namespace GTA
{
	public class UIText : UIElement
	{
		public UIText(string caption, Point position, float scale) : this(caption, position, scale, Color.WhiteSmoke, Font.ChaletLondon, false, false, false)
		{
		}
		public UIText(string caption, Point position, float scale, Color color) : this(caption, position, scale, color, Font.ChaletLondon, false, false, false)
		{
		}
		public UIText(string caption, Point position, float scale, Color color, Font font, bool centered) : this(caption, position, scale, color, font, centered, false, false)
		{
		}
		public UIText(string caption, Point position, float scale, Color color, Font font, bool centered, bool shadow, bool outline)
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

		public virtual bool Enabled
		{
			get; set;
		}
		public virtual Point Position
		{
			get; set;
		}
		public virtual Color Color
		{
			get; set;
		}
		public string Caption
		{
			get; set;
		}
		public Font Font
		{
			get; set;
		}
		public float Scale
		{
			get; set;
		}
		public bool Centered
		{
			get; set;
		}
		public bool Shadow
		{
			get; set;
		}
		public bool Outline
		{
			get; set;
		}

		public virtual void Draw()
		{
			Draw(new Size());
		}
		public virtual void Draw(Size offset)
		{
			if (!Enabled)
			{
				return;
			}

			float x = (float)(Position.X + offset.Width) / UI.WIDTH;
			float y = (float)(Position.Y + offset.Height) / UI.HEIGHT;

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
			Function.Call(Hash.SET_TEXT_CENTRE, Centered ? 1 : 0);
			Function.Call(Hash._SET_TEXT_ENTRY, "CELL_EMAIL_BCON");
			SHVDN.NativeFunc.PushLongString(Caption);
			Function.Call(Hash._DRAW_TEXT, x, y);
		}
	}
}
