namespace Sequin.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core;
    using Core.Infrastructure;

    internal class ReflectionCommandRegistry : ICommandRegistry
    {
        private readonly Dictionary<string, Type> _typeMap = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public ReflectionCommandRegistry(params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("At least one assembly must be provided.");
            }

            var handlerType = typeof(IHandler<>);
            var commandTypes = from x in assemblies.SelectMany(x => x.GetTypes())
                               let interfaces = x.GetInterfaces()
                               let handlerInterface = interfaces.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType)
                               where !x.IsInterface && !x.IsAbstract && handlerInterface != null
                               select handlerInterface.GetGenericArguments()[0];

            foreach (var type in commandTypes)
            {
                _typeMap.Add(type.Name, type);
            }
        }

        public Type GetCommandType(string name)
        {
            return _typeMap.ContainsKey(name) ? _typeMap[name] : null;
        }
    }
}
