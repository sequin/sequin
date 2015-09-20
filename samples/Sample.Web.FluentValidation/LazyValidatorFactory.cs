namespace Sample.Web.FluentValidation
{
    using System;
    using global::FluentValidation;

    public class LazyValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            // Lazy factory for sample
            return new DummyCommandValidator();
        }
    }
}