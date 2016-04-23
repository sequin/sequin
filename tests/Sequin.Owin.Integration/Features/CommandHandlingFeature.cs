namespace Sequin.Owin.Integration.Features
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using CommandBus;
    using Core;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Xbehave;

    public class CommandHandlingFeature : FeatureBase
    {
        [Scenario]
        public void ExistingCommand(string commandName, string command, HttpResponseMessage response)
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
                          response = Server.PutCommand("/commands", commandName, command);
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
        public void NonExistingCommand(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command that does not exist"
                .Given(() =>
                       {
                           commandName = "FakeCommand";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the response should return as a bad request"
                .Then(() =>
                      {
                          response.StatusCode.Should().Be(HttpStatusCode.NotFound);
                          response.ReasonPhrase.Should().Be($"Command '{commandName}' does not exist.");
                      });
        }

        [Scenario]
        public void CommandCannotBeDetermined(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have not specified a command name"
                .Given(() =>
                       {
                           commandName = string.Empty;
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the response should return as a bad request"
                .Then(() =>
                      {
                          response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                          response.ReasonPhrase.Should().Be("Could not identify command from request");
                      });
        }

        [Scenario]
        public void CommandBodyNotFound(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have not provided a command body"
                .Given(() =>
                       {
                           commandName = "TrackedCommand";
                           command = null;
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the response should return as a bad request"
                .Then(() =>
                      {
                          response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                          response.ReasonPhrase.Should().Be("Command body was not provided");
                      });
        }

        [Scenario]
        public void CommandBodyIsNotJson(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have provided a non-JSON command body"
                .Given(() =>
                       {
                           commandName = "TrackedCommand";
                           command = "#%^$&$*(@#";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the response should return as a bad request"
                .Then(() =>
                      {
                          response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                          response.ReasonPhrase.Should().Be($"Command of type {commandName} could not be constructed from request body. JSON command body could not be read; it may be malformed.");
                      });
        }

        [Scenario]
        public void InvalidCommandPropertyDataType(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have provided a non-JSON command body"
                .Given(() =>
                {
                    commandName = "TestGuidCommand";
                    command = "{A:\"Derp\"}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the response should return as a bad request"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                    response.ReasonPhrase.Should().StartWith($"Command of type {commandName} could not be constructed from request body. Error converting value \"Derp\" to type 'System.Guid'");
                });
        }

        [Scenario]
        public void ExceptionInHandler(string commandName, string command, Exception exception)
        {
            "Given I have a command that causes an exception"
                .Given(() =>
                {
                    commandName = "ExceptionCommand";
                    command = "{A:1,B:2}";
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    try
                    {
                        Server.PutCommand("/commands", commandName, command);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                });

            "Then the exception is not captured"
                .Then(() =>
                {
                    exception.Should().NotBeNull();
                });
        }

        [Scenario]
        public void MultipleHandlersForCommand(string commandName, string command, NonExclusiveCommandHandlerException exception)
        {
            "Given I have a command with multiple detected handlers"
                .Given(() =>
                       {
                           commandName = "MultiHandlerCommand";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          try
                          {
                              Server.PutCommand("/commands", commandName, command);
                          }
                          catch (NonExclusiveCommandHandlerException ex)
                          {
                              exception = ex;
                          }
                      });

            "Then an exception is thrown"
                .Then(() =>
                      {
                          exception.Should().NotBeNull();
                          exception.ResolvedServiceTypes.Count.Should().Be(2);
                      });
        }

        [Scenario]
        public void UnableToConstructHandler(string commandName, string command, MissingMethodException exception)
        {
            "Given I have a command whose handler cannot be constructued"
                .Given(() =>
                       {
                           commandName = "UnconstructableCommandHandlerTest";
                           command = "{A:1,B:2}";
                       });

            "When I issue an HTTP request"
                .When(() =>
                      {
                          try
                          {
                              Server.PutCommand("/commands", commandName, command);
                          }
                          catch (MissingMethodException ex)
                          {
                              exception = ex;
                          }
                      });

            "Then an exception is thrown"
                .Then(() =>
                      {
                          exception.Should().NotBeNull();
                      });
        }

        private class TestGuidCommand
        {
            public Guid A { get; set; }
        }

        private class TestGuidCommandHandler : IHandler<TestGuidCommand>
        {
            public Task Handle(TestGuidCommand command)
            {
                return Task.FromResult(0);
            }
        }
    }
}