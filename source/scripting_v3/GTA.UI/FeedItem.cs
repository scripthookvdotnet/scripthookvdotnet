//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
	/// <summary>
	/// Represents a feed item.
	/// </summary>
	public class FeedItem
	{
		internal FeedItem(int handle)
		{
			Handle = handle;
		}

		/// <summary>
		/// Gets the feed item handle.
		/// </summary>
		public int Handle
		{
			get;
		}

		/// <summary>
		/// Deletes this <see cref="FeedItem"/> if it still exists.
		/// </summary>
		public void Delete() => Function.Call(Hash.THEFEED_REMOVE_ITEM, Handle);

		public override bool Equals(object obj)
		{
			if (obj is FeedItem feedItem)
			{
				return Equals(feedItem);
			}

			return false;
		}

		public static bool operator ==(FeedItem left, FeedItem right)
		{
			return left?.Equals(right) ?? right is null;
		}
		public static bool operator !=(FeedItem left, FeedItem right)
		{
			return !(left == right);
		}

		/// <summary>
		/// Converts an <see cref="FeedItem"/> to a native input argument.
		/// </summary>
		public static implicit operator InputArgument(FeedItem value)
		{
			// -1 is the value when feed natives fail to create items
			return new InputArgument((ulong)(value?.Handle ?? -1));
		}

		public override int GetHashCode()
		{
			return Handle.GetHashCode();
		}
	}
}
