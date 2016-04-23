namespace Sequin.Owin.Integration.Features
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Microsoft.Owin;
    using Integration;
    using Sequin.Extensions;
    using Sequin.Infrastructure;
    using Xbehave;

    public class CommandPipelineFeature : FeatureBase
    {
        [Background]
        public void FeatureBackground()
        {
            Options = new OwinSequinOptions
            {
                CommandPipeline = new[]
                                            {
                                                new CommandPipelineStage(typeof(Passthrough)),
                                                new CommandPipelineStage(typeof(ConditionalCapture))
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

            "And the response should be configured by the pipeline"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                });
        }

        private class Passthrough : OwinMiddleware
        {
            public Passthrough(OwinMiddleware next) : base(next) { }

            public override async Task Invoke(IOwinContext context)
            {
                await Next.Invoke(context);
            }
        }

        private class ConditionalCapture : OwinMiddleware
        {
            public ConditionalCapture(OwinMiddleware next) : base(next) { }

            public override async Task Invoke(IOwinContext context)
            {
                var commandName = context.GetCommand().GetType().Name;

                if (commandName == "TrackedCommand")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    await Next.Invoke(context);
                }
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