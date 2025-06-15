//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.ComponentModel;

namespace GTA.UI
{
    /// <summary>
    /// An enumeration of all the font styles that script text draw commands support.
    /// </summary>
    public enum Font
    {
        /// <summary>
        /// <para>This font is for the standard font.</para>
        /// <para>
        /// Chalet London 1960 by House Industries will be used when the game language is set to a non-CJK language.
        /// The game will use an alternative standard font that contains appropriate CJK characters when the game
        /// language is set to a CJK language.
        /// </para>
        /// </summary>
        ChaletLondon,
        /// <summary>
        /// <para>This font is for the cursive font.</para>
        /// <para>
        /// Sign Painter House Brush by House Industries will be used when the game language is set to a non-CJK
        /// language.
        /// The standard font, which is the same as <see cref="ChaletLondon"/>, will be used when the game language is
        /// set to a CJK language unless the player have custom font files installed.
        /// </para>
        /// </summary>
        HouseScript,
        /// <summary>
        /// <para>This font contains upper ASCII characters and some other shapes for crew tags in GTA Online.</para>
        /// <para>
        /// This font style has a subset of the glyphs of <see cref="ChaletComprimeCologne"/> unless the player have
        /// custom font files installed.
        /// </para>
        /// </summary>
        RockstarTag,
        /// <summary>
        /// <para>This font contains only Chevron arrows, some shield symbols, and hexagons.</para>
        /// </summary>
        Leaderboard,
        /// <summary>
        /// <para>
        /// This font is for the condensed font for gamer tags or the distance info on the radar.
        /// You should use <see cref="ChaletComprimeCologneNotGamerName"/> if you want to draw strings with condensed
        /// font if possible, but they can contain some CJK characters (e.g. localized strings from gxt files).
        /// </para>
        /// <para>
        /// The native functions for text drawing will use the condensed font Chalet Comprimé Cologne 1960 regardless
        /// of the game language setting.
        /// The natives will draw strings that contain only non-CJK characters without any trouble, but without having
        /// custom font files installed, the natives will draw rectangles (a.k.a. tofus) instead of CJK characters
        /// because Chalet Comprimé Cologne 1960 doesn't contain any CJK characters.
        /// </para>
        /// <para>This font style specifies the same font unless the player have custom font files installed.</para>
        /// </summary>
        ChaletComprimeCologne,
        /// <summary>
        /// <para>
        /// This font contains only glyphs of numbers (0 to 9), dollar sign, asterisk, plus sign, colon, semicolon,
        /// equals sign, slash, and backslash in ASCII.
        /// </para>
        /// <para>
        /// This font style has a subset of the glyphs of <see cref="ChaletLondon"/> unless the player have custom font
        /// files installed.
        /// </para>
        /// </summary>
        FixedWidthNumbersStyle,
        /// <summary>
        /// <para>
        /// This font is for the condensed font for generic uses.
        /// Consider using <see cref="ChaletComprimeCologne"/> instead when the texts you want to draw does not contain
        /// any CJK characters.
        /// </para>
        /// <para>
        /// The native functions for text drawing will use the condensed font Chalet Comprimé Cologne 1960 when
        /// the game language is set to a non-CJK language. When the game language is set to a CJK language, they will
        /// use the standard font, which is the same as <see cref="ChaletLondon"/> (unless the player have custom font
        /// files installed).
        /// </para>
        /// </summary>
        ChaletComprimeCologneNotGamerName,
        /// <summary>
        /// Pricedown will be used when the game language is set to a non-CJK language.
        /// The standard font, which is the same as <see cref="ChaletLondon"/>, will be used when the game language is
        /// set to a CJK language unless the player have custom font files installed.
        /// </summary>
        Pricedown,
        /// <summary>
        /// The font for taxi will be used when the game language is set to a non-CJK language.
        /// The font does contain invisible glyphs for lower characters.
        /// The standard font, which is the same as <see cref="ChaletLondon"/>, will be used when the game language is
        /// set to a CJK language unless the player have custom font files installed.
        /// </summary>
        Taxi,

        [Obsolete("Font.Monospace is obsolete because it is incorrect, use Font.RockstarTag instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        Monospace = RockstarTag,
    }
}
