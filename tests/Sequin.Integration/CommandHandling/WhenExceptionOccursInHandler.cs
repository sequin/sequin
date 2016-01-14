namespace Sequin.Integration.CommandHandling
{
    using System;
    using Fakes;
    using Should;
    using Xunit;

    public class WhenExceptionOccursInHandler : SequinSpecification
    {
        [Fact]
        public void ThrowsException()
        {
            Action act = () => IssueCommand(new ExceptionCommand());
            act.ShouldThrow<Exception>();
        }
    }
}
