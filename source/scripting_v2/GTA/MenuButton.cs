//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuButton : IMenuItem
	{
		private UIText _text;
		private UIRectangle _button;
		private string _caption;

		public MenuButton(string caption) : this(caption, string.Empty)
		{
		}
		public MenuButton(string caption, string description)
		{
			this._caption = caption;
			Description = description;
		}

		public virtual void Draw()
		{
			if (_button == null || _text == null)
			{
				return;
			}

			_button.Draw();
			_text.Draw();
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
			Activated(this, EventArgs.Empty);
		}

		public virtual void Change(bool right)
		{
			// Nothing to do here
		}

		public virtual void SetOriginAndSize(Point origin, Size size)
		{
			_text = new UIText(
				Caption,
				Parent.ItemTextCentered ? new Point(origin.X + size.Width / 2 + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y) : new Point(origin.X + Parent.TextOffset.X, origin.Y + Parent.TextOffset.Y),
				Parent.ItemTextScale,
				Parent.UnselectedTextColor,
				Parent.ItemFont,
				Parent.ItemTextCentered);
			_button = new UIRectangle(
				origin,
				size,
				Parent.UnselectedItemColor);
		}

		private void UpdateText()
		{
			_text.Caption = Caption;
		}

		public event EventHandler<EventArgs> Activated;

		public string Caption
		{
			get => _caption;
			set
			{
				_caption = value;
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
