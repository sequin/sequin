namespace Sequin.Integration.Fakes
{
    using Core;

    public class TrackedCommandHandler : IHandler<TrackedCommand>
    {
        public void Handle(TrackedCommand command)
        {
            LastCommand = command;
        }

        public static TrackedCommand LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;
    }
}