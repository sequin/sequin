namespace Sequin.Owin
{
    using System;
    using CommandBus;
    using global::Owin;
    using Microsoft.Owin;

    public static class SequinAppBuilderExtensions
    {
        public static void UseSequin(this IAppBuilder app)
        {
            app.UseSequin(new SequinOptions());
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions options)
        {
            options.Validate();
            RegisterSequinOptionsMiddleware(options, app);

            app.Use(typeof (HandleHttpOptions));
            app.MapWhen(x => ShouldExecuteCommandPipeline(x, options.CommandEndpointPath), x =>
            {
                RegisterPipelineMiddleware(options, x);
            });
        }

        private static void RegisterSequinOptionsMiddleware(SequinOptions options, IAppBuilder app)
        {
            app.Use((ctx, next) =>
            {
                ctx.Set("CommandEndpointPath", new PathString(options.CommandEndpointPath));
                return next();
            });
        }

        private static void RegisterPipelineMiddleware(SequinOptions options, IAppBuilder app)
        {
            app.Use<DiscoverCommand>(options.CommandNameResolver, options.CommandRegistry, options.CommandFactory);

            if (options.CommandPipeline != null)
            {
                foreach (var pipelineStage in options.CommandPipeline)
                {
                    app.Use(pipelineStage.MiddlewareType, pipelineStage.Arguments);
                }
            }

            app.Use<IssueCommand>(new ExclusiveHandlerCommandBus(options.HandlerFactory), options.PostProcessor);
        }

        private static bool ShouldExecuteCommandPipeline(IOwinContext context, string commandEndpointPath)
        {
            return context.Request.Method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(new PathString(commandEndpointPath));
        }
    }
}