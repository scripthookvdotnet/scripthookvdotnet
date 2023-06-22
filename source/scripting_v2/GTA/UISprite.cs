//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Drawing;

namespace GTA
{
	public class UISprite : UIElement, IDisposable
	{
		private readonly string _textureDict;
		private readonly string _textureName;

		public UISprite(string textureDict, string textureName, Size scale, Point position) : this(textureDict, textureName, scale, position, Color.White, 0.0f)
		{
		}
		public UISprite(string textureDict, string textureName, Size scale, Point position, Color color) : this(textureDict, textureName, scale, position, color, 0.0f)
		{
		}
		public UISprite(string textureDict, string textureName, Size scale, Point position, Color color, float rotation)
		{
			Enabled = true;
			_textureDict = textureDict;
			_textureName = textureName;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;

			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
		}

		public virtual bool Enabled
		{
			get; set;
		}
		public virtual Point Position
		{
			get; set;
		}
		public virtual Color Color
		{
			get; set;
		}
		public Size Scale
		{
			get; set;
		}
		public float Rotation
		{
			get; set;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _textureDict);
			}
		}

		public virtual void Draw()
		{
			Draw(new Size());
		}
		public virtual void Draw(Size offset)
		{
			if (!Enabled || !Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, _textureDict))
			{
				return;
			}

			float w = (float)Scale.Width / UI.WIDTH;
			float h = (float)Scale.Height / UI.HEIGHT;
			float x = (float)(Position.X + offset.Width) / UI.WIDTH + w * 0.5f;
			float y = (float)(Position.Y + offset.Height) / UI.HEIGHT + h * 0.5f;

			Function.Call(Hash.DRAW_SPRITE, _textureDict, _textureName, x, y, w, h, Rotation, Color.R, Color.G, Color.B, Color.A);
		}
	}
}
