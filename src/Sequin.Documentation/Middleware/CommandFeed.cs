namespace Sequin.Documentation.Middleware
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Core.Infrastructure;
    using Microsoft.Owin;
    using Newtonsoft.Json;

    internal class CommandFeed : OwinMiddleware
    {
        private readonly ICommandRegistry commandRegistry;
        private readonly PathString feedPath;

        public CommandFeed(OwinMiddleware next, PathString baseUrl, ICommandRegistry commandRegistry) : base(next)
        {
            this.commandRegistry = commandRegistry;
            feedPath = baseUrl.Add(new PathString("/commands"));
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path != feedPath)
            {
                await Next.Invoke(context);
                return;
            }

            WriteJson(context);
        }

        private void WriteJson(IOwinContext context)
        {
            var commands = commandRegistry.GetAll().Select(x => new Command(x)).ToArray();
            var json = JsonConvert.SerializeObject(commands);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";
            context.Response.Write(json);
        }
    }
}