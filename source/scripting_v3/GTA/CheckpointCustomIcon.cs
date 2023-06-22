//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	public struct CheckpointCustomIcon : INativeValue
	{
		byte _number;
		CheckpointCustomIconStyle _style;

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

		public ulong NativeValue
		{
			get
			{
				if (_style == CheckpointCustomIconStyle.Number)
				{
					return Number;
				}

				return (byte)(90 + (int)_style * 10 + Number);
			}
			set
			{
				if (value > 219)
				{
					throw new ArgumentOutOfRangeException("The Range of possible values is 0 to 219");
				}

				if (value < 100)
				{
					_style = CheckpointCustomIconStyle.Number;
					_number = (byte)value;
				}
				else
				{
					_style = (CheckpointCustomIconStyle)(value / 10 - 9);
					_number = (byte)(value % 10);
				}
			}
		}

		/// <summary>
		/// Gets or sets the number to display inside the icon.
		/// </summary>
		/// <value>
		/// If <see cref="Style"/> is <see cref="CheckpointCustomIconStyle.Number"/>, allowed range is 0 - 99; otherwise allowed range is 0 - 9.
		/// </value>
		public byte Number
		{
			get => _number;
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

		/// <summary>
		/// Gets or sets the icon style.
		/// </summary>
		public CheckpointCustomIconStyle Style
		{
			get => _style;
			set
			{
				_style = value;
				if (value != CheckpointCustomIconStyle.Number && _number > 9)
				{
					_number = 0;
				}
			}
		}

		public static implicit operator byte(CheckpointCustomIcon icon)
		{
			return (byte)icon.NativeValue;
		}
		public static implicit operator CheckpointCustomIcon(byte value)
		{
			var c = new CheckpointCustomIcon();
			c.NativeValue = value;
			return c;
		}

		/// <summary>
		/// Converts a <see cref="CheckpointCustomIcon"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(CheckpointCustomIcon value)
		{
			return new InputArgument(value.NativeValue);
		}

		public override int GetHashCode()
		{
			return NativeValue.GetHashCode();
		}

		public override string ToString()
		{
			return Style.ToString() + Number.ToString();
		}
	}
}
