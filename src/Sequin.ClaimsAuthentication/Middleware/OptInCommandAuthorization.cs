namespace Sequin.ClaimsAuthentication.Middleware
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using Core;
    using Microsoft.Owin;

    public class OptInCommandAuthorization : CommandAuthorization
    {
        public OptInCommandAuthorization(OwinMiddleware next) : base(next)
        {
        }

        protected override bool IsAuthorized(ClaimsIdentity identity, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes, bool isExplicitAnonymousCommand)
        {
            var authorizationContext = new CommandAuthorizationContext(identity);
            var authorizationAttrbuteList = authorizationAttributes.ToList();

            if (authorizationAttrbuteList.Any())
            {
                if (!identity.IsAuthenticated)
                {
                    authorizationContext.Reject();
                }
                else
                {
                    ProcessAuthorizationAttributes(authorizationContext, authorizationAttrbuteList);
                }
            }

            return authorizationContext.IsAuthorized;
        }
    }
}