//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
    /// <summary>
    /// Represents a jenkins-one-at-a-time hash value for common cases.
    /// Can be directly passed as an argument of native functions via <see cref="Function"/>.
    /// </summary>
    /// <remarks>
    /// The following conversions are supposed to be performed before hashing the original string:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Map ASCII uppercase letters to lowercase.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Extract the substring within the first two double quotes if the first character/byte is a double quote
    /// (hashes to the end of sequence if there is only one double quote).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Convert backslashes to forward slashes so that the has is more useful for filenames.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    public readonly struct AtHashValue : IEquatable<AtHashValue>, IJoaatHashValue
    {
        public AtHashValue(uint hash) : this()
        {
            Hash = hash;
        }

        // Exposing the value as a field allows to prevent users from naively assigning a new value via the field.
        // while allowing to assign a new value via pointer dereference and mutate via pointer.
        public readonly uint Hash;

        public uint GetJoaatHash() => Hash;

        /// <summary>
        /// Gets the null value of <see cref="AtHashValue"/>, whose hash is zero.
        /// </summary>
        public static AtHashValue Null => new AtHashValue(0);

        /// <summary>
        /// Returns a value that indicates whether this instance is the null <see cref="AtHashValue"/>, whose hash is
        /// zero.
        /// </summary>
        public bool IsNull => Hash == 0;

        /// <summary>
        /// Computes an <see cref="AtHashValue"/> from a <see cref="string"/> that contains only ASCII characters.
        /// </summary>
        /// <param name="input">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <returns>An <see cref="AtHashValue"/> that contains the calculated hash.</returns>
        /// <remarks>
        /// This method only expects ASCII characters in <paramref name="input"/>.
        /// This is because the lookup table used by the joaat hash function used in <c>GET_HASH_KEY</c> expects
        /// only ASCII characters, and the table contains values for non-ASCII characters only to guarantee correctness
        /// even if "weird" european characters "sneak in" (are included in the string).
        /// If you need to pass a string that contains non-ASCII characters, use <see cref="FromStringUtf8(string)"/>
        /// instead.
        /// </remarks>
        public static AtHashValue FromString(string input)
            => new AtHashValue(StringHash.AtStringHash(input));
        /// <summary>
        /// Computes an <see cref="AtHashValue"/> from an array of <see cref="byte"/>.
        /// </summary>
        /// <param name="input">An array of <see cref="byte"/>.</param>
        /// <returns>An <see cref="AtHashValue"/> that contains the calculated hash.</returns>
        public static AtHashValue FromBytes(byte[] input)
            => new AtHashValue(StringHash.AtStringHash(input));
        /// <summary>
        /// Computes an <see cref="AtHashValue"/> from a <see cref="string"/>.
        /// <paramref name="input"/> will be converted to a UTF-8 sequence before hashing.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> to hash. Will be converted to a UTF-8 sequence before hashing.
        /// </param>
        /// <returns>An <see cref="AtHashValue"/> that contains the calculated hash.</returns>
        public static AtHashValue FromStringUtf8(string input)
            => new AtHashValue(StringHash.AtStringHashUtf8(input));

        /// <summary>
        /// Computes a joaat hash as an <see langword="uint"/> from a <see cref="string"/> that contains only ASCII
        /// characters.
        /// </summary>
        /// <param name="value">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <returns>The calculated joaat hash.</returns>
        /// <remarks>
        /// This method only expects ASCII characters in <paramref name="value"/>.
        /// This is because the lookup table used by the joaat hash function used in <c>GET_HASH_KEY</c> expects
        /// only ASCII characters, and the table contains values for non-ASCII characters only to guarantee correctness
        /// even if "weird" european characters "sneak in" (are included in the string).
        /// If you need to pass a string that contains non-ASCII characters, use <see cref="ComputeHashUtf8(string)"/>
        /// instead.
        /// </remarks>
        public static uint ComputeHash(string value) => StringHash.AtStringHash(value);
        /// <summary>
        /// Computes a joaat hash <see cref="AtLiteralHashValue"/> from an array of <see cref="byte"/>.
        /// </summary>
        /// <param name="value">An array of <see cref="byte"/>.</param>
        /// <returns>The calculated joaat hash.</returns>
        public static uint ComputeHash(byte[] value) => StringHash.AtStringHash(value);
        /// <summary>
        /// Computes a joaat hash as an <see langword="uint"/> from a <see cref="string"/>.
        /// Converts the string to UTF-8 (which the game uses for localized strings) before hashing.
        /// </summary>
        /// <param name="value">A <see cref="string"/>.</param>
        /// <returns>The calculated joaat hash.</returns>
        public static uint ComputeHashUtf8(string value) => StringHash.AtStringHashUtf8(value);

        /// <summary>
        /// Compares this instance for equality with <paramref name="other"/>.
        /// </summary>
        /// <param name="other">
        /// A struct that represents a jenkins-one-at-a-time hash to compare to this instance, where certain ASCII
        /// characters of the original string are converted to other characters in a certain manner.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to <paramref name="other"/>; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(AtHashValue other)
        {
            return Hash == other.Hash;
        }
        public override bool Equals(object obj)
        {
            if (obj is AtHashValue model)
            {
                return Equals(model);
            }

            return false;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="AtHashValue"/> values are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// <see langword="true"/> <paramref name="left"/> and <paramref name="right"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(AtHashValue left, AtHashValue right)
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
        public static bool operator !=(AtHashValue left, AtHashValue right)
        {
            return !left.Equals(right);
        }

        public static implicit operator InputArgument(AtHashValue value)
        {
            return new InputArgument(value.Hash);
        }
        public static explicit operator AtHashValue(uint value)
        {
            return new AtHashValue(value);
        }
        public static explicit operator uint(AtHashValue value)
        {
            return value.Hash;
        }
        public static explicit operator AtHashValue(int value)
        {
            return new AtHashValue((uint)value);
        }
        public static explicit operator int(AtHashValue value)
        {
            return (int)value.Hash;
        }

        public override int GetHashCode()
        {
            return (int)Hash;
        }
    }
}
