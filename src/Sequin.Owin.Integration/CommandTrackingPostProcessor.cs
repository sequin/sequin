namespace Sequin.Owin.Integration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Jil;
    using Pipeline;

    public class CommandTrackingPostProcessor : CommandPipelineStage
    {
        public CommandTrackingPostProcessor()
        {
            ExecutedCommands = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ExecutedCommands { get; }

        protected override Task Process<TCommand>(TCommand command)
        {
            ExecutedCommands.Add(command.GetType().Name, JSON.Serialize(command));

            return Task.FromResult(0);
        }
    }
}
