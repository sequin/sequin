namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Fakes;
    using Should;
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

            IssueCommand(command);
            var handledCommand = TrackedCommandHandler.LastCommand;

            handledCommand.ShouldNotBeNull();
            handledCommand.A.ShouldEqual(1);
            handledCommand.B.ShouldEqual(2);
        }

        [Fact]
        public void ReturnsOkStatusCode()
        {
            var response = IssueCommand(new TrackedCommand());
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }
    }
}
