namespace Sequin.Integration
{
    using System.Net;
    using Core;
    using Xunit;
    using Xunit.Should;

    public class WhenCommandHandlerExists : SequinSpecification
    {
        [Fact]
        public async void ReturnsOkStatusCode()
        {
            var response = await IssueCommand("TestCommand");
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private class TestCommandHandler : IHandler<TestCommand>
        {
            public void Handle(TestCommand command)
            {
                // Do nothing
            }
        }

        private class TestCommand
        {
        }
    }
}