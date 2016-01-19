namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using System.Threading.Tasks;
    using Sequin.Core;

    public class AmbiguousCommandHandler : IHandler<AmbiguousCommand>
    {
        public Task Handle(AmbiguousCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
