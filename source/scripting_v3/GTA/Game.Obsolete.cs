using System;
using System.ComponentModel;
using GTA.Native;

namespace GTA
{
    public static partial class Game
    {
        /// <summary>
        /// Gets the version of the game.
        /// </summary>
        [Obsolete("`Game.Version` is deprecated because Script Hook V is deprecating `getGameVersion`, which " +
            "the property is based on. Use `Game.FileVersion` instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static GameVersion Version => (GameVersion)SHVDN.NativeMemory.GetGameVersion();

        /// <summary>
        /// Gets a value indicating whether there is a loading screen being displayed if Script Hook V v1.0.3337.0 or
        /// older is installed to the game.
        /// </summary>
        /// <remarks>
        /// This property always return <see langword="false"/> since Script Hook V v1.0.3351.0+ This is because
        /// SHV changed the way SHV scripts start in v1.0.3351.0 (SHV version and not game version) and they will never be
        /// able to start before the game finished showing the loading screen since SHV v1.0.3351.0+. See
        /// <see href="https://github.com/scripthookvdotnet/scripthookvdotnet/issues/1549">#1549 on the main GitHub
        /// repository</see> for details.
        /// </remarks>
        [Obsolete("`Game.IsLoading` is obsolete because Script Hook V changed the way SHV scripts start in" +
            "v1.0.3351.0 (SHV version and not game version) and they never be able to start before the game " +
            "finished showing the loading screen since SHV v1.0.3351.0+. It is advised not to use `Game.IsLoading`" +
            "at all.")]
        public static bool IsLoading => Function.Call<bool>(Hash.GET_IS_LOADING_SCREEN_ACTIVE);

        /// <summary>
        /// Calculates a Jenkins One At A Time hash from the given <see cref="string"/> which can then be used by any native function that takes a hash.
        /// Can be called in any thread.
        /// </summary>
        /// <param name="input">The input <see cref="string"/> to hash.</param>
        /// <returns>The Jenkins hash of the input <see cref="string"/>.</returns>
        /// <remarks>
        /// Converts ASCII uppercase characters to lowercase ones and backslash characters to slash ones before
        /// converting into a hash. Computes the hash from the substring between two double quotes if the first
        /// character is a double quote character.
        /// </remarks>
        [Obsolete("Use StringHash.AtStringHash(string, uint), StringHash.AtStringHashUtf8(string, uint), " +
            "AtHashValue.FromString(string, uint), or StringHash.AtStringHashUtf8(string, uint) instead.")]
        // Use AtStringHashUtf8 for compatibility reasons
        public static int GenerateHash(string input) => (int)StringHash.AtStringHashUtf8(input);
    }
}
