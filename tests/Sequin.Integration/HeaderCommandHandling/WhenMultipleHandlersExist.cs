namespace Sequin.Integration.HeaderCommandHandling
{
    using System.Net;
    using System.Threading.Tasks;
    using Core;
    using Fakes;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xunit;

    public class WhenMultipleHandlersExist : SequinHeaderSpecification
    {
        [Fact]
        public void ReturnsInternalServerError()
        {
            var response = IssueCommand(new MultiHandlerCommand());
            response.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task ReturnsExceptionInformation()
        {
            var response = IssueCommand(new MultiHandlerCommand());
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic json = JToken.Parse(responseBody);
            string message = json.message;

            message.ShouldEqual("Multiple handlers exist for the given command type.");
        }

        private class MultiHandlerCommandHandler1 : IHandler<MultiHandlerCommand>
        {
            public void Handle(MultiHandlerCommand command)
            {
                throw new System.NotImplementedException();
            }
        }

        private class MultiHandlerCommandHandler2 : IHandler<MultiHandlerCommand>
        {
            public void Handle(MultiHandlerCommand command)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
