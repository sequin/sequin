namespace Sequin.ClaimsAuthentication.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using Microsoft.Owin;

    public abstract class CommandAuthorization : OwinMiddleware
    {
        protected CommandAuthorization(OwinMiddleware next) : base(next) {}

        public override async Task Invoke(IOwinContext context)
        {
            var identity = (ClaimsIdentity)context.Authentication.User?.Identity;
            var authorizationContext = new CommandAuthorizationContext(identity);
            var commandType = context.GetCommand().GetType();

            var isExplicitAnonymousCommand = IsExplicitAnonymousCommand(commandType);
            var authorizationAttributes = GetAuthorizationAttributes(commandType);

            if (isExplicitAnonymousCommand && authorizationAttributes.Any())
            {
                throw new AmbiguousCommandAuthorizationException(commandType);
            }

            if (IsAuthorized(authorizationContext, authorizationAttributes, isExplicitAnonymousCommand))
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }

        protected abstract bool IsAuthorized(ICommandAuthorizationContext authorizationContext, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes, bool isExplicitAnonymousCommand);

        protected static void ProcessAuthorizationAttributes(ICommandAuthorizationContext authorizationContext, IEnumerable<AuthorizeCommandAttribute> authorizationAttributes)
        {
            foreach (var attribute in authorizationAttributes)
            {
                attribute.Authorize(authorizationContext);
            }
        }

        private static bool IsExplicitAnonymousCommand(Type commandType)
        {
            return commandType.GetCustomAttributes(typeof (AnonymousCommandAttribute), true).Length > 0;
        }

        private static AuthorizeCommandAttribute[] GetAuthorizationAttributes(Type commandType)
        {
            return commandType.GetCustomAttributes(typeof(AuthorizeCommandAttribute), true).Cast<AuthorizeCommandAttribute>().ToArray();
        }
    }
}
