namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using System.Threading.Tasks;
    using Sequin.Core;

    public class AuthenticatedCommandHandler : IHandler<AuthenticatedCommand>
    {
        public Task Handle(AuthenticatedCommand command)
        {
            return Task.FromResult(0);
        }
    }
}
