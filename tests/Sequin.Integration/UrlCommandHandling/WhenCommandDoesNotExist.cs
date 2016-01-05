namespace Sequin.Integration.UrlCommandHandling
{
    using System.Net;
    using Should;
    using Xunit;

    public class WhenCommandDoesNotExist : SequinUrlSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommand("FakeCommand");
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommand("FakeCommand");
            response.ReasonPhrase.ShouldEqual("Command 'FakeCommand' does not exist.");
        }
    }
}
