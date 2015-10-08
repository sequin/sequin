namespace Sequin.Integration.CommandHandling
{
    using System.Net;
    using Fakes;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xunit;

    public class WhenHandlerCannotBeCreatedWithDefaultFactory : SequinSpecification
    {
        [Fact]
        public void ReturnsInternalServerError()
        {
            var response = IssueCommand(new UnconstructableCommandHandlerTest());
            response.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void ReturnsExceptionInformation()
        {
            var response = IssueCommand(new UnconstructableCommandHandlerTest());
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic json = JToken.Parse(responseBody);
            string message = json.message;

            message.ShouldEqual("No parameterless constructor defined for this object.");
        }
    }
}
