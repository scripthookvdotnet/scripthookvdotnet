//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
    /// <summary>
    /// Represents a feed post (for the gameStream).
    /// </summary>
    /// <remarks>
    /// Feed posts internally interact to <c>CGameStream::gstPost</c>, and the global <c>CGameStream</c> instance
    /// manages up to 32 <c>CGameStream::gstPost</c>s (the name of <c>CGameStream</c> and <c>CGameStream::gstPost</c>
    /// cannot be seen in production game builds).
    /// </remarks>
    public sealed class FeedPost
    {
        internal FeedPost(int handle)
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
        /// Deletes this <see cref="FeedPost"/> if it still exists.
        /// </summary>
        public void Delete() => Function.Call(Hash.THEFEED_REMOVE_ITEM, Handle);

        public override bool Equals(object obj)
        {
            if (obj is FeedPost feedItem)
            {
                return Equals(feedItem);
            }

            return false;
        }

        public static bool operator ==(FeedPost left, FeedPost right)
        {
            return left?.Equals(right) ?? right is null;
        }
        public static bool operator !=(FeedPost left, FeedPost right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Converts an <see cref="FeedPost"/> to a native input argument.
        /// </summary>
        public static implicit operator InputArgument(FeedPost value)
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
