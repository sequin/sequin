namespace Sequin.Infrastructure
{
    using System;

    internal class UnknownCommandException : Exception
    {
        public UnknownCommandException(string command) : base($"Command '{command}' was not recognised.")
        {
            Command = command;
        }

        public string Command { get; }
    }
}
