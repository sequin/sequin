namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using System.Threading.Tasks;
    using Infrastructure;
    using Should;
    using Xunit;

    public class WhenCommandCannotBeDeterminedFromUrl : SequinSpecification
    {
        public WhenCommandCannotBeDeterminedFromUrl()
        {
            CreateServer(new SequinOptions
            {
                CommandNameResolver = new UrlCommandNameResolver()
            });
        }

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