//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace GTA
{
	public class GlobalCollection
	{
		internal GlobalCollection()
		{
		}

		public Global this[int index]
		{
			get => new Global(index);
			set
			{
				unsafe
				{
					*(ulong*)SHVDN.NativeMemory.GetGlobalPtr(index).ToPointer() = *value.MemoryAddress;
				}
			}
		}
	}
}
