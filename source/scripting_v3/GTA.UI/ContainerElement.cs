//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;

namespace GTA.UI
{
	public class ContainerElement : IWorldDrawableElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerElement"/> class used for grouping items on screen.
		/// </summary>
		public ContainerElement() :
			this(PointF.Empty, new SizeF(Screen.Width, Screen.Height), Color.Transparent, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerElement"/> class used for grouping items on screen.
		/// </summary>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="ContainerElement"/>.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="ContainerElement"/>.</param>
		public ContainerElement(PointF position, SizeF size) :
			this(position, size, Color.Transparent, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerElement"/> class used for grouping items on screen.
		/// </summary>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="ContainerElement"/>.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="ContainerElement"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="ContainerElement"/>.</param>
		public ContainerElement(PointF position, SizeF size, Color color) :
			this(position, size, color, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerElement"/> class used for grouping items on screen.
		/// </summary>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="ContainerElement"/>.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="ContainerElement"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="ContainerElement"/>.</param>
		/// <param name="centered">Position the <see cref="ContainerElement"/> based on its center instead of top left corner, see also <seealso cref="Centered"/>.</param>
		public ContainerElement(PointF position, SizeF size, Color color, bool centered)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
			Centered = centered;
			Items = new List<IElement>();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ContainerElement"/> will be drawn.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if enabled; otherwise, <see langword="false" />.
		/// </value>
		public virtual bool Enabled
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the color of this <see cref="ContainerElement"/>.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public virtual Color Color
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the position of this <see cref="ContainerElement"/>.
		/// </summary>
		/// <value>
		/// The position scaled on a 1280*720 pixel base.
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the position will be scaled by the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </remarks>
		public virtual PointF Position
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets the size to draw the <see cref="ContainerElement"/>
		/// </summary>
		/// <value>
		/// The size on a 1280*720 pixel base
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the size will be scaled by the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </remarks>
		public SizeF Size
		{
			get; set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ContainerElement"/> should be positioned based on its center or top left corner
		/// </summary>
		/// <value>
		///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
		/// </value>
		public virtual bool Centered
		{
			get; set;
		}

		/// <summary>
		/// The <see cref="IElement"/>s Contained inside this <see cref="ContainerElement"/>
		/// </summary>
		public List<IElement> Items
		{
			get; private set;
		}

		/// <summary>
		/// Draws this <see cref="ContainerElement" /> this frame.
		/// </summary>
		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="ContainerElement" /> this frame at the specified offset.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="ContainerElement" /> using a 1280*720 pixel base.</param>
		public virtual void Draw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			InternalDraw(offset, Screen.Width, Screen.Height);

			offset += new SizeF(Position);

			if (Centered)
			{
				offset -= new SizeF(Size.Width * 0.5f, Size.Height * 0.5f);
			}

			foreach (IElement item in Items)
			{
				item.Draw(offset);
			}
		}

		/// <summary>
		/// Draws this <see cref="ContainerElement" /> this frame using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="ContainerElement" /> this frame at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="ContainerElement" /> using a <see cref="Screen.ScaledWidth" />*720 pixel base.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);

			offset += new SizeF(Position);

			if (Centered)
			{
				offset -= new SizeF(Size.Width * 0.5f, Size.Height * 0.5f);
			}

			foreach (IElement item in Items)
			{
				item.ScaledDraw(offset);
			}
		}

		/// <summary>
		/// Draws this <see cref="ContainerElement"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="ContainerElement"/> to be drawn</param>
		public virtual void WorldDraw(Vector3 position)
		{
			WorldDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="ContainerElement"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="ContainerElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="ContainerElement"/> using a 1280*720 pixel base.</param>
		public virtual void WorldDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.Width, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}
		/// <summary>
		/// Draws this <see cref="ContainerElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="ContainerElement"/> to be drawn</param>
		public virtual void WorldScaledDraw(Vector3 position)
		{
			WorldScaledDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="ContainerElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="ContainerElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="ContainerElement"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		public virtual void WorldScaledDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			float w = Size.Width / screenWidth;
			float h = Size.Height / screenHeight;
			float x = (Position.X + offset.Width) / screenWidth;
			float y = (Position.Y + offset.Height) / screenHeight;

			if (!Centered)
			{
				x += w * 0.5f;
				y += h * 0.5f;
			}

			Function.Call(Hash.DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}
	}
}
