//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public enum Relationship
	{
		Hate = 5,
		/// <summary>
		/// The correct relationship name for this enum would be <c>Wanted</c>.
		/// Will be used for cops towards the player relationship group when the player is wanted.
		/// </summary>
		Dislike = 4,
		/// <summary>
		/// The correct relationship name for this enum would be <c>Dislike</c>.
		/// </summary>
		Neutral = 3,
		/// <summary>
		/// The correct relationship name for this enum would be <c>Ignore</c>.
		/// </summary>
		Like = 2,
		/// <summary>
		/// The correct relationship name for this enum would be <c>Like</c>.
		/// </summary>
		Respect = 1,
		/// <summary>
		/// The correct relationship name for this enum would be <c>Respect</c>.
		/// </summary>
		Companion = 0,
		/// <summary>
		/// The correct relationship name for this enum would be <c>None</c>.
		/// </summary>
		Pedestrians = 255,
	}
}
