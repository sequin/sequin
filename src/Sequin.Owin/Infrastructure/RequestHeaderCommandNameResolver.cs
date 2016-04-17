namespace Sequin.Owin.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Infrastructure;
    using Microsoft.Owin;

    public class RequestHeaderCommandNameResolver : ICommandNameResolver
    {
        private const string CommandHeaderKey = "command";

        public string GetCommandName(IDictionary<string, object> environment)
        {
            var request = new OwinRequest(environment);
            var commandHeader = request.Headers.FirstOrDefault(x => x.Key.Equals(CommandHeaderKey, StringComparison.InvariantCultureIgnoreCase));

            if (commandHeader.Key != null)
            {
                var commandName = commandHeader.Value.Single();
                return commandName;
            }

            return null;
        }
    }
}
