//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuEnumScroller : IMenuItem
	{
		UIText text = null;
		UIRectangle button = null;
		int selectedIndex;
		string[] entries;

		public MenuEnumScroller(string caption, string description, string[] entries) : this(caption, description, entries, 0)
		{
		}
		public MenuEnumScroller(string caption, string description, string[] entries, int selectedIndex)
		{
			Caption = caption;
			Description = description;
			this.entries = entries;
			this.selectedIndex = selectedIndex;
		}

		public virtual void Draw()
		{
			Draw(default);
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
			Activated(this, new MenuItemIndexArgs(Index));
		}

		public virtual void Change(bool right)
		{
			if (right)
			{
				if (Index + 1 > entries.Length - 1)
				{
					Index = 0;
				}
				else
				{
					Index++;
				}
			}
			else
			{
				if (Index - 1 < 0)
				{
					Index = entries.Length - 1;
				}
				else
				{
					Index--;
				}
			}

			Changed(this, new MenuItemIndexArgs(Index));
		}

		public virtual void SetOriginAndSize(Point origin, Size size)
		{
			text = new UIText(
				string.Empty,
				Parent.ItemTextCentered ? new Point(origin.X + size.Width / 2 + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y) : new Point(origin.X + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y),
				Parent.ItemTextScale,
				Parent.UnselectedTextColor,
				Parent.ItemFont,
				Parent.ItemTextCentered);
			button = new UIRectangle(
				origin,
				size,
				Parent.UnselectedItemColor);

			UpdateText();
		}

		void UpdateText()
		{
			text.Caption = Caption + " <" + entries[selectedIndex] + ">";
		}

		public event EventHandler<MenuItemIndexArgs> Changed;
		public event EventHandler<MenuItemIndexArgs> Activated;

		public virtual int Index
		{
			get => selectedIndex;
			set
			{
				selectedIndex = value;
				UpdateText();
			}
		}

		public virtual string Value => entries[Index];
		public virtual string Caption
		{
			get; set;
		}
		public virtual string Description
		{
			get; set;
		}

		public virtual MenuBase Parent
		{
			get; set;
		}
	}
}
