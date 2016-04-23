namespace Sequin.Pipeline
{
    using System.Threading.Tasks;
    using CommandBus;

    public class CommandPipeline
    {
        private CommandPipelineStage rootStage;

        public CommandPipeline(ExclusiveHandlerCommandBus commandBus)
        {
            Guard.EnsureNotNull(commandBus, nameof(commandBus));

            IssueCommand = new IssueCommand(commandBus);
            rootStage = IssueCommand;
        }

        public CommandPipelineStage IssueCommand { get; }

        public void SetRoot(CommandPipelineStage root)
        {
            Guard.EnsureNotNull(root, nameof(root));
            rootStage = root;
        }

        public Task Execute<TCommand>(TCommand command)
        {
            Guard.EnsureNotNull(command, nameof(command));
            return rootStage.Execute(command);
        }
    }
}
