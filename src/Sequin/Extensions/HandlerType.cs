namespace Sequin.Extensions
{
    using System;

    internal struct HandlerType
    {
        public HandlerType(Type concreteType, Type interfaceType)
        {
            ConcreteType = concreteType;
            InterfaceType = interfaceType;
        }

        public Type ConcreteType { get; }

        public Type InterfaceType { get; }

        public Type CommandType => InterfaceType.GetGenericArguments()[0];
    }
}