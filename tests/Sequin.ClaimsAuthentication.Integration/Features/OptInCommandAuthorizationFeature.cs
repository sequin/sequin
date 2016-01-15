namespace Sequin.ClaimsAuthentication.Integration.Features
{
    using System.Net;
    using System.Net.Http;
    using Core;
    using Extensions;
    using Fakes;
    using FluentAssertions;
    using Middleware;
    using Xbehave;

    public class OptInCommandAuthorizationFeature : FeatureBase
    {
        [Background]
        public void FeatureBackground()
        {
            AuthorizationMiddleware = typeof (OptInCommandAuthorization);
        }

        [Scenario]
        public void UnspecifiedCommandAuthorizationAndNotSignedIn(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command with unspecified authorization rules"
                .Given(() =>
                       {
                           commandName = "UnspecifiedAuthorization";
                           command = "{}";
                       });

            "And I am not signed in"
                .And(IsAnonymousUser);

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the command should be handled"
                .Then(() =>
                      {
                          UnspecifiedAuthorizationHandler.HasExecuted.Should().BeTrue();
                      });

            "And I should get an OK response"
                .And(() =>
                     {
                         response.StatusCode.Should().Be(HttpStatusCode.OK);
                     });
        }

        [Scenario]
        public void UnspecifiedCommandAuthorizationAndSignedIn(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have a command with unspecified authorization rules"
                .Given(() =>
                {
                    commandName = "UnspecifiedAuthorization";
                    command = "{}";
                });

            "And I am signed in"
                .And(() =>
                     {
                         IsAuthenticatedUser();
                     });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should be handled"
                .Then(() =>
                {
                    UnspecifiedAuthorizationHandler.HasExecuted.Should().BeTrue();
                });

            "And I should get an OK response"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
        }

        [Scenario]
        public void AnonymousCommandAndNotSignedIn(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an anonymous command"
                .Given(() =>
                       {
                           commandName = "AnonymousCommand";
                           command = "{}";
                       });

            "And I am not signed in"
                .And(IsAnonymousUser);

            "When I issue an HTTP request"
                .When(() =>
                      {
                          response = Server.PutCommand("/commands", commandName, command);
                      });

            "Then the command should be handled"
                .Then(() =>
                      {
                          AnonymousCommandHandler.HasExecuted.Should().BeTrue();
                      });

            "And I should get an OK response"
                .And(() =>
                     {
                         response.StatusCode.Should().Be(HttpStatusCode.OK);
                     });
        }

        [Scenario]
        public void AnonymousCommandAndSignedIn(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an anonymous command"
                .Given(() =>
                {
                    commandName = "AnonymousCommand";
                    command = "{}";
                });

            "And I am signed in"
                .And(() =>
                     {
                         IsAuthenticatedUser();
                     });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should be handled"
                .Then(() =>
                {
                    AnonymousCommandHandler.HasExecuted.Should().BeTrue();
                });

            "And I should get an OK response"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
        }

        [Scenario]
        public void AuthenticatedCommandAndSignedInWithRole(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an authenticated command"
               .Given(() =>
               {
                   commandName = "AuthenticatedCommand";
                   command = "{}";
               });

            "And I am signed in"
                .And(() =>
                {
                    IsAuthenticatedUser("RoleA");
                });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should be handled"
                .Then(() =>
                {
                    AuthenticatedCommandHandler.HasExecuted.Should().BeTrue();
                });

            "And I should get an OK response"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
        }

        [Scenario]
        public void AuthenticatedCommandAndNotSignedIn(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an authenticated command"
               .Given(() =>
               {
                   commandName = "AuthenticatedCommand";
                   command = "{}";
               });

            "And I am not signed in"
                .And(IsAnonymousUser);

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should not be handled"
                .Then(() =>
                {
                    AuthenticatedCommandHandler.HasExecuted.Should().BeFalse();
                });

            "And I should get an Unauthorized response"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                });
        }

        [Scenario]
        public void AuthenticatedCommandAndSignedInWithoutRole(string commandName, string command, HttpResponseMessage response)
        {
            "Given I have an authenticated command"
               .Given(() =>
               {
                   commandName = "AuthenticatedCommand";
                   command = "{}";
               });

            "And I am signed in"
                .And(() =>
                     {
                         IsAuthenticatedUser();
                     });

            "When I issue an HTTP request"
                .When(() =>
                {
                    response = Server.PutCommand("/commands", commandName, command);
                });

            "Then the command should not be handled"
                .Then(() =>
                {
                    AuthenticatedCommandHandler.HasExecuted.Should().BeFalse();
                });

            "And I should get an Unauthorized response"
                .And(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
                });
        }

        [Scenario]
        public void AmbiguousAuthorizationConfiguration(string commandName, string command, AmbiguousCommandAuthorizationException exception)
        {
            "Given I have a command which is decorated with anonymous and authorize attributes"
                .Given(() =>
                       {
                           commandName = "AmbiguousCommand";
                           command = "{}";
                       });

            "When I issue an HTTP rquest"
                .When(() =>
                      {
                          try
                          {
                              Server.PutCommand("/commands", commandName, command);
                          }
                          catch (AmbiguousCommandAuthorizationException ex)
                          {
                              exception = ex;
                          }
                      });

            "Then an exception should be thrown"
                .Then(() =>
                      {
                          exception.Should().NotBeNull();
                      });
        }
    }
}