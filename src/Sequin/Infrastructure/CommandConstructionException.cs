namespace Sequin.Infrastructure
{
    using System;

    internal class CommandConstructionException : Exception
    {
        public CommandConstructionException(string message, Type commandType, string body, Exception innerException) : base($"Command of type {commandType.Name} could not be constructed from request body. {message}", innerException)
        {
            CommandType = commandType;
            Body = body;
        }

        public Type CommandType { get; }

        public string Body { get; }
    }
}
