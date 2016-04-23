namespace Sequin.Owin.Integration.Features
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using FluentAssertions;
    using Infrastructure;
    using Microsoft.Owin.Testing;
    using Newtonsoft.Json;
    using Owin;
    using Xbehave;

    public class JsonDeserializerCommandFactoryFeature
    {
        private CommandTrackingPostProcessor postProcessor;

        [Background]
        public void Background()
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
                        app.UseSequin(new OwinSequinOptions
                        {
                            CommandFactory = new JsonDeserializerCommandFactory(serializerSettings),
                            PostProcessor = postProcessor
                        });
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
    }
}
