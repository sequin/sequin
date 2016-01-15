namespace Sequin.ClaimsAuthentication.Integration.Fakes
{
    using Sequin.Core;

    public class AmbiguousCommandHandler : IHandler<AmbiguousCommand>
    {
        public void Handle(AmbiguousCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}
