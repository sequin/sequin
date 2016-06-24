namespace Sequin
{
    using System.Threading.Tasks;
    using Pipeline;

    public class Mediator
    {
        private readonly CommandPipeline pipeline;

        public Mediator(CommandPipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public Task Send<TCommand>(TCommand command)
        {
            return pipeline.Execute(command);
        }
    }
}