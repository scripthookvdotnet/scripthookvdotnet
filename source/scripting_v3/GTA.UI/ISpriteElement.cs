//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Drawing;

namespace GTA.UI
{
	public interface ISpriteElement : IElement
	{
		/// <summary>
		/// Gets or sets the size to draw the <see cref="ISpriteElement"/>
		/// </summary>
		/// <value>
		/// The size on a 1280*720 pixel base
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the size will be scaled by the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </remarks>
		SizeF Size
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the rotation to draw this <see cref="ISpriteElement"/>.
		/// </summary>
		/// <value>
		/// The rotation measured in degrees, clockwise increasing, 0.0 at vertical
		/// </value>
		float Rotation
		{
			get; set;
		}
	}
}
