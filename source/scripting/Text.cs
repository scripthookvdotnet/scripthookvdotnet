using System;
using System.Drawing;
using GTA.Native;

namespace GTA.UI
{
	public enum TextAlignment
	{
		Center = 0,
		Left = 1,
		Right = 2,
	}

	public class Text : IElement
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IElement" /> will be drawn.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled { get; set; }
		/// <summary>
		/// Gets or sets the color of this <see cref="IElement" />.
		/// </summary>
		/// <value>
		/// The color.
		/// </value>
		public Color Color { get; set; }
		/// <summary>
		/// Gets or sets the position of this <see cref="IElement" />.
		/// </summary>
		/// <value>
		/// The position scaled on a 1280*720 pixel base.
		/// </value>
		/// <remarks>
		/// If ScaledDraw is called, the position will be scaled by the width returned in <see cref="Screen.ScaledWidth" />.
		/// </remarks>
		public PointF Position { get; set; }
		/// <summary>
		/// Gets or sets the scale of this <see cref="Text"/>.
		/// </summary>
		/// <value>
		/// The scale usually a value between ~0.5 and 3.0, Default = 1.0
		/// </value>
		public float Scale { get; set; }
		/// <summary>
		/// Gets or sets the font of this <see cref="Text"/>.
		/// </summary>
		/// <value>
		/// The GTA Font use when drawing.
		/// </value>
		public Font Font { get; set; }
		/// <summary>
		/// Gets or sets the text to draw in this <see cref="Text"/>.
		/// </summary>
		/// <value>
		/// The caption.
		/// </value>
		public string Caption { get; set; }
		/// <summary>
		/// Gets or sets the alignment of this <see cref="Text"/>.
		/// </summary>
		/// <value>
		/// The alignment:<c>Left</c>, <c>Center</c>, <c>Right</c> Justify
		/// </value>
		public TextAlignment Alignment { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Text"/> is drawn with a shadow effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if shadow; otherwise, <c>false</c>.
		/// </value>
		public bool Shadow { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Text"/> is drawn with an outline.
		/// </summary>
		/// <value>
		///   <c>true</c> if outline; otherwise, <c>false</c>.
		/// </value>
		public bool Outline { get; set; }
		/// <summary>
		/// Gets or sets the maximun size of the <see cref="Text"/> before it wraps to a new line.
		/// </summary>
		/// <value>
		/// The width of the <see cref="Text"/>.
		/// </value>
		public float WrapWidth { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Text" /> is centered.
		/// See <see cref="Alignment"/>
		/// </summary>
		/// <value>
		///   <c>true</c> if centered; otherwise, <c>false</c>.
		/// </value>
		public bool Centered
		{
			get
			{
				return Alignment == TextAlignment.Center;
			}
			set
			{
				if (value)
				{
					Alignment = TextAlignment.Center;
				}
			}
		}
		/// <summary>
		/// Measures how many pixels in the horizontal axis this <see cref="Text"/> will use when drawn
		/// </summary>
		public float Width
		{
			get
			{
				return GetStringWidth(Caption, Font, Scale);
			}
		}
		/// <summary>
		/// Measures how many pixels in the horizontal axis this <see cref="Text"/> will use when drawn scaled by <see cref="ScaledWidth"/>
		/// </summary>
		public float ScaledWidth
		{
			get
			{
				return GetScaledStringWidth(Caption, Font, Scale);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		public Text(string caption, PointF position, float scale) : this(caption, position, scale, Color.WhiteSmoke, Font.ChaletLondon, TextAlignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		public Text(string caption, PointF position, float scale, Color color) : this(caption, position, scale, color, Font.ChaletLondon, TextAlignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		/// <param name="font">The font.</param>
		public Text(string caption, PointF position, float scale, Color color, Font font) : this(caption, position, scale, color, font, TextAlignment.Left, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		/// <param name="font">The font.</param>
		/// <param name="alignment">The alignment.</param>
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment) : this(caption, position, scale, color, font, alignment, false, false, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		/// <param name="font">The font.</param>
		/// <param name="alignment">The alignment.</param>
		/// <param name="shadow">if set to <c>true</c> [shadow].</param>
		/// <param name="outline">if set to <c>true</c> [outline].</param>
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment, bool shadow, bool outline) : this(caption, position, scale, color, font, alignment, shadow, outline, 0.0f)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Text"/> class.
		/// </summary>
		/// <param name="caption">The caption.</param>
		/// <param name="position">The position.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="color">The color.</param>
		/// <param name="font">The font.</param>
		/// <param name="alignment">The alignment.</param>
		/// <param name="shadow">if set to <c>true</c> [shadow].</param>
		/// <param name="outline">if set to <c>true</c> [outline].</param>
		/// <param name="wrapWidth">Width of the wrap.</param>
		public Text(string caption, PointF position, float scale, Color color, Font font, TextAlignment alignment, bool shadow, bool outline, float wrapWidth)
		{
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

		/// <summary>
		/// Measures how many pixels in the horizontal axis the string will use when drawn
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="scale">The scale.</param>
		/// <returns>
		/// The amount of pixels scaled on a 1280 pixel width base
		/// </returns>
		public static float GetStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_FOR_WIDTH, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < text.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, System.Math.Min(maxStringLength, text.Length - i)));
			}

			Function.Call(Hash.SET_TEXT_FONT, font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);

			return Screen.Width * Function.Call<float>(Hash._GET_TEXT_SCREEN_WIDTH, 1);
		}
		/// <summary>
		/// Measures how many pixels in the horizontal axis the string will use when drawn
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="scale">The scale.</param>
		/// <returns>
		/// The amount of pixels scaled by the pixel width base return in <see cref="Screen.ScaledWidth"/>
		/// </returns>
		public static float GetScaledStringWidth(string text, Font font = Font.ChaletLondon, float scale = 1.0f)
		{
			Function.Call(Hash._SET_TEXT_ENTRY_FOR_WIDTH, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < text.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text.Substring(i, System.Math.Min(maxStringLength, text.Length - i)));
			}

			Function.Call(Hash.SET_TEXT_FONT, font);
			Function.Call(Hash.SET_TEXT_SCALE, scale, scale);

			return Screen.ScaledWidth * Function.Call<float>(Hash._GET_TEXT_SCREEN_WIDTH, 1);
		}

		/// <summary>
		/// Draws this <see cref="Text" />.
		/// </summary>
		public virtual void Draw()
		{
			Draw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="Text" /> at the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public virtual void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}
		/// <summary>
		/// Draws this <see cref="Text" /> using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		public virtual void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		/// <summary>
		/// Draws the <see cref="Text" /> at the specified offset using the width returned in <see cref="Screen.ScaledWidth" />.
		/// </summary>
		/// <param name="offset">The offset.</param>
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
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

			Function.Call(Hash.SET_TEXT_FONT, Font);
			Function.Call(Hash.SET_TEXT_SCALE, Scale, Scale);
			Function.Call(Hash.SET_TEXT_COLOUR, Color.R, Color.G, Color.B, Color.A);
			Function.Call(Hash.SET_TEXT_JUSTIFICATION, Alignment);

			if (WrapWidth > 0.0f)
			{
				switch (Alignment)
				{
					case TextAlignment.Center:
						Function.Call(Hash.SET_TEXT_WRAP, x - (w / 2), x + (w / 2));
						break;
					case TextAlignment.Left:
						Function.Call(Hash.SET_TEXT_WRAP, x, x + w);
						break;
					case TextAlignment.Right:
						Function.Call(Hash.SET_TEXT_WRAP, x - w, x);
						break;
				}
			}
			else if (Alignment == TextAlignment.Right)
			{
				Function.Call(Hash.SET_TEXT_WRAP, 0.0f, x);
			}

			Function.Call(Hash._SET_TEXT_ENTRY, "CELL_EMAIL_BCON");

			const int maxStringLength = 99;

			for (int i = 0; i < Caption.Length; i += maxStringLength)
			{
				Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, Caption.Substring(i, System.Math.Min(maxStringLength, Caption.Length - i)));
			}

			Function.Call(Hash._DRAW_TEXT, x, y);
		}
	}
}
