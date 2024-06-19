//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    public static class MemDataMarshal
    {
        /// <summary>
        /// Reads a single 8-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static byte ReadByte(IntPtr address)
        {
            unsafe
            {
                return *(byte*)address.ToPointer();
            }
        }
        /// <summary>
        /// Reads a single 16-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static short ReadInt16(IntPtr address)
        {
            unsafe
            {
                return *(short*)address.ToPointer();
            }
        }
        /// <summary>
        /// Reads an unsigned single 16-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static ushort ReadUInt16(IntPtr address)
        {
            unsafe
            {
                return *(ushort*)address.ToPointer();
            }
        }
        /// <summary>
        /// Reads a single 32-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static int ReadInt32(IntPtr address)
        {
            unsafe
            {
                return *(int*)address.ToPointer();
            }
        }
        /// <summary>
        /// Reads a single floating-point value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static float ReadFloat(IntPtr address)
        {
            unsafe
            {
                return *(float*)address.ToPointer();
            }
        }
        /// <summary>
        /// Reads a null-terminated UTF-8 string from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The string at the address.</returns>
        public static string ReadString(IntPtr address)
        {
            unsafe
            {
                return StringMarshal.PtrToStringUtf8(address);
            }
        }
        /// <summary>
        /// Reads a single 64-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>The value at the address.</returns>
        public static IntPtr ReadAddress(IntPtr address)
        {
            unsafe
            {
                return new IntPtr(*(void**)(address.ToPointer()));
            }
        }
        /// <summary>
        /// Reads a 4x4 floating-point matrix from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>All elements of the matrix in row major arrangement.</returns>
        public static float[] ReadMatrix(IntPtr address)
        {
            unsafe
            {
                float* data = (float*)address.ToPointer();
                return new float[16] { data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11], data[12], data[13], data[14], data[15] };
            }
        }
        /// <summary>
        /// Reads a 3-component floating-point vector from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <returns>All elements of the vector.</returns>
        public static FVector3 ReadVector3(IntPtr address)
        {
            unsafe
            {
                return *(FVector3*)address.ToPointer();
            }
        }

        public static uint CreateFirstNBitMaskUInt32(int n)
        {
            int condCheckMask = -((n != 32) ? 1 : 0);
            return (uint)(((1 << (int)((uint)n & 0x1F)) & condCheckMask) - 1);
        }
        public static uint ReadUInt32BitField(IntPtr address, int startBitIndex, int bitWidth)
        {
            uint valU32 = (uint)ReadInt32(address);
            return ((valU32 >> startBitIndex) & CreateFirstNBitMaskUInt32(bitWidth));
        }
        public static int ReadInt32BitField(IntPtr address, int startBitIndex, int bitWidth)
        {
            return SignExtendInt32((int)ReadUInt32BitField(address, startBitIndex, bitWidth), bitWidth);
        }
        public static void WriteBitFieldAsUInt32(IntPtr address, uint value, int startBitIndex, int bitWidth)
        {
            uint bitMaskToModify = CreateFirstNBitMaskUInt32(bitWidth) << startBitIndex;
            uint bitMaskToKeep = ~bitMaskToModify;

            uint valU32 = (uint)ReadInt32(address);
            valU32 = (valU32 & bitMaskToKeep) | ((value << startBitIndex) & bitMaskToModify);
            WriteInt32(address, (int)valU32);
        }
        public static void WriteBitFieldAsInt32(IntPtr address, int value, int startBitIndex, int bitWidth)
        {
            uint valuesToWrite = RemoveSignExtensionInt32(value, bitWidth);
            WriteBitFieldAsUInt32(address, (uint)value, startBitIndex, bitWidth);
        }

        /// <summary>
        /// Writes a single 8-bit value to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteByte(IntPtr address, byte value)
        {
            unsafe
            {
                byte* data = (byte*)address.ToPointer();
                *data = value;
            }
        }
        /// <summary>
        /// Writes a single 16-bit value to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteInt16(IntPtr address, short value)
        {
            unsafe
            {
                short* data = (short*)address.ToPointer();
                *data = value;
            }
        }
        /// <summary>
        /// Writes an unsigned single 16-bit value to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteUInt16(IntPtr address, ushort value)
        {
            unsafe
            {
                ushort* data = (ushort*)address.ToPointer();
                *data = value;
            }
        }
        /// <summary>
        /// Writes a single 32-bit value to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteInt32(IntPtr address, int value)
        {
            unsafe
            {
                int* data = (int*)address.ToPointer();
                *data = value;
            }
        }
        /// <summary>
        /// Writes a single floating-point value to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteFloat(IntPtr address, float value)
        {
            unsafe
            {
                float* data = (float*)address.ToPointer();
                *data = value;
            }
        }
        /// <summary>
        /// Writes a 4x4 floating-point matrix to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The elements of the matrix in row major arrangement to write.</param>
        public static void WriteMatrix(IntPtr address, float[] value)
        {
            unsafe
            {
                float* data = (float*)(address.ToPointer());
                for (int i = 0; i < value.Length; i++)
                {
                    data[i] = value[i];
                }
            }
        }
        /// <summary>
        /// Writes a 3-component floating-point to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The vector components to write.</param>
        public static void WriteVector3(IntPtr address, FVector3 value)
        {
            unsafe
            {
                *(FVector3*)address.ToPointer() = value;
            }
        }
        /// <summary>
        /// Writes a single 64-bit value from the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteAddress(IntPtr address, IntPtr value)
        {
            unsafe
            {
                long* data = (long*)address.ToPointer();
                *data = value.ToInt64();
            }
        }

        /// <summary>
        /// Sets or clears a single bit in the 32-bit value at the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="bit">The bit index to change.</param>
        /// <param name="value">Whether to set or clear the bit.</param>
        public static void SetBit(IntPtr address, int bit, bool value = true)
        {
            if (bit < 0 || bit > 31)
            {
                ThrowArgumentOutOfRangeException_InvalidBitRange(nameof(bit));
            }

            unsafe
            {
                int* data = (int*)address.ToPointer();
                if (value)
                {
                    *data |= (1 << bit);
                }
                else
                {
                    *data &= ~(1 << bit);
                }
            }
        }
        /// <summary>
        /// Checks a single bit in the 32-bit value at the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="address">The memory address to access.</param>
        /// <param name="bit">The bit index to check.</param>
        /// <returns><see langword="true" /> if the bit is set, <see langword="false" /> if it is unset.</returns>
        public static bool IsBitSet(IntPtr address, int bit)
        {
            if (bit < 0 || bit > 31)
            {
                ThrowArgumentOutOfRangeException_InvalidBitRange(nameof(bit));
                return false;
            }

            unsafe
            {
                int* data = (int*)address.ToPointer();
                return (*data & (1 << bit)) != 0;
            }
        }

        /// <summary>
        /// Sign extends the value to a 32-bit integer so <paramref name="value"/> will be read as the intended value.
        /// </summary>
        /// <param name="value">The value to interpret as the intended value.</param>
        /// <param name="bitWidth">The bit width.</param>
        /// <returns>The sign-extended value.</returns>
        public static int SignExtendInt32(int value, int bitWidth)
        {
            const int MaxI32BitWidth = 32;
            // Right shift must be arithmetic shift to calculate the correct result
            return ((value << (MaxI32BitWidth - bitWidth)) >> (MaxI32BitWidth - bitWidth));
        }
        public static uint RemoveSignExtensionInt32(int value, int bitWidth)
        {
            return ((uint)value & CreateFirstNBitMaskUInt32(bitWidth));
        }

        private static void ThrowArgumentOutOfRangeException_InvalidBitRange(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName, "The bit index has to be between 0 and 31");
        }
    }
}
