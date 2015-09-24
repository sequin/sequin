namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Xunit;

    public class WhenCommandBodyIsNotJson : SequinSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommandWithBody("TrackedCommand", "#%^$&$*(@#");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommandWithBody("TrackedCommand", "#%^$&$*(@#");
            Assert.Equal("Command could not be constructed from request body", response.ReasonPhrase);
        }
    }
}
