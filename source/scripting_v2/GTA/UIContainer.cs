//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
//

using System.Collections.Generic;
using System.Drawing;

namespace GTA
{
	public class UIContainer : UIRectangle
	{
		public UIContainer() : base()
		{
		}
		public UIContainer(Point position, Size size) : base(position, size)
		{
		}
		public UIContainer(Point position, Size size, Color color) : base(position, size, color)
		{
		}

		public List<UIElement> Items { get; set; } = new List<UIElement>();

		public override void Draw()
		{
			Draw(new Size());
		}
		public override void Draw(Size offset)
		{
			if (!Enabled)
			{
				return;
			}

			base.Draw(offset);

			foreach (UIElement item in Items)
			{
				item.Draw(new Size(base.Position + offset));
			}
		}
	}
}
