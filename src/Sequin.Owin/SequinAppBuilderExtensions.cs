namespace Sequin.Owin
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using Extensions;
    using global::Owin;
    using Microsoft.Owin;

    public static class SequinAppBuilderExtensions
    {
        public static void UseSequin(this IAppBuilder app)
        {
            app.UseSequin(new ResponseMiddleware[0]);
        }

        public static void UseSequin(this IAppBuilder app, IEnumerable<ResponseMiddleware> responseMiddlewares)
        {
            app.UseSequin(SequinOptions.Configure()
                                       .WithOwinDefaults()
                                       .Build(), responseMiddlewares);
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions options)
        {
            app.UseSequin(options, new ResponseMiddleware[0]);
        }

        public static void UseSequin(this IAppBuilder app, SequinOptions options, IEnumerable<ResponseMiddleware> responseMiddlewares)
        {
            app.UseRequestScopeContext();

            app.Use((ctx, next) =>
            {
                ctx.Set("CommandEndpointPath", new PathString(options.CommandPath));
                return next();
            });

            app.Use(typeof(HandleHttpOptions));
            app.MapWhen(x => ShouldExecuteCommandPipeline(x, options.CommandPath), x =>
            {
                RegisterPipelineMiddleware(options, responseMiddlewares, x);
            });
        }

        private static void RegisterPipelineMiddleware(SequinOptions options, IEnumerable<ResponseMiddleware> responseMiddlewares, IAppBuilder app)
        {
            app.Use<DiscoverCommand>(options.CommandNameResolver, options.CommandFactory);

            foreach (var middleware in responseMiddlewares)
            {
                app.Use(middleware.MiddlewareType, middleware.Arguments);
            }

            app.Use<ExecuteCommandPipeline>(options.CommandPipeline);
        }

        private static bool ShouldExecuteCommandPipeline(IOwinContext context, string commandEndpointPath)
        {
            return context.Request.Method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(new PathString(commandEndpointPath));
        }
    }
}