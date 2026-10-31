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

        /// <summary>
        /// Sets a value indicating whether this <see cref="VehicleWheel"/> is burst.
        /// </summary>
        [Obsolete("Use VehicleWheel.IsBurst instead.")]
        public bool IsBursted
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address) || SHVDN.NativeMemory.Vehicle.CWheelTireHealthOffset == 0)
                    return false;

                return SHVDN.MemDataMarshal.ReadFloat(address + SHVDN.NativeMemory.Vehicle.CWheelTireHealthOffset) <= 0f;
            }
        }
    }
}
