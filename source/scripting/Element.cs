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
		bool Centered { get; set; }

		void Draw();
		void Draw(SizeF offset);
		void ScaledDraw();
		void ScaledDraw(SizeF offset);
	}

	public class Rectangle : IElement
	{
		public virtual bool Enabled { get; set; }
		public virtual Color Color { get; set; }
		public virtual PointF Position { get; set; }
		public virtual bool Centered { get; set; }
		public SizeF Size { get; set; }

		public Rectangle() : this(PointF.Empty, new SizeF(Screen.Width, Screen.Height), Color.Transparent, false)
		{
		}
		public Rectangle(PointF position, SizeF size) : this(position, size, Color.Transparent, false)
		{
		}
		public Rectangle(PointF position, SizeF size, Color color) : this(position, size, color, false)
		{
		}
		public Rectangle(PointF position, SizeF size, Color color, bool centered)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
			Centered = centered;
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
			float x = ((Position.X + offset.Width) / Screen.Width) + ((!Centered) ? w * 0.5f : 0.0f);
			float y = ((Position.Y + offset.Height) / Screen.Height) + ((!Centered) ? h * 0.5f : 0.0f);

			Function.Call(Hash.DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
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

			float w = Size.Width / Screen.ScaledWidth;
			float h = Size.Height / Screen.Height;
			float x = ((Position.X + offset.Width) / Screen.ScaledWidth) + ((!Centered) ? w * 0.5f : 0.0f);
			float y = ((Position.Y + offset.Height) / Screen.Height) + ((!Centered) ? h * 0.5f : 0.0f);

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
		public Container(PointF position, SizeF size, Color color, bool centered) : base(position, size, color, centered)
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

			SizeF newOfset = new SizeF(Position + offset);

			if (Centered)
			{
				newOfset -= new SizeF(Size.Width / 2.0f, Size.Height / 2.0f);
			}

			foreach (var item in Items)
			{
				item.Draw(newOfset);
			}
		}
		public override void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		public override void ScaledDraw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			base.ScaledDraw(offset);

			SizeF newOfset = new SizeF(Position + offset);

			if (Centered)
			{
				newOfset -= new SizeF(Size.Width / 2.0f, Size.Height / 2.0f);
			}

			foreach (var item in Items)
			{
				item.ScaledDraw(newOfset);
			}
		}
	}
}
