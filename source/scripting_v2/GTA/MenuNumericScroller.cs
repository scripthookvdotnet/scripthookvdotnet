//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuNumericScroller : IMenuItem
	{
		UIText text = null;
		UIRectangle button = null;
		int timesIncremented;

		public MenuNumericScroller(string caption, string description, double min, double max, double inc) : this(caption, description, min, max, inc, 0)
		{
		}
		public MenuNumericScroller(string caption, string description, double min, double max, double inc, int timesIncremented)
		{
			Caption = caption;
			Description = description;
			Min = min;
			Max = max;
			Increment = inc;
			DecimalFigures = -1;
			this.timesIncremented = timesIncremented;
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
			Activated(this, new MenuItemDoubleValueArgs(Value));
		}

		public virtual void Change(bool right)
		{
			if (right)
			{
				if (TimesIncremented + 1 > (int)((Max - Min) / Increment))
				{
					TimesIncremented = 0;
				}
				else
				{
					TimesIncremented++;
				}
			}
			else
			{
				if (TimesIncremented - 1 < 0)
				{
					TimesIncremented = (int)((Max - Min) / Increment);
				}
				else
				{
					TimesIncremented--;
				}
			}

			Changed(this, new MenuItemDoubleValueArgs(Value));
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
			double number = Min + Increment * TimesIncremented;
			string numberString;

			if (DecimalFigures == -1)
			{
				numberString = number.ToString();
			}
			else if (DecimalFigures == 0)
			{
				numberString = ((int)number).ToString();
			}
			else
			{
				numberString = number.ToString("F" + DecimalFigures);
			}

			text.Caption = Caption + " <" + numberString + ">";
		}

		public event EventHandler<MenuItemDoubleValueArgs> Changed;
		public event EventHandler<MenuItemDoubleValueArgs> Activated;

		public int DecimalFigures
		{
			get;
			set;
		}

		public int TimesIncremented
		{
			get => timesIncremented;
			set
			{
				timesIncremented = value;
				UpdateText();
			}
		}

		public double Min
		{
			get;
			set;
		}
		public double Max
		{
			get;
			set;
		}

		public double Value => timesIncremented * Increment;

		public double Increment
		{
			get;
			set;
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
