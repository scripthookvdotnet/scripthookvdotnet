//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Drawing;

namespace GTA
{
	public interface IMenuItem
	{
		/** Called when the MenuItem should be drawn */
		void Draw();

		/** Called when the MenuItem should be drawn with an offset */
		void Draw(Size offset);

		/** Called when the user selects this item */
		void Select();

		/** Called when the user deselects this item */
		void Deselect();

		/** Called when the user activates this item (e.g. numpad-5) */
		void Activate();

		/** Called when the user changes this item (e.g. numpad-4 and 6) */
		void Change(bool right);

		/** Called by the Menu to set this item's origin */
		void SetOriginAndSize(Point topLeftOrigin, Size size);

		/** Set by the parent so that the MenuItem can access its properties */
		MenuBase Parent
		{
			get; set;
		}

		string Caption
		{
			get; set;
		}

		string Description
		{
			get; set;
		}
	}
}
