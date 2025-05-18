//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;

namespace GTA
{
    /// <summary>
    /// Represents a dictionary struct for a clip/animation set, which should represent a key name for <c>fwClipSet</c>.
    /// Clip/Animation sets are defined in <c>clip_sets.ymt</c> (compiled file of <c>clip_sets.pso.meta</c> according to
    /// the official scripting headers) or <c>clip_sets.xml</c> files.
    /// Note that clip/animation sets are different from clip/animation dictionaries, which is created from <c>ycd</c>
    /// files (you can request clip sets with <see cref="CrClipDictionary"/>).
    /// </summary>
    public readonly struct ClipSet : IEquatable<ClipSet>, IScriptStreamingResource
    {
        public ClipSet(string name) : this()
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
        /// Computes the hash of <see cref="Name"/> in the same way as how the game calculates hashes for clip sets to
        /// store in the global <c>rage::fwClipSetManager</c> and as how
        /// <see cref="StringHash.AtStringHash(string, uint)"/> calculates.
        /// May be useful when you want to get the identifier in the same way as how the game handles clip sets or when
        /// you investigate game memory to see how clip sets (<c>rage::fwClipSet</c>) are stored in the
        /// <c>rage::fwClipSetManager</c>.
        /// </summary>
        /// <returns>The hash value calculated from <see cref="Name"/>.</returns>
        public AtHashValue HashName() => AtHashValue.FromString(Name);

        /// <summary>
        /// Gets a value indicating whether this <see cref="ClipSet"/> is loaded
        /// so the animations of this <see cref="ClipSet"/> are ready to use.
        /// </summary>
        public bool IsLoaded => Function.Call<bool>(Hash.HAS_CLIP_SET_LOADED, Name);

        /// <summary>
        /// <para>
        /// Requests the global streaming loader to load this <see cref="ClipSet"/> so it will be eventually loaded
        /// (unless getting interrupted by a <see cref="MarkAsNoLongerNeeded()"/> call of another SHVDN script).
        /// </para>
        /// <para>
        /// You will need to test if the resource is loaded with <see cref="IsLoaded"/> every frame until
        /// the <see cref="ClipSet"/> is loaded before you can use it. The game starts loading pending streaming objects
        /// every frame (with `<c>CStreaming::Update()</c>`) before the script update call.
        /// </para>
        /// </summary>
        public void Request()
        {
            Function.Call(Hash.REQUEST_CLIP_SET, Name);
        }

        /// <summary>
        /// Tells the game we have finished using this <see cref="ClipSet"/> and it can be freed from memory.
        /// </summary>
        public void MarkAsNoLongerNeeded()
        {
            Function.Call(Hash.REMOVE_CLIP_SET, Name);
        }

        public bool Equals(ClipSet other)
        {
            return HashName() == other.HashName();
        }
        public override bool Equals(object obj)
        {
            if (obj is ClipSet model)
            {
                return Equals(model);
            }

            return false;
        }

        public static bool operator ==(ClipSet left, ClipSet right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(ClipSet left, ClipSet right)
        {
            return !left.Equals(right);
        }

        public static implicit operator InputArgument(ClipSet value)
        {
            return new InputArgument(value.Name);
        }
        public static explicit operator ClipSet(string value)
        {
            return new ClipSet(value);
        }
        public static explicit operator string(ClipSet value)
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
