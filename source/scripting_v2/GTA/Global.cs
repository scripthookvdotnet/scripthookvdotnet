//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Text;
using System.Runtime.InteropServices;
using GTA.Math;

namespace GTA
{
	public unsafe struct Global
	{
		private readonly IntPtr _address;

		internal Global(int index)
		{
			_address = SHVDN.NativeMemory.GetGlobalPtr(index);
		}

		public ulong* MemoryAddress => (ulong*)_address.ToPointer();

		public void SetInt(int value)
		{
			SHVDN.NativeMemory.WriteInt32(_address, value);
		}
		public void SetFloat(float value)
		{
			SHVDN.NativeMemory.WriteFloat(_address, value);
		}
		public void SetString(string value)
		{
			int size = Encoding.UTF8.GetByteCount(value);
			Marshal.Copy(Encoding.UTF8.GetBytes(value), 0, _address, size);
			*((byte*)MemoryAddress + size) = 0;
		}
		public void SetVector3(Vector3 value)
		{
			SHVDN.NativeMemory.WriteVector3(_address, value.ToInternalFVector3());
		}

		public int GetInt()
		{
			return SHVDN.NativeMemory.ReadInt32(_address);
		}
		public float GetFloat()
		{
			return SHVDN.NativeMemory.ReadFloat(_address);
		}
		public string GetString()
		{
			return SHVDN.StringMarshal.PtrToStringUtf8(_address);
		}
		public Vector3 GetVector3()
		{
			return new Vector3(SHVDN.NativeMemory.ReadVector3(_address));
		}
	}
}
