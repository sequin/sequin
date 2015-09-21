namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Core;
    using Xunit;

    public class WhenCommandBodyCannotBeFound : SequinSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommand("NullBodyCommand");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommand("NullBodyCommand");
            Assert.Equal("Command body was not provided", response.ReasonPhrase);
        }

        private class NullBodyCommandHandler : IHandler<NullBodyCommand>
        {
            public void Handle(NullBodyCommand command)
            {
                // Do nothing
            }
        }

        private class NullBodyCommand
        {
            public int A { get; set; }
            public int B { get; set; }
        }
    }
}
