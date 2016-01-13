namespace Sequin.ClaimsAuthentication.Middleware
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Microsoft.Owin;

    public class OptInCommandAuthorization : CommandAuthorization
    {
        public OptInCommandAuthorization(OwinMiddleware next) : base(next) { }

        protected override bool IsAuthorized(ICommandAuthorizationContext authorizationContext, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes, bool isExplicitAnonymousCommand)
        {
            var attributes = authorizationAttributes.ToList();

            if (attributes.Any())
            {
                if (!authorizationContext.IsAuthenticated)
                {
                    authorizationContext.Reject();
                }
                else
                {
                    ProcessAuthorizationAttributes(authorizationContext, attributes);
                }
            }

            return authorizationContext.IsAuthorized;
        }
    }
}