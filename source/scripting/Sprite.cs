using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using GTA.Native;

namespace GTA.UI
{
	public interface ISprite : IElement
	{
		SizeF Scale { get; set; }
		float Rotation { get; set; }
	}

	public class Sprite : ISprite, IDisposable
	{
		#region Fields
		string _textureDict, _textureName;
		#endregion

		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public SizeF Scale { get; set; }
		public float Rotation { get; set; }

		public Sprite(string textureDict, string textureName, SizeF scale, PointF position) : this(textureDict, textureName, scale, position, Color.WhiteSmoke, 0f)
		{
		}
		public Sprite(string textureDict, string textureName, SizeF scale, PointF position, Color color) : this(textureDict, textureName, scale, position, color, 0f)
		{
		}
		public Sprite(string textureDict, string textureName, SizeF scale, PointF position, Color color, float rotation)
		{
			_textureDict = textureDict;
			_textureName = textureName;

			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;

			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
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
			Draw(SizeF.Empty);
		}
		public virtual void Draw(SizeF offset)
		{
			if (!Enabled || !Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, _textureDict))
			{
				return;
			}

			float scaleX = Scale.Width / Screen.Width;
			float scaleY = Scale.Height / Screen.Height;
			float positionX = ((Position.X + offset.Width) / Screen.Width) + scaleX * 0.5f;
			float positionY = ((Position.Y + offset.Height) / Screen.Height) + scaleY * 0.5f;

			Function.Call(Hash.DRAW_SPRITE, _textureDict, _textureName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
		}
	}
	public class CustomSprite : ISprite
	{
		#region Fields
		int _id, _index, _lastDrawFrame;
		static int _globalLevel = 0, _globalLastDrawFrame = 0;
		static Dictionary<string, int> _textures = new Dictionary<string, int>();
		#endregion

		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public SizeF Scale { get; set; }
		public float Rotation { get; set; }

		public CustomSprite(string filename, SizeF scale, PointF position) : this(filename, scale, position, Color.WhiteSmoke, 0.0f)
		{
		}
		public CustomSprite(string filename, SizeF scale, PointF position, Color color) : this(filename, scale, position, color, 0.0f)
		{
		}
		public CustomSprite(string filename, SizeF scale, PointF position, Color color, float rotation)
		{
			if (!File.Exists(filename))
			{
				throw new FileNotFoundException(filename);
			}

			if (_textures.ContainsKey(filename))
			{
				_id = _textures[filename];
			}
			else
			{
				_id = MemoryAccess.CreateTexture(filename);
				_textures.Add(filename, _id);
			}

			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
		}

		public void Draw()
		{
			Draw(SizeF.Empty);
		}
		public void Draw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			int frameCount = Function.Call<int>(Hash.GET_FRAME_COUNT);

			if (_lastDrawFrame != frameCount)
			{
				_index = 0;
				_lastDrawFrame = frameCount;
			}
			if (_globalLastDrawFrame != frameCount)
			{
				_globalLevel = 0;
				_globalLastDrawFrame = frameCount;
			}

			float aspectRatio = Function.Call<float>(Hash._GET_SCREEN_ASPECT_RATIO, 0);

			float scaleX = Scale.Width / Screen.Width;
			float scaleY = Scale.Height / Screen.Height;
			float positionX = ((Position.X + offset.Width) / Screen.Width) + scaleX * 0.5f;
			float positionY = ((Position.Y + offset.Height) / Screen.Height) + scaleY * 0.5f;

			MemoryAccess.DrawTexture(_id, _index++, _globalLevel++, 100, scaleX, scaleY / aspectRatio, 0.5f, 0.5f, positionX, positionY, Rotation, aspectRatio, Color);
		}
	}
}
