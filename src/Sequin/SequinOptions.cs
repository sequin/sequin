namespace Sequin
{
    using System;
    using Discovery;
    using Extensions;
    using Infrastructure;
    using Pipeline;

    public abstract class SequinOptions
    {
        protected SequinOptions()
        {
            var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            CommandEndpointPath = "/commands";
            CommandRegistry = new ReflectionCommandRegistry(appDomainAssemblies);
            HandlerFactory = new ReflectionHandlerFactory(appDomainAssemblies);
        }

        public string CommandEndpointPath { get; set; }

        public ICommandRegistry CommandRegistry { get; set; }

        public IHandlerFactory HandlerFactory { get; set; }

        public ICommandNameResolver CommandNameResolver { get; set; }

        public CommandFactory CommandFactory { get; set; }

        public CommandPipelineStage[] CommandPipeline { get; set; }

        public CommandPipelineStage PostProcessor { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(CommandEndpointPath))
            {
                throw new SequinConfigurationException(nameof(CommandEndpointPath));
            }

            if (CommandRegistry == null)
            {
                throw new SequinConfigurationException(nameof(CommandRegistry));
            }

            if (HandlerFactory == null)
            {
                throw new SequinConfigurationException(nameof(HandlerFactory));
            }

            if (CommandNameResolver == null)
            {
                throw new SequinConfigurationException(nameof(CommandNameResolver));
            }

            if (CommandFactory == null)
            {
                throw new SequinConfigurationException(nameof(CommandFactory));
            }
        }
    }
}