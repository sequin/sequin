namespace Sequin.Configuration
{
    using System;
    using CommandBus;
    using Discovery;
    using Infrastructure;
    using Pipeline;

    public class OptionsBuilder : IOptionsContext
    {
        private ICommandRegistry commandRegistry;
        private IHandlerFactory handlerFactory;
        private Func<CommandPipeline, CommandPipelineStage> configurePipeline;
        private CommandPipelineStage postProcessPipelineStage;

        internal OptionsBuilder()
        {
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            WithCommandRegistry(x => new ReflectionCommandRegistry(appDomainAssemblies))
                .WithHandlerFactory(x => new ReflectionHandlerFactory(appDomainAssemblies));
        }

        ICommandRegistry IOptionsContext.CommandRegistry => commandRegistry;
        IHandlerFactory IOptionsContext.HandlerFactory => handlerFactory;
        Func<CommandPipeline, CommandPipelineStage> IOptionsContext.ConfigurePipeline => configurePipeline;
        CommandPipelineStage IOptionsContext.PostProcessPipelineStage => postProcessPipelineStage;

        public HttpOptionsBuilder ForHttp()
        {
            return HttpOptionsBuilder.FromContext(this);
        }

        public OptionsBuilder WithCommandRegistry(Func<IOptionsContext, ICommandRegistry> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandRegistry = resolver(this);
            Guard.EnsureNotNull(commandRegistry, nameof(commandRegistry));

            return this;
        }

        public OptionsBuilder WithHandlerFactory(Func<IOptionsContext, IHandlerFactory> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            handlerFactory = resolver(this);
            Guard.EnsureNotNull(handlerFactory, nameof(handlerFactory));

            return this;
        }

        public OptionsBuilder WithPipeline(Func<CommandPipeline, CommandPipelineStage> configure)
        {
            configurePipeline = configure;
            return this;
        }

        public OptionsBuilder WithPostProcessPipeline(CommandPipelineStage commandPipelineStage)
        {
            postProcessPipelineStage = commandPipelineStage;
            return this;
        }

        public virtual Options Build()
        {
            var commandPipeline = CreateCommandPipeline();
            return new Options(commandRegistry, handlerFactory, commandPipeline);
        }

        protected CommandPipeline CreateCommandPipeline()
        {
            var commandPipeline = new CommandPipeline(new ExclusiveHandlerCommandBus(handlerFactory));

            if (configurePipeline != null)
            {
                var pipelineRoot = configurePipeline(commandPipeline);
                commandPipeline.SetRoot(pipelineRoot);
            }

            commandPipeline.IssueCommand.Next = postProcessPipelineStage;
            return commandPipeline;
        }
    }
}
