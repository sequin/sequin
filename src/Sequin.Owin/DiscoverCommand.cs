namespace Sequin.Owin
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Owin;
    using Sequin.Infrastructure;

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

        public override async Task Invoke(IOwinContext context)
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
            catch (CommandConstructionException ex)
            {
                context.Response.BadRequest(ex.Message);
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
                context.Response.NotFound($"Command '{ex.Command}' does not exist.");
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