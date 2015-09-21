namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Xunit;

    public class WhenCommandDoesNotExist : SequinSpecification
    {
        [Fact]
        public void ReturnsBadRequest()
        {
            var response = IssueCommand("FakeCommand");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public void ReturnsReasonPhrase()
        {
            var response = IssueCommand("FakeCommand");
            Assert.Equal("Command 'FakeCommand' does not exist.", response.ReasonPhrase);
        }
    }
}
