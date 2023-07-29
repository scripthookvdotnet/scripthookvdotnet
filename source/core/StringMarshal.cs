//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace SHVDN
{
	public static class StringMarshal
	{
		private static byte[] s_strBufferForStringToCoTaskMemUtf8 = new byte[100];

		public static string PtrToStringUtf8(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return string.Empty;
			}

			unsafe
			{
				byte* data = (byte*)ptr.ToPointer();

				// Calculate length of null-terminated string
				int len = 0;
				while (data[len] != 0)
				{
					++len;
				}

				return PtrToStringUtf8(ptr, len);
			}
		}
		public static string PtrToStringUtf8(IntPtr ptr, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException(null, nameof(len));
			}

			if (ptr == IntPtr.Zero)
			{
				return null;
			}

			if (len == 0)
			{
				return string.Empty;
			}

			unsafe
			{
				return Encoding.UTF8.GetString((byte*)ptr.ToPointer(), len);
			}
		}
		public static IntPtr StringToCoTaskMemUtf8(string s)
		{
			if (s == null)
			{
				return IntPtr.Zero;
			}

			unsafe
			{
				int byteCountUtf8 = Encoding.UTF8.GetByteCount(s);
				if (byteCountUtf8 > s_strBufferForStringToCoTaskMemUtf8.Length)
				{
					s_strBufferForStringToCoTaskMemUtf8 = new byte[byteCountUtf8 * 2];
				}

				Encoding.UTF8.GetBytes(s, 0, s.Length, s_strBufferForStringToCoTaskMemUtf8, 0);
				IntPtr dest = Marshal.AllocCoTaskMem(byteCountUtf8 + 1);
				if (dest == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}

				Marshal.Copy(s_strBufferForStringToCoTaskMemUtf8, 0, dest, byteCountUtf8);
				// Add null-terminator to end
				((byte*)dest.ToPointer())[byteCountUtf8] = 0;

				return dest;
			}
		}
	}
}
