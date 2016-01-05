namespace Sequin.Integration.HeaderCommandHandling
{
    using System.Net;
    using System.Threading.Tasks;
    using Should;
    using Xunit;

    public class WhenCommandCannotBeDetermined : SequinHeaderSpecification
    {
        [Fact]
        public async Task ReturnsBadRequest()
        {
            var response = await CreateRequest().SendAsync("PUT");
            response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReturnsReasonPhrase()
        {
            var response = await CreateRequest().SendAsync("PUT");
            response.ReasonPhrase.ShouldEqual("Could not identify command from request");
        }
    }
}