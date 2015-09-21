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

        protected RequestBuilder CreateRequest()
        {
            return Server.CreateRequest("/commands");
        }

        protected HttpResponseMessage IssueCommand(string commandName, object command = null)
        {
            var requestBuilder = CreateRequest().AddHeader("command", commandName)
                                                .AddHeader("Content-Type", "application/json");

            if (command != null)
            {
                requestBuilder = requestBuilder.And(request => request.Content = new StringContent(JsonConvert.SerializeObject(command)));
            }

            var task = requestBuilder.SendAsync("PUT");
            task.Wait();

            return task.Result;
        }
    }
}
