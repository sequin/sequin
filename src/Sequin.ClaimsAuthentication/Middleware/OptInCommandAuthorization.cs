namespace Sequin.ClaimsAuthentication.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using ClaimsAuthentication;
    using Extensions;

    public class OptInCommandAuthorization : OwinMiddleware
    {
        public OptInCommandAuthorization(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var identity = (ClaimsIdentity)context.Authentication.User.Identity;
            var commandType = context.GetCommand().GetType();

            if (IsAuthorized(identity, commandType))
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }

        private static bool IsAuthorized(ClaimsIdentity identity, Type commandType)
        {
            var authorizationAttributes = GetAuthorizationAttributes(commandType).ToList();
            var authorizationContext = new CommandAuthorizationContext(identity, commandType);

            if (authorizationAttributes.Any())
            {
                if (!identity.IsAuthenticated)
                {
                    authorizationContext.Reject();
                }
                else
                {
                    authorizationAttributes.ForEach(x => x.Authorize(authorizationContext));
                }
            }

            return authorizationContext.IsAuthorized;
        }

        private static IEnumerable<AuthorizeCommandAttribute> GetAuthorizationAttributes(Type commandType)
        {
            return commandType.GetCustomAttributes(typeof (AuthorizeCommandAttribute), true).Cast<AuthorizeCommandAttribute>();
        }
    }
}