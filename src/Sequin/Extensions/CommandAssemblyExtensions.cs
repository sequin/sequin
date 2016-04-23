namespace Sequin.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class CommandAssemblyExtensions
    {
        public static IEnumerable<HandlerType> GetHandlers(this IEnumerable<Assembly> assemblies)
        {
            var handlerType = typeof(IHandler<>);

            return from x in assemblies.SelectMany(x => x.GetTypes())
                   let interfaces = x.GetInterfaces()
                   let handlerInterface = interfaces.SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType)
                   where !x.IsInterface && !x.IsAbstract && handlerInterface != null
                   select new HandlerType(x, handlerInterface);
        }

        public static IEnumerable<Type> GetCommands(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.GetHandlers().Select(x => x.CommandType).Distinct();
        }
    }
}