namespace Sequin.Integration.HeaderCommandHandling
{
    using System.Net;
    using Should;
    using Xunit;

    public class WhenCommandBodyCannotBeFound : SequinHeaderSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommand("TrackedCommand");
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommand("TrackedCommand");
            response.ReasonPhrase.ShouldEqual("Command body was not provided");
        }
    }
}
