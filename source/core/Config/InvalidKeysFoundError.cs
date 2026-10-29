//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace SHVDN
{
    internal class InvalidKeysFoundError
    {
        public class PositionAndUnparsedStrInfo
        {
            public PositionAndUnparsedStrInfo(int position, string unparsedStr)
            {
                Position = position;
                UnparsedString = unparsedStr;
            }

            public int Position { get; }
            public string UnparsedString { get; }
        }

        private readonly PositionAndUnparsedStrInfo[] _unparsedStrInfo;

        public InvalidKeysFoundError(PositionAndUnparsedStrInfo[] input)
        {
            _unparsedStrInfo = new PositionAndUnparsedStrInfo[input.Length];

            input.CopyTo(_unparsedStrInfo, 0);
        }

        public PositionAndUnparsedStrInfo[] GetUnparsedStrSequence()
        {
            var newArray = new PositionAndUnparsedStrInfo[_unparsedStrInfo.Length];
            _unparsedStrInfo.CopyTo(newArray, 0);

            return newArray;
        }
    }
}
