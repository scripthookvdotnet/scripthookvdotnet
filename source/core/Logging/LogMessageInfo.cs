//
// Copyright (C) 2026 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

namespace SHVDN
{
    internal struct LogMessageInfo
    {
        public LogMessageInfo(Log.Level level, string message)
        {
            Level = level;
            Message = message;
        }

        public Log.Level Level { get; set; }
        public string Message { get; set; }
    }
}
