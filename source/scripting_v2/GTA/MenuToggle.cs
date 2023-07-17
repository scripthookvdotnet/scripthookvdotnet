//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuToggle : IMenuItem
	{
		private UIText _text;
		private UIRectangle _button;
		private bool _toggleSelection;

		public MenuToggle(string caption, string description) : this(caption, description, false)
		{
		}
		public MenuToggle(string caption, string description, bool value)
		{
			Caption = caption;
			Description = description;
			_toggleSelection = value;
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
			ChangeSelection();
		}

		public virtual void Change(bool right)
		{
			ChangeSelection();
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
			_text.Caption = Caption + (_toggleSelection ? " <ON>" : " <OFF>");
		}

		private void ChangeSelection()
		{
			Value = !_toggleSelection;

			Changed(this, EventArgs.Empty);
		}

		public event EventHandler<EventArgs> Changed;

		public virtual bool Value
		{
			get => _toggleSelection;
			set
			{
				_toggleSelection = value;
				UpdateText();
			}
		}

		public virtual string Caption
		{
			get;
			set;
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
