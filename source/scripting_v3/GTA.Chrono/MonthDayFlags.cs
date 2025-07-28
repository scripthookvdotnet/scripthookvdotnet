//
// Copyright 2012-2014 The Rust Project Developers
// Copyright 2023 kagikn & contributors
// Licensed under the MIT License.
// Original license:
// https://github.com/chronotope/chrono/blob/a44142528eb8bab32f9e16cb74e84bb060f4a667/LICENSE.txt
//

namespace GTA.Chrono
{
    /// <summary>
    /// <para>
    /// Month, day of month and year flags: `<c>(month &lt;&lt; 9) | (day &lt;&lt; 4) | flags</c>`
    /// </para>
    /// <para>
    /// The whole bits except for the least 3 bits are referred as `<c>Mdl</c>` (month, day of month and leap flag),
    /// which is an index to the <see cref="Internals.MdlToOl"/> lookup table.
    /// </para>
    /// <para>
    /// The methods implemented on <see cref="MonthDayFlags"/> do not always return a valid value.
    /// Dates that can't exist, like February 30, can still be represented.
    /// Use <see cref="IsValid"/> to check whether the date is valid.
    /// </para>
    /// </summary>
    internal readonly struct MonthDayFlags
    {
        private readonly uint _value;

        internal MonthDayFlags(uint value) : this()
        {
            _value = value;
        }
        internal MonthDayFlags(int month, int day, YearFlags flags) : this()
        {
            _value = (uint)((month << 9) | (day << 4) | flags.Value);
        }

        internal uint Value => _value;

        internal static MonthDayFlags? New(int month, int day, YearFlags flags)
        {
            if (month < 1 || month > 12 || day < 1 || day > 31)
            {
                return null;
            }

            return new MonthDayFlags(month, day, flags);
        }

        internal static MonthDayFlags FromOrdFlags(OrdFlags of)
        {
            uint ol = of.Value >> 3;
            if (ol <= Internals.MaxOl)
            {
                // Array is indexed from `1 to Internals.MaxOl`, with a `0` index having a meaningless value.
                return new MonthDayFlags(of.Value + ((uint)Internals.OlToMdl[ol] << 3));
            }
            else
            {
                // throwing an exception here would be reasonable, but we are just going on with a safe value.
                return new MonthDayFlags(0);
            }
        }

        internal bool IsValid
        {
            get
            {
                uint mdl = _value >> 3;
                if (mdl > Internals.MaxMdl)
                {
                    return false;
                }

                return Internals.MdlToOl[mdl] >= 0;
            }
        }

        internal int Month => (int)(_value >> 9);

        internal OrdFlags? ToOrdFlags()
        {
            return OrdFlags.FromMonthDayFlags(this);
        }

        internal MonthDayFlags? WithMonth(int month)
        {
            if (month < 0 || month > 12)
            {
                return null;
            }

            return new MonthDayFlags((_value & 0b1_1111_1111) | ((uint)month << 9));
        }

        internal int Day => (int)((_value >> 4) & 0b1_1111);

        internal MonthDayFlags? WithDay(int day)
        {
            if (day < 0 || day > 31)
            {
                return null;
            }

            return new MonthDayFlags((_value & ~0b1_1111_0000u) | ((uint)day << 4));
        }

        internal MonthDayFlags WithFlags(YearFlags flags) => new((_value & ~0b1111u) | (flags.Value));

        public bool Equals(MonthDayFlags other)
        {
            return _value == other._value;
        }
        public override bool Equals(object obj)
        {
            if (obj is MonthDayFlags flags)
            {
                return Equals(flags);
            }

            return false;
        }

        public static bool operator ==(MonthDayFlags left, MonthDayFlags right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(MonthDayFlags left, MonthDayFlags right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
