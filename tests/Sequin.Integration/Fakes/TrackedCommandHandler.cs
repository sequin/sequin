namespace Sequin.Integration.Fakes
{
    using System.Threading.Tasks;

    public class TrackedCommandHandler : IHandler<TrackedCommand>
    {
        public Task Handle(TrackedCommand command)
        {
            LastCommand = command;
            return Task.FromResult(0);
        }

        public static TrackedCommand LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;

        public static void Reset()
        {
            LastCommand = null;
        }
    }
}