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
		static Dictionary<string, int> _activeTextures = new Dictionary<string, int>();
		#endregion

		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public SizeF Scale { get; set; }
		public float Rotation { get; set; }
		public bool Centered { get; set; }

		public Sprite(string textureDict, string textureName, SizeF scale, PointF position)
			: this(textureDict, textureName, scale, position, Color.WhiteSmoke, 0f, false)
		{
		}
		public Sprite(string textureDict, string textureName, SizeF scale, PointF position, Color color)
			: this(textureDict, textureName, scale, position, color, 0f, false)
		{
		}
		public Sprite(string textureDict, string textureName, SizeF scale, PointF position, Color color, float rotation)
			: this(textureDict, textureName, scale, position, color, rotation, false)
		{
		}
		public Sprite(string textureDict, string textureName, SizeF scale, PointF position, Color color, float rotation, bool centered)
		{
			_textureDict = textureDict;
			_textureName = textureName;

			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;

			Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, _textureDict);
			if (_activeTextures.ContainsKey(textureDict.ToLower()))
			{
				_activeTextures[textureDict.ToLower()] += 1;
			}
			else
			{
				_activeTextures.Add(textureDict.ToLower(), 1);
			}
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
				if (_activeTextures.ContainsKey(_textureDict.ToLower()))
				{
					int current = _activeTextures[_textureDict.ToLower()];
					if (current == 1)
					{
						Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _textureDict);
						_activeTextures.Remove(_textureDict.ToLower());
					}
					else
					{
						_activeTextures[_textureDict.ToLower()] = current - 1;
					}
				}
				else
				{
					//In practice this should never get executed
					Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, _textureDict);
				}

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
			float positionX = ((Position.X + offset.Width) / Screen.Width) + ((!Centered) ? scaleX * 0.5f : 0.0f);
			float positionY = ((Position.Y + offset.Height) / Screen.Height) + ((!Centered) ? scaleY * 0.5f : 0.0f);

			Function.Call(Hash.DRAW_SPRITE, _textureDict, _textureName, positionX, positionY, scaleX, scaleY, Rotation, Color.R, Color.G, Color.B, Color.A);
		}
	}
	public class CustomSprite : ISprite
	{
		#region Fields
		int _id;
		static int _globalLevel = 0, _globalLastDrawFrame = 0;
		static Dictionary<string, int> _textures = new Dictionary<string, int>();
		static Dictionary<int, int> _lastDraw = new Dictionary<int, int>();
		static Dictionary<int, int> _indexes = new Dictionary<int, int>();
		#endregion

		public bool Enabled { get; set; }
		public Color Color { get; set; }
		public PointF Position { get; set; }
		public SizeF Scale { get; set; }
		public float Rotation { get; set; }
		public bool Centered { get; set; }

		public CustomSprite(string filename, SizeF scale, PointF position)
			: this(filename, scale, position, Color.WhiteSmoke, 0.0f, false)
		{
		}
		public CustomSprite(string filename, SizeF scale, PointF position, Color color)
			: this(filename, scale, position, color, 0.0f, false)
		{
		}
		public CustomSprite(string filename, SizeF scale, PointF position, Color color, float rotation)
			: this(filename, scale, position, color, rotation, false)
		{
		}
		public CustomSprite(string filename, SizeF scale, PointF position, Color color, float rotation, bool centered)
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
			if (!_indexes.ContainsKey(_id))
			{
				_indexes.Add(_id, 0);
			}
			if (!_lastDraw.ContainsKey(_id))
			{
				_lastDraw.Add(_id, 0);
			}

			Enabled = true;
			Scale = scale;
			Position = position;
			Color = color;
			Rotation = rotation;
			Centered = centered;
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

			if (_lastDraw[_id] != frameCount)
			{
				_lastDraw[_id] = frameCount;
				_indexes[_id] = 0;
			}
			if (_globalLastDrawFrame != frameCount)
			{
				_globalLevel = 0;
				_globalLastDrawFrame = frameCount;
			}

			float aspectRatio = Function.Call<float>(Hash._GET_SCREEN_ASPECT_RATIO, 0);

			float scaleX = Scale.Width / Screen.Width;
			float scaleY = Scale.Height / Screen.Height;
			float positionX = ((Position.X + offset.Width) / Screen.Width) + ((!Centered) ? scaleX * 0.5f : 0.0f);
			float positionY = ((Position.Y + offset.Height) / Screen.Height) + ((!Centered) ? scaleY * 0.5f : 0.0f);

			MemoryAccess.DrawTexture(_id, _indexes[_id]++, _globalLevel++, 100, scaleX, scaleY / aspectRatio, 0.5f, 0.5f, positionX, positionY, Rotation, aspectRatio, Color);
		}
	}
}
