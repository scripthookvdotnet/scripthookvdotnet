//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuToggle : IMenuItem
	{
		UIText text = null;
		UIRectangle button = null;
		bool toggleSelection;

		public MenuToggle(string caption, string description) : this(caption, description, false)
		{
		}
		public MenuToggle(string caption, string description, bool value)
		{
			Caption = caption;
			Description = description;
			toggleSelection = value;
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
			ChangeSelection();
		}

		public virtual void Change(bool right)
		{
			ChangeSelection();
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
			text.Caption = Caption + (toggleSelection ? " <ON>" : " <OFF>");
		}

		void ChangeSelection()
		{
			Value = !toggleSelection;

			Changed(this, EventArgs.Empty);
		}

		public event EventHandler<EventArgs> Changed;

		public virtual bool Value
		{
			get => toggleSelection;
			set
			{
				toggleSelection = value;
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
