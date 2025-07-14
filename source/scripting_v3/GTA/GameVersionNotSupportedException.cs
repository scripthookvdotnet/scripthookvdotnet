//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace GTA
{
    /// <summary>
    /// The exception that is thrown when an invoked method is not supported in the running game version.
    /// </summary>
    /// <remarks>
    /// <see cref="GameVersionNotSupportedException"/> indicates that no implementation exists for the running game
    /// version for an invoked method or property. There are two typical cases where a
    /// <see cref="GameVersionNotSupportedException"/> is thrown:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// The relevant implementation is completely absent and operation cannot be performed in a meaningful way in
    /// the running game version. For example, <see cref="Vehicle.SetRestrictedAmmoCount(int, int)"/> cannot be
    /// implemented for the game versions earlier than v1.0.877.1 due to the absence of the member of restricted ammo
    /// count in <c>CVehicle</c>.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// An invoked method or property calls a native function that does not exist in the running game version.
    /// In this case, some issues can be resolved with a custom wrapper implementation for earlier game version
    /// if relevant implementation is present.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Serializable]
    public sealed class GameVersionNotSupportedException : Exception
    {
        [Obsolete("`GameVersionNotSupportedException.MinimumSupportedGameVersion` is deprecated " +
            "because Script Hook V is deprecating `getGameVersion`, which the property is based on. " +
            "Use `GameVersionNotSupportedException.MinimumSupportedGameFileVersion` instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GameVersion MinimumSupportedGameVersion { get; }
        public Version MinimumSupportedGameFileVersion { get; }

        private Dictionary<Version, GameVersion> _supportedGameVersionEnumMaps
            = new Dictionary<Version, GameVersion>
        {
            { VersionConstsForGameVersion.v1_0_335_2, GameVersion.v1_0_335_2_Steam },
            { VersionConstsForGameVersion.v1_0_350_1, GameVersion.v1_0_350_1_Steam },
            { VersionConstsForGameVersion.v1_0_350_2, GameVersion.v1_0_350_2_NoSteam },
            { VersionConstsForGameVersion.v1_0_372_2, GameVersion.v1_0_372_2_Steam },
            { VersionConstsForGameVersion.v1_0_393_2, GameVersion.v1_0_393_2_Steam },
            { VersionConstsForGameVersion.v1_0_393_4, GameVersion.v1_0_393_4_Steam },
            { VersionConstsForGameVersion.v1_0_463_1, GameVersion.v1_0_463_1_Steam },
            { VersionConstsForGameVersion.v1_0_505_2, GameVersion.v1_0_505_2_Steam },
            { VersionConstsForGameVersion.v1_0_573_1, GameVersion.v1_0_573_1_Steam },
            { VersionConstsForGameVersion.v1_0_617_1, GameVersion.v1_0_617_1_Steam },
            { VersionConstsForGameVersion.v1_0_678_1, GameVersion.v1_0_678_1_Steam },
            { VersionConstsForGameVersion.v1_0_757_2, GameVersion.v1_0_757_2_Steam },
            { VersionConstsForGameVersion.v1_0_757_4, GameVersion.v1_0_757_3_Steam },
            { VersionConstsForGameVersion.v1_0_791_2, GameVersion.v1_0_791_2_Steam },
            { VersionConstsForGameVersion.v1_0_877_1, GameVersion.v1_0_877_1_Steam },
            { VersionConstsForGameVersion.v1_0_944_2, GameVersion.v1_0_944_2_Steam },
            { VersionConstsForGameVersion.v1_0_1011_1, GameVersion.v1_0_1011_1_Steam },
            { VersionConstsForGameVersion.v1_0_1032_1, GameVersion.v1_0_1032_1_Steam },
            { VersionConstsForGameVersion.v1_0_1103_2, GameVersion.v1_0_1103_2_Steam },
            { VersionConstsForGameVersion.v1_0_1180_2, GameVersion.v1_0_1180_2_Steam },
            { VersionConstsForGameVersion.v1_0_1290_1, GameVersion.v1_0_1290_1_Steam },
            { VersionConstsForGameVersion.v1_0_1365_1, GameVersion.v1_0_1365_1_Steam },
            { VersionConstsForGameVersion.v1_0_1493_0, GameVersion.v1_0_1493_0_Steam },
            { VersionConstsForGameVersion.v1_0_1493_1, GameVersion.v1_0_1493_1_Steam },
            { VersionConstsForGameVersion.v1_0_1604_0, GameVersion.v1_0_1604_0_Steam },
            { VersionConstsForGameVersion.v1_0_1604_1, GameVersion.v1_0_1604_1_Steam },
            { VersionConstsForGameVersion.v1_0_1734_0, GameVersion.v1_0_1737_0_Steam },
            { VersionConstsForGameVersion.v1_0_1737_0, GameVersion.v1_0_1737_0_Steam },
            { VersionConstsForGameVersion.v1_0_1737_6, GameVersion.v1_0_1737_6_Steam },
            { VersionConstsForGameVersion.v1_0_1868_0, GameVersion.v1_0_1868_0_Steam },
            { VersionConstsForGameVersion.v1_0_1868_1, GameVersion.v1_0_1868_1_Steam },
            { VersionConstsForGameVersion.v1_0_1868_4, GameVersion.v1_0_1868_4_EGS },
            { VersionConstsForGameVersion.v1_0_2060_0, GameVersion.v1_0_2060_0_Steam },
            { VersionConstsForGameVersion.v1_0_2060_1, GameVersion.v1_0_2060_1_Steam },
            { VersionConstsForGameVersion.v1_0_2189_0, GameVersion.v1_0_2189_0_Steam },
            { VersionConstsForGameVersion.v1_0_2215_0, GameVersion.v1_0_2215_0_Steam },
            { VersionConstsForGameVersion.v1_0_2245_0, GameVersion.v1_0_2245_0_Steam },
            { VersionConstsForGameVersion.v1_0_2372_0, GameVersion.v1_0_2372_0_Steam },
            { VersionConstsForGameVersion.v1_0_2372_2, GameVersion.v1_0_2372_0_Steam },
            { VersionConstsForGameVersion.v1_0_2545_0, GameVersion.v1_0_2545_0_Steam },
            { VersionConstsForGameVersion.v1_0_2612_1, GameVersion.v1_0_2612_1_Steam },
            { VersionConstsForGameVersion.v1_0_2628_2, GameVersion.v1_0_2628_2_Steam },
            { VersionConstsForGameVersion.v1_0_2699_0, GameVersion.v1_0_2699_0_Steam },
            { VersionConstsForGameVersion.v1_0_2699_16, GameVersion.v1_0_2699_16 },
            { VersionConstsForGameVersion.v1_0_2802_0, GameVersion.v1_0_2802_0 },
            { VersionConstsForGameVersion.v1_0_2824_0, GameVersion.v1_0_2824_0 },
            { VersionConstsForGameVersion.v1_0_2845_0, GameVersion.v1_0_2845_0 },
            { VersionConstsForGameVersion.v1_0_2944_0, GameVersion.v1_0_2944_0 },
            { VersionConstsForGameVersion.v1_0_3028_0, GameVersion.v1_0_3028_0 },
            { VersionConstsForGameVersion.v1_0_3095_0, GameVersion.v1_0_3095_0 },
            { VersionConstsForGameVersion.v1_0_3179_0, GameVersion.v1_0_3179_0 },
            { VersionConstsForGameVersion.v1_0_3258_0, GameVersion.v1_0_3258_0 },
            { VersionConstsForGameVersion.v1_0_3274_0, GameVersion.v1_0_3274_0 },
            { VersionConstsForGameVersion.v1_0_3323_0, GameVersion.v1_0_3323_0 },
            { VersionConstsForGameVersion.v1_0_3337_0, GameVersion.v1_0_3337_0 },
        };

        internal GameVersionNotSupportedException(Version minSupportedGameVersion, string className, string propertyOrMethodName) : base(BuildErrorMessage(minSupportedGameVersion, className, propertyOrMethodName))
        {
            MinimumSupportedGameFileVersion = minSupportedGameVersion;

            if (_supportedGameVersionEnumMaps.TryGetValue(minSupportedGameVersion, out GameVersion gameVersion))
            {
#pragma warning disable CS0618 // Type or member is obsolete
                MinimumSupportedGameVersion = gameVersion;
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                // shouldn't come here unless we mess up in our codebase

#pragma warning disable CS0618 // Type or member is obsolete
                MinimumSupportedGameVersion = GameVersion.Unknown;
#pragma warning restore CS0618 // Type or member is obsolete

            }
        }

        private GameVersionNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            MinimumSupportedGameFileVersion
                = (Version)info.GetValue("MinimumSupportedGameFileVersion", typeof(GameVersion));

#pragma warning disable CS0618 // Type or member is obsolete
            MinimumSupportedGameVersion
                = (GameVersion)info.GetValue("MinimumSupportedGameVersion", typeof(GameVersion));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("MinimumSupportedGameFileVersion", MinimumSupportedGameFileVersion, typeof(Version));

#pragma warning disable CS0618 // Type or member is obsolete
            info.AddValue("MinimumSupportedVersion", MinimumSupportedGameVersion, typeof(GameVersion));
#pragma warning restore CS0618 // Type or member is obsolete
        }

        internal static string BuildErrorMessage(Version minSupportedGameVersion, string className, string propertyOrMethodName)
        {
            // The formatted game version (such as "v1.0.335.2") string won't take more than 16 characters
            // on condition that the build number (the third number) is up to 65535 and the other 3 numbers
            // is between 0 and 9.
            const int SbCapacity = 16;
            StringBuilder sb = new StringBuilder(SbCapacity);
            sb.Append("v");
            sb.Append(minSupportedGameVersion.Major);
            sb.Append(".");
            sb.Append(minSupportedGameVersion.Minor);
            sb.Append(".");
            sb.Append(minSupportedGameVersion.Build);
            sb.Append(".");
            sb.Append(minSupportedGameVersion.Revision);
            string formattedSupportedGameVersionStr = sb.ToString();

            return $"{className}.{propertyOrMethodName} is supported only in the game version " +
                $"{formattedSupportedGameVersionStr} or later.";
        }

        internal static void ThrowIfNotSupported(Version minSupportedVersion, string className, string propertyOrMethodName)
        {
            if (Game.FileVersion < minSupportedVersion)
            {
                Throw(minSupportedVersion, className, propertyOrMethodName);
            }
        }

        internal static void Throw(Version minSupportedVersion, string className, string propertyOrMethodName)
            => throw new GameVersionNotSupportedException(minSupportedVersion, className, propertyOrMethodName);
    }
}
