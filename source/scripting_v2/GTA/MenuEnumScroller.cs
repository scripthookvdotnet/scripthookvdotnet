//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuEnumScroller : IMenuItem
	{
		private UIText _text;
		private UIRectangle _button;
		private int _selectedIndex;
		private string[] _entries;

		public MenuEnumScroller(string caption, string description, string[] entries) : this(caption, description, entries, 0)
		{
		}
		public MenuEnumScroller(string caption, string description, string[] entries, int selectedIndex)
		{
			Caption = caption;
			Description = description;
			this._entries = entries;
			this._selectedIndex = selectedIndex;
		}

		public virtual void Draw()
		{
			Draw(default);
		}
		public virtual void Draw(Size offset)
		{
			if (_button == null || _text == null)
			{
				return;
			}

			_button.Draw(offset);
			_text.Draw(offset);
		}

		public virtual void Select()
		{
			if (_button == null)
			{
				return;
			}

			_button.Color = Parent.SelectedItemColor;
			_text.Color = Parent.SelectedTextColor;
		}
		public virtual void Deselect()
		{
			if (_button == null)
			{
				return;
			}

			_button.Color = Parent.UnselectedItemColor;
			_text.Color = Parent.UnselectedTextColor;
		}
		public virtual void Activate()
		{
			Activated(this, new MenuItemIndexArgs(Index));
		}

		public virtual void Change(bool right)
		{
			if (right)
			{
				if (Index + 1 > _entries.Length - 1)
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
					Index = _entries.Length - 1;
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
			_text = new UIText(
				string.Empty,
				Parent.ItemTextCentered ? new Point(origin.X + size.Width / 2 + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y) : new Point(origin.X + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y),
				Parent.ItemTextScale,
				Parent.UnselectedTextColor,
				Parent.ItemFont,
				Parent.ItemTextCentered);
			_button = new UIRectangle(
				origin,
				size,
				Parent.UnselectedItemColor);

			UpdateText();
		}

		private void UpdateText()
		{
			_text.Caption = Caption + " <" + _entries[_selectedIndex] + ">";
		}

		public event EventHandler<MenuItemIndexArgs> Changed;
		public event EventHandler<MenuItemIndexArgs> Activated;

		public virtual int Index
		{
			get => _selectedIndex;
			set
			{
				_selectedIndex = value;
				UpdateText();
			}
		}

		public virtual string Value => _entries[Index];
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
