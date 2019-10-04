//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using GTA.Math;
using GTA.Native;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GTA
{
	//[Obsolete("The built-in menu implementation is obsolete. Please consider using external alternatives instead.")]
	public class Menu : MenuBase
	{
		// Keep in sync with UI.WIDTH
		const float WIDTH = 1280;
		// Keep in sync with UI.HEIGHT
		const float HEIGHT = 720;

		UIRectangle rectHeader = null;
		UIRectangle rectFooter = null;
		UIText textHeader = null;
		UIText textFooter = null;
		int selectedIndex = -1;
		int maxDrawLimit = 10;
		int startScrollOffset = 2;
		int scrollOffset = 0;
		string footerDescription = "footer description";

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
			if (rectHeader == null || textHeader == null || (HasFooter && (rectFooter == null || textFooter == null)))
			{
				return;
			}

			if (HasFooter)
			{
				rectFooter.Draw(offset);
				textFooter.Draw(offset);
			}
			rectHeader.Draw(offset);
			textHeader.Draw(offset);
			for (int i = 0; i < ItemDrawCount; i++)
			{
				Items[i + CurrentScrollOffset].Draw(offset);
			}
			DrawScrollArrows(CurrentScrollOffset > 0, CurrentScrollOffset < MaxScrollOffset, offset);
		}

		void DrawScrollArrows(bool up, bool down, Size offset)
		{
			if (!up && !down)
			{
				return;
			}

			if (Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, "CommonMenu"))
			{
				Vector2 Resolution = Function.Call<Vector2>(Hash.GET_TEXTURE_RESOLUTION, "CommonMenu", "arrowright");
				if (up)
				{
					float xscale = Resolution.X / WIDTH;
					float yscale = Resolution.Y / HEIGHT;
					float xpos = (Width + offset.Width) / WIDTH - xscale * 0.5f;
					float ypos = (HeaderHeight + offset.Height + ItemHeight / 2) / HEIGHT;
					Function.Call(Hash.DRAW_SPRITE, "CommonMenu", "arrowright", xpos, ypos, xscale, yscale, -90.0f, 255, 255, 255, 255);
				}
				if (down)
				{
					float xscale = Resolution.X / WIDTH;
					float yscale = Resolution.Y / HEIGHT;
					float xpos = (Width + offset.Width) / WIDTH - xscale * 0.5f;
					float ypos = (HeaderHeight + offset.Height + ItemHeight * ItemDrawCount - ItemHeight / 2) / HEIGHT;
					Function.Call(Hash.DRAW_SPRITE, "CommonMenu", "arrowright", xpos, ypos, xscale, yscale, 90.0f, 255, 255, 255, 255);
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

			selectedIndex = 0;
			footerDescription = Items[selectedIndex].Description;
			Items[selectedIndex].Select();

			int itemsHeight = ItemDrawCount * ItemHeight;
			rectHeader = new UIRectangle(default, new Size(Width, HeaderHeight), HeaderColor);
			if (HasFooter)
			{
				rectFooter = new UIRectangle(new Point(0, HeaderHeight + itemsHeight), new Size(Width, FooterHeight), FooterColor);
			}

			textHeader = new UIText(Caption,
				HeaderCentered ? new Point(Width / 2, 0) : default,
				HeaderTextScale,
				HeaderTextColor,
				HeaderFont,
				HeaderCentered);

			if (HasFooter)
			{
				textFooter = new UIText(footerDescription, FooterCentered ? new Point(Width / 2, HeaderHeight + itemsHeight) : new Point(0, HeaderHeight + itemsHeight), FooterTextScale, FooterTextColor, FooterFont, FooterCentered);
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
			if (selectedIndex < 0 || selectedIndex >= Items.Count)
			{
				return;
			}

			Items[selectedIndex].Activate();
		}

		public void OnChangeSelection(int newIndex)
		{
			if (newIndex < CurrentScrollOffset)
			{
				CurrentScrollOffset = newIndex - startScrollOffset - 1;
			}
			else if (newIndex > CurrentScrollOffset + ItemDrawCount)
			{
				CurrentScrollOffset = newIndex + startScrollOffset + 1 - ItemDrawCount;
			}

			Items[selectedIndex].Deselect();
			selectedIndex = newIndex;
			footerDescription = Items[selectedIndex].Description;
			Items[selectedIndex].Select();

			// Update footer
			if (HasFooter)
			{
				int itemsHeight = Items.Count * ItemHeight;
				textFooter = new UIText(footerDescription, FooterCentered ? new Point(Width / 2, HeaderHeight + itemsHeight) : new Point(0, HeaderHeight + itemsHeight), FooterTextScale, FooterTextColor, FooterFont, FooterCentered);
			}

			SelectedIndexChanged(this, new SelectedIndexChangedArgs(selectedIndex));
		}
		public override void OnChangeItem(bool right)
		{
			if (selectedIndex < 0 || selectedIndex >= Items.Count)
			{
				return;
			}

			Items[selectedIndex].Change(right);
		}
		public override void OnChangeSelection(bool down)
		{
			int newIndex = down ? selectedIndex + 1 : selectedIndex - 1;
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
				if (newIndex - CurrentScrollOffset > ItemDrawCount - startScrollOffset - 1)
				{
					CurrentScrollOffset++;
				}
			}
			else
			{
				if (newIndex - CurrentScrollOffset < startScrollOffset)
				{
					CurrentScrollOffset--;
				}
			}

			OnChangeSelection(newIndex);
		}

		void OnChangeDrawLimit()
		{
			if (CurrentScrollOffset > MaxScrollOffset)
			{
				CurrentScrollOffset = MaxScrollOffset;
			}
			if (SelectedIndex < CurrentScrollOffset)
			{
				CurrentScrollOffset = SelectedIndex - startScrollOffset - 1;
			}
			else if (SelectedIndex > CurrentScrollOffset + ItemDrawCount)
			{
				CurrentScrollOffset = SelectedIndex + startScrollOffset + 1 - ItemDrawCount;
			}

			UpdateItemPositions();

			if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
			{
				Items[SelectedIndex].Select();
			}
		}

		void UpdateItemPositions()
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
			get => maxDrawLimit;
			set
			{
				if (value < 6 || value > 20)
				{
					throw new ArgumentOutOfRangeException("MaxDrawLimit", "MaxDrawLimit must be between 6 and 20");
				}

				maxDrawLimit = value;
				StartScrollOffset = StartScrollOffset; // Make sure value still falls in correct range
				OnChangeDrawLimit();
			}
		}

		public int SelectedIndex
		{
			get => selectedIndex;
			set => OnChangeSelection(value);
		}

		public int StartScrollOffset
		{
			get => startScrollOffset;
			set
			{
				if (value < 0)
				{
					startScrollOffset = 0;
				}
				else if (value > MaxDrawLimit / 2 - 1)
				{
					startScrollOffset = MaxDrawLimit / 2 - 1;
				}
				else
				{
					startScrollOffset = value;
				}
			}
		}

		public event EventHandler<SelectedIndexChangedArgs> SelectedIndexChanged;

		int ItemDrawCount => Items.Count < maxDrawLimit ? Items.Count : maxDrawLimit;
		int MaxScrollOffset => Items.Count < ItemDrawCount ? 0 : Items.Count - ItemDrawCount;
		int CurrentScrollOffset
		{
			get => scrollOffset;
			set
			{
				if (value > MaxScrollOffset)
				{
					scrollOffset = MaxScrollOffset;
				}
				else if (value < 0)
				{
					scrollOffset = 0;
				}
				else
				{
					scrollOffset = value;
				}

				UpdateItemPositions();
			}
		}
	}
}
