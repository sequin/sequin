namespace Sequin.Configuration
{
    using System;
    using CommandBus;
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class SequinOptionsBuilder : ISequinOptionsContext
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

            WithCommandPath(x => "/commands")
                .WithCommandRegistry(x => new ReflectionCommandRegistry(appDomainAssemblies))
                .WithHandlerFactory(x => new ReflectionHandlerFactory(appDomainAssemblies));
        }

        string ISequinOptionsContext.CommandPath => commandPath;
        ICommandRegistry ISequinOptionsContext.CommandRegistry => commandRegistry;
        IHandlerFactory ISequinOptionsContext.HandlerFactory => handlerFactory;
        ICommandNameResolver ISequinOptionsContext.CommandNameResolver => commandNameResolver;
        CommandFactory ISequinOptionsContext.CommandFactory => commandFactory;

        public SequinOptionsBuilder WithCommandPath(Func<ISequinOptionsContext, string> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            var value = resolver(this);
            Guard.EnsureNotNullOrWhitespace(value, nameof(value));

            commandPath = value.StartsWith("/") ? value : $"/{value}";

            return this;
        }

        public SequinOptionsBuilder WithCommandRegistry(Func<ISequinOptionsContext, ICommandRegistry> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandRegistry = resolver(this);
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));

            return this;
        }

        public SequinOptionsBuilder WithHandlerFactory(Func<ISequinOptionsContext, IHandlerFactory> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            handlerFactory = resolver(this);
            Guard.EnsureNotNull(handlerFactory, nameof(handlerFactory));

            return this;
        }

        public SequinOptionsBuilder WithCommandNameResolver(Func<ISequinOptionsContext, ICommandNameResolver> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandNameResolver = resolver(this);
            Guard.EnsureNotNull(commandNameResolver, nameof(commandNameResolver));

            return this;
        }

        public SequinOptionsBuilder WithCommandFactory(Func<ISequinOptionsContext, CommandFactory> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandFactory = resolver(this);
            Guard.EnsureNotNull(commandFactory, nameof(commandFactory));

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
