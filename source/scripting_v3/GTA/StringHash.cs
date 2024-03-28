//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Text;

namespace GTA
{
    /// <summary>
    /// A static class for jenkins-one-at-a-time hash methods, which is very robust for as a full 32-bit hash function
    /// and is heavily used by the game.
    /// </summary>
    public static class StringHash
    {
        // Performs ASCII uppercase to ASCII lowercase and backslash to slash conversion, does not perform any conversions to non-ASCII characters.
        // Use this table because character conversion with this table performs faster than calculating converted characters using branch jump instructions.
        // The former method is used in GTA5.exe and the latter one is used in GTAIV.exe.
        private static readonly byte[] s_normalizeCaseAndSlashLookup =
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A,
            0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15,
            0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B,
            0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36,
            0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x61,
            0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C,
            0x6D, 0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77,
            0x78, 0x79, 0x7A, 0x5B, 0x2F, 0x5D, 0x5E, 0x5F, 0x60, 0x61, 0x62,
            0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D,
            0x6E, 0x6F, 0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78,
            0x79, 0x7A, 0x7B, 0x7C, 0x7D, 0x7E, 0x7F, 0x80, 0x81, 0x82, 0x83,
            0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E,
            0x8F, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99,
            0x9A, 0x9B, 0x9C, 0x9D, 0x9E, 0x9F, 0xA0, 0xA1, 0xA2, 0xA3, 0xA4,
            0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xAB, 0xAC, 0xAD, 0xAE, 0xAF,
            0xB0, 0xB1, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA,
            0xBB, 0xBC, 0xBD, 0xBE, 0xBF, 0xC0, 0xC1, 0xC2, 0xC3, 0xC4, 0xC5,
            0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xCB, 0xCC, 0xCD, 0xCE, 0xCF, 0xD0,
            0xD1, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xDB,
            0xDC, 0xDD, 0xDE, 0xDF, 0xE0, 0xE1, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6,
            0xE7, 0xE8, 0xE9, 0xEA, 0xEB, 0xEC, 0xED, 0xEE, 0xEF, 0xF0, 0xF1,
            0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC,
            0xFD, 0xFE, 0xFF
        };

        /// <summary>
        /// Partially computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from a
        /// <see cref="string"/> that contains only ASCII characters. Does not finalize the hash.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The partially calculated joaat hash.</returns>
        /// <remarks>
        /// <para>
        /// The following conversions will be performed before hashing the original string:
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
        /// </para>
        ///
        /// <para>
        /// This method only expects ASCII characters in <paramref name="input"/>.
        /// This is because the lookup table used by the joaat hash function used in <c>GET_HASH_KEY</c> expects
        /// only ASCII characters, and the table contains values for non-ASCII characters only to guarantee correctness
        /// even if "weird" european characters "sneak in" (are included in the string).
        /// If you need to pass a string that contains non-ASCII characters, use
        /// <see cref="AtPartialStringHashUtf8(string, uint)"/> instead.
        /// </para>
        /// </remarks>
        public static uint AtPartialStringHash(string input, uint initValue = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return initValue;
            }

