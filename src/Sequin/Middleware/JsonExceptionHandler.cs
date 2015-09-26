namespace Sequin.Middleware
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    internal class JsonExceptionHandler : OwinMiddleware
    {
        private readonly bool hideExceptionDetail;

        public JsonExceptionHandler(OwinMiddleware next, bool hideExceptionDetail) : base(next)
        {
            this.hideExceptionDetail = hideExceptionDetail;
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.Exception(ex, hideExceptionDetail);
            }
        }
    }
}
