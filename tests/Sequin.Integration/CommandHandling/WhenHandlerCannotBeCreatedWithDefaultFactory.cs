namespace Sequin.Integration.CommandHandling
{
    using System;
    using Fakes;
    using Should;
    using Xunit;

    public class WhenHandlerCannotBeCreatedWithDefaultFactory : SequinSpecification
    {
        [Fact]
        public void ThrowsException()
        {
            Action act = () => IssueCommand(new UnconstructableCommandHandlerTest());
            act.ShouldThrow<MissingMethodException>();
        }
    }
}
