namespace Sequin.Integration
{
    using System;
    using System.Net.Http;
    using Infrastructure;
    using Microsoft.Owin.Testing;
    using Newtonsoft.Json;
    public abstract class SequinUrlSpecification : SequinSpecification
    {
        protected SequinUrlSpecification()
        {
            CreateServer(new SequinOptions
            {
                CommandNameResolver = new UrlCommandNameResolver()
            });
        }

        protected RequestBuilder CreateRequest(string command)
        {
            return Server.CreateRequest($"/commands/{command}");
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
            var requestBuilder = CreateRequest(commandName)
                                                .AddHeader("Content-Type", "application/json");

            return SendCommand(commandBody, requestBuilder);
        }


    }

    public abstract class SequinSpecification
    {
        protected TestServer Server { get; private set; }

        protected void CreateServer()
        {
            Server = TestServer.Create(app =>
            {
                app.UseSequin();
            });
        }

        protected void CreateServer(SequinOptions options)
        {
            Server = TestServer.Create(app =>
            {
                app.UseSequin(options);
            });
        }

        protected static HttpResponseMessage SendCommand(string commandBody, RequestBuilder request)
        {
            if (commandBody != null)
            {
                request = request.And(req => req.Content = new StringContent(commandBody));
            }

            var task = request.SendAsync("PUT");

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

    public abstract class SequinHeaderSpecification : SequinSpecification
    {
        protected SequinHeaderSpecification()
        {
            CreateServer();
        }

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

            return SendCommand(commandBody, requestBuilder);
        }
    }
}
