namespace Sample.Web.Commands
{
    using System.Threading.Tasks;
    using Sequin;

    public class DummyCommandHandler : IHandler<DummyCommand>
    {
        public Task Handle(DummyCommand command)
        {
            // Do nothing
            return Task.FromResult(0);
        }
    }
}