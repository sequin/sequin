namespace Sample.Web.ClaimsAuthentication.Commands
{
    using System.Threading.Tasks;
    using Sequin.Core;

    public class MyAnonymousCommandHandler : IHandler<MyAnonymousCommand>
    {
        public Task Handle(MyAnonymousCommand command)
        {
            return Task.FromResult(0);
        }
    }
}