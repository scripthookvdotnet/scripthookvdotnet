//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;

namespace GTA
{
    /// <summary>
    /// Represents a jenkins-one-at-a-time (joaat) hash value for special cases, where no character conversions are
    /// performed before hashing.
    /// </summary>
    /// <remarks>
    /// Cannot be directly passed as an argument of native functions via <see cref="GTA.Native.Function"/>
    /// as none of them expect joaat hashes with no character conversions applied.
    /// </remarks>
    public readonly struct AtLiteralHashValue : IEquatable<AtLiteralHashValue>, IJoaatHashValue
    {
        public AtLiteralHashValue(uint hash) : this()
        {
            Hash = hash;
        }

        // Exposing the value as a field allows to prevent users from naively assigning a new value via the field.
        // while allowing to assign a new value via pointer dereference and mutate via pointer.
        public readonly uint Hash;

        public uint GetJoaatHash() => Hash;

        /// <summary>
        /// Gets the null value of <see cref="AtLiteralHashValue"/>, whose hash is zero.
        /// </summary>
        public static AtLiteralHashValue Null => new AtLiteralHashValue(0);

        /// <summary>
        /// Returns a value that indicates whether this instance is the null <see cref="AtLiteralHashValue"/>,
        /// whose hash is zero.
        /// </summary>
        public bool IsNull => Hash == 0;

        /// <summary>
        /// Computes an <see cref="AtLiteralHashValue"/> from a <see cref="string"/> that contains only ASCII
        /// characters.
        /// </summary>
        /// <param name="input">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <returns>An <see cref="AtLiteralHashValue"/> that contains the calculated hash.</returns>
        /// <remarks>
        /// This method only expects ASCII characters in <paramref name="input"/>.
        /// This is because the lookup table used by the joaat hash function used in <c>GET_HASH_KEY</c> expects
        /// only ASCII characters, and the table contains values for non-ASCII characters only to guarantee correctness
        /// even if "weird" european characters "sneak in" (are included in the string).
        /// If you need to pass a string that contains non-ASCII characters, use <see cref="FromStringUtf8(string)"/>
        /// instead.
        /// </remarks>
        public static AtLiteralHashValue FromString(string input)
            => new AtLiteralHashValue(StringHash.AtLiteralStringHash(input));
        /// <summary>
        /// Computes an <see cref="AtLiteralHashValue"/> from an array of <see cref="byte"/>.
        /// </summary>
        /// <param name="input">An array of <see cref="byte"/>.</param>
        /// <returns>An <see cref="AtLiteralHashValue"/> that contains the calculated hash.</returns>
        public static AtLiteralHashValue FromBytes(byte[] input)
            => new AtLiteralHashValue(StringHash.AtLiteralStringHash(input));
        /// <summary>
        /// Computes an <see cref="AtLiteralHashValue"/> from a <see cref="string"/>.
        /// <paramref name="input"/> will be converted to a UTF-8 sequence before hashing.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> to hash. Will be converted to a UTF-8 sequence before hashing.
        /// </param>
        /// <returns>An <see cref="AtLiteralHashValue"/> that contains the calculated hash.</returns>
        public static AtLiteralHashValue FromStringUtf8(string input)
            => new AtLiteralHashValue(StringHash.AtLiteralStringHashUtf8(input));

        /// <summary>
        /// Compares this instance for equality with <paramref name="other"/>.
        /// </summary>
        /// <param name="other">
        /// A struct that represents a jenkins-one-at-a-time hash with no character conversion applied to compare to
        /// this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(AtLiteralHashValue other)
        {
            return Hash == other.Hash;
        }
        public override bool Equals(object obj)
        {
            if (obj is AtLiteralHashValue model)
            {
                return Equals(model);
            }

            return false;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="AtLiteralHashValue"/> values are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <see langword="true"/> <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(AtLiteralHashValue left, AtLiteralHashValue right)
        {
            return left.Equals(right);
        }
        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="AtLiteralHashValue"/> values are not equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <see langword="true"/> <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(AtLiteralHashValue left, AtLiteralHashValue right)
        {
            return !left.Equals(right);
        }

        public static explicit operator AtLiteralHashValue(uint value)
        {
            return new AtLiteralHashValue(value);
        }
        public static explicit operator uint(AtLiteralHashValue value)
        {
            return value.Hash;
        }

        public override int GetHashCode()
        {
            return (int)Hash;
        }
    }
}
