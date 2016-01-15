namespace Sequin.FluentValidation.Integration.Fakes
{
    using global::FluentValidation;

    public class ValidatedCommandValidator : AbstractValidator<ValidatedCommand>
    {
        public ValidatedCommandValidator()
        {
            RuleFor(x => x.A).GreaterThan(0);
            RuleFor(x => x.B).GreaterThan(0);

            Custom(x =>
                   {
                       if (x.ForceException)
                       {
                           throw new ForcedValidatorException();
                       }

                       return null;
                   });
        }
    }
}