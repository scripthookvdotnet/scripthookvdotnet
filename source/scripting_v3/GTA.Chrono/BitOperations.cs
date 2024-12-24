using System.Runtime.CompilerServices;

namespace GTA.Chrono
{
    internal static class BitOperations
    {
        private static readonly byte[] Log2DeBruijn = new byte[32]
        {
            00, 09, 01, 10, 13, 21, 02, 29,
            11, 14, 16, 18, 22, 25, 03, 30,
            08, 12, 20, 28, 15, 17, 24, 07,
            19, 27, 23, 06, 26, 05, 04, 31
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Log2(uint value)
        {
            // Maybe we could use write assembly code where a `lzcnt` opcode is used and call it via a function pointer?
            // We'll need to test if the CPU supports `lzcnt` before trying to use it, of course.
            return Log2SoftwareFallback(value);
        }

        private static int Log2SoftwareFallback(uint value)
        {
            // No AggressiveInlining due to large method size.
            // Has conventional contract 0->0 (Log(0) is undefined).

            // Fill trailing zeros with ones, eg 00010010 becomes 00011111
            value |= value >> 01;
            value |= value >> 02;
            value |= value >> 04;
            value |= value >> 08;
            value |= value >> 16;

            // uint.MaxValue >> 27 is always in range [0 - 31] so there's no need to let perform bounds check, really
            return (int)Log2DeBruijn[(value * 0x07C4ACDDu) >> 27];
        }
    }
}
