//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using System.Drawing;

namespace GTA.UI
{
	public interface IElement
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IElement"/> will be drawn.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if enabled; otherwise, <see langword="false" />.
		/// </value>
		bool Enabled
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the color of this <see cref="IElement"/>.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		Color Color
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="IElement"/>.
		/// </summary>
		/// <value>
		/// The position scaled on a 1280*720 pixel base.
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the position will be scaled by the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </remarks>
		PointF Position
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IElement"/> should be positioned based on its center or top left corner
		/// </summary>
		/// <value>
		///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
		/// </value>
		bool Centered
		{
			get; set;
		}

		/// <summary>
		/// Draws this <see cref="IElement"/> this frame.
		/// </summary>
		void Draw();
		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified offset.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IElement"/> using a 1280*720 pixel base.</param>
		void Draw(SizeF offset);

		/// <summary>
		/// Draws this <see cref="IElement"/> this frame using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		void ScaledDraw();
		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IElement"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		void ScaledDraw(SizeF offset);

		/// <summary>
		/// Draws this <see cref="IElement"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IElement"/> to be drawn</param>
		void WorldDraw(Vector3 position);
		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IElement"/> using a 1280*720 pixel base.</param>
		void WorldDraw(Vector3 position, SizeF offset);

		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IElement"/> to be drawn</param>
		void WorldScaledDraw(Vector3 position);
		/// <summary>
		/// Draws this <see cref="IElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="IElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="IElement"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		void WorldScaledDraw(Vector3 position, SizeF offset);
	}
}
