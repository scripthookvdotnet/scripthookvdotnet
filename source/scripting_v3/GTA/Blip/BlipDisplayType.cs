//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum BlipDisplayType
	{
		NoDisplay,
		BothMapSelectable = 2,
		MainMapSelectable,
		/// <summary>
		/// The default value on blip creation. Works in the same way as <see cref="BothMapSelectable"/>.
		/// </summary>
		Default,
		MiniMapOnly,
		BothMapNoSelectable = 8,
	}
}
