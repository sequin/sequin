namespace Sequin.Infrastructure
{
    using System;

    public class CommandConstructionException : Exception
    {
        public CommandConstructionException(Type commandType, string body, Exception innerException) : base($"Command of type {commandType.Name} could not be constructed from request body.", innerException)
        {
            CommandType = commandType;
            Body = body;
        }

        public Type CommandType { get; }

        public string Body { get; }
    }
}
