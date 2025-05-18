//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using GTA.Native;

namespace GTA.Graphics
{
    /// <summary>
    /// Represents a struct that contains a texture dictionary <see cref="string"/>.
    /// </summary>
    public readonly struct Txd : IEquatable<Txd>, IScriptStreamingResource
    {
        public Txd(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of this texture dictionary.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Computes the hash of <see cref="Name"/> in the same way as how the game calculates hashes for texture
        /// dictionaries to store in the global <c>rage::fwTxdStore</c> and as how
        /// <see cref="StringHash.AtStringHashUtf8(string, uint)"/> calculates.
        /// May be useful when you want to get the identifier in the same way as how the game handles texture
        /// dictionaries or when you investigate game memory to see how textures are stored in texture dictionaries
        /// (should be in a <c>rage::pgDictionary&lt;rage::grcTexturePC&gt;</c> instance).
        /// </summary>
        /// <returns>The hash value calculated from <see cref="Name"/>.</returns>
        public AtHashValue HashName() => AtHashValue.FromString(Name);

        /// <summary>
        /// Gets a value indicating whether the textures of this <see cref="Txd"/> are loaded
        /// the global <c>rage::fwTxdStore</c> so they are ready to use.
        /// </summary>
        /// <remarks>
        /// You might want to check if this property returns <see langword="true"/> before calling
        /// <see cref="Request()"/>. If already, you should not want to call <see cref="MarkAsNoLongerNeeded()"/>
        /// after your script finished using the texture dictionary.
        /// Another SHVDN script will crash the game for access violation during some texture
        /// operation if you call <see cref="MarkAsNoLongerNeeded()"/> on a <see cref="Txd"/>
        /// that is loaded before your script tried to use and if the game tries to unload the textures of the texture
        /// dictionary equivalent to said <see cref="Txd"/>.
        /// </remarks>
        public bool IsLoaded => Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, Name);

        /// <summary>
        /// <para>
        /// Requests the global streaming loader to load this <see cref="Txd"/> so it will be eventually loaded
        /// (unless getting interrupted by a <see cref="MarkAsNoLongerNeeded()"/> call of another SHVDN script).
        /// </para>
        /// <para>
        /// You will need to test if the resource is loaded with <see cref="IsLoaded"/> every frame until
        /// the <see cref="Txd"/> is loaded before you can use it. The game starts loading pending streaming objects
        /// every frame (with `<c>CStreaming::Update()</c>`) before the script update call.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Calling this method every frame could practically avoid the game crashing due to the textures of this
        /// <see cref="Txd"/> getting unloaded at the time your script tries to use some of them, which may be useful
        /// when the <see cref="Txd"/> is going to be used for a long time like 5 seconds. That kind of game crash can
        /// happen when another <em>SHVDN</em> script calls <see cref="MarkAsNoLongerNeeded()"/> on the same
        /// <see cref="Txd"/>. Do note that the workaround is needed only because of how SHVDN runtime cannot isolate
        /// script resources from other scripts.
        /// </para>
        /// <para>
        /// Allocates a <c>CScriptResource_TextureDictionary</c> instance for the SHVDN runtime.
        /// </para>
        /// </remarks>
        public void Request()
        {
            Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, Name);
        }

        /// <summary>
        /// Tells the game we have finished using this <see cref="Txd"/> and it can be freed from memory.
        /// You do not need to call this method if this <see cref="Txd"/> was loaded by another way before your script
        /// used this <see cref="Txd"/>, such as <see cref="PedHeadshot"/>.
        /// </summary>
        /// <para>
        /// You should not call this method if the <see cref="Txd"/> is already loaded before your script tried to use
        /// it (though SHVDN runtime cannot prevent other scripts from doing). You can test if the <see cref="Txd"/> is
        /// loaded with <see cref="IsLoaded"/>.
        /// </para>
        /// <para>
        /// Releases a <c>CScriptResource_TextureDictionary</c> instance from the <c>CGameScriptHandler</c> for
        /// the SHVDN runtime.
        /// </para>
        /// <remarks>
        /// </remarks>
        public void MarkAsNoLongerNeeded()
        {
            Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, Name);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the Jenkins-one-at-a-time (joaat) hash value of <see cref="Name"/>
        /// matches that of <paramref name="other"/>, as the game uses joaat hashes as identifiers of texture dictionaries.
        /// </summary>
        public bool Equals(Txd other)
        {
            return HashName() == other.HashName();
        }
        public override bool Equals(object obj)
        {
            if (obj is Txd txd)
            {
                return Equals(txd);
            }

            return false;
        }

        public static bool operator ==(Txd left, Txd right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Txd left, Txd right)
        {
            return !left.Equals(right);
        }

        public static implicit operator InputArgument(Txd value)
        {
            return new InputArgument(value.Name);
        }
        public static explicit operator Txd(string value)
        {
            return new Txd(value);
        }
        public static explicit operator string(Txd value)
        {
            return value.Name;
        }

        public override int GetHashCode()
        {
            return (int)HashName();
        }

        public override string ToString() => Name;
    }
}
