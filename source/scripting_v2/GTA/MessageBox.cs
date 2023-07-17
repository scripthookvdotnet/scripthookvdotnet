//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Drawing;

namespace GTA
{
	[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class MessageBox : MenuBase
	{
		private UIRectangle _rectNo;
		private UIRectangle _rectYes;
		private UIRectangle _rectBody;
		private UIText _text;
		private UIText _textNo;
		private UIText _textYes;
		private bool _selection = true;

		public MessageBox(string headerCaption)
		{
			HeaderTextColor = Color.White;
			HeaderFont = Font.HouseScript;
			HeaderTextScale = 0.5f;
			HeaderCentered = true;
			SelectedItemColor = Color.FromArgb(200, 255, 105, 180);
			UnselectedItemColor = Color.FromArgb(200, 176, 196, 222);
			SelectedTextColor = Color.Black;
			UnselectedTextColor = Color.DarkSlateGray;
			ItemFont = Font.ChaletLondon;
			ItemTextScale = 0.4f;
			ItemTextCentered = true;
			Caption = headerCaption;

			Width = 200;
			Height = 50;
			ButtonHeight = 30;
			OkCancel = false;
		}

		public override void Draw()
		{
			Draw(default);
		}
		public override void Draw(Size offset)
		{
			_rectBody.Draw(offset);
			_text.Draw(offset);
			_rectYes.Draw(offset);
			_rectNo.Draw(offset);
			_textYes.Draw(offset);
			_textNo.Draw(offset);
		}

		public override void Initialize()
		{
			_rectNo = new UIRectangle(
				new Point(Width / 2, Height),
				new Size(Width / 2, ButtonHeight),
				UnselectedItemColor);
			_rectYes = new UIRectangle(
				new Point(0, Height),
				new Size(Width / 2, ButtonHeight),
				UnselectedItemColor);
			_rectBody = new UIRectangle(
				default,
				new Size(Width, Height), HeaderColor);
			_text = new UIText(
				Caption,
				HeaderCentered ? new Point(Width / 2, 0) : default,
				HeaderTextScale,
				HeaderTextColor,
				HeaderFont,
				HeaderCentered);
			_textNo = new UIText(
				OkCancel ? "Cancel" : "No",
				new Point(Width / 4 * 3, Height),
				ItemTextScale,
				UnselectedTextColor,
				ItemFont,
				ItemTextCentered);
			_textYes = new UIText(
				OkCancel ? "OK" : "Yes",
				new Point(Width / 4, Height),
				ItemTextScale,
				UnselectedTextColor,
				ItemFont,
				ItemTextCentered);

			OnChangeItem(false);
		}

		public override void OnOpen()
		{
		}
		public override void OnClose()
		{
		}
		public override void OnActivate()
		{
			if (!_selection)
			{
				No(this, EventArgs.Empty);
			}
			else
			{
				Yes(this, EventArgs.Empty);
			}

			Parent.PopMenu();
		}

		public override void OnChangeItem(bool right)
		{
			_selection = !_selection;

			if (_selection)
			{
				_rectNo.Color = UnselectedItemColor;
				_rectYes.Color = SelectedItemColor;
				_textNo.Color = UnselectedTextColor;
				_textYes.Color = SelectedTextColor;
			}
			else
			{
				_rectNo.Color = SelectedItemColor;
				_rectYes.Color = UnselectedItemColor;
				_textNo.Color = SelectedTextColor;
				_textYes.Color = UnselectedTextColor;
			}
		}
		public override void OnChangeSelection(bool down)
		{
		}

		public event EventHandler<EventArgs> No;
		public event EventHandler<EventArgs> Yes;

		public int Width
		{
			get; set;
		}
		public int Height
		{
			get; set;
		}
		public int ButtonHeight
		{
			get; set;
		}

		/** Use Ok and Cancel instead of Yes and No */
		public bool OkCancel
		{
			get; set;
		}
	}
}
