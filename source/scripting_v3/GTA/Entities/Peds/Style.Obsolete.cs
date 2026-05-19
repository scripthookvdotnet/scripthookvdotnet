using System;
using System.ComponentModel;

namespace GTA
{
    public sealed partial class Style
    {
        [Obsolete("Use the indexer overload with the type PedPropAnchorPoint instead."),
         EditorBrowsable(EditorBrowsableState.Never)]
        public PedProp this[PedPropType propId] => this[(PedPropAnchorPoint)propId];
    }
}
