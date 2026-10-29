//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System.Collections.Generic;

namespace SHVDN
{
    internal sealed class InvalidKeysFoundErrorBuilder
    {
        private readonly List<InvalidKeysFoundError.PositionAndUnparsedStrInfo> _invalidKeys = new();

        public void Add(int position, string unparsedStr)
        {
            var posAndKeyInfo = new InvalidKeysFoundError.PositionAndUnparsedStrInfo(position, unparsedStr);

            _invalidKeys.Add(posAndKeyInfo);
        }

        public InvalidKeysFoundError Build()
        {
            return new InvalidKeysFoundError(_invalidKeys.ToArray());
        }
    }
}
