namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using Sequin.Core;

    public class AnonymousCommandHandler : IHandler<AnonymousCommand>
    {
        public void Handle(AnonymousCommand command)
        {
            LastCommand = command;
        }

        public static AnonymousCommand LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;

        public static void Reset()
        {
            LastCommand = null;
        }
    }
}
