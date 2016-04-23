namespace Sequin.Integration.Fakes
{
    using System;
    using System.Threading.Tasks;
    using Pipeline;

    public class ExceptionPipelineStage : CommandPipelineStage
    {
        protected override Task Process<TCommand>(TCommand command)
        {
            throw new InvalidOperationException();
        }
    }
}
