//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.InteropServices;

namespace SHVDN
{
    // This is provided to avoid unnessesary GC pressure for creating temp managed arrays when you pass methods
    // vector 3 values to method in NativeMemory that take ones.
    [StructLayout(LayoutKind.Explicit, Size = 0xC)]
    public struct FVector3
    {
        [FieldOffset(0x0)]
        public float X;
        [FieldOffset(0x4)]
        public float Y;
        [FieldOffset(0x8)]
        public float Z;

        public FVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
