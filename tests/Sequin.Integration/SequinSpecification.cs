namespace Sequin.Integration
{
    using System.Net.Http;
    using Microsoft.Owin.Testing;
    using Newtonsoft.Json;

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

        protected HttpResponseMessage IssueCommand(string commandName, object command = null)
        {
            var requestBuilder = Server.CreateRequest("/commands").AddHeader("command", commandName);

            if (command != null)
            {
                requestBuilder = requestBuilder.AddHeader("Content-Type", "application/json")
                                               .And(request => request.Content = new StringContent(JsonConvert.SerializeObject(command)));
            }

            var task = requestBuilder.SendAsync("PUT");
            task.Wait();

            return task.Result;
        }
    }
}
