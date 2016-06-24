namespace Sequin.Configuration
{
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class Options
    {
        public static OptionsBuilder Configure()
        {
            return new OptionsBuilder();
        }

        internal Options(ICommandRegistry commandRegistry, IHandlerFactory handlerFactory, CommandPipeline commandPipeline)
        {
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));
            Guard.EnsureNotNull(handlerFactory, nameof(handlerFactory));
            Guard.EnsureNotNull(commandPipeline, nameof(commandPipeline));

            CommandRegistry = commandRegistry;
            HandlerFactory = handlerFactory;
            CommandPipeline = commandPipeline;
        }

        public ICommandRegistry CommandRegistry { get; }
        public IHandlerFactory HandlerFactory { get; }
        public CommandPipeline CommandPipeline { get; }
    }
}
