namespace Sequin.Integration.Features
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using FluentAssertions;
    using Infrastructure;
    using Microsoft.Owin;
    using Owin;
    using Sequin.Extensions;
    using Xbehave;

    public class CommandPostProcessingFeature : FeatureBase
    {
        private CommandTrackingPostProcessor postProcessor;

        [Background]
        public void FeatureBackground()
        {
            postProcessor = new CommandTrackingPostProcessor();

            Options = new OwinSequinOptions
                      {
                          PostProcessor = postProcessor,
                          CommandPipeline = new[]
                                            {
                                                new CommandPipelineStage(typeof(ConditionalCapture))
                                            }
                      };
        }

        [Scenario]
        public void CommandSuccessfullyIssued(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a valid command"
                .Given(() =>
                       {
                           commandName = "TrackedCommand";
                           command = "{A:2,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the post processor is executed"
                .Then(() =>
                      {
                          postProcessor.ExecutedCommands.Should().ContainKey(commandName);
                      });

            "And I get post-processed response details returned"
                .And(() =>
                     {
                         var body = response.BodyAs<Dictionary<string, string>>();
                         body.Should().ContainKey(commandName);
                     });
        }

        [Scenario]
        public void CommandNotIssued(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command that will be captured by the pipeline"
                .Given(() =>
                       {
                           commandName = "CapturedCommand";
                           command = "{A:2,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the post processor is not executed"
                .Then(() =>
                      {
                          postProcessor.ExecutedCommands.Should().NotContainKey(commandName);
                      });
        }

        private class ConditionalCapture : OwinMiddleware
        {
            public ConditionalCapture(OwinMiddleware next) : base(next) { }

            public override async Task Invoke(IOwinContext context)
            {
                var commandName = context.GetCommand().GetType().Name;

                if (commandName == "CapturedCommand")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    await Next.Invoke(context);
                }
            }
        }

        private class CapturedCommand
        {
            public int A { get; set; }
            public int B { get; set; }
        }

        private class CapturedCommandHandler : IHandler<CapturedCommand>
        {
            public Task Handle(CapturedCommand command)
            {
                return Task.FromResult(0);
            }
        }
    }
}