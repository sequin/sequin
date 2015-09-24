namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Fakes;
    using Xunit;

    public class WhenCommandAndHandlerExists : SequinSpecification
    {
        [Fact]
        public void ExecutesHandler()
        {
            var command = new TrackedCommand
            {
                A = 1,
                B = 2
            };

            IssueCommand("TrackedCommand", command);
            var handledCommand = TrackedCommandHandler.LastCommand;

            Assert.NotNull(handledCommand);
            Assert.Equal(1, handledCommand.A);
            Assert.Equal(2, handledCommand.B);
        }

        [Fact]
        public void ReturnsOkStatusCode()
        {
            var response = IssueCommand("TrackedCommand", new TrackedCommand());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
