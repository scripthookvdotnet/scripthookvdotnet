using System;
using System.Drawing;
using System.Collections.Generic;
using GTA.Native;

namespace GTA.UI
{
	public interface IElement
	{
		bool Enabled { get; set; }
		Color Color { get; set; }
		PointF Position { get; set; }

		void Draw();
		void Draw(SizeF offset);
	}

	public class Rectangle : IElement
	{
		public virtual bool Enabled { get; set; }
		public virtual Color Color { get; set; }
		public virtual PointF Position { get; set; }
		public SizeF Size { get; set; }

		public Rectangle() : this(PointF.Empty, new SizeF(Screen.Width, Screen.Height), Color.Transparent)
		{
		}
		public Rectangle(PointF position, SizeF size) : this(position, size, Color.Transparent)
		{
		}
		public Rectangle(PointF position, SizeF size, Color color)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
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

			float w = Size.Width / Screen.Width;
			float h = Size.Height / Screen.Height;
			float x = ((Position.X + offset.Width) / Screen.Width) + w * 0.5f;
			float y = ((Position.Y + offset.Height) / Screen.Height) + h * 0.5f;

			Function.Call(Hash.DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}
	}
	public class Container : Rectangle
	{
		public List<IElement> Items { get; private set; }

		public Container()
		{
			Items = new List<IElement>();
		}
		public Container(PointF position, SizeF size) : base(position, size)
		{
			Items = new List<IElement>();
		}
		public Container(PointF position, SizeF size, Color color) : base(position, size, color)
		{
			Items = new List<IElement>();
		}

		public override void Draw()
		{
			Draw(SizeF.Empty);
		}
		public override void Draw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			base.Draw(offset);

			foreach (var item in Items)
			{
				item.Draw(new SizeF(Position + offset));
			}
		}
	}
}
