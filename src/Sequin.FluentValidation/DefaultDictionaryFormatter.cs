namespace Sequin.FluentValidation
{
    using System.Linq;
    using global::FluentValidation.Results;

    internal class DefaultDictionaryFormatter : IValidationResultFormatter
    {
        public object Format(ValidationResult validationResult)
        {
            var errors = validationResult.Errors
                                         .GroupBy(k => k.PropertyName)
                                         .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
