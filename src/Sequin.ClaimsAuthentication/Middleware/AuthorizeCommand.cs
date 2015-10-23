namespace Sequin.ClaimsAuthentication.Middleware
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using ClaimsAuthentication;
    using Extensions;

    public class AuthorizeCommand : OwinMiddleware
    {
        public AuthorizeCommand(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            var identity = (ClaimsIdentity)context.Authentication.User.Identity;
            if (identity.IsAuthenticated)
            {
                var commandType = context.GetCommand().GetType();
                var authorizationContext = new CommandAuthorizationContext(identity, commandType);
                ProcessAuthorizationAttributes(authorizationContext);

                if (authorizationContext.IsAuthorized)
                {
                    await Next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                }
            }
            else
            {
                context.Response.StatusCode = 401;
            }
        }

        private static void ProcessAuthorizationAttributes(CommandAuthorizationContext context)
        {
            var authorizationAttributes = context.CommandType.GetCustomAttributes(typeof(AuthorizeCommandAttribute), true).Cast<AuthorizeCommandAttribute>();
            foreach (var authorizationAttribute in authorizationAttributes)
            {
                authorizationAttribute.Authorize(context);

                if (!context.IsAuthorized)
                {
                    return;
                }
            }
        }
    }
}