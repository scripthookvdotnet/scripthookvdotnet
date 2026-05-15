//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;

namespace GTA
{
    /// <summary>Represents a fire in the game world.</summary>
    public sealed class ScriptFire : PoolObject
    {
        /// <summary>Initializes a new instance of the <see cref="ScriptFire"/> class.</summary>
        /// <param name="handle">The handle of the fire.</param>
        internal ScriptFire(int handle) : base(handle)
        {
        }

        /// <summary>Creates a new <see cref="ScriptFire"/> from an existing handle.</summary>
        /// <param name="handle">The handle of the fire.</param>
        /// <returns>A new <see cref="ScriptFire"/> instance.</returns>
        public static ScriptFire FromHandle(int handle) => new ScriptFire(handle);

        /// <summary>Removes this <see cref="ScriptFire"/> from the game world.</summary>
        public override void Delete() => Function.Call(Hash.REMOVE_SCRIPT_FIRE, Handle);

        /// <summary> Determines if this <see cref="ScriptFire"/> still exists in the game world. </summary>
        public override bool Exists() => SHVDN.NativeMemory.IsScriptFireHandleValid(Handle);
    }
}
