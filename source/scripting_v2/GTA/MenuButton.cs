//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuButton : IMenuItem
	{
		UIText text = null;
		UIRectangle button = null;
		string caption;

		public MenuButton(string caption) : this(caption, string.Empty)
		{
		}
		public MenuButton(string caption, string description)
		{
			this.caption = caption;
			Description = description;
		}

		public virtual void Draw()
		{
			if (button == null || text == null)
			{
				return;
			}

			button.Draw();
			text.Draw();
		}
		public virtual void Draw(Size offset)
		{
			if (button == null || text == null)
			{
				return;
			}

			button.Draw(offset);
			text.Draw(offset);
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
			Activated(this, EventArgs.Empty);
		}

		public virtual void Change(bool right)
		{
			// Nothing to do here
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
		}

		void UpdateText()
		{
			text.Caption = Caption;
		}

		public event EventHandler<EventArgs> Activated;

		public string Caption
		{
			get => caption;
			set
			{
				caption = value;
				UpdateText();
			}
		}
		public string Description
		{
			get; set;
		}

		public MenuBase Parent
		{
			get; set;
		}
	}
}
