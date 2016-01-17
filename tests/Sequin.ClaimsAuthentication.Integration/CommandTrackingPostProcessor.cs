namespace Sequin.ClaimsAuthentication.Integration
{
    using System.Collections.Generic;
    using Jil;
    using Microsoft.Owin;
    using Sequin.Core.Infrastructure;
    using Sequin.Extensions;

    public class CommandTrackingPostProcessor : ICommandPostProcessor
    {
        public CommandTrackingPostProcessor()
        {
            ExecutedCommands = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ExecutedCommands { get; }
         
        public void Execute(IDictionary<string, object> environment)
        {
            var context = new OwinContext(environment);
            var command = context.GetCommand();

            ExecutedCommands.Add(command.GetType().Name, JSON.Serialize(command));
        }
    }
}
