namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using Sequin.Core;

    public class UnspecifiedAuthorizationHandler : IHandler<UnspecifiedAuthorization>
    {
        public void Handle(UnspecifiedAuthorization command)
        {
            LastCommand = command;
        }

        public static UnspecifiedAuthorization LastCommand { get; private set; }

        public static bool HasExecuted => LastCommand != null;

        public static void Reset()
        {
            LastCommand = null;
        }
    }
}
