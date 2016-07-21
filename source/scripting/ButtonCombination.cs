using System;
namespace GTA
{
    public enum Button
    {
        PadUp = 117,
        PadDown = 100,
        PadLeft = 108,
        PadRight = 114,
        PadA = 97,
        PadB = 98,
        PadX = 120,
        PadY = 121,
        PadLB = 49,
        PadLT = 50,
        PadRB = 51,
        PadRT = 52
    }

    public struct ButtonCombination
    {
        public int Hash { get; private set; }
        public int Length { get; private set; }

        public ButtonCombination(params Button[] buttons)
        {
            if (buttons.Length < 6 || buttons.Length > 15)
            {
                throw new ArgumentException("The amount of buttons must be between 6 and 15");
            }
            uint hash = 0;
            foreach (Button button in buttons)
            {
                hash += (uint)button;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            Hash = unchecked((int)hash);

            Length = buttons.Length;
        }
    }
}
