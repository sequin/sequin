namespace Sequin.Integration.Fakes
{
    using System.Threading.Tasks;
    using Pipeline;

    public class TrackedPipelineStage : CommandPipelineStage
    {
        public object LastCommand { get; private set; }
        public bool HasExecuted => LastCommand != null;

        protected override Task Process<TCommand>(TCommand command)
        {
            LastCommand = command;
            return Task.FromResult(0);
        }
    }
}
