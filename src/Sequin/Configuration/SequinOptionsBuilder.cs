namespace Sequin.Configuration
{
    using System;
    using CommandBus;
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class SequinOptionsBuilder
    {
        private string commandPath;
        private ICommandRegistry commandRegistry;
        private IHandlerFactory handlerFactory;
        private ICommandNameResolver commandNameResolver;
        private CommandFactory commandFactory;
        private Func<CommandPipeline, CommandPipelineStage> configurePipeline;
        private CommandPipelineStage postProcessPipelineStage;

        internal SequinOptionsBuilder()
        {
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            WithCommandPath("/commands")
                .WithCommandRegistry(new ReflectionCommandRegistry(appDomainAssemblies))
                .WithHandlerFactory(new ReflectionHandlerFactory(appDomainAssemblies));
        }

        public SequinOptionsBuilder WithCommandPath(string commandPath)
        {
            Guard.EnsureNotNullOrWhitespace(commandPath, nameof(commandPath));

            this.commandPath = commandPath.StartsWith("/") ? commandPath : $"/{commandPath}";
            return this;
        }

        public SequinOptionsBuilder WithCommandRegistry(ICommandRegistry commandRegistry)
        {
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));

            this.commandRegistry = commandRegistry;
            return this;
        }

        public SequinOptionsBuilder WithHandlerFactory(IHandlerFactory handlerFactory)
        {
            Guard.EnsureNotNull(handlerFactory, nameof(handlerFactory));

            this.handlerFactory = handlerFactory;
            return this;
        }

        public SequinOptionsBuilder WithCommandNameResolver(ICommandNameResolver commandNameResolver)
        {
            Guard.EnsureNotNull(commandNameResolver, nameof(commandNameResolver));

            this.commandNameResolver = commandNameResolver;
            return this;
        }

        public SequinOptionsBuilder WithCommandFactory(CommandFactory commandFactory)
        {
            Guard.EnsureNotNull(commandFactory, nameof(commandFactory));

            this.commandFactory = commandFactory;
            return this;
        }

        public SequinOptionsBuilder WithPipeline(Func<CommandPipeline, CommandPipelineStage> configurePipeline)
        {
            this.configurePipeline = configurePipeline;
            return this;
        }

        public SequinOptionsBuilder WithPostProcessPipeline(CommandPipelineStage commandPipelineStage)
        {
            postProcessPipelineStage = commandPipelineStage;
            return this;
        }

        public SequinOptions Build()
        {
            var commandPipeline = new CommandPipeline(new ExclusiveHandlerCommandBus(handlerFactory));

            if (configurePipeline != null)
            {
                var pipelineRoot = configurePipeline(commandPipeline);
                commandPipeline.SetRoot(pipelineRoot);
            }

            commandPipeline.IssueCommand.Next = postProcessPipelineStage;

            return new SequinOptions(commandPath, commandRegistry, handlerFactory, commandNameResolver, commandFactory, commandPipeline);
        }
    }
}
