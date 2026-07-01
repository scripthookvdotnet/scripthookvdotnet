using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class GameVersionNotSupportedException : Exception
    {
        [Obsolete("`GameVersionNotSupportedException.MinimumSupportedGameVersion` is deprecated " +
    "because Script Hook V is deprecating `getGameVersion`, which the property is based on. " +
    "Use `GameVersionNotSupportedException.MinimumSupportedGameFileVersion` instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GameVersion MinimumSupportedGameVersion { get; }
    }
}
