namespace Sequin.ClaimsAuthentication.Core
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class AuthorizeCommandAttribute : Attribute
    {
        private readonly string[] requiredRoles;

        public AuthorizeCommandAttribute(params string[] requiredRoles)
        {
            this.requiredRoles = requiredRoles;
        }

        public void Authorize(ICommandAuthorizationContext context)
        {
            if (requiredRoles.Any(role => !context.HasClaim(ClaimTypes.Role, role)))
            {
                context.Reject();
            }
        }
    }
}