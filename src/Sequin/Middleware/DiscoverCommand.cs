namespace Sequin.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Core.Infrastructure;
    using Infrastructure;

    internal class DiscoverCommand : OwinMiddleware
    {
        private readonly ICommandNameResolver _commandNameResolver;
        private readonly ICommandRegistry _commandRegistry;
        private readonly ICommandFactory _commandFactory;

        public DiscoverCommand(OwinMiddleware next, ICommandNameResolver commandNameResolver, ICommandRegistry commandRegistry, ICommandFactory commandFactory) : base(next)
        {
            _commandNameResolver = commandNameResolver;
            _commandRegistry = commandRegistry;
            _commandFactory = commandFactory;
        }

        public async override Task Invoke(IOwinContext context)
        {
            var command = ConstructCommand(context);
            if (command != null)
            {
                context.SetCommand(command);
                await Next.Invoke(context);
            }
        }

        private object ConstructCommand(IOwinContext context)
        {
            var commandType = GetCommandType(context);
            if (commandType != null)
            {
                var command = _commandFactory.Create(commandType, context.Request);
                if (command == null)
                {
                    context.Response.BadRequest("Command body was not provided");
                }

                return command;
            }

            return null;
        }

        private Type GetCommandType(IOwinContext context)
        {
            var commandName = _commandNameResolver.GetCommandName(context.Request);
            if (string.IsNullOrWhiteSpace(commandName))
            {
                context.Response.BadRequest("Could not identify command from request");
                return null;
            }

            var commandType = _commandRegistry.GetCommandType(commandName);
            if (commandType == null)
            {
                context.Response.BadRequest($"Command '{commandName}' does not exist.");
                return null;
            }

            return commandType;
        }
    }
}