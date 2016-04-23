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

        public static void EnsureNotNullOrWhitespace(string value, string argumentName)
        {
            EnsureNotNull(value, argumentName);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("The string provided was empty or whitespace", argumentName);
            }
        }
    }
}