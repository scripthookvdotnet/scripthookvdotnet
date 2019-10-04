//
// Copyright (C) 2015 crosire & contributors
// License: https://github.com/crosire/scripthookvdotnet#license
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
