namespace Sequin.Integration.HeaderCommandHandling
{
    using System.Net;
    using Should;
    using Xunit;

    public class WhenCommandBodyIsNotJson : SequinHeaderSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommandWithBody("TrackedCommand", "#%^$&$*(@#");
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommandWithBody("TrackedCommand", "#%^$&$*(@#");
            response.ReasonPhrase.ShouldEqual("Command could not be constructed from request body");
        }
    }
}
