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

        public CommandPipelineStage Root => rootStage;
        public CommandPipelineStage IssueCommand { get; }

        public void SetRoot(CommandPipelineStage root)
        {
            Guard.EnsureNotNull(root, nameof(root));
            EnsureIssueCommandInPipeline(root);

            rootStage = root;
        }

        private static void EnsureIssueCommandInPipeline(CommandPipelineStage root)
        {
            var issueCommandStage = FindStage<IssueCommand>(root);
            if (issueCommandStage == null)
            {
                throw new MissingIssueCommandStageException();
            }
        }

        private static TStage FindStage<TStage>(CommandPipelineStage stage) where TStage : CommandPipelineStage
        {
            var s = stage as TStage;
            if (s != null)
            {
                return s;
            }

            if (stage.Next != null)
            {
                return FindStage<TStage>(stage.Next);
            }

            return null;
        }

        public Task Execute<TCommand>(TCommand command)
        {
            Guard.EnsureNotNull(command, nameof(command));
            return rootStage.Execute(command);
        }
    }
}
