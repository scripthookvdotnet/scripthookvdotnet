using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class PedProp : IPedVariation
    {
        [Obsolete("PedProp.Type is obsolete, use PedProp.AnchorPoint instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public PedPropType Type => (PedPropType)AnchorPoint;

        [Obsolete("PedProp.HasTextureVariations is obsolete because it does not make sense " +
          "as texture count cannot be determined without specifying both prop position id and drawable id."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasTextureVariations => Count > 1 && TextureCount > 1;

        [Obsolete("PedProp.HasAnyVariations is obsolete because it does not make sense " +
          "as texture count cannot be determined without specifying both prop position id and drawable id."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public bool HasAnyVariations => HasVariations;
    }
}
