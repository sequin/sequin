namespace Sequin.FluentValidation.Integration.Extensions
{
    using System;
    using System.Net.Http;
    using Microsoft.Owin.Testing;

    internal static class TestServerCommands
    {
        public static HttpResponseMessage PutCommand(this TestServer server, string url, string commandName, string command)
        {
            var request = server.CreateRequest(url)
                                .AddHeader("command", commandName)
                                .AddHeader("content-type", "application/json");

            if (command != null)
            {
                request = request.And(x => x.Content = new StringContent(command));
            }

            try
            {
                return request.SendAsync("PUT").Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}