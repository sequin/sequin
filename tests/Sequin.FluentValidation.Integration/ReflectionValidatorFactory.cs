namespace Sequin.FluentValidation.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using global::FluentValidation;

    internal class ReflectionValidatorFactory : ValidatorFactoryBase
    {
        private readonly Dictionary<Type, Type> validatorMap;

        public ReflectionValidatorFactory(params Assembly[] assemblies)
        {
            var validatorType = typeof (IValidator<>);
            var validators = from x in assemblies.SelectMany(x => x.GetTypes())
                             let interfaces = x.GetInterfaces()
                             let validatorInterface = interfaces.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType)
                             where !x.IsInterface && !x.IsAbstract && validatorInterface != null
                             select new
                                    {
                                        ConcreteType = x,
                                        InterfaceType = validatorInterface
                                    };

            validatorMap = validators.ToDictionary(x => x.InterfaceType, x => x.ConcreteType);
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (validatorMap.ContainsKey(validatorType))
            {
                var concreteType = validatorMap[validatorType];
                return (IValidator) Activator.CreateInstance(concreteType);
            }

            return null;
        }
    }
}
