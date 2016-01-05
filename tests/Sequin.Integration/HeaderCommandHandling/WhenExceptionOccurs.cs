namespace Sequin.Integration.HeaderCommandHandling
{
    using System.Net;
    using System.Threading.Tasks;
    using Fakes;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xunit;

    public class WhenExceptionOccurs : SequinHeaderSpecification
    {
        [Fact]
        public void ReturnsInternalServerError()
        {
            var response = IssueCommand(new ExceptionCommand());
            response.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ReturnsExceptionDetail()
        {
            var response = IssueCommand(new ExceptionCommand());
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic json = JToken.Parse(responseBody);
            string exceptionType = json.type;
            string message = json.message;
            string stackTrace = json.stackTrace;

            exceptionType.ShouldEqual("System.Exception");
            message.ShouldEqual("Exception command");
            stackTrace.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task DoesNotReturnErrorDetailIfConfiguredToHide()
        {
            CreateServer(new SequinOptions
            {
                HideExceptionDetail = true
            });

            var response = IssueCommand(new ExceptionCommand());
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic json = JToken.Parse(responseBody);
            string exceptionType = json.type;
            string message = json.message;
            string stackTrace = json.stackTrace;

            message.ShouldEqual("Exception command");
            exceptionType.ShouldBeNull();
            stackTrace.ShouldBeNull();
        }
    }
}
