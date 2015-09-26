namespace Sequin.Integration.CommandHandling
{
    using System;
    using System.Net;
    using Core;
    using Newtonsoft.Json.Linq;
    using Should;
    using Xunit;

    public class WhenExceptionOccurs : SequinSpecification
    {
        [Fact]
        public void ReturnsInternalServerError()
        {
            var response = IssueCommand(new ExceptionCommand());
            response.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async void ReturnsExceptionDetail()
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
        public async void DoesNotReturnErrorDetailIfConfiguredToHide()
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

        private class ExceptionCommandHandler : IHandler<ExceptionCommand>
        {
            public void Handle(ExceptionCommand command)
            {
                throw new Exception("Exception command");
            }
        }

        private class ExceptionCommand
        {
            
        }
    }
}
