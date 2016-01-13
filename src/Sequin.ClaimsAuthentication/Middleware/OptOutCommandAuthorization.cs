namespace Sequin.ClaimsAuthentication.Middleware
{
    using System.Collections.Generic;
    using Core;
    using Microsoft.Owin;

    public class OptOutCommandAuthorization : CommandAuthorization
    {
        public OptOutCommandAuthorization(OwinMiddleware next) : base(next) {}

        protected override bool IsAuthorized(ICommandAuthorizationContext authorizationContext, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes, bool isExplicitAnonymousCommand)
        {
            if (!isExplicitAnonymousCommand)
            {
                if (!authorizationContext.IsAuthenticated)
                {
                    authorizationContext.Reject();
                }
                else
                {
                    ProcessAuthorizationAttributes(authorizationContext, authorizationAttributes);
                }
            }

            return authorizationContext.IsAuthorized;
        }
    }
}
