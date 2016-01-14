namespace Sequin.Integration.CommandHandling
{
    using System;
    using CommandBus;
    using Core;
    using Fakes;
    using Should;
    using Xunit;

    public class WhenMultipleHandlersExist : SequinSpecification
    {
        [Fact]
        public void ThrowsException()
        {
            Action act = () => IssueCommand(new MultiHandlerCommand());
            act.ShouldThrow<NonExclusiveCommandHandlerException>();
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
