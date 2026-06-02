using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class PedComponent : IPedVariation
    {
        /// <summary>
        /// Returns <see langword="true"/> if there are textures for current drawable id (<see cref="Index"/>).
        /// </summary>
        [Obsolete("PedComponent.HasTextureVariations is obsolete because it does not make sense " +
                  "as texture count cannot be determined without specifying both component id and drawable id."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasTextureVariations => Count > 0 && TextureCount > 1;

        [Obsolete("PedComponent.HasAnyVariation is obsolete because it does not make sense " +
                  "as texture count cannot be determined without specifying both component id and drawable id."),
        EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasAnyVariations => HasVariations || HasTextureVariations;
    }
}
