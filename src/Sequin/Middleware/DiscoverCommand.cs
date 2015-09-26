namespace Sequin.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Core.Infrastructure;
    using Extensions;
    using Infrastructure;

    internal class DiscoverCommand : OwinMiddleware
    {
        private readonly ICommandNameResolver commandNameResolver;
        private readonly ICommandRegistry commandRegistry;
        private readonly ICommandFactory commandFactory;

        public DiscoverCommand(OwinMiddleware next, ICommandNameResolver commandNameResolver, ICommandRegistry commandRegistry, ICommandFactory commandFactory) : base(next)
        {
            this.commandNameResolver = commandNameResolver;
            this.commandRegistry = commandRegistry;
            this.commandFactory = commandFactory;
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
                object command = null;

                try
                {
                    command = commandFactory.Create(commandType, context.Request.Body);
                    if (command == null)
                    {
                        context.Response.BadRequest("Command body was not provided");
                    }
                }
                catch (CommandConstructionException)
                {
                    context.Response.BadRequest("Command could not be constructed from request body");
                }

                return command;
            }

            return null;
        }

        private Type GetCommandType(IOwinContext context)
        {
            var commandName = commandNameResolver.GetCommandName(context.Environment);
            if (string.IsNullOrWhiteSpace(commandName))
            {
                context.Response.BadRequest("Could not identify command from request");
                return null;
            }

            var commandType = commandRegistry.GetCommandType(commandName);
            if (commandType == null)
            {
                context.Response.BadRequest($"Command '{commandName}' does not exist.");
                return null;
            }

            return commandType;
        }
    }
}