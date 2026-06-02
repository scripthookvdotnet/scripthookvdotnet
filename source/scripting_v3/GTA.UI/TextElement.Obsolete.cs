using System;
using System.ComponentModel;

namespace GTA.UI
{
    public partial class TextElement : IWorldDrawableElement
    {
        /// <summary>
        /// Gets or sets a value indicating whether the alignment of this <see cref="TextElement" /> is centered.
        /// See <see cref="Alignment"/>
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
        /// </value>
        [Obsolete("`TextElement.Centered` is obsolete because it is redundant and setting the property to false is " +
            "confusing. Use `TextElement.Alignment` instead."), EditorBrowsable(EditorBrowsableState.Never)]
        public bool Centered
        {
            get => Alignment == Alignment.Center;
            set => Alignment = value ? Alignment.Center : Alignment.Left;
        }
    }
}
