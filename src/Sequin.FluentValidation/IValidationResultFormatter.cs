namespace Sequin.FluentValidation
{
    using global::FluentValidation.Results;

    public interface IValidationResultFormatter
    {
        object Format(ValidationResult validationResult);
    }
}