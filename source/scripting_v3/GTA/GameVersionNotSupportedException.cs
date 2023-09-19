//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

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
	/// implemented for the game versions earlier than v1.0.877.1 due to the absent of the member of restricted ammo
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
		public GameVersion MinimumSupportedGameVersion { get; }

		internal GameVersionNotSupportedException(GameVersion minimumSupportedGameVersion, string className, string propertyOrMethodName) : base(BuildErrorMessage(minimumSupportedGameVersion, className, propertyOrMethodName))
		{
			MinimumSupportedGameVersion = minimumSupportedGameVersion;
		}

		private GameVersionNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			MinimumSupportedGameVersion = (GameVersion)info.GetValue("MinimumSupportedGameVersion", typeof(GameVersion));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("MinimumSupportedVersion", MinimumSupportedGameVersion, typeof(GameVersion));
			base.GetObjectData(info, context);
		}

		internal static string BuildErrorMessage(GameVersion minimumSupportedGameVersion, string className, string propertyOrMethodName)
		{
			string maximumNotSupportedGameVersionEnumStr = Enum.GetName(typeof(GameVersion), (GameVersion)((int)minimumSupportedGameVersion - 1)).Replace('_', '.');

			string versionRegexPattern = @"v1\.0\.\d{3,5}\.\d";
			string maximumNotSupportedGameVersionWithoutPlatformName = Regex.Match(maximumNotSupportedGameVersionEnumStr, versionRegexPattern).Value;

			return $"{className}.{propertyOrMethodName} is not supported in from v1.0.335.2 to {maximumNotSupportedGameVersionWithoutPlatformName}, inclusive.";
		}

		internal static void ThrowIfNotSupported(GameVersion minSupportedVersion, string className, string propertyOrMethodName)
		{
			if (Game.Version < minSupportedVersion)
			{
				Throw(minSupportedVersion, className, propertyOrMethodName);
			}
		}

		internal static void Throw(GameVersion minSupportedVersion, string className, string propertyOrMethodName)
			=> throw new GameVersionNotSupportedException(minSupportedVersion, className, propertyOrMethodName);
	}
}
