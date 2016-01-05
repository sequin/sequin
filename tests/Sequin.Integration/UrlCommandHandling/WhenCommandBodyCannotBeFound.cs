namespace Sequin.Integration.UrlCommandHandling
{
    using System;
    using System.Net;
    using Should;
    using Xunit;

    public class WhenCommandBodyCannotBeFound : SequinUrlSpecification
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
