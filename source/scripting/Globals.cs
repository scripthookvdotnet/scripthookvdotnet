using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct Global
	{
		internal Global(IntPtr address) : this()
		{
			MemoryAddress = address;
		}

		public IntPtr MemoryAddress { get; private set; }

		public int GetInt()
		{
			return MemoryAccess.ReadInt(MemoryAddress);
		}
		public float GetFloat()
		{
			return MemoryAccess.ReadFloat(MemoryAddress);
		}
		public Vector3 GetVector3()
		{
			return MemoryAccess.ReadPaddedVector3(MemoryAddress);
		}
		public string GetString()
		{
			return MemoryAccess.ReadString(MemoryAddress);
		}
		public void SetInt(int value)
		{
			MemoryAccess.WriteInt(MemoryAddress, value);
		}
		public void SetFloat(float value)
		{
			MemoryAccess.WriteFloat(MemoryAddress, value);
		}
		public void SetVector3(Vector3 value)
		{
			MemoryAccess.WritePaddedVector3(MemoryAddress, value);
		}
		public void SetString(string value)
		{
			MemoryAccess.WriteString(MemoryAddress, value);
		}
		public Global GetArrayItem(int index, int itemSize)
		{
			int MaxIndex = GetInt();
			if (index < 0 || index >= MaxIndex)
			{
				throw new IndexOutOfRangeException(string.Format("The array index {0} was outside the bounds of the array size"));
			}
			if (itemSize <= 0)
			{
				throw new ArgumentOutOfRangeException("itemSize", "The item size for an array must be a positive number");
			}
			return new Global(MemoryAddress + 8 + (8*itemSize*index));
		}

		public Global GetStructItem(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(string.Format("The struct item index cannot be negative"));
			}
			return new Global(MemoryAddress + (8*index));
		}
	}

	public class GlobalCollection
	{
		internal GlobalCollection()
		{
		}

		public Global this[int id]
		{
			get
			{
				IntPtr memoryAddress = MemoryAccess.GetGlobalAddress(id);
				if (memoryAddress == IntPtr.Zero)
				{
					throw new IndexOutOfRangeException(
						string.Format("The global index {0} is outside the range of allowed global indexes", id));
				}
				return new Global(memoryAddress);
			}
		}
	}
}
