namespace Sequin.Configuration
{
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class HttpOptions : Options
    {
        internal HttpOptions(string commandPath, ICommandRegistry commandRegistry, IHandlerFactory handlerFactory, ICommandNameResolver commandNameResolver, CommandFactory commandFactory, CommandPipeline commandPipeline) : base(commandRegistry, handlerFactory, commandPipeline)
        {
            Guard.EnsureNotNullOrWhitespace(commandPath, nameof(commandPath));
            Guard.EnsureNotNull(commandNameResolver, nameof(commandNameResolver));
            Guard.EnsureNotNull(commandFactory, nameof(commandFactory));

            CommandPath = commandPath;
            CommandNameResolver = commandNameResolver;
            CommandFactory = commandFactory;
        }

        public string CommandPath { get; }
        public ICommandNameResolver CommandNameResolver { get; }
        public CommandFactory CommandFactory { get; }
    }
}
