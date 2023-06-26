//
// Copyright (C) 2015 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using System.Drawing;

namespace GTA.UI
{
	public interface IWorldDrawableElement : IElement
	{
		/// <summary>
		/// Draws this <see cref="IWorldDrawableElement"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IWorldDrawableElement"/> to be drawn</param>
		void WorldDraw(Vector3 position);
		/// <summary>
		/// Draws this <see cref="IWorldDrawableElement"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IWorldDrawableElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IWorldDrawableElement"/> using a 1280*720 pixel base.</param>
		void WorldDraw(Vector3 position, SizeF offset);

		/// <summary>
		/// Draws this <see cref="IWorldDrawableElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IWorldDrawableElement"/> to be drawn</param>
		void WorldScaledDraw(Vector3 position);
		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IWorldDrawableElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IWorldDrawableElement"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		void WorldScaledDraw(Vector3 position, SizeF offset);
	}
}
