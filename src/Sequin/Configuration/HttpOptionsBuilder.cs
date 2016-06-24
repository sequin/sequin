namespace Sequin.Configuration
{
    using System;
    using Discovery;

    public class HttpOptionsBuilder : OptionsBuilder, IHttpOptionsContext
    {
        private string commandPath;
        private ICommandNameResolver commandNameResolver;
        private CommandFactory commandFactory;

        private HttpOptionsBuilder(IOptionsContext context)
        {
            Guard.EnsureNotNull(context, nameof(context));

            WithCommandPath(x => "/commands")
                .WithPipeline(context.ConfigurePipeline)
                .WithPostProcessPipeline(context.PostProcessPipelineStage);

            if (context.CommandRegistry != null)
            {
                WithCommandRegistry(x => context.CommandRegistry);
            }

            if (context.HandlerFactory != null)
            {
                WithHandlerFactory(x => context.HandlerFactory);
            }
        }

        string IHttpOptionsContext.CommandPath => commandPath;
        ICommandNameResolver IHttpOptionsContext.CommandNameResolver => commandNameResolver;
        CommandFactory IHttpOptionsContext.CommandFactory => commandFactory;

        internal static HttpOptionsBuilder FromContext(IOptionsContext context)
        {
            return new HttpOptionsBuilder(context);
        }

        public HttpOptionsBuilder WithCommandPath(Func<IHttpOptionsContext, string> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            var value = resolver(this);
            Guard.EnsureNotNullOrWhitespace(value, nameof(value));

            commandPath = value.StartsWith("/") ? value : $"/{value}";

            return this;
        }

        public HttpOptionsBuilder WithCommandNameResolver(Func<IHttpOptionsContext, ICommandNameResolver> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandNameResolver = resolver(this);
            Guard.EnsureNotNull(commandNameResolver, nameof(commandNameResolver));

            return this;
        }

        public HttpOptionsBuilder WithCommandFactory(Func<IHttpOptionsContext, CommandFactory> resolver)
        {
            Guard.EnsureNotNull(resolver, nameof(resolver));

            commandFactory = resolver(this);
            Guard.EnsureNotNull(commandFactory, nameof(commandFactory));

            return this;
        }

        public override Options Build()
        {
            var commandPipeline = CreateCommandPipeline();
            var context = (IOptionsContext) this;

            return new HttpOptions(commandPath, context.CommandRegistry, context.HandlerFactory, commandNameResolver, commandFactory, commandPipeline);
        }
    }
}