namespace Sequin.Integration.CommandHandling
{
    using System;
    using System.Net;
    using Core;
    using Xunit;
    using Xunit.Should;

    public class WhenCommandHandlerExists : SequinSpecification
    {
        [Fact]
        public async void ExecutesHandler()
        {
            var command = new TestCommand
            {
                A = 1,
                B = 2
            };

            await IssueCommand("TestCommand", command);
            var handledCommand = TestCommandHandler.LastCommand;

            handledCommand.ShouldBeSameAs(command);
        }

        [Fact]
        public async void ReturnsOkStatusCode()
        {
            var response = await IssueCommand("TestCommand", new TestCommand());
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private class TestCommandHandler : IHandler<TestCommand>
        {
            public TestCommandHandler()
            {
                LastCommand = null;
            }

            public void Handle(TestCommand command)
            {
                LastCommand = command;
            }

            public static TestCommand LastCommand { get; private set; }
        }

        private class TestCommand
        {
            public int A { get; set; }
            public int B { get; set; }
        }
    }
}
