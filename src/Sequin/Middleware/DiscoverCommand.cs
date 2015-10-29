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
            try
            {
                var command = ConstructCommand(context);
                if (command != null)
                {
                    context.SetCommand(command);
                    await Next.Invoke(context);
                }
            }
            catch (CommandConstructionException)
            {
                context.Response.BadRequest("Command could not be constructed from request body");
            }
            catch (EmptyCommandBodyException)
            {
                context.Response.BadRequest("Command body was not provided");
            }
            catch (UnidentifiableCommandException)
            {
                context.Response.BadRequest("Could not identify command from request");
            }
            catch (UnknownCommandException ex)
            {
                context.Response.BadRequest($"Command '{ex.Command}' does not exist.");
            }
        }

        private object ConstructCommand(IOwinContext context)
        {
            var commandType = GetCommandType(context);
            var command = commandFactory.Create(commandType, context.Environment);

            if (command == null)
            {
                throw new EmptyCommandBodyException(commandType);
            }

            return command;
        }

        private Type GetCommandType(IOwinContext context)
        {
            var commandName = commandNameResolver.GetCommandName(context.Environment);
            if (string.IsNullOrWhiteSpace(commandName))
            {
                throw new UnidentifiableCommandException();
            }

            var commandType = commandRegistry.GetCommandType(commandName);
            if (commandType == null)
            {
                throw new UnknownCommandException(commandName);
            }

            return commandType;
        }
    }
}