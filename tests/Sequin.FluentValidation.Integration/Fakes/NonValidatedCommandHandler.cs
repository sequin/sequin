namespace Sequin.FluentValidation.Integration.Fakes
{
    using System.Threading.Tasks;
    using Core;

    public class NonValidatedCommandHandler : IHandler<NonValidatedCommand>
    {
        public Task Handle(NonValidatedCommand command)
        {
            return Task.FromResult(0);
        }
    }
}
