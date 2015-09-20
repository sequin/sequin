namespace Sequin
{
    using System;
    using Core.Infrastructure;
    using Owin;
    using Microsoft.Owin;
    using Middleware;

    public static class SequinAppBuilderExtensions
    {
        public static void UseSequin(this IAppBuilder app)
        {
            app.UseSequin(new SequinOptions());
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions options)
        {
            options.Validate();

            app.MapWhen(x => ShouldExecuteCommandPipeline(x, options.CommandEndpointPath), x =>
            {
                x.Use<DiscoverCommand>(options.CommandNameResolver, options.CommandRegistry, options.CommandFactory);

                if (options.CommandPipeline != null)
                {
                    foreach (var pipelineStage in options.CommandPipeline)
                    {
                        x.Use(pipelineStage.MiddlewareType, pipelineStage.Arguments);
                    }
                }

                x.Use<IssueCommand>(new ExclusiveHandlerCommandBus(options.HandlerFactory));
            });
        }

        private static bool ShouldExecuteCommandPipeline(IOwinContext context, string commandEndpointPath)
        {
            return context.Request.Method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(new PathString(commandEndpointPath));
        }
    }
}