namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Xunit;

    public class WhenCommandCannotBeDetermined : SequinSpecification
    {
        [Fact]
        public async void ReturnsBadRequest()
        {
            var response = await CreateRequest().SendAsync("PUT");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void ReturnsReasonPhrase()
        {
            var response = await CreateRequest().SendAsync("PUT");
            Assert.Equal("Could not identify command from request", response.ReasonPhrase);
        }
    }
}