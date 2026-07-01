using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class VehicleWheel
    {
        /// <summary>
        /// Gets the script wheel index for native functions.
        /// </summary>
        [Obsolete("Use VehicleWheel.BoneId or VehicleWheel.ScriptIndex instead."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public int Index => (int)ScriptIndex;
    }
}
