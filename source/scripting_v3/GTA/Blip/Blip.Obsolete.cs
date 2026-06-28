using System;
using System.ComponentModel;
using GTA.Native;

namespace GTA
{
    public sealed partial class Blip
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Blip"/> shows the dollar sign at the top left corner of the <see cref="Blip"/>.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> to show the dollar sign; otherwise, <see langword="false" />.
        /// </value>
        [Obsolete(
            "`Blip.ShowsDollarSign` is obsolete because the setter changes whether to show the tick, and also " +
            "because `SHOW_FOR_SALE_ICON_ON_BLIP` was added in b2802, which reveals `ShowsDollarSign` is too " +
            "different from internal flags for the blip \"for sale\" icon. For what the setter does, use " +
            "`Blip.ShowsTick` instead."),
            EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShowsDollarSign
        {
            get => SHVDN.NativeMemory.GetBlipPropertyFlag(MemoryAddress, SHVDN.BlipPropertyFlags.ShowForSale);
            set => Function.Call(Hash.SHOW_TICK_ON_BLIP, Handle, value);
        }

        /// <summary>
        /// Sets the scale of this <see cref="Blip"/> on the map.
        /// </summary>
        [Obsolete("Use Blip.ScaleF instead.")]
        public float Scale
        {
            set => Function.Call(Hash.SET_BLIP_SCALE, Handle, value);
        }

        /// <summary>
        /// Gets or sets the x-axis scale of this <see cref="Blip"/> on the map.
        /// The value is the same as <see cref="ScaleY"/> in v1.0.393.4 or earlier versions.
        /// </summary>
        [Obsolete("Use Blip.ScaleF instead.")]
        public float ScaleX
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0;

                return SHVDN.MemDataMarshal.ReadFloat(address + 0x50);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return;

                SHVDN.MemDataMarshal.WriteFloat(address + 0x50, value);
            }
        }

        /// <summary>
        /// Gets or sets the y-axis scale of this <see cref="Blip"/> on the map.
        /// The value is the same as <see cref="ScaleX"/> in v1.0.393.4 or earlier versions.
        /// </summary>
        [Obsolete("Use Blip.ScaleF instead.")]
        public float ScaleY
        {
            get
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return 0;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_463_1 ? 0x54 : 0x50;
                return SHVDN.MemDataMarshal.ReadFloat(address + offset);
            }
            set
            {
                if (!TryGetMemoryAddress(out IntPtr address))
                    return;

                int offset = Game.FileVersion >= ExeVersionConsts.v1_0_463_1 ? 0x54 : 0x50;
                SHVDN.MemDataMarshal.WriteFloat(address + offset, value);
            }
        }
    }
}
