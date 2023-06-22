//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Drawing;

namespace GTA
{
	public class MenuBase
	{
		/** Draws the menu */
		public virtual void Draw()
		{
		}

		// Drawing with an offset implies that this menu is not the active one
		/** Draws the menu with an offset */
		public virtual void Draw(Size offset)
		{
		}

		/** Called when the menu is first added to the Viewport */
		public virtual void Initialize()
		{
		}

		/** Called when the menu gains or regains focus */
		public virtual void OnOpen()
		{
		}

		/** Called when the user hits the back button or unfocuses from this menu */
		public virtual void OnClose()
		{
		}

		/** Called when the user hits the activate button */
		public virtual void OnActivate()
		{
		}

		/** Called when the user changes the current element (i.e. left and right) */
		public virtual void OnChangeItem(bool right)
		{
		}

		/** Called when the user changes what element is selected (i.e. up and down) */
		public virtual void OnChangeSelection(bool down)
		{
		}

		public Viewport Parent
		{
			get; set;
		}

		public Font HeaderFont
		{
			get; set;
		}
		public bool HeaderCentered
		{
			get; set;
		}
		public Color HeaderColor
		{
			get; set;
		}
		public Color HeaderTextColor
		{
			get; set;
		}
		public float HeaderTextScale
		{
			get; set;
		}

		public Font FooterFont
		{
			get; set;
		}
		public bool FooterCentered
		{
			get; set;
		}
		public Color FooterColor
		{
			get; set;
		}
		public Color FooterTextColor
		{
			get; set;
		}
		public float FooterTextScale
		{
			get; set;
		}

		public Color SelectedItemColor
		{
			get; set;
		}
		public Color UnselectedItemColor
		{
			get; set;
		}
		public Color SelectedTextColor
		{
			get; set;
		}
		public Color UnselectedTextColor
		{
			get; set;
		}

		public Font ItemFont
		{
			get; set;
		}
		public bool ItemTextCentered
		{
			get; set;
		}
		public float ItemTextScale
		{
			get; set;
		}

		public Point Position
		{
			get; set;
		}

		public Point TextOffset
		{
			get; set;
		}

		public string Caption
		{
			get; set;
		}
	}
}
