//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System.Drawing;

namespace GTA
{
	public class UIRectangle : UIElement
	{
		public UIRectangle() : this(new Point(), new Size(UI.WIDTH, UI.HEIGHT), Color.Transparent)
		{
		}
		public UIRectangle(Point position, Size size) : this(position, size, Color.Transparent)
		{
		}
		public UIRectangle(Point position, Size size, Color color)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
		}

		public virtual bool Enabled
		{
			get; set;
		}
		public virtual Point Position
		{
			get; set;
		}
		public Size Size
		{
			get; set;
		}
		public virtual Color Color
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

			float w = (float)Size.Width / UI.WIDTH;
			float h = (float)Size.Height / UI.HEIGHT;
			float x = (float)(Position.X + offset.Width) / UI.WIDTH + w * 0.5f;
			float y = (float)(Position.Y + offset.Height) / UI.HEIGHT + h * 0.5f;

			Function.Call(Hash.DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}
	}
}
