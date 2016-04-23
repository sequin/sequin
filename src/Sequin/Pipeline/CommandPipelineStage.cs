namespace Sequin.Pipeline
{
    using System.Threading.Tasks;

    public abstract class CommandPipelineStage
    {
        private bool shouldContinue = true;

        public CommandPipelineStage Next { get; set; }

        protected abstract Task Process<TCommand>(TCommand command);

        internal async Task Execute<TCommand>(TCommand command)
        {
            Guard.EnsureNotNull(command, nameof(command));

            await Process(command);

            if (Next != null && shouldContinue)
            {
                await Next.Execute(command);
            }
        }

        protected void Stop()
        {
            shouldContinue = false;
        }
    }
}
