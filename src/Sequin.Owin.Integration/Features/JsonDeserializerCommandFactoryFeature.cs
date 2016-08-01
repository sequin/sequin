namespace Sequin.Owin.Integration.Features
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Configuration;
    using Discovery;
    using Extensions;
    using FluentAssertions;
    using Microsoft.Owin.Testing;
    using Newtonsoft.Json;
    using Owin.Extensions;
    using Sequin.Discovery;
    using Xbehave;

    public class JsonDeserializerCommandFactoryFeature : FeatureBase
    {
        private CommandTrackingPostProcessor postProcessor;

        [Background]
        public void FeatureBackground()
        {
            postProcessor = new CommandTrackingPostProcessor();
        }

        [Scenario]
        public void CustomisableSerializerSettings(TestServer server, string commandName, string command, HttpResponseMessage response)
        {
            "Given I have custom serializer settings"
                .Given(() =>
                {
                    var serializerSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};

                    server = TestServer.Create(app =>
                    {
                        var options = (HttpOptions) Options.Configure()
                                                                       .WithPostProcessPipeline(postProcessor)
                                                                       .ForHttp()
                                                                       .WithOwinDefaults()
                                                                       .WithCommandFactory(x => new JsonDeserializerCommandFactory(x.CommandRegistry, new OwinEnvironmentBodyProvider(), serializerSettings))
                                                                       .Build();

                        app.UseSequin(options);
                    });
                });

            "And I have a command which requires these settings"
                .And(() =>
                {
                    commandName = "TestCommand";
                    command = "{A:null}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = server.PutCommand("/commands", commandName, command);
                });

            "Then the command should have been executed"
                .Then(() =>
                {
                    postProcessor.ExecutedCommands.ContainsKey(commandName).Should().BeTrue();

                    var serializedCommand = postProcessor.ExecutedCommands[commandName];
                    serializedCommand.Should().Contain(Guid.Empty.ToString());
                });
        }

        [Scenario]
        public void MalformedJsonErrors(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command which sets a JSON object to a property expecting an array"
                .Given(() =>
                {
                    commandName = "StringArrayCommand";
                    command = "{A:{prop:val}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then a BadRequest response should be returned"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                    response.ReasonPhrase.Should().NotBeEmpty();
                });
        }

        private class TestCommand
        {
            public Guid A { get; set; }
        }

        private class TestCommandHandler : IHandler<TestCommand>
        {
            public Task Handle(TestCommand command)
            {
                return Task.FromResult(0);
            }
        }

        private class StringArrayCommand
        {
            public string[] A { get; set; }
        }

        private class StringArrayCommandHandler : IHandler<StringArrayCommand>
        {
            public Task Handle(StringArrayCommand command)
            {
                throw new NotImplementedException();
            }
        }
    }
}
