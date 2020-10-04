//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

namespace GTA
{
	public enum BlipCategoryType
	{
		NoDistanceShown = 1,
		DistanceShown,
		/// <summary>
		/// <para>Blips will show under the "Other Players" category listing in the map legend, regardless of name. Also shows distance in the map legend.</para>
		/// <para>
		/// When the game language is set to a non-CJK language, the blip name will show with <see cref="UI.Font.ChaletComprimeCologne"/>.
		/// When the game language is set to a CJK language and the vanilla font files are used, the blip name will show with the default CJK font used for the current language.
		/// </para>
		/// </summary>
		OtherPlayers = 7,
		/// <summary>
		/// <para>Blips will show under the "Property" category listing in the map legend, regardless of name.</para>
		/// </summary>
		Property = 10,
		/// <summary>
		/// <para>Blips will show under the "Owned Property" category listing in the map legend, regardless of name.</para>
		/// </summary>
		OwnedProperty,
	}
}
