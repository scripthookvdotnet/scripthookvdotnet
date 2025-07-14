//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
    /// <summary>
    /// Represents a dictionary struct for a creature clip/animation dictionary, which is created from a ycd file.
    /// Note that clip/animation dictionaries are different from clip sets, which is defined in
    /// <c>clip_sets.ymt</c> or <c>clip_sets.xml</c> files (you can request clip sets with <see cref="ClipSet"/>).
    /// </summary>
    /// <remarks>
    /// Although some natives have the string "ANIM_DICT" but there's no ones that have "CLIP_DICT" on the other hand,
    /// GTA V doesn't have game classes for animation dictionaries different from clip dictionaries
    /// while the exe has <c>rage::crClipDictionary</c> and <c>rage::fwClipDictionaryStore</c> classes.
    /// </remarks>
    public readonly struct CrClipDictionary : IEquatable<CrClipDictionary>, IScriptStreamingResource
    {
        public CrClipDictionary(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// Computes the hash of <see cref="Name"/> in the same way as how the game calculates hashes for clip
        /// dictionaries to store in the global <c>rage::fwClipDictionaryStore</c> and as how
        /// <see cref="StringHash.AtStringHash(string, uint)"/> calculates.
        /// May be useful when you want to get the identifier in the same way as how the game handles texture
        /// dictionaries or when you investigate game memory to see how clips are stored in clip dictionaries.
        /// </summary>
        /// <returns>The hash value calculated from <see cref="Name"/>.</returns>
        public AtHashValue HashName() => AtHashValue.FromString(Name);

        /// <summary>
        /// Gets a value indicating whether this <see cref="CrClipDictionary"/> exists
        /// in the <c>fwClipDictionaryStore</c> pool.
        /// </summary>
        public bool Exists => Function.Call<bool>(Hash.DOES_ANIM_DICT_EXIST, Name);

        /// <summary>
        /// Gets a value indicating whether this <see cref="CrClipDictionary"/> is loaded
        /// so the animations of this <see cref="CrClipDictionary"/> are ready to use.
        /// </summary>
        public bool IsLoaded => Function.Call<bool>(Hash.HAS_ANIM_DICT_LOADED, Name);

        /// <summary>
        /// <para>
        /// Requests the global streaming loader to load this <see cref="CrClipDictionary"/> so it will be eventually
        /// loaded (unless getting interrupted by a <see cref="MarkAsNoLongerNeeded()"/> call of another SHVDN script).
        /// </para>
        /// <para>
        /// You will need to test if the <see cref="CrClipDictionary"/> is loaded with <see cref="IsLoaded"/> every
        /// frame until it is loaded before you can use it. The game starts loading pending streaming objects every
        /// frame (with `<c>CStreaming::Update()</c>`) before the script update call.
        /// </para>
        /// </summary>
        public void Request()
        {
            Function.Call(Hash.REQUEST_ANIM_DICT, Name);
        }

        /// <summary>
        /// Tells the game we have finished using this <see cref="CrClipDictionary"/> and it can be freed from memory.
        /// </summary>
        public void MarkAsNoLongerNeeded()
        {
            Function.Call(Hash.REMOVE_ANIM_DICT, Name);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the Jenkins-one-at-a-time (joaat) hash value of <see cref="Name"/>
        /// matches that of <paramref name="other"/>, as the game uses joaat hashes as identifiers of clip dictionaries.
        /// </summary>
        public bool Equals(CrClipDictionary other)
        {
            return HashName() == other.HashName();
        }
        public override bool Equals(object obj)
        {
            if (obj is CrClipDictionary clipDict)
            {
                return Equals(clipDict);
            }

            return false;
        }

        public static bool operator ==(CrClipDictionary left, CrClipDictionary right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(CrClipDictionary left, CrClipDictionary right)
        {
            return !left.Equals(right);
        }

        public static implicit operator InputArgument(CrClipDictionary value)
        {
            return new InputArgument(value.Name);
        }
        public static explicit operator CrClipDictionary(string value)
        {
            return new CrClipDictionary(value);
        }
        public static explicit operator string(CrClipDictionary value)
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
