//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuLabel : IMenuItem
	{
		private UIText _text;
		private UIRectangle _button;
		private UIRectangle _underlineAbove;
		private UIRectangle _underlineBelow;
		private string _caption;

		public MenuLabel(string caption) : this(caption, false)
		{
		}
		public MenuLabel(string caption, bool underlined)
		{
			this._caption = caption;
			Description = string.Empty;
			UnderlineColor = Color.Black;
			UnderlineHeight = 2;
			UnderlinedAbove = false;
			UnderlinedBelow = underlined;
		}

		public virtual void Draw()
		{
			if (_button == null || _text == null)
			{
				return;
			}

			_button.Draw();
			_text.Draw();

			if (UnderlinedAbove && _underlineAbove != null)
			{
				_underlineAbove.Draw();
			}

			if (UnderlinedBelow && _underlineBelow != null)
			{
				_underlineBelow.Draw();
			}
		}
		public virtual void Draw(Size offset)
		{
			if (_button == null || _text == null)
			{
				return;
			}

			_button.Draw(offset);
			_text.Draw(offset);

			if (UnderlinedAbove && _underlineAbove != null)
			{
				_underlineAbove.Draw(offset);
			}

			if (UnderlinedBelow && _underlineBelow != null)
			{
				_underlineBelow.Draw(offset);
			}
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
		}

		public virtual void Change(bool right)
		{
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

			if (UnderlinedAbove)
			{
				_underlineAbove = new UIRectangle(
					new Point(origin.X, origin.Y),
					new Size(size.Width, UnderlineHeight),
					UnderlineColor);
			}
			if (UnderlinedBelow)
			{
				_underlineBelow = new UIRectangle(
					new Point(origin.X, origin.Y + size.Height - UnderlineHeight),
					new Size(size.Width, 2),
					UnderlineColor);
			}
		}

		private void UpdateText()
		{
			_text.Caption = Caption;
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
			get => _caption;
			set
			{
				_caption = value;
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
