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
