//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace GTA
{
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

			return $"{className}.{propertyOrMethodName} is not supported in between v1.0.335.2 to {maximumNotSupportedGameVersionWithoutPlatformName}.";
		}
	}
}
