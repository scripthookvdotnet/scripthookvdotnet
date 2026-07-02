using System;
using System.ComponentModel;
using GTA.Native;

namespace GTA
{
    public sealed partial class Blip
    {
        /// <summary>
        /// Gets the type of this <see cref="Blip"/>.
        /// </summary>
        [Obsolete("Use Blip.BlipType instead.")]
        public int Type => Function.Call<int>(Hash.GET_BLIP_INFO_ID_TYPE, Handle);

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
    }
}
