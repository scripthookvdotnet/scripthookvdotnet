//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Drawing;
using System.Collections.Generic;

namespace GTA
{
	public sealed class Viewport
	{
		public Viewport()
		{
			// Set property defaults
			MenuPosition = new Point(100, 50);
			MenuOffset = new Point(250, 0);
		}

		/** Add a menu to the stack of active menus and set it as focused */
		public void AddMenu(MenuBase newMenu)
		{
			// Reset the ease time, so the back menus will nicely ease to the side
			mEaseTime = 0.0f;
			mEaseDirection = true;
			mIsEasing = true;

			// Add it to the top of the stack
			// This should automatically put this menu in to focus, as it is the highest index in the list
			mMenuStack.Add(newMenu);
			newMenu.Parent = this;
			newMenu.Position = MenuPosition;
			newMenu.Initialize();
			newMenu.OnOpen();
		}

		/** Remove a menu from the stack of active menus */
		public void RemoveMenu(MenuBase menu)
		{
			// Reset the ease time
			mEaseTime = 0.0f;
			mEaseDirection = true;
			mIsEasing = true;

			menu.OnClose();
			mMenuStack.Remove(menu);
		}

		/** Remove the active menu from the stack, this will focus the next highest menu */
		public void PopMenu()
		{
			if (mMenuStack.Count <= 0)
			{
				return;
			}

			//Reset the ease time
			mEaseTime = 0.0f;
			mEaseDirection = false;
			mIsEasing = true;

			//Removes the highest menu
			//This should automatically give control back to the menu behind this one
			mMenuStack[mMenuStack.Count - 1].OnClose();
			mMenuStack.RemoveAt(mMenuStack.Count - 1);
		}

		/** Closes all menus */
		public void CloseAllMenus()
		{
			while (mMenuStack.Count != 0)
			{
				PopMenu();
			}
		}

		/** Draw all the active UIs */
		public void Draw()
		{
			//Menus should be drawn from lowest index to highest index
			//This way the unfocused menus will be placed behind the current focused menu
			int menuCount = mMenuStack.Count;

			if (!MenuTransitions)
			{
				if (mMenuStack.Count == 0)
				{
					return;
				}

				var offset = new Size(MenuPosition.X, MenuPosition.Y);
				mMenuStack[mMenuStack.Count - 1].Draw(offset);
				return;
			}

			//Calculate the easing
			if (mIsEasing)
			{
				if (mEaseTime < 1.0f)
				{
					mEaseTime += Game.LastFrameTime;
				}

				if (mEaseTime > 1.0f)
				{
					mEaseTime = 1.0f;
					mIsEasing = false;
				}
				float varOffsetX = EaseOut(mEaseTime, 1.0f, 0.0f, MenuOffset.X);
				float varOffsetY = EaseOut(mEaseTime, 1.0f, 0.0f, MenuOffset.Y);
				mEaseOffset = new Point((int)varOffsetX, (int)varOffsetY);
			}
			else
			{
				mEaseTime = 1.0f;
				mEaseDirection = false;
				mEaseOffset = MenuOffset;
			}

			//The last index should be drawn without offset
			//And the second-last with the offset generated from the easing function
			//All the indices before that should be drawn a full offset from each other
			//This means that we can subtract
			int i = 0;
			foreach (MenuBase menu in mMenuStack)
			{
				if (mIsEasing)
				{
					if (mEaseDirection)
					{
						float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 2) + MenuPosition.X);
						float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 2) + MenuPosition.Y);

						var offset = new Size((int)(baseOffsetX + mEaseOffset.X), (int)(baseOffsetY + mEaseOffset.Y));
						menu.Draw(offset);
					}
					else
					{
						float baseOffsetX = (float)(MenuOffset.X * (menuCount - i) + MenuPosition.X);
						float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i) + MenuPosition.Y);

						var offset = new Size((int)(baseOffsetX - mEaseOffset.X), (int)(baseOffsetY - mEaseOffset.Y));
						menu.Draw(offset);
					}
				}
				else
				{
					if (i == menuCount)
					{
						menu.Draw();
					}
					else
					{
						float baseOffsetX = (float)(MenuOffset.X * (menuCount - i - 1) + MenuPosition.X);
						float baseOffsetY = (float)(MenuOffset.Y * (menuCount - i - 1) + MenuPosition.Y);

						var offset = new Size((int)(baseOffsetX), (int)(baseOffsetY));
						menu.Draw(offset);
					}
				}
				i++;
			}
		}

		/** Handles when the activate button is pressed (e.g. numpad-5) */
		public void HandleActivate()
		{
			if (mMenuStack.Count <= 0)
			{
				return;
			}

			mMenuStack[mMenuStack.Count - 1].OnActivate();
		}

		/** Handles when the back button is pressed (e.g. numpad-0) */
		public void HandleBack()
		{
			PopMenu();
		}

		/** Handles when the user presses the up or down button (e.g. numpad-2 and 8) */
		public void HandleChangeSelection(bool down)
		{
			if (mMenuStack.Count <= 0)
			{
				return;
			}

			mMenuStack[mMenuStack.Count - 1].OnChangeSelection(down);
		}

		/** Handles when the user presses the left or right button (e.g. numpad-4 and 6) */
		public void HandleChangeItem(bool right)
		{
			if (mMenuStack.Count <= 0)
			{
				return;
			}

			mMenuStack[mMenuStack.Count - 1].OnChangeItem(right);
		}

		/** Number of active menus */
		public int ActiveMenus => mMenuStack.Count;

		/** Have more than one menu on the screen on the time and transition them */
		public bool MenuTransitions
		{
			get; set;
		}

		/** The offset each menu in the stack has from the one above it */
		public Point MenuOffset
		{
			get; set;
		}

		/** The top left position of the current menu */
		public Point MenuPosition
		{
			get; set;
		}

		//easeOutBack function
		private static float EaseOut(float time, float duration, float value0, float deltaValue)
		{
			const float s = 1.70158f;
			return deltaValue * ((time = time / duration - 1) * time * ((s + 1) * time + s) + 1) + value0;
		}

		// This is a list (or stack) of the active menus, the highest index is the one that's currently in focus
		// The reason this is a List and not a Stack is because we need to be able to access and draw the unfocused windows too
		private readonly List<MenuBase> mMenuStack = new List<MenuBase>();

		// The current time input for the ease function for the menu offset
		// 1f means that the offset is full;
		private float mEaseTime = 1.0f;
		// Are we currently easing the menu offsets?
		private bool mIsEasing = false;
		// Are we easing in (false) or out (true)
		private bool mEaseDirection = false;
		// Current ease offset
		private Point mEaseOffset;
	}
}
