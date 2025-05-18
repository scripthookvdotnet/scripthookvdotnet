//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
    /// <summary>
    /// An interface for streaming resources that can be requested and pinned by scripts
    /// (by increasing reference counts).
    /// </summary>
    public interface IScriptStreamingResource
    {
        /// <summary>
        /// Gets a value that indicates whether the resource is loaded and ready to use.
        /// </summary>
        bool IsLoaded
        {
            get;
        }

        /// <summary>
        /// <para>
        /// Requests the global streaming loader to load the steaming resource so it will be eventually loaded
        /// (unless getting interrupted by a <see cref="MarkAsNoLongerNeeded()"/> call of another SHVDN script).
        /// </para>
        /// <para>
        /// You will need to test if the resource is loaded with <see cref="IsLoaded"/> every frame until it is loaded
        /// before you can use it. The game starts loading pending streaming objects every frame
        /// (with `<c>CStreaming::Update()</c>`) before the script update call.
        /// </para>
        /// </summary>
        void Request();

        /// <summary>
        /// Tells the game we have finished using the streaming resource, so it can be freed from memory.
        /// </summary>
        void MarkAsNoLongerNeeded();
    }
}
