namespace Sequin.Discovery
{
    using System;
    using Extensions;
    using Infrastructure;

    public abstract class CommandFactory
    {
        private readonly ICommandRegistry commandRegistry;

        protected CommandFactory(ICommandRegistry commandRegistry)
        {
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));
            this.commandRegistry = commandRegistry;
        }

        public object Construct(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new UnidentifiableCommandException();
            }

            var commandType = commandRegistry.GetCommandType(commandName);
            if (commandType == null)
            {
                throw new UnknownCommandException(commandName);
            }

            var command = Create(commandType);
            if (command == null)
            {
                throw new EmptyCommandBodyException(commandType);
            }

            return command;
        }

        protected abstract object Create(Type commandType);
    }
}
