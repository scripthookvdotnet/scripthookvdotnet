//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Math;

namespace GTA
{
	public interface ISpatial
	{
		Vector3 Position
		{
			get; set;
		}
		Vector3 Rotation
		{
			get; set;
		}
	}

	public interface IHandleable
	{
		int Handle { get; }

		bool Exists();
	}
}
