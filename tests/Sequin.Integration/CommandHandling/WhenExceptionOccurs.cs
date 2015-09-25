namespace Sequin.Integration.CommandHandling
{
    using System;
    using System.Net;
    using Core;
    using Should;
    using Xunit;

    public class WhenExceptionOccurs : SequinSpecification
    {
        [Fact(Skip = "Not Implemented")]
        public void ReturnsInternalServerError()
        {
            var response = IssueCommand(new ExceptionCommand());
            response.StatusCode.ShouldEqual(HttpStatusCode.InternalServerError);
        }

        [Fact(Skip = "Not Implemented")]
        public void ReturnsExceptionDetail()
        {
            
        }

        [Fact(Skip = "Not Implemented")]
        public void DoesNotReturnErrorDetailIfConfiguredToHide()
        {
            
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
