namespace Sequin.Owin.Sample.Commands
{
  using System.Threading.Tasks;

  public class DummyCommandHandler : IHandler<DummyCommand>
    {
        public Task Handle(DummyCommand command)
        {
            // Do nothing
            return Task.FromResult(0);
        }
    }
}