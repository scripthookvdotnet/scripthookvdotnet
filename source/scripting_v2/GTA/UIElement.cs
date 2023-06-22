//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Drawing;

namespace GTA
{
	public interface UIElement
	{
		void Draw();
		void Draw(Size offset);

		bool Enabled
		{
			get; set;
		}

		Point Position
		{
			get; set;
		}

		Color Color
		{
			get; set;
		}
	}
}
