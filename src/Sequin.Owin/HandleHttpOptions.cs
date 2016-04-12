namespace Sequin.Owin
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class HandleHttpOptions : OwinMiddleware
    {
        public HandleHttpOptions(OwinMiddleware next) : base(next) {}

        public override async Task Invoke(IOwinContext context)
        {
            if (ShouldHandleOptions(context))
            {
                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Headers.Add("Allow", new[] { "PUT", "OPTIONS" });
            }
            else
            {
                await Next.Invoke(context);
            }
        }

        private static bool ShouldHandleOptions(IOwinContext context)
        {
            var commandEndpointPath = (PathString)context.Environment["CommandEndpointPath"];

            return context.Request.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase) &&
                   context.Request.Path.StartsWithSegments(commandEndpointPath);
        }
    }
}