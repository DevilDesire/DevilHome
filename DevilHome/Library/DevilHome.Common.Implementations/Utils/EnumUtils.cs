using System;
using System.Linq;

namespace DevilHome.Common.Implementations.Utils
{
    public static class EnumUtils
    {
        public static T Parse<T>(this string input) where T : struct
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (Enum.GetNames(typeof(T)).Any(
                      e => e.Trim().ToUpperInvariant() == input.Trim().ToUpperInvariant()))
                {
                    return (T)Enum.Parse(typeof(T), input, true);
                }
            }
            return default(T);
        }
    }
}