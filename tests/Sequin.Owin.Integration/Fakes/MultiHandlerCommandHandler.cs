namespace Sequin.Owin.Integration.Fakes
{
    using System.Threading.Tasks;

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
