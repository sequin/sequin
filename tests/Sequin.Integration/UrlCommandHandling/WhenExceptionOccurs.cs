namespace Sequin.Integration.UrlCommandHandling
{
    using System.Net;
    using System.Threading.Tasks;
    using Fakes;
    using Infrastructure;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xunit;

    public class WhenExceptionOccurs : SequinUrlSpecification
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
                HideExceptionDetail = true,
                CommandNameResolver = new UrlCommandNameResolver()
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
