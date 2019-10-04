//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuLabel : IMenuItem
	{
		UIText text = null;
		UIRectangle button = null;
		UIRectangle underlineAbove = null;
		UIRectangle underlineBelow = null;
		string caption;

		public MenuLabel(string caption) : this(caption, false)
		{
		}
		public MenuLabel(string caption, bool underlined)
		{
			this.caption = caption;
			Description = string.Empty;
			UnderlineColor = Color.Black;
			UnderlineHeight = 2;
			UnderlinedAbove = false;
			UnderlinedBelow = underlined;
		}

		public virtual void Draw()
		{
			if (button == null || text == null)
			{
				return;
			}

			button.Draw();
			text.Draw();

			if (UnderlinedAbove && underlineAbove != null)
			{
				underlineAbove.Draw();
			}

			if (UnderlinedBelow && underlineBelow != null)
			{
				underlineBelow.Draw();
			}
		}
		public virtual void Draw(Size offset)
		{
			if (button == null || text == null)
			{
				return;
			}

			button.Draw(offset);
			text.Draw(offset);

			if (UnderlinedAbove && underlineAbove != null)
			{
				underlineAbove.Draw(offset);
			}

			if (UnderlinedBelow && underlineBelow != null)
			{
				underlineBelow.Draw(offset);
			}
		}

		public virtual void Select()
		{
			if (button == null)
			{
				return;
			}

			button.Color = Parent.SelectedItemColor;
			text.Color = Parent.SelectedTextColor;
		}
		public virtual void Deselect()
		{
			if (button == null)
			{
				return;
			}

			button.Color = Parent.UnselectedItemColor;
			text.Color = Parent.UnselectedTextColor;
		}
		public virtual void Activate()
		{
		}

		public virtual void Change(bool right)
		{
		}

		public virtual void SetOriginAndSize(Point origin, Size size)
		{
			text = new UIText(
				Caption,
				Parent.ItemTextCentered ? new Point(origin.X + size.Width / 2 + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y) : new Point(origin.X + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y),
				Parent.ItemTextScale,
				Parent.UnselectedTextColor,
				Parent.ItemFont,
				Parent.ItemTextCentered);
			button = new UIRectangle(
				origin,
				size,
				Parent.UnselectedItemColor);

			if (UnderlinedAbove)
			{
				underlineAbove = new UIRectangle(
					new Point(origin.X, origin.Y),
					new Size(size.Width, UnderlineHeight),
					UnderlineColor);
			}
			if (UnderlinedBelow)
			{
				underlineBelow = new UIRectangle(
					new Point(origin.X, origin.Y + size.Height - UnderlineHeight),
					new Size(size.Width, 2),
					UnderlineColor);
			}
		}

		void UpdateText()
		{
			text.Caption = Caption;
		}

		public int UnderlineHeight
		{
			get;
			set;
		}

		public bool UnderlinedAbove
		{
			get;
			set;
		}
		public bool UnderlinedBelow
		{
			get;
			set;
		}

		public Color UnderlineColor
		{
			get;
			set;
		}

		public virtual string Caption
		{
			get => caption;
			set
			{
				caption = value;
				UpdateText();
			}
		}
		public virtual string Description
		{
			get;
			set;
		}

		public virtual MenuBase Parent
		{
			get;
			set;
		}
	}
}
