namespace Sequin.Discovery
{
    using System;

    public class CommandConstructionException : Exception
    {
        public CommandConstructionException(string message, Type commandType, string body, Exception innerException) : base($"Command of type {commandType.Name} could not be constructed from request body. {FormatErrorMessage(message)}", innerException)
        {
            CommandType = commandType;
            Body = body;
        }

        public Type CommandType { get; }
        public string Body { get; }

        private static string FormatErrorMessage(string message)
        {
            return message.Replace(Environment.NewLine, " ");
        }
    }
}
