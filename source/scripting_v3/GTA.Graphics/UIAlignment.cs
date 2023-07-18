//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA.Graphics
{
	/// <summary>
	/// An enumeration of all possible values where scripted draw commands align 2d graphic elements in different ways
	/// and the default value for no special alignment.
	/// </summary>
	/// <remarks>
	/// This enumeration specifies <see cref="byte"/> as the base type as the game internally uses 1-byte values
	/// for UI alignment types.
	/// </remarks>
	public enum UIAlignment : byte
	{
		/// <summary>
		/// The left alignment value, which can be used for the horizontal alignment.
		/// </summary>
		/// <remarks>
		/// Represents 'L' out of the  ASCII characters.
		/// </remarks>
		Left = 76,
		/// <summary>
		/// The right alignment value, which can be used for the horizontal alignment.
		/// </summary>
		/// <remarks>
		/// Represents 'R' out of the ASCII characters.
		/// </remarks>
		Right = 82,
		/// <summary>
		/// The top alignment value, which can be used for the vertical alignment.
		/// </summary>
		/// <remarks>
		/// Represents 'T' out of the ASCII characters.
		/// </remarks>
		Top = 84,
		/// <summary>
		/// The bottom alignment value, which can be used for the vertical alignment.
		/// </summary>
		/// <remarks>
		/// Represents 'B' out of the ASCII characters.
		/// </remarks>
		Bottom = 66,
		/// <summary>
		/// The canonical value of the ignore value (represents 'I' in ASCII characters),
		/// where script 2d draw commands skips the UI alignment.
		/// </summary>
		/// <remarks>
		/// Although this value is the initial value for the UI alignment when the game launches,
		/// script 2d draw commands do some alignment operation only if one of the acceptable values is set
		/// (you can confirm this by searching the function that can be found with
		/// <c>"40 80 FE 4C 75 08 F3 0F 10 7C 24 78 EB 2A 40 80 FE 52 75 0C"</c>).
		/// </remarks>
		Ignore = 73,
	}
}
