//
// Copyright (C) 2023 kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GTA.Chrono
{
    /// <summary>
    /// The exception that is thrown when an invoked method tries to return the normalized current date or date time
    /// but the internal month starting from 0 is not in the range of 0 to 11 and therefore the date cannot be
    /// semantically normalized.
    /// </summary>
    [Serializable]
    public sealed class InvalidInternalMonthOfGameClockException : Exception
    {
        public int Month0 { get; }

        public int Month => Month0 + 1;

        internal InvalidInternalMonthOfGameClockException(int month0)
            : base($"The internal month of the game clock was not in the legal range 0 to 11 (was set to {month0}).")
        {
            Month0 = month0;
        }

        private InvalidInternalMonthOfGameClockException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Month0 = info.GetInt32("Month0");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Month0", Month0, typeof(int));
            info.AddValue("Month", Month, typeof(int));
        }
    }
}
