namespace Sequin.Documentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Command
    {
        private readonly Type commandType;

        public Command(Type commandType)
        {
            this.commandType = commandType;
        }

        private static Dictionary<string, object> GetPropertiesForType(Type type)
        {
            return type.GetProperties().Where(x => x.CanWrite).OrderBy(x => x.Name).ToDictionary(x => x.Name, x => (object)x.PropertyType.Name);
        }

        public string Name => commandType.Name;
        public IDictionary<string, object> Properties => GetPropertiesForType(commandType);
    }
}
