namespace Sequin.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    public class ReflectionHandlerFactory : IHandlerFactory
    {
        private readonly Dictionary<Type, List<Type>> commandHandlerMap; 

        public ReflectionHandlerFactory(params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("At least one assembly must be provided.");
            }

            var query = from x in assemblies.GetHandlers()
                        group x by x.CommandType into g
                        select g;

            commandHandlerMap = query.ToDictionary(x => x.Key, x => x.Select(y => y.ConcreteType).ToList());
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
