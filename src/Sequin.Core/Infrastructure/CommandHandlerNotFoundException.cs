namespace Sequin.Core.Infrastructure
{
    using System;

    public class CommandHandlerNotFoundException : CommandHandlerException
    {
        internal CommandHandlerNotFoundException(Type commandType) : base(commandType, "A handler for the given command could not be resolved.")
        {
            
        }
    }
}