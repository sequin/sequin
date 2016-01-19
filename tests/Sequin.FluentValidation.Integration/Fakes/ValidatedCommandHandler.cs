namespace Sequin.FluentValidation.Integration.Fakes
{
    using System.Threading.Tasks;
    using Core;

    public class ValidatedCommandHandler : IHandler<ValidatedCommand>
    {
        public Task Handle(ValidatedCommand command)
        {
            return Task.FromResult(0);
        }
    }
}