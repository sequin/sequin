namespace Sequin.Integration
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Owin.Testing;

    public abstract class SequinSpecification
    {
        protected SequinSpecification()
        {
            Server = TestServer.Create(app =>
            {
                app.UseSequin();
            });
        }

        protected TestServer Server { get; }

        protected async Task<HttpResponseMessage> IssueCommand(string commandName, object command)
        {
            return await Server.CreateRequest("/commands")
                               .AddHeader("command", commandName)
                               .SendAsync("PUT");
        }
    }
}
