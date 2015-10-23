namespace Sequin.ClaimsAuthentication
{
    using System;
    using System.Security.Claims;

    public class CommandAuthorizationContext
    {
        public CommandAuthorizationContext(ClaimsIdentity identity, Type commandType)
        {
            Identity = identity;
            CommandType = commandType;
        }

        public void Reject()
        {
            IsAuthorized = false;
        }

        public bool IsAuthorized { get; private set; } = true;
        public ClaimsIdentity Identity { get; }
        public Type CommandType { get; }
    }
}