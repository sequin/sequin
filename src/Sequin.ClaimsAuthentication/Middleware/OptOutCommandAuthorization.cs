namespace Sequin.ClaimsAuthentication.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using Core;
    using Microsoft.Owin;

    public class OptOutCommandAuthorization : CommandAuthorization
    {
        public OptOutCommandAuthorization(OwinMiddleware next) : base(next) {}

        protected override bool IsAuthorized(ClaimsIdentity identity, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes, bool isExplicitAnonymousCommand)
        {
            throw new NotImplementedException();
        }
    }
}
