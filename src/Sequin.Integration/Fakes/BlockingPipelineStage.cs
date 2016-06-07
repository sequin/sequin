namespace Sequin.Integration.Fakes
{
    using System.Threading.Tasks;
    using Pipeline;

    public class BlockingPipelineStage : CommandPipelineStage
    {
        public bool HasExecuted { get; private set; }

        protected override Task Process<TCommand>(TCommand command)
        {
            HasExecuted = true;
            Stop();

            return Task.FromResult(0);
        }
    }
}
