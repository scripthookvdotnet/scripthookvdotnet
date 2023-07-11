//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum BlipCategoryType
	{
		NoDistanceShown = 1,
		DistanceShown,
		/// <summary>
		/// <para>Blips will show under the "Other Players" category listing in the map legend, regardless of name. Also shows distance in the map legend.</para>
		/// <para>the blip name will show with <see cref="UI.Font.ChaletComprimeCologneNotGamerName"/>.</para>
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
