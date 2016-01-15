namespace Sequin.Integration.Features
{
    using System.Net;
    using System.Net.Http;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Infrastructure;
    using Xbehave;

    public class UrlCommandNameResolutionFeature : FeatureBase
    {
        [Background]
        public void FeatureBackground()
        {
            Options = new SequinOptions
                      {
                          CommandNameResolver = new UrlCommandNameResolver()
                      };
        }

        [Scenario]
        public void ResolveCommandNameFromUrl(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a valid command"
                .Given(() =>
                       {
                           commandName = "TrackedCommand";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand($"/commands/{commandName}", command);
                      });

            "Then command should be issued to the correct handler"
                .Then(() =>
                      {
                          var handledCommand = TrackedCommandHandler.LastCommand;
                          handledCommand.Should().NotBeNull();
                          handledCommand.A.Should().Be(1);
                          handledCommand.B.Should().Be(2);
                      });

            "And response should return OK"
                .And(() =>
                     {
                         response.StatusCode.Should().Be(HttpStatusCode.OK);
                     });
        }

        [Scenario]
        public void CommandCannotBeDetermined(string command, HttpResponseMessage response)
        {
            "Given I have not specified a command name"
                .Given(() =>
                {
                    command = "{A:1,B:2}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", command);
                });

            "Then the response return as a bad request"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                    response.ReasonPhrase.Should().Be("Could not identify command from request");
                });
        }
    }
}