using System;

namespace GTA
{
    public class InvalidVersionException : Exception
    {
        public InvalidVersionException()
        {
        }

        public InvalidVersionException(string msg) : base(msg)
        {
        }

        public InvalidVersionException(string msg, Exception innerException) : base(msg, innerException)
        {
        }
    }
}
