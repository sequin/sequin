namespace Sequin.Pipeline
{
    using System.Threading.Tasks;
    using CommandBus;

    public class IssueCommand : CommandPipelineStage
    {
        private readonly ExclusiveHandlerCommandBus commandBus;

        internal IssueCommand(ExclusiveHandlerCommandBus commandBus)
        {
            this.commandBus = commandBus;
        }

        protected override Task Process<TCommand>(TCommand command)
        {
            return commandBus.Issue(command);
        }
    }
}
