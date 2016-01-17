namespace Sequin.Integration.Features
{
    using System.Collections.Generic;
    using System.Net.Http;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Xbehave;

    public class CommandPostProcessingFeature : FeatureBase
    {
        private CommandTrackingPostProcessor postProcessor;

        [Background]
        public void FeatureBackground()
        {
            postProcessor = new CommandTrackingPostProcessor();

            Options = new SequinOptions
                      {
                          PostProcessor = postProcessor
                      };
        }

        [Scenario]
        public void ExecuteProcessor(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command"
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
                          postProcessor.ExecutedCommands.ContainsKey(commandName).Should().BeTrue();
                      });

            "And I get post-processed response details returned"
                .And(() =>
                     {
                         var body = response.BodyAs<Dictionary<string, string>>();
                         body.Should().ContainKey(commandName);
                     });
        }
    }
}