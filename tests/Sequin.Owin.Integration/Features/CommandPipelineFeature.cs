namespace Sequin.Owin.Integration.Features
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Integration;
    using Pipeline;
    using Xbehave;

    public class CommandPipelineFeature : FeatureBase
    {
        [Background]
        public void FeatureBackground()
        {
            Options = new OwinSequinOptions
            {
                CommandPipeline = new CommandPipelineStage[]
                                            {
                                                new Passthrough(),
                                                new ConditionalCapture()
                                            }
            };
        }

        [Scenario]
        public void PassthroughPipelineAndIssue(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command that should passthrough the pipeline"
                .Given(() =>
                {
                    commandName = "PassthroughCommand";
                    command = "{A:1,B:2}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the response should return as OK"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
        }

        [Scenario]
        public void StopRequestInPipeline(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command that should be captured in the pipeline"
                .Given(() =>
                {
                    commandName = "TrackedCommand";
                    command = "{A:1,B:2}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should not be issued"
                .Then(() =>
                {
                    var handledCommand = TrackedCommandHandler.LastCommand;
                    handledCommand.Should().BeNull();
                });
        }

        private class Passthrough : CommandPipelineStage
        {
            protected override Task Process<TCommand>(TCommand command)
            {
                return Task.FromResult(0);
            }
        }

        private class ConditionalCapture : CommandPipelineStage
        {
            protected override Task Process<TCommand>(TCommand command)
            {
                var commandName = command.GetType().Name;

                if (commandName == "TrackedCommand")
                {
                    Stop();
                }

                return Task.FromResult(0);
            }
        }

        private class PassthroughCommand
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        private class PassthroughCommandHandler : IHandler<PassthroughCommand>
        {
            public Task Handle(PassthroughCommand command)
            {
                return Task.FromResult(0);
            }
        }
    }
}