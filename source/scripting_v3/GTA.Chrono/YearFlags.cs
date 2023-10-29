//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Chrono
{
	/// <summary>
	/// The YearFlags stores this information into 4 bits `<c>abbb</c>`,
	/// where `<c>a</c>` is `<c>1</c>` for the common year (simplifies the <see cref="OrdFlags"/> validation)
	/// and `<c>bbb</c>` is a non-zero <see cref="IsoDayOfWeek"/> (mapping <see cref="IsoDayOfWeek.Monday"/> to 7,
	/// <see cref="IsoDayOfWeek.Tuesday"/> to 1 and so on) of the last day in the past year.
	/// </summary>
	public readonly struct YearFlags
	{
		// It would be a bit better if there were octal number literals with the prefix "0o" in C#,
		// just like how Rust, JavaScript, Go, Python 3, and Swift define the prefix for octal literals...
#pragma warning disable IDE1006 // Naming Styles
		static readonly YearFlags A = new(0b1000 + 5);
		static readonly YearFlags AG = new(5);
		static readonly YearFlags B = new(0b1000 + 4);
		static readonly YearFlags BA = new(4);
		static readonly YearFlags C = new(0b1000 + 3);
		static readonly YearFlags CB = new(3);
		static readonly YearFlags D = new(0b1000 + 2);
		static readonly YearFlags DC = new(2);
		static readonly YearFlags E = new(0b1000 + 1);
		static readonly YearFlags ED = new(1);
		static readonly YearFlags F = new(0b1000 + 7);
		static readonly YearFlags FE = new(7);
		static readonly YearFlags G = new(0b1000 + 6);
		static readonly YearFlags GF = new(6);
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// Contains dominical letters.
		/// </summary>
		static readonly YearFlags[] YearToFlags = new YearFlags[400]
		{
			BA, G, F, E, DC, B, A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA,
			G, F, E, DC, B, A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G,
			F, E, DC, B, A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F,
			E, DC, B, A, G, FE, D, C, B, AG, F, E, D, // 100
			C, B, A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC,
			B, A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B,
			A, G, FE, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A,
			G, FE, D, C, B, AG, F, E, D, CB, A, G, F, // 200
			E, D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE,
			D, C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE, D,
			C, B, AG, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE, D, C,
			B, AG, F, E, D, CB, A, G, F, ED, C, B, A, // 300
			G, F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE, D, C, B, AG,
			F, E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE, D, C, B, AG, F,
			E, D, CB, A, G, F, ED, C, B, A, GF, E, D, C, BA, G, F, E, DC, B, A, G, FE, D, C, B, AG, F, E,
			D, CB, A, G, F, ED, C, B, A, GF, E, D, C, // 400
		};

		internal YearFlags(byte flags)
		{
			Value = flags;
		}

		internal byte Value { get; }

		internal static YearFlags FromYear(int year) => FromYearMod400(year.RemEuclid(400));

		internal static YearFlags FromYearMod400(int year) => YearToFlags[year];

		internal int DayCount => 366 - (Value >> 3);

		internal int CalcIsoWeekDelta()
		{
			int delta = Value & 0xb0111;
			if (delta < 3)
			{
				delta += 7;
			}

			return delta;

			// G -> 6
			// F -> 7
			// E -> 8
			// D -> 9
			// C -> 3
			// B -> 4
			// A -> 5
		}

		internal bool IsLeapYear => (Value & 0b1000) == 0;

		internal uint IsoWeekCount => 52u + ((0b0000_0100_0000_0110u >> Value) & 1u);
	}
}
