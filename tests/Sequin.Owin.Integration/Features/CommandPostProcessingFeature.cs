namespace Sequin.Owin.Integration.Features
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core;
    using Extensions;
    using FluentAssertions;
    using Owin;
    using Pipeline;
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
                CommandPipeline = new CommandPipelineStage[]
                                            {
                                                new ConditionalCapture()
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

        private class ConditionalCapture : CommandPipelineStage
        {
            protected override Task Process<TCommand>(TCommand command)
            {
                var commandName = command.GetType().Name;

                if (commandName == "CapturedCommand")
                {
                    Stop();
                }

                return Task.FromResult(0);
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