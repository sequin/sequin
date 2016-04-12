namespace Sequin.Core.Infrastructure
{
    using System;

    public abstract class CommandHandlerException : Exception
    {
        protected CommandHandlerException(Type commandType, string message) : base(message)
        {
            CommandType = commandType;
        }

        public Type CommandType { get; }
    }
}