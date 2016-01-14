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
            return identity != null && identity.HasClaim(type, value);
        }

        public bool IsAuthenticated => identity?.IsAuthenticated ?? false;

        public bool IsAuthorized { get; private set; } = true;
    }
}