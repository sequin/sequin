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
