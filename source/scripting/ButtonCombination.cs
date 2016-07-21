using System;
namespace GTA
{
    /// <summary>
    /// The Buttons enum used for creating <see cref="ButtonCombination"/>s
    /// </summary>
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

    /// <summary>
    /// A value type used for checking if a sequence of <see cref="Button"/>s has been pressed.
    /// Check if the combination has been entered with <see cref="Game.WasButtonCombinationJustEntered"/>.
    /// </summary>
    public struct ButtonCombination
    {
        /// <summary>
        /// Gets the calculated hash of the Sequence
        /// </summary>
        public int Hash { get; private set; }
        /// <summary>
        /// Gets the length of the sequence
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Creates a <see cref="ButtonCombination"/> from a given list of <see cref="Button"/>s.
        /// </summary>
        /// <param name="buttons">The sequence of <see cref="Button"/>s in the order a user should enter in game.</param>
        /// <remarks>There must be between 6 and 15 inclusive <see cref="Button"/>s otherwise an <see cref="ArgumentException"/> is thrown</remarks>
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
