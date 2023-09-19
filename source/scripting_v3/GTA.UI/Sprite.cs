//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace GTA.UI
{
	/// <summary>
	/// A sprite element using a built-in texture.
	/// </summary>
	public class Sprite : ISpriteElement, IWorldDrawableElement, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Sprite"/> class used for drawing in game textures on the screen.
		/// </summary>
		/// <param name="textureDict">The Texture dictionary where the <see cref="Sprite"/> is stored (the *.ytd file).</param>
		/// <param name="textureName">Name of the <see cref="Sprite"/> inside the Texture dictionary.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="Sprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="Sprite"/>.</param>
		public Sprite(string textureDict, string textureName, SizeF size, PointF position) :
			this(textureDict, textureName, size, position, Color.WhiteSmoke, 0f, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Sprite"/> class used for drawing in game textures on the screen.
		/// </summary>
		/// <param name="textureDict">The Texture dictionary where the <see cref="Sprite"/> is stored (the *.ytd file).</param>
		/// <param name="textureName">Name of the <see cref="Sprite"/> inside the Texture dictionary.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="Sprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="Sprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="Sprite"/>.</param>
		public Sprite(string textureDict, string textureName, SizeF size, PointF position, Color color) :
			this(textureDict, textureName, size, position, color, 0f, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Sprite"/> class used for drawing in game textures on the screen.
		/// </summary>
		/// <param name="textureDict">The Texture dictionary where the <see cref="Sprite"/> is stored (the *.ytd file).</param>
		/// <param name="textureName">Name of the <see cref="Sprite"/> inside the Texture dictionary.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="Sprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="Sprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="Sprite"/>.</param>
		/// <param name="rotation">Set the rotation to draw the sprite, measured in degrees, see also <seealso cref="Rotation"/>.</param>
		public Sprite(string textureDict, string textureName, SizeF size, PointF position, Color color, float rotation) :
			this(textureDict, textureName, size, position, color, rotation, false)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Sprite"/> class used for drawing in game textures on the screen.
		/// </summary>
		/// <param name="textureDict">The Texture dictionary where the <see cref="Sprite"/> is stored (the *.ytd file).</param>
		/// <param name="textureName">Name of the <see cref="Sprite"/> inside the Texture dictionary.</param>
		/// <param name="size">Set the <see cref="Size"/> of the <see cref="Sprite"/>.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="Sprite"/>.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="Sprite"/>.</param>
		/// <param name="rotation">Set the rotation to draw the sprite, measured in degrees, see also <seealso cref="Rotation"/>.</param>
		/// <param name="centered">Position the <see cref="Sprite"/> based on its center instead of top left corner, see also <seealso cref="Centered"/>.</param>
		public Sprite(string textureDict, string textureName, SizeF size, PointF position, Color color, float rotation, bool centered)
		{
			byte[] data = Encoding.UTF8.GetBytes(textureDict + "\0");
			_pinnedDict = Marshal.AllocCoTaskMem(data.Length);
			Marshal.Copy(data, 0, _pinnedDict, data.Length);
			data = Encoding.UTF8.GetBytes(textureName + "\0");
			_pinnedName = Marshal.AllocCoTaskMem(data.Length);
			Marshal.Copy(data, 0, _pinnedName, data.Length);

			_textureDict = textureDict;
			_textureName = textureName;

			Enabled = true;
			Size = size;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;

			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, _pinnedDict);

			int hashedDictName = Game.GenerateHash(textureDict);
			if (s_activeTextures.ContainsKey(hashedDictName))
			{
				s_activeTextures[hashedDictName] += 1;
			}
			else
			{
				s_activeTextures.Add(hashedDictName, 1);
			}
		}

		#region Fields
		private readonly string _textureDict, _textureName;
		/// <summary>
		/// The dictionary to count how many instances use the same texture dictionary.
		/// Using hashes for texture dictionary names should do the job since the game uses hashes for those names in the fwTxdStore.
		/// </summary>
		private static readonly Dictionary<int, int> s_activeTextures = new();
		private IntPtr _pinnedDict, _pinnedName;
		private bool _disposed;
		private RectangleF _textureCoordinates;
		#endregion

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing || _disposed)
			{
				return;
			}

			int hashedDictName = Game.GenerateHash(_textureDict);
			if (s_activeTextures.TryGetValue(hashedDictName, out int currentCount))
			{
				if (currentCount == 1)
				{
					Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _pinnedDict);
					s_activeTextures.Remove(hashedDictName);
				}
				else
				{
					s_activeTextures[hashedDictName] = currentCount - 1;
				}
			}
			else
			{
				// In practice this should never get executed
				Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _pinnedDict);
			}

			Marshal.FreeCoTaskMem(_pinnedDict);
			Marshal.FreeCoTaskMem(_pinnedName);
			_pinnedDict = IntPtr.Zero;
			_pinnedName = IntPtr.Zero;

			_disposed = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Sprite" /> will be drawn.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if enabled; otherwise, <see langword="false" />.
		/// </value>
		public bool Enabled
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the color of this <see cref="Sprite" />.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the position of this <see cref="Sprite" />.
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
		/// Gets or sets the texture coordinates of this <see cref="Sprite" />.
		/// Currently only supports in v1.0.1868.0 or later game versions, but may support all game version in the future
		/// since there is the function that DRAW_SPRITE_ARX_WITH_UV eventually calls in all game versions.
		/// </summary>
		/// <value>
		/// The texture coordinates allows to specify the coordinates (measured in percentage: 1f = 100%), used to draw the sprite.
		/// </value>
		public RectangleF TextureCoordinates
		{
			get => _textureCoordinates;
			set
			{
				// Although you can find what DRAW_SPRITE_ARX_WITH_UV eventually calls with ["48 8B 41 10 66 39 70 58 74 06 48 8B 78 50 EB 07" - 0xA],
				// but we have to prevent the setter from calling in the game versions prior to b1868 since we haven't find a alternative way to do what DRAW_SPRITE_ARX_WITH_UV does
				if (Game.Version < GameVersion.v1_0_1868_0_Steam)
				{
					GameVersionNotSupportedException.ThrowIfNotSupported((GameVersion.v1_0_1868_0_Steam), nameof(Sprite), nameof(TextureCoordinates));
				}

				_textureCoordinates = value;
			}
		}
		/// <summary>
		/// Gets or sets the size to draw the <see cref="Sprite" />
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
		/// Gets or sets the rotation to draw this <see cref="Sprite" />.
		/// </summary>
		/// <value>
		/// The rotation measured in degrees, clockwise increasing, 0.0 at vertical
		/// </value>
		public float Rotation
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Sprite"/> should be positioned based on its center or top left corner
		/// </summary>
		/// <value>
		///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
		/// </value>
		public bool Centered
		{
			get; set;
		}

		/// <summary>
		/// Draws this <see cref="Sprite" />.
		/// </summary>
		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="Sprite" /> at the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public virtual void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}

		/// <summary>
		/// Draws this <see cref="Sprite" /> using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="Sprite" /> at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
		}

		/// <summary>
		/// Draws this <see cref="Sprite"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="Sprite"/> to be drawn</param>
		public virtual void WorldDraw(Vector3 position)
		{
			WorldDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="Sprite"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="Sprite"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="Sprite"/> using a 1280*720 pixel base.</param>
		public virtual void WorldDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.Width, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}
		/// <summary>
		/// Draws this <see cref="Sprite"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="Sprite"/> to be drawn</param>
		public virtual void WorldScaledDraw(Vector3 position)
		{
			WorldScaledDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="Sprite"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="Sprite"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="Sprite"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		public virtual void WorldScaledDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			if (!Enabled || !Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, _textureDict))
			{
				return;
			}

			float scaleX = Size.Width / screenWidth;
			float scaleY = Size.Height / screenHeight;
			float positionX = (Position.X + offset.Width) / screenWidth;
			float positionY = (Position.Y + offset.Height) / screenHeight;

			if (!Centered)
			{
				positionX += scaleX * 0.5f;
				positionY += scaleY * 0.5f;
			}

			if (TextureCoordinates != RectangleF.Empty)
			{
				float u1 = TextureCoordinates.Top;
				float v1 = TextureCoordinates.Left;
				float u2 = TextureCoordinates.Right;
				float v2 = TextureCoordinates.Bottom;

				Function.Call(Hash.DRAW_SPRITE_ARX_WITH_UV, _pinnedDict, _pinnedName, positionX, positionY, scaleX, scaleY, u1, v1, u2, v2, Rotation, Color.R, Color.G, Color.B, Color.A);

				return;
			}

			Function.Call(Hash.DRAW_SPRITE, _pinnedDict, _pinnedName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
		}
	}
}
