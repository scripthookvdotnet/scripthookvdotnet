using System;
using GTA.Math;
using GTA.Native;

namespace GTA
{
	public struct Global
	{
		internal Global(IntPtr address)
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
			return MemoryAccess.ReadVector3(MemoryAddress);
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
			MemoryAccess.WriteVector3(MemoryAddress, value);
		}
		public void SetString(string value)
		{
			MemoryAccess.WriteString(MemoryAddress, value);
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
				return new Global(MemoryAccess.GetGlobalAddress(id));
			}
		}
	}
}
