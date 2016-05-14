namespace Sequin.Owin
{
    using System;
    using System.Linq;
    using CommandBus;
    using global::Owin;
    using Microsoft.Owin;
    using Pipeline;

    public static class SequinAppBuilderExtensions
    {
        public static void UseSequin(this IAppBuilder app)
        {
            app.UseSequin(new OwinSequinOptions());
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions_Old options)
        {
            options.Validate();

            app.UseRequestScopeContext();

            RegisterSequinOptionsMiddleware(options, app);

            app.Use(typeof (HandleHttpOptions));
            app.MapWhen(x => ShouldExecuteCommandPipeline(x, options.CommandEndpointPath), x =>
            {
                RegisterPipelineMiddleware(options, x);
            });
        }

        private static void RegisterSequinOptionsMiddleware(SequinOptions_Old options, IAppBuilder app)
        {
            app.Use((ctx, next) =>
            {
                ctx.Set("CommandEndpointPath", new PathString(options.CommandEndpointPath));
                return next();
            });
        }

        private static void RegisterPipelineMiddleware(SequinOptions_Old options, IAppBuilder app)
        {
            app.Use<DiscoverCommand>(options.CommandNameResolver, options.CommandFactory);

            var pipeline = new CommandPipeline(new ExclusiveHandlerCommandBus(options.HandlerFactory));

            if (options.CommandPipeline != null && options.CommandPipeline.Any())
            {
                pipeline.SetRoot(options.CommandPipeline.First());

                for (var i = 0; i < options.CommandPipeline.Length; i++)
                {
                    var isLast = i == options.CommandPipeline.Length - 1;
                    options.CommandPipeline[i].Next = isLast ? pipeline.IssueCommand : options.CommandPipeline[i + 1];
                }
            }

            if (options.PostProcessor != null)
            {
                pipeline.IssueCommand.Next = options.PostProcessor;
            }

            app.Use<ExecuteCommandPipeline>(pipeline);
        }

        private static bool ShouldExecuteCommandPipeline(IOwinContext context, string commandEndpointPath)
        {
            return context.Request.Method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(new PathString(commandEndpointPath));
        }
    }
}