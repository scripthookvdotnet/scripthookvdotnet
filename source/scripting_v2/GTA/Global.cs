using System;
using System.Text;
using System.Runtime.InteropServices;
using GTA.Math;

namespace GTA
{
	public unsafe struct Global
	{
		readonly IntPtr address;

		internal Global(int index)
		{
			address = SHVDN.NativeMemory.GetGlobalPtr(index);
		}

		public unsafe ulong* MemoryAddress => (ulong*)address.ToPointer();

		public void SetInt(int value)
		{
			SHVDN.NativeMemory.WriteInt32(address, value);
		}
		public void SetFloat(float value)
		{
			SHVDN.NativeMemory.WriteFloat(address, value);
		}
		public void SetString(string value)
		{
			int size = Encoding.UTF8.GetByteCount(value);

			Marshal.Copy(Encoding.UTF8.GetBytes(value), 0, address, size);

			unsafe { *((byte*)MemoryAddress + size) = 0; }
		}
		public void SetVector3(Vector3 value)
		{
			SHVDN.NativeMemory.WriteVector3(address, value.ToArray());
		}

		public int GetInt()
		{
			return SHVDN.NativeMemory.ReadInt32(address);
		}
		public float GetFloat()
		{
			return SHVDN.NativeMemory.ReadFloat(address);
		}
		public string GetString()
		{
			return SHVDN.NativeMemory.PtrToStringUTF8(address);
		}
		public Vector3 GetVector3()
		{
			var data = SHVDN.NativeMemory.ReadVector3(address);
			return new Vector3(data[0], data[1], data[2]);
		}
	}
}
