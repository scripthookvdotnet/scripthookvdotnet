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
	public class TextElement : IWorldDrawableElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		public TextElement(string caption, PointF position, float scale) :
			this(caption, position, scale, Color.WhiteSmoke, Font.ChaletLondon, Alignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		public TextElement(string caption, PointF position, float scale, Color color) :
			this(caption, position, scale, color, Font.ChaletLondon, Alignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="font">Sets the <see cref="Font"/> used when drawing the text.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Font font) :
			this(caption, position, scale, color, font, Alignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="font">Sets the <see cref="Font"/> used when drawing the text.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="GTA.UI.Alignment.Left"/>,<see cref="GTA.UI.Alignment.Center"/> or <see cref="GTA.UI.Alignment.Right"/>.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Font font, Alignment alignment) :
			this(caption, position, scale, color, font, alignment, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="font">Sets the <see cref="Font"/> used when drawing the text.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="GTA.UI.Alignment.Left"/>,<see cref="GTA.UI.Alignment.Center"/> or <see cref="GTA.UI.Alignment.Right"/>.</param>
		/// <param name="shadow">Sets whether or not to draw the <see cref="TextElement"/> with a <see cref="Shadow"/> effect.</param>
		/// <param name="outline">Sets whether or not to draw the <see cref="TextElement"/> with an <see cref="Outline"/> around the letters.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Font font, Alignment alignment, bool shadow, bool outline) :
			this(caption, position, scale, color, font, alignment, shadow, outline, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="TextElement"/> class used for drawing text on the screen.
		/// </summary>
		/// <param name="caption">The <see cref="TextElement"/> to draw.</param>
		/// <param name="position">Set the <see cref="Position"/> on screen where to draw the <see cref="TextElement"/>.</param>
		/// <param name="scale">Sets a <see cref="Scale"/> used to increase of decrease the size of the <see cref="TextElement"/>, for no scaling use 1.0f.</param>
		/// <param name="color">Set the <see cref="Color"/> used to draw the <see cref="TextElement"/>.</param>
		/// <param name="font">Sets the <see cref="Font"/> used when drawing the text.</param>
		/// <param name="alignment">Sets the <see cref="Alignment"/> used when drawing the text, <see cref="GTA.UI.Alignment.Left"/>,<see cref="GTA.UI.Alignment.Center"/> or <see cref="GTA.UI.Alignment.Right"/>.</param>
		/// <param name="shadow">Sets whether or not to draw the <see cref="TextElement"/> with a <see cref="Shadow"/> effect.</param>
		/// <param name="outline">Sets whether or not to draw the <see cref="TextElement"/> with an <see cref="Outline"/> around the letters.</param>
		/// <param name="wrapWidth">Sets how many horizontal pixel to draw before wrapping the <see cref="TextElement"/> on the next line down.</param>
		public TextElement(string caption, PointF position, float scale, Color color, Font font, Alignment alignment, bool shadow, bool outline, float wrapWidth)
		{
			_pinnedText = new List<IntPtr>();
			Enabled = true;
			Caption = caption;
			Position = position;
			Scale = scale;
			Color = color;
			Font = font;
			Alignment = alignment;
			Shadow = shadow;
			Outline = outline;
			WrapWidth = wrapWidth;
		}

		~TextElement()
		{
			foreach (IntPtr ptr in _pinnedText)
			{
				Marshal.FreeCoTaskMem(ptr); //free any existing allocated text
			}
			_pinnedText.Clear();
		}

		private string _caption;
		private readonly List<IntPtr> _pinnedText;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement" /> will be drawn.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if enabled; otherwise, <see langword="false" />.
		/// </value>
		public bool Enabled
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the color of this <see cref="TextElement" />.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the position of this <see cref="TextElement" />.
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
		/// Gets or sets the scale of this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The scale usually a value between ~0.5 and 3.0, Default = 1.0
		/// </value>
		public float Scale
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the font of this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The GTA Font use when drawing.
		/// </value>
		public Font Font
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the text to draw in this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The caption.
		/// </value>
		public string Caption
		{
			get => _caption;
			set
			{
				_caption = value;
				foreach (IntPtr ptr in _pinnedText)
				{
					Marshal.FreeCoTaskMem(ptr); //free any existing allocated text
				}
				_pinnedText.Clear();

				SHVDN.NativeFunc.PushLongString(value, (string str) =>
				{
					byte[] data = Encoding.UTF8.GetBytes(str + "\0");
					IntPtr next = Marshal.AllocCoTaskMem(data.Length);
					Marshal.Copy(data, 0, next, data.Length);
					_pinnedText.Add(next);
				});
			}
		}

		/// <summary>
		/// Gets or sets the alignment of this <see cref="TextElement"/>.
		/// </summary>
		/// <value>
		/// The alignment:<c>Left</c>, <c>Center</c>, <c>Right</c> Justify
		/// </value>
		public Alignment Alignment
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement"/> is drawn with a shadow effect.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if shadow; otherwise, <see langword="false" />.
		/// </value>
		public bool Shadow
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TextElement"/> is drawn with an outline.
		/// </summary>
		/// <value>
		///   <see langword="true" /> if outline; otherwise, <see langword="false" />.
		/// </value>
		public bool Outline
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets the maximum size of the <see cref="TextElement"/> before it wraps to a new line.
		/// </summary>
		/// <value>
		/// The width of the <see cref="TextElement"/>.
		/// </value>
		public float WrapWidth
		{
			get; set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether the alignment of this <see cref="TextElement" /> is centered.
		/// See <see cref="Alignment"/>
		/// </summary>
		/// <value>
		///   <see langword="true" /> if centered; otherwise, <see langword="false" />.
		/// </value>
		public bool Centered
		{
			get => Alignment == Alignment.Center;
			set
			{
				if (value)
				{
					Alignment = Alignment.Center;
				}
			}
		}

		/// <summary>
		/// Measures how many pixels in the horizontal axis this <see cref="TextElement"/> will use when drawn	against a 1280 pixel base
		/// </summary>
		public float Width
		{
			get
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, SHVDN.NativeMemory.CellEmailBcon);

				foreach (IntPtr ptr in _pinnedText)
				{
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ptr);
				}

				Function.Call(Hash.SET_TEXT_FONT, (int)Font);
				Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);

				return Screen.Width * Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, 1);
			}
		}
		/// <summary>
		/// Measures how many pixels in the horizontal axis this <see cref="TextElement"/> will use when drawn against a <see cref="ScaledWidth"/> pixel base
		/// </summary>
		public float ScaledWidth
		{
			get
			{
				Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, SHVDN.NativeMemory.CellEmailBcon);

				foreach (IntPtr ptr in _pinnedText)
				{
					Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ptr);
				}

				Function.Call(Hash.SET_TEXT_FONT, (int)Font);
				Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);

				return Screen.ScaledWidth * Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, 1);
			}
		}

		/// <summary>
		/// Measures how many lines the text string will use when drawn on screen against a <see cref="Screen.Width"/> pixel base.
		/// </summary>
		public int LineCount => CalculateLineCountInternal(Screen.Width, Screen.Height);
		/// <summary>
		/// Measures how many lines the text string will use when drawn on screen against a <see cref="Screen.ScaledWidth"/> pixel base.
		/// </summary>
		public int ScaledLineCount => CalculateLineCountInternal(Screen.ScaledWidth, Screen.Height);

		private int CalculateLineCountInternal(float screenWidth, float screenHeight)
		{
			Function.Call(Hash.SET_TEXT_FONT, (int)Font);
			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);

			float x = Position.X / screenWidth;
			float y = Position.Y / screenHeight;
			float w = WrapWidth / screenWidth;

			bool shouldSetWrapToDefault = false;

			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case Alignment.Center:
						Function.Call(Hash.SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case Alignment.Left:
						Function.Call(Hash.SET_TEXT_WRAP, x, x + w);
						break;
					case Alignment.Right:
						Function.Call(Hash.SET_TEXT_WRAP, x - w, x);
						break;
				}
				shouldSetWrapToDefault = true;
			}
			else if (Alignment == Alignment.Right)
			{
				Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
				shouldSetWrapToDefault = true;
			}
			Function.Call(Hash.SET_TEXT_JUSTIFICATION, (int)Alignment);

			Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, SHVDN.NativeMemory.CellEmailBcon);

			foreach (IntPtr ptr in _pinnedText)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ptr);
			}

			int result = Function.Call<int>(Hash.END_TEXT_COMMAND_GET_NUMBER_OF_LINES_FOR_STRING, x, y);

			if (shouldSetWrapToDefault)
			{
				// The static start x value (that 2nd argument changes) is set to 0 and the static end x value (that 2nd argument changes) is set to 1f when the exe gets loaded
				Function.Call(Hash.SET_TEXT_WRAP, 0f, 1f);
			}

			return result;
		}

		/// <summary>
		/// Measures how many pixels in the horizontal axis the string will use when drawn
		/// </summary>
		/// <param name="text">The string of text to measure.</param>
		/// <param name="font">The <see cref="GTA.UI.Font"/> of the texture to measure.</param>
		/// <param name="scale">Sets a scale value for increasing or decreasing the size of the text, default value 1.0f - no scaling.</param>
		/// <returns>
		/// The amount of pixels scaled on a 1280 pixel width base
		/// </returns>
		public static float GetStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(text);
			Function.Call(Hash.SET_TEXT_FONT, (int)font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);

			return Screen.Width * Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, 1);
		}
		/// <summary>
		/// Measures how many pixels in the horizontal axis the string will use when drawn
		/// </summary>
		/// <param name="text">The string of text to measure.</param>
		/// <param name="font">The <see cref="GTA.UI.Font"/> of the texture to measure.</param>
		/// <param name="scale">Sets a scale value for increasing or decreasing the size of the text, default value 1.0f - no scaling.</param>
		/// <returns>
		/// The amount of pixels scaled by the pixel width base return in <see cref="Screen.ScaledWidth"/>
		/// </returns>
		public static float GetScaledStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash.BEGIN_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, SHVDN.NativeMemory.CellEmailBcon);
			SHVDN.NativeFunc.PushLongString(text);
			Function.Call(Hash.SET_TEXT_FONT, (int)font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);

			return Screen.ScaledWidth * Function.Call<float>(Hash.END_TEXT_COMMAND_GET_SCREEN_WIDTH_OF_DISPLAY_TEXT, 1);
		}

		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame.
		/// </summary>
		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame at the specified offset.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement" /> using a 1280*720 pixel base.</param>
		public virtual void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}

		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="TextElement" /> this frame at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement" /> using a <see cref="Screen.ScaledWidth" />*720 pixel base.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
		}

		/// <summary>
		/// Draws this <see cref="TextElement"/> this frame in the specified <see cref="Vector3"/> position.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="TextElement"/> to be drawn</param>
		public virtual void WorldDraw(Vector3 position)
		{
			WorldDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="TextElement"/> this frame at the specified <see cref="Vector3"/> position and offset.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="TextElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement"/> using a 1280*720 pixel base.</param>
		public virtual void WorldDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.Width, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}
		/// <summary>
		/// Draws this <see cref="TextElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="TextElement"/> to be drawn</param>
		public virtual void WorldScaledDraw(Vector3 position)
		{
			WorldScaledDraw(position, SizeF.Empty);
		}
		/// <summary>
		/// Draws this <see cref="TextElement"/> this frame at the specified <see cref="Vector3"/> position and offset using the width returned in <see cref="Screen.ScaledWidth"/>.
		/// </summary>
		/// <param name="position">Position in the world where you want the <see cref="TextElement"/> to be drawn</param>
		/// <param name="offset">The offset to shift the draw position of this <see cref="TextElement"/> using a <see cref="Screen.ScaledWidth"/>*720 pixel base.</param>
		public virtual void WorldScaledDraw(Vector3 position, SizeF offset)
		{
			Function.Call(Hash.SET_DRAW_ORIGIN, position.X, position.Y, position.Z, 0);
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
			Function.Call(Hash.CLEAR_DRAW_ORIGIN);
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			if (!Enabled)
			{
				return;
			}

			float x = (Position.X + offset.Width) / screenWidth;
			float y = (Position.Y + offset.Height) / screenHeight;
			float w = WrapWidth / screenWidth;

			if (Shadow)
			{
				Function.Call(Hash.SET_TEXT_DROP_SHADOW);
			}
			if (Outline)
			{
				Function.Call(Hash.SET_TEXT_OUTLINE);
			}

			Function.Call(Hash.SET_TEXT_FONT, (int)Font);
			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);
			Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			Function.Call(Hash.SET_TEXT_JUSTIFICATION, (int)Alignment);

			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case Alignment.Center:
						Function.Call(Hash.SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case Alignment.Left:
						Function.Call(Hash.SET_TEXT_WRAP, x, x + w);
						break;
					case Alignment.Right:
						Function.Call(Hash.SET_TEXT_WRAP, x - w, x);
						break;
				}
			}
			else if (Alignment == Alignment.Right)
			{
				Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
			}

			Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, SHVDN.NativeMemory.CellEmailBcon);

			foreach (IntPtr ptr in _pinnedText)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ptr);
			}

			Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y);
		}
	}
}
