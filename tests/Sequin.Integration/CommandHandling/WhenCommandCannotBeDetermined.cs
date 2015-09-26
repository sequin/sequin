namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Should;
    using Xunit;

    public class WhenCommandCannotBeDetermined : SequinSpecification
    {
        [Fact]
        public async void ReturnsBadRequest()
        {
            var response = await CreateRequest().SendAsync("PUT");
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void ReturnsReasonPhrase()
        {
            var response = await CreateRequest().SendAsync("PUT");
            response.ReasonPhrase.ShouldEqual("Could not identify command from request");
        }
    }
}