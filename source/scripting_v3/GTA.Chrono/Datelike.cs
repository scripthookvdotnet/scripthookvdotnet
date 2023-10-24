//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Chrono;

public interface Datelike<T>
{
	public int Year { get; }

	public int Month { get; }

	public int ZeroBasedMonth { get; }

	public int Day { get; }

	public int ZeroBasedDay { get; }

	public int DayOfYear { get; }

	public int ZeroBasedDayOfYear { get; }

	public DayOfWeek DayOfWeek { get; }

	public IsoDayOfWeek IsoDayOfWeek { get; }

	public T WithYear(int year);

	public T WithMonth(int month);

	public T WithZeroBasedMonth(int month);

	public T WithDay(int day);

	public T WithZeroBasedDay(int day);

	public T WithDayOfYear(int dayOfYear);

	public T WithZeroBasedDayOfYear(int dayOfYear);
}
