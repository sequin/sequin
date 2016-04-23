namespace Sequin.Owin.Infrastructure
{
    using System.Collections.Generic;
    using Microsoft.Owin;
    using Sequin.Infrastructure;

    public class UrlCommandNameResolver : ICommandNameResolver
    {
        public string GetCommandName(IDictionary<string, object> environment)
        {
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