namespace Sequin.Integration.Fakes
{
    using Core;

    public class MultiHandlerCommandHandler1 : IHandler<MultiHandlerCommand>
    {
        public void Handle(MultiHandlerCommand command)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MultiHandlerCommandHandler2 : IHandler<MultiHandlerCommand>
    {
        public void Handle(MultiHandlerCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
