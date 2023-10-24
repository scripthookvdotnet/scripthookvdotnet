//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

public interface Timelike<T>
{
	public int Hour { get; }

	public (bool isPM, int hour) Hour12 { get; }

	public int Minute { get; }

	public int Second { get; }

	public int SecondsFromMidnight { get; }

	public bool GetHour12(out int hour);

	public T WithHour(int hour);

	public T WithMinute(int minute);

	public T WithSecond(int second);
}
