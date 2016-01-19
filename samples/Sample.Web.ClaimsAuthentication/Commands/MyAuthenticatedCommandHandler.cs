namespace Sample.Web.ClaimsAuthentication.Commands
{
    using System.Threading.Tasks;
    using Sequin.Core;

    public class MyAuthenticatedCommandHandler : IHandler<MyAuthenticatedCommand>
    {
        public Task Handle(MyAuthenticatedCommand command)
        {
            return Task.FromResult(0);
        }
    }
}