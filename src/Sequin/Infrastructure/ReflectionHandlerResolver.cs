namespace Sequin.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core;
    using Core.Infrastructure;

    public class ReflectionHandlerResolver : IHandlerResolver
    {
        private readonly Dictionary<Type, List<Type>> commandHandlerMap; 

        public ReflectionHandlerResolver(params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("At least one assembly must be provided.");
            }

            var handlerType = typeof(IHandler<>);
            var query = from x in assemblies.SelectMany(x => x.GetTypes())
                        let interfaces = x.GetInterfaces()
                        let handlerInterface = interfaces.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType)
                        where !x.IsInterface && !x.IsAbstract && handlerInterface != null
                        let commandType = handlerInterface.GetGenericArguments()[0]
                        group x by commandType into g
                        select g;

            commandHandlerMap = query.ToDictionary(x => x.Key, x => x.ToList());
        }

        public ICollection<IHandler<T>> GetForCommand<T>()
        {
            var commandType = typeof (T);
            if (commandHandlerMap.ContainsKey(commandType))
            {
                return commandHandlerMap[commandType].Select(x => (IHandler<T>)Activator.CreateInstance(x)).ToList();
            }

            return new List<IHandler<T>>();
        }
    }
}
