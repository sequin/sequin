namespace Sample.Web.Commands
{
    using System.Threading.Tasks;
    using Sequin.Core;

    public class DummyCommandHandler : IHandler<DummyCommand>
    {
        public Task Handle(DummyCommand command)
        {
            // Do nothing
            return Task.FromResult(0);
        }
    }
}