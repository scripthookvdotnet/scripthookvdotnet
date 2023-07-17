//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MenuNumericScroller : IMenuItem
	{
		private UIText _text;
		private UIRectangle _button;
		private int _timesIncremented;

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
			this._timesIncremented = timesIncremented;
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
			double number = Min + Increment * TimesIncremented;
			string numberString;

			switch (DecimalFigures)
			{
				case -1:
					numberString = number.ToString();
					break;
				case 0:
					numberString = ((int)number).ToString();
					break;
				default:
					numberString = number.ToString($"F{DecimalFigures.ToString()}");
					break;
			}

			_text.Caption = $"{Caption} <{numberString}>";
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
			get => _timesIncremented;
			set
			{
				_timesIncremented = value;
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

		public double Value => _timesIncremented * Increment;

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
