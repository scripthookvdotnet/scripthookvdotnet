//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GTA
{
	[Serializable]
	public class GameVersionNotSupportedException : Exception
	{
		public GameVersion MinimumSupportedGameVersion { get; }

		public GameVersionNotSupportedException()
		{
		}

		public GameVersionNotSupportedException(string message) : base(message)
		{
		}

		public GameVersionNotSupportedException(string message, Exception inner) : base(message, inner)
		{
		}

		public GameVersionNotSupportedException(string message, GameVersion minimumSupportedGameVersion) : base(message)
		{
			MinimumSupportedGameVersion = minimumSupportedGameVersion;
		}

		protected GameVersionNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			MinimumSupportedGameVersion = (GameVersion)info.GetValue("MinimumSupportedGameVersion", typeof(GameVersion));
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("MinimumSupportedVersion", MinimumSupportedGameVersion, typeof(GameVersion));
			base.GetObjectData(info, context);
		}
	}
}
