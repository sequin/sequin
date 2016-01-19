namespace Sequin.Integration.Fakes
{
    using System.Threading.Tasks;
    using Core;

    public class MultiHandlerCommandHandler1 : IHandler<MultiHandlerCommand>
    {
        public Task Handle(MultiHandlerCommand command)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MultiHandlerCommandHandler2 : IHandler<MultiHandlerCommand>
    {
        public Task Handle(MultiHandlerCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
