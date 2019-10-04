//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct CheckpointCustomIcon
	{
		private CheckpointCustomIconStyle _style;
		private byte _number;

		public CheckpointCustomIconStyle Style
		{
			get
			{
				return _style;
			}
			set
			{
				_style = value;
				if (value != CheckpointCustomIconStyle.Number)
				{
					if (_number > 9)
					{
						_number = 0;
					}
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CheckpointCustomIcon" /> struct.
		/// </summary>
		/// <param name="iconStyle">The icon style.</param>
		/// <param name="iconNumber">The icon number,
		/// if <paramref name="iconStyle"/> is <see cref="CheckpointCustomIconStyle.Number"/> allowed range is 0 - 99
		/// otherwise allowed range is 0 - 9. </param>
		public CheckpointCustomIcon(CheckpointCustomIconStyle iconStyle, byte iconNumber)
		{
			//initialise them so vs doesnt complain
			_style = CheckpointCustomIconStyle.Number;
			_number = 0;

			Style = iconStyle;
			Number = iconNumber;
		}

		/// <summary>
		/// Gets or sets the number to display inside the icon.
		/// </summary>
		/// <value>
		/// The number.
		/// if <see cref="Style"/> is <see cref="CheckpointCustomIconStyle.Number"/> allowed range is 0 - 99
		/// otherwise allowed range is 0 - 9.
		/// </value>
		public byte Number
		{
			get
			{
				return _number;
			}
			set
			{
				if (_style == CheckpointCustomIconStyle.Number)
				{
					if (value > 99)
					{
						throw new ArgumentOutOfRangeException("The maximum number value is 99");
					}
					_number = value;
				}
				else
				{
					if (value > 9)
					{
						throw new ArgumentOutOfRangeException("The maximum number value when not using CheckpointCustomIconStyle.Number is 9");
					}
					_number = value;
				}

			}
		}

		private byte getValue()
		{
			if (_style == CheckpointCustomIconStyle.Number)
			{
				return _number;
			}
			return (byte)(90 + (int)_style * 10 + _number);
		}


		public static implicit operator InputArgument(CheckpointCustomIcon icon)
		{
			return new InputArgument((int)icon.getValue());
		}

		public static implicit operator byte(CheckpointCustomIcon icon)
		{
			return icon.getValue();
		}

		public static implicit operator CheckpointCustomIcon(byte value)
		{
			var c = new CheckpointCustomIcon();
			if (value > 219)
			{
				throw new ArgumentOutOfRangeException("The Range of possible values is 0 to 219");
			}
			if (value < 100)
			{
				c._style = CheckpointCustomIconStyle.Number;
				c._number = value;
			}
			else
			{
				c._style = (CheckpointCustomIconStyle)(value / 10 - 9);
				c._number = (byte)(value % 10);
			}
			return c;
		}

		public override string ToString()
		{
			return Style.ToString() + Number.ToString();
		}

		public override int GetHashCode()
		{
			return getValue().GetHashCode();
		}
	}
}
