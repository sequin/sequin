namespace Sequin.Owin
{
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Owin;
    using Sequin.Discovery;

    internal class DiscoverCommand : OwinMiddleware
    {
        private readonly ICommandNameResolver commandNameResolver;
        private readonly CommandFactory commandFactory;

        public DiscoverCommand(OwinMiddleware next, ICommandNameResolver commandNameResolver, CommandFactory commandFactory) : base(next)
        {
            this.commandNameResolver = commandNameResolver;
            this.commandFactory = commandFactory;
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                var commandName = commandNameResolver.GetCommandName();
                var command = commandFactory.Construct(commandName);

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
    }
}