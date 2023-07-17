//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace GTA.UI
{
	/// <summary>
	/// A sprite element using a custom image texture.
	/// </summary>
	public class CustomSprite : ISpriteElement, IWorldDrawableElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomSprite"/> class used for drawing external textures on the screen.
		/// </summary>
		/// <param name="filename">Full path to location of the <see cref="CustomSprite"/> on the disc.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="CustomSprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="CustomSprite"/>.</param>
		/// <exception cref="FileNotFoundException">Thrown if the specified file doesn't exist</exception>
		public CustomSprite(string filename, SizeF size, PointF position) :
			this(filename, size, position, Color.WhiteSmoke, 0.0f, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomSprite"/> class used for drawing external textures on the screen.
		/// </summary>
		/// <param name="filename">Full path to location of the <see cref="CustomSprite"/> on the disc.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="CustomSprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="CustomSprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="CustomSprite"/>.</param>
		/// <exception cref="FileNotFoundException">Thrown if the specified file doesn't exist</exception>
		public CustomSprite(string filename, SizeF size, PointF position, Color color) :
			this(filename, size, position, color, 0.0f, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomSprite"/> class used for drawing external textures on the screen.
		/// </summary>
		/// <param name="filename">Full path to location of the <see cref="CustomSprite"/> on the disc.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="CustomSprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="CustomSprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="CustomSprite"/>.</param>
		/// <param name="rotation">Set the rotation to draw the sprite, measured in degrees, see also <seealso cref="Rotation"/>.</param>
		/// <exception cref="FileNotFoundException">Thrown if the specified file doesn't exist</exception>
		public CustomSprite(string filename, SizeF size, PointF position, Color color, float rotation) :
			this(filename, size, position, color, rotation, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomSprite"/> class used for drawing external textures on the screen.
		/// </summary>
		/// <param name="filename">Full path to location of the <see cref="CustomSprite"/> on the disc.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="CustomSprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="CustomSprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="CustomSprite"/>.</param>
		/// <param name="rotation">Set the rotation to draw the sprite, measured in degrees, see also <seealso cref="Rotation"/>.</param>
		/// <param name="centered">Position the <see cref="CustomSprite"/> based on its center instead of top left corner, see also <seealso cref="Centered"/>.</param>
		/// <exception cref="FileNotFoundException">Thrown if the specified file doesn't exist</exception>
		public CustomSprite(string filename, SizeF size, PointF position, Color color, float rotation, bool centered)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(filename);
			}

			if (s_textures.TryGetValue(filename, out int texture))
			{
				_id = texture;
			}
			else
			{
				_id = SHVDN.NativeMemory.CreateTexture(filename);
				s_textures.Add(filename, _id);
			}

			if (!s_indexes.ContainsKey(_id))
			{
				s_indexes.Add(_id, 0);
			}
			if (!s_lastDraw.ContainsKey(_id))
			{
				s_lastDraw.Add(_id, 0);
			}

			Enabled = true;
			Size = size;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;
		}

		#region Fields
		int _id;
		static int s_globalLevel, s_globalLastDrawFrame;
		static Dictionary<string, int> s_textures = new();
		static Dictionary<int, int> s_lastDraw = new();
		static Dictionary<int, int> s_indexes = new();
		#endregion

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CustomSprite" /> will be drawn.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if enabled; otherwise, <see langword="false" />.
		/// </value>
		public bool Enabled
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the color of this <see cref="CustomSprite" />.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the position of this <see cref="CustomSprite" />.
		/// </summary>
		/// <value>
		/// The position scaled on a 1280*720 pixel base.
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the position will be scaled by the width returned in <see cref="Screen.ScaledWidth" />.
		/// </remarks>
		public PointF Position
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the size to draw the <see cref="CustomSprite" />
		/// </summary>
		/// <value>
		/// The size on a 1280*720 pixel base
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the size will be scaled by the width returned in <see cref="Screen.ScaledWidth" />.
		/// </remarks>
		public SizeF Size
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the rotation to draw this <see cref="CustomSprite" />.
		/// </summary>
		/// <value>
		/// The rotation measured in degrees, clockwise increasing, 0.0 at vertical
		/// </value>
		public float Rotation
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CustomSprite"/> should be positioned based on its center or top left corner
		/// </summary>
		/// <value>
		///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
		/// </value>
		public bool Centered
		{
			get; set;
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite" />.
		/// </summary>
		public void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="CustomSprite" /> at the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite" /> using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="CustomSprite" /> at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="CustomSprite"/> to be drawn</param>
		public virtual void WorldDraw(Vector3 position)
		{
			WorldDraw(position, SizeF.Empty);
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="CustomSprite"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="CustomSprite"/> using a 1280*720 pixel base.</param>
		public virtual void WorldDraw(Vector3 position, SizeF offset)
		{
			PointF pointF = Screen.WorldToScreen(position);
			if (!(pointF == PointF.Empty))
			{
				InternalDraw(new SizeF(pointF) + offset, Screen.Width, Screen.Height);
			}
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="CustomSprite"/> to be drawn</param>
		public virtual void WorldScaledDraw(Vector3 position)
		{
			WorldScaledDraw(position, SizeF.Empty);
		}

		/// <summary>
		/// Draws this <see cref="CustomSprite"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="CustomSprite"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="CustomSprite"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		public virtual void WorldScaledDraw(Vector3 position, SizeF offset)
		{
			PointF pointF = Screen.WorldToScreen(position, scaleWidth: true);
			if (!(pointF == PointF.Empty))
			{
				InternalDraw(new SizeF(pointF) + offset, Screen.ScaledWidth, Screen.Height);
			}
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			if (!Enabled)
			{
				return;
			}

			int frameCount = Function.Call<int>(Hash.GET_FRAME_COUNT);

			if (s_lastDraw[_id] != frameCount)
			{
				s_indexes[_id] = 0;
				s_lastDraw[_id] = frameCount;
			}
			if (s_globalLastDrawFrame != frameCount)
			{
				s_globalLevel = 0;
				s_globalLastDrawFrame = frameCount;
			}

			float scaleX = Size.Width / screenWidth;
			float scaleY = Size.Height / screenHeight;
			float positionX = (Position.X + offset.Width) / screenWidth;
			float positionY = (Position.Y + offset.Height) / screenHeight;
			float aspectRatio = Screen.AspectRatio;

			if (!Centered)
			{
				positionX += scaleX * 0.5f;
				positionY += scaleY * 0.5f;
			}

			SHVDN.NativeMemory.DrawTexture(_id, s_indexes[_id]++, s_globalLevel++, 100, scaleX, scaleY / aspectRatio, 0.5f, 0.5f, positionX, positionY, Rotation * 0.00277777778f, aspectRatio, Color.R / 255f, Color.G / 255f, Color.B / 255f, Color.A / 255f);
		}
	}
}
