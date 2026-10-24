//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace SHVDN
{
    internal static class LockHelper
    {
        public static T ReadLocked<T>(ref T field, ref object variableLock)
        {
            lock (variableLock)
            {
                return field;
            }
        }
        public static T WriteLocked<T>(ref T field, T value, ref object variableLock)
        {
            lock (variableLock)
            {
                return field = value;
            }
        }
    }
}
