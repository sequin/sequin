namespace Sequin
{
    using System;
    using Core.Infrastructure;
    using Infrastructure;

    public class SequinOptions
    {
        public SequinOptions()
        {
            CommandEndpointPath = "/commands";
            CommandRegistry = new ReflectionCommandRegistry(AppDomain.CurrentDomain.GetAssemblies());
            CommandNameResolver = new RequestHeaderCommandNameResolver();
            CommandFactory = new JsonDeserializerCommandFactory();
        }

        public string CommandEndpointPath { get; set; }

        public ICommandRegistry CommandRegistry { get; set; }

        public ITypeResolver TypeResolver { get; set; }

        public ICommandNameResolver CommandNameResolver { get; set; }

        public ICommandFactory CommandFactory { get; set; }

        public CommandPipelineStage[] CommandPipeline { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrWhiteSpace(CommandEndpointPath))
            {
                throw new SequinConfigurationException(nameof(CommandEndpointPath));
            }

            if (CommandRegistry == null)
            {
                throw new SequinConfigurationException(nameof(CommandRegistry));
            }

            if (TypeResolver == null)
            {
                throw new SequinConfigurationException(nameof(TypeResolver));
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