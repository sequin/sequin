namespace Sequin.Owin
{
    using System;
    using Configuration;
    using Extensions;
    using global::Owin;
    using Microsoft.Owin;

    public static class SequinAppBuilderExtensions
    {
        public static void UseSequin(this IAppBuilder app)
        {
            app.UseSequin(SequinOptions.Configure()
                                       .WithOwinDefaults()
                                       .Build());
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions options)
        {
            app.UseRequestScopeContext();

            RegisterSequinOptionsMiddleware(options, app);

            app.Use(typeof (HandleHttpOptions));
            app.MapWhen(x => ShouldExecuteCommandPipeline(x, options.CommandPath), x =>
            {
                RegisterPipelineMiddleware(options, x);
            });
        }

        private static void RegisterSequinOptionsMiddleware(SequinOptions options, IAppBuilder app)
        {
            app.Use((ctx, next) =>
            {
                ctx.Set("CommandEndpointPath", new PathString(options.CommandPath));
                return next();
            });
        }

        private static void RegisterPipelineMiddleware(SequinOptions options, IAppBuilder app)
        {
            app.Use<DiscoverCommand>(options.CommandNameResolver, options.CommandFactory);
            app.Use<ExecuteCommandPipeline>(options.CommandPipeline);
        }

        private static bool ShouldExecuteCommandPipeline(IOwinContext context, string commandEndpointPath)
        {
            return context.Request.Method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(new PathString(commandEndpointPath));
        }
    }
}