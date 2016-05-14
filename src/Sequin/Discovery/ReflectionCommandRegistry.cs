namespace Sequin.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    internal class ReflectionCommandRegistry : ICommandRegistry
    {
        private readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>(StringComparer.InvariantCultureIgnoreCase);

        public ReflectionCommandRegistry(params Assembly[] assemblies)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("At least one assembly must be provided.");
            }

            var commandTypes = assemblies.GetCommands();
            foreach (var type in commandTypes)
            {
                typeMap.Add(type.Name, type);
            }
        }

        public Type GetCommandType(string name)
        {
            return typeMap.ContainsKey(name) ? typeMap[name] : null;
        }
    }
}
