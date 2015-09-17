namespace Sequin.Infrastructure
{
    using System;
    using System.Linq;
    using Microsoft.Owin;

    public class RequestHeaderCommandNameResolver : ICommandNameResolver
    {
        private const string CommandHeaderKey = "command";

        public string GetCommandName(IOwinRequest request)
        {
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
