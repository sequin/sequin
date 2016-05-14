namespace Sequin.Owin.Discovery
{
    using global::Owin;
    using Microsoft.Owin;
    using Sequin.Discovery;

    public class UrlCommandNameResolver : ICommandNameResolver
    {
        public string GetCommandName()
        {
            var environment = OwinRequestScopeContext.Current.Environment;
            var request = new OwinRequest(environment);

            var commandEndpointPath = (PathString) environment["CommandEndpointPath"];
            var requestPath = new PathString(request.Uri.LocalPath);

            PathString remainingPath;
            if (requestPath.StartsWithSegments(commandEndpointPath, out remainingPath))
            {
                var commandName = remainingPath.Value.TrimStart('/');
                return commandName;
            }

            return null;
        }
    }
}