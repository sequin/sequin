namespace Sequin.FluentValidation.Integration.Features
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Xbehave;

    public class CommandValidationFeature : FeatureBase
    {
        [Scenario]
        public void ValidCommand(string commandName, string command)
        {
            "Given I have a valid command"
                .Given(() =>
                       {
                           commandName = "ValidatedCommand";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          Server.PutCommand("/commands", commandName, command);
                      });

            "Then the command should be handled"
                .Then(() =>
                      {
                          HasExecuted(commandName).Should().BeTrue();
                      });
        }

        [Scenario]
        public void InvalidCommand(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an invalid command"
                .Given(() =>
                       {
                           commandName = "ValidatedCommand";
                           command = "{A:0,B:0}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the command should not be handled"
                .Then(() =>
                      {
                          HasExecuted(commandName).Should().BeFalse();
                      });

            "And I should get a Bad Request response"
                .And(() =>
                     {
                         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                     });

            "And the response body should contain the validation errors"
                .And(() =>
                     {
                         var body = response.BodyAs<Dictionary<string, string[]>>();
                         body.Should().NotBeNull();
                         body.Should().ContainKeys("a", "b");
                     });
        }

        [Scenario]
        public void NoRegisteredValidator(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command with no registered validator"
                .Given(() =>
                       {
                           commandName = "NonValidatedCommand";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .Given(() =>
                       {
                           response = Server.PutCommand("/commands", commandName, command);
                       });

            "Then the command should not be handled"
                .Then(() =>
                      {
                          HasExecuted(commandName).Should().BeFalse();
                      });

            "And I should get a Bad Request response"
                .And(() =>
                     {
                         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                         response.ReasonPhrase.Should().Be("No command validator registered for the specified command type.");
                     });
        }

        [Scenario]
        public void ExceptionInValidator(string commandName, string command, ForcedValidatorException exception)
        {
            "Given I have a command that causes an exception in the validator"
                .Given(() =>
                       {
                           commandName = "ValidatedCommand";
                           command = "{A:1,B:2,ForceException:true}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          try
                          {
                              Server.PutCommand("/commands", commandName, command);
                          }
                          catch (ForcedValidatorException ex)
                          {
                              exception = ex;
                          }
                      });

            "Then the exception is not swallowed"
                .Then(() =>
                      {
                          exception.Should().NotBeNull();
                      });
        }
    }
}