            return AtPartialStringHash(Encoding.ASCII.GetBytes(input), initValue);
        }

        /// <summary>
        /// Partially computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from an array of
        /// <see cref="byte"/>. Does not finalize the hash.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">An array of <see cref="byte"/>.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The partially calculated joaat hash.</returns>
        /// <remarks>
        /// The following conversions will be performed before hashing the original string:
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
        public static uint AtPartialStringHash(byte[] input, uint initValue = 0)
        {
            if (input == null || input.Length <= 0)
            {
                return initValue;
            }

            if (input[0] == 0x22)
            {
                return AtPartialStringHashForDoubleQuotedString(input, 1, initValue);
            }

            uint hash = initValue;
            foreach (byte c in input)
            {
                hash += s_normalizeCaseAndSlashLookup[c];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            return hash;
        }

        /// <summary>
        /// Partially computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from
        /// <paramref name="input"/>. Does not finalize the hash.
        /// <paramref name="input"/> will be converted to a UTF-8 sequence before hashing.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> to hash. Will be converted to a UTF-8 sequence before hashing.
        /// </param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The partially calculated joaat hash.</returns>
        /// <remarks>
        /// The following conversions will be performed before hashing the original string:
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
        public static uint AtPartialStringHashUtf8(string input, uint initValue = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return initValue;
            }

            return AtPartialStringHash(Encoding.UTF8.GetBytes(input), initValue);
        }

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from a <see cref="string"/> that
        /// contains only ASCII characters.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        /// <remarks>
        /// <para>
        /// The following conversions will be performed before hashing the original string:
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
        /// </para>
        ///
        /// <para>
        /// This method only expects ASCII characters in <paramref name="input"/>.
        /// This is because the lookup table used by the joaat hash function used in <c>GET_HASH_KEY</c> expects
        /// only ASCII characters, and the table contains values for non-ASCII characters only to guarantee correctness
        /// even if "weird" european characters "sneak in" (are included in the string).
        /// If you need to pass a string that contains non-ASCII characters, use
        /// <see cref="AtStringHashUtf8(string, uint)"/> instead.
        /// </para>
        /// </remarks>
        public static uint AtStringHash(string input, uint initValue = 0)
            => AtFinalizeHash(AtPartialStringHash(input, initValue));

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from an array of
        /// <see cref="byte"/>.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">An array of <see cref="byte"/>.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        /// <remarks>
        /// The following conversions will be performed before hashing the original string:
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
        public static uint AtStringHash(byte[] input, uint initValue = 0)
            => AtFinalizeHash(AtPartialStringHash(input, initValue));

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from <paramref name="input"/>.
        /// <paramref name="input"/> will be converted to a UTF-8 sequence before hashing.
        /// Uppercase letters and backslashes will be converted to lowercase letters and slashes respectively.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> to hash. Will be converted to a UTF-8 sequence before hashing.
        /// </param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        /// <remarks>
        /// The following conversions will be performed before hashing the original string:
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
        public static uint AtStringHashUtf8(string input, uint initValue = 0)
            => AtFinalizeHash(AtPartialStringHashUtf8(input, initValue));

        // For literal string hash variants, you can find the equivalent function of the method in
        // the game with: "EB 15 0F BE C0 48 FF C1"

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from a <see cref="string"/> that
        /// contains only ASCII characters. Unlike <see cref="AtStringHash(string, uint)"/>, this method does not
        /// perform any character conversion before hashing, where the technique is heavily used when comparing member
        /// names in pso/meta config files.
        /// </summary>
        /// <param name="input">A <see cref="string"/> that contains only ASCII characters.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        public static uint AtLiteralStringHash(string input, uint initValue = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return initValue;
            }

            return AtPartialStringHash(Encoding.ASCII.GetBytes(input), initValue);
        }

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from an array of
        /// <see cref="byte"/>. Unlike <see cref="AtStringHash(string, uint)"/>, this method does not
        /// perform any character conversion before hashing, where the technique is heavily used when comparing member
        /// names in pso/meta config files.
        /// </summary>
        /// <param name="input">An array of <see cref="byte"/>.</param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        public static uint AtLiteralStringHash(byte[] input, uint initValue = 0)
        {
            if (input == null)
            {
                return initValue;
            }

            uint hash = initValue;
            foreach (byte c in input)
            {
                hash += c;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            return AtFinalizeHash(hash);
        }

        /// <summary>
        /// Computes a jenkins-one-at-a-time (joaat) hash as an <see langword="uint"/> from <paramref name="input"/>.
        /// <paramref name="input"/> will be converted to a UTF-8 sequence before hashing.
        /// Unlike<see cref="AtStringHash(string, uint)"/>, this method does not perform any character conversion
        /// before hashing, where the technique is heavily used when comparing member names in pso/meta config files.
        /// </summary>
        /// <param name="input">
        /// A <see cref="string"/> to hash. Will be converted to a UTF-8 sequence before hashing.
        /// </param>
        /// <param name="initValue">The initial value when hashing starts.</param>
        /// <returns>The calculated joaat hash.</returns>
        public static uint AtLiteralStringHashUtf8(string input, uint initValue = 0)
        {
            if (string.IsNullOrEmpty(input))
            {
                return initValue;
            }

            return AtPartialStringHash(Encoding.UTF8.GetBytes(input), initValue);
        }

        /// <summary>
        /// Finalizes a jenkins-one-at-a-time (joaat) hash.
        /// </summary>
        /// <param name="partialHashValue">A partial hash value to finalize.</param>
        /// <returns>The finalized hash value.</returns>
        public static uint AtFinalizeHash(uint partialHashValue)
        {
            uint hash = partialHashValue;
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            return hash;
        }

        private static uint AtPartialStringHashForDoubleQuotedString(byte[] input, int start, uint initValue)
        {
            uint hash = initValue;

            for (int i = start; i < input.Length; i++)
            {
                byte c = input[i];
                if (c == 0x22)
                {
                    break;
                }

                hash += s_normalizeCaseAndSlashLookup[c];
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            return hash;
        }
    }
}
