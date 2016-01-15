namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using Sequin.Core;

    public class AuthenticatedCommandHandler : IHandler<AuthenticatedCommand>
    {
        public void Handle(AuthenticatedCommand command)
        {
            LastCommand = command;
        }

        public static AuthenticatedCommand LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;

        public static void Reset()
        {
            LastCommand = null;
        }
    }
}
