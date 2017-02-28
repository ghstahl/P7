using System;
using System.Diagnostics;

namespace P7.Core.Utils
{
    public static class DateTimeExtensions
    {
        [DebuggerStepThrough]
        public static bool HasExceeded(this DateTime creationTime, int seconds)
        {
            return (P7ServerDateTime.UtcNow > creationTime.AddSeconds(seconds));
        }

        [DebuggerStepThrough]
        public static int GetLifetimeInSeconds(this DateTime creationTime)
        {
            return ((int)(P7ServerDateTime.UtcNow - creationTime).TotalSeconds);
        }

        [DebuggerStepThrough]
        public static bool HasExpired(this DateTime? expirationTime)
        {
            if (expirationTime.HasValue &&
                expirationTime.Value.HasExpired())
            {
                return true;
            }

            return false;
        }

        [DebuggerStepThrough]
        public static bool HasExpired(this DateTime expirationTime)
        {
            if (expirationTime < P7ServerDateTime.UtcNow)
            {
                return true;
            }

            return false;
        }
    }
}