namespace Sequin.ClaimsAuthentication
{
    using System.Security.Claims;
    using Core;

    internal class CommandAuthorizationContext : ICommandAuthorizationContext
    {
        private readonly ClaimsIdentity identity;

        public CommandAuthorizationContext(ClaimsIdentity identity)
        {
            this.identity = identity;
        }

        public void Reject()
        {
            IsAuthorized = false;
        }

        public bool HasClaim(string type, string value)
        {
            return identity.HasClaim(type, value);
        }

        public bool IsAuthorized { get; private set; } = true;
    }
}