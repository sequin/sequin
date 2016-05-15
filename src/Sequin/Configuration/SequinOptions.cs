namespace Sequin.Configuration
{
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class SequinOptions
    {
        public static SequinOptionsBuilder Configure()
        {
            return new SequinOptionsBuilder();
        }

        internal SequinOptions(string commandPath, ICommandRegistry commandRegistry, IHandlerFactory handlerFactory, ICommandNameResolver commandNameResolver, CommandFactory commandFactory, CommandPipeline commandPipeline)
        {
            Guard.EnsureNotNullOrWhitespace(commandPath, nameof(commandPath));
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));
            Guard.EnsureNotNull(handlerFactory, nameof(handlerFactory));
            Guard.EnsureNotNull(commandNameResolver, nameof(commandNameResolver));
            Guard.EnsureNotNull(commandFactory, nameof(commandFactory));
            Guard.EnsureNotNull(commandPipeline, nameof(commandPipeline));

            CommandPath = commandPath;
            CommandRegistry = commandRegistry;
            HandlerFactory = handlerFactory;
            CommandNameResolver = commandNameResolver;
            CommandFactory = commandFactory;
            CommandPipeline = commandPipeline;
        }

        public string CommandPath { get; }
        public ICommandRegistry CommandRegistry { get; }
        public IHandlerFactory HandlerFactory { get; }
        public ICommandNameResolver CommandNameResolver { get; }
        public CommandFactory CommandFactory { get; }
        public CommandPipeline CommandPipeline { get; }
    }
}
