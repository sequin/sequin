namespace Sequin.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Infrastructure;
    using Microsoft.Owin;

    public class UrlCommandNameResolver : ICommandNameResolver
    {
        public string GetCommandName(IDictionary<string, object> environment)
        {
            var request = new OwinRequest(environment);
            var commandName = request.Uri.Segments.LastOrDefault();

            return commandName;
        }
    }
}