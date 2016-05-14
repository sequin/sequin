namespace Sequin.Owin.Discovery
{
    using System;
    using System.Linq;
    using global::Owin;
    using Microsoft.Owin;
    using Sequin.Discovery;

    public class RequestHeaderCommandNameResolver : ICommandNameResolver
    {
        private const string CommandHeaderKey = "command";

        public string GetCommandName()
        {
            var request = new OwinRequest(OwinRequestScopeContext.Current.Environment);
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
