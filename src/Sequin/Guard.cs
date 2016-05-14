namespace Sequin
{
    using System;

    internal static class Guard
    {
        public static void EnsureNotNull(object value, string argumentName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}