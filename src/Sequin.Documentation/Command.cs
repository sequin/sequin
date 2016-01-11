namespace Sequin.Documentation
{
    using System;
    using System.Collections;
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
            var properties = from x in type.GetProperties()
                             where x.CanWrite
                             select new
                             {
                                 Key = x.Name,
                                 Value = GetPropertyTypeSchema(x.PropertyType)
                             };

            return properties.ToDictionary(x => x.Key, x => x.Value);
        }

        private static object GetPropertyTypeSchema(Type propertyType)
        {
            if (IsCollection(propertyType))
            {
                var enumerableType = propertyType.IsArray ? propertyType.GetElementType() : propertyType.GetGenericArguments().Single();
                return new[] { GetPropertyTypeSchema(enumerableType) };
            }

            if (IsDictionary(propertyType))
            {
                var keyType = propertyType.GenericTypeArguments[0];
                var keySchema = IsComplexType(keyType) ? GetPropertiesForType(keyType) : GetPropertyTypeSchema(keyType);

                var valueType = propertyType.GenericTypeArguments[1];
                var valueSchema = IsComplexType(valueType) ? GetPropertiesForType(valueType) : GetPropertyTypeSchema(valueType);

                return new Dictionary<object, object>
                {
                    {keySchema, valueSchema}
                };
            }

            if (IsComplexType(propertyType))
            {
                return GetPropertiesForType(propertyType);
            }

            return propertyType.Name;
        }

        private static bool IsCollection(Type type)
        {
            return type.GetInterface(typeof (IEnumerable<>).FullName) != null && (type.IsArray || type.GetGenericArguments().Length == 1);
        }

        private static bool IsDictionary(Type type)
        {
            return type.GetInterface(typeof(IDictionary<,>).FullName) != null;
        }

        private static bool IsComplexType(Type type)
        {
            return type.GetProperties().Any(x => x.CanWrite);
        }

        public string Name => commandType.Name;
        public IDictionary<string, object> Schema => GetPropertiesForType(commandType);
    }
}
