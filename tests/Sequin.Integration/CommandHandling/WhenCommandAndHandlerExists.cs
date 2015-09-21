namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Core;
    using Xunit;

    public class WhenCommandAndHandlerExists : SequinSpecification
    {
        [Fact]
        public void ExecutesHandler()
        {
            var command = new TestCommand
            {
                A = 1,
                B = 2
            };

            IssueCommand("TestCommand", command);
            var handledCommand = TestCommandHandler.LastCommand;

            Assert.Equal(1, handledCommand.A);
            Assert.Equal(2, handledCommand.B);
        }

        [Fact]
        public void ReturnsOkStatusCode()
        {
            var response = IssueCommand("TestCommand", new TestCommand());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
