namespace Sequin.Integration
{
    using System;
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

        protected HttpResponseMessage IssueCommand(object command)
        {
            return IssueCommandWithBody(command.GetType().Name, JsonConvert.SerializeObject(command));
        }

        protected HttpResponseMessage IssueCommand(string commandName)
        {
            return IssueCommandWithBody(commandName);
        }

        protected HttpResponseMessage IssueCommandWithBody(string commandName, string commandBody = null)
        {
            var requestBuilder = CreateRequest().AddHeader("command", commandName)
                                                .AddHeader("Content-Type", "application/json");

            if (commandBody != null)
            {
                requestBuilder = requestBuilder.And(request => request.Content = new StringContent(commandBody));
            }

            var task = requestBuilder.SendAsync("PUT");

            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }

            return task.Result;
        }
    }
}
