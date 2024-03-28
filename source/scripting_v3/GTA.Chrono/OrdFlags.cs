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
    /// Ordinal (day of year) and year flags: `<c>(ordinal &lt;&lt; 4) | flags</c>`.
    /// </para>
    /// <para>
    /// The whole bits except for the least 3 bits are referred as `<c>Ol</c>` (ordinal and leap flag),
    /// which is an index to the <see cref="Internals.OlToMdl"/> lookup table.
    /// </para>
    /// <para>
    /// The methods implemented on <see cref="OrdFlags"/> always return a valid value.
    /// </para>
    /// </summary>
    internal readonly struct OrdFlags
    {
        private readonly uint _value;

        // OL: (ordinal << 1) | leap year flag (where the least bit indicates non-leap year)
        private const int MinOrdLeap = 1 << 1;
        private const int MaxOrdLeap = 366 << 1; // `(366 << 1) | 1` would be day 366 in a non-leap year

        internal OrdFlags(uint value) : this()
        {
            _value = value;
        }
        internal OrdFlags(int ordinal, YearFlags flags) : this()
        {
            _value = ((uint)ordinal << 4) + flags.Value;
        }

        internal uint Value => _value;

        internal uint Ordinal => (_value >> 4);

        internal static OrdFlags? New(int ordinal, YearFlags flags)
        {
            var of = new OrdFlags((int)ordinal, flags);
            return of.Validate();
        }

        internal OrdFlags? WithOrdinal(int ordinal)
        {
            if (ordinal < 0)
            {
                return null;
            }
            var of = new OrdFlags(((uint)ordinal << 4) | (_value & 0b1111));
            return of.Validate();
        }

        internal OrdFlags WithOrdinalUnchecked(uint ordinal)
        {
            return new OrdFlags((ordinal << 4) | (_value & 0b1111));
        }

        internal YearFlags Flags => new((byte)(_value & 0b1111));

        internal bool IsLeapYear => ((_value & 0b1000) == 0);

        internal static OrdFlags FromOrdinalDateUnchecked(int year, int ordinal)
        {
            var yearFlags = YearFlags.FromYear(year);
            return new OrdFlags(ordinal, yearFlags);
        }

        internal static OrdFlags? FromMonthDayFlags(MonthDayFlags mdf)
        {
            uint mdl = mdf.Value >> 3;
            if (mdl > Internals.MaxMdl)
            {
                // throwing an exception on out-of-bounds indexing would be reasonable, but just return null.
                return null;
            }
            // Array is indexed from `1` to `Internals.MaxMdl`, with a `0` index having a meaningless value.
            sbyte v = Internals.MdlToOl[mdl];
            var of = new OrdFlags(mdf.Value - (((uint)v & 0x3ff) << 3));
            return of.Validate();
        }

        internal MonthDayFlags ToMonthDayFlags() => MonthDayFlags.FromOrdFlags(this);

        internal uint Ol => _value >> 3;

        internal OrdFlags? Validate()
        {
            return Ol switch
            {
                (>= Internals.MinOl and <= Internals.MaxOl) => this,
                _ => null
            };
        }

        // Use ref values rather than ValueTuple. This method is internal use only and ValueTuple's performance is
        // noticeably worse than returning via ref values, which is different from how ValueTuple is handled in CoreCLR.
        internal void GetIsoWeekDateRaw(out uint ord, out IsoDayOfWeek weekday)
        {
            uint weekOrd = Ordinal + (uint)Flags.CalcIsoWeekDelta();

            ord = weekOrd / 7;
            weekday = GetIsoWeekdayFromU32Mod7(weekOrd);
        }

        internal IsoDayOfWeek IsoDayOfWeek => GetIsoWeekdayFromU32Mod7((_value >> 4) + (_value & 0b111));

        internal static IsoDayOfWeek GetIsoWeekdayFromU32Mod7(uint n)
        {
            return (n % 7) switch
            {
                0 => IsoDayOfWeek.Monday,
                1 => IsoDayOfWeek.Tuesday,
                2 => IsoDayOfWeek.Wednesday,
                3 => IsoDayOfWeek.Thursday,
                4 => IsoDayOfWeek.Friday,
                5 => IsoDayOfWeek.Saturday,
                _ => IsoDayOfWeek.Sunday,
            };
        }

        public bool Equals(OrdFlags other)
        {
            return _value == other._value;
        }
        public override bool Equals(object obj)
        {
            if (obj is OrdFlags flags)
            {
                return Equals(flags);
            }

            return false;
        }

        public static bool operator ==(OrdFlags left, OrdFlags right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(OrdFlags left, OrdFlags right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
