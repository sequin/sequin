namespace Sequin.Infrastructure
{
    using System;

    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(string command) : base($"Command '{command}' was not recognised.")
        {
            Command = command;
        }

        public string Command { get; }
    }
}
