namespace Sequin.Documentation
{
    using Core.Infrastructure;
    using Microsoft.Owin;
    using Middleware;
    using Owin;

    public static class SequinDocumentationAppBuilderExtensions
    {
        public static void UseSequinDocumentation(this IAppBuilder app)
        {
            app.UseSequinDocumentation("/sequin");
        }

        public static void UseSequinDocumentation(this IAppBuilder app, string baseUrl)
        {
            var basePath = new PathString(baseUrl);
            var commandRegistry = (ICommandRegistry)app.Properties["CommandRegistry"];

            app.MapWhen(x => ShouldHandle(x, basePath), x =>
            {
                x.Use<CommandFeed>(basePath, commandRegistry);
            });
        }

        private static bool ShouldHandle(IOwinContext context, PathString basePath)
        {
            return context.Request.Path.StartsWithSegments(basePath);
        }
    }
}