//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GTA
{
	// [Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class Menu : MenuBase
	{
		private UIRectangle _rectHeader;
		private UIRectangle _rectFooter;
		private UIText _textHeader;
		private UIText _textFooter;
		private int _selectedIndex = -1;
		private int _maxDrawLimit = 10;
		private int _startScrollOffset = 2;
		private int _scrollOffset;
		private string _footerDescription = "footer description";

		public Menu(string headerCaption, IMenuItem[] items) : this(headerCaption, items, 10)
		{
		}
		public Menu(string headerCaption, IMenuItem[] items, int MaxItemsToDraw)
		{
			// Put the items in the item stack
			// The menu itself will be initialized when it gets added to the viewport
			foreach (IMenuItem item in items)
			{
				Items.Add(item);
				item.Parent = this;
			}

			Caption = headerCaption;

			MaxDrawLimit = MaxItemsToDraw;

			// Set defaults for the properties
			HeaderFont = Font.HouseScript;
			HeaderCentered = true;
			HeaderColor = Color.FromArgb(200, 255, 20, 147);
			HeaderTextColor = Color.White;
			HeaderTextScale = 0.5f;

			FooterFont = Font.ChaletLondon;
			FooterCentered = false;
			FooterColor = Color.FromArgb(200, 255, 182, 193);
			FooterTextColor = Color.Black;
			FooterTextScale = 0.4f;

			SelectedItemColor = Color.FromArgb(200, 255, 105, 180);
			UnselectedItemColor = Color.FromArgb(200, 176, 196, 222);
			SelectedTextColor = Color.Black;
			UnselectedTextColor = Color.DarkSlateGray;

			ItemFont = Font.ChaletLondon;
			ItemTextScale = 0.4f;
			ItemTextCentered = true;

			Width = 200;
			ItemHeight = 30;
			HeaderHeight = 30;
			FooterHeight = 60;

			HasFooter = true;
		}

		public override void Draw()
		{
			Draw(new Size());
		}
		public override void Draw(Size offset)
		{
			if (_rectHeader == null || _textHeader == null || (HasFooter && (_rectFooter == null || _textFooter == null)))
			{
				return;
			}

			if (HasFooter)
			{
				_rectFooter.Draw(offset);
				_textFooter.Draw(offset);
			}

			_rectHeader.Draw(offset);
			_textHeader.Draw(offset);

			for (int i = 0; i < ItemDrawCount; i++)
			{
				Items[i + CurrentScrollOffset].Draw(offset);
			}

			DrawScrollArrows(CurrentScrollOffset > 0, CurrentScrollOffset < MaxScrollOffset, offset);
		}

		private void DrawScrollArrows(bool up, bool down, Size offset)
		{
			if (!up && !down)
			{
				return;
			}

			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "CommonMenu"))
			{
				Vector2 resolution = Function.Call<Vector2>(Hash.GET_TEXTURE_RESOLUTION, "CommonMenu", "arrowright");

				if (up)
				{
					float w = resolution.X / UI.WIDTH;
					float h = resolution.Y / UI.HEIGHT;
					float x = (float)(Width + offset.Width) / UI.WIDTH - w * 0.5f;
					float y = (float)(HeaderHeight + offset.Height + ItemHeight / 2) / UI.HEIGHT;

					Function.Call(Hash.DRAW_SPRITE, "CommonMenu", "arrowright", x, y, w, h, -90.0f, 255, 255, 255, 255);
				}
				if (down)
				{
					float w = resolution.X / UI.WIDTH;
					float h = resolution.Y / UI.HEIGHT;
					float x = (float)(Width + offset.Width) / UI.WIDTH - w * 0.5f;
					float y = (float)(HeaderHeight + offset.Height + ItemHeight * ItemDrawCount - ItemHeight / 2) / UI.HEIGHT;

					Function.Call(Hash.DRAW_SPRITE, "CommonMenu", "arrowright", x, y, w, h, 90.0f, 255, 255, 255, 255);
				}
			}
			else
			{
				Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, "CommonMenu", 0);
			}
		}

		public override void Initialize()
		{
			int currentY = HeaderHeight;
			var itemSize = new Size(Width, ItemHeight);
			for (int i = 0; i < ItemDrawCount; i++)
			{
				Items[i + CurrentScrollOffset].SetOriginAndSize(new Point(0, currentY), itemSize);
				currentY += ItemHeight;
			}

			_selectedIndex = 0;
			_footerDescription = Items[_selectedIndex].Description;
			Items[_selectedIndex].Select();

			int itemsHeight = ItemDrawCount * ItemHeight;
			_rectHeader = new UIRectangle(default, new Size(Width, HeaderHeight), HeaderColor);
			if (HasFooter)
			{
				_rectFooter = new UIRectangle(new Point(0, HeaderHeight + itemsHeight), new Size(Width, FooterHeight), FooterColor);
			}

			_textHeader = new UIText(Caption,
				HeaderCentered ? new Point(Width / 2, 0) : default,
				HeaderTextScale,
				HeaderTextColor,
				HeaderFont,
				HeaderCentered);

			if (HasFooter)
			{
				_textFooter = new UIText(_footerDescription, FooterCentered ? new Point(Width / 2, HeaderHeight + itemsHeight) : new Point(0, HeaderHeight + itemsHeight), FooterTextScale, FooterTextColor, FooterFont, FooterCentered);
			}
		}

		public override void OnOpen()
		{
		}
		public override void OnClose()
		{
		}
		public override void OnActivate()
		{
			if (_selectedIndex < 0 || _selectedIndex >= Items.Count)
			{
				return;
			}

			Items[_selectedIndex].Activate();
		}

		public void OnChangeSelection(int newIndex)
		{
			if (newIndex < CurrentScrollOffset)
			{
				CurrentScrollOffset = newIndex - _startScrollOffset - 1;
			}
			else if (newIndex > CurrentScrollOffset + ItemDrawCount)
			{
				CurrentScrollOffset = newIndex + _startScrollOffset + 1 - ItemDrawCount;
			}

			Items[_selectedIndex].Deselect();
			_selectedIndex = newIndex;
			_footerDescription = Items[_selectedIndex].Description;
			Items[_selectedIndex].Select();

			// Update footer
			if (HasFooter)
			{
				int itemsHeight = Items.Count * ItemHeight;
				_textFooter = new UIText(_footerDescription, FooterCentered ? new Point(Width / 2, HeaderHeight + itemsHeight) : new Point(0, HeaderHeight + itemsHeight), FooterTextScale, FooterTextColor, FooterFont, FooterCentered);
			}

			SelectedIndexChanged(this, new SelectedIndexChangedArgs(_selectedIndex));
		}
		public override void OnChangeItem(bool right)
		{
			if (_selectedIndex < 0 || _selectedIndex >= Items.Count)
			{
				return;
			}

			Items[_selectedIndex].Change(right);
		}
		public override void OnChangeSelection(bool down)
		{
			int newIndex = down ? _selectedIndex + 1 : _selectedIndex - 1;
			if (newIndex >= Items.Count)
			{
				newIndex = 0;
			}

			if (newIndex < 0)
			{
				newIndex = Items.Count - 1;
			}

			if (down)
			{
				if (newIndex - CurrentScrollOffset > ItemDrawCount - _startScrollOffset - 1)
				{
					CurrentScrollOffset++;
				}
			}
			else
			{
				if (newIndex - CurrentScrollOffset < _startScrollOffset)
				{
					CurrentScrollOffset--;
				}
			}

			OnChangeSelection(newIndex);
		}

		private void OnChangeDrawLimit()
		{
			if (CurrentScrollOffset > MaxScrollOffset)
			{
				CurrentScrollOffset = MaxScrollOffset;
			}
			if (SelectedIndex < CurrentScrollOffset)
			{
				CurrentScrollOffset = SelectedIndex - _startScrollOffset - 1;
			}
			else if (SelectedIndex > CurrentScrollOffset + ItemDrawCount)
			{
				CurrentScrollOffset = SelectedIndex + _startScrollOffset + 1 - ItemDrawCount;
			}

			UpdateItemPositions();

			if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
			{
				Items[SelectedIndex].Select();
			}
		}

		private void UpdateItemPositions()
		{
		}

		public int Width
		{
			get; set;
		}
		public int ItemHeight
		{
			get; set;
		}

		public int HeaderHeight
		{
			get; set;
		}
		public int FooterHeight
		{
			get; set;
		}

		public bool HasFooter
		{
			get; set;
		}

		public List<IMenuItem> Items { get; set; } = new List<IMenuItem>();

		public int MaxDrawLimit
		{
			get => _maxDrawLimit;
			set
			{
				if (value < 6 || value > 20)
				{
					throw new ArgumentOutOfRangeException(nameof(MaxDrawLimit), "MaxDrawLimit must be between 6 and 20");
				}

				_maxDrawLimit = value;
				StartScrollOffset = StartScrollOffset; // Make sure value still falls in correct range
				OnChangeDrawLimit();
			}
		}

		public int SelectedIndex
		{
			get => _selectedIndex;
			set => OnChangeSelection(value);
		}

		public int StartScrollOffset
		{
			get => _startScrollOffset;
			set
			{
				if (value < 0)
				{
					_startScrollOffset = 0;
				}
				else if (value > MaxDrawLimit / 2 - 1)
				{
					_startScrollOffset = MaxDrawLimit / 2 - 1;
				}
				else
				{
					_startScrollOffset = value;
				}
			}
		}

		public event EventHandler<SelectedIndexChangedArgs> SelectedIndexChanged;

		private int ItemDrawCount => Items.Count < _maxDrawLimit ? Items.Count : _maxDrawLimit;
		private int MaxScrollOffset => Items.Count < ItemDrawCount ? 0 : Items.Count - ItemDrawCount;

		private int CurrentScrollOffset
		{
			get => _scrollOffset;
			set
			{
				if (value > MaxScrollOffset)
				{
					_scrollOffset = MaxScrollOffset;
				}
				else if (value < 0)
				{
					_scrollOffset = 0;
				}
				else
				{
					_scrollOffset = value;
				}

				UpdateItemPositions();
			}
		}
	}
}
